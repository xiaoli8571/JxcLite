namespace JxcLite.Pages.OrderData;

public partial class OrderForm
{
    private OrderService Service;
    private AntTable<OrderListInfo> table;
    private new List<OrderListInfo> ListItems = [];

    protected override async Task OnInitFormAsync()
    {
        await base.OnInitFormAsync();
        Service = await CreateServiceAsync<OrderService>();
        ListItems = Model.Data.Lists ?? [];
        Model.OnSaving = async data =>
        {
            // 同步明细到保存数据
            data.Lists = ListItems;
            // 空数字字段填默认值
            foreach (var item in ListItems)
            {
                item.Qty ??= 0;
                item.Price ??= 0;
                item.Amount ??= 0;
                item.Unit ??= "件";
            }
            return true;
        };
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);
        if (firstRender)
        {
            // 编辑时按 Id 加载完整数据(含明细);新建时生成新单号
            var id = Model.Data.Id;
            var data = await Service.GetOrderAsync(id ?? "Order");
            if (data != null)
            {
                Model.Data = data;
                ListItems = data.Lists ?? [];
                StateChanged();
            }
        }
    }

    private void OnAddRow()
    {
        ListItems.Add(new OrderListInfo { SeqNo = ListItems.Count + 1 });
        StateChanged();
    }

    private async Task OnPrint()
    {
        await JS.PrintAsync<OrderPrint>(f => f.Set(c => c.Model, Model.Data)
                                              .Set(c => c.Lists, ListItems));
    }
}
