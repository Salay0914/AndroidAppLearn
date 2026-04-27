using System.Collections.ObjectModel;
using SyncfusionMAUIApp1.Services;

namespace SyncfusionMAUIApp1;

public partial class ScanSignalPage : ContentPage
{
    private readonly IDb01Client _db01 = MauiProgram.Services.GetRequiredService<IDb01Client>();
    private readonly ISignalFeedbackService _feedback = MauiProgram.Services.GetRequiredService<ISignalFeedbackService>();

    private int _index;
    public ObservableCollection<SignalPlotPoint> Points { get; } = new();

    public ScanSignalPage()
    {
        InitializeComponent();
        BindingContext = this;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _db01.ReadingReceived += OnReading;
    }

    protected override void OnDisappearing()
    {
        _db01.ReadingReceived -= OnReading;
        base.OnDisappearing();
    }

    private void OnReading(object? sender, Db01Reading e)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            ReadingLabel.Text = $"ID: {e.MarkerId}  强度: {e.SignalStrength:F1}  深度: {e.DepthMeters:F2} m  电量: {e.DeviceBatteryPercent}%";
            StrengthBar.Progress = Math.Clamp(e.SignalStrength / 100d, 0, 1);
            _feedback.PlaySignalPulse(StrengthBar.Progress);

            _index++;
            Points.Add(new SignalPlotPoint { Index = _index, Rssi = e.SignalStrength });
            while (Points.Count > 80)
                Points.RemoveAt(0);
        });
    }

    private async void OnStartClicked(object? sender, EventArgs e)
    {
        await _db01.StartAsync().ConfigureAwait(true);
        StartBtn.IsEnabled = false;
        StopBtn.IsEnabled = true;
    }

    private async void OnStopClicked(object? sender, EventArgs e)
    {
        await _db01.StopAsync().ConfigureAwait(true);
        StartBtn.IsEnabled = true;
        StopBtn.IsEnabled = false;
    }

    public sealed class SignalPlotPoint
    {
        public int Index { get; set; }
        public double Rssi { get; set; }
    }
}
