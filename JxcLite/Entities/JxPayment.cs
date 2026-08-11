namespace JxcLite.Entities;

/// <summary>
/// 收付款单信息实体类。
/// </summary>
[DisplayName("收付款单信息")]
public class JxPayment : EntityBase
{
    /// <summary>
    /// 取得或设置单据类型（应收、应付）。
    /// </summary>
    [Required]
    [MaxLength(50)]
    [DisplayName("单据类型")]
    public string Type { get; set; }

    /// <summary>
    /// 取得或设置单据编号。
    /// </summary>
    [Required]
    [MaxLength(50)]
    [Column(Width = 120, IsQuery = true, IsViewLink = true)]
    [DisplayName("单据编号")]
    public string PaymentNo { get; set; }

    /// <summary>
    /// 取得或设置单据状态。
    /// </summary>
    [Required]
    [MaxLength(50)]
    [DisplayName("单据状态")]
    public string Status { get; set; }

    /// <summary>
    /// 取得或设置单据日期。
    /// </summary>
    [Required]
    [Column(Width = 120, Type = FieldType.Date)]
    [DisplayName("单据日期")]
    public DateTime? PaymentDate { get; set; }

    /// <summary>
    /// 取得或设置商业伙伴。
    /// </summary>
    [Required]
    [MaxLength(50)]
    [DisplayName("商业伙伴")]
    public string PartnerId { get; set; }

    /// <summary>
    /// 取得或设置商业伙伴。
    /// </summary>
    [Column(Width = 180, IsQuery = true)]
    [DisplayName("商业伙伴")]
    public virtual string PartnerName { get; set; }

    /// <summary>
    /// 取得或设置单据来源（对账、新增）。
    /// </summary>
    [Required]
    [MaxLength(50)]
    [Column(Width = 120)]
    [DisplayName("单据来源")]
    public string Source { get; set; }

    /// <summary>
    /// 取得或设置总金额。
    /// </summary>
    [Column(Width = 130)]
    [DisplayName("总金额(元)")]
    public double? TotalAmount { get; set; }

    /// <summary>
    /// 取得或设置已收付金额。
    /// </summary>
    [Column(Width = 150)]
    [DisplayName("已收付金额(元)")]
    public double? PaidAmount { get; set; }

    /// <summary>
    /// 取得或设置剩余金额。
    /// </summary>
    [Column(Width = 140)]
    [DisplayName("剩余金额(元)")]
    public double? RemainAmount { get; set; }

    /// <summary>
    /// 取得或设置备注。
    /// </summary>
    [Column(Width = 200)]
    [DisplayName("备注")]
    public string Note { get; set; }

    /// <summary>
    /// 取得或设置附件。
    /// </summary>
    [MaxLength(500)]
    [Column(Width = 80, Type = FieldType.File)]
    [DisplayName("附件")]
    public string Files { get; set; }

    /// <summary>
    /// 取得或设置记录。
    /// </summary>
    [DisplayName("记录")]
    public List<PaymentRecord> Records { get; set; } = [];

    /// <summary>
    /// 取得或设置业务单据ID。
    /// </summary>
    [MaxLength(50)]
    [DisplayName("业务单据ID")]
    public string BizId { get; set; }
}

public class PaymentRecord
{
    public DateTime? PayDate { get; set; }
    public double? Amount { get; set; }
    public string Note { get; set; }
}