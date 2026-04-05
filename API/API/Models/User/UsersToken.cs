using System;

namespace API.Models;

public class UserToken
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string? RefreshToken { get; set; } = string.Empty;
    public DateTime? ExpiryDate { get; set; }
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    public DateTime? DateModified { get; set; }
}
