using SQLite;

namespace SyncfusionMAUIApp1.Data;

public sealed class InspectionTaskEntity
{
    [PrimaryKey]
    public string Id { get; set; } = Guid.NewGuid().ToString("N");

    public string Title { get; set; } = string.Empty;

    public long PlannedDateUtcTicks { get; set; }

    /// <summary>逗号分隔的标识器 ID 列表。</summary>
    public string RequiredMarkerIdsCsv { get; set; } = string.Empty;
}
