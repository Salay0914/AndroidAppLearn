using Microsoft.Extensions.Logging;
using SF_HandheldTerminal.Services;
using Syncfusion.Maui.Core.Hosting;
using Syncfusion.Maui.Toolkit.Hosting;

namespace SF_HandheldTerminal;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.Services.AddSingleton<ILoginAuthService, DemoLoginAuthService>();
        builder
				.ConfigureSyncfusionToolkit()
                .UseMauiApp<App>()
                .ConfigureSyncfusionCore()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("Roboto-Medium.ttf", "Roboto-Medium");
                    fonts.AddFont("Roboto-Regular.ttf", "Roboto-Regular");
                    fonts.AddFont("UIFontIcons.ttf", "FontIcons");
                    fonts.AddFont("Dashboard.ttf", "DashboardFontIcons");
                });
			//Register Syncfusion license https://help.syncfusion.com/common/essential-studio/licensing/how-to-generate
			//Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("YOUR LICENSE KEY");

#if DEBUG
    		builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
