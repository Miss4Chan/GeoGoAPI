namespace GeoGoAPI._models.dtos.virtualobjects;

public class VirtualObjectDto
{
    public int Id { get; set; }
    public int VirtualPlaceId { get; set; }

    public required string Name { get; set; }
    public required string ModelUrl { get; set; }
    public required string ModelUrlTexture { get; set; }

    public float PX { get; set; }
    public float PY { get; set; }
    public float PZ { get; set; }

    public float RX { get; set; }
    public float RY { get; set; }
    public float RZ { get; set; }

    public float SX { get; set; }
    public float SY { get; set; }
    public float SZ { get; set; }

    public string? TextDisplayed { get; set; }
}
