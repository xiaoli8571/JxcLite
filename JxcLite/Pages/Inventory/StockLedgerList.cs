namespace JxcLite.Pages.Inventory;

/// <summary>
/// 库存流水查询页面。
/// </summary>
[Route("/wms/stockLedgers")]
[Menu(AppConstant.Inventory, "库存流水表", "swap", 2)]
public class StockLedgerList : BaseTablePage<StockLedgerInfo>
{
    private InventoryService Service;

    protected override async Task OnInitPageAsync()
    {
        await base.OnInitPageAsync();
        Service = await CreateServiceAsync<InventoryService>();
        Table.OnQuery = Service.QueryStockLedgersAsync;
        Table.Column(c => c.BillType).Template((b, r) => b.Tag(r.BillType));
    }

    [Action] public Task Export() => Table.ExportDataAsync();
}
