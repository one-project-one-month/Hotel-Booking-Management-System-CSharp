using HotelManagementSystem.Data;
using HotelManagementSystem.Data.Models.User;

namespace HotelManagementSystem.Service.Services.Interface
{
    public interface IUserService
    {
        public Task<CustomEntityResult<LoginResponseModel>> LoginAsync(LoginRequestModel model);
        public Task<CustomEntityResult<RegisterUserResponseModel>> RegisterUser(RegisterUserRequestModel model);
        public Task<CustomEntityResult<SeedRoleResponseModel>> SeedRole();
        public Task<CustomEntityResult<ForgotPasswordResponseModel>> ForgotPasswordAsync(ForgetPasswordRequestModel model);
        public Task<CustomEntityResult<ResetPasswordResponseModel>> ResetPasswordAsync(ResetPasswordRequestModel model);
        public Task<CustomEntityResult<CreateUserResponseModel>> CreateUserProfileAsync(string userId, CreateUserProfileRequestModel model);
    }
}