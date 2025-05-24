using HotelManagementSystem.Data;
using HotelManagementSystem.Data.Dtos.SearchRoom;
using HotelManagementSystem.Data.Dtos.User;
using HotelManagementSystem.Service.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementSystem.Service.Repositories.Implementation
{
    public class SearchRoomRepository : ISearchRoomRepository
    {

        public async Task<CustomEntityResult<SearchRoomResponseDto>> CreateUser(SearchRoomRequestDto model)

        {
            try
            {
                var createUserResponse = new SearchRoomResponseDto();
                return CustomEntityResult<SearchRoomResponseDto>.GenerateSuccessEntityResult(createUserResponse);
            }
            catch (Exception ex)
            {
                return CustomEntityResult<SearchRoomResponseDto>.GenerateFailEntityResult(ResponseMessageConstants.RESPONSE_CODE_SERVERERROR, ex.Message + ex.InnerException);
            }
        }
    }
}

