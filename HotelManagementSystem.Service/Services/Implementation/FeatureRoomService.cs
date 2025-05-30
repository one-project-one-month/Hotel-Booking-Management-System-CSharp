using HotelManagementSystem.Data;
using HotelManagementSystem.Data.Dtos.Room;
using HotelManagementSystem.Data.Models.Room;
using HotelManagementSystem.Service.Repositories.Interface;
using HotelManagementSystem.Service.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementSystem.Service.Services.Implementation;

public class FeatureRoomService: IFeatureRoomService
{
    public readonly IFeatureRoomRepository _featureRoomRepo;
    public FeatureRoomService(IFeatureRoomRepository featureRoomReepo)
    {
        _featureRoomRepo = featureRoomReepo;
    }

    public CustomEntityResult<GetFeatureRoomsResponseModel> GetFeatureRoom()
    {
        try
        {
            var result = _featureRoomRepo.GetRoomByFeature();
            if (result.IsError)
            {
                return CustomEntityResult<GetFeatureRoomsResponseModel>.GenerateFailEntityResult(result.Result.RespCode, result.Result.RespDescription);
            }

            var getFeatureRoomsResponse = new GetFeatureRoomsResponseModel
            {
                Rooms  = result.Result.Rooms.Select(b => new GetFeatureRoomResponseModel
            {
                RoomId = b.RoomId,
                RoomNo = b.RoomNo,
                RoomTypeId = b.RoomTypeId,
                RoomStatus = b.RoomStatus,
                GuestLimit = b.GuestLimit
            }).ToList()
        };
            return CustomEntityResult<GetFeatureRoomsResponseModel>.GenerateSuccessEntityResult(getFeatureRoomsResponse);

        }
        catch (Exception ex)
        {

            return CustomEntityResult<GetFeatureRoomsResponseModel>.GenerateFailEntityResult(ResponseMessageConstants.RESPONSE_CODE_SERVERERROR, ex.Message + (ex.InnerException?.Message ?? ""));
        }
    }

}
