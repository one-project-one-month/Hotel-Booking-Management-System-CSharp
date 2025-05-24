using HotelManagementSystem.Data;
using HotelManagementSystem.Data.Models;
using HotelManagementSystem.Data.Models.Booking;
using HotelManagementSystem.Helpers;
using HotelManagementSystem.Service.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementSystem.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("api/[Controller]")]
    public class BookingController : Controller
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }
        [HttpPost]
        [Route("CreateBooking")]
        public async Task<ActionResult<BasedResponseModel>> CreateBooking(CreateBookingRequestModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _bookingService.CreateBooking(model);
                return !result.IsError ? APIHelper.GenerateSuccessResponse(result.Result) : APIHelper.GenerateFailResponse(result.Result);
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                return BadRequest(message);
            }
        }
        [HttpGet]
        [Route("GetBookingById/{bookingId}")]
        public async Task<ActionResult<BasedResponseModel>> GetBookingById(GetBookingByIdRequestModel bookingId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            #region check required
            if (string.IsNullOrEmpty(bookingId.BookingId))
            {
                return BadRequest(nameof(bookingId.BookingId) + ResponseMessageConstants.RESPONSE_MESSAGE_REQUIRED);
            }
            #endregion
            try
            {
                var result = await _bookingService.GetBookingById(bookingId);
                return !result.IsError ? APIHelper.GenerateSuccessResponse(result.Result) : APIHelper.GenerateFailResponse(result.Result);
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                return BadRequest(message);
            }
        }
        [HttpGet]
        [Route("BookingList")]
        public async Task<ActionResult<BasedResponseModel>> BookingList()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _bookingService.BookingList();
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