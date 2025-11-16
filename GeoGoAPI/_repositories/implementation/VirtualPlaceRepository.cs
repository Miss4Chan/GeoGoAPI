using GeoGoAPI._models;
using GeoGoAPI._models.entities;
using GeoGoAPI._repositories.interfaces;
using Microsoft.EntityFrameworkCore;

namespace GeoGoAPI._repositories.implementation;

public class VirtualPlaceRepository(GeoGoDbContext context) : IVirtualPlaceRepository
{
    public async Task<List<VirtualPlace>> GetAllAsync(bool includeDeleted = false)
    {
        var query = context.VirtualPlaces.AsQueryable();

        if (includeDeleted)
            query = query.IgnoreQueryFilters();

        return await query
            .Include(vp => vp.Place)
            .Include(vp => vp.VirtualObjects)
            .OrderBy(vp => vp.Place!.Name)
            .ToListAsync();
    }

    public async Task<VirtualPlace?> GetByIdAsync(int id, bool includeDeleted = false)
    {
        var query = context.VirtualPlaces.AsQueryable();

        if (includeDeleted)
            query = query.IgnoreQueryFilters();

        return await query
            .Include(vp => vp.Place)
            .Include(vp => vp.VirtualObjects)
            .FirstOrDefaultAsync(vp => vp.Id == id);
    }

    public async Task<VirtualPlace?> GetByPlaceIdAsync(int placeId, bool includeDeleted = false)
    {
        var query = context.VirtualPlaces.AsQueryable();

        if (includeDeleted)
            query = query.IgnoreQueryFilters();

        return await query
            .Include(vp => vp.Place)
            .Include(vp => vp.VirtualObjects)
            .FirstOrDefaultAsync(vp => vp.PlaceId == placeId);
    }

    public async Task AddAsync(VirtualPlace virtualPlace)
    {
        await context.VirtualPlaces.AddAsync(virtualPlace);
    }

    public void Update(VirtualPlace virtualPlace)
    {
        context.VirtualPlaces.Update(virtualPlace);
    }

    public void Delete(VirtualPlace virtualPlace)
    {
        context.VirtualPlaces.Remove(virtualPlace);
    }

    public async Task SaveChangesAsync() => await context.SaveChangesAsync();
}
