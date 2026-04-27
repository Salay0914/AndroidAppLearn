using SyncfusionMAUIApp1.Contracts;

namespace SyncfusionMAUIApp1.Services;

public interface IStationApi
{
    Task<IReadOnlyList<InspectionTaskDto>> GetTasksAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<CableArchiveDto>> GetArchivesByMarkerIdsAsync(IEnumerable<string> markerIds, CancellationToken cancellationToken = default);
    Task SubmitInspectionAsync(InspectionSubmitDto dto, CancellationToken cancellationToken = default);
}
