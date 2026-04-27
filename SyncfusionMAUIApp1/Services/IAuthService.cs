namespace SyncfusionMAUIApp1.Services;

public interface IAuthService
{
    Task<AuthResult> SignInAsync(string userName, string password, CancellationToken cancellationToken = default);
    Task<AuthResult> SignInWithBiometricAsync(CancellationToken cancellationToken = default);
}

public sealed record AuthResult(bool Success, string? Message);
