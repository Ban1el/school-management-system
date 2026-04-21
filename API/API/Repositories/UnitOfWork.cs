using System;
using API.Data;
using API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;


namespace API.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private IDbContextTransaction? _transaction;
    public IUserRepository Users { get; }
    public IUserTokenRepository UserTokens { get; }
    public IErrorLogRepository ErrorLogs { get; }
    public IAuditTrailRepository AuditTrails { get; }
    public IAddressRepository Addresses { get; }
    public IGenderRepository Genders { get; }

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
        Users = new UserRepository(context);
        UserTokens = new UserTokenRepository(context);
        ErrorLogs = new ErrorLogRepository(context);
        AuditTrails = new AuditTrailRepository(context);
        Addresses = new AddressRepository(context);
        Genders = new GenderRepository(context);
    }

    public async Task<int> SaveChangesAsync() =>
        await _context.SaveChangesAsync();

    public async Task BeginTransactionAsync() =>
        _transaction = await _context.Database.BeginTransactionAsync();

    public async Task CommitAsync()
    {
        await _transaction!.CommitAsync();
        await _transaction.DisposeAsync();
        _transaction = null;
    }

    public async Task RollbackAsync()
    {
        await _transaction!.RollbackAsync();
        await _transaction.DisposeAsync();
        _transaction = null;
    }

    public void Dispose() => _context.Dispose();
}