namespace GeoGoAPI._models.entities;

public class UserTwin
{
    public int Id { get; set; }

    public int UserId { get; set; }
    public AppUser? User { get; set; }

    public ICollection<CategoryWeights> CategoryWeights { get; set; } = new List<CategoryWeights>();
    public ICollection<PlaceLikes> PlaceLikes { get; set; } = new List<PlaceLikes>();
}
