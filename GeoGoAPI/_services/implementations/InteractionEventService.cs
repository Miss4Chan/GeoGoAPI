using GeoGoAPI._models.entities;
using GeoGoAPI._repositories.interfaces;
using GeoGoAPI._services.interfaces;

namespace GeoGoAPI._services.implementations;

public class InteractionEventService(IInteractionEventRepository repo) : IInteractionEventService
{
    public async Task<InteractionEvent> LogEventAsync(
        int? userTwinId,
        int? placeId,
        int? virtualPlaceId,
        int? virtualObjectId,
        string eventType,
        string? metadata = null
    )
    {
        var ev = new InteractionEvent
        {
            UserTwinId = userTwinId,
            PlaceId = placeId,
            VirtualPlaceId = virtualPlaceId,
            VirtualObjectId = virtualObjectId,
            EventType = eventType,
            // Timestamp will be set by DB default, but we also set UTC explicitly
            Timestamp = DateTime.UtcNow,
            Metadata = metadata,
        };

        await repo.AddAsync(ev);
        await repo.SaveChangesAsync();

        return ev;
    }

    public async Task<InteractionEvent?> GetByIdAsync(int id)
    {
        return await repo.GetByIdAsync(id);
    }

    public async Task<IReadOnlyList<InteractionEvent>> GetByTwinAsync(int userTwinId)
    {
        return await repo.GetByTwinAsync(userTwinId);
    }

    public async Task<IReadOnlyList<InteractionEvent>> GetByPlaceAsync(int placeId)
    {
        return await repo.GetByPlaceAsync(placeId);
    }

    public async Task<IReadOnlyList<InteractionEvent>> GetByVirtualPlaceAsync(int virtualPlaceId)
    {
        return await repo.GetByVirtualPlaceAsync(virtualPlaceId);
    }

    public async Task<IReadOnlyList<InteractionEvent>> GetByVirtualObjectAsync(int virtualObjectId)
    {
        return await repo.GetByVirtualObjectAsync(virtualObjectId);
    }
}
