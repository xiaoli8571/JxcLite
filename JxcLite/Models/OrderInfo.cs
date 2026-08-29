namespace JxcLite.Models;

/// <summary>
/// 客户订单信息类。
/// </summary>
public class OrderInfo : EntityBase, IAppFlowInfo
{
    public bool IsVerify { get; set; }
    public bool IsVerifyForm { get; set; }
    public new bool IsNew { get; set; }

    /// <summary>
    /// 取得或设置状态。
    /// </summary>
    [MaxLength(50)]
    [DisplayName("状态")]
    public string Status { get; set; }

    /// <summary>
    /// 取得或设置订单号。
    /// </summary>
    [Required]
    [MaxLength(50)]
    [Column(Width = 140, IsQuery = true, IsViewLink = true)]
    [DisplayName("订单号")]
    public string OrderNo { get; set; }

    /// <summary>
    /// 取得或设置客户单据编号(对应出货单业务单号)。
    /// </summary>
    [MaxLength(50)]
    [Column(Width = 120)]
    [DisplayName("单据编号")]
    public string CustomerNo { get; set; }

    /// <summary>
    /// 取得或设置订单日期。
    /// </summary>
    [Column(Width = 100, Type = FieldType.Date)]
    [DisplayName("订单日期")]
    public DateTime? OrderDate { get; set; }

    /// <summary>
    /// 取得或设置客户名称。
    /// </summary>
    [MaxLength(100)]
    [Column(Width = 140)]
    [DisplayName("客户名称")]
    public string CustomerName { get; set; }

    /// <summary>
    /// 取得或设置联系人。
    /// </summary>
    [MaxLength(50)]
    [Column(Width = 100)]
    [DisplayName("联系人")]
    public string Contact { get; set; }

    /// <summary>
    /// 取得或设置电话。
    /// </summary>
    [MaxLength(50)]
    [Column(Width = 120)]
    [DisplayName("电话")]
    public string Phone { get; set; }

    /// <summary>
    /// 取得或设置交货地点。
    /// </summary>
    [MaxLength(200)]
    [Column(Width = 200)]
    [DisplayName("交货地点")]
    public string Address { get; set; }

    /// <summary>
    /// 取得或设置备注。
    /// </summary>
    [MaxLength(2000)]
    [DisplayName("备注")]
    public string Note { get; set; }

    /// <summary>
    /// 取得或设置审核。
    /// </summary>
    [MaxLength(50)]
    [DisplayName("审核")]
    public string Auditor { get; set; }

    /// <summary>
    /// 取得或设置制单。
    /// </summary>
    [MaxLength(50)]
    [DisplayName("制单")]
    public string Maker { get; set; }

    /// <summary>
    /// 取得或设置订单明细。
    /// </summary>
    public List<OrderListInfo> Lists { get; set; } = [];
}

/// <summary>
/// 客户订单明细信息类。
/// </summary>
public class OrderListInfo : EntityBase
{
    /// <summary>
    /// 取得或设置表头Id。
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string HeadId { get; set; }

    /// <summary>
    /// 取得或设置序号。
    /// </summary>
    [DisplayName("序号")]
    public int SeqNo { get; set; }

    /// <summary>
    /// 取得或设置存货编码。
    /// </summary>
    [MaxLength(50)]
    [DisplayName("存货编码")]
    public string GoodsCode { get; set; }

    /// <summary>
    /// 取得或设置品名。
    /// </summary>
    [MaxLength(100)]
    [DisplayName("品名")]
    public string GoodsName { get; set; }

    /// <summary>
    /// 取得或设置规格。
    /// </summary>
    [MaxLength(100)]
    [DisplayName("规格")]
    public string Spec { get; set; }

    /// <summary>
    /// 取得或设置颜色。
    /// </summary>
    [MaxLength(50)]
    [DisplayName("颜色")]
    public string Color { get; set; }

    /// <summary>
    /// 取得或设置单位。
    /// </summary>
    [MaxLength(50)]
    [DisplayName("单位")]
    public string Unit { get; set; }

    /// <summary>
    /// 取得或设置订购数量。
    /// </summary>
    [DisplayName("订购数量")]
    public double? Qty { get; set; }

    /// <summary>
    /// 取得或设置单价。
    /// </summary>
    [DisplayName("单价")]
    public double? Price { get; set; }

    /// <summary>
    /// 取得或设置金额。
    /// </summary>
    [DisplayName("金额")]
    public double? Amount { get; set; }

    /// <summary>
    /// 取得或设置客户订单号。
    /// </summary>
    [MaxLength(50)]
    [DisplayName("客户订单号")]
    public string CustomerOrderNo { get; set; }

    /// <summary>
    /// 取得或设置交货日期。
    /// </summary>
    [MaxLength(50)]
    [DisplayName("交货日期")]
    public string DeliveryDate { get; set; }

    /// <summary>
    /// 取得或设置备注。
    /// </summary>
    [MaxLength(200)]
    [DisplayName("备注")]
    public string Note { get; set; }
}
