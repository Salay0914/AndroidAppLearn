using SyncfusionMAUIApp1.Services;

namespace SyncfusionMAUIApp1;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();
    }

    private async void OnLoginClicked(object? sender, EventArgs e)
    {
        var auth = MauiProgram.Services.GetRequiredService<IAuthService>();
        var result = await auth.SignInAsync(UserEntry.Text ?? string.Empty, PasswordEntry.Text ?? string.Empty).ConfigureAwait(true);
        if (!result.Success)
        {
            await DisplayAlert("登录失败", result.Message ?? "未知错误", "确定").ConfigureAwait(true);
            return;
        }

        NavigateToShell();
    }

    private async void OnBiometricClicked(object? sender, EventArgs e)
    {
        var auth = MauiProgram.Services.GetRequiredService<IAuthService>();
        var result = await auth.SignInWithBiometricAsync().ConfigureAwait(true);
        if (!result.Success)
        {
            await DisplayAlert("登录失败", result.Message ?? "未知错误", "确定").ConfigureAwait(true);
            return;
        }

        NavigateToShell();
    }

    private static void NavigateToShell()
    {
        if (Application.Current?.Windows.Count > 0)
            Application.Current.Windows[0].Page = new AppShell();
    }
}
