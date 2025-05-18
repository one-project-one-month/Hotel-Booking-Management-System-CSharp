using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementSystem.Data.Models.User
{
    public class RegisterUserModel
    {
    }

    public class RegisterUserRequestModel : BasedRequestModel
    {
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }

    public class RegisterUserResponseModel : BasedResponseModel { }
    
}
