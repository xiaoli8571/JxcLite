namespace JxcLite.Pages.Reports;

class GoodsProfit : BaseTable<ProfitInfo>
{
    private ReportService Service;

    protected override async Task OnInitAsync()
    {
        await base.OnInitAsync();
        Service = await CreateServiceAsync<ReportService>();

        Table.AutoHeight = true;
        Table.OnQuery = Service.QueryProfitsAsync;
        Table.Initialize();
    }

    [Action] public Task Export() => Table.ExportDataAsync();
}