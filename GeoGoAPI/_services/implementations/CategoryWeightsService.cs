using GeoGoAPI._models.entities;
using GeoGoAPI._repositories.interfaces;
using GeoGoAPI._services.interfaces;

namespace GeoGoAPI._services.implementations;

public class CategoryWeightsService(ICategoryWeightsRepository repo) : ICategoryWeightsService
{
    public async Task<IReadOnlyList<CategoryWeights>> GetByTwinIdAsync(int twinId)
    {
        return await repo.GetByTwinIdAsync(twinId);
    }

    public async Task<CategoryWeights> SetWeightAsync(int twinId, int categoryId, double score)
    {
        var cw = await repo.GetAsync(twinId, categoryId);

        if (cw is null)
        {
            cw = new CategoryWeights
            {
                UserTwinId = twinId,
                CategoryId = categoryId,
                Score = score,
            };
            await repo.AddAsync(cw);
        }
        else
        {
            cw.Score = score;
            repo.Update(cw);
        }

        await repo.SaveChangesAsync();
        return cw;
    }

    public async Task<CategoryWeights> IncrementWeightAsync(
        int twinId,
        int categoryId,
        double delta
    )
    {
        var cw = await repo.GetAsync(twinId, categoryId);

        if (cw is null)
        {
            cw = new CategoryWeights
            {
                UserTwinId = twinId,
                CategoryId = categoryId,
                Score = delta,
            };
            await repo.AddAsync(cw);
        }
        else
        {
            cw.Score += delta;
            repo.Update(cw);
        }

        await repo.SaveChangesAsync();
        return cw;
    }
}
