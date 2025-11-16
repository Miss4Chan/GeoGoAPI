using GeoGoAPI._models.entities;
using GeoGoAPI._repositories.interfaces;
using GeoGoAPI._services.interfaces;

namespace GeoGoAPI._services.implementations;

public class PlaceLikesService(IPlaceLikesRepository repo) : IPlaceLikesService
{
    public async Task<PlaceLikes?> GetAsync(int twinId, int placeId)
    {
        return await repo.GetAsync(twinId, placeId);
    }

    public async Task<IReadOnlyList<PlaceLikes>> GetByTwinIdAsync(int twinId)
    {
        return await repo.GetByTwinIdAsync(twinId);
    }

    public async Task<IReadOnlyList<PlaceLikes>> GetByPlaceIdAsync(int placeId)
    {
        return await repo.GetByPlaceIdAsync(placeId);
    }

    public async Task<PlaceLikes> CreateIfNotExistsAsync(
        int twinId,
        int placeId,
        double initialScore = 0
    )
    {
        var existing = await repo.GetAsync(twinId, placeId);
        if (existing is not null)
            return existing;

        var like = new PlaceLikes
        {
            UserTwinId = twinId,
            PlaceId = placeId,
            Score = initialScore,
        };

        await repo.AddAsync(like);
        await repo.SaveChangesAsync();

        return like;
    }

    public async Task<PlaceLikes> IncrementScoreAsync(int twinId, int placeId, double delta)
    {
        var like = await repo.GetAsync(twinId, placeId);

        if (like is null)
        {
            like = new PlaceLikes
            {
                UserTwinId = twinId,
                PlaceId = placeId,
                Score = delta,
            };
            await repo.AddAsync(like);
        }
        else
        {
            like.Score += delta;
            repo.Update(like);
        }

        await repo.SaveChangesAsync();
        return like;
    }

    public async Task<bool> RemoveAsync(int twinId, int placeId)
    {
        var like = await repo.GetAsync(twinId, placeId);
        if (like is null)
            return false;

        repo.Delete(like);
        await repo.SaveChangesAsync();
        return true;
    }
}
