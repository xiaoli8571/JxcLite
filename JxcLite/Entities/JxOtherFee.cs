namespace JxcLite.Entities;

/// <summary>
/// 其他费用信息实体类。
/// </summary>
[DisplayName("其他费用信息")]
public class JxOtherFee : EntityBase
{
    public virtual bool IsVerify { get; set; }

    /// <summary>
    /// 取得或设置类型（收入、支出）。
    /// </summary>
    [Required]
    [MaxLength(50)]
    [Category(nameof(FeeType))]
    [DisplayName("类型")]
    public string Type { get; set; }

    /// <summary>
    /// 取得或设置费用编号。
    /// </summary>
    [Required]
    [MaxLength(50)]
    [DisplayName("费用编号")]
    public string FeeNo { get; set; }

    /// <summary>
    /// 取得或设置单据状态。
    /// </summary>
    [Required]
    [MaxLength(50)]
    [DisplayName("单据状态")]
    public string Status { get; set; }

    /// <summary>
    /// 取得或设置发生金额。
    /// </summary>
    [DisplayName("发生金额")]
    public double? Amount { get; set; }

    /// <summary>
    /// 取得或设置发生日期。
    /// </summary>
    [Required]
    [DisplayName("发生日期")]
    public DateTime? FeeDate { get; set; }

    /// <summary>
    /// 取得或设置所属部门。
    /// </summary>
    [MaxLength(50)]
    [DisplayName("所属部门")]
    public string Department { get; set; }

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
}