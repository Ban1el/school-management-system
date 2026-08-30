using API.Data;
using API.DTOs.Gender;
using API.DTOs.Role;
using API.Repositories.Interfaces;

namespace API.Services
{
    public class RoleService
    {
        private readonly AppDbContext _context;
        private readonly IUnitOfWork _uow;

        public RoleService(IUnitOfWork uow, AppDbContext context)
        {
            _uow = uow;
            _context = context;
        }

        public async Task<List<RoleDto>> GetRolesActiveAsync()
        {
            return await _uow.Roles.GetRolesActiveAsync();
        }
    }
}
