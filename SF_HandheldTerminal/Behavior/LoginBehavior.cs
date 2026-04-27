using Microsoft.Extensions.DependencyInjection;
using SF_HandheldTerminal.Services;
using SF_HandheldTerminal.Views.Forms;
using SF_HandheldTerminal.Views.Main;
using Syncfusion.Maui.DataForm;
using Syncfusion.Maui.Toolkit.Buttons;

namespace SF_HandheldTerminal;

public class LoginBehavior : Behavior<Login>
{
    private SfDataForm? logInForm;
    private SfButton? saveButton;
    private Login? loginPage;
    private bool isBusy;

    protected override void OnAttachedTo(BindableObject bindable)
    {
        base.OnAttachedTo(bindable);
        loginPage = bindable as Login;
        if (loginPage == null)
            return;

        logInForm = loginPage.FindByName("logInForm") as SfDataForm;
        saveButton = loginPage.FindByName("saveButton") as SfButton;
        if (saveButton != null)
            saveButton.Clicked += OnSaveButtonClicked;
    }

    protected override void OnDetachingFrom(BindableObject bindable)
    {
        if (saveButton != null)
            saveButton.Clicked -= OnSaveButtonClicked;
        saveButton = null;
        logInForm = null;
        loginPage = null;
        base.OnDetachingFrom(bindable);
    }

    private async void OnSaveButtonClicked(object? sender, EventArgs e)
    {
        if (loginPage == null || logInForm == null || saveButton == null || isBusy)
            return;

        // 默认 CommitMode 为 LostFocus：若在密码框未失焦时点击登录，编辑值尚未写回 DataObject，Password 会一直为空。
        logInForm.Commit();
        if (!logInForm.Validate())
            return;

        if (loginPage.BindingContext is not LoginPageViewModel vm)
            return;

        var auth = loginPage.Handler?.MauiContext?.Services.GetService<ILoginAuthService>();
        if (auth == null)
        {
            await loginPage.DisplayAlert("错误", "未注册登录服务。", "确定");
            return;
        }

        isBusy = true;
        saveButton.IsEnabled = false;
        try
        {
            var result = await auth.AuthenticateAsync(vm.ContactsInfo.Email, vm.ContactsInfo.Password).ConfigureAwait(true);
            if (!result.Succeeded)
            {
                await loginPage.DisplayAlert("登录失败", result.ErrorMessage ?? "请检查账号或密码。", "确定");
                return;
            }

            vm.PersistRememberPreference(vm.ContactsInfo.Email);

            if (Application.Current?.Windows.FirstOrDefault() is { } window)
                window.Page = new MainTabPage();
        }
        finally
        {
            saveButton.IsEnabled = true;
            isBusy = false;
        }
    }
}
