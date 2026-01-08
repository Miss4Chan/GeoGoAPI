using GeoGoAPI._models.entities;

namespace GeoGoAPI._services.interfaces;

public interface IInteractionEventService
{
    Task<InteractionEvent> LogEventAsync(
        int? userTwinId,
        int? placeId,
        int? virtualPlaceId,
        int? virtualObjectId,
        string eventType,
        string? metadata = null
    );

    Task<InteractionEvent?> GetByIdAsync(int id);
    Task<IReadOnlyList<InteractionEvent>> GetByTwinAsync(int userTwinId);
    Task<IReadOnlyList<InteractionEvent>> GetByPlaceAsync(int placeId);
    Task<IReadOnlyList<InteractionEvent>> GetByVirtualPlaceAsync(int virtualPlaceId);
    Task<IReadOnlyList<InteractionEvent>> GetByVirtualObjectAsync(int virtualObjectId);
    Task<List<InteractionHistoryDto>> GetUserHistoryAsync(int userId, int limit = 30);
}
