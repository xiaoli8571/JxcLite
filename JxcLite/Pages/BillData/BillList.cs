namespace JxcLite.Pages.BillData;

/// <summary>
/// 采购退货单列表页面。
/// </summary>
[Route("/bms/ImportReturn")]
[Menu(AppConstant.Import, "采购退货单", "unordered-list", 2)]
public class ImportReturn : BillList
{
    protected override string Type => BillType.ImportReturn;

    [Action] public void New() => Table.NewForm(Service.SaveBillAsync, new BillInfo { Type = Type });
    [Action] public void DeleteM() => Table.DeleteM(Service.DeleteBillsAsync);
    [Action] public Task Export() => Table.ExportDataAsync();
}

/// <summary>
/// 销售退货单列表页面。
/// </summary>
[Route("/bms/ExportReturn")]
[Menu(AppConstant.Export, "销售退货单", "unordered-list", 2)]
public class ExportReturn : BillList
{
    protected override string Type => BillType.ExportReturn;

    [Action] public void New() => Table.NewForm(Service.SaveBillAsync, new BillInfo { Type = Type });
    [Action] public void DeleteM() => Table.DeleteM(Service.DeleteBillsAsync);
    [Action] public Task Export() => Table.ExportDataAsync();
}

/// <summary>
/// 业务单据列表。
/// </summary>
public class BillList : BaseTablePage<BillInfo>
{
    internal BillService Service;

    /// <summary>
    /// 取得业务单据类型（进货、进退货、销货、销退货）。
    /// </summary>
    protected virtual string Type { get; }
    protected string PartnerName => AppUtils.GetPartnerName(Type);

    protected override async Task OnInitPageAsync()
    {
        await base.OnInitPageAsync();
        Service = await CreateServiceAsync<BillService>();
        Table.Toolbar.ShowCount = 6;
        Table.Form = new FormInfo { Width = 1400, NoFooter = true };
        Table.FormType = typeof(BillForm);
        Table.OnQuery = QueryBillsAsync;
        Table.Column(c => c.Status).Template((b, r) => b.Tag(r.Status));
        Table.Column(c => c.PartnerName).Name(PartnerName);
        Table.Column(c => c.SettleMode).Template((b, r) => b.Tag(r.SettleMode));
        Table.Column(c => c.BillDate).Type(FieldType.Date);

        Table.ActionCount = 3;
        Table.ActionWidth = "140";
        Table.UpdateRowActions = UpdateRowActions;
    }

    [Action]
    public void Edit(BillInfo row)
    {
        row.IsVerify = false;
        row.IsVerifyForm = false;
        Table.EditForm(Service.SaveBillAsync, row);
    }

    [Action] public void Delete(BillInfo row) => Table.Delete(Service.DeleteBillsAsync, row);

    [Action]
    public void Verify(BillInfo row)
    {
        row.IsVerify = true;
        row.IsVerifyForm = true;
        Table.EditForm(Service.SaveBillAsync, row, Language.Verify);
    }

    [Action]
    public void UnVerify(BillInfo row)
    {
        UI.Confirm("确定要反审该记录？", async () =>
        {
            var result = await Service.UnVerifyBillsAsync([row]);
            UI.Result(result, RefreshAsync);
        });
    }

    private Task<PagingResult<BillInfo>> QueryBillsAsync(PagingCriteria criteria)
    {
        criteria.SetQuery(nameof(BillInfo.Type), QueryType.Equal, Type);
        return Service.QueryBillsAsync(criteria);
    }

    private void UpdateRowActions(BillInfo row, List<ActionInfo> actions)
    {
        foreach (var action in actions)
        {
            if (action.Id == nameof(Edit))
                action.Visible = row.Status == BizStatus.Save;
            else if (action.Id == nameof(Delete))
                action.Visible = row.Status == BizStatus.Save;
            else if (action.Id == nameof(Verify))
                action.Visible = row.Status == BizStatus.Save;
            else if (action.Id == nameof(UnVerify))
                action.Visible = row.Status == BizStatus.Verified;
        }
    }
}