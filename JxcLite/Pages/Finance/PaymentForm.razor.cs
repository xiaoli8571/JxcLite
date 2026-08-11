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
        Model.Data.Records.Add(new PaymentRecord());
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