using GeoGoAPI._models.entities;

namespace GeoGoAPI._services.interfaces;

public interface ICategoryWeightsService
{
    Task<IReadOnlyList<CategoryWeights>> GetByTwinIdAsync(int twinId);

    Task<CategoryWeights> SetWeightAsync(int twinId, int categoryId, double score);

    Task<CategoryWeights> IncrementWeightAsync(int twinId, int categoryId, double delta);
}
