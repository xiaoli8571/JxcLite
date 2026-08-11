namespace JxcLite.Pages.OrderData;

/// <summary>
/// 客户订单列表页面。
/// </summary>
[Route("/bms/Order")]
[Menu(AppConstant.Export, "客户订单", "file-text", 1)]
public class OrderList : BaseTablePage<OrderInfo>
{
    internal OrderService Service;

    protected override async Task OnInitPageAsync()
    {
        await base.OnInitPageAsync();
        Service = await CreateServiceAsync<OrderService>();
        Table.Toolbar.ShowCount = 6;
        Table.Form = new FormInfo { Width = 1400, NoFooter = true };
        Table.FormType = typeof(OrderForm);
        Table.OnQuery = QueryOrdersAsync;
        Table.Column(c => c.OrderNo).Name("订单号");
        Table.Column(c => c.OrderDate).Type(FieldType.Date);
        Table.Column(c => c.CustomerName).Name("客户名称");
        Table.Column(c => c.Contact).Name("联系人");
        Table.Column(c => c.Phone).Name("电话");
        Table.Column(c => c.Address).Name("交货地点");

        Table.ActionCount = 3;
        Table.ActionWidth = "140";
        Table.UpdateRowActions = UpdateRowActions;
    }

    private Task<PagingResult<OrderInfo>> QueryOrdersAsync(PagingCriteria criteria)
    {
        return Service.QueryOrdersAsync(criteria);
    }

    private void UpdateRowActions(OrderInfo row, List<ActionInfo> actions)
    {
    }

    [Action] public void New() => Table.NewForm(Service.SaveOrderAsync, new OrderInfo());
    [Action] public void Edit(OrderInfo row) => Table.EditForm(Service.SaveOrderAsync, row);
    [Action] public void DeleteM() => Table.DeleteM(Service.DeleteOrdersAsync);
    [Action] public Task Export() => Table.ExportDataAsync();
}
