using HotelManagementSystem.Data.Models;
namespace HotelManagementSystem.Data.Dtos.User
{
    public class GetAllUSerInfoDto
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string RoleName { get; set; } = null!;
        public string? Gender { get; set; }
        public string? Address { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public DateTime? CreatedAt { get; set; }
    }

    public class GetAllUserInfoRequestDto : BasedRequestModel
    {

    }

    public class GetAllUserInforResponseDto : BasedResponseModel
    {
        public List<GetAllUSerInfoDto>? Users { get; set; }
    }
}
