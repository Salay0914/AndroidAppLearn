using SF_HandheldTerminal.Models.Cable;
using SF_HandheldTerminal.Services;

namespace SF_HandheldTerminal.Views.Settings
{
    public partial class Settings : ContentView
    {
        public Settings()
        {
            InitializeComponent();
            BindingContext = new SettingsViewModel();
        }

        private async void OnSettingsItemTapped(object sender, TappedEventArgs e)
        {
            if (sender is BindableObject bo && bo.BindingContext is SettingsItem item) {
                await AppNavigation.PushAsync(new SettingsDetailPage(item.Title));
            }
        }
    }
}
