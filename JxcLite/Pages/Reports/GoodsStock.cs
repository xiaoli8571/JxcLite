namespace JxcLite.Pages.Reports;

class GoodsStock : BaseTable<InventoryInfo>
{
    private InventoryService Service;

    protected override async Task OnInitAsync()
    {
        await base.OnInitAsync();
        Service = await CreateServiceAsync<InventoryService>();

        Table.AutoHeight = true;
        Table.OnQuery = Service.QueryInventoriesAsync;
        Table.Initialize();
    }

    [Action] public Task Export() => Table.ExportDataAsync();
}