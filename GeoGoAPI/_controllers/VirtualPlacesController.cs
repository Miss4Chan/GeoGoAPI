using GeoGoAPI._models.entities;
using GeoGoAPI._services.interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace GeoGoAPI._controllers;

[Authorize]
// Mobile can read, backoffice can create/delete mappings.
[SwaggerTag("Virtual places (AR spaces) linked to physical places. Mobile reads, admin manages.")]
public class VirtualPlacesController(IVirtualPlaceService virtualPlaceService) : BaseApiController
{
    [HttpGet]
    [SwaggerOperation(
        Summary = "Get all virtual places",
        Description = "Typically used by tools/admin. Includes the linked physical place."
    )]
    [SwaggerResponse(200, "List of virtual places", typeof(List<VirtualPlace>))]
    public async Task<ActionResult<List<VirtualPlace>>> GetAll(
        [FromQuery] bool includeDeleted = false
    )
    {
        var list = await virtualPlaceService.GetAllAsync(includeDeleted);
        return Ok(list);
    }

    [HttpGet("{id:int}")]
    [SwaggerOperation(Summary = "Get virtual place by id")]
    [SwaggerResponse(200, "Virtual place found", typeof(VirtualPlace))]
    [SwaggerResponse(404, "Virtual place not found")]
    public async Task<ActionResult<VirtualPlace>> GetById(
        int id,
        [FromQuery] bool includeDeleted = false
    )
    {
        var vp = await virtualPlaceService.GetByIdAsync(id, includeDeleted);
        return vp is null ? NotFound() : Ok(vp);
    }

    [HttpGet("by-place/{placeId:int}")]
    [SwaggerOperation(
        Summary = "Get virtual place by physical place id",
        Description = "Can be used by the mobile app to check if a place has AR content."
    )]
    [SwaggerResponse(200, "Virtual place found", typeof(VirtualPlace))]
    [SwaggerResponse(404, "Virtual place not found")]
    public async Task<ActionResult<VirtualPlace>> GetByPlaceId(
        int placeId,
        [FromQuery] bool includeDeleted = false
    )
    {
        var vp = await virtualPlaceService.GetByPlaceIdAsync(placeId, includeDeleted);
        return vp is null ? NotFound() : Ok(vp);
    }

    /// <summary>
    /// Creates a VirtualPlace for the given physical place if it does not already exist.
    /// </summary>
    [HttpPost("for-place/{placeId:int}")]
    [SwaggerOperation(
        Summary = "Create virtual place for a physical place",
        Description = "Backoffice endpoint. Mobile app should not call this."
    )]
    [SwaggerResponse(
        201,
        "Virtual place created or returned if already existed",
        typeof(VirtualPlace)
    )]
    [SwaggerResponse(404, "Place not found or is deleted")]
    public async Task<ActionResult<VirtualPlace>> CreateForPlace(int placeId)
    {
        var vp = await virtualPlaceService.CreateForPlaceAsync(placeId);
        if (vp is null)
            return NotFound("Place not found or is deleted.");

        return CreatedAtAction(nameof(GetById), new { id = vp.Id }, vp);
    }

    /// <summary>
    /// Deletes the VirtualPlace associated with a given physical place.
    /// </summary>
    [HttpDelete("for-place/{placeId:int}")]
    [SwaggerOperation(
        Summary = "Delete virtual place for a physical place",
        Description = "Backoffice endpoint. Mobile app should not call this."
    )]
    [SwaggerResponse(204, "Deleted successfully")]
    [SwaggerResponse(404, "Virtual place not found")]
    public async Task<IActionResult> DeleteForPlace(int placeId)
    {
        var ok = await virtualPlaceService.DeleteForPlaceAsync(placeId);
        return ok ? NoContent() : NotFound();
    }
}
