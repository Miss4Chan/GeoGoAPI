namespace GeoGoAPI._models.entities;

public class VirtualObject
{
    public int Id { get; set; }
    public int VirtualPlaceId { get; set; }
    public VirtualPlace? VirtualPlace { get; set; }

    public required string Name { get; set; }
    public required string ModelUrl { get; set; }
    public required string ModelUrlTexture { get; set; }

    // Transform (Position / Rotation / Scale)
    public float PX { get; set; }
    public float PY { get; set; }
    public float PZ { get; set; }

    public float RX { get; set; }
    public float RY { get; set; }
    public float RZ { get; set; }

    public float SX { get; set; } = 1f;
    public float SY { get; set; } = 1f;
    public float SZ { get; set; } = 1f;

    public string? TextDisplayed { get; set; }

    public ICollection<InteractionEvent> InteractionEvents { get; set; } = [];
}
