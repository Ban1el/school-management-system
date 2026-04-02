using System;

namespace API.DTOs.UserToken;

public class UserTokenDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    public DateTime? DateModified { get; set; }
}
