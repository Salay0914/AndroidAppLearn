using SF_HandheldTerminal.Models.Cable;
using SF_HandheldTerminal.ViewModel;
using System.Collections.ObjectModel;

namespace SF_HandheldTerminal
{
    /// <summary>
    /// 概览页 ViewModel：当前仅提供静态占位数据，待数据层就绪后注入服务。
    /// </summary>
    public sealed class OverviewViewModel : BaseViewModel
    {
        public string HeroTitle { get; }
        public string HeroSubtitle { get; }
        public string HeroGreeting { get; }

        public DeviceStatusSummary StatusSummary { get; }
        public HealthBreakdown HealthBreakdown { get; }
        public ObservableCollection<AlarmRecord> LatestAlarms { get; }
        public ObservableCollection<MonitoringRecord> RecentRecords { get; }

        public OverviewViewModel()
        {
            HeroTitle = "铁路电缆监测手持终端";
            HeroGreeting = "您好，运维人员";
            HeroSubtitle = "实时巡检 · 智能告警";

            StatusSummary = new DeviceStatusSummary
            {
                UpdatedAtText = "更新时间：2024-05-20 09:41:00",
                Items = new ObservableCollection<DeviceStatusItem>
                {
                    new() { Name = "设备总数", Count = 12, IconGlyph = "\ue706", IconBackground = Color.FromArgb("#3F73C9") },
                    new() { Name = "正常设备", Count = 9,  IconGlyph = "\ue741", IconBackground = Color.FromArgb("#2BBF6E") },
                    new() { Name = "告警设备", Count = 2,  IconGlyph = "\ue707", IconBackground = Color.FromArgb("#F5A623") },
                    new() { Name = "离线设备", Count = 1,  IconGlyph = "\ue724", IconBackground = Color.FromArgb("#9AA4B0") },
                },
            };

            HealthBreakdown = new HealthBreakdown
            {
                HealthScore = 83,
                Items = new ObservableCollection<HealthBreakdownItem>
                {
                    new() { Name = "健康",     Count = 10, Percentage = 83, PercentageText = "10 (83%)", Color = Color.FromArgb("#2BBF6E") },
                    new() { Name = "轻度风险", Count = 1,  Percentage = 8,  PercentageText = "1 (8%)",   Color = Color.FromArgb("#F5A623") },
                    new() { Name = "高风险",   Count = 1,  Percentage = 9,  PercentageText = "1 (9%)",   Color = Color.FromArgb("#E74C3C") },
                },
            };

            LatestAlarms = new ObservableCollection<AlarmRecord>
            {
                new() { CableName = "电缆A-3", Description = "绝缘电阻低于阈值", Risk = RiskLevel.High,   RiskText = "高风险" },
                new() { CableName = "电缆B-1", Description = "温度异常升高",     Risk = RiskLevel.Medium, RiskText = "中风险" },
            };

            RecentRecords = new ObservableCollection<MonitoringRecord>
            {
                new() { CableName = "电缆A-1", StatusText = "正常",   Risk = RiskLevel.Normal, Time = "2024-05-20 09:40" },
                new() { CableName = "电缆B-2", StatusText = "正常",   Risk = RiskLevel.Normal, Time = "2024-05-20 09:38" },
                new() { CableName = "电缆C-1", StatusText = "中风险", Risk = RiskLevel.Medium, Time = "2024-05-20 09:30" },
            };
        }
    }
}
