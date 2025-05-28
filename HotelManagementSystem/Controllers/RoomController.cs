using HotelManagementSystem.Data;
using HotelManagementSystem.Data.Models;
using HotelManagementSystem.Data.Models.Room;
using HotelManagementSystem.Helpers;
using HotelManagementSystem.Service.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementSystem.Controllers;

[ApiController]
[AllowAnonymous]
[Route("api/[Controller]")]
public class RoomController : ControllerBase
{
    private readonly IRoomService _service;
    
    public RoomController(IRoomService service)
    {
        _service = service;
    }

    [HttpPost]
    [Route("createroom")]
    public async Task<ActionResult<BasedResponseModel>> CreateRoom(CreateRoomRequestModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        #region check required

        if (String.IsNullOrEmpty(model.RoomNo))
        {
            return BadRequest(nameof(model.RoomNo) + ResponseMessageConstants.RESPONSE_MESSAGE_REQUIRED);
        }

        #endregion

        #region check format

        

        #endregion

        try
        {
            var result = await _service.CreateRoom(model);
            return !result.IsError ? APIHelper.GenerateSuccessResponse(result.Result) : APIHelper.GenerateFailResponse(result.Result);
        }
        catch (Exception ex)
        {
            var message = ex.Message;
            return StatusCode(Convert.ToInt16(ResponseMessageConstants.RESPONSE_CODE_SERVERERROR), ex.Message + ex.InnerException);
        }
    }

}