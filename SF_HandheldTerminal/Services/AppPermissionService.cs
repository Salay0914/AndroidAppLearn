using Microsoft.Maui.ApplicationModel;

namespace SF_HandheldTerminal.Services;

public interface IAppPermissionService
{
    Task RequestBluetoothAndLocationPermissionsAsync();
}

public sealed class AppPermissionService : IAppPermissionService
{
#if ANDROID
    public async Task RequestBluetoothAndLocationPermissionsAsync()
    {
        await RequestPermissionAsync<Permissions.LocationWhenInUse>();

        if (OperatingSystem.IsAndroidVersionAtLeast(31)) {
            await RequestPermissionAsync<BluetoothRuntimePermissions>();
        }
    }
#else
    public Task RequestBluetoothAndLocationPermissionsAsync()
    {
        return Task.CompletedTask;
    }
#endif

    private static async Task<PermissionStatus> RequestPermissionAsync<TPermission>()
        where TPermission : Permissions.BasePermission, new()
    {
        var status = await Permissions.CheckStatusAsync<TPermission>();
        if (status != PermissionStatus.Granted) {
            status = await Permissions.RequestAsync<TPermission>();
        }

        return status;
    }

#if ANDROID
    private sealed class BluetoothRuntimePermissions : Permissions.BasePlatformPermission
    {
        public override (string androidPermission, bool isRuntime)[] RequiredPermissions { get; } =
        [
            (Android.Manifest.Permission.BluetoothScan, true),
            (Android.Manifest.Permission.BluetoothConnect, true),
        ];
    }
#endif
}
