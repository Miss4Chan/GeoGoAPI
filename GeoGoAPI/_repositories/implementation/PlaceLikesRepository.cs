using GeoGoAPI._models;
using GeoGoAPI._models.entities;
using GeoGoAPI._repositories.interfaces;
using Microsoft.EntityFrameworkCore;

namespace GeoGoAPI._repositories.implementation;

public class PlaceLikesRepository(GeoGoDbContext context) : IPlaceLikesRepository
{
    public async Task<PlaceLikes?> GetAsync(int userTwinId, int placeId)
    {
        return await context
            .PlaceLikes.Include(pl => pl.Place)
            .ThenInclude(p => p!.Category)
            .Include(pl => pl.UserTwin)
            .FirstOrDefaultAsync(pl => pl.UserTwinId == userTwinId && pl.PlaceId == placeId);
    }

    // ! Ive left the ! inside since I know that a place will always have a category id its just that
    // ! the linter here doesnt know that since the virutal object is indeed optional
    public async Task<List<PlaceLikes>> GetByTwinIdAsync(int userTwinId)
    {
        return await context
            .PlaceLikes.Include(pl => pl.Place)
            .ThenInclude(p => p!.Category)
            .Where(pl => pl.UserTwinId == userTwinId)
            .ToListAsync();
    }

    public async Task<List<PlaceLikes>> GetByPlaceIdAsync(int placeId)
    {
        return await context
            .PlaceLikes.Include(pl => pl.UserTwin)
            .Where(pl => pl.PlaceId == placeId)
            .ToListAsync();
    }

    public async Task AddAsync(PlaceLikes like)
    {
        await context.PlaceLikes.AddAsync(like);
    }

    public void Update(PlaceLikes like)
    {
        context.PlaceLikes.Update(like);
    }

    public void Delete(PlaceLikes like)
    {
        context.PlaceLikes.Remove(like);
    }

    public async Task SaveChangesAsync() => await context.SaveChangesAsync();
}
