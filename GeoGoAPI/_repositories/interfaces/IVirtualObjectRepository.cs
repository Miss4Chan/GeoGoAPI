using GeoGoAPI._models.entities;

namespace GeoGoAPI._repositories.interfaces;

public interface IVirtualObjectRepository
{
    Task<VirtualObject?> GetByIdAsync(int id);
    Task<List<VirtualObject>> GetByVirtualPlaceIdAsync(int virtualPlaceId);

    Task AddAsync(VirtualObject virtualObject);
    void Update(VirtualObject virtualObject);
    void Delete(VirtualObject virtualObject);

    Task SaveChangesAsync();
}
