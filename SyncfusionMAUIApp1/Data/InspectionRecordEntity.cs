using SQLite;

namespace SyncfusionMAUIApp1.Data;

public sealed class InspectionRecordEntity
{
    [PrimaryKey, AutoIncrement]
    public int RowId { get; set; }

    [Indexed(Name = "task_marker", Order = 1)]
    public string TaskId { get; set; } = string.Empty;

    [Indexed(Name = "task_marker", Order = 2)]
    public string MarkerId { get; set; } = string.Empty;

    public long ReadAtUtcTicks { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public double? DepthMeters { get; set; }
    public string? PhotoPath { get; set; }
    public string Operator { get; set; } = string.Empty;
}
