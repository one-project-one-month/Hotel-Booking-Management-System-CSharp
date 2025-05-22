namespace HotelManagementSystem.Data.Models.User
{
    public class ForgotPasswordModel
    {
    }
    public class ForgetPasswordRequestModel : BasedRequestModel
    {
        public string Email { get; set; } = null!;
    }
    public class ForgotPasswordResponseModel : BasedResponseModel
    {

    }
}
