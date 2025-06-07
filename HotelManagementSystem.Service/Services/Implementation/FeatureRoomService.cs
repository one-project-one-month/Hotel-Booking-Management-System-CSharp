using HotelManagementSystem.Data.Models.Room;
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

    public CustomEntityResult<GetFeatureRoomsResponseDto> GetFeatureRoom()
    {
        try
        {
            var result = _featureRoomRepo.GetRoomByFeature();
            if (result.IsError)
            {
                return CustomEntityResult<GetFeatureRoomsResponseDto>.GenerateFailEntityResult(result.Result.RespCode, result.Result.RespDescription);
            }
            return result;

        }
        catch (Exception)
        {

            throw;
        }
    }

}
