namespace HotelManagementSystem.Data.Models.Room;

public class CreateRoomModel
{
    
}

public class CreateRoomRequestModel: BasedRequestModel
{
    public string? RoomNo { get; set; }

    public string? RoomStatus { get; set; }

    public int? GuestLimit { get; set; }

    public Guid RoomTypeId { get; set; }

    public bool IsFeatured { get; set; }
}

public class CreateRoomResponseModel : BasedResponseModel
{
    public Guid RoomId { get; set; }
}