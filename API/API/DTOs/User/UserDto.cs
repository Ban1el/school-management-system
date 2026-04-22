using System.Text.Json.Serialization;

namespace API.DTOs.Users;

public class UserDto
{
    public int Id { get; set; }
    public string RoleName { get; set; } = string.Empty;
    public int RoleId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    [JsonIgnore]
    public string Password { get; set; } = string.Empty;
    [JsonIgnore]
    public string PasswordSalt { get; set; } = string.Empty;
    public string? FirstName { get; set; }
    public string? MiddleName { get; set; }
    public string? LastName { get; set; }
    public string? MobileNumber { get; set; }
    public string? BarangayName { get; set; }
    public int? BarangayId { get; set; }
    public string? CityMunicipalityName { get; set; }
    public int? CityMunicipalityId { get; set; }
    public string? ProvinceName { get; set; }
    public int? ProvinceId { get; set; }
    public string? RegionName { get; set; }
    public int? RegionId { get; set; }
    public string? StreetAddress { get; set; }
    public string? ZipCode { get; set; }
    public bool IsActive { get; set; }
}
