using HotelManagementSystem.Data.Models;

namespace HotelManagementSystem.Data.Dtos.Booking
{
    public class CancelDto
    {
    }
    public class CancelRequestDto
    {
        public Guid BookingId { get; set; }
    }
    public class CancelResponseDto : BasedResponseModel
    {

    }
}
