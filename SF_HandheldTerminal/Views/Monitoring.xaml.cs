using Syncfusion.Maui.Toolkit.Buttons;

namespace SF_HandheldTerminal.Views.Catalog
{
    public partial class Monitoring : ContentPage
    {
        public Monitoring()
        {
            InitializeComponent();
        }

        private void FavouriteButton_Clicked(object sender, EventArgs e)
        {
            SfButton? button = sender as SfButton;
            var travel = button?.BindingContext as SF_HandheldTerminal.Models.Catalog.Travel;
            if (travel != null) {
                travel.IsFavourite = !travel.IsFavourite;
            }
        }
    }
}