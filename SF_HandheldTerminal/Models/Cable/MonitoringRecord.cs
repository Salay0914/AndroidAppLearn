namespace SF_HandheldTerminal.Models.Cable
{
    public sealed class MonitoringRecord
    {
        public string CableName { get; set; } = string.Empty;
        public string StatusText { get; set; } = string.Empty;
        public RiskLevel Risk { get; set; }
        public string Time { get; set; } = string.Empty;
    }
}
