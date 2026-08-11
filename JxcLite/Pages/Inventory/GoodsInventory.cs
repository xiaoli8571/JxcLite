namespace JxcLite.Pages.Inventory;

/// <summary>
/// 商品库存表页面。
/// </summary>
[Route("/wms/inventories")]
[Menu(AppConstant.Inventory, "商品库存表", "table", 1)]
public class GoodsInventory : BaseTablePage<InventoryInfo>
{
    private InventoryService Service;

    protected override async Task OnInitPageAsync()
    {
        await base.OnInitPageAsync();
        Service = await CreateServiceAsync<InventoryService>();
        Table.OnQuery = Service.QueryInventoriesAsync;
    }

    [Action] public Task Export() => Table.ExportDataAsync();
}