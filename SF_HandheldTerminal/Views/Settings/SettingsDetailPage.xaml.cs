namespace SF_HandheldTerminal.Views.Settings
{
    public partial class SettingsDetailPage : ContentPage
    {
        public SettingsDetailPage(string title)
        {
            InitializeComponent();
            DetailTitleLabel.Text = title;
            Title = title;
        }

        public SettingsDetailPage() : this("设置项")
        {
        }
    }
}
