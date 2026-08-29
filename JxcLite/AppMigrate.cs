namespace JxcLite;

public sealed class AppMigrate
{
    public static async Task UpdateAsync(Database db)
    {
        // 创建表
        await CreateTableAsync(db);
        // 初始化数据字典(全新环境无系统表时允许跳过,不阻塞后续索引/编号规则初始化)
        try
        {
            await InitDictionaryAsync(db);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"InitDictionary skipped: {ex.Message}");
        }
        // 创建常用索引
        await CreateIndexesAsync(db);
        // 初始化单据编号规则
        await InitNoRuleAsync(db);
        // 重建商品库存(修复历史版本重复调整库存导致的账实不符)
        await RebuildInventoryAsync(db);
    }

    /// <summary>
    /// 创建常用查询索引(提升列表与关联查询性能)。
    /// </summary>
    private static async Task CreateIndexesAsync(Database db)
    {
        var sqls = new[]
        {
            "create index if not exists IX_JxBill_Type on JxBill(Type)",
            "create index if not exists IX_JxBill_PartnerId on JxBill(PartnerId)",
            "create index if not exists IX_JxBillList_HeadId on JxBillList(HeadId)",
            "create index if not exists IX_JxBillList_GoodsId on JxBillList(GoodsId)",
            "create index if not exists IX_JxInventory_GoodsId on JxInventory(GoodsId)",
            "create index if not exists IX_JxStockLedger_GoodsId on JxStockLedger(GoodsId)",
            "create index if not exists IX_JxStockLedger_BillId on JxStockLedger(BillId)",
            "create index if not exists IX_JxPayment_BizId on JxPayment(BizId)",
            "create index if not exists IX_JxPayment_PartnerId on JxPayment(PartnerId)",
            "create index if not exists IX_JxAccountList_HeadId on JxAccountList(HeadId)",
            "create index if not exists IX_JxAccountList_BillId on JxAccountList(BillId)",
            "create index if not exists IX_JxOrder_OrderNo on JxOrder(OrderNo)",
            "create index if not exists IX_JxOrder_CustomerNo on JxOrder(CustomerNo)",
            "create index if not exists IX_JxProcess_GoodsId on JxProcess(GoodsId)",
            "create index if not exists IX_JxPartner_Type on JxPartner(Type)"
        };
        foreach (var sql in sqls)
        {
            try
            {
                await db.ExecuteAsync(sql);
            }
            catch (Exception ex)
            {
                // 索引仅影响性能;个别数据库语法差异时忽略
                Console.WriteLine($"CreateIndex failed: {sql} -> {ex.Message}");
            }
        }
    }

    /// <summary>
    /// 按公式重建库存表：期初 + 进货 - 进退 - 销货 + 销退 - 加工领用 + 加工退回。
    /// </summary>
    private static async Task RebuildInventoryAsync(Database db)
    {
        var inventories = await db.QueryListAsync<JxInventory>("select * from JxInventory");
        if (inventories != null)
        {
            foreach (var item in inventories)
                await db.DeleteAsync(item);
        }

        var goods = await db.QueryListAsync<JxGoods>("select * from JxGoods");
        if (goods == null)
            return;

        foreach (var item in goods)
        {
            var qty = await GetInventoryQtyAsync(db, item);
            await db.SaveAsync(new JxInventory { GoodsId = item.Id, StockQty = qty });
        }
    }

    private static async Task<double> GetInventoryQtyAsync(Database db, JxGoods goods)
    {
        var sql = $@"
select ifnull(a.InitialQty,0)+ifnull(b.ImportQty,0)-ifnull(b.ImportReturnQty,0)-ifnull(b.ExportQty,0)+ifnull(b.ExportReturnQty,0)-ifnull(p.ProcessUseQty,0)+ifnull(p.ProcessReturnQty,0) 
from JxGoods a
left join (
  select l.GoodsId
        ,sum(case when h.Type='{BillType.Import}' then l.Qty else 0 end) as ImportQty
        ,sum(case when h.Type='{BillType.Export}' then l.Qty else 0 end) as ExportQty
        ,sum(case when h.Type='{BillType.ImportReturn}' then l.Qty else 0 end) as ImportReturnQty
        ,sum(case when h.Type='{BillType.ExportReturn}' then l.Qty else 0 end) as ExportReturnQty 
  from JxBillList l, JxBill h 
  where l.HeadId=h.Id and l.GoodsId=@goodsId 
  group by l.GoodsId 
) b on b.GoodsId=a.Id 
left join (
  select t.GoodsId
        ,sum(case when t.Type='{AppNoRule.Process}' then t.QtyNum else 0 end) as ProcessUseQty
        ,sum(case when t.Type='ProcessReturn' then t.QtyNum else 0 end) as ProcessReturnQty
  from (
    select GoodsId,Type,cast(InputQty as real) as QtyNum 
    from JxProcess 
    where GoodsId=@goodsId and GoodsId<>'' and Status<>'{BizStatus.Verified}'
  ) t
  group by t.GoodsId 
) p on p.GoodsId=a.Id 
where a.Id=@goodsId";
        return await db.ScalarAsync<double>(sql, new { goodsId = goods.Id });
    }

    private static async Task CreateTableAsync(Database db)
    {
        await db.CreateTableAsync<JxGoods>();
        await db.CreateTableAsync<JxPartner>();
        await db.CreateTableAsync<JxBill>();
        await db.CreateTableAsync<JxBillList>();
        await db.CreateTableAsync<JxInventory>();
        await db.CreateTableAsync<JxStockLedger>();
        await db.CreateTableAsync<JxAccount>();
        await db.CreateTableAsync<JxAccountList>();
        await db.CreateTableAsync<JxOtherFee>();
        await db.CreateTableAsync<JxPayment>();
        await db.CreateTableAsync<JxOrder>();
        await db.CreateTableAsync<JxOrderList>();
        await db.CreateTableAsync<JxProcess>();
    }

    private static async Task InitDictionaryAsync(Database db)
    {
        await db.InitDictionaryAsync(AppConfig.AppId, DicCategory.GoodsType, "商品类型", [
            "金属制品"
        ]);
        await db.InitDictionaryAsync(AppConfig.AppId, DicCategory.Unit, "计量单位", [
            "pcs"
        ]);
    }

    private static async Task InitNoRuleAsync(Database db)
    {
        await db.InitNoRuleAsync(AppConfig.AppId, AppNoRule.Import, "采购进货单号", [
            new NoRuleItem(NoRuleType.Fixed, "IM"),
            new NoRuleItem(NoRuleType.DateTime, "yyyyMM"),
            new NoRuleItem(NoRuleType.Serial, "4")
        ]);
        await db.InitNoRuleAsync(AppConfig.AppId, AppNoRule.ImportReturn, "采购退货单号", [
            new NoRuleItem(NoRuleType.Fixed, "IR"),
            new NoRuleItem(NoRuleType.DateTime, "yyyyMM"),
            new NoRuleItem(NoRuleType.Serial, "4")
        ]);
        await db.InitNoRuleAsync(AppConfig.AppId, AppNoRule.Export, "销售出货单号", [
            new NoRuleItem(NoRuleType.Fixed, "EX"),
            new NoRuleItem(NoRuleType.DateTime, "yyyyMM"),
            new NoRuleItem(NoRuleType.Serial, "4")
        ]);
        await db.InitNoRuleAsync(AppConfig.AppId, AppNoRule.ExportReturn, "销售退货单号", [
            new NoRuleItem(NoRuleType.Fixed, "ER"),
            new NoRuleItem(NoRuleType.DateTime, "yyyyMM"),
            new NoRuleItem(NoRuleType.Serial, "4")
        ]);
        await db.InitNoRuleAsync(AppConfig.AppId, AppNoRule.AccountCustomer, "客户对账单号", [
            new NoRuleItem(NoRuleType.Fixed, "AC"),
            new NoRuleItem(NoRuleType.DateTime, "yyyyMM"),
            new NoRuleItem(NoRuleType.Serial, "4")
        ]);
        await db.InitNoRuleAsync(AppConfig.AppId, AppNoRule.AccountSupplier, "供应商对账单号", [
            new NoRuleItem(NoRuleType.Fixed, "AS"),
            new NoRuleItem(NoRuleType.DateTime, "yyyyMM"),
            new NoRuleItem(NoRuleType.Serial, "4")
        ]);
        await db.InitNoRuleAsync(AppConfig.AppId, AppNoRule.OtherFee, "其他费用单号", [
            new NoRuleItem(NoRuleType.Fixed, "OF"),
            new NoRuleItem(NoRuleType.DateTime, "yyyyMM"),
            new NoRuleItem(NoRuleType.Serial, "4")
        ]);
        await db.InitNoRuleAsync(AppConfig.AppId, AppNoRule.PaymentIn, "收款单号", [
            new NoRuleItem(NoRuleType.Fixed, "PC"),
            new NoRuleItem(NoRuleType.DateTime, "yyyyMM"),
            new NoRuleItem(NoRuleType.Serial, "4")
        ]);
        await db.InitNoRuleAsync(AppConfig.AppId, AppNoRule.PaymentOut, "付款单号", [
            new NoRuleItem(NoRuleType.Fixed, "PS"),
            new NoRuleItem(NoRuleType.DateTime, "yyyyMM"),
            new NoRuleItem(NoRuleType.Serial, "4")
        ]);
        await db.InitNoRuleAsync(AppConfig.AppId, AppNoRule.Order, "客户订单号", [
            new NoRuleItem(NoRuleType.Fixed, "OD"),
            new NoRuleItem(NoRuleType.DateTime, "yyyyMM"),
            new NoRuleItem(NoRuleType.Serial, "4")
        ]);
        await db.InitNoRuleAsync(AppConfig.AppId, AppNoRule.Process, "加工单号", [
            new NoRuleItem(NoRuleType.Fixed, "JG"),
            new NoRuleItem(NoRuleType.DateTime, "yyyyMM"),
            new NoRuleItem(NoRuleType.Serial, "4")
        ]);
    }
}