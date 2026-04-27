namespace SyncfusionMAUIApp1;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute(nameof(TaskDetailPage), typeof(TaskDetailPage));
        Routing.RegisterRoute(nameof(CableArchiveDetailPage), typeof(CableArchiveDetailPage));
    }
}
