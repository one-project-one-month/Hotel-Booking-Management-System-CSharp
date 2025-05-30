using HotelManagementSystem.Data;
using HotelManagementSystem.Data.Models;
using HotelManagementSystem.Data.Models.Room;
using HotelManagementSystem.Helpers;
using HotelManagementSystem.Service.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MimeKit.Cryptography;

namespace HotelManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeatureRoomController : ControllerBase
    {
        private readonly IFeatureRoomService _featureRoomService;

        public FeatureRoomController(IFeatureRoomService featureRoomService)
        {
            _featureRoomService = featureRoomService;
        }

        [HttpGet]
        [Route("GetFeatureRoom")]
        public async Task<ActionResult<GetFeatureRoomsResponseModel>> GetFeatureRoom()
        {
            if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
            try
            {
                var result = await _featureRoomService.GetFeatureRoom();
                return !result.IsError ? APIHelper.GenerateSuccessResponse(result.Result) : APIHelper.GenerateFailResponse(result.Result);
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                return BadRequest(message);
            }

        }
    }
}
