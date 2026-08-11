namespace JxcLite.Pages.BaseData;

/// <summary>
/// 供应商管理列表页面。
/// </summary>
[Route("/bds/suppliers")]
[Menu(Constants.BaseData, "供应商管理", "usergroup-delete", 5)]
public class SupplierList : PartnerList
{
    protected override string Type => PartnerType.Supplier;
}

/// <summary>
/// 客户管理列表页面。
/// </summary>
[Route("/bds/customers")]
[Menu(Constants.BaseData, "客户管理", "usergroup-add", 6)]
public class CustomerList : PartnerList
{
    protected override string Type => PartnerType.Customer;
}

/// <summary>
/// 商业伙伴列表。
/// </summary>
public class PartnerList : BaseTablePage<JxPartner>
{
    private BaseDataService Service;

    /// <summary>
    /// 取得商业伙伴类型（客户、供应商）。
    /// </summary>
    protected virtual string Type { get; }

    protected override async Task OnInitPageAsync()
    {
        await base.OnInitPageAsync();
        Service = await CreateServiceAsync<BaseDataService>();
        Table.Form = new FormInfo { Width = 1000 };
        Table.OnQuery = QueryPartnersAsync;
    }

    [Action] public void New() => Table.NewForm(Service.SavePartnerAsync, new JxPartner { Type = Type });
    [Action] public void DeleteM() => Table.DeleteM(Service.DeletePartnersAsync);
    [Action] public void Edit(JxPartner row) => Table.EditForm(Service.SavePartnerAsync, row);
    [Action] public void Delete(JxPartner row) => Table.Delete(Service.DeletePartnersAsync, row);
    [Action] public Task Import() => Table.ShowImportAsync();
    [Action] public Task Export() => Table.ExportDataAsync();

    private Task<PagingResult<JxPartner>> QueryPartnersAsync(PagingCriteria criteria)
    {
        criteria.SetQuery(nameof(JxPartner.Type), QueryType.Equal, Type);
        return Service.QueryPartnersAsync(criteria);
    }
}