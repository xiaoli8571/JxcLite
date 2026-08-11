namespace JxcLite.Entities;

/// <summary>
/// 对账单表头信息类。
/// </summary>
public class JxAccount : EntityBase
{
    /// <summary>
    /// 取得或设置单据类型（客户、供应商）。
    /// </summary>
    [Required]
    [MaxLength(50)]
    [DisplayName("单据类型")]
    public string Type { get; set; }

    /// <summary>
    /// 取得或设置对账单号。
    /// </summary>
    [Required]
    [MaxLength(50)]
    [DisplayName("对账单号")]
    public string AccountNo { get; set; }

    /// <summary>
    /// 取得或设置单证状态。
    /// </summary>
    [Required]
    [MaxLength(50)]
    [DisplayName("单证状态")]
    public string Status { get; set; }

    /// <summary>
    /// 取得或设置对账日期。
    /// </summary>
    [Required]
    [DisplayName("对账日期")]
    public DateTime? AccountDate { get; set; }

    /// <summary>
    /// 取得或设置商业伙伴。
    /// </summary>
    [Required]
    [MaxLength(50)]
    [DisplayName("商业伙伴")]
    public string PartnerId { get; set; }

    /// <summary>
    /// 取得或设置业务日期。
    /// </summary>
    [Required]
    [MaxLength(100)]
    [DisplayName("业务日期")]
    public string BizDates { get; set; }

    /// <summary>
    /// 取得或设置合同号。
    /// </summary>
    [MaxLength(50)]
    [DisplayName("合同号")]
    public string ContractNo { get; set; }

    /// <summary>
    /// 取得或设置发票号。
    /// </summary>
    [MaxLength(50)]
    [DisplayName("发票号")]
    public string InvoiceNo { get; set; }

    /// <summary>
    /// 取得或设置总金额。
    /// </summary>
    [DisplayName("总金额")]
    public double? TotalAmount { get; set; }

    /// <summary>
    /// 取得或设置备注。
    /// </summary>
    [DisplayName("备注")]
    public string Note { get; set; }

    /// <summary>
    /// 取得或设置附件。
    /// </summary>
    [MaxLength(500)]
    [DisplayName("附件")]
    public string Files { get; set; }
}