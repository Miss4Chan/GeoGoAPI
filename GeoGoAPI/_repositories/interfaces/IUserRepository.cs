using GeoGoAPI._models.entities;

namespace GeoGoAPI._repositories.interfaces;

public interface IUserRepository
{
    Task<bool> UserExistsAsync(string username);
    Task<AppUser?> GetByUserNameAsync(string username);
    void Add(AppUser user);
    Task SaveChangesAsync();
}
