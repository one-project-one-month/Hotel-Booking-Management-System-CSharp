using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelManagementSystem.Data.Dtos.User;
using HotelManagementSystem.Data.Models.User;
using HotelManagementSystem.Data;
using HotelManagementSystem.Data.Models.SearchRoom;
using HotelManagementSystem.Service.Services.Interface;

namespace HotelManagementSystem.Service.Services.Implementation
{
    public class SearchRoomService : ISearchRoomService
    {
        public Task<CustomEntityResult<SearchRoomResponseModel>> SearchRoom(SearchRoomRequestModel model)
        {
            try
            {

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
