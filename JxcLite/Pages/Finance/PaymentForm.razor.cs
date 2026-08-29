namespace JxcLite.Pages.Finance;

public partial class PaymentForm
{
    private FinanceService Service;

    private string PaidAmountName => Model.Data.Type == PartnerType.Customer ? "已收金额" : "已付金额";
    private string RecordName => Model.Data.Type == PartnerType.Customer ? "收款记录" : "付款记录";

    protected override async Task OnInitFormAsync()
    {
        await base.OnInitFormAsync();
        Service = await CreateServiceAsync<FinanceService>();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);
        if (firstRender)
        {
            var id = Model.Data.IsNew ? Model.Data.Type : Model.Data.Id;
            Model.Data = await Service.GetPaymentAsync(id);
            StateChanged();
        }
    }

    private async Task OnFilesChangedAsync(List<FileDataInfo> files)
    {
        Model.Files[nameof(JxPayment.Files)] = files;
    }

    private void OnAdd()
    {
        // 预填当天日期;金额预填剩余未收付金额,减少手工录入
        var remain = Model.Data.RemainAmount ?? Model.Data.TotalAmount;
        Model.Data.Records.Add(new PaymentRecord
        {
            PayDate = DateTime.Now,
            Amount = remain > 0 ? remain : null
        });
    }

    private void OnDelete(PaymentRecord record)
    {
        Model.Data.Records.Remove(record);
    }

    private void OnAmountChange(double? amount)
    {
        Model.Data.PaidAmount = Model.Data.Records.Sum(x => x.Amount);
        Model.Data.RemainAmount = Model.Data.TotalAmount - Model.Data.PaidAmount;
    }
}