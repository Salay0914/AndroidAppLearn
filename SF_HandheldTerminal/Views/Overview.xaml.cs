namespace SF_HandheldTerminal.Views.Dashboard
{
    public partial class Overview : ContentPage
    {
        public Overview()
        {
            InitializeComponent();
        }

        private void OnChipSelectionChanging(object sender, Syncfusion.Maui.Core.Chips.SelectionChangedEventArgs e)
        {
            // Access the ViewModel and call the function
            var viewModel = (OverviewViewModel)BindingContext;
            viewModel.ChipSelectionChanged(e);
        }
    }
}