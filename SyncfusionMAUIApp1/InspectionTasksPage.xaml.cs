using SyncfusionMAUIApp1.Contracts;
using SyncfusionMAUIApp1.Services;

namespace SyncfusionMAUIApp1;

public partial class InspectionTasksPage : ContentPage
{
    public InspectionTasksPage()
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
        var api = MauiProgram.Services.GetRequiredService<IStationApi>();
        var tasks = await api.GetTasksAsync().ConfigureAwait(true);
        TasksView.ItemsSource = tasks;
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
        if (e.CurrentSelection.FirstOrDefault() is not InspectionTaskDto dto)
            return;

        TasksView.SelectedItem = null;
        await Shell.Current.GoToAsync($"{nameof(TaskDetailPage)}?TaskId={Uri.EscapeDataString(dto.Id)}").ConfigureAwait(true);
    }
}
