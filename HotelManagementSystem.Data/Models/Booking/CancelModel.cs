using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementSystem.Data.Models.Booking
{
    public class CancelModel
    {
    }
    public class CancelRequestModel
    {
        public Guid BookingId { get; set; }
    }

    public class CancelResponseModel : BasedResponseModel
    {

    }
}
