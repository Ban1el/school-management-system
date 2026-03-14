using System;

namespace API.Models;

public class Role : BaseEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}
