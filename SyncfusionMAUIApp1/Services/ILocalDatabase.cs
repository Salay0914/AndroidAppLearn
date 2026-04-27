using SyncfusionMAUIApp1.Contracts;
using SyncfusionMAUIApp1.Data;

namespace SyncfusionMAUIApp1.Services;

public interface ILocalDatabase
{
    Task EnsureInitializedAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<InspectionTaskDto>> GetTaskDtosAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<CableArchiveDto>> GetArchiveDtosAsync(CancellationToken cancellationToken = default);
    Task<CableArchiveDto?> GetArchiveByMarkerAsync(string markerId, CancellationToken cancellationToken = default);
    Task SaveInspectionRecordAsync(string taskId, InspectionPointDto point, string operatorName, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<InspectionPointDto>> GetInspectionRecordsAsync(string taskId, CancellationToken cancellationToken = default);
    Task EnqueueOutboxAsync(string operation, string payloadJson, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SyncOutboxEntity>> DequeueOutboxAsync(int max, CancellationToken cancellationToken = default);
    Task MarkOutboxDoneAsync(string id, CancellationToken cancellationToken = default);
    Task IncrementOutboxRetryAsync(string id, CancellationToken cancellationToken = default);
    Task<bool> IsTaskFullyScannedAsync(string taskId, CancellationToken cancellationToken = default);
}
