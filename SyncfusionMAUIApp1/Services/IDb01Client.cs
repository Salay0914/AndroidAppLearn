namespace SyncfusionMAUIApp1.Services;

public interface IDb01Client
{
    bool IsRunning { get; }
    event EventHandler<Db01Reading>? ReadingReceived;
    Task StartAsync(CancellationToken cancellationToken = default);
    Task StopAsync(CancellationToken cancellationToken = default);
}
