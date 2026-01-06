using GeoGoAPI._models.entities;

namespace GeoGoAPI._repositories.interfaces;

public interface IInteractionEventRepository
{
    Task<InteractionEvent?> GetByIdAsync(int id);

    Task<List<InteractionEvent>> GetByTwinAsync(int userTwinId);
    Task<List<InteractionEvent>> GetByPlaceAsync(int placeId);
    Task<List<InteractionEvent>> GetByVirtualPlaceAsync(int virtualPlaceId);
    Task<List<InteractionEvent>> GetByVirtualObjectAsync(int virtualObjectId);
    Task<List<InteractionEvent>> GetUserHistoryWithDetailsAsync(int userTwinId, int limit = 30);

    Task AddAsync(InteractionEvent interactionEvent);

    Task SaveChangesAsync();
}
