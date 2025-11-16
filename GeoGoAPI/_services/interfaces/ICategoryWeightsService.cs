using GeoGoAPI._models.entities;

namespace GeoGoAPI._services.interfaces;

public interface ICategoryWeightsService
{
    Task<IReadOnlyList<CategoryWeights>> GetByTwinIdAsync(int twinId);

    /// <summary>
    /// Sets the absolute score for a given (twin, category).
    /// Creates the row if it does not exist.
    /// </summary>
    Task<CategoryWeights> SetWeightAsync(int twinId, int categoryId, double score);

    /// <summary>
    /// Adds delta to the existing score (can be negative).
    /// Creates the row with the given delta if it does not exist.
    /// </summary>
    Task<CategoryWeights> IncrementWeightAsync(int twinId, int categoryId, double delta);
}
