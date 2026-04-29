using Microsoft.Extensions.DependencyInjection;
using SF_HandheldTerminal.Services;
using System.Diagnostics;

namespace SF_HandheldTerminal.Views.Main;

public partial class MainTabPage : ContentPage
{
    private readonly SF_HandheldTerminal.Views.Dashboard.Overview _overviewPage;
    private readonly SF_HandheldTerminal.Views.Catalog.Monitoring _monitoringPage;
    private bool _hasRequestedPermissions;

    public MainTabPage()
    {
        InitializeComponent();
        _overviewPage = new SF_HandheldTerminal.Views.Dashboard.Overview();
        _monitoringPage = new SF_HandheldTerminal.Views.Catalog.Monitoring();

        AttachPageContent(OverviewContentHost, _overviewPage);
        AttachPageContent(MonitorContentHost, _monitoringPage);

        SettingsTabItem.Content = new SF_HandheldTerminal.Views.Settings.Settings();
        Loaded += OnLoaded;
    }

    private async void OnLoaded(object? sender, EventArgs e)
    {
        if (_hasRequestedPermissions) {
            return;
        }

        _hasRequestedPermissions = true;
        var permissionService = Handler?.MauiContext?.Services.GetService<IAppPermissionService>();
        if (permissionService is null) {
            return;
        }

        try {
            await permissionService.RequestBluetoothAndLocationPermissionsAsync();
        }
        catch (Exception ex) {
            Debug.WriteLine($"[Permissions] Failed to request Bluetooth/location permissions: {ex}");
        }
    }

    private static void AttachPageContent(ContentView host, ContentPage page)
    {
        // Reuse ContentPage's visual tree inside tab hosts to avoid
        // "child already has a parent" lifecycle issues.
        var pageContent = page.Content;
        page.Content = null;
        host.Content = pageContent;
        host.BindingContext = page.BindingContext;
    }
}
