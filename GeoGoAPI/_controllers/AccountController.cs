using System.Security.Cryptography;
using System.Text;
using GeoGoAPI._models;
using GeoGoAPI._models.dtos;
using GeoGoAPI._models.dtos.account;
using GeoGoAPI._models.entities;
using GeoGoAPI._repositories.interfaces;
using GeoGoAPI._services.interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace GeoGoAPI._controllers;

[SwaggerTag("User account registration & login (JWT-based).")]
public class AccountController(
    GeoGoDbContext context,
    IUserRepository users,
    ITokenService tokenService
) : BaseApiController
{
    /// <summary>
    /// Registers a new user and creates a linked UserTwin (digital twin).
    /// </summary>
    [AllowAnonymous]
    [HttpPost("register")]
    [SwaggerOperation(
        Summary = "Register a new user",
        Description = "Creates an AppUser, a UserTwin, and returns a JWT for immediate use."
    )]
    [SwaggerResponse(201, "User registered successfully", typeof(UserDto))]
    [SwaggerResponse(400, "Username is taken or invalid payload")]
    public async Task<ActionResult<UserDto>> Register(RegisterDto dto)
    {
        if (await users.UserExistsAsync(dto.Username))
            return BadRequest("Username is taken");

        using var hmac = new HMACSHA512();
        var user = new AppUser
        {
            UserName = dto.Username.ToLower(),
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.Password)),
            PasswordSalt = hmac.Key,
        };

        // Create the twin on first sign up - there is only one twin per user
        var twin = new UserTwin { User = user };
        context.UserTwins.Add(twin);

        users.Add(user);
        await users.SaveChangesAsync();

        var result = new UserDto
        {
            Username = user.UserName,
            Token = tokenService.CreateToken(user),
        };

        // 201 with Location would require a GET endpoint; 200 is also fine.
        return Created(string.Empty, result);
    }

    /// <summary>
    /// Logs a user in and returns a short-lived JWT.
    /// </summary>
    [SwaggerOperation(Summary = "Logs a user in", Description = "Returns a 15-min JWT on success.")]
    [SwaggerResponse(200, "OK", typeof(UserDto))]
    [SwaggerResponse(401, "Invalid credentials")]
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto dto)
    {
        var user = await users.GetByUserNameAsync(dto.Username.ToLower());
        if (user == null)
            return Unauthorized("Invalid username");

        using var hmac = new HMACSHA512(user.PasswordSalt);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.Password));

        var match =
            computedHash.Length == user.PasswordHash.Length
            && !computedHash.Where((t, i) => t != user.PasswordHash[i]).Any();
        if (!match)
            return Unauthorized("Invalid password");

        return new UserDto { Username = user.UserName, Token = tokenService.CreateToken(user) };
    }
}
