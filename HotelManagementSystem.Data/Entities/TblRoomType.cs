using System;
using System.Collections.Generic;

namespace HotelManagementSystem.Data.Entities;

public partial class TblRoomType
{
    public Guid RoomTypeId { get; set; }

    public string RoomTypeName { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public byte[]? ImgUrl { get; set; }

    public virtual ICollection<TblRoom> TblRooms { get; set; } = new List<TblRoom>();
}
