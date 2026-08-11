namespace JxcLite.Components;

public class SelectGoods : AntDropdownTable<JxGoods>
{
    private BaseDataService Service;

    [Parameter] public string Type { get; set; }
    [Parameter] public string BillId { get; set; }

    protected override Func<JxGoods, string> OnValue => d => d.Code;

    protected override async Task OnInitializeAsync()
    {
        await base.OnInitializeAsync();
        Service = await CreateServiceAsync<BaseDataService>();

        IsSearch = true;
        Table.OnQuery = QueryGoodsesAsync;
        Table.AddColumn(c => c.Category).Width(120);
        Table.AddColumn(c => c.Code, true).Width(120).ViewLink(false);
        if (Type != BillType.Import)
        {
            var stockName = "库存数量";
            if (Type == BillType.ExportReturn)
                stockName = "出货数量";
            Table.AddColumn(c => c.BillNo).Width(120).Name("业务单号");
            Table.AddColumn(c => c.StockQty).Width(120).Name(stockName);
        }
        Table.AddColumn(c => c.Name).Width(160);
        Table.AddColumn(c => c.Model).Width(200);
        Table.AddColumn(c => c.Unit).Width(120);
        Table.AddColumn(c => c.Producer).Width(120);
    }

    private Task<PagingResult<JxGoods>> QueryGoodsesAsync(PagingCriteria criteria)
    {
        criteria.Parameters[nameof(Type)] = Type;
        criteria.Parameters[nameof(BillId)] = BillId;
        return Service.QueryGoodsesAsync(criteria);
    }
}

public class SelectPartner : AntDropdownTable<JxPartner>
{
    private BaseDataService Service;

    [Parameter] public string Type { get; set; }

    protected override Func<JxPartner, string> OnValue => d => d.Name;

    protected override async Task OnInitializeAsync()
    {
        await base.OnInitializeAsync();
        Service = await CreateServiceAsync<BaseDataService>();

        IsSearch = true;
        Table.OnQuery = QueryPartnersAsync;
        Table.AddColumn(c => c.Type).Width(80).Tag();
        Table.AddColumn(c => c.Name, true).ViewLink(false).Ellipsis(true);
        Table.AddColumn(c => c.ShortName).Width(140).Ellipsis(true);
    }

    private Task<PagingResult<JxPartner>> QueryPartnersAsync(PagingCriteria criteria)
    {
        criteria.SetQuery(nameof(JxPartner.Type), QueryType.Equal, Type);
        return Service.QueryPartnersAsync(criteria);
    }
}

public class SelectBill : AntDropdownTable<BillInfo>
{
    private BillService Service;

    [Parameter] public string Type { get; set; }
    [Parameter] public string PartnerId { get; set; }

    protected override Func<BillInfo, string> OnValue => d => d.BillNo;

    protected override async Task OnInitializeAsync()
    {
        await base.OnInitializeAsync();
        Service = await CreateServiceAsync<BillService>();

        IsSearch = true;
        Table.OnQuery = QueryBillsAsync;
        Table.AddColumn(c => c.BillNo, true).ViewLink(false);
        Table.AddColumn(c => c.BillDate);
        Table.AddColumn(c => c.PartnerName).Name(AppUtils.GetPartnerName(Type)).Ellipsis(true);
        Table.AddColumn(c => c.InvoiceNo);
    }

    private Task<PagingResult<BillInfo>> QueryBillsAsync(PagingCriteria criteria)
    {
        criteria.SetQuery(nameof(BillInfo.Type), QueryType.Equal, Type);
        criteria.SetQuery(nameof(BillInfo.PartnerId), QueryType.Equal, PartnerId);
        return Service.QueryBillsAsync(criteria);
    }
}