using System;

namespace API.Models;

public class AuditTrail
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Module { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
    public string Data { get; set; } = string.Empty;
    public string IpAddress { get; set; } = string.Empty;
    public string ReqIpAddress { get; set; } = string.Empty;
    public DateTime DateCreated { get; set; }
    public bool IsRequest { get; set; }
    public string Path { get; set; } = string.Empty;
    public string RefId { get; set; } = string.Empty;
}
