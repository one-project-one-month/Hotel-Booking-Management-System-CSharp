using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementSystem.Data.Models.RoomType
{
    public class UpdateRoomTypeModel
    {

    }

    public class UpdateRoomTypeRequestModel: BasedRequestModel
    {
        public string? RoomTypeName { get; set; } = null!;

        public string? Description { get; set; }

        public decimal? Price { get; set; }

        public string? RoomImg { get; set; }

        public string? RoomImgMimeType { get; set; }
    }

    public class UpdateRoomTypeResponseModel : BasedResponseModel
    {
        public string? RoomTypeName { get; set; } = null!;

        public string? Description { get; set; }

        public decimal? Price { get; set; }

        public string? RoomImg { get; set; }

        public string? RoomImgMimeType { get; set; }
    }
}
