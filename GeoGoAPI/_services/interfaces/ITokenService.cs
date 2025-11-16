namespace GeoGoAPI._services.interfaces;

using GeoGoAPI._models.entities;

public interface ITokenService
{
    string CreateToken(AppUser user);
}
