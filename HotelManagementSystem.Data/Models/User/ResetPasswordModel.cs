namespace HotelManagementSystem.Data.Models.User
{
    public class ResetPasswordModel
    {
    }
    public class ResetPasswordRequestModel
    {
        public required string Email { get; set; } = null!;
        public required string OTP { get; set; }
        public required string Password { get; set; }
    }
    public class ResetPasswordResponseModel : BasedResponseModel
    {

    }
}
