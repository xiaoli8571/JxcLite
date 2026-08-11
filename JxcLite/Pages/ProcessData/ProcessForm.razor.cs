namespace JxcLite.Pages.ProcessData;

public partial class ProcessForm
{
    private ProcessService Service;
    private List<string> Factories = [];

    private async Task<List<CodeInfo>> OnSearchFactory(string key, int size)
    {
        var list = Factories;
        if (!string.IsNullOrWhiteSpace(key))
            list = list.Where(f => f.Contains(key, StringComparison.OrdinalIgnoreCase)).ToList();
        return list.Take(size <= 0 ? 10 : size)
                   .Select(f => new CodeInfo(f, f))
                   .ToList();
    }

    protected override async Task OnInitFormAsync()
    {
        await base.OnInitFormAsync();
        Service = await CreateServiceAsync<ProcessService>();
        // 加载供应商列表作为加工工厂候选(可选+手动输入)
        try
        {
            Factories = await Service.GetFactoriesAsync();
        }
        catch
        {
            Factories = [];
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);
        if (firstRender)
        {
            var data = await Service.GetProcessAsync(Model.Data.Id ?? "Process");
            if (data != null)
            {
                Model.Data = data;
                StateChanged();
            }
        }
    }

    private async Task OnRefreshPreview()
    {
        var result = await Service.SaveProcessAsync(new UploadInfo<ProcessInfo> { Model = Model.Data });
        if (result.IsValid)
        {
            var data = await Service.GetProcessAsync(Model.Data.Id ?? Model.Data.BillNo);
            if (data != null)
            {
                Model.Data = data;
                StateChanged();
            }
        }
        UI.Result(result);
    }

    private async Task OnPrint()
    {
        await JS.PrintAsync<ProcessPrint>(f => f.Set(c => c.Model, Model.Data));
    }
}
