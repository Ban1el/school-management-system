using System;
using API.Data;
using API.Models;
using API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories;

public class ErrorLogRepository(AppDbContext _context) : IErrorLogRepository
{
    public async Task CreateAsync(int userId, string description, string clientIpAddress)
    {
        await _context.ErrorLogs.AddAsync(new ErrorLog
        {
            UserId = userId,
            Description = description,
            ClientIpAddress = clientIpAddress,
        });
    }
}
