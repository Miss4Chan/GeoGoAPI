using GeoGoAPI._models;
using GeoGoAPI._models.entities;
using GeoGoAPI._repositories.interfaces;
using Microsoft.EntityFrameworkCore;

namespace GeoGoAPI._repositories.implementation;

public class CategoryRepository(GeoGoDbContext context) : ICategoryRepository
{
    public async Task<List<Category>> GetAllAsync() =>
        await context.Categories.OrderBy(c => c.Name).ToListAsync();

    public async Task<Category?> GetByIdAsync(int id) =>
        await context.Categories.FirstOrDefaultAsync(c => c.Id == id);

    public async Task<bool> ExistsByNameAsync(string name) =>
        await context.Categories.AnyAsync(c => c.Name.ToLower() == name.ToLower());

    public async Task AddAsync(Category category) => await context.Categories.AddAsync(category);

    public void Update(Category category) => context.Categories.Update(category);

    public void Delete(Category category) => context.Categories.Remove(category);

    public async Task<bool> IsInUseAsync(int categoryId) =>
        await context.Places.IgnoreQueryFilters().AnyAsync(p => p.CategoryId == categoryId);

    public async Task SaveChangesAsync() => await context.SaveChangesAsync();
}
