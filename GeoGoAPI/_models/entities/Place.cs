namespace GeoGoAPI._models.entities;

public class Place
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public int CategoryId { get; set; }
    public Category? Category { get; set; }
    public bool Active { get; set; } = true;

    // Soft delete basically we dont delete truly we just set it to true
    public bool IsDeleted { get; set; } = false;
    public DateTime? DeletedAtUtc { get; set; }
    public VirtualPlace? VirtualPlace { get; set; }

    public ICollection<PlaceLikes> PlaceLikes { get; set; } = [];
}
