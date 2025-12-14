using GeoGoAPI._models.dtos.places;
using GeoGoAPI._models.entities;
using GeoGoAPI._repositories.interfaces;
using GeoGoAPI._services.interfaces;

namespace GeoGoAPI._services.implementations;

public class PlaceService(IPlaceRepository repo) : IPlaceService
{
    public async Task<List<PlaceDto>> GetAllAsync(bool includeDeleted = false)
    {
        var list = await repo.GetAllAsync(includeDeleted);
        return list.Select(MapToDto).ToList();
    }

    public async Task<PlaceDto?> GetByIdAsync(int id, bool includeDeleted = false)
    {
        var place = await repo.GetByIdAsync(id, includeDeleted);
        return place is null ? null : MapToDto(place);
    }

    public async Task<PlaceDto> CreateAsync(CreatePlaceDto dto)
    {
        if (!await repo.CategoryExistsAsync(dto.CategoryId))
            throw new ArgumentException("Category does not exist", nameof(dto.CategoryId));

        var place = new Place
        {
            Name = dto.Name,
            Latitude = dto.Latitude,
            Longitude = dto.Longitude,
            CategoryId = dto.CategoryId,
            Active = dto.Active,
        };

        place.VirtualPlace = new VirtualPlace { Place = place };

        await repo.AddAsync(place);
        await repo.SaveChangesAsync();

        return MapToDto(place);
    }

    public async Task<PlaceDto?> UpdateAsync(int id, UpdatePlaceDto dto)
    {
        var place = await repo.GetByIdAsync(id, includeDeleted: true);
        if (place is null)
            return null;

        if (!await repo.CategoryExistsAsync(dto.CategoryId))
            throw new ArgumentException("Category does not exist", nameof(dto.CategoryId));

        place.Name = dto.Name;
        place.Latitude = dto.Latitude;
        place.Longitude = dto.Longitude;
        place.CategoryId = dto.CategoryId;
        place.Active = dto.Active;

        repo.Update(place);
        await repo.SaveChangesAsync();

        return MapToDto(place);
    }

    public async Task<bool> SoftDeleteAsync(int id)
    {
        var place = await repo.GetByIdAsync(id, includeDeleted: true);
        if (place is null)
            return false;

        if (place.IsDeleted)
            return true;

        await repo.SoftDeleteAsync(place);
        await repo.SaveChangesAsync();
        return true;
    }

    public async Task<bool> RestoreAsync(int id)
    {
        var place = await repo.GetByIdAsync(id, includeDeleted: true);
        if (place is null)
            return false;

        if (!place.IsDeleted)
            return true;

        await repo.RestoreAsync(place);
        await repo.SaveChangesAsync();
        return true;
    }

    private static PlaceDto MapToDto(Place p) =>
        new()
        {
            Id = p.Id,
            Name = p.Name,
            VirtualPlaceId = p.VirtualPlace!.Id,
            Latitude = p.Latitude,
            Longitude = p.Longitude,
            CategoryId = p.CategoryId,
            Active = p.Active,
            IsDeleted = p.IsDeleted,
            DeletedAtUtc = p.DeletedAtUtc,
        };
}
