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
        var seriesImport = new Dictionary<string, object>();
        var seriesExport = new Dictionary<string, object>();
        for (int i = 1; i <= endDay; i++)
        {
            var date = new DateTime(now.Year, now.Month, i);
            seriesImport[i.ToString("00")] = await GetBillCountAsync(db, BillType.Import, date);
            seriesExport[i.ToString("00")] = await GetBillCountAsync(db, BillType.Export, date);
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

    private static Task<int> GetBillCountAsync(Database db, string type, DateTime date)
    {
        var day = date.ToString("yyyy-MM-dd");
        var sql = $"select count(1) from JxBill where CompNo=@CompNo and Type=@type and BillDate between '{day} 00:00:00' and '{day} 23:59:59'";
        if (db.DatabaseType == DatabaseType.Access)
            sql = sql.Replace("'", "#");
        return db.ScalarAsync<int>(sql, new { db.User.CompNo, type });
    }
}