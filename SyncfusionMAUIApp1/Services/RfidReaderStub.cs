namespace SyncfusionMAUIApp1.Services;

public sealed class RfidReaderStub : IRfidReader
{
    public Task<string?> ReadOnceAsync(CancellationToken cancellationToken = default) => Task.FromResult<string?>(null);
}
