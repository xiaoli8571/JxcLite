namespace JxcLite.Models;

/// <summary>
/// 商品利润表类。
/// </summary>
public class ProfitInfo
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
    /// 取得或设置进货金额。
    /// </summary>
    [Column(Width = 140, Align = "right", IsSum = true)]
    [DisplayName("进货金额(元)")]
    public double? ImportAmount { get; set; }

    /// <summary>
    /// 取得或设置进退货金额。
    /// </summary>
    [Column(Width = 150, Align = "right", IsSum = true)]
    [DisplayName("进退货金额(元)")]
    public double? ImportReturnAmount { get; set; }

    /// <summary>
    /// 取得或设置销货金额。
    /// </summary>
    [Column(Width = 140, Align = "right", IsSum = true)]
    [DisplayName("销货金额(元)")]
    public double? ExportAmount { get; set; }

    /// <summary>
    /// 取得或设置销退货金额。
    /// </summary>
    [Column(Width = 150, Align = "right", IsSum = true)]
    [DisplayName("销退货金额(元)")]
    public double? ExportReturnAmount { get; set; }

    /// <summary>
    /// 取得或设置销售利润。
    /// </summary>
    [Column(Width = 140, Align = "right", IsSum = true)]
    [DisplayName("销售利润(元)")]
    public double? Profit { get; set; }
}