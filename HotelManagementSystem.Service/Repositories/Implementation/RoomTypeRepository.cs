using HotelManagementSystem.Data;
using HotelManagementSystem.Data.Data;
using HotelManagementSystem.Data.Dtos.Room;
using HotelManagementSystem.Data.Dtos.RoomType;
using HotelManagementSystem.Data.Entities;
using HotelManagementSystem.Data.Models;
using HotelManagementSystem.Service.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementSystem.Service.Repositories.Implementation;

public class RoomTypeRepository : IRoomTypeRepository
{
    private readonly HotelDbContext _hotelDbContext;

    public RoomTypeRepository(HotelDbContext hotelDbContext)
    {
        _hotelDbContext = hotelDbContext;
    }

    public async Task<CustomEntityResult<RoomTypeListResponseDto>> GetRoomTypes()
    {
        try
        {
            var lst = await _hotelDbContext.TblRoomTypes
                .Include(x=>x.TblRoomTypeImage)
                .Select(x=>new RoomTypeDto
            {
                RoomTypeName = x.RoomTypeName,
                Description = x.Description,
                RoomImg = x.TblRoomTypeImage != null ? x.TblRoomTypeImage.RoomImg : null,
                RoomImgMimeType = x.TblRoomTypeImage != null ? x.TblRoomTypeImage.RoomImgMimeType : null,
                Price = x.Price,
            }).ToListAsync();
            var response = new RoomTypeListResponseDto
            {
                RoomTypeList = lst
            };
            return CustomEntityResult<RoomTypeListResponseDto>.GenerateSuccessEntityResult(response);
        } catch (Exception ex)
        {
            return CustomEntityResult<RoomTypeListResponseDto>.GenerateFailEntityResult(ResponseMessageConstants.RESPONSE_CODE_SERVERERROR, ex.Message + ex.InnerException);
        }
    }

    public async Task<CustomEntityResult<RoomTypeResponseDto>> GetRoomTypeById (Guid id)
    {
        try
        {
            var roomType = await _hotelDbContext.TblRoomTypes
                .Include(x=>x.TblRoomTypeImage)
                .FirstOrDefaultAsync(x => x.RoomTypeId == id);
            if (roomType == null) return CustomEntityResult<RoomTypeResponseDto>.GenerateFailEntityResult(ResponseMessageConstants.RESPONSE_CODE_NOTFOUND, "Room Type Not Found");
            var dto = new RoomTypeDto
            {
                RoomTypeName = roomType.RoomTypeName,
                Description = roomType.Description,
                RoomImg = roomType.TblRoomTypeImage != null ? roomType.TblRoomTypeImage.RoomImg : null,
                RoomImgMimeType = roomType.TblRoomTypeImage != null ? roomType.TblRoomTypeImage.RoomImgMimeType : null,
                Price = roomType.Price,
            };
            var response = new RoomTypeResponseDto
            {
                RoomType = dto
            };

            return CustomEntityResult<RoomTypeResponseDto>.GenerateSuccessEntityResult(response);
        } catch (Exception ex)
        {
            return CustomEntityResult<RoomTypeResponseDto>.GenerateFailEntityResult(ResponseMessageConstants.RESPONSE_CODE_SERVERERROR, ex.Message + ex.InnerException);
        }
    }

    public async Task<CustomEntityResult<CreateRoomTypeResponseDto>> CreateRoomType(CreateRoomTypeRequestDto requestDto)
    {
        try
        {
            var roomType = new TblRoomType
            {
                RoomTypeId = Guid.NewGuid(),
                RoomTypeName = requestDto.RoomTypeName,
                Description = requestDto.Description,
                Price = requestDto.Price,
            };

            await _hotelDbContext.TblRoomTypes.AddAsync(roomType);

            if(requestDto.RoomImgMimeType != null || requestDto.RoomImgMimeType != null)
            {
                var roomTypeImg = new TblRoomTypeImage
                {
                    RoomTypeId = roomType.RoomTypeId,
                    RoomImg = requestDto.RoomImg,
                    RoomImgMimeType = requestDto.RoomImgMimeType,
                };

                await _hotelDbContext.TblRoomTypeImages.AddAsync(roomTypeImg);
            }

            int result = await _hotelDbContext.SaveChangesAsync();
            if (result == 0) return CustomEntityResult<CreateRoomTypeResponseDto>.GenerateFailEntityResult(ResponseMessageConstants.RESPONSE_CODE_SERVERERROR, "Create RoomType Failed");

            CreateRoomTypeResponseDto response = new CreateRoomTypeResponseDto()
            {
                RoomTypeId = roomType.RoomTypeId,
            };

            return CustomEntityResult<CreateRoomTypeResponseDto>.GenerateSuccessEntityResult(response);
        }
        catch (Exception ex)
        {
            return CustomEntityResult<CreateRoomTypeResponseDto>.GenerateFailEntityResult(ResponseMessageConstants.RESPONSE_CODE_SERVERERROR, ex.Message + ex.InnerException);
        }
    }

