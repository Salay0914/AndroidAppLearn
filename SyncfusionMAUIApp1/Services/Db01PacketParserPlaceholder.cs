namespace SyncfusionMAUIApp1.Services;

public sealed class Db01PacketParserPlaceholder : IDb01PacketParser
{
    public bool TryParse(ReadOnlySpan<byte> payload, out Db01Reading reading)
    {
        reading = new Db01Reading();
        _ = payload;
        return false;
    }
}
