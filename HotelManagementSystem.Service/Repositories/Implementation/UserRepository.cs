using HotelManagementSystem.Data;
using HotelManagementSystem.Data.Data;
using HotelManagementSystem.Data.Dtos.User;
using HotelManagementSystem.Data.Entities;
using HotelManagementSystem.Data.Models;
using HotelManagementSystem.Data.Models.Constants;
using HotelManagementSystem.Service.Exceptions;
using HotelManagementSystem.Service.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementSystem.Service.Reposities.Implementation;

public class UserRepository : IUserRepository
{
    private readonly HotelDbContext _context;
    public UserRepository(HotelDbContext context)
    {
        _context = context;

    }

    public async Task<CustomEntityResult<RegisterUserResponseDto>> RegisterUser(RegisterUserrequestDto model)
    {
        try
        {
            var registerUserRequest = new TblUser
            {
                UserName = model.UserName,
                Email = model.Email,
                Password = model.Password
            };
            registerUserRequest.RoleId = await SeedRoleToUser();
            var registerUser = await _context.AddAsync(registerUserRequest);
            await _context.SaveChangesAsync();
            var registerUserResponse = new RegisterUserResponseDto();
            return CustomEntityResult<RegisterUserResponseDto>.GenerateSuccessEntityResult(registerUserResponse);
        }
        catch (Exception ex)
        {
            return CustomEntityResult<RegisterUserResponseDto>.GenerateFailEntityResult(ResponseMessageConstants.RESPONSE_CODE_SERVERERROR, ex.Message + ex.InnerException);
        }
    }

    public async Task<TblUser> GetUserByEmail(string email)
    {
        var user = await _context.TblUsers.FirstOrDefaultAsync(x => x.Email == email);
        if (user == null)
        {
            throw new UserNotFoundException(email);
        }
        return user;
    }

    public async Task<CustomEntityResult<SeedRoleResponseDto>> SeedRoleAsync(SeedRoleDto roleName)
    {
        try
        {
            var roles = new TblRole
            {
                RoleId = Guid.NewGuid(),
                RoleName = roleName.RoleName
            };
            await _context.AddAsync(roles);
            await _context.SaveChangesAsync();

            var seedRoleResponse = new SeedRoleResponseDto();
            return CustomEntityResult<SeedRoleResponseDto>.GenerateSuccessEntityResult(seedRoleResponse);
        }
        catch (Exception ex)
        {
            return CustomEntityResult<SeedRoleResponseDto>.GenerateFailEntityResult(ResponseMessageConstants.RESPONSE_CODE_SERVERERROR, ex.Message + ex.InnerException);
        }
    }

    public async Task<string> GetUserRolebyIdAsync(Guid id)
    {
        var user = await _context.TblUsers.FirstOrDefaultAsync(x => x.UserId == id);
        if (user == null)
        {
            throw new UserDoesNotExitException(id);
        }
        var role = await _context.TblRoles.FirstOrDefaultAsync(x => x.RoleId == user.RoleId);
        if (role == null)
        {
            throw new RoleDoesNotExistException("Role does not exit!");
        }
        return role.RoleName;
    }

    public async Task<bool> RoleExitAsync(string roleName)
    {
        return await _context.TblRoles.AnyAsync(r => r.RoleName == roleName);
    }

    public async Task<Guid> SeedRoleToUser()
    {
        var Role = await _context.TblRoles.FirstOrDefaultAsync(r => r.RoleName == RoleConstants.User);
        if (Role == null)
        {
            throw new RoleDoesNotExistException("User Role does not exit!");
        }
        var RoleId = Role.RoleId;
        return RoleId;
    }

    public async Task UpdateUserAsync(TblUser user)
    {
        try
        {
            var existingUser = await _context.TblUsers
                .FirstOrDefaultAsync(u => u.UserId == user.UserId);
            if (existingUser == null)
            {
                throw new UserNotFoundException(user.Email);
            }
            _context.TblUsers.Update(existingUser);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"{ex.Message}", ex);
        }
    }

    public async Task<CustomEntityResult<BasedResponseModel>> DeletePasswordOTPAsync(TblUser user)
    {
        try
        {
            var existingUser = await _context.TblUsers
                .FirstOrDefaultAsync(u => u.UserId == user.UserId);

            if (existingUser == null)
            {
                throw new UserNotFoundException(user.Email);
            }
            existingUser.ForgetPasswordOtp = null;
            existingUser.OtpExpireAt = default;

            _context.TblUsers.Update(existingUser);
            await _context.SaveChangesAsync();

            var returnModel = new BasedResponseModel();
            return CustomEntityResult<BasedResponseModel>.GenerateSuccessEntityResult(returnModel);
        }
        catch(Exception ex)
        {
            return CustomEntityResult<BasedResponseModel>.GenerateFailEntityResult(ResponseMessageConstants.RESPONSE_CODE_SERVERERROR, ex.Message + ex.InnerException);
        }
    }

    public async Task<string> GetValidPasswordOtpByEmailAsync(string email)
    {
        try
        {
            var user = await _context.TblUsers
                .FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                throw new UserNotFoundException(user.Email);
            }

            var token = user.ForgetPasswordOtp;
            if (token == null)
            {
                throw new OTPNotFoudException("OTP not found");
            }
            return token;
        }
        catch(Exception ex)
        {
            throw new Exception($"{ex.Message}", ex);
        }
    }

    public async Task<CustomEntityResult<BasedResponseModel>> UpdateTokenAsync(string email, string resetPasswordOTP)
    {
        try
        {
            var user = await _context.TblUsers
                .FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                throw new UserNotFoundException(user.Email);
            }
            var token = user.ForgetPasswordOtp;
            user.OtpExpireAt = DateTime.UtcNow.AddMinutes(15);

            _context.TblUsers.Update(user);
            await _context.SaveChangesAsync();
            var returnModel = new BasedResponseModel();
            return CustomEntityResult<BasedResponseModel>.GenerateSuccessEntityResult(returnModel);
        }
        catch (Exception ex)
        {
            return CustomEntityResult<BasedResponseModel>.GenerateFailEntityResult(ResponseMessageConstants.RESPONSE_CODE_SERVERERROR, ex.Message + ex.InnerException);
        }
    }

    public async Task<CustomEntityResult<BasedResponseModel>> UpdatePasswordAsync(Guid userId, string newPassword)
    {
        try
        {
            var user = await _context.TblUsers.FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null)
            {
                throw new UserDoesNotExitException(userId);
            }
            await _context.SaveChangesAsync();
            var returnModel = new BasedResponseModel();
            return CustomEntityResult<BasedResponseModel>.GenerateSuccessEntityResult(returnModel);
        }
        catch (Exception ex)
        {
            return CustomEntityResult<BasedResponseModel>.GenerateFailEntityResult(ResponseMessageConstants.RESPONSE_CODE_SERVERERROR, ex.Message + ex.InnerException);
        }
    }
}