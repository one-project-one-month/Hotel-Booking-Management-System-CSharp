using HotelManagementSystem.Data.Dtos.SearchRoom;

namespace HotelManagementSystem.Service.Repositories.Implementation
{
    public class SearchRoomRepository : ISearchRoomRepository
    {
        private readonly HotelDbContext _context;

        public SearchRoomRepository(HotelDbContext context)
        {
            _context = context;
        }

        public async Task<CustomEntityResult<SearchRoomResponseDto>> SearchRoom(SearchRoomRequestDto model)

        {
            try
            {

                //var availableRooms = await _context.TblRooms.FromSqlInterpolated
                //    ($@"
                //        SELECT 
                //        r.Room_No AS RoomId, r.RoomType_Id AS RoomTypeId, r.Room_Status AS RoomStatus, r.Guest_Limit AS GuestLimit, 
                //        r.Is_Featured AS IsFeatured

                //        FROM Tbl_Rooms r

                //        JOIN Tbl_RoomType rt ON r.RoomType_Id AS RoomTypeId = rt.RoomType_Id AS RoomTypeId
                //        WHERE r.Room_Id AS RoomId NOT IN
                //        (
                //            SELECT rb.Room_Id AS RoomId FROM  Tbl_Room_Booking rb
                //            JOIN Tbl_Booking b ON rb.Booking_Id AS BookingId = b.Booking_Id AS BookingId
                //            WHERE b.CheckIn_Time AS CheckInTime < {model.CheckOutDate} 
                //                AND b.CheckOut_Time AS CheckOutTime > {model.CheckInDate}
                //        )
                //        AND ({model.RoomType} IS NULL OR rt.RoomType_Name AS RoomTypeName LIKE '%' + {model.RoomType} + '%')
                //        AND ({model.GuestLimit} IS NULL OR GuestLimit >= {model.GuestLimit})


                //    ").ToListAsync();
                //var availableRooms = await _context.TblRooms.FromSqlInterpolated($@"
                //        SELECT
                //            rt.RoomType_Name AS RoomTypeName,
                //            rt.Price AS Price,
                //            rt.Description AS Description,
                //            rt.Img_Url AS ImgUrl,

                //            r.Room_No AS RoomNo,    

                //            r.Guest_Limit AS GuestLimit,
                //            r.Is_Featured AS IsFeatured
                //        FROM Tbl_Rooms r
                //        JOIN Tbl_RoomType rt ON r.RoomType_Id = rt.RoomType_Id
                //        WHERE r.Room_Id NOT IN
                //            (
                //                SELECT rb.Room_Id
                //                FROM Tbl_Room_Booking rb
                //                JOIN Tbl_Booking b ON rb.Booking_Id = b.Booking_Id
                //                WHERE b.CheckIn_Time < {model.CheckOutDate}
                //                  AND b.CheckOut_Time > {model.CheckInDate}
                //            )
                //        AND ({model.RoomType} IS NULL OR rt.RoomType_Name LIKE '%' + {model.RoomType} + '%')
                //        AND ({model.GuestLimit} IS NULL OR r.Guest_Limit >= {model.GuestLimit})


                //    ").ToListAsync();


                //Console.ReadLine();

                //var availableRooms = await _context.TblRooms.FromSqlInterpolated(@$"SELECT
                //        r.Room_Id AS RoomId,
                //            rt.RoomType_Name AS RoomTypeName,
                //            rt.Price AS Price,
                //            rt.Description AS Description,
                //            rt.Img_Url AS ImgUrl,

                //            r.Room_No AS RoomNo,    

                //            r.Guest_Limit AS GuestLimit,
                //            r.Is_Featured AS IsFeatured
                //        FROM Tbl_Rooms r
                //        JOIN Tbl_RoomType rt ON r.RoomType_Id = rt.RoomType_Id").ToListAsync();

                var availableRooms = await _context.TblRooms
                      .Include(r => r.RoomType)
                      .Where(r =>
                                !_context.TblRoomBookings
                                    .Where( rb => rb.Booking.CheckInTime < model.CheckOutDate &&
                                            rb.Booking.CheckOutTime > model.CheckInDate)
                                    .Select(rb => rb.RoomId)
                                    .Contains(r.RoomId)
                                        &&
                                    (string.IsNullOrEmpty(model.RoomType) || r.RoomType.RoomTypeName.Contains(model.RoomType))
                                        &&
                                    (!model.GuestLimit.HasValue || r.GuestLimit >= model.GuestLimit.Value)
                            )
                    .Select(r => new RoomSearchDto
                    {
                        RoomId = r.RoomId,
                        RoomNumber = r.RoomNo,
                        RoomType = r.RoomType.RoomTypeName,
                        Price = r.RoomType.Price,
                        GuestLimit = r.GuestLimit,
                        Description = r.RoomType.Description
                    })
                    .ToListAsync();


                var searchRoomList = availableRooms.Select(x => new RoomSearchDto()
                {
                    RoomId = x.RoomId,
                    RoomType = x.RoomType,
                    Price = x.Price,
                    GuestLimit = x.GuestLimit ?? 0,
                    RoomNumber = x.RoomNumber,
                    Description = x.Description,
                    ImgUrl = x.ImgUrl 

                }).ToList();

                var searchRoomResponse = new SearchRoomResponseDto
                {
                    Rooms = searchRoomList,
                };
                
                return CustomEntityResult<SearchRoomResponseDto>.GenerateSuccessEntityResult(searchRoomResponse);
            }
            catch (Exception ex)
            {
                return CustomEntityResult<SearchRoomResponseDto>.GenerateFailEntityResult(ResponseMessageConstants.RESPONSE_CODE_SERVERERROR, ex.Message + ex.InnerException);
            }
        }
    }
}

