namespace JxcLite.Pages.Finance;

/// <summary>
/// 其他费用列表页面。
/// </summary>
[Route("/fms/other")]
[Menu(AppConstant.Finance, "其他费用", "file", 3)]
public class FeeList : BaseTablePage<OtherFeeInfo>
{
    private FinanceService Service;

    protected override async Task OnInitPageAsync()
    {
        await base.OnInitPageAsync();
        Service = await CreateServiceAsync<FinanceService>();
        Table.Form = new FormInfo { Width = 800, NoFooter = true };
        Table.FormType = typeof(OtherFeeForm);
        Table.OnQuery = Service.QueryOtherFeesAsync;
        Table.Column(c => c.Type).Tag();
        Table.Column(c => c.Status).Tag();

        Table.ActionCount = 3;
        Table.ActionWidth = "140";
        Table.UpdateRowActions = UpdateRowActions;
    }

    [Action] public void New() => Table.NewForm(Service.SaveOtherFeeAsync, new OtherFeeInfo());
    [Action] public void DeleteM() => Table.DeleteM(Service.DeleteOtherFeesAsync);
    [Action] public void Edit(OtherFeeInfo row) => Table.EditForm(Service.SaveOtherFeeAsync, row);
    [Action] public void Delete(OtherFeeInfo row) => Table.Delete(Service.DeleteOtherFeesAsync, row);

    [Action]
    public void Verify(OtherFeeInfo row)
    {
        row.IsVerify = true;
        Table.EditForm(Service.SaveOtherFeeAsync, row, Language.Verify);
    }

    [Action]
    public void UnVerify(OtherFeeInfo row)
    {
        UI.Confirm("确定要反审该记录？", async () =>
        {
            var result = await Service.UnVerifyOtherFeesAsync([row]);
            UI.Result(result, RefreshAsync);
        });
    }

    [Action] public Task Export() => Table.ExportDataAsync();

    private void UpdateRowActions(OtherFeeInfo row, List<ActionInfo> actions)
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