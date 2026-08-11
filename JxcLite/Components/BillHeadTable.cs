namespace JxcLite.Components;

/// <summary>
/// 通用业务单据表头表格组件。
/// </summary>
public class BillHeadTable : BaseTable<BillInfo>
{
    private BillService Service;

    /// <summary>
    /// 取得或设置对账单表头信息。
    /// </summary>
    [Parameter] public AccountInfo Account { get; set; }

    protected override async Task OnInitAsync()
    {
        await base.OnInitAsync();
        Service = await CreateServiceAsync<BillService>();
        Table.ShowPager = true;
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

        var model = new FormModel<BillInfo>(this)
        {
            Title = "新增对账明细",
            Info = new FormInfo { Width = 1100, Maximizable = true },
            //Type = typeof(ListForm), // 表单组件类型
            Data = new BillInfo(),
            //OnSaving = async d =>
            //{
            //    return true;
            //},
            //OnSave = Service.SaveListAsync, // 保存数据的方法
            OnSaved = async d => await RefreshAsync()
        };
        UI.ShowForm(model);
    }

    public void DeleteM() { }// => Table.DeleteM(Service.DeleteInvoicesAsync);
    public void Edit(BillInfo row) { }// => Table.EditForm(Service.SaveInvoiceAsync, row);
    public void Delete(BillInfo row) { }// => Table.Delete(Service.DeleteInvoicesAsync, row);

    private Task<PagingResult<BillInfo>> QueryBillsAsync(PagingCriteria criteria)
    {
        criteria.Parameters[nameof(BillQueryType)] = BillQueryType.Account;
        criteria.SetQuery("BizId", QueryType.Equal, Account.Id);
        return Service.QueryBillsAsync(criteria);
    }
}