namespace HotelManagementSystem.Data.Models.Room;

public class CreateRoomModel
{
    
}

public class CreateRoomRequestModel
{
    public string RoomNo { get; set; }

    public string? RoomStatus { get; set; }

    public byte[]? ImgUrl { get; set; }

    public int? GuestLimit { get; set; }
}

public class CreateRoomResponseModel : BasedResponseModel
{
    public Guid RoomId { get; set; }
}