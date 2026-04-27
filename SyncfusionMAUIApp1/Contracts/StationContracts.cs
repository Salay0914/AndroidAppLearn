namespace SyncfusionMAUIApp1.Contracts;

public sealed class InspectionTaskDto
{
    public string Id { get; set; } = Guid.NewGuid().ToString("N");
    public string Title { get; set; } = string.Empty;
    public DateTime PlannedDateUtc { get; set; } = DateTime.UtcNow.Date;
    public IReadOnlyList<string> RequiredMarkerIds { get; set; } = Array.Empty<string>();
}

public sealed class CableArchiveDto
{
    public string MarkerId { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public double BurialDepthMeters { get; set; }
    public string PipeMaterial { get; set; } = string.Empty;
    public double DiameterMm { get; set; }
    public DateTime? LaidOnUtc { get; set; }
    public string OwnerUnit { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
}

public sealed class InspectionSubmitDto
{
    public string TaskId { get; set; } = string.Empty;
    public string Operator { get; set; } = string.Empty;
    public IReadOnlyList<InspectionPointDto> Points { get; set; } = Array.Empty<InspectionPointDto>();
}

public sealed class InspectionPointDto
{
    public string MarkerId { get; set; } = string.Empty;
    public DateTime ReadAtUtc { get; set; } = DateTime.UtcNow;
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public double? DepthMeters { get; set; }
    public string? PhotoPath { get; set; }
}
