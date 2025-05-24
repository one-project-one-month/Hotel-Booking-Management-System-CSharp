using HotelManagementSystem.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementSystem.Data.Dtos.SearchRoom
{
    public class SearchRoomDto
    {
    }
    public class SearchRoomRequestDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }

    public class SearchRoomResponseDto : BasedResponseModel
    {

    }
}
