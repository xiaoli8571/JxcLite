using JxcLite.Pages.Inventory;
using Known.Reports;

namespace JxcLite.Pages.Reports;

[Route("/rps/imports")]
[Menu(AppConstant.Report, "进货报表", "import", 1)]
public class ImportReportPage : Report
{
    public override bool IsAdd => false;
    public override string SysId => "Import";

    protected override async Task OnInitPageAsync()
    {
        await base.OnInitPageAsync();

        Reports.Add(new ComponentInfo { Id = 1, Name = "进货明细表", Type = typeof(ImportDetail) });
        Reports.Add(new ComponentInfo { Id = 2, Name = "进退货明细表", Type = typeof(ImportReturnDetail) });
    }
}

[Route("/rps/exports")]
[Menu(AppConstant.Report, "销货报表", "export", 2)]
public class ExportReportPage : Report
{
    public override bool IsAdd => false;
    public override string SysId => "Export";

    protected override async Task OnInitPageAsync()
    {
        await base.OnInitPageAsync();

        Reports.Add(new ComponentInfo { Id = 1, Name = "销货明细表", Type = typeof(ExportDetail) });
        Reports.Add(new ComponentInfo { Id = 2, Name = "销退货明细表", Type = typeof(ExportReturnDetail) });
    }
}

[Route("/rps/stocks")]
[Menu(AppConstant.Report, "库存报表", "fund", 3)]
public class StockReportPage : Report
{
    public override bool IsAdd => false;
    public override string SysId => "Stock";

    protected override async Task OnInitPageAsync()
    {
        await base.OnInitPageAsync();

        Reports.Add(new ComponentInfo { Id = 1, Name = "商品库存表", Type = typeof(GoodsStock) });
    }
}

[Route("/rps/finances")]
//[Menu(AppConstant.Report, "财务报表", "pie-chart", 4)]  // 精简系统:隐藏财务报表菜单
public class FinanceReportPage : Report
{
    public override bool IsAdd => false;
    public override string SysId => "Finance";

    protected override async Task OnInitPageAsync()
    {
        await base.OnInitPageAsync();

        Reports.Add(new ComponentInfo { Id = 1, Name = "商品利润表", Type = typeof(GoodsProfit) });
    }
}