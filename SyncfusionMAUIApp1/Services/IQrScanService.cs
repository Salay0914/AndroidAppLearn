namespace SyncfusionMAUIApp1.Services;

public interface IQrScanService
{
    Task<string?> ScanAsync(Page hostPage, CancellationToken cancellationToken = default);
}
