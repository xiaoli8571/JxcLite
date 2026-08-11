namespace JxcLite.Entities;

/// <summary>
/// 业务单据表体信息类。
/// </summary>
public class JxBillList : EntityBase
{
    /// <summary>
    /// 取得或设置表头ID。
    /// </summary>
    [Required]
    [MaxLength(50)]
    [DisplayName("表头ID")]
    public string HeadId { get; set; }

    /// <summary>
    /// 取得或设置发票ID。
    /// </summary>
    [MaxLength(50)]
    [DisplayName("发票ID")]
    public string InvoiceId { get; set; }

    /// <summary>
    /// 取得或设置商品ID。
    /// </summary>
    [Required]
    [MaxLength(50)]
    [DisplayName("商品ID")]
    public string GoodsId { get; set; }

    /// <summary>
    /// 取得或设置序号。
    /// </summary>
    [Required]
    [DisplayName("序号")]
    public int SeqNo { get; set; }

    /// <summary>
    /// 取得或设置数量。
    /// </summary>
    [Required]
    [DisplayName("数量")]
    public double? Qty { get; set; }

    /// <summary>
    /// 取得或设置单价。
    /// </summary>
    [Required]
    [DisplayName("单价")]
    public double? Price { get; set; }

    /// <summary>
    /// 取得或设置金额。
    /// </summary>
    [Required]
    [DisplayName("金额")]
    public double? Amount { get; set; }

    /// <summary>
    /// 取得或设置税率。
    /// </summary>
    [Required]
    [DisplayName("税率")]
    public int? TaxRate { get; set; } = 13;

    /// <summary>
    /// 取得或设置税额。
    /// </summary>
    [Required]
    [DisplayName("税额")]
    public double? TaxAmount { get; set; }

    /// <summary>
    /// 取得或设置价税合计。
    /// </summary>
    [Required]
    [DisplayName("价税合计")]
    public double? TotalAmount { get; set; }

    /// <summary>
    /// 取得或设置备注。
    /// </summary>
    [DisplayName("备注")]
    public string Note { get; set; }

    // 以下为虚拟属性，不映射到数据库表中
    public virtual bool IsTax { get; set; }

    /// <summary>
    /// 取得或设置关联表体ID。
    /// </summary>
    [MaxLength(50)]
    [DisplayName("关联表体ID")]
    public string RefListId { get; set; }

    /// <summary>
    /// 取得或设置商品类别。
    /// </summary>
    [DisplayName("商品类别")]
    public virtual string Category { get; set; }

    /// <summary>
    /// 取得或设置商品编码。
    /// </summary>
    [DisplayName("商品编码")]
    public virtual string Code { get; set; }

    /// <summary>
    /// 取得或设置商品名称。
    /// </summary>
    [DisplayName("商品名称")]
    public virtual string Name { get; set; }

    /// <summary>
    /// 取得或设置规格型号。
    /// </summary>
    [DisplayName("规格型号")]
    public virtual string Model { get; set; }

    /// <summary>
    /// 取得或设置产地。
    /// </summary>
    [DisplayName("产地")]
    public virtual string Producer { get; set; }

    /// <summary>
    /// 取得或设置计量单位。
    /// </summary>
    [DisplayName("计量单位")]
    public virtual string Unit { get; set; }

    /// <summary>
    /// 取得或设置颜色。
    /// </summary>
    [DisplayName("颜色")]
    public string Color { get; set; }

    /// <summary>
    /// 取得或设置件数。
    /// </summary>
    [DisplayName("件数")]
    public double? PkgQty { get; set; }

    /// <summary>
    /// 取得或设置件数(文本,支持中文如"1支""1包")。
    /// </summary>
    [MaxLength(50)]
    [DisplayName("件数(文本)")]
    public string PkgQtyText { get; set; }

    /// <summary>
    /// 取得或设置是否整行合并(虚拟属性,不映射数据库)。
    /// </summary>
    public virtual bool IsMergedRow { get; set; }

    /// <summary>
    /// 取得或设置整行合并后的内容(虚拟属性,不映射数据库)。
    /// </summary>
    public virtual string MergeContent { get; set; }

    /// <summary>
    /// 序号(字符串形式,用于表格输入框)。
    /// </summary>
    public virtual string SeqNoText
    {
        get => SeqNo.ToString();
        set
        {
            if (int.TryParse(value, out var seq))
                SeqNo = seq;
        }
    }

    /// <summary>
    /// 是否自动补的空白行(虚拟属性,不映射数据库)。
    /// </summary>
    public virtual bool IsBlankRow { get; set; }
}