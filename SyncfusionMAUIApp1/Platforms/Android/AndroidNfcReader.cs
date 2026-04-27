using Android.App;
using Android.Content;
using Android.Nfc;

namespace SyncfusionMAUIApp1.Services;

public sealed class AndroidNfcReader : INfcReader
{
    public bool IsAvailable => NfcAdapter.GetDefaultAdapter(Android.App.Application.Context)?.IsEnabled == true;
    public string? LastTagId { get; private set; }
    public event EventHandler<string>? TagReceived;

    public void StartListening()
    {
        // 完整前台调度需在 MainActivity 配置 PendingIntent；此处保留接口形态。
    }

    public void StopListening()
    {
    }

    public static void HandleIntent(Intent? intent)
    {
        if (intent?.Action is not (NfcAdapter.ActionTagDiscovered or NfcAdapter.ActionNdefDiscovered))
            return;

#pragma warning disable CA1416
        var tag = intent.GetParcelableExtra(NfcAdapter.ExtraTag) as Tag;
#pragma warning restore CA1416
        if (tag?.GetId() is not { Length: > 0 } id)
            return;

        var hex = Convert.ToHexString(id);
        if (MauiProgram.Services.GetService<INfcReader>() is AndroidNfcReader reader)
        {
            reader.LastTagId = hex;
            reader.TagReceived?.Invoke(reader, hex);
        }
    }
}
