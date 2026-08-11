namespace JxcLite;

public sealed class AppMigrate
{
    public static async Task UpdateAsync(Database db)
    {
        // 创建表
        await CreateTableAsync(db);
        // 初始化数据字典
        await InitDictionaryAsync(db);
        // 初始化单据编号规则
        await InitNoRuleAsync(db);
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
    }
}