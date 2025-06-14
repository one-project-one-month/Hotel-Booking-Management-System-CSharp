using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementSystem.Data.Models.SearchRoom
{
    public class SearchRoomRequestModel:BasedRequestModel
    {
        public Guid? RoomTypeId { get; set; }

        public DateTime? CheckInDate { get; set; }

        public DateTime? CheckOutDate { get; set; }

        public int? GuestLimit { get; set; } = 1;
    }
}
