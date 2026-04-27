using Android.Bluetooth;

namespace SyncfusionMAUIApp1.Services;

/// <summary>
/// DB01 蓝牙（BLE/SPP）对接占位：拿到协议后在此连接 GATT/SPP 并喂给 <see cref="IDb01PacketParser"/>。
/// </summary>
public sealed class Db01BluetoothAndroidClient
{
    private readonly BluetoothAdapter? _adapter = BluetoothAdapter.DefaultAdapter;

    public bool IsBluetoothSupported => _adapter is not null;

    public Task<bool> TryConnectAsync(CancellationToken cancellationToken = default)
    {
        _ = cancellationToken;
        return Task.FromResult(false);
    }
}
