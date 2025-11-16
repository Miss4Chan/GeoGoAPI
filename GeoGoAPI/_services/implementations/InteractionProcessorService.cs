using GeoGoAPI._models.dtos.interactions;
using GeoGoAPI._models.dtos.twin;
using GeoGoAPI._repositories.interfaces;
using GeoGoAPI._services.interfaces;

namespace GeoGoAPI._services.implementations;

public class InteractionProcessorService(
    IUserTwinService userTwinService,
    IPlaceRepository placeRepository,
    IPlaceLikesService placeLikesService,
    ICategoryWeightsService categoryWeightsService,
    IInteractionEventService interactionEventService
) : IInteractionProcessorService
{
    public async Task<DigitalTwinProfileDto?> ProcessInteractionAsync(
        int userId,
        InteractionRequestDto request
    )
    {
        // 1. Resolve the twin for the given user
        var twin = await userTwinService.GetTwinByUserIdAsync(userId);
        if (twin is null)
            return null; // or throw if you prefer

        var twinId = twin.Id;

        // 2. Load the place (not including deleted ones by default)
        var place = await placeRepository.GetByIdAsync(request.PlaceId, includeDeleted: false);
        if (place is null)
            return null; // or throw if invalid place

        // Virtual place id (if exists)
        var virtualPlaceId = place.VirtualPlace?.Id;

        // 3. Ensure a PlaceLike exists for (twin, place)
        //    and 4. Increment its score by delta (default 1.0)
        var delta = request.ScoreDelta ?? 1.0;
        var updatedLike = await placeLikesService.IncrementScoreAsync(twinId, place.Id, delta);

        // 5. Log InteractionEvent (linking twin, place, virtualPlace, virtualObject)
        await interactionEventService.LogEventAsync(
            userTwinId: twinId,
            placeId: place.Id,
            virtualPlaceId: virtualPlaceId,
            virtualObjectId: request.VirtualObjectId,
            eventType: request.EventType,
            metadata: request.Metadata
        );

        // 6. Recompute CategoryWeights for this twin based on all PlaceLikes
        await RecomputeCategoryWeightsForTwinAsync(twinId);

        // 7. Return updated digital twin profile (raw + normalized) for this user
        var profile = await userTwinService.GetPreferenceProfileAsync(userId);
        return profile;
    }

    /// <summary>
    /// Recomputes category weights as sum of PlaceLikes.Score per category
    /// for the given twin, and writes them to CategoryWeights.
    /// </summary>
    private async Task RecomputeCategoryWeightsForTwinAsync(int twinId)
    {
        // Get all likes with their Places (and Categories)
        var likes = await placeLikesService.GetByTwinIdAsync(twinId);

        // Group by category and sum scores
        var grouped = likes
            .Where(pl => pl.Place?.CategoryId != null)
            .GroupBy(pl => pl.Place!.CategoryId)
            .Select(g => new { CategoryId = g.Key, TotalScore = g.Sum(pl => pl.Score) });

        // Upsert each category weight with the new total
        foreach (var item in grouped)
        {
            await categoryWeightsService.SetWeightAsync(twinId, item.CategoryId, item.TotalScore);
        }

        // Note: If you want to also zero-out categories that no longer have likes,
        // you'd need to load existing weights and remove/reset ones not in the grouped set.
    }
}
