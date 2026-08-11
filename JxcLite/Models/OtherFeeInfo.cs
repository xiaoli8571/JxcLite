namespace JxcLite.Models;

/// <summary>
/// 其他费用信息类。
/// </summary>
[DisplayName("其他费用信息")]
public class OtherFeeInfo : IAppFlowInfo
{
    public string Id { get; set; }
    public bool IsVerify { get; set; }
    public bool IsNew { get; set; }

    /// <summary>
    /// 取得或设置类型（收入、支出）。
    /// </summary>
    [Required]
    [MaxLength(50)]
    [Column(Width = 100)]
    [Category(nameof(FeeType))]
    [DisplayName("类型")]
    public string Type { get; set; }

    /// <summary>
    /// 取得或设置费用编号。
    /// </summary>
    [Required]
    [MaxLength(50)]
    [Column(Width = 120, IsQuery = true, IsViewLink = true)]
    [DisplayName("费用编号")]
    public string FeeNo { get; set; }

    /// <summary>
    /// 取得或设置单据状态。
    /// </summary>
    [Required]
    [MaxLength(50)]
    [Column(Width = 120)]
    [DisplayName("单据状态")]
    public string Status { get; set; }

    /// <summary>
    /// 取得或设置发生金额。
    /// </summary>
    [Column(Width = 120)]
    [DisplayName("发生金额")]
    public double? Amount { get; set; }

    /// <summary>
    /// 取得或设置发生日期。
    /// </summary>
    [Required]
    [Column(Width = 120, Type = FieldType.Date)]
    [DisplayName("发生日期")]
    public DateTime? FeeDate { get; set; }

    /// <summary>
    /// 取得或设置所属部门。
    /// </summary>
    [MaxLength(50)]
    [Column(Width = 150, IsQuery = true)]
    [DisplayName("所属部门")]
    public string Department { get; set; }

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
}