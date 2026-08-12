namespace JxcLite.Entities;

/// <summary>
/// 商品信息类。
/// </summary>
public class JxGoods : EntityBase
{
    /// <summary>
    /// 取得或设置商品类别。
    /// </summary>
    [Required]
    [MaxLength(50)]
    [Column(Width = 120)]
    [Form(Row = 1, Column = 3, Type = nameof(FieldType.Select))]
    [Category(DicCategory.GoodsType)]
    [DisplayName("商品类别")]
    public string Category { get; set; }

    /// <summary>
    /// 取得或设置商品编码。
    /// </summary>
    [Required]
    [MaxLength(50)]
    [Column(Width = 150, IsViewLink = true, Ellipsis = true)]
    [Form(Row = 1, Column = 1)]
    [DisplayName("商品编码")]
    public string Code { get; set; }

    /// <summary>
    /// 取得或设置商品名称。
    /// </summary>
    [Required]
    [MaxLength(200)]
    [Column(Width = 180, IsQuery = true, Ellipsis = true)]
    [Form(Row = 1, Column = 2)]
    [DisplayName("商品名称")]
    public string Name { get; set; }

    /// <summary>
    /// 取得或设置颜色。
    /// </summary>
    [MaxLength(100)]
    [Column(Width = 100, IsQuery = true)]
    [Form(Row = 1, Column = 3)]
    [DisplayName("颜色")]
    public string Color { get; set; }

    /// <summary>
    /// 取得或设置规格型号。
    /// </summary>
    [MaxLength(500)]
    [Column(Width = 200, Ellipsis = true)]
    [Form(Row = 2, Column = 1)]
    [DisplayName("规格型号")]
    public string Model { get; set; }

    /// <summary>
    /// 取得或设置产地。
    /// </summary>
    [MaxLength(50)]
    [Column(Width = 100)]
    [Form(Row = 3, Column = 1)]
    [DisplayName("产地")]
    public string Producer { get; set; }

    /// <summary>
    /// 取得或设置计量单位。
    /// </summary>
    [Required]
    [MaxLength(50)]
    [Column(Width = 120)]
    [Form(Row = 3, Column = 2, Type = nameof(FieldType.Select))]
    [Category(DicCategory.Unit)]
    [DisplayName("计量单位")]
    public string Unit { get; set; }

    /// <summary>
    /// 取得或设置采购单价。
    /// </summary>
    [Column(Width = 140)]
    [Form(Row = 3, Column = 3, Unit = "元")]
    [DisplayName("采购单价(元)")]
    public double? BuyPrice { get; set; }

    /// <summary>
    /// 取得或设置销售单价。
    /// </summary>
    [Column(Width = 140)]
    [Form(Row = 4, Column = 1, Unit = "元")]
    [DisplayName("销售单价(元)")]
    public double? SalePrice { get; set; }

    /// <summary>
    /// 取得或设置安全库存。
    /// </summary>
    [Column(Width = 120)]
    [Form(Row = 4, Column = 2)]
    [DisplayName("安全库存")]
    public int? SafeQty { get; set; }

    /// <summary>
    /// 取得或设置备注。
    /// </summary>
    [Column(Width = 200)]
    [Form(Row = 5, Column = 1, Type = nameof(FieldType.TextArea))]
    [DisplayName("备注")]
    public string Note { get; set; }

    /// <summary>
    /// 取得或设置附件。
    /// </summary>
    [MaxLength(500)]
    [Column(Width = 100)]
    [Form(Row = 6, Column = 1, Type = nameof(FieldType.File))]
    [DisplayName("附件")]
    public string Files { get; set; }

    // 以下为虚拟属性，不映射到数据库表中
    public virtual string ListId { get; set; }
    public virtual string BillNo { get; set; }
    public virtual double? StockQty { get; set; }
    public virtual double? Price { get; set; }
}