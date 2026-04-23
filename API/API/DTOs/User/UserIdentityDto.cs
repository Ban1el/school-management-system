using System;

namespace API.DTOs.User;

public class UserIdentityDto
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
}
