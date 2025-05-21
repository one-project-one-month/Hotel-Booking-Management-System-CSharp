using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementSystem.Data.Models.User
{
    public class LoginModel
    {

    }
    public class LoginRequestModel
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
    public class LoginResponseModel : BasedResponseModel
    {
        public string AccessToken { get; set; } = null!;
        public DateTime ExpireAt { get; set; }
    }
}
