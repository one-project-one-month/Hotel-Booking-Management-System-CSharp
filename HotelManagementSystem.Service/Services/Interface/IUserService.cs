using HotelManagementSystem.Data;
using HotelManagementSystem.Data.Models.User;

namespace HotelManagementSystem.Service.Services.Interface;

public interface IUserService
{
    public Task<CustomEntityResult<CreateUserResponseModel>> CreateUser(CreateUserRequestModel model);
}