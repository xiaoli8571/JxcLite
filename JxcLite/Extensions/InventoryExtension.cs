namespace JxcLite.Extensions;

public static class InventoryExtension
{
    public static async Task AdjustStockAsync(this Database db, JxBill bill, List<JxBillList> lists)
    {
        foreach (var item in lists)
        {
            var qtyChange = bill.Type switch
            {
                BillType.Import => item.Qty ?? 0,
                BillType.Export => -(item.Qty ?? 0),
                BillType.ImportReturn => -(item.Qty ?? 0),
                BillType.ExportReturn => item.Qty ?? 0,
                _ => 0
            };

            if (qtyChange == 0) continue;

            var inventory = await db.QueryAsync<JxInventory>(d => d.GoodsId == item.GoodsId);
            if (inventory == null)
            {
                // 首次建立库存记录时带入商品期初数量
                var goods = await db.QueryByIdAsync<JxGoods>(item.GoodsId);
                inventory = new JxInventory { GoodsId = item.GoodsId, StockQty = goods?.InitialQty ?? 0 };
                await db.SaveAsync(inventory);
            }
            inventory.StockQty = (inventory.StockQty ?? 0) + qtyChange;
            await db.SaveAsync(inventory);

            var ledger = new JxStockLedger
            {
                GoodsId = item.GoodsId,
                BillId = bill.Id,
                BillNo = bill.BillNo,
                BillType = bill.Type,
                ListId = item.Id,
                QtyChange = qtyChange,
                BalanceQty = inventory.StockQty,
                BillDate = bill.BillDate
            };
            await db.SaveAsync(ledger);
        }
    }

    internal static async Task ReverseStockAsync(this Database db, JxBill bill, List<JxBillList> lists)
    {
        foreach (var item in lists)
        {
            var qtyChange = bill.Type switch
            {
                BillType.Import => -(item.Qty ?? 0),
                BillType.Export => item.Qty ?? 0,
                BillType.ImportReturn => item.Qty ?? 0,
                BillType.ExportReturn => -(item.Qty ?? 0),
                _ => 0
            };

            if (qtyChange == 0) continue;

            var inventory = await db.QueryAsync<JxInventory>(d => d.GoodsId == item.GoodsId);
            if (inventory != null)
            {
                inventory.StockQty = (inventory.StockQty ?? 0) + qtyChange;
                await db.SaveAsync(inventory);

                var ledger = new JxStockLedger
                {
                    GoodsId = item.GoodsId,
                    BillId = bill.Id,
                    BillNo = bill.BillNo,
                    BillType = bill.Type,
                    ListId = item.Id,
                    QtyChange = qtyChange,
                    BalanceQty = inventory.StockQty,
                    BillDate = bill.BillDate
                };
                await db.SaveAsync(ledger);
            }
        }
    }
}
