using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementSystem.Data.Models.Room
{
    public class UpdateRoomModel
    {
        public string? RoomNo { get; set; }
        public string? RoomStatus { get; set; }
        public int? GuestLimit { get; set; }
        public Guid? RoomTypeId { get; set; }
        public bool? IsFeatured { get; set; }
    }

    public class UpdateRoomRequestModel : BasedRequestModel
    {
        public string? RoomNo { get; set; }
        public string? RoomStatus { get; set; }
        public int? GuestLimit { get; set; }
        public Guid? RoomTypeId { get; set; }
        public bool? IsFeatured { get; set; }
        //public UpdateRoomModel RequestModel { get; set; }
    }

    public class UpdateRoomResponseModel : BasedResponseModel
    {
        public string? RoomNo { get; set; }
        public string? RoomStatus { get; set; }
        public int? GuestLimit { get; set; }
        public Guid? RoomTypeId { get; set; }
        public bool? IsFeatured { get; set; }
        //public UpdateRoomModel ResponseModel { get; set; }
    }
}
