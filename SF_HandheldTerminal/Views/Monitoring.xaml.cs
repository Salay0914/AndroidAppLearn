using SF_HandheldTerminal.Services;
using SF_HandheldTerminal.Views.Cable;

namespace SF_HandheldTerminal.Views.Catalog
{
    public partial class Monitoring : ContentPage
    {
        public Monitoring()
        {
            InitializeComponent();
        }

        private async void OnScanTapped(object sender, TappedEventArgs e)
        {
            await AppNavigation.PushAsync(new ScannerPage());
        }

        private async void OnCurrentCableTapped(object sender, TappedEventArgs e)
        {
            await AppNavigation.PushAsync(new CableDetailPage());
        }

        private async void OnStartMonitoringClicked(object sender, EventArgs e)
        {
            // 占位：等待业务接入
            var window = Application.Current?.Windows.FirstOrDefault();
            if (window?.Page is Page page)
                await page.DisplayAlert("提示", "开始新一轮监测（占位）", "确定");
        }
    }
}
