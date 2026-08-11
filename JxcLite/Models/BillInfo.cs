namespace JxcLite.Models;

/// <summary>
/// 业务单据表头信息信息类。
/// </summary>
[DisplayName("业务单据表头信息")]
public class BillInfo : IAppFlowInfo
{
    /// <summary>
    /// 取得或设置ID。
    /// </summary>
    public string Id { get; set; }
    public bool IsVerify { get; set; }
    public bool IsVerifyForm { get; set; }
    public bool IsNew { get; set; }
    public string CreateBy { get; set; }
    public DateTime CreateTime { get; set; }

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
    [Column(Width = 120, IsQuery = true, IsViewLink = true)]
    [DisplayName("业务单号")]
    public string BillNo { get; set; }

    /// <summary>
    /// 取得或设置单证状态。
    /// </summary>
    [Required]
    [MaxLength(50)]
    [Column(Width = 120)]
    [Category(nameof(BizStatus))]
    [DisplayName("单证状态")]
    public string Status { get; set; }

    /// <summary>
    /// 取得或设置出货日期。
    /// </summary>
    [Required]
    [Column(Width = 120, IsQuery = true, Type = FieldType.Date)]
    [DisplayName("出货日期")]
    public DateTime? BillDate { get; set; }

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

    /// <summary>
    /// 取得或设置合同号。
    /// </summary>
    [MaxLength(50)]
    [Column(Width = 100)]
    [DisplayName("合同号")]
    public string ContractNo { get; set; }

    /// <summary>
    /// 取得或设置发票号。
    /// </summary>
    [MaxLength(50)]
    [Column(Width = 180)]
    [DisplayName("发票号")]
    public string InvoiceNo { get; set; }

    /// <summary>
    /// 取得或设置结算方式。
    /// </summary>
    [Required]
    [MaxLength(50)]
    [Column(Width = 120)]
    [Category(nameof(SettleModeType))]
    [DisplayName("结算方式")]
    public string SettleMode { get; set; } = SettleModeType.Cash;

    /// <summary>
    /// 取得或设置是否含税。
    /// </summary>
    [Column(Width = 100)]
    [DisplayName("含税")]
    public bool IsTax { get; set; } = true;

    /// <summary>
    /// 取得或设置总金额。
    /// </summary>
    [Column(Width = 130)]
    [DisplayName("总金额(元)")]
    public double? SumAmount { get; set; }

    /// <summary>
    /// 取得或设置总税额。
    /// </summary>
    [Column(Width = 130)]
    [DisplayName("总税额(元)")]
    public double? SumTaxAmount { get; set; }

    /// <summary>
    /// 取得或设置总价税合计。
    /// </summary>
    [Column(Width = 140)]
    [DisplayName("价税合计(元)")]
    public double? SumTotalAmount { get; set; }

    /// <summary>
    /// 取得或设置物流公司。
    /// </summary>
    [MaxLength(120)]
    [Column(Width = 150)]
    [DisplayName("物流公司")]
    public string Logistics { get; set; }

    /// <summary>
    /// 取得或设置物流单号。
    /// </summary>
    [MaxLength(50)]
    [Column(Width = 120)]
    [DisplayName("物流单号")]
    public string LogisticsNo { get; set; }

    /// <summary>
    /// 取得或设置物流费用。
    /// </summary>
    [Column(Width = 140)]
    [DisplayName("物流费用(元)")]
    public double? LogisticsFee { get; set; }

    /// <summary>
    /// 取得或设置关联单号。
    /// </summary>
    [MaxLength(50)]
    [DisplayName("关联单号")]
    public string RefBillId { get; set; }

    /// <summary>
    /// 取得或设置关联单号。
    /// </summary>
    [MaxLength(50)]
    [Column(Width = 120)]
    [DisplayName("关联单号")]
    public string RefBillNo { get; set; }

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
    [Column(Width = 100, Type = FieldType.File)]
    [DisplayName("附件")]
    public string Files { get; set; }

    public string ReturnType => Type == BillType.ImportReturn ? BillType.Import : BillType.Export;
    public FactoryInfo Factory { get; set; }
    public List<JxBillList> Lists { get; set; }

    /// <summary>
    /// 取得或设置制单人(打印时可自定义,虚拟属性不存库)。
    /// </summary>
    public virtual string PrintUser { get; set; }
}