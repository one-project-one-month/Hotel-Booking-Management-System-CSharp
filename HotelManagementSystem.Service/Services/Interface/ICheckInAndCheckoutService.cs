using HotelManagementSystem.Data.Models.CheckInAndCheckOutModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementSystem.Service.Services.Interface;

public interface ICheckInAndCheckoutService
{
    public Task<CustomEntityResult<CreateCheckInAndCheckOutResponseModel>> CreateCheckInAndCheckout(CreateCheckInAndCheckOutRequestModel requestModel);
    public Task<CustomEntityResult<CheckOutResponseModel>> CheckOutAsync(CheckOutRequestModel model);
}
