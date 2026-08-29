namespace JxcLite.Extensions;

static class ProcessExtension
{
    /// <summary>
    /// 加工单保存时调整库存：加工领用(Process)扣减，加工退回(ProcessReturn)加回。
    /// </summary>
    internal static async Task AdjustProcessStockAsync(this Database db, JxProcess model)
    {
        var qtyChange = GetQtyChange(model);
        if (qtyChange == 0)
            return;

        var inventory = await db.QueryAsync<JxInventory>(d => d.GoodsId == model.GoodsId);
        if (inventory == null)
        {
            var goods = await db.QueryByIdAsync<JxGoods>(model.GoodsId);
            inventory = new JxInventory { GoodsId = model.GoodsId, StockQty = goods?.InitialQty ?? 0 };
            await db.SaveAsync(inventory);
        }
        inventory.StockQty = (inventory.StockQty ?? 0) + qtyChange;
        await db.SaveAsync(inventory);

        var ledger = new JxStockLedger
        {
            GoodsId = model.GoodsId,
            BillId = model.Id,
            BillNo = model.BillNo,
            BillType = model.Type,
            QtyChange = qtyChange,
            BalanceQty = inventory.StockQty,
            BillDate = model.BillDate ?? DateTime.Now
        };
        await db.SaveAsync(ledger);
    }

    /// <summary>
    /// 加工单删除/重新保存前冲销库存调整。
    /// </summary>
    internal static async Task ReverseProcessStockAsync(this Database db, JxProcess model)
    {
        var qtyChange = -GetQtyChange(model);
        if (qtyChange == 0)
            return;

        var inventory = await db.QueryAsync<JxInventory>(d => d.GoodsId == model.GoodsId);
        if (inventory == null)
            return;

        inventory.StockQty = (inventory.StockQty ?? 0) + qtyChange;
        await db.SaveAsync(inventory);

        var ledger = new JxStockLedger
        {
            GoodsId = model.GoodsId,
            BillId = model.Id,
            BillNo = model.BillNo,
            BillType = model.Type,
            QtyChange = qtyChange,
            BalanceQty = inventory.StockQty,
            BillDate = model.BillDate ?? DateTime.Now
        };
        await db.SaveAsync(ledger);
    }

    private static double GetQtyChange(JxProcess model)
    {
        if (string.IsNullOrWhiteSpace(model?.GoodsId))
            return 0;
        if (!double.TryParse(model.InputQty, out var qty) || qty == 0)
            return 0;

        return model.Type == "ProcessReturn" ? qty : -qty;
    }
}
