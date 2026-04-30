namespace SF_HandheldTerminal.Models.Cable
{
    /// <summary>
    /// 监测页"告警记录"Tab 的列表项：含时间、传感器、实测值与处理状态。
    /// </summary>
    public sealed class MonitoringAlarmRecord
    {
        public string Time { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string MeasuredText { get; set; } = string.Empty;
        public RiskLevel Risk { get; set; }
        public string RiskText { get; set; } = string.Empty;
        public string HandleStatus { get; set; } = string.Empty;
        public bool IsHandled { get; set; }
    }
}
