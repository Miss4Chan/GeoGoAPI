using GeoGoAPI._models.dtos.places;
using GeoGoAPI._services.interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeoGoAPI._controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class PlacesController(IPlaceService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<PlaceDto>>> GetAll(
        [FromQuery] bool includeDeleted = false
    ) => Ok(await service.GetAllAsync(includeDeleted));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PlaceDto>> GetById(
        int id,
        [FromQuery] bool includeDeleted = false
    )
    {
        var result = await service.GetByIdAsync(id, includeDeleted);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<PlaceDto>> Create([FromBody] CreatePlaceDto dto)
    {
        var created = await service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<PlaceDto>> Update(int id, [FromBody] UpdatePlaceDto dto)
    {
        var result = await service.UpdateAsync(id, dto);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> SoftDelete(int id) =>
        await service.SoftDeleteAsync(id) ? NoContent() : NotFound();

    [HttpPost("{id:int}/restore")]
    public async Task<IActionResult> Restore(int id) =>
        await service.RestoreAsync(id) ? NoContent() : NotFound();
}
