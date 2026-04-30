using System.Collections.ObjectModel;

namespace SF_HandheldTerminal.Models.Cable
{
    /// <summary>
    /// 电缆健康状态卡片：环形图占比 + 图例项。
    /// </summary>
    public sealed class HealthBreakdown
    {
        public int HealthScore { get; set; }
        public ObservableCollection<HealthBreakdownItem> Items { get; set; } = new();
    }

    public sealed class HealthBreakdownItem
    {
        public string Name { get; set; } = string.Empty;
        public int Count { get; set; }
        public double Percentage { get; set; }
        public string PercentageText { get; set; } = string.Empty;
        public Color Color { get; set; } = Colors.Gray;
    }
}
