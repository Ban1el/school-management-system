using System;

namespace API.Models.Address;

public class Barangay : BaseEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int CityId { get; set; }
}