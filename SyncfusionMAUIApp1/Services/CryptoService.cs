using System.Security.Cryptography;
using System.Text;

namespace SyncfusionMAUIApp1.Services;

/// <summary>
/// 应用层对称加密示例；生产环境应结合安全评审与密钥托管（Keystore 等）。
/// </summary>
public sealed class CryptoService : ICryptoService
{
    private const string KeyStorage = "cable_crypto_key_v1";
    private readonly SemaphoreSlim _lock = new(1, 1);

    public async Task<string> EncryptAsync(string plainText, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(plainText))
            return plainText;

        await _lock.WaitAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            var key = await EnsureKeyMaterialAsync(cancellationToken).ConfigureAwait(false);
            using var aes = Aes.Create();
            aes.Key = key;
            aes.GenerateIV();
            using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            var plainBytes = Encoding.UTF8.GetBytes(plainText);
            await using var ms = new MemoryStream();
            await ms.WriteAsync(aes.IV, cancellationToken).ConfigureAwait(false);
            await using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
            {
                await cs.WriteAsync(plainBytes, cancellationToken).ConfigureAwait(false);
            }

            return Convert.ToBase64String(ms.ToArray());
        }
        finally
        {
            _lock.Release();
        }
    }

    public async Task<string> DecryptAsync(string cipherTextBase64, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(cipherTextBase64))
            return cipherTextBase64;

        await _lock.WaitAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            var key = await EnsureKeyMaterialAsync(cancellationToken).ConfigureAwait(false);
            var buffer = Convert.FromBase64String(cipherTextBase64);
            using var aes = Aes.Create();
            aes.Key = key;
            var iv = new byte[aes.BlockSize / 8];
            Array.Copy(buffer, 0, iv, 0, iv.Length);
            using var decryptor = aes.CreateDecryptor(aes.Key, iv);
            await using var ms = new MemoryStream(buffer, iv.Length, buffer.Length - iv.Length);
            await using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
            using var sr = new StreamReader(cs, Encoding.UTF8);
            return await sr.ReadToEndAsync(cancellationToken).ConfigureAwait(false);
        }
        finally
        {
            _lock.Release();
        }
    }

    private static async Task<byte[]> EnsureKeyMaterialAsync(CancellationToken cancellationToken)
    {
        var existing = await SecureStorage.Default.GetAsync(KeyStorage).ConfigureAwait(false);
        if (!string.IsNullOrEmpty(existing))
            return Convert.FromBase64String(existing);

        using var aes = Aes.Create();
        aes.KeySize = 256;
        aes.GenerateKey();
        var keyB64 = Convert.ToBase64String(aes.Key);
        await SecureStorage.Default.SetAsync(KeyStorage, keyB64).ConfigureAwait(false);
        return aes.Key;
    }
}
