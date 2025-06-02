using HotelManagementSystem.Data;
using HotelManagementSystem.Data.Models;
using HotelManagementSystem.Data.Models.Room;
using HotelManagementSystem.Helpers;
using HotelManagementSystem.Service.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementSystem.Controllers;

[ApiController]
[Route("api/[Controller]")]
public class RoomController : ControllerBase
{
    private readonly IRoomService _service;
    
    public RoomController(IRoomService service)
    {
        _service = service;
    }

    [HttpGet]
    [Route("getrooms")]
    public async Task<ActionResult<BasedResponseModel>> GetRooms()
    {
        var result = await _service.GetRooms();
        return !result.IsError ? APIHelper.GenerateSuccessResponse(result.Result) : APIHelper.GenerateFailResponse(result.Result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BasedResponseModel>> GetRoomById(Guid id)
    {
        var result = await _service.GetRoomById(id); 
        return !result.IsError ? APIHelper.GenerateSuccessResponse(result.Result) : APIHelper.GenerateFailResponse(result.Result);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [Route("createroom")]
    public async Task<ActionResult<BasedResponseModel>> CreateRoom(CreateRoomRequestModel requestModel)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        #region check required

        if (String.IsNullOrEmpty(requestModel.RoomNo))
        {
            return BadRequest(nameof(requestModel.RoomNo) + ResponseMessageConstants.RESPONSE_MESSAGE_REQUIRED);
        }

        if(requestModel.RoomTypeId == Guid.Empty)
        {
            return BadRequest(nameof(requestModel.RoomTypeId) + ResponseMessageConstants.RESPONSE_MESSAGE_REQUIRED);
        }

        #endregion

        #region check format



        #endregion

        try
        {
            var result = await _service.CreateRoom(requestModel);
            return !result.IsError ? APIHelper.GenerateSuccessResponse(result.Result) : APIHelper.GenerateFailResponse(result.Result);
        }
        catch (Exception ex)
        {
            var message = ex.Message;
            return StatusCode(Convert.ToInt16(ResponseMessageConstants.RESPONSE_CODE_SERVERERROR), ex.Message + ex.InnerException);
        }
    }

    [HttpPatch("updateroom/{id}")]
    public async Task<ActionResult<BasedResponseModel>> UpdateRoom(Guid id, UpdateRoomRequestModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        try
        {
            var result = await _service.UpdateRoom(id,model);
            return !result.IsError ? APIHelper.GenerateSuccessResponse(result.Result) : APIHelper.GenerateFailResponse(result.Result);
        }
        catch (Exception ex)
        {
            var message = ex.Message;
            return StatusCode(Convert.ToInt16(ResponseMessageConstants.RESPONSE_CODE_SERVERERROR), ex.Message + ex.InnerException);
        }
    }

    [HttpDelete("deleteroom/{id}")]
    public async Task<ActionResult<BasedResponseModel>> DeleteRoom(Guid id)
    {
        var result = await _service.DeleteRoom(id);
        return !result.IsError ? APIHelper.GenerateSuccessResponse(result.Result) : APIHelper.GenerateFailResponse(result.Result);
    }
}