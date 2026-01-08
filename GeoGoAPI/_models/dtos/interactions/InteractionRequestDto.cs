namespace GeoGoAPI._models.dtos.interactions;

public class InteractionRequestDto
{
    public int PlaceId { get; set; }
    public int? VirtualObjectId { get; set; }
    public required string EventType { get; set; }

    public string? Metadata { get; set; }

    public double? ScoreDelta { get; set; }
}
