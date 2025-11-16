namespace GeoGoAPI._models.dtos.interactions;

public class InteractionRequestDto
{
    /// <summary>
    /// Place the user is interacting with (physical place).
    /// Required.
    /// </summary>
    public int PlaceId { get; set; }

    /// <summary>
    /// Optional specific virtual object inside the virtual place.
    /// </summary>
    public int? VirtualObjectId { get; set; }

    /// <summary>
    /// Type of event, e.g. "OBJECT_TAP", "PLACE_ENTER", "PLACE_EXIT".
    /// </summary>
    public required string EventType { get; set; }

    /// <summary>
    /// Optional metadata (JSON, string, etc.).
    /// </summary>
    public string? Metadata { get; set; }

    /// <summary>
    /// Optional score delta applied to this interaction for the place.
    /// Default is 1.0 if not set.
    /// </summary>
    public double? ScoreDelta { get; set; }
}
