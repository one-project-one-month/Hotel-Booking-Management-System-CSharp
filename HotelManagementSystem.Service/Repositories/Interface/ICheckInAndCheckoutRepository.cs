using HotelManagementSystem.Data.Dtos.CheckInAndCheckOutDto;
using HotelManagementSystem.Data.Models.CheckInAndCheckOutDto;

namespace HotelManagementSystem.Service.Repositories.Interface;

public interface ICheckInAndCheckoutRepository
{
    public Task<CustomEntityResult<CreateCheckInAndCheckOutResponseDto>> CreateCheckInAndCheckout(CreateCheckInAndCheckOutRequestDto requestDto);
    public Task<CustomEntityResult<CheckOutResponseDto>> CheckOutAsync(CheckOutRequestDto dto);
};
