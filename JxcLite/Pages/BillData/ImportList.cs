namespace JxcLite.Pages.BillData;

/// <summary>
/// 采购进货单列表页面。
/// </summary>
[Route("/bms/ImportList")]
[Menu(AppConstant.Import, "采购进货单", "unordered-list", 1)]
public class ImportList : BillList
{
    protected override string Type => BillType.Import;

    public static Action<ImportList> OnImportInvoice { get; set; }

    [Action] public void New() => Table.NewForm(Service.SaveBillAsync, new BillInfo { Type = Type });
    [Action] public void DeleteM() => Table.DeleteM(Service.DeleteBillsAsync);

    [Action(Name = "导入发票", Icon = "import")]
    public void ImportInvoice() => OnImportInvoice?.Invoke(this);

    [Action] public Task Export() => Table.ExportDataAsync();
}