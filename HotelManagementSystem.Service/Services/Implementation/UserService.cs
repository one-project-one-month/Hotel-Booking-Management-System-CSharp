using HotelManagementSystem.Data;
using HotelManagementSystem.Data.Dtos.User;
using HotelManagementSystem.Data.Models.Constants;
using HotelManagementSystem.Data.Models.User;
using HotelManagementSystem.Service.Helpers.Auth.Token;
using HotelManagementSystem.Service.Repositories.Interface;
using HotelManagementSystem.Service.Services.Interface;

namespace HotelManagementSystem.Service.Services.Implementation;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepo;
    private readonly ITokenProcessors _tokenProcessor;

    public UserService(IUserRepository userRepo, ITokenProcessors tokenProcessor)
    {
        _userRepo = userRepo;
        _tokenProcessor = tokenProcessor;
    }

    public async Task<CustomEntityResult<RegisterUserResponseModel>> RegisterUser(RegisterUserRequestModel model)
    {
        try
        {
            var registerUserRequest = new RegisterUserrequestDto
            {
                UserName = model.UserName,
                Email = model.Email,
                Password = model.Password,
            };
            var registerUser = await _userRepo.RegisterUser(registerUserRequest);
            if (registerUser.IsError)
            {
                return CustomEntityResult<RegisterUserResponseModel>.GenerateFailEntityResult(registerUser.Result.RespCode, registerUser.Result.RespDescription);
            }

            var registerUserResponse = new RegisterUserResponseModel();
            return CustomEntityResult<RegisterUserResponseModel>.GenerateSuccessEntityResult(registerUserResponse);
        }
        catch (Exception ex)
        {
            return CustomEntityResult<RegisterUserResponseModel>.GenerateFailEntityResult(ResponseMessageConstants.RESPONSE_CODE_SERVERERROR, ex.Message + (ex.InnerException?.Message ?? ""));
        }
    }

    public async Task<CustomEntityResult<SeedRoleResponseModel>> SeedRole()
    {
        try
        {
            var roles = new[]
            {
                RoleConstants.Admin,
                RoleConstants.User
            };
            foreach (var role in roles)
            {
                if (!await _userRepo.RoleExitAsync(role))
                {
                    var seedRoleRequest = new SeedRoleDto
                    {
                        RoleName = role,
                    };
                    await _userRepo.SeedRoleAsync(seedRoleRequest);
                }
            }
            var seedRoleResponse = new SeedRoleResponseModel();
            return CustomEntityResult<SeedRoleResponseModel>.GenerateSuccessEntityResult(seedRoleResponse);
        }
        catch (Exception ex)
        {
            return CustomEntityResult<SeedRoleResponseModel>.GenerateFailEntityResult(ResponseMessageConstants.RESPONSE_CODE_SERVERERROR, ex.Message + (ex.InnerException?.Message ?? ""));
        }
    }

    public async Task<CustomEntityResult<LoginResponseModel>> LoginAsync(LoginRequestModel model)
    {
        try
        {
            var email = model.Email;
            var user = await _userRepo.GetUserByEmail(email);
            var LoginResponse = new LoginRequestDto
            {
                Email = model.Email,
                Password = model.Password
            };
            var result = await _tokenProcessor.GenerateToken(LoginResponse);
            if (result.IsError)
            {
                return CustomEntityResult<LoginResponseModel>.GenerateFailEntityResult(result.Result.RespCode, result.Result.RespDescription);
            }

            var refreshToken = _tokenProcessor.GenerateRefreshToken();
            var refreshTokenExpireAt = DateTime.UtcNow.AddDays(7);
            user.RefreshToken = refreshToken;
            user.TokenExpireAt = refreshTokenExpireAt;
            await _userRepo.UpdateUserAsync(user);
            _tokenProcessor.WriteTokenInHttpOnlyCookie("refresh_token", refreshToken, refreshTokenExpireAt);
            var loginResponse = new LoginResponseModel
            {
                AccessToken = result.Result.AccessToken,
                ExpireAt = result.Result.ExpireAt
            };
            return CustomEntityResult<LoginResponseModel>.GenerateSuccessEntityResult(loginResponse);
        }
        catch (Exception ex)
        {
            return CustomEntityResult<LoginResponseModel>.GenerateFailEntityResult(ResponseMessageConstants.RESPONSE_CODE_SERVERERROR, ex.Message + (ex.InnerException?.Message ?? ""));
        }
    }
}