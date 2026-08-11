namespace JxcLite.Pages.BillData;

public partial class BillPrint
{
    private string BillName => Model?.Type == BillType.Export ? "送货单" : $"{Model?.Type}单";

    [Parameter] public BillInfo Model { get; set; }

    /// <summary>
    /// 是否编辑模式(在纸张上直接编辑)。
    /// </summary>
    [Parameter] public bool IsEditMode { get; set; } = true;

    /// <summary>
    /// 是否自动合并相同商品行。
    /// </summary>
    public bool AutoMerge { get; set; } = false;

    /// <summary>
    /// 当前勾选的行。
    /// </summary>
    private List<JxBillList> SelectedRows { get; set; } = [];

    /// <summary>
    /// 每列宽度(百分比),与 ColNames 一一对应(10列:选/序号/产品名称/规格/颜色/件数/单位/数量/单价/金额)。
    /// 规格列加宽(一行放得下两个字),颜色列压缩;合计约 100%。
    /// </summary>
    [Parameter]
    public List<double> ColWidths { get; set; } = [2.2, 4.7, 21.2, 8.2, 22.2, 6.7, 6.7, 9.2, 8.2, 12.7];

    /// <summary>
    /// 列名(与 ColWidths 对应,10列含"选")。
    /// </summary>
    private List<string> ColNames { get; } = ["选", "序号", "产品名称", "规格", "颜色", "件数", "单位", "数量", "单价", "金额"];

    /// <summary>
    /// 列宽合计(便于调整到 100%)。
    /// </summary>
    private double TotalWidth => ColWidths.Sum();

