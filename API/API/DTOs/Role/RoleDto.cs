using API.DTOs.Common;

namespace API.DTOs.Role
{
    public class RoleDto: BaseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
