using HotelManagementSystem.Data;
using HotelManagementSystem.Data.Models.User;

namespace HotelManagementSystem.Service.Services.Interface
{
    public interface IUserService
    {
        Task<CustomEntityResult<LoginResponseModel>> LoginAsync(LoginRequestModel model);
        Task<CustomEntityResult<RegisterUserResponseModel>> RegisterUser(RegisterUserRequestModel model);
        Task<CustomEntityResult<SeedRoleResponseModel>> SeedRole();
    }
}