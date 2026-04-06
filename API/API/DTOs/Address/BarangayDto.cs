using System;
using API.DTOs.Common;

namespace API.DTOs.Address;

public class BarangayDto : BaseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int? CityId { get; set; }
}
