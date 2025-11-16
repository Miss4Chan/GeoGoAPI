namespace GeoGoAPI._models.entities;

public class CategoryWeights
{
    public int UserTwinId { get; set; }
    public int CategoryId { get; set; }
    public double Score { get; set; }

    public UserTwin? UserTwin { get; set; }
    public Category? Category { get; set; }
}
