using System.Collections.ObjectModel;

namespace SF_HandheldTerminal.Models.Cable
{
    public sealed class DeviceStatusSummary
    {
        public string UpdatedAtText { get; set; } = string.Empty;
        public ObservableCollection<DeviceStatusItem> Items { get; set; } = new();
    }
}
