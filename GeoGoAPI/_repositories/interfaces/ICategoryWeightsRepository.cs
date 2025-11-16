using GeoGoAPI._models.entities;

namespace GeoGoAPI._repositories.interfaces;

public interface ICategoryWeightsRepository
{
    Task<List<CategoryWeights>> GetByTwinIdAsync(int userTwinId);
    Task<CategoryWeights?> GetAsync(int userTwinId, int categoryId);

    Task AddAsync(CategoryWeights weight);
    void Update(CategoryWeights weight);

    Task SaveChangesAsync();
}
