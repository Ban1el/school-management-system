using System;
using API.DTOs.Common;

namespace API.DTOs.Address;

public class CityMunicipalityDto : BaseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int? ProvinceId { get; set; }
    public int? RegionId { get; set; }
}
