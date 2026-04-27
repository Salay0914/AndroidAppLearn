namespace SyncfusionMAUIApp1.Services;

/// <summary>
/// 无硬件时模拟 DB01 推送：周期性随机信号/深度/标识器 ID。
/// </summary>
public sealed class MockDb01Client : IDb01Client
{
    private readonly Random _random = new();
    private PeriodicTimer? _timer;
    private CancellationTokenSource? _cts;
    private Task? _loop;

    public bool IsRunning => _timer is not null;

    public event EventHandler<Db01Reading>? ReadingReceived;

    public Task StartAsync(CancellationToken cancellationToken = default)
    {
        StopInternal();
        _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        _timer = new PeriodicTimer(TimeSpan.FromMilliseconds(600));
        var token = _cts.Token;
        _loop = Task.Run(async () =>
        {
            try
            {
                while (await _timer.WaitForNextTickAsync(token).ConfigureAwait(false))
                {
                    var marker = $"LY801-{_random.Next(1, 4):0000}";
                    var rssi = _random.NextDouble() * 100d;
                    var depth = 0.8 + _random.NextDouble() * 0.8;
                    var battery = _random.Next(40, 100);
                    ReadingReceived?.Invoke(this, new Db01Reading
                    {
                        MarkerId = marker,
                        SignalStrength = rssi,
                        DepthMeters = depth,
                        DeviceBatteryPercent = battery,
                        TimestampUtc = DateTime.UtcNow,
                    });
                }
            }
            catch (OperationCanceledException)
            {
                // ignore
            }
        }, token);

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken = default)
    {
        StopInternal();
        return Task.CompletedTask;
    }

    private void StopInternal()
    {
        try
        {
            _cts?.Cancel();
        }
        catch
        {
            // ignore
        }

        _timer?.Dispose();
        _timer = null;
        _cts?.Dispose();
        _cts = null;
        _loop = null;
    }
}
