using HotelManagementSystem.Data.Models.User;
using Sprache;

namespace HotelManagementSystem.Controllers;

[ApiController]
[Route("api/[Controller]")]
public class UserController : BaseController
{
    private readonly IUserService _service;
    public UserController(IHttpContextAccessor httpContextAccessor, IUserService service) : base(httpContextAccessor)
    {
        _service = service;
    }

    [HttpPost]
    [Route("SeedRole")]
    public async Task<ActionResult<SeedRoleResponseModel>> SeedRoleAsync()
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        try
        {
            var result = await _service.SeedRole();
            return !result.IsError ? APIHelper.GenerateSuccessResponse(result.Result) : APIHelper.GenerateFailResponse(result.Result);
        }
        catch (Exception ex)
        {
            var message = ex.Message;
            return BadRequest(500);
        }
    }

    //[Authorize(Roles ="Admin")]
    [HttpPost]
    [Route("SeedRoleToAdmin")]
    public async Task<ActionResult<BasedResponseModel>> SeedRoleToAdmin(SeedRoleToAdminRequestModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        try
        {
            var result = await _service.SeedRoleToAdmin(model);
            return !result.IsError ? APIHelper.GenerateSuccessResponse(result.Result) : APIHelper.GenerateFailResponse(result.Result);
        }
        catch (Exception ex)
        {
            var message = ex.Message;
            return BadRequest(500);
        }
    }

    [HttpPost]
    [Route("register")]
    public async Task<ActionResult<BasedResponseModel>> RegisterUserAsync(RegisterUserRequestModel model)
    {
        #region UserGetClaimsValue

        #endregion

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            #region CheckRequiredField
            #endregion

            #region Check Format
            var passwordErrors = PasswordPolicyValidator.Validate(model.Password);
            if (passwordErrors.Any())
            {
                return BadRequest(new BasedResponseModel
                {
                    RespCode = "400",
                    RespDescription = string.Join(", ", passwordErrors)
                });
            }
            #endregion

            var result = await _service.RegisterUser(model);

            return !result.IsError ? APIHelper.GenerateSuccessResponse(result.Result) : APIHelper.GenerateFailResponse(result.Result);
        }
        catch (Exception ex)
        {
            var message = ex.Message;
            return BadRequest(500);
        }
    }

    [HttpPost]
    [Route("login")]
    public async Task<ActionResult<BasedResponseModel>> LoginAsync(LoginRequestModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        #region Check Format
        var passwordErrors = PasswordPolicyValidator.Validate(model.Password);
        if (passwordErrors.Any())
        {
            return BadRequest(new BasedResponseModel
            {
                RespCode = "400",
                RespDescription = string.Join(", ", passwordErrors)
            });
        }
        #endregion
        try
        {
            var result = await _service.LoginAsync(model);

            return !result.IsError ? APIHelper.GenerateSuccessResponse(result.Result) : APIHelper.GenerateFailResponse(result.Result);
        }
        catch (Exception ex)
        {
            var message = ex.Message;
            return BadRequest(500);
        }
    }

    [HttpPost]
    [Route("forgotpassword")]
    public async Task<ActionResult<BasedResponseModel>> ForgotPasswordAsync(ForgetPasswordRequestModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        try
        {
            var result = await _service.ForgotPasswordAsync(model);
            return !result.IsError ? APIHelper.GenerateSuccessResponse(result.Result) : APIHelper.GenerateFailResponse(result.Result);
        }
        catch (Exception ex)
        {
            var message = ex.Message;
            return BadRequest(500);
        }
    }

    [HttpPost]
    [Route("resetpassword")]
    public async Task<ActionResult<BasedResponseModel>> ResetPasswordAsync(ResetPasswordRequestModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        #region Check Format
        var passwordErrors = PasswordPolicyValidator.Validate(model.Password);
        if (passwordErrors.Any())
        {
            return BadRequest(new BasedResponseModel
            {
                RespCode = "400",
                RespDescription = string.Join(", ", passwordErrors)
            });
        }
        #endregion
        try
        {
            var result = await _service.ResetPasswordAsync(model);
            return !result.IsError ? APIHelper.GenerateSuccessResponse(result.Result) : APIHelper.GenerateFailResponse(result.Result);
        }
        catch (Exception ex)
        {
            var message = ex.Message;
            return BadRequest(500);
        }
    }

    [HttpPost]
    [Route("createuserprofilebyadmin")]
    public async Task<ActionResult<CreateUserResponseModel>> CreateUserProfileByAdminAsync(CreateUserProfileByAdminRequestModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        try
        {
            var result = await _service.CreateUserProfileByAdminAsync(model);
            return !result.IsError ? APIHelper.GenerateSuccessResponse(result.Result) : APIHelper.GenerateFailResponse(result.Result);
        }
        catch (Exception ex)
        {
            var message = ex.Message;
            return BadRequest(500);
        }
    }

    [Authorize]
    [HttpPatch]
    public async Task<ActionResult<UpdateUserProfileByIdResponseModel>> UpdateUserProfileByIdAsync(CreateUserProfileRequestModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        try
        {
            if (string.IsNullOrEmpty(UserId))
            {
                return BadRequest("User not found. Please login again.");
            }
            var result = await _service.UpdateUserProfileByIdAsync(UserId, model);
            return !result.IsError ? APIHelper.GenerateSuccessResponse(result.Result) : APIHelper.GenerateFailResponse(result.Result);
        }
        catch (Exception ex)
        {
            var message = ex.Message;
            return BadRequest(500);
        }
    }

    //[Authorize]
    [HttpGet]
    public async Task<ActionResult<GetAllUserInfoResponseModel>> GetAllUserProfileAsync()
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        try
        {
            var result = await _service.GetAllUserInfoAsync();
            return !result.IsError ? APIHelper.GenerateSuccessResponse(result.Result) : APIHelper.GenerateFailResponse(result.Result);
        }
        catch (Exception ex)
        {
            var message = ex.Message;
            return BadRequest(500);
        }
    }

    //[Authorize]
    [HttpGet]
    [Route("getuserprofilebyid")]
    public async Task<ActionResult<GetUserProfileByIdResponseModel>> GetUserById()
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        try
        {
            if (string.IsNullOrEmpty(UserId))
            {
                return BadRequest("User not found. Please login again.");
            }
            var result = await _service.GetUserProfileByIdAsync(UserId);
            return !result.IsError ? APIHelper.GenerateSuccessResponse(result.Result) : APIHelper.GenerateFailResponse(result.Result);
        }
        catch (Exception ex)
        {
            var message = ex.Message;
            return BadRequest(500);
        }
    }
}