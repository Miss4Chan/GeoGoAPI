using System.Security.Claims;
using GeoGoAPI._models.dtos.interactions;
using GeoGoAPI._models.dtos.twin;
using GeoGoAPI._services.interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace GeoGoAPI._controllers;

[Authorize]
[SwaggerTag("Endpoints for logging user interactions and updating the digital twin.")]
public class InteractionsController(
    IInteractionProcessorService processorService,
    IInteractionEventService interactionEventService
) : BaseApiController
{
    /// <summary>
    /// Processes a user interaction with a place / virtual object,
    /// updates likes + category weights, and returns updated digital twin profile.
    /// </summary>
    [HttpPost("process")]
    [SwaggerOperation(
        Summary = "Process a user interaction",
        Description = "Logs an interaction, updates PlaceLikes + CategoryWeights, and returns the updated digital twin profile."
    )]
    [SwaggerResponse(200, "Interaction processed", typeof(DigitalTwinProfileDto))]
    [SwaggerResponse(400, "Invalid payload")]
    [SwaggerResponse(401, "User not authenticated")]
    [SwaggerResponse(404, "Twin or place not found")]
    public async Task<ActionResult<DigitalTwinProfileDto>> ProcessInteraction(
        [FromBody] InteractionRequestDto dto
    )
    {
        if (dto is null)
            return BadRequest("Interaction payload is required.");

        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(userIdStr) || !int.TryParse(userIdStr, out var userId))
            return Unauthorized("Invalid or missing user id claim.");

        var result = await processorService.ProcessInteractionAsync(userId, dto);
        if (result is null)
            return NotFound("Could not process interaction (user twin or place not found).");

        return Ok(result);
    }

    [HttpGet("history")]
    [SwaggerOperation(
        Summary = "Get user interaction history",
        Description = "Returns the last 30 interactions for the authenticated user with place, category, and object details."
    )]
    [SwaggerResponse(200, "History retrieved", typeof(List<InteractionHistoryDto>))]
    [SwaggerResponse(401, "User not authenticated")]
    public async Task<ActionResult<List<InteractionHistoryDto>>> GetInteractionHistory()
    {
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(userIdStr) || !int.TryParse(userIdStr, out var userId))
            return Unauthorized("Invalid or missing user id claim.");

        var history = await interactionEventService.GetUserHistoryAsync(userId, limit: 30);
        return Ok(history);
    }
}
