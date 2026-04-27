using System;

namespace API.DTOs.User;

public class UserProfileUpdateDto
{
    public string? Username { get; set; } = string.Empty;
    public string? Email { get; set; } = string.Empty;
    public string? FirstName { get; set; }
    public string? MiddleName { get; set; }
    public string? LastName { get; set; }
    public string? MobileNumber { get; set; }
    public int? BarangayId { get; set; }
    public int? CityMunicipalityId { get; set; }
    public int? ProvinceId { get; set; }
    public int? RegionId { get; set; }
    public string? StreetAddress { get; set; }
    public string? ZipCode { get; set; }
}
