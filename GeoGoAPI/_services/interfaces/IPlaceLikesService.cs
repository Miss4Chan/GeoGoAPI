using GeoGoAPI._models.entities;

namespace GeoGoAPI._services.interfaces;

public interface IPlaceLikesService
{
    Task<PlaceLikes?> GetAsync(int twinId, int placeId);
    Task<IReadOnlyList<PlaceLikes>> GetByTwinIdAsync(int twinId);
    Task<IReadOnlyList<PlaceLikes>> GetByPlaceIdAsync(int placeId);

    Task<PlaceLikes> CreateIfNotExistsAsync(int twinId, int placeId, double initialScore = 0);

    Task<PlaceLikes> IncrementScoreAsync(int twinId, int placeId, double delta);

    Task<bool> RemoveAsync(int twinId, int placeId);
}
