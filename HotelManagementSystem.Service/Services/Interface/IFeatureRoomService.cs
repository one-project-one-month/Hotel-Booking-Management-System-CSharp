using HotelManagementSystem.Data.Models.Room;
using HotelManagementSystem.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelManagementSystem.Data.Dtos.Room;

namespace HotelManagementSystem.Service.Services.Interface;

public interface IFeatureRoomService
{
    Task<CustomEntityResult<GetFeatureRoomsResponseModel>> GetFeatureRoom();
}
