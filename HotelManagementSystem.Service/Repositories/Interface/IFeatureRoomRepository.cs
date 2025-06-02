using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementSystem.Service.Repositories.Interface
{
    public interface IFeatureRoomRepository
    {
        public CustomEntityResult<GetFeatureRoomsResponseDto> GetRoomByFeature();
    }
}
