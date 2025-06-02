namespace HotelManagementSystem.Service.Repositories.Implementation
{
    public class FeatureRoomRepository: IFeatureRoomRepository
    {
        private readonly HotelDbContext _hotelDbContext;
        public FeatureRoomRepository(HotelDbContext AppDbConnect)
        {
            _hotelDbContext = AppDbConnect;
        }

        public CustomEntityResult<GetFeatureRoomsResponseDto> GetRoomByFeature()
        {
            try
            {
                var lst = _hotelDbContext.TblRooms.Where(x => x.IsFeatured == true).ToList();
                if(lst.Count <1 )
                {
                    return CustomEntityResult<GetFeatureRoomsResponseDto>.GenerateFailEntityResult(ResponseMessageConstants.RESPONSE_CODE_NOTFOUND, "No featured rooms found.");
                }

                var RoomResponse = lst.Select(b => new GetFeatureRoomResponseDto
                {
                    RoomId = b.RoomId,
                    RoomNo = b.RoomNo,
                    RoomTypeId = b.RoomTypeId,
                    RoomStatus = b.RoomStatus,
                    GuestLimit = b.GuestLimit
                }).ToList();

                var getFeatureRoomsResponse = new GetFeatureRoomsResponseDto
                {
                    Rooms = RoomResponse
                };

                return CustomEntityResult<GetFeatureRoomsResponseDto>.GenerateSuccessEntityResult(getFeatureRoomsResponse);
            }
            catch (Exception ex)
            {
                throw new Exception("Fail to get selected room.", ex);
            }
        }
    }
}