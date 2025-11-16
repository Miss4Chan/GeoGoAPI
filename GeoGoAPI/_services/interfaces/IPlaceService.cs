using GeoGoAPI._models.dtos.places;

namespace GeoGoAPI._services.interfaces;

public interface IPlaceService
{
    Task<List<PlaceDto>> GetAllAsync(bool includeDeleted = false);
    Task<PlaceDto?> GetByIdAsync(int id, bool includeDeleted = false);
    Task<PlaceDto> CreateAsync(CreatePlaceDto dto);
    Task<PlaceDto?> UpdateAsync(int id, UpdatePlaceDto dto);
    Task<bool> SoftDeleteAsync(int id);
    Task<bool> RestoreAsync(int id);
}
