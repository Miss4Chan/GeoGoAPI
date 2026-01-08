using GeoGoAPI._models.entities;

namespace GeoGoAPI._services.interfaces;

public interface IVirtualPlaceService
{
    Task<List<VirtualPlace>> GetAllAsync(bool includeDeleted = false);
    Task<VirtualPlace?> GetByIdAsync(int id, bool includeDeleted = false);
    Task<VirtualPlace?> GetByPlaceIdAsync(int placeId, bool includeDeleted = false);

    Task<VirtualPlace?> CreateForPlaceAsync(int placeId);

    Task<bool> DeleteForPlaceAsync(int placeId);
}
