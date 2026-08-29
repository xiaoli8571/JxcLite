namespace JxcLite.Pages.Finance;

public partial class AccountBillForm
{
    /// <summary>
    /// 根据对账伙伴类型取得可选单据类型（供应商=进货，客户=销货）。
    /// </summary>
    private string SelectBillType => Model.Data.AccountType == PartnerType.Supplier ? BillType.Import : BillType.Export;

    /// <summary>
    /// 选择单据后记录单据ID与单号。
    /// </summary>
    private void OnBillChange(BillInfo info)
    {
        Model.Data.BillId = info?.Id;
        Model.Data.BillNo = info?.BillNo;
    }
}
