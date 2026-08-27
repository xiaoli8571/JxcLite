using JxcLite.Pages.BaseData;

namespace JxcLite;

public static class AppConfig
{
    public static string AppId => "JxcLite";
    public static string AppName => "东帆进销存ERP系统";

    public static Func<Database, string, Task> OnBillDelete { get; set; }

    public static void AddJxcLite(this IServiceCollection services, AppType type, bool isAuto = false)
    {
        Console.WriteLine(AppName);
#if DEBUG
        Config.IsDebug = true;
        Config.IsDevelopment = true;
#endif
        if (isAuto)
            Config.RenderMode = RenderType.Auto;
        Config.AddModule(typeof(AppConfig).Assembly);

        services.AddKnown(option =>
        {
            option.Id = AppId;
            option.Name = AppName;
            option.Type = type;
            option.DefaultPageSize = 20;
            if (type == AppType.Web)
                option.IsPlatform = true;
        });
        services.AddModules();
        services.AddUIConfig();
    }

    private static void AddModules(this IServiceCollection services)
    {
        Config.Modules.AddItem("0", AppConstant.Import, "进货管理", "import", 2);
        Config.Modules.AddItem("0", AppConstant.Export, "销货管理", "export", 3);
        Config.Modules.AddItem("0", AppConstant.Inventory, "库存管理", "block", 4);
        Config.Modules.AddItem("0", AppConstant.Finance, "财务管理", "pay-circle", 5);
        Config.Modules.AddItem("0", AppConstant.Report, "统计报表", "bar-chart", 6);
        Config.Modules.AddItem("0", AppConstant.Process, "加工管理", "tool", 7);
    }

    private static void AddUIConfig(this IServiceCollection services)
    {
        UIConfig.EnableEdit = false;
        UIConfig.Copyright = "东帆进销存ERP系统 © 东帆纺织品有限公司";
        //UIConfig.SoftTerms = string.Empty;
        UIConfig.TagColor = GetTagColor;
        UIConfig.CompanyTabs.Set<FactoryList>(2, "工厂信息");

        KStyleSheet.AddStyle("_content/JxcLite/css/web.css");
        KStyleSheet.AddStyle("css/app.css");
    }

    private static string GetTagColor(string text)
    {
        if (text == BillType.Import) return "blue";
        else if (text == BillType.ImportReturn) return "orange";
        else if (text == BillType.Export) return "gold";
        else if (text == BillType.ExportReturn) return "purple";
        else if (text == SettleModeType.Cash || text == FeeType.Income || text == PartnerType.Customer) return "gold";
        else if (text == "微信" || text == FeeType.Expense) return "green";
        else if (text == "支付宝" || text == PartnerType.Supplier) return "blue";
        else if (text == SettleModeType.Account || text == "月结") return "processing";
        return "";
    }
}