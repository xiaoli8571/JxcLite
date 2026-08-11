namespace JxcLite.Entities;

/// <summary>
/// 商品库存信息类。
/// </summary>
public class JxInventory : EntityBase
{
    /// <summary>
    /// 取得或设置商品ID。
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string GoodsId { get; set; }

    /// <summary>
    /// 取得或设置库存数量。
    /// </summary>
    [DisplayName("库存数量")]
    public double? StockQty { get; set; }
}
