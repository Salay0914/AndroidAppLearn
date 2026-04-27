namespace SyncfusionMAUIApp1.Services;

public sealed class NfcReaderStub : INfcReader
{
    public bool IsAvailable => false;
    public string? LastTagId => null;
    public event EventHandler<string>? TagReceived;
    public void StartListening() { }
    public void StopListening() { }
}
