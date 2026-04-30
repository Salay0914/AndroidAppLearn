using System.Collections.ObjectModel;

namespace SF_HandheldTerminal.Models.Cable
{
    /// <summary>
    /// 设置页中的一个分组（如"设备管理"、"系统设置"）。
    /// </summary>
    public sealed class SettingsGroup : ObservableCollection<SettingsItem>
    {
        public string Title { get; set; } = string.Empty;

        public SettingsGroup() { }

        public SettingsGroup(string title, IEnumerable<SettingsItem> items)
            : base(items)
        {
            Title = title;
        }
    }

    /// <summary>
    /// 设置页中的单个条目。
    /// </summary>
    public sealed class SettingsItem
    {
        public string IconGlyph { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string ValueText { get; set; } = string.Empty;
        public bool ShowChevron { get; set; } = true;
    }
}
