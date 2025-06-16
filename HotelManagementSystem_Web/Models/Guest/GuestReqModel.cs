using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelManagementSystem_Web.Models.Guest
{
    public class GuestReqModel 
    {
        public Guid guestId { get; set; }

        public Guid? userId { get; set; } = Guid.Empty;

        public string nrc { get; set; } = null!;

        public string phoneNo { get; set; } = null!;

        public DateTime? createdAt { get; set; }

        public string name { get; set; } = null!;

        public string? email { get; set; }
    }
}