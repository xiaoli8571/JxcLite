namespace JxcLite.Extensions;

public static class MaxNoExtension
{
    public static Task<string> GetMaxBillNoAsync(this Database db, string type)
    {
        var ruleCode = AppNoRule.Import;
        if (type == BillType.Import) ruleCode = AppNoRule.Import;
        else if (type == BillType.ImportReturn) ruleCode = AppNoRule.ImportReturn;
        else if (type == BillType.Export) ruleCode = AppNoRule.Export;
        else if (type == BillType.ExportReturn) ruleCode = AppNoRule.ExportReturn;
        return db.GetMaxRuleNoAsync<JxBill>(ruleCode, nameof(JxBill.BillNo));
    }

    public static Task<string> GetMaxAccountNoAsync(this Database db, string type)
    {
        var ruleCode = type == PartnerType.Supplier ? AppNoRule.AccountSupplier : AppNoRule.AccountCustomer;
        return db.GetMaxRuleNoAsync<JxAccount>(ruleCode, nameof(JxAccount.AccountNo));
    }

    public static Task<string> GetMaxPaymentNoAsync(this Database db, string type)
    {
        var ruleCode = type == PartnerType.Supplier ? AppNoRule.PaymentOut : AppNoRule.PaymentIn;
        return db.GetMaxRuleNoAsync<JxPayment>(ruleCode, nameof(JxPayment.PaymentNo));
    }

    public static Task<string> GetMaxOtherFeeNoAsync(this Database db)
    {
        return db.GetMaxRuleNoAsync<JxOtherFee>(AppNoRule.OtherFee, nameof(JxOtherFee.FeeNo));
    }
}