using System;
using API.Data;
using API.DTOs.AudiTrail;
using API.Models;
using API.Repositories.Interfaces;

namespace API.Repositories;

public class AuditTrailRepository(AppDbContext _context) : IAuditTrailRepository
{
    public async Task CreateAsync(int userId, string module, string action, string method, string description, string data, string clientIpAddress, bool isRequest, string path, string refId)
    {
        await _context.AuditTrails.AddAsync(new AuditTrail
        {
            UserId = userId,
            Module = module,
            Action = action,
            Method = method,
            Description = description,
            Data = data,
            ClientIpAddress = clientIpAddress,
            IsRequest = isRequest,
            Path = path,
            RefId = refId,
        });
    }
}
