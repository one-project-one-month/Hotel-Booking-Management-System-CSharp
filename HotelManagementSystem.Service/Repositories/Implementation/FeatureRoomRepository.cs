using HotelManagementSystem.Data;
using HotelManagementSystem.Data.Data;
using HotelManagementSystem.Data.Dtos.Room;
using HotelManagementSystem.Data.Entities;
using HotelManagementSystem.Service.Exceptions;
using HotelManagementSystem.Service.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
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

        public async Task<CustomEntityResult<GetFeatureRoomsResponseDto>> GetRoomByFeature()
        {
            try
            {
                var lst = await _hotelDbContext.TblRooms.Where(x => x.Is_Featured == true).ToListAsync();
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
                return CustomEntityResult<GetFeatureRoomsResponseDto>.GenerateFailEntityResult(ResponseMessageConstants.RESPONSE_CODE_SERVERERROR, ex.Message + ex.InnerException);
            }
        }
    }
}