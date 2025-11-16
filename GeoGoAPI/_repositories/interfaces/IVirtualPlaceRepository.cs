using GeoGoAPI._models.entities;

namespace GeoGoAPI._repositories.interfaces;

public interface IVirtualPlaceRepository
{
    Task<List<VirtualPlace>> GetAllAsync(bool includeDeleted = false);
    Task<VirtualPlace?> GetByIdAsync(int id, bool includeDeleted = false);
    Task<VirtualPlace?> GetByPlaceIdAsync(int placeId, bool includeDeleted = false);

    Task AddAsync(VirtualPlace virtualPlace);
    void Update(VirtualPlace virtualPlace);
    void Delete(VirtualPlace virtualPlace);

    Task SaveChangesAsync();
}
