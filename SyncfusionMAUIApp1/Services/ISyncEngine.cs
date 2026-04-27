namespace SyncfusionMAUIApp1.Services;

public interface ISyncEngine
{
    Task<int> ProcessOutboxAsync(CancellationToken cancellationToken = default);
}
