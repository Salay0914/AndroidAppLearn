namespace SF_HandheldTerminal.Models.Cable
{
    /// <summary>
    /// 概览页"设备状态概览"卡片中的单格指标。
    /// </summary>
    public sealed class DeviceStatusItem
    {
        public string Name { get; set; } = string.Empty;
        public int Count { get; set; }
        public string IconGlyph { get; set; } = string.Empty;
        public Color IconBackground { get; set; } = Colors.SteelBlue;
        public Color IconForeground { get; set; } = Colors.White;
    }
}
