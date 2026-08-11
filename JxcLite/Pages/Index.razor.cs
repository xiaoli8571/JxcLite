namespace JxcLite.Pages;

public partial class Index
{
    private HomeService Service;
    private SpaceCard space;
    private ChartCard chart;
    private CommFuncCard func;

    protected override async Task OnInitPageAsync()
    {
        await base.OnInitPageAsync();
        Service = await CreateServiceAsync<HomeService>();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);
        if (firstRender)
        {
            var info = await Service.GetHomeAsync();
            var counts = new List<StatisticCountInfo>
            {
                new() { Name = BillType.Import, Count = info?.Statistics?.ImportCount },
                new() { Name = BillType.Export, Count = info?.Statistics?.ExportCount }
            };
            space?.SetCounts(counts);

            var option = new ChartCardOption { Id = "Order", Title = "单量统计" };
            option.Charts.Add(new CardChartInfo
            {
                Type = "Bar",
                Title = $"{DateTime.Now:yyyyMM}月进销单量统计",
                Datas = info?.Statistics?.BillDatas
            });
            await chart?.SetOptionAsync(option);

            func?.SetMenus(info?.VisitMenuIds);
        }
    }
}