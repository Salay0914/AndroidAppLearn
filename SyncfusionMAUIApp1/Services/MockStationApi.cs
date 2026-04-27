using SyncfusionMAUIApp1.Contracts;

namespace SyncfusionMAUIApp1.Services;

public sealed class MockStationApi : IStationApi
{
    private readonly ILocalDatabase _db;

    public MockStationApi(ILocalDatabase db) => _db = db;

    public async Task<IReadOnlyList<InspectionTaskDto>> GetTasksAsync(CancellationToken cancellationToken = default)
    {
        await _db.EnsureInitializedAsync(cancellationToken).ConfigureAwait(false);
        return await _db.GetTaskDtosAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task<IReadOnlyList<CableArchiveDto>> GetArchivesByMarkerIdsAsync(IEnumerable<string> markerIds, CancellationToken cancellationToken = default)
    {
        await _db.EnsureInitializedAsync(cancellationToken).ConfigureAwait(false);
        var ids = markerIds.Where(x => !string.IsNullOrWhiteSpace(x)).Distinct().ToHashSet();
        var all = await _db.GetArchiveDtosAsync(cancellationToken).ConfigureAwait(false);
        return all.Where(a => ids.Contains(a.MarkerId)).ToList();
    }

    public Task SubmitInspectionAsync(InspectionSubmitDto dto, CancellationToken cancellationToken = default) =>
        Task.CompletedTask;
}
