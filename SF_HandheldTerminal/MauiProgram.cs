using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;
using SF_HandheldTerminal.Services;
using Syncfusion.Maui.Core.Hosting;
using Syncfusion.Maui.Toolkit.Hosting;
using System.Diagnostics;

namespace SF_HandheldTerminal;

public static class MauiProgram
{
    private static bool _diagnosticsHooked;

    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.Services.AddSingleton<ILoginAuthService, DemoLoginAuthService>();
        builder.Services.AddSingleton<IAppPermissionService, AppPermissionService>();
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
        HookGlobalDiagnostics();
        builder.Logging.AddDebug();
        builder.Logging.SetMinimumLevel(LogLevel.Trace);

        builder.ConfigureLifecycleEvents(events =>
        {
#if WINDOWS
            events.AddWindows(windows =>
            {
                windows.OnLaunched((app, _) =>
                {
                    app.UnhandledException += (_, args) =>
                    {
                        Debug.WriteLine($"[WinUI.UnhandledException] {args.Exception}");
                    };
                });
            });
#endif
        });
#endif

        return builder.Build();
    }

#if DEBUG

    private static void HookGlobalDiagnostics()
    {
        if (_diagnosticsHooked) {
            return;
        }

        _diagnosticsHooked = true;

        AppDomain.CurrentDomain.FirstChanceException += (_, e) =>
        {
            Debug.WriteLine($"[FirstChance] {e.Exception.GetType().FullName}: {e.Exception.Message}");
            Debug.WriteLine(e.Exception.StackTrace);
        };

        AppDomain.CurrentDomain.UnhandledException += (_, e) =>
        {
            Debug.WriteLine($"[AppDomain.UnhandledException] {e.ExceptionObject}");
        };

        TaskScheduler.UnobservedTaskException += (_, e) =>
        {
            Debug.WriteLine($"[TaskScheduler.UnobservedTaskException] {e.Exception}");
        };
    }

#endif
}