namespace JxcLite.Entities;

/// <summary>
/// 对账单表体信息类。
/// </summary>
public class JxAccountList : EntityBase
{
    /// <summary>
    /// 取得或设置表头ID。
    /// </summary>
    [Required]
    [MaxLength(50)]
    [DisplayName("表头ID")]
    public string HeadId { get; set; }

    /// <summary>
    /// 取得或设置单据ID。
    /// </summary>
    [Required]
    [MaxLength(50)]
    [DisplayName("单据ID")]
    public string BillId { get; set; }
}