namespace SyncfusionMAUIApp1.Services;

public interface ISecureCredentialStore
{
    Task SaveTokenAsync(string token, CancellationToken cancellationToken = default);
    Task<string?> GetTokenAsync(CancellationToken cancellationToken = default);
    Task ClearAsync(CancellationToken cancellationToken = default);
}
