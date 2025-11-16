using GeoGoAPI._models.entities;

namespace GeoGoAPI._services.interfaces;

public interface IPlaceLikesService
{
    Task<PlaceLikes?> GetAsync(int twinId, int placeId);
    Task<IReadOnlyList<PlaceLikes>> GetByTwinIdAsync(int twinId);
    Task<IReadOnlyList<PlaceLikes>> GetByPlaceIdAsync(int placeId);

    /// <summary>
    /// Ensure a like row exists for (twin, place).
    /// If it already exists, returns it unchanged.
    /// If not, creates with initial Score.
    /// </summary>
    Task<PlaceLikes> CreateIfNotExistsAsync(int twinId, int placeId, double initialScore = 0);

    /// <summary>
    /// Increment the like score by delta (can be negative).
    /// Ensures the like row exists.
    /// </summary>
    Task<PlaceLikes> IncrementScoreAsync(int twinId, int placeId, double delta);

    /// <summary>
    /// Removes the like (unlike).
    /// Returns true if it existed and was removed.
    /// </summary>
    Task<bool> RemoveAsync(int twinId, int placeId);
}
