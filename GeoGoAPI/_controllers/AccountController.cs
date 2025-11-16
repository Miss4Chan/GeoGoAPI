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

public class AccountController(
    GeoGoDbContext context,
    IUserRepository users,
    ITokenService tokenService
) : BaseApiController
{
    [AllowAnonymous]
    [HttpPost("register")]
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

        //Create the twin on first sign up of the user - there is only one twin per one user
        var twin = new UserTwin { User = user };
        context.UserTwins.Add(twin);

        users.Add(user);
        await users.SaveChangesAsync();

        return new UserDto { Username = user.UserName, Token = tokenService.CreateToken(user) };
    }

    [SwaggerOperation(Summary = "Logs a user in", Description = "Returns a 15-min JWT on success")]
    [SwaggerResponse(200, "OK")]
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
