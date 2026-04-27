using SyncfusionMAUIApp1.Services;

namespace SyncfusionMAUIApp1;

public partial class CableMapPage : ContentPage
{
    private readonly IGeolocationAppService _geo = MauiProgram.Services.GetRequiredService<IGeolocationAppService>();

    public CableMapPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await UpdateGpsAsync().ConfigureAwait(true);
    }

    private async void OnRefreshGpsClicked(object? sender, EventArgs e) =>
        await UpdateGpsAsync().ConfigureAwait(true);

    private async Task UpdateGpsAsync()
    {
        var p = await _geo.GetCurrentLocationAsync().ConfigureAwait(true);
        if (p is null)
        {
            GpsLabel.Text = "无法获取定位（请检查权限与室外环境）。";
            return;
        }

        GpsLabel.Text = $"纬度 {p.Latitude:F6}，经度 {p.Longitude:F6}（精度约 {p.AccuracyMeters:F0} m）";
        DemoMarker.Latitude = p.Latitude;
        DemoMarker.Longitude = p.Longitude;
    }
}
