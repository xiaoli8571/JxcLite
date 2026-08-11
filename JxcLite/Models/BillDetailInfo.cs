namespace JxcLite.Models;

/// <summary>
/// 进销单明细表类。
/// </summary>
public class BillDetailInfo
{
    /// <summary>
    /// 取得或设置单据类型。
    /// </summary>
    [Column(Width = 120)]
    [DisplayName("单据类型")]
    public string Type { get; set; }

    /// <summary>
    /// 取得或设置业务单号。
    /// </summary>
    [Column(Width = 120)]
    [DisplayName("业务单号")]
    public string BillNo { get; set; }

    /// <summary>
    /// 取得或设置单证状态。
    /// </summary>
    [Column(Width = 120)]
    [DisplayName("单证状态")]
    public string Status { get; set; }

    /// <summary>
    /// 取得或设置单证日期。
    /// </summary>
    [Column(Width = 120, IsQuery = true, Type = FieldType.Date)]
    [DisplayName("单证日期")]
    public DateTime BillDate { get; set; }

    /// <summary>
    /// 取得或设置商业伙伴。
    /// </summary>
    [Column(Width = 200, Ellipsis = true)]
    [DisplayName("商业伙伴")]
    public string PartnerName { get; set; }

    /// <summary>
    /// 取得或设置合同号。
    /// </summary>
    [Column(Width = 120)]
    [DisplayName("合同号")]
    public string ContractNo { get; set; }

    /// <summary>
    /// 取得或设置发票号。
    /// </summary>
    [Column(Width = 180)]
    [DisplayName("发票号")]
    public string InvoiceNo { get; set; }

    /// <summary>
    /// 取得或设置结算方式。
    /// </summary>
    [Column(Width = 120)]
    [DisplayName("结算方式")]
    public string SettleMode { get; set; }

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
    [Column(Width = 150, Ellipsis = true)]
    [DisplayName("物流公司")]
    public string Logistics { get; set; }

    /// <summary>
    /// 取得或设置物流单号。
    /// </summary>
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
    [Column(Width = 120)]
    [DisplayName("关联单号")]
    public string RefBillNo { get; set; }

    /// <summary>
    /// 取得或设置表头备注。
    /// </summary>
    [Column(Width = 200, Ellipsis = true)]
    [DisplayName("表头备注")]
    public string Note { get; set; }

    /// <summary>
    /// 取得或设置序号。
    /// </summary>
    [Column(Width = 100)]
    [DisplayName("序号")]
    public int SeqNo { get; set; }

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
    [Column(Width = 160, Ellipsis = true)]
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
    /// 取得或设置数量。
    /// </summary>
    [Column(Width = 120)]
    [DisplayName("数量")]
    public double? Qty { get; set; }

    /// <summary>
    /// 取得或设置单价。
    /// </summary>
    [Column(Width = 120)]
    [DisplayName("单价(元)")]
    public double? Price { get; set; }

    /// <summary>
    /// 取得或设置金额。
    /// </summary>
    [Column(Width = 120)]
    [DisplayName("金额(元)")]
    public double? Amount { get; set; }

    /// <summary>
    /// 取得或设置表体备注。
    /// </summary>
    [Column(Width = 200, Ellipsis = true)]
    [DisplayName("表体备注")]
    public string ListNote { get; set; }
}