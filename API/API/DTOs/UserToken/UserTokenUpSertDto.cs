using System;

namespace API.DTOs.UserToken;

public class UserTokenUpSertDto
{
    public int UserId { get; set; }
    public string RefreshToken { get; set; } = string.Empty;
}
