using System;

namespace API.Models.Address;

public class Province : BaseEntity
{
    public int Id { get; set; }
    public int RegionId { get; set; }
    public string Name { get; set; } = string.Empty;
    public Region Region { get; set; } = null!;
}