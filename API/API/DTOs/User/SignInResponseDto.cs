using System;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace API.DTOs.Users;

public class SignInResponseDto
{
    [JsonIgnore]
    public int userId { get; set; }
    [JsonIgnore]
    public string accesstoken { get; set; } = string.Empty;
    [JsonIgnore]
    public string refreshtoken { get; set; } = string.Empty;
}
