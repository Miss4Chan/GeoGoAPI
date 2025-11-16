using GeoGoAPI._models.dtos.places;
using GeoGoAPI._services.interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace GeoGoAPI._controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
[SwaggerTag("Admin endpoints for managing physical places (with automatic VirtualPlaces).")]
public class PlacesController(IPlaceService service) : ControllerBase
{
    [HttpGet]
    [SwaggerOperation(
        Summary = "Get all places",
        Description = "Use includeDeleted=true to also see soft-deleted places."
    )]
    [SwaggerResponse(200, "List of places", typeof(List<PlaceDto>))]
    public async Task<ActionResult<List<PlaceDto>>> GetAll(
        [FromQuery] bool includeDeleted = false
    ) => Ok(await service.GetAllAsync(includeDeleted));

    [HttpGet("{id:int}")]
    [SwaggerOperation(Summary = "Get place by id")]
    [SwaggerResponse(200, "Place found", typeof(PlaceDto))]
    [SwaggerResponse(404, "Place not found")]
    public async Task<ActionResult<PlaceDto>> GetById(
        int id,
        [FromQuery] bool includeDeleted = false
    )
    {
        var result = await service.GetByIdAsync(id, includeDeleted);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    [SwaggerOperation(
        Summary = "Create a new place",
        Description = "Also auto-creates an associated VirtualPlace for AR content."
    )]
    [SwaggerResponse(201, "Place created", typeof(PlaceDto))]
    [SwaggerResponse(400, "Category does not exist")]
    public async Task<ActionResult<PlaceDto>> Create([FromBody] CreatePlaceDto dto)
    {
        var created = await service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    [SwaggerOperation(Summary = "Update an existing place")]
    [SwaggerResponse(200, "Place updated", typeof(PlaceDto))]
    [SwaggerResponse(404, "Place not found")]
    public async Task<ActionResult<PlaceDto>> Update(int id, [FromBody] UpdatePlaceDto dto)
    {
        var result = await service.UpdateAsync(id, dto);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpDelete("{id:int}")]
    [SwaggerOperation(
        Summary = "Soft-delete a place",
        Description = "Marks the place as deleted and hides it (and its AR content) from queries."
    )]
    [SwaggerResponse(204, "Soft-deleted successfully")]
    [SwaggerResponse(404, "Place not found")]
    public async Task<IActionResult> SoftDelete(int id) =>
        await service.SoftDeleteAsync(id) ? NoContent() : NotFound();

    [HttpPost("{id:int}/restore")]
    [SwaggerOperation(Summary = "Restore a soft-deleted place")]
    [SwaggerResponse(204, "Restored successfully")]
    [SwaggerResponse(404, "Place not found")]
    public async Task<IActionResult> Restore(int id) =>
        await service.RestoreAsync(id) ? NoContent() : NotFound();
}
