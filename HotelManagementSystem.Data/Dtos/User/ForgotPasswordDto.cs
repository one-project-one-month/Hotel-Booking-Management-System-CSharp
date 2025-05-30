using HotelManagementSystem.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementSystem.Data.Dtos.User
{
    public class ForgotPasswordDto
    {
    }
    public class ForgotPasswordRequestDto
    {
        public string Email { get; set; } = null!;
        public string Otp { get; set; } = null!;
    }
    public class ForgotPasswordResponseDto : BasedResponseModel
    {
    }
}
