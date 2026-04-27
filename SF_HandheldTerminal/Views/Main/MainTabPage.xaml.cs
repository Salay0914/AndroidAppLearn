namespace SF_HandheldTerminal.Views.Main;

public partial class MainTabPage : ContentPage
{
    public MainTabPage()
    {
        InitializeComponent();
        SettingsTabItem.Content = new SF_HandheldTerminal.Views.Settings.Settings();
    }
}
