using System;
using API.DTOs.Common;

namespace API.DTOs.Gender;

public class GenderDto : BaseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}
