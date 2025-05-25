using HotelManagementSystem.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementSystem.Data.Dtos.RoomType
{
    public class CreateRoomTypeDto
    {
    }

    public class CreateRoomTypeRequestDto : BasedRequestModel
    {
        public string RoomTypeName { get; set; } = null!;

        public string? Description { get; set; }

        public decimal Price { get; set; }

        public byte[]? ImgUrl { get; set; }
    }
}
