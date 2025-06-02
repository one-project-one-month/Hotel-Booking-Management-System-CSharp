namespace HotelManagementSystem.Data.Entities;

public partial class TblRoomTypeImage
{
    public Guid RoomTypeId { get; set; }

    public byte[]? RoomImg { get; set; }

    public string? RoomImgMimeType { get; set; }

    public virtual TblRoomType RoomType { get; set; } = null!;
}
