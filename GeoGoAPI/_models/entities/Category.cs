namespace GeoGoAPI._models.entities;

public class Category
{
    public int Id { get; set; }
    public required string Name { get; set; }

    public ICollection<Place> Places { get; set; } = new List<Place>();
    public ICollection<CategoryWeights> CategoryWeights { get; set; } = new List<CategoryWeights>();
}
