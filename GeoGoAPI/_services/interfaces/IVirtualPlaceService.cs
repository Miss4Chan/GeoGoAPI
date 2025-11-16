using GeoGoAPI._models.entities;

namespace GeoGoAPI._services.interfaces;

public interface IVirtualPlaceService
{
    Task<List<VirtualPlace>> GetAllAsync(bool includeDeleted = false);
    Task<VirtualPlace?> GetByIdAsync(int id, bool includeDeleted = false);
    Task<VirtualPlace?> GetByPlaceIdAsync(int placeId, bool includeDeleted = false);

    /// <summary>
    /// Creates a VirtualPlace for the given Place if it does not exist.
    /// Returns null if the Place does not exist or is deleted.
    /// </summary>
    Task<VirtualPlace?> CreateForPlaceAsync(int placeId);

    /// <summary>
    /// Deletes the virtual place for a given placeId (if any).
    /// Returns true if deleted, false if not found.
    /// </summary>
    Task<bool> DeleteForPlaceAsync(int placeId);
}
