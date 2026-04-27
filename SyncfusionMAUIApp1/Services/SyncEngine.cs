using System.Text.Json;
using SyncfusionMAUIApp1.Contracts;

namespace SyncfusionMAUIApp1.Services;

public sealed class SyncEngine : ISyncEngine
{
    private readonly ILocalDatabase _db;
    private readonly IStationApi _api;

    public SyncEngine(ILocalDatabase db, IStationApi api)
    {
        _db = db;
        _api = api;
    }

    public async Task<int> ProcessOutboxAsync(CancellationToken cancellationToken = default)
    {
        await _db.EnsureInitializedAsync(cancellationToken).ConfigureAwait(false);
        var pending = await _db.DequeueOutboxAsync(20, cancellationToken).ConfigureAwait(false);
        var processed = 0;
        foreach (var item in pending)
        {
            try
            {
                if (string.Equals(item.Operation, "SubmitInspection", StringComparison.OrdinalIgnoreCase))
                {
                    var dto = JsonSerializer.Deserialize<InspectionSubmitDto>(item.PayloadJson);
                    if (dto is not null)
                        await _api.SubmitInspectionAsync(dto, cancellationToken).ConfigureAwait(false);
                }

                await _db.MarkOutboxDoneAsync(item.Id, cancellationToken).ConfigureAwait(false);
                processed++;
            }
            catch
            {
                await _db.IncrementOutboxRetryAsync(item.Id, cancellationToken).ConfigureAwait(false);
            }
        }

        return processed;
    }
}
