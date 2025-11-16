using GeoGoAPI._models;
using GeoGoAPI._models.entities;
using GeoGoAPI._repositories.interfaces;
using Microsoft.EntityFrameworkCore;

namespace GeoGoAPI._repositories.implementation;

public class CategoryWeightsRepository(GeoGoDbContext context) : ICategoryWeightsRepository
{
    public async Task<List<CategoryWeights>> GetByTwinIdAsync(int userTwinId)
    {
        return await context
            .CategoryWeights.Include(cw => cw.Category)
            .Where(cw => cw.UserTwinId == userTwinId)
            .ToListAsync();
    }

    public async Task<CategoryWeights?> GetAsync(int userTwinId, int categoryId)
    {
        return await context
            .CategoryWeights.Include(cw => cw.Category)
            .FirstOrDefaultAsync(cw => cw.UserTwinId == userTwinId && cw.CategoryId == categoryId);
    }

    public async Task AddAsync(CategoryWeights weight)
    {
        await context.CategoryWeights.AddAsync(weight);
    }

    public void Update(CategoryWeights weight)
    {
        context.CategoryWeights.Update(weight);
    }

    public async Task SaveChangesAsync() => await context.SaveChangesAsync();
}
