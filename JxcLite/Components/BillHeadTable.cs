using JxcLite.Pages.Finance;

namespace JxcLite.Components;

/// <summary>
/// 通用业务单据表头表格组件。
/// </summary>
public class BillHeadTable : BaseTable<BillInfo>
{
    private BillService Service;
    private FinanceService FinService;

    /// <summary>
    /// 取得或设置对账单表头信息。
    /// </summary>
    [Parameter] public AccountInfo Account { get; set; }

    protected override async Task OnInitAsync()
    {
        await base.OnInitAsync();
        Service = await CreateServiceAsync<BillService>();
        FinService = await CreateServiceAsync<FinanceService>();
        Table.OnQuery = QueryBillsAsync;
        if (!ReadOnly)
        {
            Table.Toolbar.AddAction(nameof(New));
            Table.Toolbar.AddAction(nameof(DeleteM));
            Table.SelectType = TableSelectType.Checkbox;
        }
        Table.AddColumn(c => c.BillNo, true).Width(100);
        Table.AddColumn(c => c.Status).Width(100).Template((b, r) => b.Tag(r.Status));
        Table.AddColumn(c => c.BillDate).Width(100).Type(FieldType.Date);
        Table.AddColumn(c => c.PartnerName).Width(150);
        Table.AddColumn(c => c.ContractNo).Width(100);
        Table.AddColumn(c => c.InvoiceNo).Width(100);
        Table.AddColumn(c => c.SumAmount).Width(140).Sum();
        Table.AddColumn(c => c.SumTaxAmount).Width(140).Sum();
        Table.AddColumn(c => c.SumTotalAmount).Width(140).Sum();
        Table.AddColumn(c => c.Note).Width(200);
        if (!ReadOnly)
        {
            Table.AddAction(nameof(Delete));
        }
    }

    public void New()
    {
        if (Account.IsNew)
        {
            UI.Error("请先保存表头信息再添加明细！");
            return;
        }

        var data = new AccountBillInfo
        {
            AccountId = Account.Id,
            AccountType = Account.Type,
            PartnerId = Account.PartnerId
        };
        var model = new FormModel<AccountBillInfo>(this)
        {
            Title = "新增对账明细",
            Info = new FormInfo { Width = 500 },
            Type = typeof(AccountBillForm),
            Data = data,
            OnSave = d => FinService.SaveAccountBillAsync(new UploadInfo<AccountBillInfo> { Model = d }),
            OnSaved = async d => await RefreshAsync()
        };
        UI.ShowForm(model);
    }

    public void DeleteM() => Table.DeleteM(infos => FinService.DeleteAccountBillsAsync(Account.Id, infos));

    public void Delete(BillInfo row) => Table.Delete(infos => FinService.DeleteAccountBillsAsync(Account.Id, infos), row);

    private Task<PagingResult<BillInfo>> QueryBillsAsync(PagingCriteria criteria)
    {
        criteria.Parameters[nameof(BillQueryType)] = BillQueryType.Account;
        criteria.SetQuery("AccountId", QueryType.Equal, Account.Id);
        return Service.QueryBillsAsync(criteria);
    }
}
