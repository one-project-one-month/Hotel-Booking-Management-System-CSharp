namespace HotelManagementSystem.Data.Entities;

public partial class TblRoom
{
    public Guid RoomId { get; set; }

    public string RoomNo { get; set; } = null!;

    public Guid RoomTypeId { get; set; }

    public string? RoomStatus { get; set; }

    public byte[]? ImgUrl { get; set; }

    public int? GuestLimit { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual TblRoomType RoomType { get; set; } = null!;

    public virtual ICollection<TblRoomBooking> TblRoomBookings { get; set; } = new List<TblRoomBooking>();
}
