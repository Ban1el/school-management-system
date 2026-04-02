using System;
using API.Common;
using API.Data;
using API.DTOs;
using API.DTOs.ErrorLog;
using API.Models;
using API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace API.Services;

public class ErrorLogService
{
    private readonly AppDbContext _context;
    private readonly IUnitOfWork _uow;

    public ErrorLogService(IUnitOfWork uow, AppDbContext context)
    {
        _uow = uow;
        _context = context;
    }

    public async Task CreateAsync(ErrorLogCreateDto dto)
    {
        await _uow.BeginTransactionAsync();

        try
        {
            await _uow.ErrorLogs.CreateAsync(dto.UserId, dto.Description, dto.ClientIpAddress);
            await _uow.SaveChangesAsync();
            await _uow.CommitAsync();
        }
        catch
        {
            await _uow.RollbackAsync();
            throw;
        }
    }
}
