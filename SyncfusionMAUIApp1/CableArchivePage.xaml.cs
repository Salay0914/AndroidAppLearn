using SyncfusionMAUIApp1.Contracts;
using SyncfusionMAUIApp1.Services;

namespace SyncfusionMAUIApp1;

public partial class CableArchivePage : ContentPage
{
    private readonly ILocalDatabase _db = MauiProgram.Services.GetRequiredService<ILocalDatabase>();

    public CableArchivePage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadAsync().ConfigureAwait(true);
    }

    private async Task LoadAsync()
    {
        await _db.EnsureInitializedAsync().ConfigureAwait(true);
        var rows = await _db.GetArchiveDtosAsync().ConfigureAwait(true);
        ArchivesView.ItemsSource = rows;
    }

    private async void OnRefreshing(object? sender, EventArgs e)
    {
        try
        {
            await LoadAsync().ConfigureAwait(true);
        }
        finally
        {
            Refresh.IsRefreshing = false;
        }
    }

    private async void OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is not CableArchiveDto dto)
            return;

        ArchivesView.SelectedItem = null;
        await Shell.Current.GoToAsync($"{nameof(CableArchiveDetailPage)}?MarkerId={Uri.EscapeDataString(dto.MarkerId)}").ConfigureAwait(true);
    }
}
