using System;
using API.DTOs.Common;

namespace API.DTOs.Address;

public class RegionDto : BaseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}
