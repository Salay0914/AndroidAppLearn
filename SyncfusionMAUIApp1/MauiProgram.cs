using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Syncfusion.Maui.Core.Hosting;
using SyncfusionMAUIApp1.Services;

using Syncfusion.Maui.Toolkit.Hosting;
namespace SyncfusionMAUIApp1;

public static class MauiProgram
{
    private static MauiApp? _host;

    public static IServiceProvider Services => _host?.Services ?? throw new InvalidOperationException("MauiApp 尚未初始化。");

    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
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

        RegisterAppServices(builder.Services);

        builder.Services.AddHttpClient<StationHttpApiClient>(client =>
        {
            client.BaseAddress = new Uri("https://station-host.local/");
            client.Timeout = TimeSpan.FromSeconds(30);
        });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        _host = builder.Build();
        return _host;
    }

    private static void RegisterAppServices(IServiceCollection services)
    {
        services.AddSingleton<ISessionService, SessionService>();
        services.AddSingleton<ICryptoService, CryptoService>();
        services.AddSingleton<ILocalDatabase, LocalDatabase>();
        services.AddSingleton<ISecureCredentialStore, SecureCredentialStore>();
        services.AddSingleton<IAuthService, AuthService>();
        services.AddSingleton<MockDb01Client>();
        services.AddSingleton<IDb01Client>(sp => sp.GetRequiredService<MockDb01Client>());
        services.AddSingleton<IDb01PacketParser, Db01PacketParserPlaceholder>();
        services.AddSingleton<ISignalFeedbackService, SignalFeedbackService>();
        services.AddSingleton<IGeolocationAppService, GeolocationAppService>();
        services.AddSingleton<IQrScanService, QrScanService>();
        services.AddSingleton<IRfidReader, RfidReaderStub>();
        services.AddSingleton<MockStationApi>();
        services.AddSingleton<IStationApi>(sp => sp.GetRequiredService<MockStationApi>());
        services.AddSingleton<ISyncEngine, SyncEngine>();

#if ANDROID
        services.AddSingleton<INfcReader, AndroidNfcReader>();
        services.AddSingleton<IBiometricAuthUi, AndroidBiometricAuthUi>();
#else
        services.AddSingleton<INfcReader, NfcReaderStub>();
        services.AddSingleton<IBiometricAuthUi, NoOpBiometricAuthUi>();
#endif
    }
}
