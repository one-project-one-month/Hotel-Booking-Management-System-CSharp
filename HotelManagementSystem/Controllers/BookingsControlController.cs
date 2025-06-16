using HotelManagementSystem.Data.Models.BookingControl;

namespace HotelManagementSystem.Controllers;

[ApiController]
[Route("api/[Controller]")]
public class BookingsControlController : ControllerBase
{
    private readonly IBookingControlService _bookingControlService;

    public BookingsControlController(IBookingControlService bookingControlService)
    {
        _bookingControlService = bookingControlService;
    }
    [HttpGet]
    [Route("/admin/Bookings")]
    public async Task<ActionResult<GetBookingsResponseModel>> GetBookings()
    {
        try
        {
            var result = await _bookingControlService.GetBookings();
            return !result.IsError ? APIHelper.GenerateSuccessResponse(result.Result) : APIHelper.GenerateFailResponse(result.Result);
        }
        catch (Exception ex)
        {
            var message = ex.Message;
            return StatusCode(Convert.ToInt16(ResponseMessageConstants.RESPONSE_CODE_SERVERERROR), ex.Message + ex.InnerException);
        }
    }

    [HttpPatch]
    [Route("/admin/UpdateBooking/{BookingId}")]
    public async Task<ActionResult<UpdateBookingResponseModel>> UpdateBooking(Guid BookingId, [FromBody] UpdateBookingRequestModel requestModel)
    {
        try
        {
            if (BookingId != requestModel.BookingId)
            {
                return BadRequest("Booking ID in URL does not match body.");
            }

            var result = await _bookingControlService.UpdateBooking(requestModel);
            return !result.IsError
                ? APIHelper.GenerateSuccessResponse(result.Result)
                : APIHelper.GenerateFailResponse(result.Result);
        }
        catch (Exception ex)
        {
            var message = ex.Message;
            return StatusCode(Convert.ToInt16(ResponseMessageConstants.RESPONSE_CODE_SERVERERROR), ex.Message + ex.InnerException);
        }
    }

    [HttpPost]
    [Route("/admin/DeleteBooking")]
    public async Task<ActionResult<DeleteBookingResponseModel>> DeleteBooking(DeleteBookingRequestModel Booking)
    {
        try
        {
            var result = await _bookingControlService.DeleteBooking(Booking);
            return !result.IsError ? APIHelper.GenerateSuccessResponse(result.Result) : APIHelper.GenerateFailResponse(result.Result);
        }
        catch (Exception ex)
        {
            var message = ex.Message;
            return StatusCode(Convert.ToInt16(ResponseMessageConstants.RESPONSE_CODE_SERVERERROR), ex.Message + ex.InnerException);
        }
    }

    [HttpPost]
    [Route("/admin/CreateBooking")]
    public async Task<ActionResult<CreateBookingByAdminResponseModel>> CreateBooking(CreateBookingByAdminRequestModel model)
    {
        try
        {
            var result = await _bookingControlService.CreateBookingByAdmin(model);
            return !result.IsError ? APIHelper.GenerateSuccessResponse(result.Result) : APIHelper.GenerateFailResponse(result.Result);
        }
        catch (Exception ex)
        {
            var message = ex.Message;
            return StatusCode(Convert.ToInt16(ResponseMessageConstants.RESPONSE_CODE_SERVERERROR), ex.Message + ex.InnerException);
        }
    }
}
