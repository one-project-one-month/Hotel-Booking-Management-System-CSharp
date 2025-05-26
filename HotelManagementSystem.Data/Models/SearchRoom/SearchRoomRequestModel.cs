using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementSystem.Data.Models.SearchRoom
{
    public class SearchRoomRequestModel:BasedRequestModel
    {
        public string ? RoomType { get; set; }

        public decimal ? Price { get; set; }
        public int ?  GuestLimit { get; set; }

        public DateOnly CheckInDate { get; set; }
        public DateOnly CheckOutDate { get; set; }
    }
}
