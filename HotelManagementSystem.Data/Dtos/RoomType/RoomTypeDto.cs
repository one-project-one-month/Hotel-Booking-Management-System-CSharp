using HotelManagementSystem.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementSystem.Data.Dtos.RoomType
{
    public class RoomTypeDto
    {
        public Guid RoomTypeId { get; set; }
        public string? RoomTypeName { get; set; } = null!;

        public string? Description { get; set; }

        public decimal? Price { get; set; }

        public byte[]? RoomImg { get; set; }

        public string? RoomImgMimeType { get; set; }
    }

    public class RoomTypeResponseDto : BasedResponseModel
    {
        public RoomTypeDto RoomType { get; set; }
    }

    public class RoomTypeListResponseDto: BasedResponseModel
    {
        public List<RoomTypeDto> RoomTypeList { get; set; }
    }

}
