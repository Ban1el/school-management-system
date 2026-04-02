using System;
using API.DTOs.AudiTrail;

namespace API.Repositories.Interfaces;

public interface IAuditTrailRepository
{
    Task CreateAsync(int userId, string module, string action, string method, string description, string data, string clientIpAddress, bool isRequest, string path, string refId);
}
