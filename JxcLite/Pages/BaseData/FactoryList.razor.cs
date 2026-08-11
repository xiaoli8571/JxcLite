namespace JxcLite.Pages.BaseData;

public partial class FactoryList
{
    private List<FactoryInfo> Items = [];

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);
        if (firstRender)
        {
            Items = await Admin.GetFactoriesAsync();
            StateChanged();
        }
    }

    private async Task OnSave()
    {
        var result = await Admin.SaveFactoriesAsync(Items);
        UI.Result(result);
    }

    private void OnAdd()
    {
        Items.Add(new FactoryInfo());
    }

    private void OnDelete(FactoryInfo item)
    {
        Items.Remove(item);
    }
}