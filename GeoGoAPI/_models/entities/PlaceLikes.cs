namespace GeoGoAPI._models.entities;

public class PlaceLikes
{
    public int UserTwinId { get; set; }
    public int PlaceId { get; set; }

    public UserTwin? UserTwin { get; set; }
    public Place? Place { get; set; }
    public double Score { get; set; }

    public ICollection<InteractionEvent> InteractionEvents { get; set; } = [];
}
