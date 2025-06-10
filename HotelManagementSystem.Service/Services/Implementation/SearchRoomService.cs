using HotelManagementSystem.Data.Models.SearchRoom;
using HotelManagementSystem.Service.Services.Interface;
using HotelManagementSystem.Data.Dtos.SearchRoom;

namespace HotelManagementSystem.Service.Services.Implementation
{
   
    public class SearchRoomService : ISearchRoomService
    {
        private readonly ISearchRoomRepository _searchRoomRepo;

        public SearchRoomService(ISearchRoomRepository searchRoomRepo)
        {
            _searchRoomRepo = searchRoomRepo;
        }

        public async Task<CustomEntityResult<SearchRoomResponseModel>> SearchRoom(SearchRoomRequestModel model)
        {
            try
            {
                var result = await _searchRoomRepo.SearchRoom(new SearchRoomRequestDto
                {
                    RoomType = model.RoomType,
                    GuestLimit = model.GuestLimit,
                    Price = model.Price
                });

                var searchRoomResponse = new SearchRoomResponseModel()
                {
                    Rooms = result.Result.Rooms!.Select(s => new RoomModel
                    {
                        RoomId = s.RoomId,
                        RoomType = s.RoomType,
                        Price = s.Price,
                        GuestLimit = s.GuestLimit ?? 0,
                        RoomNumber = s.RoomNumber,
                        Description = s.Description,
                        ImgUrl = s.ImgUrl != null ? Convert.ToBase64String(s.ImgUrl) : null
                    }).ToList()
                };

                return CustomEntityResult<SearchRoomResponseModel>.GenerateSuccessEntityResult(searchRoomResponse);
                
            }
            catch (Exception ex)
            {
                return CustomEntityResult<SearchRoomResponseModel>.GenerateFailEntityResult(ResponseMessageConstants.RESPONSE_CODE_SERVERERROR, ex.Message + ex.InnerException);
            }
        }
    }
}
