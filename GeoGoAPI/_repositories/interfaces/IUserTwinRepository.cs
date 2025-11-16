using GeoGoAPI._models.entities;

namespace GeoGoAPI._repositories.interfaces;

public interface IUserTwinRepository
{
    Task<UserTwin?> GetByIdAsync(int id);
    Task<UserTwin?> GetByUserIdAsync(int userId);
    Task<UserTwin?> GetTwinWithCategoryWeightsAsync(int userId);
    Task SaveChangesAsync();
}
