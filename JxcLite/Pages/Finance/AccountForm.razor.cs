namespace JxcLite.Pages.Finance;

public partial class AccountForm
{
    private FinanceService Service;
    private KUpload upload;

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
            var id = Model.Data.IsNew ? Model.Data.Type : Model.Data.Id;
            var data = await Service.GetAccountAsync(id);
            data.IsVerify = Model.Data.IsVerify;
            Model.Data = data;
            StateChanged();
        }
    }

    private async Task OnFilesChangedAsync(List<FileDataInfo> files)
    {
        if (Model.Data.IsNew)
        {
            Model.Files[nameof(AccountInfo.Files)] = files;
        }
        else
        {
            Model.Files[nameof(AccountInfo.Files)] = files;
            await Model.SaveAsync(d => upload.SetValueAsync(d.Files), false);
        }
    }
}