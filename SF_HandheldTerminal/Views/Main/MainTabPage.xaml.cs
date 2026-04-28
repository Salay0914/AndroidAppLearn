namespace SF_HandheldTerminal.Views.Main;

public partial class MainTabPage : ContentPage
{
    private readonly SF_HandheldTerminal.Views.Dashboard.Overview _overviewPage;
    private readonly SF_HandheldTerminal.Views.Catalog.Monitoring _monitoringPage;

    public MainTabPage()
    {
        InitializeComponent();
        _overviewPage = new SF_HandheldTerminal.Views.Dashboard.Overview();
        _monitoringPage = new SF_HandheldTerminal.Views.Catalog.Monitoring();

        AttachPageContent(OverviewContentHost, _overviewPage);
        AttachPageContent(MonitorContentHost, _monitoringPage);

        SettingsTabItem.Content = new SF_HandheldTerminal.Views.Settings.Settings();
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
