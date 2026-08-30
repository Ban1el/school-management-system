using API.Data;
using API.DTOs.Address;
using API.DTOs.Role;
using API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class RoleRepository(AppDbContext _context) : IRoleRepository
    {
        public async Task<List<RoleDto>> GetRolesActiveAsync()
        {
            return await _context.Roles.Select(r => new RoleDto
            {
                Id = r.Id,
                Name = r.Name,
                DateCreated = r.DateCreated,
                CreatedBy = r.CreatedBy,
                DateModified = r.DateModified,
                ModifiedBy = r.ModifiedBy,
                IsActive = r.IsActive
            }).ToListAsync();
        }
    }
}