    public async Task<CustomEntityResult<UpdateRoomTypeResponseDto>> UpdateRoomType(Guid id, UpdateRoomTypeRequestDto requestDto)
    {
        try
        {
            var roomType = await _hotelDbContext.TblRoomTypes.FirstOrDefaultAsync(x => x.RoomTypeId == id);

            if (roomType is null) return CustomEntityResult<UpdateRoomTypeResponseDto>.GenerateFailEntityResult(ResponseMessageConstants.RESPONSE_CODE_NOTFOUND, "Room Type Not FOund");

            if (!string.IsNullOrEmpty(requestDto.RoomTypeName)) roomType.RoomTypeName = requestDto.RoomTypeName;

            if (!string.IsNullOrEmpty(requestDto.Description)) roomType.Description = requestDto.Description;

            if (requestDto.Price.HasValue) roomType.Price = requestDto.Price.Value;


            if((requestDto.RoomImg != null && requestDto.RoomImg.Length > 0)|| (requestDto.RoomImgMimeType != null && requestDto.RoomImgMimeType.Length > 0))
            {
                var roomTypeImg = await _hotelDbContext.TblRoomTypeImages.FirstOrDefaultAsync(x => x.RoomTypeId == id);
                if (roomTypeImg is null)
                {
                    var newRoomTypeImg = new TblRoomTypeImage()
                    {
                        RoomTypeId = id,
                        RoomImg = requestDto.RoomImg,
                        RoomImgMimeType = requestDto.RoomImgMimeType,
                    };

                    await _hotelDbContext.TblRoomTypeImages.AddAsync(newRoomTypeImg);
                }
                else
                {
                    roomTypeImg.RoomImg = requestDto.RoomImg;
                    roomTypeImg.RoomImgMimeType = requestDto.RoomImgMimeType;
                }
            }

            int result = await _hotelDbContext.SaveChangesAsync();
            if (result == 0) return CustomEntityResult<UpdateRoomTypeResponseDto>.GenerateFailEntityResult(ResponseMessageConstants.RESPONSE_CODE_SERVERERROR, "Update Failed");

            var responseDto = new UpdateRoomTypeResponseDto()
            {
                RoomTypeName = roomType.RoomTypeName,
                Description = roomType.Description,
                RoomImg = requestDto.RoomImg,
                RoomImgMimeType = requestDto.RoomImgMimeType,
                Price = roomType.Price,
            };
            return CustomEntityResult<UpdateRoomTypeResponseDto>.GenerateSuccessEntityResult(responseDto);
        } catch (Exception ex)
        {
            return CustomEntityResult<UpdateRoomTypeResponseDto>.GenerateFailEntityResult(ResponseMessageConstants.RESPONSE_CODE_SERVERERROR, ex.Message + ex.InnerException);
        }
    }

    public async Task<CustomEntityResult<BasedResponseModel>> DeleteRoomType(Guid id)
    {
        try
        {
            var roomType = await _hotelDbContext.TblRoomTypes
                .Include(x=>x.TblRoomTypeImage)
                .FirstOrDefaultAsync(x => x.RoomTypeId == id);
            if (roomType is null) return CustomEntityResult<BasedResponseModel>.GenerateFailEntityResult(ResponseMessageConstants.RESPONSE_CODE_NOTFOUND, "Room Type not found");

            if(roomType.TblRoomTypeImage != null)
            {
                _hotelDbContext.TblRoomTypeImages.Remove(roomType.TblRoomTypeImage);
            }

            _hotelDbContext.Remove(roomType);
            int result = await _hotelDbContext.SaveChangesAsync();
            if(result == 0) return CustomEntityResult<BasedResponseModel>.GenerateFailEntityResult(ResponseMessageConstants.RESPONSE_CODE_SERVERERROR, "Delete Failed");

            var responseModel = new BasedResponseModel {
                RespCode = "200",
                RespDescription = "Delete Successful"
            };
            return CustomEntityResult<BasedResponseModel>.GenerateSuccessEntityResult(responseModel);
        } catch (Exception ex)
        {
            return CustomEntityResult<BasedResponseModel>.GenerateFailEntityResult(ResponseMessageConstants.RESPONSE_CODE_SERVERERROR, ex.Message + ex.InnerException);
        }
    }
}
