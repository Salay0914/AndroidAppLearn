namespace SF_HandheldTerminal.Models.Cable
{
    /// <summary>
    /// 电缆设备的轻量占位模型，仅用于 UI 展示。
    /// </summary>
    public sealed class CableDevice
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string MonitoringTimeText { get; set; } = string.Empty;
        public bool IsOnline { get; set; }
        public RiskLevel Risk { get; set; }
        public string RiskText { get; set; } = string.Empty;
    }
}
