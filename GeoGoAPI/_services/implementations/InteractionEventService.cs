using GeoGoAPI._models.entities;
using GeoGoAPI._repositories.interfaces;
using GeoGoAPI._services.interfaces;

namespace GeoGoAPI._services.implementations;

public class InteractionEventService(IInteractionEventRepository repo, IUserTwinRepository twinRepo)
    : IInteractionEventService
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

    public async Task<List<InteractionHistoryDto>> GetUserHistoryAsync(int userId, int limit = 30)
    {
        // Get the user's twin
        var twin = await twinRepo.GetByUserIdAsync(userId);
        if (twin == null)
            return new List<InteractionHistoryDto>();

        // Get interactions with all related data loaded
        var interactions = await repo.GetUserHistoryWithDetailsAsync(twin.Id, limit);

        // Map to DTOs
        return interactions
            .Select(ie => new InteractionHistoryDto
            {
                Id = ie.Id,
                Timestamp = ie.Timestamp,
                EventType = ie.EventType,
                Metadata = ie.Metadata,

                // Place info
                PlaceId = ie.PlaceId,
                PlaceName = ie.PlaceLike?.Place?.Name,
                CategoryName = ie.PlaceLike?.Place?.Category?.Name,

                // Virtual Object info
                VirtualObjectId = ie.VirtualObjectId,
                VirtualObjectName = ie.VirtualObject?.Name,
                StepsJson = ie.VirtualObject?.StepsJson,

                // Try to extract score delta from metadata if it exists
                ScoreDelta = ExtractScoreDeltaFromMetadata(ie.Metadata),
            })
            .ToList();
    }

    private static double? ExtractScoreDeltaFromMetadata(string? metadata)
    {
        if (string.IsNullOrEmpty(metadata))
            return null;

        try
        {
            var scoreDeltaIndex = metadata.IndexOf(
                "\"score_delta\":",
                StringComparison.OrdinalIgnoreCase
            );
            if (scoreDeltaIndex == -1)
                scoreDeltaIndex = metadata.IndexOf(
                    "\"scoreDelta\":",
                    StringComparison.OrdinalIgnoreCase
                );

            if (scoreDeltaIndex >= 0)
            {
                var valueStart = metadata.IndexOf(':', scoreDeltaIndex) + 1;
                var valueEnd = metadata.IndexOfAny(new[] { ',', '}' }, valueStart);
                if (valueEnd == -1)
                    valueEnd = metadata.Length;

                var valueStr = metadata.Substring(valueStart, valueEnd - valueStart).Trim();
                if (double.TryParse(valueStr, out var score))
                    return score;
            }
        }
        catch
        {
            Console.WriteLine("Failed to parse score delta from metadata.");
        }

        return null;
    }
}
