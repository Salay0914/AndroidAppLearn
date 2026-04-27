namespace SF_HandheldTerminal.Views.Dashboard
{
    public partial class HealthCare : ContentPage
    {
        public HealthCare()
        {
            InitializeComponent();
        }

        private void OnChipSelectionChanging(object sender, Syncfusion.Maui.Core.Chips.SelectionChangedEventArgs e)
        {
            // Access the ViewModel and call the function
            var viewModel = (HealthCareViewModel)BindingContext;
            viewModel.ChipSelectionChanged(e);
        }
    }
}