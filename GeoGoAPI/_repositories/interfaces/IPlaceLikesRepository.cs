using GeoGoAPI._models.entities;

namespace GeoGoAPI._repositories.interfaces;

public interface IPlaceLikesRepository
{
    Task<PlaceLikes?> GetAsync(int userTwinId, int placeId);
    Task<List<PlaceLikes>> GetByTwinIdAsync(int userTwinId);
    Task<List<PlaceLikes>> GetByPlaceIdAsync(int placeId);

    Task AddAsync(PlaceLikes like);
    void Update(PlaceLikes like);
    void Delete(PlaceLikes like);

    Task SaveChangesAsync();
}
