namespace JxcLite.Pages.ProcessData;

/// <summary>
/// 加工单列表页面。
/// </summary>
[Route("/bms/Process")]
[Menu(AppConstant.Process, "加工单", "tool", 1)]
public class ProcessList : BaseTablePage<ProcessInfo>
{
    internal ProcessService Service;

    protected override async Task OnInitPageAsync()
    {
        await base.OnInitPageAsync();
        Service = await CreateServiceAsync<ProcessService>();
        Table.Toolbar.ShowCount = 6;
        Table.Form = new FormInfo { Width = 1100, NoFooter = true };
        Table.FormType = typeof(ProcessForm);
        Table.OnQuery = QueryProcessAsync;
        Table.Column(c => c.BillNo).Name("内部单号");
        Table.Column(c => c.FactoryNo).Name("加工厂单号");
        Table.Column(c => c.Factory).Name("加工工厂");
        Table.Column(c => c.BillDate).Type(FieldType.Date);
        Table.Column(c => c.GoodsSpec).Name("品名规格");
        Table.Column(c => c.Color).Name("颜色");
        Table.Column(c => c.InputQty).Name("投坯数量");
        Table.Column(c => c.DeliveryDate).Name("要求交期");

        Table.ActionCount = 3;
        Table.ActionWidth = "140";
        Table.UpdateRowActions = UpdateRowActions;
    }

    private Task<PagingResult<ProcessInfo>> QueryProcessAsync(PagingCriteria criteria)
    {
        criteria.SetQuery("Type", "Process");
        return Service.QueryProcessAsync(criteria);
    }

    private void UpdateRowActions(ProcessInfo row, List<ActionInfo> actions)
    {
    }

    [Action] public void New() => Table.NewForm(Service.SaveProcessAsync, new ProcessInfo { Type = "Process" });
    [Action] public void Edit(ProcessInfo row) => Table.EditForm(Service.SaveProcessAsync, row);
    [Action] public void DeleteM() => Table.DeleteM(Service.DeleteProcessAsync);
    [Action] public Task Export() => Table.ExportDataAsync();
}

/// <summary>
/// 加工退货单列表页面。
/// </summary>
[Route("/bms/ProcessReturn")]
[Menu(AppConstant.Process, "加工退货单", "rollback", 2)]
public class ProcessReturnList : BaseTablePage<ProcessInfo>
{
    internal ProcessService Service;

    protected override async Task OnInitPageAsync()
    {
        await base.OnInitPageAsync();
        Service = await CreateServiceAsync<ProcessService>();
        Table.Toolbar.ShowCount = 6;
        Table.Form = new FormInfo { Width = 1100, NoFooter = true };
        Table.FormType = typeof(ProcessForm);
        Table.OnQuery = QueryProcessAsync;
        Table.Column(c => c.BillNo).Name("内部单号");
        Table.Column(c => c.FactoryNo).Name("加工厂单号");
        Table.Column(c => c.Factory).Name("加工工厂");
        Table.Column(c => c.BillDate).Type(FieldType.Date);
        Table.Column(c => c.GoodsSpec).Name("品名规格");
        Table.Column(c => c.Color).Name("颜色");
        Table.Column(c => c.InputQty).Name("投坯数量");
        Table.Column(c => c.DeliveryDate).Name("要求交期");

        Table.ActionCount = 3;
        Table.ActionWidth = "140";
        Table.UpdateRowActions = UpdateRowActions;
    }

    private Task<PagingResult<ProcessInfo>> QueryProcessAsync(PagingCriteria criteria)
    {
        criteria.SetQuery("Type", "ProcessReturn");
        return Service.QueryProcessAsync(criteria);
    }

    private void UpdateRowActions(ProcessInfo row, List<ActionInfo> actions)
    {
    }

    [Action] public void New() => Table.NewForm(Service.SaveProcessAsync, new ProcessInfo { Type = "ProcessReturn" });
    [Action] public void Edit(ProcessInfo row) => Table.EditForm(Service.SaveProcessAsync, row);
    [Action] public void DeleteM() => Table.DeleteM(Service.DeleteProcessAsync);
    [Action] public Task Export() => Table.ExportDataAsync();
}
