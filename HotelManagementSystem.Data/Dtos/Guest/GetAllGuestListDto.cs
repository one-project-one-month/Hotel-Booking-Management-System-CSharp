using HotelManagementSystem.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementSystem.Data.Dtos.Guest
{
    public class GetAllGuestListDto
    {
        public Guid GuestId { get; set; }

        public Guid? UserId { get; set; }

        public string Nrc { get; set; } = null!;

        public string PhoneNo { get; set; } = null!;

        public DateTime? CreatedAt { get; set; }

        public string Name { get; set; } = null!;

        public string? Email { get; set; }
    }

    public class  GetAllGuestListRequestDto
    {
        
    }

    public class GetAllGuestListResponseDto : BasedResponseModel
    {
        public List<GetAllGuestListDto>? Guests { get; set; }
    }
}
