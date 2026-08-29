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
        Table.Column(c => c.InventoryQty).Template((b, r) =>
        {
            var qty = (r.InventoryQty ?? 0).ToString("0.##");
            if (r.SafeQty > 0 && (r.InventoryQty ?? 0) <= r.SafeQty)
                b.Tag(qty, "red");
            else
                b.AddContent(0, qty);
        });
    }

    [Action] public Task Export() => Table.ExportDataAsync();
}