namespace SF_HandheldTerminal.Models.Cable
{
    /// <summary>
    /// 实时数据列表中的单条传感器读数。
    /// </summary>
    public sealed class SensorReading
    {
        public string Name { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public string Unit { get; set; } = string.Empty;
        public string Threshold { get; set; } = string.Empty;
        public RiskLevel Status { get; set; }
        public string StatusText { get; set; } = string.Empty;
        public string IconGlyph { get; set; } = string.Empty;
        public Color IconBackground { get; set; } = Colors.SteelBlue;
    }
}
