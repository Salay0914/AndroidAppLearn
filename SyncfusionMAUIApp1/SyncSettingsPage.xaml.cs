using SyncfusionMAUIApp1.Services;

namespace SyncfusionMAUIApp1;

public partial class SyncSettingsPage : ContentPage
{
    public SyncSettingsPage()
    {
        InitializeComponent();
    }

    private async void OnSyncClicked(object? sender, EventArgs e)
    {
        var sync = MauiProgram.Services.GetRequiredService<ISyncEngine>();
        var n = await sync.ProcessOutboxAsync().ConfigureAwait(true);
        SyncResultLabel.Text = $"本次处理 {n} 条队列记录。";
    }

    private void OnLogoutClicked(object? sender, EventArgs e)
    {
        var session = MauiProgram.Services.GetRequiredService<ISessionService>();
        var creds = MauiProgram.Services.GetRequiredService<ISecureCredentialStore>();
        session.Logout();
        _ = creds.ClearAsync();

        if (Application.Current?.Windows.Count > 0)
            Application.Current.Windows[0].Page = new NavigationPage(new LoginPage());
    }
}
