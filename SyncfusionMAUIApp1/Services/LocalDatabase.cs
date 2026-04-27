using SQLite;
using SQLitePCL;
using SyncfusionMAUIApp1.Contracts;
using SyncfusionMAUIApp1.Data;

namespace SyncfusionMAUIApp1.Services;

public sealed class LocalDatabase : ILocalDatabase
{
    private readonly ICryptoService _crypto;
    private SQLiteAsyncConnection? _connection;
    private readonly SemaphoreSlim _initLock = new(1, 1);
    private bool _initialized;

    public LocalDatabase(ICryptoService crypto) => _crypto = crypto;

    private async Task<SQLiteAsyncConnection> GetConnectionAsync(CancellationToken cancellationToken)
    {
        await EnsureInitializedAsync(cancellationToken).ConfigureAwait(false);
        return _connection ?? throw new InvalidOperationException("Database not initialized.");
    }

    public async Task EnsureInitializedAsync(CancellationToken cancellationToken = default)
    {
        if (_initialized)
            return;

        await _initLock.WaitAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            if (_initialized)
                return;

            Batteries_V2.Init();
            var path = Path.Combine(FileSystem.AppDataDirectory, "cable_ops.db3");
            _connection = new SQLiteAsyncConnection(path, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.SharedCache);
            await _connection.CreateTableAsync<InspectionTaskEntity>().ConfigureAwait(false);
            await _connection.CreateTableAsync<CableArchiveEntity>().ConfigureAwait(false);
            await _connection.CreateTableAsync<InspectionRecordEntity>().ConfigureAwait(false);
            await _connection.CreateTableAsync<SyncOutboxEntity>().ConfigureAwait(false);

            var taskCount = await _connection.Table<InspectionTaskEntity>().CountAsync().ConfigureAwait(false);
            if (taskCount == 0)
                await SeedAsync(cancellationToken).ConfigureAwait(false);

            _initialized = true;
        }
        finally
        {
            _initLock.Release();
        }
    }

    private async Task SeedAsync(CancellationToken cancellationToken)
    {
        var markers = new[] { "LY801-0001", "LY801-0002", "LY801-0003" };
        foreach (var m in markers)
        {
            var enc = await _crypto.EncryptAsync($"示例备注：{m}", cancellationToken).ConfigureAwait(false);
            await _connection!.InsertOrReplaceAsync(new CableArchiveEntity
            {
                MarkerId = m,
                Location = "示例站场 K1+200",
                BurialDepthMeters = 1.2 + markers.ToList().IndexOf(m) * 0.1,
                PipeMaterial = "HDPE",
                DiameterMm = 110,
                LaidOnUtcTicks = DateTime.UtcNow.AddYears(-2).Ticks,
                OwnerUnit = "电务段示例",
                EncryptedNotes = enc
            }).ConfigureAwait(false);
        }

        await _connection!.InsertAsync(new InspectionTaskEntity
        {
            Id = "task-demo-1",
            Title = "示例：咽喉区电缆巡检",
            PlannedDateUtcTicks = DateTime.UtcNow.Date.Ticks,
            RequiredMarkerIdsCsv = string.Join(',', markers)
        }).ConfigureAwait(false);
    }

    public async Task<IReadOnlyList<InspectionTaskDto>> GetTaskDtosAsync(CancellationToken cancellationToken = default)
    {
        var conn = await GetConnectionAsync(cancellationToken).ConfigureAwait(false);
        var rows = await conn.Table<InspectionTaskEntity>().ToListAsync().ConfigureAwait(false);
        return rows.Select(r => new InspectionTaskDto
        {
            Id = r.Id,
            Title = r.Title,
            PlannedDateUtc = new DateTime(r.PlannedDateUtcTicks, DateTimeKind.Utc),
            RequiredMarkerIds = string.IsNullOrWhiteSpace(r.RequiredMarkerIdsCsv)
                ? Array.Empty<string>()
                : r.RequiredMarkerIdsCsv.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
        }).ToList();
    }

    public async Task<IReadOnlyList<CableArchiveDto>> GetArchiveDtosAsync(CancellationToken cancellationToken = default)
    {
        var conn = await GetConnectionAsync(cancellationToken).ConfigureAwait(false);
        var rows = await conn.Table<CableArchiveEntity>().ToListAsync().ConfigureAwait(false);
        var list = new List<CableArchiveDto>();
        foreach (var r in rows)
        {
            var notes = string.Empty;
            if (!string.IsNullOrEmpty(r.EncryptedNotes))
                notes = await _crypto.DecryptAsync(r.EncryptedNotes, cancellationToken).ConfigureAwait(false);

            list.Add(new CableArchiveDto
            {
                MarkerId = r.MarkerId,
                Location = r.Location,
                BurialDepthMeters = r.BurialDepthMeters,
                PipeMaterial = r.PipeMaterial,
                DiameterMm = r.DiameterMm,
                LaidOnUtc = r.LaidOnUtcTicks is null ? null : new DateTime(r.LaidOnUtcTicks.Value, DateTimeKind.Utc),
                OwnerUnit = r.OwnerUnit,
                Notes = notes
            });
        }

        return list;
    }

    public async Task<CableArchiveDto?> GetArchiveByMarkerAsync(string markerId, CancellationToken cancellationToken = default)
    {
        var all = await GetArchiveDtosAsync(cancellationToken).ConfigureAwait(false);
        return all.FirstOrDefault(a => string.Equals(a.MarkerId, markerId, StringComparison.OrdinalIgnoreCase));
    }

    public async Task SaveInspectionRecordAsync(string taskId, InspectionPointDto point, string operatorName, CancellationToken cancellationToken = default)
    {
        var conn = await GetConnectionAsync(cancellationToken).ConfigureAwait(false);
        await conn.InsertAsync(new InspectionRecordEntity
        {
            TaskId = taskId,
            MarkerId = point.MarkerId,
            ReadAtUtcTicks = point.ReadAtUtc.ToUniversalTime().Ticks,
            Latitude = point.Latitude,
            Longitude = point.Longitude,
            DepthMeters = point.DepthMeters,
            PhotoPath = point.PhotoPath,
            Operator = operatorName
        }).ConfigureAwait(false);
    }

    public async Task<IReadOnlyList<InspectionPointDto>> GetInspectionRecordsAsync(string taskId, CancellationToken cancellationToken = default)
    {
        var conn = await GetConnectionAsync(cancellationToken).ConfigureAwait(false);
        var rows = await conn.Table<InspectionRecordEntity>().Where(x => x.TaskId == taskId).ToListAsync().ConfigureAwait(false);
        return rows.Select(r => new InspectionPointDto
        {
            MarkerId = r.MarkerId,
            ReadAtUtc = new DateTime(r.ReadAtUtcTicks, DateTimeKind.Utc),
            Latitude = r.Latitude,
            Longitude = r.Longitude,
            DepthMeters = r.DepthMeters,
            PhotoPath = r.PhotoPath
        }).ToList();
    }

    public async Task EnqueueOutboxAsync(string operation, string payloadJson, CancellationToken cancellationToken = default)
    {
        var conn = await GetConnectionAsync(cancellationToken).ConfigureAwait(false);
        await conn.InsertAsync(new SyncOutboxEntity
        {
            Operation = operation,
            PayloadJson = payloadJson
        }).ConfigureAwait(false);
    }

    public async Task<IReadOnlyList<SyncOutboxEntity>> DequeueOutboxAsync(int max, CancellationToken cancellationToken = default)
    {
        var conn = await GetConnectionAsync(cancellationToken).ConfigureAwait(false);
        var all = await conn.Table<SyncOutboxEntity>().ToListAsync().ConfigureAwait(false);
        return all.OrderBy(x => x.CreatedUtcTicks).Take(max).ToList();
    }

    public async Task MarkOutboxDoneAsync(string id, CancellationToken cancellationToken = default)
    {
        var conn = await GetConnectionAsync(cancellationToken).ConfigureAwait(false);
        await conn.DeleteAsync(new SyncOutboxEntity { Id = id }).ConfigureAwait(false);
    }

    public async Task IncrementOutboxRetryAsync(string id, CancellationToken cancellationToken = default)
    {
        var conn = await GetConnectionAsync(cancellationToken).ConfigureAwait(false);
        var row = await conn.GetAsync<SyncOutboxEntity>(id).ConfigureAwait(false);
        row.RetryCount++;
        await conn.UpdateAsync(row).ConfigureAwait(false);
    }

    public async Task<bool> IsTaskFullyScannedAsync(string taskId, CancellationToken cancellationToken = default)
    {
        var conn = await GetConnectionAsync(cancellationToken).ConfigureAwait(false);
        var task = await conn.GetAsync<InspectionTaskEntity>(taskId).ConfigureAwait(false);
        var required = task.RequiredMarkerIdsCsv.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToHashSet(StringComparer.OrdinalIgnoreCase);
        if (required.Count == 0)
            return false;

        var scanned = await conn.Table<InspectionRecordEntity>()
            .Where(x => x.TaskId == taskId)
            .ToListAsync()
            .ConfigureAwait(false);

        var scannedIds = scanned.Select(s => s.MarkerId).ToHashSet(StringComparer.OrdinalIgnoreCase);
        return required.All(id => scannedIds.Contains(id));
    }
}
