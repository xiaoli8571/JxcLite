namespace JxcLite.Pages.OrderData;

public partial class OrderPrint
{
    [Parameter] public OrderInfo Model { get; set; }
    [Parameter] public List<OrderListInfo> Lists { get; set; } = [];
}
