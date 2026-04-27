namespace SyncfusionMAUIApp1.Services;

public interface ISessionService
{
    string? CurrentUserName { get; }
    string? AuthToken { get; }
    bool IsAuthenticated { get; }
    void Login(string userName, string? token = null);
    void Logout();
    event EventHandler? AuthChanged;
}
