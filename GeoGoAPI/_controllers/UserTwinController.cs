using System.Security.Claims;
using GeoGoAPI._models.dtos.twin;
using GeoGoAPI._services.interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace GeoGoAPI._controllers;

[Authorize]
[SwaggerTag("Endpoints for accessing the user's digital twin (preferences/profile).")]
public class UserTwinController(IUserTwinService userTwinService) : BaseApiController
{
    /// <summary>
    /// Gets the digital twin preference profile (raw + normalized weights)
    /// for the currently authenticated user.
    /// </summary>
    [HttpGet("profile")]
    [SwaggerOperation(
        Summary = "Get current user's digital twin profile",
        Description = "Returns raw and normalized category weights for the logged-in user."
    )]
    [SwaggerResponse(200, "Profile returned", typeof(DigitalTwinProfileDto))]
    [SwaggerResponse(401, "User not authenticated")]
    [SwaggerResponse(404, "Digital twin not found")]
    public async Task<ActionResult<DigitalTwinProfileDto>> GetProfile()
    {
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(userIdStr) || !int.TryParse(userIdStr, out var userId))
            return Unauthorized("Invalid or missing user id claim.");

        var profile = await userTwinService.GetPreferenceProfileAsync(userId);
        if (profile is null)
            return NotFound("Digital twin not found for this user.");

        return Ok(profile);
    }
}
