using System;

namespace API.Repositories.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IUserRepository Users { get; }
    IUserTokenRepository UserTokens { get; }
    IAuditTrailRepository AuditTrails { get; }
    IAddressRepository Addresses { get; }
    IGenderRepository Genders { get; }
    IRoleRepository Roles { get; }
    Task<int> SaveChangesAsync();
    Task BeginTransactionAsync();
    Task CommitAsync();
    Task RollbackAsync();
}
