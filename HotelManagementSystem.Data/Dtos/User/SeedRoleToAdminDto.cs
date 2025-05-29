using HotelManagementSystem.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementSystem.Data.Dtos.User
{
    internal class SeedRoleToAdminDto
    {
    }
    public class SeedRoleToAdminRequestDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
    public class SeedRoleToAdminResponseDto : BasedResponseModel
    {

    }
}
