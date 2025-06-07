using HotelManagementSystem.Data.Models.Guest;

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

        [HttpGet]
        [Route("GetGuestList")]
        public async Task<IActionResult> GetGuestList()
        {
            try
            {
                var result = await _service.GetAllGuestList();
                if (result.IsError)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, result.Result.RespDescription);
                }
                return Ok(result.Result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("GetGuestById/{id}")]
        public async Task<IActionResult> GetGuestById([FromQuery] Guid id)
        {
            try
            {
                var requestModel = new GetGuestByIdRequestModel
                {
                    GuestId = id
                };

                var result = await _service.GetGuestById(requestModel);
                if (result.IsError)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, result.Result.RespDescription);
                }
                return Ok(result.Result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
