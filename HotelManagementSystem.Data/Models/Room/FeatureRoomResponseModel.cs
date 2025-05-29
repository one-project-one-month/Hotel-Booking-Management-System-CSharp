using HotelManagementSystem.Data.Dtos.Room;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementSystem.Data.Models.Room;

public class GetFeatureRoomResponseModel : BasedResponseModel
{
    public Guid RoomId { get; set; }

    public string RoomNo { get; set; } = null!;

    public Guid RoomTypeId { get; set; }

    public string? RoomStatus { get; set; }

    public int? GuestLimit { get; set; }
 
}
public class GetFeatureRoomsResponseModel : BasedResponseModel
{
    public List<GetFeatureRoomResponseModel> Rooms { get; set; } = new();
}
