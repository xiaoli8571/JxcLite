namespace JxcLite.Models;

/// <summary>
/// 商品库存信息类。
/// </summary>
public class InventoryInfo
{
    /// <summary>
    /// 取得或设置商品类别。
    /// </summary>
    [Column(Width = 120)]
    [DisplayName("商品类别")]
    public string Category { get; set; }

    /// <summary>
    /// 取得或设置商品编码。
    /// </summary>
    [Column(Width = 150, Ellipsis = true)]
    [DisplayName("商品编码")]
    public string Code { get; set; }

    /// <summary>
    /// 取得或设置商品名称。
    /// </summary>
    [Column(Width = 150, IsQuery = true, Ellipsis = true)]
    [DisplayName("商品名称")]
    public string Name { get; set; }

    /// <summary>
    /// 取得或设置颜色。
    /// </summary>
    [Column(Width = 100)]
    [DisplayName("颜色")]
    public string Color { get; set; }

    /// <summary>
    /// 取得或设置规格型号。
    /// </summary>
    [Column(Width = 200, Ellipsis = true)]
    [DisplayName("规格型号")]
    public string Model { get; set; }

    /// <summary>
    /// 取得或设置产地。
    /// </summary>
    [Column(Width = 120)]
    [DisplayName("产地")]
    public string Producer { get; set; }

    /// <summary>
    /// 取得或设置计量单位。
    /// </summary>
    [Column(Width = 120)]
    [DisplayName("计量单位")]
    public string Unit { get; set; }

    /// <summary>
    /// 取得或设置安全库存。
    /// </summary>
    [Column(Width = 120, Align = "right")]
    [DisplayName("安全库存")]
    public double? SafeQty { get; set; }

    /// <summary>
    /// 取得或设置期初库存。
    /// </summary>
    [Column(Width = 120, Align = "right")]
    [DisplayName("期初库存")]
    public double? InitialQty { get; set; }

    /// <summary>
    /// 取得或设置进货数量。
    /// </summary>
    [Column(Width = 120, Align = "right")]
    [DisplayName("进货数量")]
    public double? ImportQty { get; set; }

    /// <summary>
    /// 取得或设置进退货数量。
    /// </summary>
    [Column(Width = 140, Align = "right")]
    [DisplayName("进退货数量")]
    public double? ImportReturnQty { get; set; }

    /// <summary>
    /// 取得或设置销货数量。
    /// </summary>
    [Column(Width = 120, Align = "right")]
    [DisplayName("销货数量")]
    public double? ExportQty { get; set; }

    /// <summary>
    /// 取得或设置销退货数量。
    /// </summary>
    [Column(Width = 140, Align = "right")]
    [DisplayName("销退货数量")]
    public double? ExportReturnQty { get; set; }

    /// <summary>
    /// 取得或设置加工领用数量。
    /// </summary>
    [Column(Width = 140, Align = "right")]
    [DisplayName("加工领用")]
    public double? ProcessUseQty { get; set; }

    /// <summary>
    /// 取得或设置加工退回数量。
    /// </summary>
    [Column(Width = 140, Align = "right")]
    [DisplayName("加工退回")]
    public double? ProcessReturnQty { get; set; }

    /// <summary>
    /// 取得或设置当前库存。
    /// </summary>
    [Column(Width = 120, Align = "right")]
    [DisplayName("当前库存")]
    public double? InventoryQty { get; set; }
}