using HotelManagementSystem.Data;
using HotelManagementSystem.Data.Dtos.RoomType;
using HotelManagementSystem.Data.Models;
using HotelManagementSystem.Data.Models.Room;
using HotelManagementSystem.Data.Models.RoomType;
using HotelManagementSystem.Helpers;
using HotelManagementSystem.Service.Services.Implementation;
using HotelManagementSystem.Service.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Crypto.Digests;

namespace HotelManagementSystem.Controllers
{
    //[Route("api/[controller]")]
    [Route("admin/roomtypes")]
    [ApiController]
    public class RoomTypeController : ControllerBase
    {
        private readonly IRoomTypeService _roomTypeService;

        public RoomTypeController(IRoomTypeService roomTypeService)
        {
            _roomTypeService = roomTypeService;
        }


        [HttpGet]
        public async Task<ActionResult<BasedResponseModel>> GetRoomTypes()
        {
            try
            {
                var result = await _roomTypeService.GetRoomTypes();
                return !result.IsError ? APIHelper.GenerateSuccessResponse(result.Result) : APIHelper.GenerateFailResponse(result.Result); 
            } catch (Exception ex)
            {
                return StatusCode(Convert.ToInt16(ResponseMessageConstants.RESPONSE_CODE_SERVERERROR), ex.Message + ex.InnerException);
            }
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<BasedResponseModel>> GetRoomTypeById(Guid id)
        {
            try
            {
                var result = await _roomTypeService.GetRoomTypeById(id);
                return !result.IsError? APIHelper.GenerateSuccessResponse(result.Result): APIHelper.GenerateFailResponse(result.Result);
            } catch (Exception ex)
            {
                return StatusCode(Convert.ToInt16(ResponseMessageConstants.RESPONSE_CODE_SERVERERROR), ex.Message + ex.InnerException);
            }
        }

        [HttpPost]
        public async Task<ActionResult<BasedResponseModel>> CreateRoomType(CreateRoomTypeRequestModel requestModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            #region check required

            if (String.IsNullOrEmpty(requestModel.RoomTypeName))
            {
                return BadRequest(nameof(requestModel.RoomTypeName) + ResponseMessageConstants.RESPONSE_MESSAGE_REQUIRED);
            }
            if(!requestModel.Price.HasValue || requestModel.Price.Value <=0 )
            {
                return BadRequest(nameof(requestModel.Price) + ResponseMessageConstants.RESPONSE_MESSAGE_REQUIRED);
            }
            #endregion

            try
            {
                var result = await _roomTypeService.CreateRoomType(requestModel);
                return !result.IsError ? APIHelper.GenerateSuccessResponse(result.Result) : APIHelper.GenerateFailResponse(result.Result);
            } catch (Exception ex)
            {
                return StatusCode(Convert.ToInt16(ResponseMessageConstants.RESPONSE_CODE_SERVERERROR), ex.Message + ex.InnerException);
            }          
        }


        [HttpPatch("{id}")]
        public async Task<ActionResult<BasedResponseModel>> UpdateRoomType (Guid id, UpdateRoomTypeRequestModel requestModel)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _roomTypeService.UpdateRoomType(id, requestModel);
                return !result.IsError ? APIHelper.GenerateSuccessResponse(result.Result) : APIHelper.GenerateFailResponse(result.Result);
            } catch(Exception ex)
            {
                return StatusCode(Convert.ToInt16(ResponseMessageConstants.RESPONSE_CODE_SERVERERROR), ex.Message + ex.InnerException);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<BasedResponseModel>> DeleteRoomType (Guid id)
        {
            try
            {
                var result = await _roomTypeService.DeleteRoomType(id);
                return !result.IsError ? APIHelper.GenerateSuccessResponse(result.Result) : APIHelper.GenerateFailResponse(result.Result);
            } catch (Exception ex)
            {
                return StatusCode(Convert.ToInt16(ResponseMessageConstants.RESPONSE_CODE_SERVERERROR), ex.Message + ex.InnerException);
            }
        }

    }
}
