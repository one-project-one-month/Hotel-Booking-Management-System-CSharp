using HotelManagementSystem.Data.Models.CheckInAndCheckOutDto;
using HotelManagementSystem.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementSystem.Service.Repositories.Interface;

public interface ICheckInAndCheckoutRepository
{
    public Task<CustomEntityResult<CreateCheckInAndCheckOutResponseDto>> CreateCheckInAndCheckout(CreateCheckInAndCheckOutRequestDto requestDto);
};
