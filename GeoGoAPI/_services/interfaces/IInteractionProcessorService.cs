using GeoGoAPI._models.dtos.interactions;
using GeoGoAPI._models.dtos.twin;

namespace GeoGoAPI._services.interfaces;

public interface IInteractionProcessorService
{
    Task<DigitalTwinProfileDto?> ProcessInteractionAsync(int userId, InteractionRequestDto request);
}
