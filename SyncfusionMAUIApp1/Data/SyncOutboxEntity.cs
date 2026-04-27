using SQLite;

namespace SyncfusionMAUIApp1.Data;

public sealed class SyncOutboxEntity
{
    [PrimaryKey]
    public string Id { get; set; } = Guid.NewGuid().ToString("N");

    [Indexed]
    public string Operation { get; set; } = string.Empty;

    public string PayloadJson { get; set; } = "{}";

    public long CreatedUtcTicks { get; set; } = DateTime.UtcNow.Ticks;

    public int RetryCount { get; set; }
}
