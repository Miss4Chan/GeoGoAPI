using GeoGoAPI._models.dtos.twin;
using GeoGoAPI._models.entities;

namespace GeoGoAPI._services.interfaces;

public interface IUserTwinService
{
    Task<UserTwin?> GetTwinByIdAsync(int twinId);
    Task<UserTwin?> GetTwinByUserIdAsync(int userId);
    Task<DigitalTwinProfileDto?> GetPreferenceProfileAsync(int userId);
}
