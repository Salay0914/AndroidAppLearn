using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using Syncfusion.Maui.DataForm;

namespace SF_HandheldTerminal;

public class LoginPageViewModel : INotifyPropertyChanged
{
    private const string PrefRemember = "login_remember";
    private const string PrefSavedEmail = "login_saved_email";

    private bool _rememberMe;

    public LoginPageViewModel()
    {
        ContactsInfo = new ContactsInfo();
        if (Preferences.Default.Get(PrefRemember, false)) {
            _rememberMe = true;
            ContactsInfo.Email = Preferences.Default.Get(PrefSavedEmail, string.Empty);
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public ContactsInfo ContactsInfo { get; }

    public bool RememberMe
    {
        get => _rememberMe;
        set => SetField(ref _rememberMe, value);
    }

    internal void PersistRememberPreference(string email)
    {
        if (RememberMe) {
            Preferences.Default.Set(PrefRemember, true);
            Preferences.Default.Set(PrefSavedEmail, email.Trim());
        }
        else {
            Preferences.Default.Set(PrefRemember, false);
            Preferences.Default.Remove(PrefSavedEmail);
        }
    }

    private void SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
            return;
        field = value;
        if (propertyName != null)
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

public class ContactsInfo
{
    public ContactsInfo()
    {
        Email = string.Empty;
        Password = string.Empty;
    }

    [Display(Name = "邮箱")]
    [DataFormDisplayOptions(ColumnSpan = 3)]
    [EmailAddress(ErrorMessage = "请输入有效邮箱")]
    [Required(ErrorMessage = "请输入邮箱")]
    public string Email { get; set; }

    [Display(Name = "密码")]
    [DataType(DataType.Password)]
    [Required(ErrorMessage = "请输入密码")]
    [MinLength(6, ErrorMessage = "密码至少 6 位")]
    [DataFormDisplayOptions(ColumnSpan = 3)]
    public string Password { get; set; }
}