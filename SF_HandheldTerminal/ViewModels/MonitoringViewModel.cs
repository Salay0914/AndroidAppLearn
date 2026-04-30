using SF_HandheldTerminal.Models.Cable;
using SF_HandheldTerminal.ViewModel;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace SF_HandheldTerminal
{
    /// <summary>
    /// 监测页 ViewModel：当前仅提供静态占位数据，待数据层就绪后注入服务。
    /// </summary>
    public sealed class MonitoringViewModel : BaseViewModel
    {
        private string searchText = string.Empty;
        private int selectedTabIndex;
        private TrendMetric? selectedTrendMetric;

        public CableDevice CurrentCable { get; }
        public ObservableCollection<SensorReading> SensorReadings { get; }
        public ObservableCollection<TrendMetric> TrendMetrics { get; }
        public ObservableCollection<MonitoringAlarmRecord> AlarmRecords { get; }
        public ObservableCollection<DeviceInfoSection> DeviceInfoSections { get; }

        public ICommand SelectTrendMetricCommand { get; }

        public string SearchText
        {
            get => searchText;
            set => SetProperty(ref searchText, value, nameof(SearchText));
        }

        public int SelectedTabIndex
        {
            get => selectedTabIndex;
            set => SetProperty(ref selectedTabIndex, value, nameof(SelectedTabIndex));
        }

        public TrendMetric? SelectedTrendMetric
        {
            get => selectedTrendMetric;
            set => SetProperty(ref selectedTrendMetric, value, nameof(SelectedTrendMetric));
        }

        public MonitoringViewModel()
        {
            CurrentCable = new CableDevice
            {
                Id = "A-3",
                Name = "电缆A-3",
                Location = "位置：K23+450 信号机房",
                MonitoringTimeText = "监测时间：2024-05-20 09:41:00",
                IsOnline = true,
                Risk = RiskLevel.High,
                RiskText = "高风险",
            };

            SensorReadings = new ObservableCollection<SensorReading>
            {
                new()
                {
                    Name = "绝缘电阻",
                    Value = "0.45",
                    Unit = "MΩ",
                    Threshold = "阈值：≥1.00 MΩ",
                    Status = RiskLevel.High,
                    StatusText = "异常",
                    IconGlyph = "Ω",
                    IconBackground = Color.FromArgb("#3F73C9"),
                },
                new()
                {
                    Name = "电缆温度",
                    Value = "48.6",
                    Unit = "℃",
                    Threshold = "阈值：≤60.0 ℃",
                    Status = RiskLevel.Normal,
                    StatusText = "正常",
                    IconGlyph = "\ue702",
                    IconBackground = Color.FromArgb("#F5A623"),
                },
                new()
                {
                    Name = "电缆电压",
                    Value = "220.5",
                    Unit = "V",
                    Threshold = "阈值：≤250.0 V",
                    Status = RiskLevel.Normal,
                    StatusText = "正常",
                    IconGlyph = "\ue74d",
                    IconBackground = Color.FromArgb("#F0B400"),
                },
                new()
                {
                    Name = "泄漏电流",
                    Value = "2.3",
                    Unit = "mA",
                    Threshold = "阈值：≤5.0 mA",
                    Status = RiskLevel.Normal,
                    StatusText = "正常",
                    IconGlyph = "\ue70c",
                    IconBackground = Color.FromArgb("#A569E5"),
                },
                new()
                {
                    Name = "护层电流",
                    Value = "12.1",
                    Unit = "mA",
                    Threshold = "阈值：≤30.0 mA",
                    Status = RiskLevel.Normal,
                    StatusText = "正常",
                    IconGlyph = "\ue734",
                    IconBackground = Color.FromArgb("#2BBF6E"),
                },
            };

            TrendMetrics = BuildTrendMetrics();
            SelectedTrendMetric = TrendMetrics.FirstOrDefault();
            if (SelectedTrendMetric is not null)
                SelectedTrendMetric.IsSelected = true;

            SelectTrendMetricCommand = new Command<TrendMetric>(OnSelectTrendMetric);

            AlarmRecords = BuildAlarmRecords();
            DeviceInfoSections = BuildDeviceInfoSections();
        }

        private void OnSelectTrendMetric(TrendMetric? metric)
        {
            if (metric is null || ReferenceEquals(metric, SelectedTrendMetric))
                return;

            foreach (var item in TrendMetrics)
                item.IsSelected = ReferenceEquals(item, metric);

            SelectedTrendMetric = metric;
        }

        private static ObservableCollection<TrendMetric> BuildTrendMetrics()
        {
            string[] hours = { "08:00", "08:30", "09:00", "09:30", "10:00", "10:30", "11:00", "11:30" };

            ObservableCollection<TrendPoint> Points(params double[] values)
            {
                var collection = new ObservableCollection<TrendPoint>();
                for (int i = 0; i < values.Length && i < hours.Length; i++)
                    collection.Add(new TrendPoint { TimeText = hours[i], Value = values[i] });
                return collection;
            }

            var metrics = new ObservableCollection<TrendMetric>
            {
                new()
                {
                    Name = "绝缘电阻",
                    Unit = "MΩ",
                    Color = Color.FromArgb("#E74C3C"),
                    LatestValueText = "当前 0.45 MΩ",
                    Points = Points(1.20, 1.10, 0.95, 0.82, 0.70, 0.58, 0.50, 0.45),
                },
                new()
                {
                    Name = "电缆温度",
                    Unit = "℃",
                    Color = Color.FromArgb("#F5A623"),
                    LatestValueText = "当前 48.6 ℃",
                    Points = Points(35.2, 36.5, 38.1, 40.0, 42.4, 45.0, 47.2, 48.6),
                },
                new()
                {
                    Name = "电缆电压",
                    Unit = "V",
                    Color = Color.FromArgb("#3F73C9"),
                    LatestValueText = "当前 220.5 V",
                    Points = Points(218.0, 219.4, 220.1, 220.8, 221.2, 220.6, 220.0, 220.5),
                },
                new()
                {
                    Name = "泄漏电流",
                    Unit = "mA",
                    Color = Color.FromArgb("#A569E5"),
                    LatestValueText = "当前 2.3 mA",
                    Points = Points(1.4, 1.5, 1.7, 1.8, 2.0, 2.1, 2.2, 2.3),
                },
            };

            // 三位精度对电压、整数百分比对电流；这里统一用两位小数，足够区分。
            foreach (var metric in metrics)
                metric.RecomputeStats();

            return metrics;
        }

        private static ObservableCollection<MonitoringAlarmRecord> BuildAlarmRecords()
        {
            return new ObservableCollection<MonitoringAlarmRecord>
            {
                new()
                {
                    Time = "2024-05-20 09:41",
                    Title = "绝缘电阻低于阈值",
                    Description = "实测低于安全阈值，建议立即排查电缆受潮或绝缘老化情况。",
                    MeasuredText = "实测 0.45 MΩ / 阈值 ≥1.00 MΩ",
                    Risk = RiskLevel.High,
                    RiskText = "高风险",
                    HandleStatus = "未处理",
                    IsHandled = false,
                },
                new()
                {
                    Time = "2024-05-20 09:18",
                    Title = "温度持续升高",
                    Description = "近 30 分钟温升 6.4 ℃，需关注负载变化与散热条件。",
                    MeasuredText = "实测 48.6 ℃ / 阈值 ≤60.0 ℃",
                    Risk = RiskLevel.Medium,
                    RiskText = "中风险",
                    HandleStatus = "处理中",
                    IsHandled = false,
                },
                new()
                {
                    Time = "2024-05-20 08:52",
                    Title = "泄漏电流超过预警值",
                    Description = "瞬时尖峰，已自动复位，建议下一轮巡检关注。",
                    MeasuredText = "实测 4.8 mA / 阈值 ≤5.0 mA",
                    Risk = RiskLevel.Light,
                    RiskText = "轻度风险",
                    HandleStatus = "已确认",
                    IsHandled = true,
                },
                new()
                {
                    Time = "2024-05-19 22:14",
                    Title = "通信中断 2 分钟",
                    Description = "蓝牙链路重连后已恢复，无数据丢失。",
                    MeasuredText = "蓝牙信号短暂丢失",
                    Risk = RiskLevel.Offline,
                    RiskText = "已恢复",
                    HandleStatus = "已处理",
                    IsHandled = true,
                },
            };
        }

        private static ObservableCollection<DeviceInfoSection> BuildDeviceInfoSections()
        {
            return new ObservableCollection<DeviceInfoSection>
            {
                new()
                {
                    Title = "基础信息",
                    IconGlyph = "\ue706",
                    IconBackground = Color.FromArgb("#3F73C9"),
                    Entries = new ObservableCollection<DeviceInfoEntry>
                    {
                        new() { Label = "设备型号",  Value = "DB01-Pro" },
                        new() { Label = "设备编号",  Value = "SN-20240518-A03" },
                        new() { Label = "出厂日期",  Value = "2024-04-12" },
                        new() { Label = "固件版本",  Value = "v1.4.2 (2024-05-10)" },
                    },
                },
                new()
                {
                    Title = "连接状态",
                    IconGlyph = "\ue74a",
                    IconBackground = Color.FromArgb("#2BBF6E"),
                    Entries = new ObservableCollection<DeviceInfoEntry>
                    {
                        new() { Label = "蓝牙",      Value = "已连接", ShowStatusDot = true, StatusDotColor = Color.FromArgb("#2BBF6E") },
                        new() { Label = "网络",      Value = "Wi-Fi 已接入", ShowStatusDot = true, StatusDotColor = Color.FromArgb("#2BBF6E") },
                        new() { Label = "信号强度",  Value = "强 (-58 dBm)" },
                        new() { Label = "最后心跳",  Value = "2024-05-20 09:41:00" },
                    },
                },
                new()
                {
                    Title = "电池状态",
                    IconGlyph = "\ue743",
                    IconBackground = Color.FromArgb("#F5A623"),
                    Entries = new ObservableCollection<DeviceInfoEntry>
                    {
                        new() { Label = "电池电量",  Value = "82%" },
                        new() { Label = "充电状态",  Value = "未充电" },
                        new() { Label = "预计续航",  Value = "约 11 小时" },
                    },
                },
                new()
                {
                    Title = "安装位置",
                    IconGlyph = "\ue729",
                    IconBackground = Color.FromArgb("#A569E5"),
                    Entries = new ObservableCollection<DeviceInfoEntry>
                    {
                        new() { Label = "线路位置",  Value = "K23+450 信号机房" },
                        new() { Label = "经度",      Value = "116.3974° E" },
                        new() { Label = "纬度",      Value = "39.9087° N" },
                        new() { Label = "海拔",      Value = "48.6 m" },
                    },
                },
            };
        }
    }
}
