using HotelManagementSystem.Data;
using HotelManagementSystem.Data.Dtos.User;
using HotelManagementSystem.Data.Models.Constants;
using HotelManagementSystem.Data.Models.User;
using HotelManagementSystem.Service.Repositories.Interface;
using HotelManagementSystem.Service.Services.Interface;

namespace HotelManagementSystem.Service.Services.Implementation;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepo;
    
    public UserService(IUserRepository userRepo)
    {
        _userRepo = userRepo;
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
            if(registerUser.IsError)
            {
                return CustomEntityResult<RegisterUserResponseModel>.GenerateFailEntityResult(registerUser.Result.RespCode, registerUser.Result.RespDescription);
            }

            var registerUserResponse = new RegisterUserResponseModel();
            return CustomEntityResult<RegisterUserResponseModel>.GenerateSuccessEntityResult(registerUserResponse);
        }
        catch(Exception ex)
        {
            return CustomEntityResult<RegisterUserResponseModel>.GenerateFailEntityResult(ResponseMessageConstants.RESPONSE_CODE_SERVERERROR, ex.Message + ex.InnerException);
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
                if(!await _userRepo.RoleExitAsync(role))
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
        catch(Exception ex)
        {
            return CustomEntityResult<SeedRoleResponseModel>.GenerateFailEntityResult(ResponseMessageConstants.RESPONSE_CODE_SERVERERROR, ex.Message + ex.InnerException);
        }
    }
}