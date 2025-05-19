using HotelManagementSystem.Data;
using HotelManagementSystem.Data.Dtos.User;

namespace HotelManagementSystem.Service.Repositories.Interface;

public interface IUserRepository
{
    public Task<CustomEntityResult<RegisterUserResponseDto>> RegisterUser(RegisterUserrequestDto model);
    public Task<CustomEntityResult<SeedRoleResponseDto>> SeedRoleAsync(SeedRoleDto roleName);
    public Task<bool> RoleExitAsync(string roleName);
}