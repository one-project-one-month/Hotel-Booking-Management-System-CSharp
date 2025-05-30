using HotelManagementSystem.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementSystem.Data.Dtos.Room
{
    public class UpdateRoomDto
    {
        public string? RoomNo { get; set; }
        public string? RoomStatus { get; set; }
        public int? GuestLimit { get; set; }
        public Guid? RoomTypeId { get; set; }
        public bool? IsFeatured { get; set; }
    }

    public class UpdateRoomRequestDto : BasedRequestModel
    {
        public string? RoomNo { get; set; }
        public string? RoomStatus { get; set; }
        public int? GuestLimit { get; set; }
        public Guid? RoomTypeId { get; set; }
        public bool? IsFeatured { get; set; }
        //public UpdateRoomDto RequestDto { get; set; }
    }

    public class UpdateRoomResponseDto : BasedResponseModel
    {
        public string? RoomNo { get; set; }
        public string? RoomStatus { get; set; }
        public int? GuestLimit { get; set; }
        public Guid? RoomTypeId { get; set; }
        public bool? IsFeatured { get; set; }
        //public UpdateRoomDto ResponseDto { get; set; }
    }
}
