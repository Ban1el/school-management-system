using System.Text.Json.Serialization;

namespace API.DTOs.Users;

public class UserDto
{
    public int Id { get; set; }
    public int RoleId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    [JsonIgnore]
    public string Password { get; set; } = string.Empty;
    [JsonIgnore]
    public string PasswordSalt { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}
