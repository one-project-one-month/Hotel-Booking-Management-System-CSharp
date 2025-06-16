using HotelManagementSystem.Data.Models.SearchRoom;
using HotelManagementSystem.Service;

namespace HotelManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchRoomController : ControllerBase
    {
        private readonly ISearchRoomService _service;

        public SearchRoomController(ISearchRoomService service)
        {
            _service = service;
        }

        [HttpPost]
        [Route("searchroom")]
        public async Task<ActionResult<BasedResponseModel>> SearchRoom([FromBody] SearchRoomRequestModel model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (model.RoomTypeId is null && model.GuestLimit <=0 && model.CheckInDate is null && model.CheckOutDate is null)
            {
                return BadRequest();
            }
            try
            {
                var result = await _service.SearchRoom(model);



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