    private string BillDateText { get; set; }
    private string CurrencyText { get; set; } = "RMB";

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        BillDateText = Model?.BillDate?.ToString("yyyy-MM-dd") ?? "";
    }

    /// <summary>
    /// 需要补足到的最小行数(固定6行明细区)。
    /// </summary>
    private int EmptyRows
    {
        get
        {
            var count = RenderLines?.Count ?? 0;
            return Math.Max(0, 6 - count);
        }
    }

    /// <summary>
    /// 渲染行:自动合并(相同商品)后输出。
    /// </summary>
    private List<RenderLine> RenderLines
    {
        get
        {
            var source = Model?.Lists ?? [];
            if (source.Count == 0)
                return [];

            var lines = new List<RenderLine>();

            // 自动合并:相同商品(名称+规格+颜色)合并
            var autoGroups = new List<List<JxBillList>>();
            if (AutoMerge)
            {
                foreach (var item in source)
                {
                    var last = autoGroups.LastOrDefault();
                    if (last != null && last.Count > 0
                        && last[0].Name == item.Name && last[0].Model == item.Model && last[0].Color == item.Color)
                    {
                        last.Add(item);
                    }
                    else
                    {
                        autoGroups.Add([item]);
                    }
                }
            }
            else
            {
                foreach (var item in source)
                {
                    autoGroups.Add([item]);
                }
            }

            foreach (var group in autoGroups)
            {
                lines.Add(new RenderLine(group));
            }
            return lines;
        }
    }

    /// <summary>
    /// 当前行是否整行合并。
    /// </summary>
    private bool IsRowMerged(RenderLine line)
    {
        return line != null && line.Rows.Any(r => r.IsMergedRow);
    }

    private bool IsSelected(RenderLine line)
    {
        return line.Rows.Any(r => SelectedRows.Contains(r));
    }

    /// <summary>
    /// 调整列宽。
    /// </summary>
    private void OnWidthChange(int index, object value)
    {
        if (index < 0 || index >= ColWidths.Count)
            return;

        if (double.TryParse(value?.ToString(), out var width) && width >= 2 && width <= 30)
        {
            ColWidths[index] = width;
        }
        StateHasChanged();
    }

    private void OnRowCheckChanged(RenderLine line, bool isChecked)
    {
        foreach (var row in line.Rows)
        {
            if (isChecked && !SelectedRows.Contains(row))
                SelectedRows.Add(row);
            else if (!isChecked)
                SelectedRows.Remove(row);
        }
        StateHasChanged();
    }

    /// <summary>
    /// 合并选中的行:整行合并成一个跨所有列的大格子。
    /// </summary>
    private void OnMergeSelected()
    {
        if (SelectedRows == null || SelectedRows.Count == 0)
        {
            MergeTip = "请先勾选要合并的行";
            StateHasChanged();
            return;
        }

        MergeTip = "";
        foreach (var row in SelectedRows)
        {
            row.IsMergedRow = true;
        }
        SelectedRows.Clear();
        StateHasChanged();
    }

    private string MergeTip { get; set; } = "";

    /// <summary>
    /// 取消全部手动合并。
    /// </summary>
    private void OnUnmergeAll()
    {
        foreach (var row in (Model?.Lists ?? []))
        {
            row.IsMergedRow = false;
            row.MergeContent = null;
        }
        SelectedRows.Clear();
        StateHasChanged();
    }

    /// <summary>
    /// 把纸张上编辑的内容保存回表单(调用父组件刷新)。
    /// </summary>
    private async Task OnSaveToForm()
    {
        if (!string.IsNullOrWhiteSpace(BillDateText) && DateTime.TryParse(BillDateText, out var d))
        {
            Model.BillDate = d;
        }
        if (OnChanged != null)
        {
            await OnChanged(Model);
        }
    }

    /// <summary>
    /// 请求父组件执行打印(父组件有 JS 上下文)。
    /// </summary>
    private async Task OnRequestPrint()
    {
        if (OnPrintRequest != null)
        {
            await OnPrintRequest(Model, ColWidths);
        }
    }

    [Parameter] public Func<BillInfo, Task> OnChanged { get; set; }

    [Parameter] public Func<BillInfo, List<double>, Task> OnPrintRequest { get; set; }

    /// <summary>
    /// 简单的HTML编码方法，防止XSS攻击(不转义引号,避免打印乱码)。
    /// </summary>
    private static string SafeHtmlEncode(string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        return input.Replace("&", "&amp;")
                    .Replace("<", "&lt;")
                    .Replace(">", "&gt;");
    }

    /// <summary>
    /// 渲染行:单行或合并组。
    /// </summary>
    private class RenderLine
    {
        public List<JxBillList> Rows { get; }
        public bool IsMerged => Rows.Count > 1;

        public RenderLine(List<JxBillList> rows)
        {
            Rows = rows ?? [];
        }

        public string SeqNoText => Rows.Count == 1
            ? Rows[0].SeqNo.ToString()
            : $"{Rows.First().SeqNo}-{Rows.Last().SeqNo}";

        private string JoinHtml(Func<JxBillList, string> selector)
        {
            var sb = new System.Text.StringBuilder();
            for (var i = 0; i < Rows.Count; i++)
            {
                if (i > 0) sb.Append("<br/>");
                sb.Append(SafeHtmlEncode(selector(Rows[i])));
            }
            return sb.ToString();
        }

        public string NameHtml => JoinHtml(r => r.Name);
        public string ModelHtml => JoinHtml(r => r.Model);
        public string ColorHtml => JoinHtml(r => r.Color);
        public string NoteHtml => JoinHtml(r => r.Note);
        public string UnitText => Rows.Count == 1 ? Rows[0].Unit : string.Join("/", Rows.Select(r => r.Unit).Distinct());

        public string PkgQtyText => Rows.Count == 1
            ? (!string.IsNullOrWhiteSpace(Rows[0].PkgQtyText) ? Rows[0].PkgQtyText : (Rows[0].PkgQty?.ToString() ?? ""))
            : string.Join("/", Rows.Select(r => !string.IsNullOrWhiteSpace(r.PkgQtyText) ? r.PkgQtyText : (r.PkgQty?.ToString() ?? "")).Distinct());

        public string QtyText => Rows.Count == 1
            ? (Rows[0].Qty?.ToString() ?? "")
            : Rows.Sum(r => r.Qty ?? 0).ToString();

        public string PriceText => Rows.Count == 1
            ? (Rows[0].Price?.ToString("F2") ?? "")
            : "";

        public string AmountText => Rows.Count == 1
            ? (Rows[0].Amount?.ToString("F2") ?? "")
            : Rows.Sum(r => r.Amount ?? 0).ToString("F2");
    }
}
