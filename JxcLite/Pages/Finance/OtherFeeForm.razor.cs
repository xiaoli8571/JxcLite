namespace JxcLite.Pages.Finance;

public partial class OtherFeeForm
{
    private FinanceService Service;

    protected override async Task OnInitFormAsync()
    {
        await base.OnInitFormAsync();
        Service = await CreateServiceAsync<FinanceService>();
        this.SetSaveVerify(Model);
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);
        if (firstRender)
        {
            var data = await Service.GetOtherFeeAsync(Model.Data.Id);
            data.IsVerify = Model.Data.IsVerify;
            Model.Data = data;
            StateChanged();
        }
    }

    private async Task OnFilesChangedAsync(List<FileDataInfo> files)
    {
        Model.Files[nameof(JxOtherFee.Files)] = files;
    }
}