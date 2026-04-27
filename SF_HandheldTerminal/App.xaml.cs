using SF_HandheldTerminal.Views.Forms;

namespace SF_HandheldTerminal;

public partial class App : Application
{
    public App()
    {
        //Register Syncfusion license https://help.syncfusion.com/common/essential-studio/licensing/how-to-generate
        //Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("YOUR LICENSE KEY");
        InitializeComponent();
    }

    public static string ImageServerPath { get; } = "https://cdn.syncfusion.com/essential-ui-kit-for-.net-maui/common/uikitimages/";

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new Login());
    }
}