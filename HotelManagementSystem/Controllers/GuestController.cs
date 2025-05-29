using HotelManagementSystem.Data.Models.Guest;
using HotelManagementSystem.Service.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GuestController : ControllerBase
    {
        private readonly IGuestService _service;
        public GuestController(IGuestService service)
        {
            _service = service;
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("CreateGuest")]
        public async Task<IActionResult> CreateGuest([FromBody] CreateGuestRequestModel model)
        {
            if (model == null)
            {
                return BadRequest("Invalid guest data.");
            }
            var result = await _service.CreateGuest(model);
            if (result.IsError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.Result.RespDescription);
            }
            return Ok(result.Result);
        }
    }
}
