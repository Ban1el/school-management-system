using System;

namespace API.Models;

public class User : BaseEntity
{
    public int Id { get; set; }
    public int RoleId { get; set; }
    public int? RegionId { get; set; }
    public int? ProvinceId { get; set; }
    public int? CityMunicipalityId { get; set; }
    public int? BarangayId { get; set; }
    public string? StreetAddress { get; set; }
    public string? FirstName { get; set; }
    public string? MiddleName { get; set; }
    public string? LastName { get; set; }
    public string? MobileNumber { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string PasswordSalt { get; set; } = string.Empty;

}
