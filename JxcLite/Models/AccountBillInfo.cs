namespace JxcLite.Models;

/// <summary>
/// 对账单添加对账明细(选择关联业务单据)使用的模型。
/// </summary>
public class AccountBillInfo
{
    /// <summary>
    /// 取得或设置对账单ID。
    /// </summary>
    public string AccountId { get; set; }

    /// <summary>
    /// 取得或设置对账类型（客户、供应商）。
    /// </summary>
    public string AccountType { get; set; }

    /// <summary>
    /// 取得或设置商业伙伴ID。
    /// </summary>
    public string PartnerId { get; set; }

    /// <summary>
    /// 取得或设置选中单据的业务单号(显示用)。
    /// </summary>
    [DisplayName("选择单据")]
    public string BillNo { get; set; }

    /// <summary>
    /// 取得或设置选中单据的ID。
    /// </summary>
    public string BillId { get; set; }
}
