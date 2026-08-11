namespace JxcLite.Entities;

/// <summary>
/// 业务单据表头信息类。
/// </summary>
public class JxBill : EntityBase
{
    /// <summary>
    /// 取得或设置单据类型（进货、进退货、销货、销退货）。
    /// </summary>
    [Required]
    [MaxLength(50)]
    [DisplayName("单据类型")]
    public string Type { get; set; }

    /// <summary>
    /// 取得或设置业务单号。
    /// </summary>
    [Required]
    [MaxLength(50)]
    [DisplayName("业务单号")]
    public string BillNo { get; set; }

    /// <summary>
    /// 取得或设置单证状态。
    /// </summary>
    [Required]
    [MaxLength(50)]
    [DisplayName("单证状态")]
    public string Status { get; set; }

    /// <summary>
    /// 取得或设置出货日期。
    /// </summary>
    [Required]
    [DisplayName("出货日期")]
    public DateTime? BillDate { get; set; }

    /// <summary>
    /// 取得或设置商业伙伴。
    /// </summary>
    [Required]
    [MaxLength(50)]
    [DisplayName("商业伙伴")]
    public string PartnerId { get; set; }

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
    /// 取得或设置结算方式。
    /// </summary>
    [Required]
    [MaxLength(50)]
    [DisplayName("结算方式")]
    public string SettleMode { get; set; }

    /// <summary>
    /// 取得或设置总金额。
    /// </summary>
    [DisplayName("总金额")]
    public double? SumAmount { get; set; }

    /// <summary>
    /// 取得或设置总税额。
    /// </summary>
    [DisplayName("总税额")]
    public double? SumTaxAmount { get; set; }

    /// <summary>
    /// 取得或设置总价税合计。
    /// </summary>
    [DisplayName("价税合计")]
    public double? SumTotalAmount { get; set; }

    /// <summary>
    /// 取得或设置物流公司。
    /// </summary>
    [MaxLength(100)]
    [DisplayName("物流公司")]
    public string Logistics { get; set; }

    /// <summary>
    /// 取得或设置物流单号。
    /// </summary>
    [MaxLength(50)]
    [DisplayName("物流单号")]
    public string LogisticsNo { get; set; }

    /// <summary>
    /// 取得或设置物流费用。
    /// </summary>
    [DisplayName("物流费用")]
    public double? LogisticsFee { get; set; }

    /// <summary>
    /// 取得或设置关联单ID。
    /// </summary>
    [MaxLength(50)]
    [DisplayName("关联单")]
    public string RefBillId { get; set; }

    /// <summary>
    /// 取得或设置是否含税。
    /// </summary>
    [DisplayName("含税")]
    public bool IsTax { get; set; } = true;

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

    /// <summary>
    /// 取得或设置客户地址。
    /// </summary>
    [MaxLength(255)]
    [DisplayName("客户地址")]
    public string PartnerAddress { get; set; }

    /// <summary>
    /// 取得或设置客户联系人。
    /// </summary>
    [MaxLength(100)]
    [DisplayName("客户联系人")]
    public string PartnerContact { get; set; }
}