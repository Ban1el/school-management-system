using System;
using API.DTOs.ErrorLog;

namespace API.Repositories.Interfaces;

public interface IErrorLogRepository
{
    Task CreateAsync(int userId, string description, string clientIpAddress);
}
