using GeoGoAPI._models;
using GeoGoAPI._models.entities;
using GeoGoAPI._repositories.interfaces;
using Microsoft.EntityFrameworkCore;

namespace GeoGoAPI._repositories.implementation;

public class VirtualObjectRepository(GeoGoDbContext context) : IVirtualObjectRepository
{
    public async Task<VirtualObject?> GetByIdAsync(int id)
    {
        return await context
            .VirtualObjects.Include(vo => vo.VirtualPlace)
            .ThenInclude(vp => vp!.Place)
            .FirstOrDefaultAsync(vo => vo.Id == id);
    }

    public async Task<List<VirtualObject>> GetByVirtualPlaceIdAsync(int virtualPlaceId)
    {
        return await context
            .VirtualObjects.Include(vo => vo.VirtualPlace)
            .Where(vo => vo.VirtualPlaceId == virtualPlaceId)
            .ToListAsync();
    }

    public async Task AddAsync(VirtualObject virtualObject)
    {
        await context.VirtualObjects.AddAsync(virtualObject);
    }

    public void Update(VirtualObject virtualObject)
    {
        context.VirtualObjects.Update(virtualObject);
    }

    public void Delete(VirtualObject virtualObject)
    {
        context.VirtualObjects.Remove(virtualObject);
    }

    public async Task SaveChangesAsync() => await context.SaveChangesAsync();
}
