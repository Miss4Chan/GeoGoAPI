using GeoGoAPI._models.entities;
using GeoGoAPI._repositories.interfaces;
using GeoGoAPI._services.interfaces;

namespace GeoGoAPI._services.implementations;

public class VirtualPlaceService(
    IVirtualPlaceRepository virtualPlaceRepository,
    IPlaceRepository placeRepository
) : IVirtualPlaceService
{
    public async Task<List<VirtualPlace>> GetAllAsync(bool includeDeleted = false)
    {
        return await virtualPlaceRepository.GetAllAsync(includeDeleted);
    }

    public async Task<VirtualPlace?> GetByIdAsync(int id, bool includeDeleted = false)
    {
        return await virtualPlaceRepository.GetByIdAsync(id, includeDeleted);
    }

    public async Task<VirtualPlace?> GetByPlaceIdAsync(int placeId, bool includeDeleted = false)
    {
        return await virtualPlaceRepository.GetByPlaceIdAsync(placeId, includeDeleted);
    }

    public async Task<VirtualPlace?> CreateForPlaceAsync(int placeId)
    {
        // Make sure the place exists and is not soft-deleted
        var place = await placeRepository.GetByIdAsync(placeId, includeDeleted: false);
        if (place is null)
            return null;

        // Ensure we don't create duplicates (1:1)
        var existing = await virtualPlaceRepository.GetByPlaceIdAsync(
            placeId,
            includeDeleted: true
        );
        if (existing is not null)
            return existing;

        var vp = new VirtualPlace { PlaceId = placeId, Place = place };

        await virtualPlaceRepository.AddAsync(vp);
        await virtualPlaceRepository.SaveChangesAsync();

        return vp;
    }

    public async Task<bool> DeleteForPlaceAsync(int placeId)
    {
        var existing = await virtualPlaceRepository.GetByPlaceIdAsync(
            placeId,
            includeDeleted: true
        );
        if (existing is null)
            return false;

        virtualPlaceRepository.Delete(existing);
        await virtualPlaceRepository.SaveChangesAsync();
        return true;
    }
}
