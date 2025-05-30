using System;
using System.Collections.Generic;

namespace HotelManagementSystem.Data.Entities;

public partial class TblUserProfileImage
{
    public Guid UserId { get; set; }

    public byte[]? ProfileImg { get; set; }

    public string? ProfileImgMimeType { get; set; }

    public virtual TblUser User { get; set; } = null!;
}
