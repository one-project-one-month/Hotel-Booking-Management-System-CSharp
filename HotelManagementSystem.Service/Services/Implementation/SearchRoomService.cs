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

        //public Guid RoomId { get; set; }
        //public string? RoomTypeName { get; set; }
        //public decimal? Price { get; set; }
        //public int? GuestLimit { get; set; }
        //public string? RoomNumber { get; set; }
        //public string? Description { get; set; }
        //public byte[]? ImgUrl { get; set; }

        public async Task<CustomEntityResult<SearchRoomResponseModel>> SearchRoom(SearchRoomRequestModel model)
        {
            try
            {
                var result = await _searchRoomRepo.SearchRoom(new SearchRoomRequestDto
                {
                    RoomTypeId = model.RoomTypeId,
                    GuestLimit = model.GuestLimit,
                    CheckInDate = model.CheckInDate,
                    CheckOutDate = model.CheckOutDate,
                });

                var roomDtos = result.Result.Rooms.Select(r => new RoomModel
                {
                    RoomTypeId = r.RoomTypeId,
                    RoomTypeName = r.RoomTypeName,
                    Price = r.Price,
                    GuestLimit = r.GuestLimit,
                    RoomNumber = r.RoomNumber,
                    Description = r.Description,
                    ImgUrl = r.ImgUrl,
                    ImgMimeType = r.ImgMimeType
                }).ToList();

                var searchRoomResponse = new SearchRoomResponseModel()
                {
                    Rooms = roomDtos
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
