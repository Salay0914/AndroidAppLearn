using SQLite;

namespace SyncfusionMAUIApp1.Data;

public sealed class CableArchiveEntity
{
    [PrimaryKey]
    public string MarkerId { get; set; } = string.Empty;

    public string Location { get; set; } = string.Empty;
    public double BurialDepthMeters { get; set; }
    public string PipeMaterial { get; set; } = string.Empty;
    public double DiameterMm { get; set; }
    public long? LaidOnUtcTicks { get; set; }
    public string OwnerUnit { get; set; } = string.Empty;

    /// <summary>加密存储的备注/敏感扩展信息。</summary>
    public string? EncryptedNotes { get; set; }
}
