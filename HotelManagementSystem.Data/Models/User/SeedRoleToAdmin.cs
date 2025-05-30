using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementSystem.Data.Models.User
{
    internal class SeedRoleToAdmin
    {
    }
    public class SeedRoleToAdminRequestModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
    public class SeedRoleToAdminResponseModel : BasedResponseModel
    {

    }
}
