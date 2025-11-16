using GeoGoAPI._models;
using GeoGoAPI._models.entities;
using GeoGoAPI._repositories.interfaces;
using Microsoft.EntityFrameworkCore;

namespace GeoGoAPI._repositories.implementation;

public class PlaceRepository(GeoGoDbContext context) : IPlaceRepository
{
    public async Task<List<Place>> GetAllAsync(bool includeDeleted = false)
    {
        var query = context.Places.AsQueryable();

        if (includeDeleted)
            query = query.IgnoreQueryFilters();

        return await query
            .Include(p => p.Category)
            .Include(p => p.VirtualPlace)
            .OrderBy(p => p.Name)
            .ToListAsync();
    }

    public async Task<Place?> GetByIdAsync(int id, bool includeDeleted = false)
    {
        var query = context.Places.AsQueryable();
        if (includeDeleted)
            query = query.IgnoreQueryFilters();

        return await query
            .Include(p => p.Category)
            .Include(p => p.VirtualPlace)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task AddAsync(Place place)
    {
        await context.Places.AddAsync(place);
    }

    public void Update(Place place)
    {
        context.Places.Update(place);
    }

    public async Task SoftDeleteAsync(Place place)
    {
        place.IsDeleted = true;
        place.DeletedAtUtc = DateTime.UtcNow;
        await Task.CompletedTask;
    }

    public async Task RestoreAsync(Place place)
    {
        place.IsDeleted = false;
        place.DeletedAtUtc = null;
        await Task.CompletedTask;
    }

    public async Task<bool> CategoryExistsAsync(int categoryId) =>
        await context.Categories.AnyAsync(c => c.Id == categoryId);

    public async Task SaveChangesAsync() => await context.SaveChangesAsync();
}
