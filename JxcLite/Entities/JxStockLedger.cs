namespace JxcLite.Entities;

/// <summary>
/// 库存流水信息类。
/// </summary>
public class JxStockLedger : EntityBase
{
    /// <summary>
    /// 取得或设置商品ID。
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string GoodsId { get; set; }

    /// <summary>
    /// 取得或设置单据ID。
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string BillId { get; set; }

    /// <summary>
    /// 取得或设置业务单号。
    /// </summary>
    [Required]
    [MaxLength(50)]
    [DisplayName("业务单号")]
    public string BillNo { get; set; }

    /// <summary>
    /// 取得或设置单据类型。
    /// </summary>
    [Required]
    [MaxLength(50)]
    [DisplayName("单据类型")]
    public string BillType { get; set; }

    /// <summary>
    /// 取得或设置表体ID。
    /// </summary>
    [MaxLength(50)]
    public string ListId { get; set; }

    /// <summary>
    /// 取得或设置数量变化（正数入库，负数出库）。
    /// </summary>
    [Required]
    [DisplayName("数量变化")]
    public double? QtyChange { get; set; }

    /// <summary>
    /// 取得或设置结存数量。
    /// </summary>
    [DisplayName("结存数量")]
    public double? BalanceQty { get; set; }

    /// <summary>
    /// 取得或设置单证日期。
    /// </summary>
    [Required]
    [DisplayName("单证日期")]
    public DateTime? BillDate { get; set; }
}
