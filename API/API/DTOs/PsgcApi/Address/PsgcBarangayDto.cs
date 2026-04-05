using System;
using System.Text.Json.Serialization;

namespace API.DTOs.Address;

public class PsgcBarangayDto
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Region { get; set; } = string.Empty;
    public string Province { get; set; } = string.Empty;
    [JsonPropertyName("city_municipality")]
    public string CityMunicipality { get; set; } = string.Empty;
    [JsonPropertyName("zip_code")]
    public string ZipCode { get; set; } = string.Empty;
    public string District { get; set; } = string.Empty;
}
