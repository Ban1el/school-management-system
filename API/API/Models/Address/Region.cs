using System;

namespace API.Models.Address;

public class Region : BaseEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}