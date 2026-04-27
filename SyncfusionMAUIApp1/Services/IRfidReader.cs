namespace SyncfusionMAUIApp1.Services;

/// <summary>LYHR-803S RFID 模块占位；需厂商 SDK 对接。</summary>
public interface IRfidReader
{
    Task<string?> ReadOnceAsync(CancellationToken cancellationToken = default);
}
