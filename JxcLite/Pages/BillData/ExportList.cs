namespace JxcLite.Pages.BillData;

/// <summary>
/// 销售出货单列表页面。
/// </summary>
[Route("/bms/ExportList")]
[Menu(AppConstant.Export, "销售出货单", "unordered-list", 1)]
public class ExportList : BillList
{
    private List<FactoryInfo> Factories = [];

    protected override string Type => BillType.Export;

    protected override async Task OnInitPageAsync()
    {
        await base.OnInitPageAsync();
        Factories = await Admin.GetFactoriesAsync();

        if (Context.HasButton(nameof(Print)))
        {
            if (Factories != null && Factories.Count > 0)
            {
                foreach (var item in Factories)
                {
                    Table.Toolbar.Items.Add(new ActionInfo
                    {
                        Name = item.ShortName,
                        Group = nameof(Print),
                        OnClick = this.Callback<MouseEventArgs>(e => Print(item.ShortName))
                    });
                }
            }
        }
    }

    [Action] public void New() => Table.NewForm(Service.SaveBillAsync, new BillInfo { Type = Type });

    [Action]
    public void Copy() => Table.SelectRow(async row =>
    {
        var row1 = await Service.GetBillAsync(Type);
        row1.PartnerId = row.PartnerId;
        row1.ContractNo = row.ContractNo;
        row1.InvoiceNo = row.InvoiceNo;
        row1.SettleMode = row.SettleMode;
        row1.Logistics = row.Logistics;
        row1.Note = row.Note;
        Table.NewForm(Service.SaveBillAsync, row1);
    });

    [Action] public void DeleteM() => Table.DeleteM(Service.DeleteBillsAsync);
    [Action(Visible = false)] public void Print() { }
    [Action] public Task Export() => Table.ExportDataAsync();

    private void Print(string factory) => Table.SelectRow(async row =>
    {
        var row1 = await Service.GetBillAsync(row.Id);
        row1.Factory = Factories?.FirstOrDefault(d => d.ShortName == factory);
        await JS.PrintAsync<BillPrint>(f => f.Set(c => c.Model, row1));
    });
}