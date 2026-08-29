namespace JxcLite.Extensions;

static class PaymentExtension
{
    internal static Task AddPaymentAsync(this Database db, JxBill info)
    {
        return Task.CompletedTask;
    }

    internal static Task DeletePaymentAsync(this Database db, JxBill info)
    {
        return Task.CompletedTask;
    }

    internal static async Task AddPaymentAsync(this Database db, JxAccount info)
    {
        var model = new JxPayment
        {
            Type = info.Type,
            PaymentNo = await db.GetMaxPaymentNoAsync(info.Type),
            Status = BizStatus.Save,
            PaymentDate = DateTime.Now,
            PartnerId = info.PartnerId,
            Source = PaymentSource.Account,
            TotalAmount = info.TotalAmount,
            Note = $"对账单：{info.AccountNo}",
            BizId = info.Id
        };
        await db.SaveAsync(model);
    }

    internal static async Task DeletePaymentAsync(this Database db, JxAccount info)
    {
        var model = await db.QueryAsync<JxPayment>(d => d.BizId == info.Id);
        if (model == null)
            return;

        if (model.Records != null && model.Records.Count > 0)
            throw new Exception("对账单对应的收付款单已支付，无法取消审核对账单！");

        await db.DeleteAsync(model);
    }

    internal static async Task AddPaymentAsync(this Database db, JxOtherFee info)
    {
        var type = info.Type == FeeType.Income ? PartnerType.Customer : PartnerType.Supplier;
        var model = new JxPayment
        {
            Type = type,
            PaymentNo = await db.GetMaxPaymentNoAsync(type),
            Status = BizStatus.Save,
            PaymentDate = DateTime.Now,
            PartnerId = info.Department,
            Source = PaymentSource.Other,
            TotalAmount = info.Amount,
            Note = $"其他费用单：{info.FeeNo}",
            BizId = info.Id
        };
        await db.SaveAsync(model);
    }

    internal static async Task DeletePaymentAsync(this Database db, JxOtherFee info)
    {
        var model = await db.QueryAsync<JxPayment>(d => d.BizId == info.Id);
        if (model == null)
            return;

        if (model.Records != null && model.Records.Count > 0)
            throw new Exception("其他费用对应的收付款单已支付，无法取消审核其他费用！");

        await db.DeleteAsync(model);
    }
}