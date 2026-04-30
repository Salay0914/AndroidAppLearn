using System.Collections.ObjectModel;
using System.ComponentModel;

namespace SF_HandheldTerminal.Models.Cable
{
    /// <summary>
    /// 趋势图中的单个指标维度（绝缘电阻 / 温度 / 电压 / 泄漏电流 ...）。
    /// </summary>
    public sealed class TrendMetric : INotifyPropertyChanged
    {
        private bool isSelected;

        public string Name { get; set; } = string.Empty;
        public string Unit { get; set; } = string.Empty;
        public Color Color { get; set; } = Colors.SteelBlue;
        public string LatestValueText { get; set; } = string.Empty;

        /// <summary>
        /// 当前指标在所选时间窗口内的最小值文本。
        /// </summary>
        public string MinText { get; set; } = string.Empty;

        /// <summary>
        /// 当前指标在所选时间窗口内的平均值文本。
        /// </summary>
        public string AvgText { get; set; } = string.Empty;

        /// <summary>
        /// 当前指标在所选时间窗口内的最大值文本。
        /// </summary>
        public string MaxText { get; set; } = string.Empty;

        public ObservableCollection<TrendPoint> Points { get; set; } = new();

        public bool IsSelected
        {
            get => isSelected;
            set
            {
                if (isSelected == value)
                    return;
                isSelected = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsSelected)));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// 根据当前 Points 重新计算 Min / Avg / Max 三个统计字段。
        /// </summary>
        public void RecomputeStats(string format = "F2")
        {
            if (Points.Count == 0) {
                MinText = AvgText = MaxText = "-";
                return;
            }

            double min = Points.Min(p => p.Value);
            double max = Points.Max(p => p.Value);
            double avg = Points.Average(p => p.Value);

            MinText = $"{min.ToString(format, System.Globalization.CultureInfo.InvariantCulture)} {Unit}".Trim();
            AvgText = $"{avg.ToString(format, System.Globalization.CultureInfo.InvariantCulture)} {Unit}".Trim();
            MaxText = $"{max.ToString(format, System.Globalization.CultureInfo.InvariantCulture)} {Unit}".Trim();
        }
    }

    /// <summary>
    /// 趋势图上的一个数据点。
    /// </summary>
    public sealed class TrendPoint
    {
        public string TimeText { get; set; } = string.Empty;
        public double Value { get; set; }
    }
}
