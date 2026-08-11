namespace JxcLite.Models;

/// <summary>
/// 对账单表头信息信息类。
/// </summary>
[DisplayName("对账单表头信息")]
public class AccountInfo : IAppFlowInfo
{
    public string Id { get; set; }
    public bool IsVerify { get; set; }
    public bool IsNew { get; set; }

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
    [Column(Width = 120, IsQuery = true, IsViewLink = true)]
    [DisplayName("对账单号")]
    public string AccountNo { get; set; }

    /// <summary>
    /// 取得或设置单证状态。
    /// </summary>
    [Required]
    [MaxLength(50)]
    [Column(Width = 120)]
    [DisplayName("单证状态")]
    public string Status { get; set; }

    /// <summary>
    /// 取得或设置对账日期。
    /// </summary>
    [Required]
    [Column(Width = 120, IsQuery = true, Type = FieldType.Date)]
    [DisplayName("对账日期")]
    public DateTime? AccountDate { get; set; }

    /// <summary>
    /// 取得或设置商业伙伴ID。
    /// </summary>
    public string PartnerId { get; set; }

    /// <summary>
    /// 取得或设置商业伙伴名称。
    /// </summary>
    [Required]
    [MaxLength(100)]
    [Column(Width = 180, IsQuery = true, Ellipsis = true)]
    [DisplayName("商业伙伴")]
    public string PartnerName { get; set; }

    /// <summary>
    /// 取得或设置业务日期。
    /// </summary>
    [Required]
    [MaxLength(100)]
    [Column(Width = 120)]
    [Form]
    [DisplayName("业务日期")]
    public string BizDates { get; set; }

    /// <summary>
    /// 取得或设置合同号。
    /// </summary>
    [MaxLength(50)]
    [Column(Width = 100)]
    [Form]
    [DisplayName("合同号")]
    public string ContractNo { get; set; }

    /// <summary>
    /// 取得或设置发票号。
    /// </summary>
    [MaxLength(50)]
    [Column(Width = 100)]
    [Form]
    [DisplayName("发票号")]
    public string InvoiceNo { get; set; }

    /// <summary>
    /// 取得或设置总金额。
    /// </summary>
    [Column(Width = 130)]
    [Form]
    [DisplayName("总金额(元)")]
    public double? TotalAmount { get; set; }

    /// <summary>
    /// 取得或设置备注。
    /// </summary>
    [Column(Width = 200)]
    [Form(Type = nameof(FieldType.TextArea))]
    [DisplayName("备注")]
    public string Note { get; set; }

    /// <summary>
    /// 取得或设置附件。
    /// </summary>
    [MaxLength(500)]
    [Column(Width = 100)]
    [Form]
    [DisplayName("附件")]
    public string Files { get; set; }
}