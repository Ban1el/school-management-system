using System;
using API.Data;
using API.DTOs.AudiTrail;
using API.Models;
using API.Repositories.Interfaces;

namespace API.Services;

public class AuditTrailService
{
    private readonly AppDbContext _context;
    private readonly IUnitOfWork _uow;

    public AuditTrailService(IUnitOfWork uow, AppDbContext context)
    {
        _uow = uow;
        _context = context;
    }

    public async Task CreateAsync(AuditTrailCreateDto dto)
    {
        await _uow.BeginTransactionAsync();

        try
        {
            await _uow.AuditTrails.CreateAsync(dto.UserId, dto.Module, dto.Action, dto.Method, dto.Description, dto.Data, dto.ClientIpAddress, dto.IsRequest, dto.Path, dto.RefId);
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
