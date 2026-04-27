namespace SF_HandheldTerminal.Services;

public sealed class LoginAuthResult
{
    public bool Succeeded { get; init; }
    public string? ErrorMessage { get; init; }

    public static LoginAuthResult Ok() => new() { Succeeded = true };

    public static LoginAuthResult Fail(string message) => new() { Succeeded = false, ErrorMessage = message };
}

public interface ILoginAuthService
{
    /// <summary>
    /// 校验账号密码。当前为演示实现，后续可替换为接口调用。
    /// </summary>
    Task<LoginAuthResult> AuthenticateAsync(string email, string password, CancellationToken cancellationToken = default);
}
