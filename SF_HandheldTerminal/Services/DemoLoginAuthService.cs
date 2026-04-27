namespace SF_HandheldTerminal.Services;

/// <summary>
/// 演示用登录：固定账号或满足基本规则的任意账号均可通过，便于联调主界面。
/// </summary>
public sealed class DemoLoginAuthService : ILoginAuthService
{
    private const string DemoEmail = "admin@local";
    private const string DemoPassword = "admin123";

    public Task<LoginAuthResult> AuthenticateAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        email = email.Trim();
        if (string.IsNullOrEmpty(email))
            return Task.FromResult(LoginAuthResult.Fail("请输入邮箱。"));

        if (string.IsNullOrEmpty(password))
            return Task.FromResult(LoginAuthResult.Fail("请输入密码。"));

        if (string.Equals(email, DemoEmail, StringComparison.OrdinalIgnoreCase)
            && password == DemoPassword)
            return Task.FromResult(LoginAuthResult.Ok());

        if (!email.Contains('@', StringComparison.Ordinal))
            return Task.FromResult(LoginAuthResult.Fail("邮箱格式不正确。"));

        if (password.Length < 6)
            return Task.FromResult(LoginAuthResult.Fail("密码至少 6 位。"));

        return Task.FromResult(LoginAuthResult.Ok());
    }
}
