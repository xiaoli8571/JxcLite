using System.Globalization;
using JxcLite;

// Known 在 UserSetting.Language 为空时回退到 CultureInfo.CurrentCulture。服务器是英文
// locale，会让框架按英文解析界面文案（确认弹窗按钮变成 OK/Cancel），故固定为中文。
var zhCN = new CultureInfo("zh-CN");
CultureInfo.DefaultThreadCurrentCulture = zhCN;
CultureInfo.DefaultThreadCurrentUICulture = zhCN;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();
builder.Services.AddApplication(option =>
{
    option.App.WebRoot = builder.Environment.WebRootPath;
    option.App.ContentRoot = builder.Environment.ContentRootPath;
    option.App.Assembly = typeof(Program).Assembly;
    option.App.Database = db =>
    {
        var connString = builder.Configuration["ConnString"];
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

var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAntiforgery();
app.UseApplication();
app.MapRazorComponents<App>()   
   .AddInteractiveServerRenderMode()
   .AddAdditionalAssemblies([.. Config.Assemblies]);
app.Run();