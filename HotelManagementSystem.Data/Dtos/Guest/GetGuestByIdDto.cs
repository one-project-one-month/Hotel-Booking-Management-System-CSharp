using HotelManagementSystem.Data.Models;
namespace HotelManagementSystem.Data.Dtos.Guest
{
    public class GetGuestByIdDto
    {
    }
    public class GetGuestByIdRequestDto
    {
        public Guid GuestId { get; set; }
    }
    public class GetGuestByIdResponseDto : BasedResponseModel
    {
        public Guid GuestId { get; set; }
        public Guid? UserId { get; set; }
        public string Nrc { get; set; } = null!;
        public string PhoneNo { get; set; } = null!;
        public DateTime? CreatedAt { get; set; }
        public string Name { get; set; } = null!;
        public string? Email { get; set; }
    }
}
