using HotelManagementSystem.Data.Models.Booking;
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
        public async Task<IActionResult> CreateBooking(CreateBookingRequestModel bookingModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _bookingService.CreaeBooking(bookingModel);
                return !result.IsError ? Ok(result.Result) : BadRequest(result.Result);
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                return StatusCode(500, message);
            }
        }
    }
}
