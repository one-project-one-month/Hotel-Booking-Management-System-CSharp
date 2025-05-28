using HotelManagementSystem.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementSystem.Data.Dtos.RoomType
{
    public class UpdateRoomTypeDto
    {
        public string? RoomTypeName { get; set; } = null!;

        public string? Description { get; set; }

        public decimal? Price { get; set; }

        public byte[]? RoomImg { get; set; }

        public string? RoomImgMimeType { get; set; }
    }

    public class UpdateRoomTypeRequestDto: BasedRequestModel
    {
        public string? RoomTypeName { get; set; } = null!;

        public string? Description { get; set; }

        public decimal? Price { get; set; }

        public byte[]? RoomImg { get; set; }

        public string? RoomImgMimeType { get; set; }
    }

    public class UpdateRoomTypeResponseDto : BasedResponseModel
    {
        public string? RoomTypeName { get; set; } = null!;

        public string? Description { get; set; }

        public decimal? Price { get; set; }

        public byte[]? RoomImg { get; set; }

        public string? RoomImgMimeType { get; set; }
    }
}
