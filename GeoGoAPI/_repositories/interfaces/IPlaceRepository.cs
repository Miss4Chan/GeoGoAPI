using GeoGoAPI._models.entities;

namespace GeoGoAPI._repositories.interfaces;

public interface IPlaceRepository
{
    Task<List<Place>> GetAllAsync(bool includeDeleted = false);
    Task<Place?> GetByIdAsync(int id, bool includeDeleted = false);
    Task AddAsync(Place place);
    void Update(Place place);
    Task SoftDeleteAsync(Place place);
    Task RestoreAsync(Place place);
    Task<bool> CategoryExistsAsync(int categoryId);
    Task SaveChangesAsync();
}
