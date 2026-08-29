namespace JxcLite.Pages.ProcessData;

using JxcLite.Services;

public partial class ProcessForm
{
    private ProcessService Service;
    private List<string> Factories = [];
    private List<string> _goodsOptions = [];
    private List<JxGoods> _goodsList = [];
    private string _stockTip;

    /// <summary>
    /// 关联商品下拉选中值(与 GoodsId 双向同步)。
    /// </summary>
    private string SelectedGoodsId
    {
        get
        {
            var goods = _goodsList?.FirstOrDefault(g => g.Id == Model?.Data?.GoodsId);
            return goods != null ? GetGoodsOptionText(goods) : Model?.Data?.GoodsId;
        }
        set
        {
            if (Model?.Data != null && SelectedGoodsId != value)
            {
                Model.Data.GoodsId = value;
                _ = OnGoodsChanged(value);
            }
        }
    }

    /// <summary>
    /// 关联商品选择变化:自动回填品名规格/颜色并刷新当前库存提示。
    /// </summary>
    private async Task OnGoodsChanged(string goodsId)
    {
        if (string.IsNullOrWhiteSpace(goodsId))
        {
            _stockTip = null;
            StateChanged();
            return;
        }
        var goods = _goodsList.FirstOrDefault(g => GetGoodsOptionText(g) == goodsId);
        if (goods != null)
        {
            Model.Data.GoodsSpec = GetGoodsSpecText(goods);
            Model.Data.Color ??= goods.Color;
            Model.Data.GoodsId = goods.Id;
        }
        await RefreshStockTipAsync();
        StateChanged();
    }

    /// <summary>
    /// 刷新关联商品的当前库存提示(期初+采购-销售-加工领用)。
    /// </summary>
    private async Task RefreshStockTipAsync()
    {
        var goodsId = Model.Data.GoodsId;
        if (string.IsNullOrWhiteSpace(goodsId))
        {
            _stockTip = null;
            return;
        }
        try
        {
            var invService = await CreateServiceAsync<InventoryService>();
            var inv = await invService.GetInventoryByGoodsIdAsync(goodsId);
            if (inv != null)
                _stockTip = $"当前库存:{inv.InventoryQty:0.##} (期初{inv.InitialQty:0.##} 进{inv.ImportQty:0.##} 销{inv.ExportQty:0.##} 加工领用{inv.ProcessUseQty:0.##})";
            else
                _stockTip = "该商品暂无库存记录";
        }
        catch
        {
            _stockTip = null;
        }
    }

    private async Task<List<CodeInfo>> OnSearchFactory(string key, int size)
    {
        var list = Factories;
        if (!string.IsNullOrWhiteSpace(key))
            list = list.Where(f => f.Contains(key, StringComparison.OrdinalIgnoreCase)).ToList();
        return list.Take(size <= 0 ? 10 : size)
                   .Select(f => new CodeInfo(f, f))
                   .ToList();
    }

    protected override async Task OnInitFormAsync()
    {
        await base.OnInitFormAsync();
        Service = await CreateServiceAsync<ProcessService>();
        // 加载供应商列表作为加工工厂候选(可选+手动输入)
        try
        {
            Factories = await Service.GetFactoriesAsync();
        }
        catch
        {
            Factories = [];
        }
        // 加载商品下拉(关联库存)
        try
        {
            _goodsList = await Service.GetGoodsListAsync();
            foreach (var g in _goodsList)
                _goodsOptions.Add(GetGoodsOptionText(g));
            await RefreshStockTipAsync();
        }
        catch
        {
            _goodsOptions = [];
            _goodsList = [];
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);
        if (firstRender)
        {
            var data = await Service.GetProcessAsync(Model.Data.Id ?? "Process");
            if (data != null)
            {
                Model.Data = data;
                await RefreshStockTipAsync();
                StateChanged();
            }
        }
    }

    /// <summary>
    /// 构造商品下拉选项文本(含编码,保证唯一)。
    /// </summary>
    private static string GetGoodsOptionText(JxGoods g)
    {
        return $"[{g.Code}] {GetGoodsSpecText(g)}";
    }

    /// <summary>
    /// 构造品名规格文本(名称+规格+颜色)。
    /// </summary>
    private static string GetGoodsSpecText(JxGoods g)
    {
        var spec = g.Name;
        if (!string.IsNullOrWhiteSpace(g.Model)) spec += $" {g.Model}";
        if (!string.IsNullOrWhiteSpace(g.Color)) spec += $" {g.Color}";
        return spec;
    }

    private async Task OnRefreshPreview()
    {
        var result = await Service.SaveProcessAsync(new UploadInfo<ProcessInfo> { Model = Model.Data });
        if (result.IsValid)
        {
            var data = await Service.GetProcessAsync(Model.Data.Id ?? Model.Data.BillNo);
            if (data != null)
            {
                Model.Data = data;
                StateChanged();
            }
        }
        UI.Result(result);
    }

    private async Task OnPrint()
    {
        await JS.PrintAsync<ProcessPrint>(f => f.Set(c => c.Model, Model.Data));
    }

    /// <summary>
    /// 导出打印预览为 PNG 图片。
    /// </summary>
    private async Task OnExportImage()
    {
        await JS.RunVoidAsync(@"
(async () => {
    const el = document.querySelector('#process-print-area .process-print');
    if (!el) { alert('未找到打印预览区域'); return; }
    if (typeof html2canvas === 'undefined') {
        await new Promise((resolve, reject) => {
            const s = document.createElement('script');
            s.src = 'https://cdn.jsdelivr.net/npm/html2canvas@1.4.1/dist/html2canvas.min.js';
            s.onload = resolve; s.onerror = reject;
            document.head.appendChild(s);
        });
    }
    const canvas = await html2canvas(el, { scale: 2, backgroundColor: '#ffffff', useCORS: true });
    const link = document.createElement('a');
    link.download = '加工单_' + new Date().toISOString().slice(0, 10) + '.png';
    link.href = canvas.toDataURL('image/png');
    link.click();
})();
");
    }
}
