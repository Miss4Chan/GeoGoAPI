namespace GeoGoAPI._models.dtos.virtualobjects;

public class UpdateVirtualObjectDto
{
    public required string Name { get; set; }
    public required string ModelUrl { get; set; }

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
}
