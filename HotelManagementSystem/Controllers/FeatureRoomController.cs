using HotelManagementSystem.Data.Models;
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
        [Route("FeatureRoom")]
        public ActionResult<BasedResponseModel> GetFeatureRoom()
        {
            try
            {
                var result = _featureRoomService.GetFeatureRoom();
                return !result.IsError ? APIHelper.GenerateSuccessResponse(result.Result) : APIHelper.GenerateFailResponse(result.Result);
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                return BadRequest(500);
            }

        }
    }
}
