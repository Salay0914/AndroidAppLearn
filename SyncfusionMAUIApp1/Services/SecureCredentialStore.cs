namespace SyncfusionMAUIApp1.Services;

public sealed class SecureCredentialStore : ISecureCredentialStore
{
    private const string TokenKey = "station_auth_token";

    public Task SaveTokenAsync(string token, CancellationToken cancellationToken = default) =>
        SecureStorage.Default.SetAsync(TokenKey, token);

    public Task<string?> GetTokenAsync(CancellationToken cancellationToken = default) =>
        SecureStorage.Default.GetAsync(TokenKey);

    public async Task ClearAsync(CancellationToken cancellationToken = default) =>
        SecureStorage.Default.Remove(TokenKey);
}
