using HotelManagementSystem.Data.Entities;
using HotelManagementSystem.Data.Models;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementSystem.Data.Dtos.Room;

public class GetFeatureRoomResponseDto: BasedResponseModel
{
    public Guid RoomId { get; set; }

    public string RoomNo { get; set; } = null!;

    public Guid RoomTypeId { get; set; }

    public string? RoomStatus { get; set; }

    public int? GuestLimit { get; set; }
}

public class GetFeatureRoomsResponseDto : BasedResponseModel
{
    public List<GetFeatureRoomResponseDto> Rooms { get; set; } = new();
}
