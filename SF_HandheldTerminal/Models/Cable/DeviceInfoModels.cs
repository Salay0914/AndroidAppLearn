using System.Collections.ObjectModel;

namespace SF_HandheldTerminal.Models.Cable
{
    /// <summary>
    /// 设备信息分组卡片（基础信息 / 连接状态 / 电池状态 / 安装位置 ...）。
    /// </summary>
    public sealed class DeviceInfoSection
    {
        public string Title { get; set; } = string.Empty;
        public string IconGlyph { get; set; } = string.Empty;
        public Color IconBackground { get; set; } = Colors.SteelBlue;
        public ObservableCollection<DeviceInfoEntry> Entries { get; set; } = new();
    }

    /// <summary>
    /// 设备信息分组中的一行：左标签、右值，可选状态点。
    /// </summary>
    public sealed class DeviceInfoEntry
    {
        public string Label { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public bool ShowStatusDot { get; set; }
        public Color StatusDotColor { get; set; } = Colors.Transparent;
    }
}
