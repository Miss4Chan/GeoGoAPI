namespace GeoGoAPI._models.dtos.places;

public class PlaceDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public int CategoryId { get; set; }
    public bool Active { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAtUtc { get; set; }
}
