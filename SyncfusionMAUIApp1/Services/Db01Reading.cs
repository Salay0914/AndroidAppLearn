namespace SyncfusionMAUIApp1.Services;

public sealed class Db01Reading
{
    public string MarkerId { get; set; } = string.Empty;
    public double SignalStrength { get; set; }
    public double DepthMeters { get; set; }
    public int DeviceBatteryPercent { get; set; }
    public DateTime TimestampUtc { get; set; } = DateTime.UtcNow;
}
