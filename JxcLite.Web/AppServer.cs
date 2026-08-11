using Known.Data;

namespace JxcLite;

static class AppServer
{
    public static void AddApplication(this IServiceCollection services, Action<CoreOption> action)
    {
        services.AddJxcLite(AppType.Web);
        services.AddKnownWeb(action);
    }

    public static void UseApplication(this WebApplication app)
    {
        app.UseKnown();
        _ = app.StartApplicationAsync();
    }

    private static async Task StartApplicationAsync(this WebApplication app)
    {
        try
        {
            using var db = Database.Create();
            var time = DateTime.Parse("2026-07-25 08:00:00");
            await db.UpdateVersionAsync("UpdateTime", time, UpdateAsync);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            Logger.Exception(ex);
        }
    }

    private static async Task UpdateAsync(Database db)
    {
        await AppMigrate.UpdateAsync(db);
    }
}
