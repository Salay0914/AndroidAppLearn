namespace SyncfusionMAUIApp1.Services;

public interface ICryptoService
{
    Task<string> EncryptAsync(string plainText, CancellationToken cancellationToken = default);
    Task<string> DecryptAsync(string cipherTextBase64, CancellationToken cancellationToken = default);
}
