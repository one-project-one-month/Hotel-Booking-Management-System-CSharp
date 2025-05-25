using HotelManagementSystem.Data.Models;

namespace HotelManagementSystem.Data.Dtos.User
{
    public class SeedRoleDto 
    {
        public string RoleName { get; set; } = null!;
    }
    public class SeedRoleResponseDto : BasedResponseModel { }
}
