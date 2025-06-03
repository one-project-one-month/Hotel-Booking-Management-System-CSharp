namespace HotelManagementSystem.Features.User;
public class BL_User
{
    private readonly DA_User _daUser;
    public BL_User(DA_User daUser)
    {
        _daUser = daUser;
    }

    public async Task<CustomEntityResult<CreateUserResponseModel>> CreateUser(CreateUserRequestModel model)
    {
        var createUserResponse = new CreateUserResponseModel();
        try
        {
            var createUser = await _daUser.CreateUser(model);
            if (createUser.IsError)
            {
                return CustomEntityResult<CreateUserResponseModel>.GenerateFailEntityResult(createUser.Result.RespCode,
                    createUser.Result.RespDescription);
            }

            return CustomEntityResult<CreateUserResponseModel>.GenerateSuccessEntityResult(createUserResponse);
        }
        catch (Exception ex)
        {
            return CustomEntityResult<CreateUserResponseModel>.GenerateFailEntityResult(ResponseMessageConstants.RESPONSE_CODE_SERVERERROR,
                ex.Message + ex.InnerException);
        }
    }
}
