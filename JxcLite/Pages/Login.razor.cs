namespace JxcLite.Pages;

public partial class Login
{
    private bool IsCaptcha => Config.System?.IsLoginCaptcha == true;
    private object QrCode => new { Text = Config.HostUrl, Width = 200, Height = 200 };

    protected override Task OnInitAsync()
    {
#if DEBUG
        Model.UserName = "Admin";
        Model.Password = "1";
#endif
        return base.OnInitAsync();
    }
}