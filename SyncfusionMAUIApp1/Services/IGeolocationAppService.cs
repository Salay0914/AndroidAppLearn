namespace SyncfusionMAUIApp1.Services;

public sealed class GeoPoint
{
    public double Latitude { get; init; }
    public double Longitude { get; init; }
    public double? AccuracyMeters { get; init; }
}

public interface IGeolocationAppService
{
    Task<GeoPoint?> GetCurrentLocationAsync(CancellationToken cancellationToken = default);
}
