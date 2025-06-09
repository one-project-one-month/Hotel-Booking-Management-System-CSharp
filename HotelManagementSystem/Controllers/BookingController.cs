
using HotelManagementSystem.Service.Services.Implementation;

namespace HotelManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class BookingController : BaseController
    {
        private readonly IBookingService _bookingService;

        public BookingController(IHttpContextAccessor httpContextAccessor, IBookingService bookingService) : base(httpContextAccessor)
        {
            _bookingService = bookingService;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<BasedResponseModel>> CreateBookingByUser(CreateBookingRequestModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                if (string.IsNullOrEmpty(UserId))
                {
                    return BadRequest(nameof(UserId) + ResponseMessageConstants.RESPONSE_MESSAGE_REQUIRED);
                }
                Guid UserID = Guid.Parse(UserId);
                model.UserId = UserID;
                var result = await _bookingService.CreateBookingByUser(model);
                return !result.IsError ? APIHelper.GenerateSuccessResponse(result.Result) : APIHelper.GenerateFailResponse(result.Result);
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                return BadRequest(message);
            }
        }

        [HttpGet]
        [Route("/{bookingId}")]
        public async Task<ActionResult<BasedResponseModel>> GetBookingById(Guid bookingId,GetBookingByIdRequestModel Model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                Model.BookingId = bookingId;
                var result = await _bookingService.GetBookingById(Model);
                return !result.IsError ? APIHelper.GenerateSuccessResponse(result.Result) : APIHelper.GenerateFailResponse(result.Result);
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                return BadRequest(message);
            }
        }

        //[Authorize]
        [HttpGet]
        [Route("/getallbookingbyuserid")]
        public async Task<ActionResult<BasedResponseModel>> GetAllBookingByUserId()
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _bookingService.GetAllBookingByUserId(UserId);
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