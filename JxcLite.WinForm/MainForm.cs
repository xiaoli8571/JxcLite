using Known.Cells;

namespace JxcLite;

public partial class MainForm : Form
{
    private readonly BlazorWebView blazorWebView;

    public MainForm()
    {
        CheckForIllegalCrossThreadCalls = false;
        InitializeComponent();

        AppSetting.Load();
        blazorWebView = new BlazorWebView();
        blazorWebView.Dock = DockStyle.Fill;
        blazorWebView.BlazorWebViewInitialized = new EventHandler<BlazorWebViewInitializedEventArgs>(WebViewInitialized);
        Controls.Add(blazorWebView);
        AddBlazorWebView();

        WindowState = FormWindowState.Maximized;
        Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
        Text = AppConfig.AppName;
    }

    protected override void OnClosing(CancelEventArgs e)
    {
        base.OnClosing(e);

        var result = Dialog.Confirm("确定退出系统？");
        if (result == DialogResult.Cancel)
            e.Cancel = true;
        else
            OnClose();
    }

    private void WebViewInitialized(object sender, BlazorWebViewInitializedEventArgs e)
    {
        e.WebView.ZoomFactor = AppSetting.ZoomFactor;
    }

    private void AddBlazorWebView()
    {
        var services = new ServiceCollection();
        services.AddWindowsFormsBlazorWebView();
#if DEBUG
        Config.IsDevelopment = true;
        Config.IsDebug = true;
        services.AddBlazorWebViewDeveloperTools();
#endif
        services.AddJxcLite(AppType.Desktop);
        services.AddKnownCells();
        services.AddKnownDesktop(option =>
        {
            option.WebRoot = Application.StartupPath;
            option.ContentRoot = Application.StartupPath;
            option.Assembly = typeof(Program).Assembly;
            option.Database = db =>
            {
                var connString = "Data Source=JxcLite.db;";
                //var connString = builder.Configuration["ConnString"];
                //db.AddAccess<System.Data.OleDb.OleDbFactory>(connString);
                db.AddSQLite<Microsoft.Data.Sqlite.SqliteFactory>(connString);
                //db.AddSqlServer<Microsoft.Data.SqlClient.SqlClientFactory>(connString);
                //db.AddMySql<MySqlConnector.MySqlConnectorFactory>(connString);
                //db.AddPgSql<Npgsql.NpgsqlFactory>(connString);
                //db.AddDM<Dm.DmClientFactory>(connString);
                //db.SqlMonitor = c => Console.WriteLine($"{DateTime.Now:HH:mm:ss} {c}");
                //db.OperateMonitors.Add(info => Console.WriteLine(info.ToString()));
            };
        });

        blazorWebView.HostPage = "index.html";
        blazorWebView.Services = services.BuildServiceProvider();
        blazorWebView.RootComponents.Add<App>("#app");
        Config.OnExit = OnClose;
        Config.ServiceProvider = blazorWebView.Services;
    }

    private void OnClose()
    {
        AppSetting.ZoomFactor = blazorWebView.WebView.ZoomFactor;
        AppSetting.Save();
        Environment.Exit(0);
    }
}