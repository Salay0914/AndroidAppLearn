namespace SyncfusionMAUIApp1.Services;

public sealed class NoOpBiometricAuthUi : IBiometricAuthUi
{
    public Task<bool> AuthenticateAsync(string title, string subtitle, CancellationToken cancellationToken = default) =>
        Task.FromResult(false);
}
