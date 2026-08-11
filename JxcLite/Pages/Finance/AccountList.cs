namespace JxcLite.Pages.Finance;

/// <summary>
/// 客户对账单列表页面。
/// </summary>
[Route("/fms/CustomerAccount")]
[Menu(AppConstant.Finance, "客户对账单", "unordered-list", 1)]
public class CustomerAccount : AccountList
{
    protected override string Type => PartnerType.Customer;
}

/// <summary>
/// 供应商对账单列表页面。
/// </summary>
[Route("/fms/SupplierAccount")]
[Menu(AppConstant.Finance, "供应商对账单", "unordered-list", 2)]
public class SupplierAccount : AccountList
{
    protected override string Type => PartnerType.Supplier;
}

/// <summary>
/// 对账单列表。
/// </summary>
public class AccountList : BaseTablePage<AccountInfo>
{
    private FinanceService Service;

    /// <summary>
    /// 取得对账类型（客户、供应商）。
    /// </summary>
    protected virtual string Type { get; }

    protected override async Task OnInitPageAsync()
    {
        await base.OnInitPageAsync();
        Service = await CreateServiceAsync<FinanceService>();
        Table.Toolbar.ShowCount = 6;
        Table.Form = new FormInfo { Width = 1300, NoFooter = true };
        Table.FormType = typeof(AccountForm);
        Table.OnQuery = QueryAccountsAsync;
        Table.Column(c => c.Status).Template((b, r) => b.Tag(r.Status));
        Table.Column(c => c.AccountDate).Type(FieldType.Date);

        Table.ActionCount = 3;
        Table.ActionWidth = "140";
        Table.UpdateRowActions = UpdateRowActions;
    }

    [Action] public void New() => Table.NewForm(Service.SaveAccountAsync, new AccountInfo { Type = Type });
    [Action] public void DeleteM() => Table.DeleteM(Service.DeleteAccountsAsync);
    [Action] public void Edit(AccountInfo row) => Table.EditForm(Service.SaveAccountAsync, row);
    [Action] public void Delete(AccountInfo row) => Table.Delete(Service.DeleteAccountsAsync, row);

    [Action]
    public void Verify(AccountInfo row)
    {
        row.IsVerify = true;
        Table.EditForm(Service.SaveAccountAsync, row, Language.Verify);
    }

    [Action]
    public void UnVerify(AccountInfo row)
    {
        UI.Confirm("确定要反审该记录？", async () =>
        {
            var result = await Service.UnVerifyAccountsAsync([row]);
            UI.Result(result, RefreshAsync);
        });
    }

    [Action] public Task Export() => Table.ExportDataAsync();

    private Task<PagingResult<AccountInfo>> QueryAccountsAsync(PagingCriteria criteria)
    {
        criteria.SetQuery(nameof(AccountInfo.Type), QueryType.Equal, Type);
        return Service.QueryAccountsAsync(criteria);
    }

    private void UpdateRowActions(AccountInfo row, List<ActionInfo> actions)
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