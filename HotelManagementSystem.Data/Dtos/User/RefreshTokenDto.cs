using HotelManagementSystem.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementSystem.Data.Dtos.User
{
    internal class RefreshTokenDto
    {
    }
    public class RefreshTokenRequestDto
    {
        public Guid Id { get; set; }
    }
    public class RefreshTokenResponseDto : BasedResponseModel
    {

    }
}
