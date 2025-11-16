using GeoGoAPI._models;
using GeoGoAPI._models.entities;
using GeoGoAPI._repositories.interfaces;
using Microsoft.EntityFrameworkCore;

namespace GeoGoAPI._repositories.implementation;

public class UserRepository(GeoGoDbContext context) : IUserRepository
{
    public async Task<bool> UserExistsAsync(string username) =>
        await context
            .Users.AsNoTracking()
            .AnyAsync(x => EF.Functions.Collate(x.UserName, "NOCASE") == username);

    public async Task<AppUser?> GetByUserNameAsync(string username) =>
        await context.Users.FirstOrDefaultAsync(x =>
            EF.Functions.Collate(x.UserName, "NOCASE") == username
        );

    public void Add(AppUser user) => context.Users.Add(user);

    public async Task SaveChangesAsync() => await context.SaveChangesAsync();
}
