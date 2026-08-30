using API.DTOs.Role;

namespace API.Repositories.Interfaces
{
    public interface IRoleRepository
    {
        Task<List<RoleDto>> GetRolesActiveAsync();
    }
}
