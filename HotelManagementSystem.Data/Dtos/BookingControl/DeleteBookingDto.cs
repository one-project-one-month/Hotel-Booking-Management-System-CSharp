using HotelManagementSystem.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementSystem.Data.Dtos.BookingControl;

public class DeleteBookingDto
{

}

public class DeleteBookingRequestDto
{
    public Guid BookingId { get; set; }
}

public class DeleteBookingResponseDto : BasedResponseModel { }
