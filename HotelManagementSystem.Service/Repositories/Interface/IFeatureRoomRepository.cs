using HotelManagementSystem.Data;
using HotelManagementSystem.Data.Dtos.Room;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementSystem.Service.Repositories.Interface
{
    public interface IFeatureRoomRepository
    {
        Task<CustomEntityResult<GetFeatureRoomsResponseDto>> GetRoomByFeature();
    }
}
