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
        var twin = await userTwinService.GetTwinByUserIdAsync(userId);
        if (twin is null)
            return null;

        var twinId = twin.Id;

        var place = await placeRepository.GetByIdAsync(request.PlaceId, includeDeleted: false);
        if (place is null)
            return null;

        var virtualPlaceId = place.VirtualPlace?.Id;

        var delta = request.ScoreDelta ?? 1.0;
        var updatedLike = await placeLikesService.IncrementScoreAsync(twinId, place.Id, delta);

        await interactionEventService.LogEventAsync(
            userTwinId: twinId,
            placeId: place.Id,
            virtualPlaceId: virtualPlaceId,
            virtualObjectId: request.VirtualObjectId,
            eventType: request.EventType,
            metadata: request.Metadata
        );

        await RecomputeCategoryWeightsForTwinAsync(twinId);

        var profile = await userTwinService.GetPreferenceProfileAsync(userId);
        return profile;
    }

    private async Task RecomputeCategoryWeightsForTwinAsync(int twinId)
    {
        var likes = await placeLikesService.GetByTwinIdAsync(twinId);

        var grouped = likes
            .Where(pl => pl.Place?.CategoryId != null)
            .GroupBy(pl => pl.Place!.CategoryId)
            .Select(g => new { CategoryId = g.Key, TotalScore = g.Sum(pl => pl.Score) });

        foreach (var item in grouped)
        {
            await categoryWeightsService.SetWeightAsync(twinId, item.CategoryId, item.TotalScore);
        }
    }
}
