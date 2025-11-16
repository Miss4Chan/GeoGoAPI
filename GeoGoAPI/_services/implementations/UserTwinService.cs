using GeoGoAPI._models.dtos.twin;
using GeoGoAPI._models.entities;
using GeoGoAPI._repositories.interfaces;
using GeoGoAPI._services.interfaces;

namespace GeoGoAPI._services.implementations;

public class UserTwinService(IUserTwinRepository repo) : IUserTwinService
{
    public async Task<UserTwin?> GetTwinByIdAsync(int twinId)
    {
        return await repo.GetByIdAsync(twinId);
    }

    public async Task<UserTwin?> GetTwinByUserIdAsync(int userId)
    {
        return await repo.GetByUserIdAsync(userId);
    }

    public async Task<DigitalTwinProfileDto?> GetPreferenceProfileAsync(int userId)
    {
        var twin = await repo.GetTwinWithCategoryWeightsAsync(userId);
        if (twin is null)
            return null;

        var raw = twin
            .CategoryWeights.Select(cw => new RawWeightDto
            {
                CategoryId = cw.CategoryId,
                CategoryName = cw.Category!.Name,
                Score = cw.Score,
            })
            .ToList();

        var total = raw.Sum(r => r.Score);
        var normalized = raw.Select(r => new NormalizedWeightDto
            {
                CategoryId = r.CategoryId,
                CategoryName = r.CategoryName,
                NormalizedScore = total > 0 ? Math.Round(r.Score / total, 4) : 0,
            })
            .ToList();

        return new DigitalTwinProfileDto
        {
            UserTwinId = twin.Id,
            RawWeights = raw,
            NormalizedWeights = normalized,
        };
    }
}
