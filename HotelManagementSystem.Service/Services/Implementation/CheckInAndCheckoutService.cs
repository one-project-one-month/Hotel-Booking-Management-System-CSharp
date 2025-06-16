using HotelManagementSystem.Data.Dtos.BookingControl;
using HotelManagementSystem.Data.Dtos.CheckInAndCheckOutDto;
using HotelManagementSystem.Data.Models.CheckInAndCheckOutDto;
using HotelManagementSystem.Data.Models.CheckInAndCheckOutModel;
using HotelManagementSystem.Data.Models.Room;
using HotelManagementSystem.Service.Repositories.Implementation;
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

    public async Task<CustomEntityResult<CheckOutResponseModel>> CheckOutAsync(CheckOutRequestModel model)
    {
        try
        {
            var resquest = new CheckOutRequestDto
            {
                GuestId = model.GuestId,
            };

            var checkOut = await _checkInAndCheckoutRepository.CheckOutAsync(resquest);
            if(checkOut.IsError)
            {
                return CustomEntityResult<CheckOutResponseModel>.GenerateFailEntityResult(checkOut.Result.RespCode, checkOut.Result.RespDescription);
            }
            var response = new CheckOutResponseModel
            {
                InvoiceId = checkOut.Result.InvoiceId,
                GuestId = checkOut.Result.GuestId,
                CheckInTime = checkOut.Result.CheckInTime,
                CheckOutTime = checkOut.Result.CheckOutTime,
                Extracharges = checkOut.Result.Extracharges,
                DepositeAmount = checkOut.Result.DepositeAmount,
                TotalAmount = checkOut.Result.TotalAmount,
                PaymentType = checkOut.Result.PaymentType,
            };
            return CustomEntityResult<CheckOutResponseModel>.GenerateSuccessEntityResult(response);
        }
        catch (Exception ex)
        {
            return CustomEntityResult<CheckOutResponseModel>.GenerateFailEntityResult(ResponseMessageConstants.RESPONSE_CODE_SERVERERROR, ex.Message + (ex.InnerException?.Message ?? ""));
        }
    }
}
