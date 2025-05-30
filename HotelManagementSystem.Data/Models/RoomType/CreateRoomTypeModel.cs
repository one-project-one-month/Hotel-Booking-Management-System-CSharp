using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementSystem.Data.Models.RoomType
{
    public class CreateRoomTypeModel
    {
    }

    public class CreateRoomTypeRequestModel : BasedRequestModel
    {
        public string RoomTypeName { get; set; } = null!;

        public string? Description { get; set; }

        public decimal? Price { get; set; }

        public string? RoomImg { get; set; }

        public string? RoomImgMimeType { get; set; }
    }

    public class CreateRoomTypeResponseModel : BasedResponseModel
    {
        public Guid RoomTypeId { get; set; }
    }
}
