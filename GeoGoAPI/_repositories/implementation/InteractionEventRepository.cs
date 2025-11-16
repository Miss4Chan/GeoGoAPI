using GeoGoAPI._models;
using GeoGoAPI._models.entities;
using GeoGoAPI._repositories.interfaces;
using Microsoft.EntityFrameworkCore;

namespace GeoGoAPI._repositories.implementation;

public class InteractionEventRepository(GeoGoDbContext context) : IInteractionEventRepository
{
    public async Task<InteractionEvent?> GetByIdAsync(int id)
    {
        return await context
            .InteractionEvents.Include(ie => ie.UserTwin)
            .Include(ie => ie.PlaceLike)
            .ThenInclude(pl => pl!.Place)
            .Include(ie => ie.VirtualPlace)
            .Include(ie => ie.VirtualObject)
            .FirstOrDefaultAsync(ie => ie.Id == id);
    }

    public async Task<List<InteractionEvent>> GetByTwinAsync(int userTwinId)
    {
        return await context
            .InteractionEvents.Where(ie => ie.UserTwinId == userTwinId)
            .OrderByDescending(ie => ie.Timestamp)
            .ToListAsync();
    }

    public async Task<List<InteractionEvent>> GetByPlaceAsync(int placeId)
    {
        return await context
            .InteractionEvents.Where(ie => ie.PlaceId == placeId)
            .OrderByDescending(ie => ie.Timestamp)
            .ToListAsync();
    }

    public async Task<List<InteractionEvent>> GetByVirtualPlaceAsync(int virtualPlaceId)
    {
        return await context
            .InteractionEvents.Where(ie => ie.VirtualPlaceId == virtualPlaceId)
            .OrderByDescending(ie => ie.Timestamp)
            .ToListAsync();
    }

    public async Task<List<InteractionEvent>> GetByVirtualObjectAsync(int virtualObjectId)
    {
        return await context
            .InteractionEvents.Where(ie => ie.VirtualObjectId == virtualObjectId)
            .OrderByDescending(ie => ie.Timestamp)
            .ToListAsync();
    }

    public async Task AddAsync(InteractionEvent interactionEvent)
    {
        await context.InteractionEvents.AddAsync(interactionEvent);
    }

    public async Task SaveChangesAsync() => await context.SaveChangesAsync();
}
