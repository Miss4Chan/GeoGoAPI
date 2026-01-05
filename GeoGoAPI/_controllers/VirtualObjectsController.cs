using GeoGoAPI._models.dtos.virtualobjects;
using GeoGoAPI._models.entities;
using GeoGoAPI._services.interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace GeoGoAPI._controllers;

[Authorize]
// Note: both the mobile app (for GETs) and the admin (for POST/PUT/DELETE) use this.
[SwaggerTag("Virtual objects (3D AR models) for a virtual place. Mobile reads, admin manages.")]
public class VirtualObjectsController(IVirtualObjectService virtualObjectService)
    : BaseApiController
{
    [HttpGet("{id:int}")]
    [SwaggerOperation(
        Summary = "Get virtual object by id",
        Description = "Used by mobile or tools to retrieve a specific 3D object."
    )]
    [SwaggerResponse(200, "Virtual object found", typeof(VirtualObjectDto))]
    [SwaggerResponse(404, "Virtual object not found")]
    public async Task<ActionResult<VirtualObjectDto>> GetById(int id)
    {
        var vo = await virtualObjectService.GetByIdAsync(id);
        return vo is null ? NotFound() : Ok(MapToDto(vo));
    }

    [HttpGet("by-virtualplace/{virtualPlaceId:int}")]
    [SwaggerOperation(
        Summary = "Get all virtual objects for a virtual place",
        Description = "This is typically called by the mobile app to load AR content for a place."
    )]
    [SwaggerResponse(200, "List of virtual objects", typeof(List<VirtualObjectDto>))]
    public async Task<ActionResult<List<VirtualObjectDto>>> GetByVirtualPlaceId(int virtualPlaceId)
    {
        var list = await virtualObjectService.GetByVirtualPlaceIdAsync(virtualPlaceId);
        return Ok(list.Select(MapToDto).ToList());
    }

    [HttpPost]
    [SwaggerOperation(
        Summary = "Create a new virtual object",
        Description = "Backoffice/management endpoint to attach a 3D model to a virtual place."
    )]
    [SwaggerResponse(201, "Virtual object created", typeof(VirtualObjectDto))]
    [SwaggerResponse(404, "Virtual place not found")]
    public async Task<ActionResult<VirtualObjectDto>> Create([FromBody] CreateVirtualObjectDto dto)
    {
        var vo = await virtualObjectService.CreateAsync(
            virtualPlaceId: dto.VirtualPlaceId,
            name: dto.Name,
            modelUrl: dto.ModelUrl,
            modelUrlTexture: dto.ModelUrlTexture,
            px: dto.PX,
            py: dto.PY,
            pz: dto.PZ,
            rx: dto.RX,
            ry: dto.RY,
            rz: dto.RZ,
            sx: dto.SX,
            sy: dto.SY,
            sz: dto.SZ,
            textDisplayed: dto.TextDisplayed,
            stepsJson: dto.StepsJson
        );

        if (vo is null)
            return NotFound("Virtual place not found.");

        var resultDto = MapToDto(vo);
        return CreatedAtAction(nameof(GetById), new { id = vo.Id }, resultDto);
    }

    [HttpPut("{id:int}")]
    [SwaggerOperation(
        Summary = "Update a virtual object",
        Description = "Backoffice/management endpoint to update 3D object properties or transform."
    )]
    [SwaggerResponse(200, "Virtual object updated", typeof(VirtualObjectDto))]
    [SwaggerResponse(404, "Virtual object not found")]
    public async Task<ActionResult<VirtualObjectDto>> Update(
        int id,
        [FromBody] UpdateVirtualObjectDto dto
    )
    {
        var updated = await virtualObjectService.UpdateAsync(
            new VirtualObject
            {
                Id = id,
                Name = dto.Name,
                ModelUrl = dto.ModelUrl,
                ModelUrlTexture = dto.ModelUrlTexture,
                PX = dto.PX,
                PY = dto.PY,
                PZ = dto.PZ,
                RX = dto.RX,
                RY = dto.RY,
                RZ = dto.RZ,
                SX = dto.SX,
                SY = dto.SY,
                SZ = dto.SZ,
                TextDisplayed = dto.TextDisplayed,
                StepsJson = dto.StepsJson,
            }
        );

        return updated is null ? NotFound() : Ok(MapToDto(updated));
    }

    [HttpDelete("{id:int}")]
    [SwaggerOperation(
        Summary = "Delete a virtual object",
        Description = "Backoffice/management endpoint to remove AR content."
    )]
    [SwaggerResponse(204, "Deleted successfully")]
    [SwaggerResponse(404, "Virtual object not found")]
    public async Task<IActionResult> Delete(int id)
    {
        var ok = await virtualObjectService.DeleteAsync(id);
        return ok ? NoContent() : NotFound();
    }

    private static VirtualObjectDto MapToDto(VirtualObject vo) =>
        new()
        {
            Id = vo.Id,
            VirtualPlaceId = vo.VirtualPlaceId,
            Name = vo.Name,
            ModelUrl = vo.ModelUrl,
            ModelUrlTexture = vo.ModelUrlTexture,
            PX = vo.PX,
            PY = vo.PY,
            PZ = vo.PZ,
            RX = vo.RX,
            RY = vo.RY,
            RZ = vo.RZ,
            SX = vo.SX,
            SY = vo.SY,
            SZ = vo.SZ,
            TextDisplayed = vo.TextDisplayed,
            StepsJson = vo.StepsJson,
        };
}
