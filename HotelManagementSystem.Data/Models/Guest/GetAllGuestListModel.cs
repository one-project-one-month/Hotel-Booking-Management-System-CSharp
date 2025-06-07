using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementSystem.Data.Models.Guest
{
    public class GetAllGuestListModel
    {
        public Guid GuestId { get; set; }

        public Guid? UserId { get; set; }

        public string Nrc { get; set; } = null!;

        public string PhoneNo { get; set; } = null!;

        public DateTime? CreatedAt { get; set; }

        public string Name { get; set; } = null!;

        public string? Email { get; set; }
    }

    public class GetAllGuestListResponseModel : BasedResponseModel
    {
        public List<GetAllGuestListModel>? Guests { get; set; }
    }
}
