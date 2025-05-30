using HotelManagementSystem.Data;
using HotelManagementSystem.Data.Dtos.BookingControl;
using HotelManagementSystem.Data.Models.BookingControl;
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

    //[HttpPut("{BookingId}")]
    [HttpPost]
    [Route("/admin/UpdateBooking")]
    public async Task<ActionResult<UpdateBookingResponseModel>> UpdateBooking(UpdateBookingRequestModel requestModel)
    {
        try
        {
            var result = await _bookingControlService.UpdateBooking(requestModel);
            return !result.IsError ? APIHelper.GenerateSuccessResponse(result.Result) : APIHelper.GenerateFailResponse(result.Result);
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
}
