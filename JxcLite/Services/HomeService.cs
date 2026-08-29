namespace JxcLite.Services;

[WebApi, Service]
class HomeService(Context context) : ServiceBase(context)
{
    public async Task<HomeInfo> GetHomeAsync()
    {
        var info = new HomeInfo();
        var user = CurrentUser;
        if (user == null)
            return info;

        await Database.QueryActionAsync(async db =>
        {
            info.VisitMenuIds = await db.GetVisitMenuIdsAsync(user.UserName, 15);
            info.Statistics = await GetStatisticsInfoAsync(db);
        });
        return info;
    }

    private static async Task<StatisticsInfo> GetStatisticsInfoAsync(Database db)
    {
        var info = new StatisticsInfo
        {
            ImportCount = await GetBillCountAsync(db, BillType.Import),
            ExportCount = await GetBillCountAsync(db, BillType.Export)
        };
        var now = DateTime.Now;
        var endDay = now.AddDays(1 - now.Day).AddMonths(1).AddDays(-1).Day;
        var importCounts = await GetMonthBillCountsAsync(db, BillType.Import, now);
        var exportCounts = await GetMonthBillCountsAsync(db, BillType.Export, now);
        var seriesImport = new Dictionary<string, object>();
        var seriesExport = new Dictionary<string, object>();
        for (int i = 1; i <= endDay; i++)
        {
            var key = i.ToString("00");
            seriesImport[key] = importCounts.TryGetValue(key, out var ic) ? ic : 0;
            seriesExport[key] = exportCounts.TryGetValue(key, out var ec) ? ec : 0;
        }
        info.BillDatas =
        [
            new ChartDataInfo { Name = BillType.Import, Series = seriesImport },
            new ChartDataInfo { Name = BillType.Export, Series = seriesExport }
        ];
        return info;
    }

    private static Task<int> GetBillCountAsync(Database db, string type)
    {
        var sql = "select count(*) from JxBill where CompNo=@CompNo and Type=@type";
        return db.ScalarAsync<int>(sql, new { db.User.CompNo, type });
    }

    /// <summary>
    /// 一次查询统计指定月份各天的单量(替代逐日查询)。
    /// </summary>
    private static async Task<Dictionary<string, int>> GetMonthBillCountsAsync(Database db, string type, DateTime now)
    {
        var result = new Dictionary<string, int>();
        var start = new DateTime(now.Year, now.Month, 1);
        var end = start.AddMonths(1);
        string sql;
        if (db.DatabaseType == DatabaseType.Access)
            sql = "select format(BillDate,'dd') as DayNo, count(*) as DayCount from JxBill where CompNo=@CompNo and Type=@type and BillDate>=@start and BillDate<@end group by format(BillDate,'dd')";
        else
            sql = "select strftime('%d', BillDate) as DayNo, count(*) as DayCount from JxBill where CompNo=@CompNo and Type=@type and BillDate>=@start and BillDate<@end group by strftime('%d', BillDate)";

        var counts = await db.QueryListAsync<DayCountInfo>(sql, new { db.User.CompNo, type, start, end });
        if (counts != null)
        {
            foreach (var item in counts)
            {
                if (!string.IsNullOrWhiteSpace(item.DayNo))
                    result[item.DayNo] = item.DayCount;
            }
        }
        return result;
    }

    private class DayCountInfo
    {
        public string DayNo { get; set; }
        public int DayCount { get; set; }
    }
}