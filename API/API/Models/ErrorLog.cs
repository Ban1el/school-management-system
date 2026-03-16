using System;

namespace API.Models;

public class ErrorLog
{
    public int Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public string IpAddress { get; set; } = string.Empty;
    public DateTime DateCreated { get; set; }
}
