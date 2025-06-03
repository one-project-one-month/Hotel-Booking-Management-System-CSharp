using System.Text.Json;
using HotelManagementSystem.Features.User;
using HotelManagementSystem.Helpers;
using HotelManagementSystem.Models;
using HotelManagementSystem.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementSystem.Api.Controllers;

[ApiController]
[AllowAnonymous]
[Route("api/[Controller]")]
public class UserController : ControllerBase
{
    private readonly BL_User _blUser;

    public UserController(BL_User blUser)
    {
        _blUser = blUser;
    }

    [HttpPost]
    [Route("CreateUser")]
    public async Task<ActionResult<BasedResponseModel>> CreateUser([FromBody] CreateUserRequestModel model)
    {
        #region UserGetClaimsValue

        //var useremail = _httpContextAccessor.HttpContext?.User.FindFirstValue(JwtRegisteredClaimNames.Email);
        //var displayname = _httpContextAccessor.HttpContext?.User.FindFirstValue(JwtRegisteredClaimNames.Name);

        #endregion

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            #region CheckRequiredField
            // if (String.IsNullOrEmpty(model.UserId))
            // {
            //     return APIHelper.GenerateResponseForRequiredField(nameof(model.UserId), _sharedLocalizer);
            // }
            #endregion

            #region Check Format
            // if(model.UserType != EntitiesConstant.USER_TYPE.USER.GetHashCode())
            // {
            //     return BadRequest(ErrorMessageConstant.EM_UserTypeNotAcceptable); ///Not Acceptable
            // }
            #endregion

            var result = await _blUser.CreateUser(model);

            return !result.IsError ? APIHelper.GenerateSuccessResponse(result.Result) : APIHelper.GenerateFailResponse(result.Result);
        }
        catch (Exception ex)
        {
            var message = ex.Message;
            return BadRequest(500);
        }
    }
}