namespace SF_HandheldTerminal.Models.Cable
{
    public sealed class AlarmRecord
    {
        public string CableName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public RiskLevel Risk { get; set; }
        public string RiskText { get; set; } = string.Empty;
    }
}
