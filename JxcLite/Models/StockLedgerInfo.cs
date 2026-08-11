namespace JxcLite.Models;

/// <summary>
/// 库存流水信息类。
/// </summary>
public class StockLedgerInfo
{
    [Column(Width = 120)]
    [DisplayName("单据类型")]
    public string BillType { get; set; }

    [Column(Width = 120)]
    [DisplayName("业务单号")]
    public string BillNo { get; set; }

    [Column(Width = 120, Type = FieldType.Date)]
    [DisplayName("单据日期")]
    public DateTime? BillDate { get; set; }

    [Column(Width = 120)]
    [DisplayName("商品类别")]
    public string Category { get; set; }

    [Column(Width = 150, Ellipsis = true)]
    [DisplayName("商品编码")]
    public string Code { get; set; }

    [Column(Width = 160, IsQuery = true, Ellipsis = true)]
    [DisplayName("商品名称")]
    public string Name { get; set; }

    [Column(Width = 200, Ellipsis = true)]
    [DisplayName("规格型号")]
    public string Model { get; set; }

    [Column(Width = 120)]
    [DisplayName("计量单位")]
    public string Unit { get; set; }

    [Column(Width = 120, Align = "right")]
    [DisplayName("入库数量")]
    public double? InQty { get; set; }

    [Column(Width = 120, Align = "right")]
    [DisplayName("出库数量")]
    public double? OutQty { get; set; }

    [Column(Width = 120, Align = "right")]
    [DisplayName("结存数量")]
    public double? BalanceQty { get; set; }

    [Column(Width = 150, Ellipsis = true)]
    [DisplayName("商业伙伴")]
    public string PartnerName { get; set; }
}
