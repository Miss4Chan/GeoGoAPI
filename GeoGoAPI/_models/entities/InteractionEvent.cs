namespace GeoGoAPI._models.entities;

public class InteractionEvent
{
    public int Id { get; set; }
    public int? UserTwinId { get; set; }
    public UserTwin? UserTwin { get; set; }
    public int? PlaceId { get; set; }
    public PlaceLikes? PlaceLike { get; set; }

    public int? VirtualPlaceId { get; set; }
    public VirtualPlace? VirtualPlace { get; set; }

    public int? VirtualObjectId { get; set; }
    public VirtualObject? VirtualObject { get; set; }

    public int? VirtualAnimationId { get; set; } // Will be added later :)))

    public required string EventType { get; set; }
    public DateTime Timestamp { get; set; }
    public string? Metadata { get; set; } // maybe?
}
