using GeoGoAPI._models.entities;
using GeoGoAPI._repositories.interfaces;
using GeoGoAPI._services.interfaces;

namespace GeoGoAPI._services.implementations;

public class VirtualObjectService(
    IVirtualObjectRepository virtualObjectRepository,
    IVirtualPlaceRepository virtualPlaceRepository
) : IVirtualObjectService
{
    public async Task<VirtualObject?> GetByIdAsync(int id)
    {
        return await virtualObjectRepository.GetByIdAsync(id);
    }

    public async Task<List<VirtualObject>> GetByVirtualPlaceIdAsync(int virtualPlaceId)
    {
        return await virtualObjectRepository.GetByVirtualPlaceIdAsync(virtualPlaceId);
    }

    public async Task<VirtualObject?> CreateAsync(
        int virtualPlaceId,
        string name,
        string modelUrl,
        string modelUrlTexture,
        float px,
        float py,
        float pz,
        float rx,
        float ry,
        float rz,
        float sx,
        float sy,
        float sz,
        string? textDisplayed,
        string? stepsJson
    )
    {
        // Ensure the virtual place exists (and is not filtered out)
        var vp = await virtualPlaceRepository.GetByIdAsync(virtualPlaceId, includeDeleted: false);
        if (vp is null)
            return null;

        var vo = new VirtualObject
        {
            VirtualPlaceId = virtualPlaceId,
            Name = name,
            ModelUrl = modelUrl,
            ModelUrlTexture = modelUrlTexture,
            PX = px,
            PY = py,
            PZ = pz,
            RX = rx,
            RY = ry,
            RZ = rz,
            SX = sx,
            SY = sy,
            SZ = sz,
            TextDisplayed = textDisplayed,
            StepsJson = stepsJson,
        };

        await virtualObjectRepository.AddAsync(vo);
        await virtualObjectRepository.SaveChangesAsync();

        return vo;
    }

    public async Task<VirtualObject?> UpdateAsync(VirtualObject updated)
    {
        var existing = await virtualObjectRepository.GetByIdAsync(updated.Id);
        if (existing is null)
            return null;

        existing.Name = updated.Name;
        existing.ModelUrl = updated.ModelUrl;
        existing.ModelUrlTexture = updated.ModelUrlTexture;

        existing.PX = updated.PX;
        existing.PY = updated.PY;
        existing.PZ = updated.PZ;

        existing.RX = updated.RX;
        existing.RY = updated.RY;
        existing.RZ = updated.RZ;

        existing.SX = updated.SX;
        existing.SY = updated.SY;
        existing.SZ = updated.SZ;

        existing.TextDisplayed = updated.TextDisplayed;
        existing.StepsJson = updated.StepsJson;

        virtualObjectRepository.Update(existing);
        await virtualObjectRepository.SaveChangesAsync();

        return existing;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var existing = await virtualObjectRepository.GetByIdAsync(id);
        if (existing is null)
            return false;

        virtualObjectRepository.Delete(existing);
        await virtualObjectRepository.SaveChangesAsync();
        return true;
    }
}
