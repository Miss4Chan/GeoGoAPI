namespace GeoGoAPI._models.entities;

public class AppUser
{
    public int Id { get; set; }
    public required string UserName { get; set; }
    public byte[] PasswordHash { get; set; } = [];
    public byte[] PasswordSalt { get; set; } = [];

    // 1:1 with UserTwin
    public UserTwin? Twin { get; set; }
}
