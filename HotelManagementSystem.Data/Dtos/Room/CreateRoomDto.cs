using HotelManagementSystem.Data.Models;

namespace HotelManagementSystem.Data.Dtos.Room;

public class CreateRoomDto
{
    
}

public class CreateRoomRequestDto
{
    public string RoomNo { get; set; }

    public string? RoomStatus { get; set; }

    public byte[]? ImgUrl { get; set; }

    public int? GuestLimit { get; set; }
}

public class CreateRoomResponseDto : BasedResponseModel
{
    public Guid RoomId { get; set; }
}