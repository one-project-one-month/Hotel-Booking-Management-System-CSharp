using HotelManagementSystem.Data;
using HotelManagementSystem.Data.Dtos.BookingControl;
using HotelManagementSystem.Data.Models.CheckInAndCheckOutDto;
using HotelManagementSystem.Data.Models.CheckInAndCheckOutModel;
using HotelManagementSystem.Service.Repositories.Implementation;
using HotelManagementSystem.Service.Repositories.Interface;
using HotelManagementSystem.Service.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementSystem.Service.Services.Implementation;

public class CheckInAndCheckoutService : ICheckInAndCheckoutService
{
    private readonly ICheckInAndCheckoutRepository _checkInAndCheckoutRepository;

    public CheckInAndCheckoutService(ICheckInAndCheckoutRepository checkInAndCheckoutRepository)
    {
        _checkInAndCheckoutRepository = checkInAndCheckoutRepository;
    }
    public async Task<CustomEntityResult<CreateCheckInAndCheckOutResponseModel>> CreateCheckInAndCheckout(CreateCheckInAndCheckOutRequestModel requestModel)
    {

        var checkInAndCheckOutResponseDto = new CreateCheckInAndCheckOutRequestDto()
        {
            GuestId = requestModel.GuestId,
            CheckInTime = requestModel.CheckInTime,
            CheckOutTime = requestModel.CheckOutTime,
            ExtraCharges = requestModel.ExtraCharges,
            Status = requestModel.Status
        };
        var result = await _checkInAndCheckoutRepository.CreateCheckInAndCheckout(checkInAndCheckOutResponseDto);
        if (result.IsError)
        {
            return CustomEntityResult<CreateCheckInAndCheckOutResponseModel>.GenerateFailEntityResult(result.Result.RespCode, result.Result.RespDescription);
        }

        var responseModle = new CreateCheckInAndCheckOutResponseModel
        {
            GuestNRC = result.Result.GuestNRC,
            GuestPhone = result.Result.GuestPhone,
            CheckInTime = result.Result.CheckInTime,
            CheckOutTime = result.Result.CheckOutTime,
            ExtraCharges = result.Result.ExtraCharges,
            Status = result.Result.Status
        };
        return CustomEntityResult<CreateCheckInAndCheckOutResponseModel>.GenerateSuccessEntityResult(responseModle);
    }
}
