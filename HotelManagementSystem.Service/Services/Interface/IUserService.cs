using HotelManagementSystem.Data;
using HotelManagementSystem.Data.Models.User;

namespace HotelManagementSystem.Service.Services.Interface;

public interface IUserService
{
    public Task<CustomEntityResult<RegisterUserResponseModel>> RegisterUser(RegisterUserRequestModel model);
    public Task<CustomEntityResult<SeedRoleResponseModel>> SeedRole();
}