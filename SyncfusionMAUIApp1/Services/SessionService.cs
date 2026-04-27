namespace SyncfusionMAUIApp1.Services;

public sealed class SessionService : ISessionService
{
    public string? CurrentUserName { get; private set; }
    public string? AuthToken { get; private set; }
    public bool IsAuthenticated => !string.IsNullOrEmpty(CurrentUserName);

    public event EventHandler? AuthChanged;

    public void Login(string userName, string? token = null)
    {
        CurrentUserName = userName;
        AuthToken = token;
        AuthChanged?.Invoke(this, EventArgs.Empty);
    }

    public void Logout()
    {
        CurrentUserName = null;
        AuthToken = null;
        AuthChanged?.Invoke(this, EventArgs.Empty);
    }
}
