namespace SyncfusionMAUIApp1.Services;

public interface INfcReader
{
    bool IsAvailable { get; }
    string? LastTagId { get; }
    event EventHandler<string>? TagReceived;
    void StartListening();
    void StopListening();
}
