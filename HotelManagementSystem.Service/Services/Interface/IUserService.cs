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
        public Task<CustomEntityResult<UpdateUserProfileByIdResponseModel>> UpdateUserProfileByIdAsync(string userId, CreateUserProfileRequestModel model);
        public Task<CustomEntityResult<GetUserProfileByIdResponseModel>> GetUserProfileByIdAsync(string Id);
        public Task<CustomEntityResult<SeedRoleToAdminResponseModel>> SeedRoleToAdmin(SeedRoleToAdminRequestModel model);
        public Task<CustomEntityResult<CreateUserResponseModel>> CreateUserProfileByAdminAsync(CreateUserProfileByAdminRequestModel model);
        public Task<CustomEntityResult<GetAllUserInfoResponseModel>> GetAllUserInfoAsync();
    }
}