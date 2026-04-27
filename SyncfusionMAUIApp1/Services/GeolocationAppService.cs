using Microsoft.Maui.Devices.Sensors;

namespace SyncfusionMAUIApp1.Services;

public sealed class GeolocationAppService : IGeolocationAppService
{
    public async Task<GeoPoint?> GetCurrentLocationAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
            var loc = await Geolocation.Default.GetLocationAsync(request, cancellationToken).ConfigureAwait(false);
            if (loc is null)
                return null;

            return new GeoPoint
            {
                Latitude = loc.Latitude,
                Longitude = loc.Longitude,
                AccuracyMeters = loc.Accuracy
            };
        }
        catch
        {
            return null;
        }
    }
}
