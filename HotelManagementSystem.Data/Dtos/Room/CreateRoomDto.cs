using HotelManagementSystem.Data.Models;

namespace HotelManagementSystem.Data.Dtos.Room;

public class CreateRoomDto
{
    
}

public class CreateRoomRequestDto
{
    public string RoomNo { get; set; } = null!;

    public string? RoomStatus { get; set; }

    public int? GuestLimit { get; set; }

    public Guid RoomTypeId { get; set; }

    public bool IsFeatured { get; set; }
}

public class CreateRoomResponseDto : BasedResponseModel
{
    public Guid RoomId { get; set; }
}