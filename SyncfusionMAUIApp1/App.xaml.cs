using SyncfusionMAUIApp1;
using SyncfusionMAUIApp1.Services;
using SyncfusionMAUIApp1.Views.Forms;

namespace SyncfusionMAUIApp1;

public partial class App : Application
{
    public App()
    {
        //Register Syncfusion license https://help.syncfusion.com/common/essential-studio/licensing/how-to-generate
        Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1JHaF1cXmhMYVF0WmFZfVhgcV9EaFZSQGYuP1ZhSXxVdkFhW31fcnFUR2BaVEN9XEE=");
        InitializeComponent();
    }

    public static string ImageServerPath { get; } = "https://cdn.syncfusion.com/essential-ui-kit-for-.net-maui/common/uikitimages/";

    protected override Window CreateWindow(IActivationState? activationState)
    {
        var session = MauiProgram.Services.GetRequiredService<ISessionService>();
        Page root = session.IsAuthenticated
            ? new AppShell()
            : new NavigationPage(new Login());
        return new Window(root);
    }
}