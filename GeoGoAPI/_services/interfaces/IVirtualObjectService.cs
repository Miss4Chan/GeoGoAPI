using GeoGoAPI._models.entities;

namespace GeoGoAPI._services.interfaces;

public interface IVirtualObjectService
{
    Task<VirtualObject?> GetByIdAsync(int id);
    Task<List<VirtualObject>> GetByVirtualPlaceIdAsync(int virtualPlaceId);

    Task<VirtualObject?> CreateAsync(
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
        string? textDisplayed
    );

    Task<VirtualObject?> UpdateAsync(VirtualObject updated);
    Task<bool> DeleteAsync(int id);
}
