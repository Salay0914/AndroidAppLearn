namespace SyncfusionMAUIApp1.Services;

public sealed class SignalFeedbackService : ISignalFeedbackService
{
    public void PlaySignalPulse(double normalizedStrength)
    {
        // 跨平台占位：可替换为 Android ToneGenerator 等。
        _ = normalizedStrength;
    }
}
