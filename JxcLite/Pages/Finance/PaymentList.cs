namespace JxcLite.Pages.Finance;

/// <summary>
/// 应收账款列表页面。
/// </summary>
[Route("/fms/CustomerPayment")]
[Menu(AppConstant.Finance, "应收账款", "unordered-list", 4)]
public class CustomerPayment : PaymentList
{
    protected override string Type => PartnerType.Customer;
}

/// <summary>
/// 应付账款列表页面。
/// </summary>
[Route("/fms/SupplierPayment")]
[Menu(AppConstant.Finance, "应付账款", "unordered-list", 5)]
public class SupplierPayment : PaymentList
{
    protected override string Type => PartnerType.Supplier;
}

public class PaymentList : BaseTablePage<JxPayment>
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
        Table.Form = new FormInfo { Width = 800 };
        Table.FormType = typeof(PaymentForm);
        Table.OnQuery = QueryPaymentsAsync;
        Table.Column(c => c.Status).Tag();
        Table.Column(c => c.PaidAmount).Name(Type == PartnerType.Customer ? "已收金额" : "已付金额");
    }

    [Action] public void New() => Table.NewForm(Service.SavePaymentAsync, new JxPayment { Type = Type });
    [Action] public void DeleteM() => Table.DeleteM(Service.DeletePaymentsAsync);
    [Action] public void Edit(JxPayment row) => Table.EditForm(Service.SavePaymentAsync, row);
    [Action] public void Delete(JxPayment row) => Table.Delete(Service.DeletePaymentsAsync, row);
    [Action] public Task Export() => Table.ExportDataAsync();

    private Task<PagingResult<JxPayment>> QueryPaymentsAsync(PagingCriteria criteria)
    {
        criteria.SetQuery(nameof(JxPayment.Type), QueryType.Equal, Type);
        return Service.QueryPaymentsAsync(criteria);
    }
}