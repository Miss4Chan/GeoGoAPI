using GeoGoAPI._models.dtos.categories;
using GeoGoAPI._models.entities;
using GeoGoAPI._repositories.interfaces;
using GeoGoAPI._services.interfaces;

namespace GeoGoAPI._services.implementations;

public class CategoryService(ICategoryRepository repo) : ICategoryService
{
    public async Task<List<CategoryDto>> GetAllAsync()
    {
        var list = await repo.GetAllAsync();
        return [.. list.Select(MapToDto)];
    }

    public async Task<CategoryDto?> GetByIdAsync(int id)
    {
        var c = await repo.GetByIdAsync(id);
        return c is null ? null : MapToDto(c);
    }

    public async Task<CategoryDto> CreateAsync(CreateCategoryDto dto)
    {
        if (await repo.ExistsByNameAsync(dto.Name))
            throw new ArgumentException("Category name already exists", nameof(dto.Name));

        var c = new Category { Name = dto.Name.Trim() };
        await repo.AddAsync(c);
        await repo.SaveChangesAsync();
        return MapToDto(c);
    }

    public async Task<CategoryDto?> UpdateAsync(int id, UpdateCategoryDto dto)
    {
        var c = await repo.GetByIdAsync(id);
        if (c is null)
            return null;

        if (
            !c.Name.Equals(dto.Name, StringComparison.OrdinalIgnoreCase)
            && await repo.ExistsByNameAsync(dto.Name)
        )
            throw new ArgumentException("Category name already exists", nameof(dto.Name));

        c.Name = dto.Name.Trim();
        repo.Update(c);
        await repo.SaveChangesAsync();
        return MapToDto(c);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var c = await repo.GetByIdAsync(id);
        if (c is null)
            return false;

        if (await repo.IsInUseAsync(id))
            throw new InvalidOperationException(
                "Cannot delete a category that is in use by one or more places."
            );

        repo.Delete(c);
        await repo.SaveChangesAsync();
        return true;
    }

    private static CategoryDto MapToDto(Category c) => new() { Id = c.Id, Name = c.Name };
}
