using HotelManagementSystem.Data;
using HotelManagementSystem.Data.Dtos.User;
using HotelManagementSystem.Data.Entities;
using HotelManagementSystem.Data.Models;

namespace HotelManagementSystem.Service.Repositories.Interface;

public interface IUserRepository
{
    public Task<CustomEntityResult<RegisterUserResponseDto>> RegisterUser(RegisterUserrequestDto model);
    public Task<CustomEntityResult<SeedRoleResponseDto>> SeedRoleAsync(SeedRoleDto roleName);
    public Task<TblUser> GetUserById(Guid userId);
    public Task<TblUser> GetUserByEmail(string email);
    public Task<string> GetUserRolebyIdAsync(Guid id);
    public Task UpdateTokenAsync(TblUser user);
    public Task<bool> RoleExitAsync(string roleName);
    public Task<CustomEntityResult<ForgotPasswordResponseDto>> UpdateTokenAsync(ForgotPasswordRequestDto dto);
    public Task<string> GetValidPasswordOtpByEmailAsync(string email);
    public Task<CustomEntityResult<BasedResponseModel>> UpdatePasswordAsync(Guid userId, string newPassword);
    public Task<CustomEntityResult<BasedResponseModel>> DeletePasswordOTPAsync(TblUser user);
    public Task<CustomEntityResult<CreateUserProfileResponseDto>> CreateUserProfileAsync(CreateUserProfileRequestDto dto);
    public Task<CustomEntityResult<GetUserProfileByIdResponseDto>> GetUserProfileByIdAsync(GetUserProfileByIdRequestDto dto);
    public Task<CustomEntityResult<SeedRoleToAdminResponseDto>> SeedRoleToAdmin(SeedRoleToAdminRequestDto dto);
}