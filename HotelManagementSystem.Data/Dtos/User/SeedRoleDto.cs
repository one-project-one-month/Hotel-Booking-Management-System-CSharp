using HotelManagementSystem.Data.Models;
using HotelManagementSystem.Data.Models.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementSystem.Data.Dtos.User
{
    public class SeedRoleDto 
    {
        public string RoleName { get; set; }
    }

    public class RoleExit
    {
        public bool IsRoleExit { get; set; }
    }

    public class SeedRoleToAdminDto
    {
        public string Email { get; set; } = null!;
    }

    public class SeedRoleToUserDto
    {
        public string Email { get; set; } = null!;
    }
    public class SeedRoleResponseDto : BasedResponseModel { }
}
