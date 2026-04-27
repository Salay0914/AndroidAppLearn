namespace SF_HandheldTerminal.Views.Settings
{
    public partial class Settings : ContentView
    {
        public Settings()
        {
            InitializeComponent();
            this.profileImage.Source = App.ImageServerPath + "ProfileImage1.png";
        }
    }
}