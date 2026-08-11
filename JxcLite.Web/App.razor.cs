namespace JxcLite;

public partial class App
{
    [Inject] private UIContext Context { get; set; }
    [CascadingParameter] private HttpContext HttpContext { get; set; }

    protected override void OnInitialized()
    {
        base.OnInitialized();
        Context.IPAddress = HttpContext.Connection?.RemoteIpAddress?.ToString();
        Context.IsMobile = HttpContext.CheckMobile();
        Config.HostUrl = HttpContext.GetHostUrl();
    }
}