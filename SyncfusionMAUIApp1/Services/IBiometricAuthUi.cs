namespace SyncfusionMAUIApp1.Services;

public interface IBiometricAuthUi
{
    Task<bool> AuthenticateAsync(string title, string subtitle, CancellationToken cancellationToken = default);
}
