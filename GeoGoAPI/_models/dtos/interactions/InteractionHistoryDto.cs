public class InteractionHistoryDto
{
    public int Id { get; set; }
    public DateTime Timestamp { get; set; }
    public string EventType { get; set; } = "";
    public string? Metadata { get; set; }

    // Place info
    public int? PlaceId { get; set; }
    public string? PlaceName { get; set; }
    public string? CategoryName { get; set; }

    // Virtual Object info
    public int? VirtualObjectId { get; set; }
    public string? VirtualObjectName { get; set; }
    public string? StepsJson { get; set; }

    // Score info (if applicable)
    public double? ScoreDelta { get; set; }
}
