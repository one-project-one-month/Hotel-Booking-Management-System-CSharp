using HotelManagementSystem.Data;
using HotelManagementSystem.Data.Data;
using HotelManagementSystem.Data.Dtos.Room;
using HotelManagementSystem.Data.Entities;
using HotelManagementSystem.Service.Exceptions;
using HotelManagementSystem.Service.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementSystem.Service.Repositories.Implementation
{
    public class FeatureRoomRepository: IFeatureRoomRepository
    {
        private readonly HotelDbContext _hotelDbContext;
        public FeatureRoomRepository(HotelDbContext AppDbConnect)
        {
            _hotelDbContext = AppDbConnect;
        }

        public CustomEntityResult<GetFeatureRoomsResponseDto> GetRoomByFeature()
        {
            try
            {
                var lst = _hotelDbContext.TblRooms.Where(x => x.Is_Featured == true).ToList();
                if(lst.Count <1 )
                {
                    throw new FeatureRoomNotExistException("Featured room doesn't exist.");
                }

                var RoomResponse = lst.Select(b => new GetFeatureRoomResponseDto
                {
                    RoomId = b.RoomId,
                    RoomNo = b.RoomNo,
                    RoomTypeId = b.RoomTypeId,
                    RoomStatus = b.RoomStatus,
                    GuestLimit = b.GuestLimit
                }).ToList();

                var getFeatureRoomsResponse = new GetFeatureRoomsResponseDto
                {
                    Rooms = RoomResponse
                };

                return CustomEntityResult<GetFeatureRoomsResponseDto>.GenerateSuccessEntityResult(getFeatureRoomsResponse);
            }
            catch (Exception ex)
            {
                throw new Exception("Fail to get selected room.", ex);
            }
        }
    }
}