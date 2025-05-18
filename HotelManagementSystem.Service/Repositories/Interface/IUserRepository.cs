using HotelManagementSystem.Data;
using HotelManagementSystem.Data.Dtos.User;

namespace HotelManagementSystem.Service.Repositories.Interface;

public interface IUserRepository
{
    public Task<CustomEntityResult<CreateUserResponseDto>> CreateUser(CreateUserRequestDto model);
}