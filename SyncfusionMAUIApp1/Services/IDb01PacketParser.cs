namespace SyncfusionMAUIApp1.Services;

/// <summary>
/// DB01 私有协议解析占位；拿到规格书后在此实现帧解析。
/// </summary>
public interface IDb01PacketParser
{
    bool TryParse(ReadOnlySpan<byte> payload, out Db01Reading reading);
}
