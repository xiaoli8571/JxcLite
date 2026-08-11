namespace JxcLite.Entities;

/// <summary>
/// 商业伙伴信息类。
/// </summary>
public class JxPartner : EntityBase
{
    /// <summary>
    /// 取得或设置类型（客户、供应商、物流）。
    /// </summary>
    [Required]
    [MaxLength(50)]
    [DisplayName("类型")]
    public string Type { get; set; }

    /// <summary>
    /// 取得或设置名称。
    /// </summary>
    [Required]
    [MaxLength(100)]
    [Column(Width = 200, IsQuery = true, IsViewLink = true, Ellipsis = true)]
    [Form(Row = 1, Column = 1)]
    [DisplayName("名称")]
    public string Name { get; set; }

    /// <summary>
    /// 取得或设置简称。
    /// </summary>
    [MaxLength(50)]
    [Column(Width = 100)]
    [Form(Row = 1, Column = 2)]
    [DisplayName("简称")]
    public string ShortName { get; set; }

    /// <summary>
    /// 取得或设置信用代码。
    /// </summary>
    [MaxLength(50)]
    [Column(Width = 180)]
    [Form(Row = 1, Column = 3)]
    [DisplayName("信用代码")]
    public string SccNo { get; set; }

    /// <summary>
    /// 取得或设置联系人。
    /// </summary>
    [MaxLength(50)]
    [Column(Width = 100)]
    [Form(Row = 2, Column = 1)]
    [DisplayName("联系人")]
    public string Contact { get; set; }

    /// <summary>
    /// 取得或设置联系电话。
    /// </summary>
    [MaxLength(50)]
    [Column(Width = 120)]
    [Form(Row = 2, Column = 2)]
    [DisplayName("联系电话")]
    public string Phone { get; set; }

    /// <summary>
    /// 取得或设置联系地址。
    /// </summary>
    [MaxLength(500)]
    [Column(Width = 200)]
    [Form(Row = 3, Column = 1)]
    [DisplayName("联系地址")]
    public string Address { get; set; }

    /// <summary>
    /// 取得或设置开票地址。
    /// </summary>
    [MaxLength(500)]
    [Column(Width = 200)]
    [Form(Row = 4, Column = 1)]
    [DisplayName("开票地址")]
    public string InvAddress { get; set; }

    /// <summary>
    /// 取得或设置开票电话。
    /// </summary>
    [MaxLength(50)]
    [Column(Width = 120)]
    [Form(Row = 4, Column = 2)]
    [DisplayName("开票电话")]
    public string InvPhone { get; set; }

    /// <summary>
    /// 取得或设置开户银行。
    /// </summary>
    [MaxLength(100)]
    [Column(Width = 180)]
    [Form(Row = 5, Column = 1)]
    [DisplayName("开户银行")]
    public string Bank { get; set; }

    /// <summary>
    /// 取得或设置银行账号。
    /// </summary>
    [MaxLength(100)]
    [Column(Width = 180)]
    [Form(Row = 5, Column = 2)]
    [DisplayName("银行账号")]
    public string Account { get; set; }

    /// <summary>
    /// 取得或设置备注。
    /// </summary>
    [Column(Width = 200)]
    [Form(Row = 6, Column = 1, Type = nameof(FieldType.TextArea))]
    [DisplayName("备注")]
    public string Note { get; set; }

    /// <summary>
    /// 取得或设置附件。
    /// </summary>
    [MaxLength(500)]
    [Column(Width = 100)]
    [Form(Row = 7, Column = 1, Type = nameof(FieldType.File))]
    [DisplayName("附件")]
    public string Files { get; set; }
}