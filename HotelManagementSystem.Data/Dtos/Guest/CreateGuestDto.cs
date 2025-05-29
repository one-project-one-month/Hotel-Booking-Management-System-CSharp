using HotelManagementSystem.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementSystem.Data.Dtos.Guest
{
    internal class CreateGuestDto
    {
    }
    public class CreateGuestRequestDto
    {
        public Guid? UserId { get; set; }
        public string Nrc { get; set; } = null!;
        public string PhoneNo { get; set; } = null!;
    }
    public class CreateGuestResponseDto : BasedResponseModel
    {
    }
}
