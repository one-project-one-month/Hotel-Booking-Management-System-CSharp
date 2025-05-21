using HotelManagementSystem.Data;
using HotelManagementSystem.Data.Dtos.User;
using HotelManagementSystem.Data.Entities;

namespace HotelManagementSystem.Service.Repositories.Interface;

public interface IUserRepository
{
    public Task<CustomEntityResult<RegisterUserResponseDto>> RegisterUser(RegisterUserrequestDto model);
    public Task<CustomEntityResult<SeedRoleResponseDto>> SeedRoleAsync(SeedRoleDto roleName);
    public Task<TblUser> GetUserByEmail(string email);
    public Task<string> GetUserRolebyIdAsync(Guid id);
    public Task UpdateUserAsync(TblUser user);
    public Task<bool> RoleExitAsync(string roleName);
}