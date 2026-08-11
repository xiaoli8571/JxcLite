namespace JxcLite.Pages.Reports;

class BillDetail : BaseTable<BillDetailInfo>
{
    private ReportService Service;

    /// <summary>
    /// 取得业务单据类型（进货、进退货、销货、销退货）。
    /// </summary>
    protected virtual string Type { get; }

    protected override async Task OnInitAsync()
    {
        await base.OnInitAsync();
        Service = await CreateServiceAsync<ReportService>();
        
        Table.AutoHeight = true;
        Table.OnQuery = QueryBillDetailsAsync;
        Table.Initialize();
        Table.Column(c => c.Type).Tag();
        Table.Column(c => c.Status).Tag();
        Table.Column(c => c.SettleMode).Tag();
    }

    [Action] public Task Export() => Table.ExportDataAsync();

    private Task<PagingResult<BillDetailInfo>> QueryBillDetailsAsync(PagingCriteria criteria)
    {
        criteria.SetQuery(nameof(BillDetailInfo.Type), QueryType.Equal, Type);
        return Service.QueryBillDetailsAsync(criteria);
    }
}