using HotelManagementSystem.Data.Dtos.SearchRoom;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using Dapper;

namespace HotelManagementSystem.Service.Repositories.Implementation
{
    public class SearchRoomRepository : ISearchRoomRepository
    {
        private readonly HotelDbContext _context;
        private readonly IConfiguration _configuration;
     

        public SearchRoomRepository(HotelDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }



        public async Task<CustomEntityResult<SearchRoomResponseDto>> SearchRoom(SearchRoomRequestDto model)
        {

            try
            {

                string connectionString = _configuration.GetConnectionString("DefaultConnection")!;
                IDbConnection connection = new SqlConnection(connectionString);

                connection.Open();
                var query = @"SELECT rt.RoomType_Id AS RoomTypeId, r.Room_No AS RoomNumber, r.Room_Id AS RoomId, rt.RoomType_Name AS RoomTypeName, rt.Description, rt.Price, rim.RoomImg AS ImgUrl, rim.RoomImgMimeType AS RoomImgMimeType , r.Guest_Limit AS GuestLimit

                            FROM Tbl_Rooms r 
                            LEFT JOIN Tbl_RoomType rt ON r.RoomType_Id = rt.RoomType_Id
                            LEFT JOIN Tbl_RoomTypeImages rim ON rt.RoomType_Id = rim.RoomType_Id

                            WHERE 
                             (

	                            (r.Guest_Limit >= @GuestLimit)
	                            AND (@RoomTypeId IS NULL OR rt.RoomType_Id = @RoomTypeId)
	                            AND (
                                    @CheckInDate IS NULL OR @CheckOutDate IS NULL
                                     OR
	                            NOT EXISTS
                                (
                                    SELECT 1 
                                    FROM Tbl_Room_Booking rb
                                    INNER JOIN Tbl_Booking b ON rb.Booking_Id = b.Booking_Id
                                    WHERE rb.Room_Id = r.Room_Id
	
                                      AND NOT(
                                                b.CheckOut_Time <= @CheckInDate
                                                OR b.CheckIn_Time >= @CheckOutDate
                                              )
                                 )
                                )
                            )

                         
                            ";

                var roomList = await connection.QueryAsync<RoomQueryResult>(query, model);

                var roomDtos = roomList.Select(r => new RoomSearchDto
                {
                    RoomId = r.RoomId,
                    RoomTypeId = r.RoomTypeId,
                    RoomTypeName = r.RoomTypeName,
                    Price = r.Price,
                    GuestLimit = r.GuestLimit,
                    RoomNumber = r.RoomNumber,
                    Description = r.Description,
                    ImgUrl = $"data:{r.RoomImgMimeType};base64,{Convert.ToBase64String(r.ImgUrl)}",
                    ImgMimeType = r.RoomImgMimeType
                }).ToList();

                var searchRoomResponse = new SearchRoomResponseDto
                {
                    Rooms =roomDtos,
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

