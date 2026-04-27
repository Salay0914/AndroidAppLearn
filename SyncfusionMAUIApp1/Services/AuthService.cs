namespace SyncfusionMAUIApp1.Services;

public sealed class AuthService : IAuthService
{
    private readonly ISessionService _session;
    private readonly ISecureCredentialStore _credentials;
    private readonly IBiometricAuthUi _biometric;

    public AuthService(ISessionService session, ISecureCredentialStore credentials, IBiometricAuthUi biometric)
    {
        _session = session;
        _credentials = credentials;
        _biometric = biometric;
    }

    public async Task<AuthResult> SignInAsync(string userName, string password, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password))
            return new AuthResult(false, "请输入用户名和密码。");

        // 演示：任意非空密码视为成功；接入主机后改为调用 IStationApi 鉴权。
        var token = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"{userName}:{DateTime.UtcNow.Ticks}"));
        await _credentials.SaveTokenAsync(token, cancellationToken).ConfigureAwait(false);
        _session.Login(userName.Trim(), token);
        return new AuthResult(true, null);
    }

    public async Task<AuthResult> SignInWithBiometricAsync(CancellationToken cancellationToken = default)
    {
        var ok = await _biometric.AuthenticateAsync("电缆智能巡检", "请验证指纹或人脸", cancellationToken).ConfigureAwait(false);
        if (!ok)
            return new AuthResult(false, "生物识别未通过或不可用。");

        var token = await _credentials.GetTokenAsync(cancellationToken).ConfigureAwait(false);
        if (string.IsNullOrEmpty(token))
            return new AuthResult(false, "请先使用密码登录一次以保存会话。");

        _session.Login("biometric_user", token);
        return new AuthResult(true, null);
    }
}
