using GeoGoAPI._models.dtos.interactions;
using GeoGoAPI._models.dtos.twin;

namespace GeoGoAPI._services.interfaces;

public interface IInteractionProcessorService
{
    /// <summary>
    /// Processes a single user interaction:
    /// - ensures PlaceLike exists
    /// - increments its score
    /// - logs an InteractionEvent
    /// - recomputes CategoryWeights for the twin
    /// - returns the updated digital twin profile (raw + normalized)
    /// </summary>
    /// <param name="userId">AppUser Id (from JWT)</param>
    /// <param name="request">Interaction details</param>
    Task<DigitalTwinProfileDto?> ProcessInteractionAsync(int userId, InteractionRequestDto request);
}
