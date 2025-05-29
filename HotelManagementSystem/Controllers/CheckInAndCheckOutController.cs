using HotelManagementSystem.Data;
using HotelManagementSystem.Data.Models.CheckInAndCheckOutModel;
using HotelManagementSystem.Helpers;
using HotelManagementSystem.Service.Services.Implementation;
using HotelManagementSystem.Service.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementSystem.Controllers;

[ApiController]
[Route("api/[controller]")]

public class CheckInAndCheckOutController : ControllerBase
{
    private readonly ICheckInAndCheckoutService _checkInAndCheckoutService;

    public CheckInAndCheckOutController(ICheckInAndCheckoutService checkInAndCheckoutService)
    {
        _checkInAndCheckoutService = checkInAndCheckoutService;
    }

    [HttpPost]
    public async Task<ActionResult<CreateCheckInAndCheckOutResponseModel>> CreateCheckInAndCheckOutAsync(CreateCheckInAndCheckOutRequestModel requestModel)
    {
        try
        {
            var result = await _checkInAndCheckoutService.CreateCheckInAndCheckout(requestModel);
            return !result.IsError ? APIHelper.GenerateSuccessResponse(result.Result) : APIHelper.GenerateFailResponse(result.Result);
        }
        catch (Exception ex)
        {
            var message = ex.Message;
            return StatusCode(Convert.ToInt16(ResponseMessageConstants.RESPONSE_CODE_SERVERERROR), ex.Message + ex.InnerException);
        }
    }
}
