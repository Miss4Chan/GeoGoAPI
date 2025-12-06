using System.Text.Json.Serialization;

namespace GeoGoAPI._models.entities;

public class VirtualPlace
{
    public int Id { get; set; }

    public int PlaceId { get; set; }

    [JsonIgnore]
    public Place? Place { get; set; }

    public ICollection<VirtualObject> VirtualObjects { get; set; } = new List<VirtualObject>();
    public ICollection<InteractionEvent> InteractionEvents { get; set; } = [];
}
