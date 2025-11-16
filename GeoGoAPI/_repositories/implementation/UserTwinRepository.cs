using GeoGoAPI._models;
using GeoGoAPI._models.entities;
using GeoGoAPI._repositories.interfaces;
using Microsoft.EntityFrameworkCore;

namespace GeoGoAPI._repositories.implementation;

public class UserTwinRepository(GeoGoDbContext context) : IUserTwinRepository
{
    public async Task<UserTwin?> GetByIdAsync(int id)
    {
        return await context
            .UserTwins.Include(t => t.CategoryWeights)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<UserTwin?> GetByUserIdAsync(int userId)
    {
        return await context
            .UserTwins.Include(t => t.CategoryWeights)
            .FirstOrDefaultAsync(t => t.UserId == userId);
    }

    public async Task<UserTwin?> GetTwinWithCategoryWeightsAsync(int userId)
    {
        return await context
            .UserTwins.Include(t => t.CategoryWeights)
            .ThenInclude(cw => cw.Category)
            .FirstOrDefaultAsync(t => t.UserId == userId);
    }

    public async Task SaveChangesAsync() => await context.SaveChangesAsync();
}
