using Android.OS;
using AndroidX.Biometric;
using AndroidX.Core.Content;
using AndroidX.Fragment.App;
using Microsoft.Maui.ApplicationModel;

namespace SyncfusionMAUIApp1.Services;

public sealed class AndroidBiometricAuthUi : IBiometricAuthUi
{
    public Task<bool> AuthenticateAsync(string title, string subtitle, CancellationToken cancellationToken = default)
    {
        var activity = Platform.CurrentActivity as FragmentActivity;
        if (activity is null)
            return Task.FromResult(false);

        if (Build.VERSION.SdkInt < BuildVersionCodes.M)
            return Task.FromResult(false);

        var tcs = new TaskCompletionSource<bool>();
        var executor = ContextCompat.GetMainExecutor(activity);

        activity.RunOnUiThread(() =>
        {
            try
            {
                // 官方 Xamarin.AndroidX.Biometric 绑定中，PromptInfo 为 BiometricPrompt 的嵌套类型。
                var promptInfo = new BiometricPrompt.PromptInfo.Builder()
                    .SetTitle(title)
                    .SetSubtitle(subtitle)
                    .SetNegativeButtonText("取消")
                    .Build();

                var callback = new BiometricAuthCallback(tcs);
                var biometricPrompt = new BiometricPrompt(activity, executor, callback);
                biometricPrompt.Authenticate(promptInfo);
            }
            catch
            {
                tcs.TrySetResult(false);
            }
        });

        return tcs.Task.WaitAsync(cancellationToken);
    }

    private sealed class BiometricAuthCallback : BiometricPrompt.AuthenticationCallback
    {
        private readonly TaskCompletionSource<bool> _tcs;

        public BiometricAuthCallback(TaskCompletionSource<bool> tcs) => _tcs = tcs;

        public override void OnAuthenticationSucceeded(BiometricPrompt.AuthenticationResult result) =>
            _tcs.TrySetResult(true);

        public override void OnAuthenticationError(int errorCode, Java.Lang.ICharSequence errString) =>
            _tcs.TrySetResult(false);

        public override void OnAuthenticationFailed() =>
            _tcs.TrySetResult(false);
    }
}
