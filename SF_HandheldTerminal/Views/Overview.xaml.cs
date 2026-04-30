using SF_HandheldTerminal.Services;
using SF_HandheldTerminal.Views.Cable;

namespace SF_HandheldTerminal.Views.Dashboard
{
    public partial class Overview : ContentPage
    {
        public Overview()
        {
            InitializeComponent();
        }

        private async void OnAlarmsViewAllTapped(object sender, TappedEventArgs e)
        {
            await AppNavigation.PushAsync(new AlarmListPage());
        }

        private async void OnRecordsViewAllTapped(object sender, TappedEventArgs e)
        {
            await AppNavigation.PushAsync(new MonitorHistoryPage());
        }
    }
}
