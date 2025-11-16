using GeoGoAPI._models.entities;

namespace GeoGoAPI._repositories.interfaces;

public interface ICategoryRepository
{
    Task<List<Category>> GetAllAsync();
    Task<Category?> GetByIdAsync(int id);
    Task<bool> ExistsByNameAsync(string name);
    Task AddAsync(Category category);
    void Update(Category category);
    void Delete(Category category);
    Task<bool> IsInUseAsync(int categoryId); // are any places using this category? like is there any with it?
    Task SaveChangesAsync();
}
