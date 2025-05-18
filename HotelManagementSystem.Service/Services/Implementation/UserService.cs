using HotelManagementSystem.Data;
using HotelManagementSystem.Data.Dtos.User;
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

    public async Task<CustomEntityResult<CreateUserResponseModel>> CreateUser(CreateUserRequestModel model)
    {
        try
        {
            #region call repo

            var createUserRequest = new CreateUserRequestDto
            {
                Name = model.Name,
                Email = model.Email
            };
            var createUser = await _userRepo.CreateUser(createUserRequest);
            if (createUser.IsError)
            {
                return CustomEntityResult<CreateUserResponseModel>.GenerateFailEntityResult(createUser.Result.RespCode, createUser.Result.RespDescription);
            }

            #endregion
            
            var createUserResponse = new CreateUserResponseModel();
            return CustomEntityResult<CreateUserResponseModel>.GenerateSuccessEntityResult(createUserResponse);
        }
        catch (Exception ex)
        {
            return CustomEntityResult<CreateUserResponseModel>.GenerateFailEntityResult(ResponseMessageConstants.RESPONSE_CODE_SERVERERROR, ex.Message + ex.InnerException);
        }
    }
}