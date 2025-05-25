using HotelManagementSystem.Data.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementSystem.Service.Repositories.Implementation
{
    public class RoomTypeRepository
    {
        private readonly HotelDbContext _hotelDbContext;

        public RoomTypeRepository(HotelDbContext hotelDbContext)
        {
            _hotelDbContext = hotelDbContext;
        }


    }
}
