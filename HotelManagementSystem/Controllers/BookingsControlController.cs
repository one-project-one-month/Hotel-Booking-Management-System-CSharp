using HotelManagementSystem.Data;
using HotelManagementSystem.Data.Dtos.BookingControl;
using HotelManagementSystem.Helpers;
using HotelManagementSystem.Service.Services.Interface;
using Microsoft.AspNetCore.Mvc;

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
    public async Task<ActionResult<GetBookingsResponseDto>> IndexAsync()
    {
        try
        {
            var result = await _bookingControlService.GetBookingControl();
            return !result.IsError ? APIHelper.GenerateSuccessResponse(result.Result) : APIHelper.GenerateFailResponse(result.Result);
        }
        catch (Exception ex)
        {
            var message = ex.Message;
            return StatusCode(Convert.ToInt16(ResponseMessageConstants.RESPONSE_CODE_SERVERERROR), ex.Message + ex.InnerException);
        }
    }
}
