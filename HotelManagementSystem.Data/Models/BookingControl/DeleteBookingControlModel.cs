using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementSystem.Data.Models.BookingControl;

public class DeleteBookingControlModel
{

}
public class DeleteBookingRequestModel
{
    public Guid BookingId { get; set; }
}

public class DeleteBookingResponseModel : BasedResponseModel { }
