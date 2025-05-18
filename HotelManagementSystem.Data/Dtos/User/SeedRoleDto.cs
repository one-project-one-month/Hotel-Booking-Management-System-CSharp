using HotelManagementSystem.Data.Models;
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
    public class SeedRoleResponseDto : BasedResponseModel { }
}
