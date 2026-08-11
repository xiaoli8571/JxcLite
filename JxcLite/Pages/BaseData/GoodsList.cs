namespace JxcLite.Pages.BaseData;

/// <summary>
/// 商品信息列表页面。
/// </summary>
[Route("/bds/goods")]
[Menu(Constants.BaseData, "商品信息", "ordered-list", 4)]
public class GoodsList : BaseTablePage<JxGoods>
{
    private BaseDataService Service;

    protected override async Task OnInitPageAsync()
    {
        await base.OnInitPageAsync();
        Service = await CreateServiceAsync<BaseDataService>();
        Table.Form = new FormInfo { Width = 1000 };
        Table.OnQuery = Service.QueryGoodsesAsync;
    }

    [Action] public void New() => Table.NewForm(Service.SaveGoodsAsync, new JxGoods());
    [Action] public void DeleteM() => Table.DeleteM(Service.DeleteGoodsesAsync);
    [Action] public void Edit(JxGoods row) => Table.EditForm(Service.SaveGoodsAsync, row);
    [Action] public void Delete(JxGoods row) => Table.Delete(Service.DeleteGoodsesAsync, row);
    [Action] public Task Import() => Table.ShowImportAsync();
    [Action] public Task Export() => Table.ExportDataAsync();
}