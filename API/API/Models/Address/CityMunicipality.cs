using System;

namespace API.Models.Address;

public class CityMunicipality : BaseEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int? ProvinceId { get; set; }
    public int? RegionId { get; set; }
    public Region? Region { get; set; } = null;
    public Province? Province { get; set; } = null;
}