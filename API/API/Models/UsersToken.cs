using System;

namespace API.Models;

public class UsersToken
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime ExpiryDate { get; set; }
    public DateTime? DateModified { get; set; }
}
