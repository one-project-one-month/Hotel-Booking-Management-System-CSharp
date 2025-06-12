using HotelManagementSystem.Data.Models.CheckInAndCheckOutModel;
using HotelManagementSystem.Service.Services.Implementation;

namespace HotelManagementSystem.Controllers;

[ApiController]
[Route("api/[controller]")]

public class CheckInAndCheckOutController : ControllerBase
{
    private readonly ICheckInAndCheckoutService _service;

    public CheckInAndCheckOutController(ICheckInAndCheckoutService checkInAndCheckoutService)
    {
        _service = checkInAndCheckoutService;
    }

    [HttpPost]
    [Route("/admin/checkincheckout")]
    public async Task<ActionResult<CreateCheckInAndCheckOutResponseModel>> CreateCheckInAndCheckOutAsync(CreateCheckInAndCheckOutRequestModel requestModel)
    {
        try
        {
            var result = await _service.CreateCheckInAndCheckout(requestModel);
            return !result.IsError ? APIHelper.GenerateSuccessResponse(result.Result) : APIHelper.GenerateFailResponse(result.Result);
        }
        catch (Exception ex)
        {
            var message = ex.Message;
            return StatusCode(Convert.ToInt16(ResponseMessageConstants.RESPONSE_CODE_SERVERERROR), ex.Message + ex.InnerException);
        }
    }

    [HttpPost]
    [Route("admin/checkout")]
    public async Task<ActionResult<CheckOutResponseModel>> CheckOutAsync(CheckOutRequestModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        try
        {
            var result = await _service.CheckOutAsync(model);
            return !result.IsError
                ? APIHelper.GenerateSuccessResponse(result.Result)
                : APIHelper.GenerateFailResponse(result.Result);
        }
        catch(Exception ex)
        {
            var message = ex.Message;
            return BadRequest(500);
        }
    }
}
