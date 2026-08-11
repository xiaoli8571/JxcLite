namespace JxcLite.Models;

/// <summary>
/// 首页信息类。
/// </summary>
public class HomeInfo
{
    /// <summary>
    /// 取得或设置常用功能菜单ID列表。
    /// </summary>
    public List<string> VisitMenuIds { get; set; }

    /// <summary>
    /// 取得或设置数据统计信息。
    /// </summary>
    public StatisticsInfo Statistics { get; set; }
}

/// <summary>
/// 数据统计信息类。
/// </summary>
public class StatisticsInfo
{
    /// <summary>
    /// 取得或设置采购进货单数量。
    /// </summary>
    public int ImportCount { get; set; }

    /// <summary>
    /// 取得或设置销售出货单数量。
    /// </summary>
    public int ExportCount { get; set; }

    /// <summary>
    /// 取得或设置单月进销货单量统计柱状图数据。
    /// </summary>
    public ChartDataInfo[] BillDatas { get; set; }
}