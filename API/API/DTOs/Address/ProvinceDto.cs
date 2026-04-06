using System;
using API.DTOs.Common;

namespace API.DTOs.Address;

public class ProvinceDto : BaseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int regionId { get; set; }
}
