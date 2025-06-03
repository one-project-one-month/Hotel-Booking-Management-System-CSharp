using HotelManagementSystem.Data.Models.RoomType;
using HotelManagementSystem.Service.Services.Interface;

namespace HotelManagementSystem.Service.Services.Implementation;

public class RoomTypeService : IRoomTypeService
{
    private readonly IRoomTypeRepository _roomTypeRepository;

    public RoomTypeService(IRoomTypeRepository roomTypeRepository)
    {
        _roomTypeRepository = roomTypeRepository;
    }

    public async Task<CustomEntityResult<RoomTypeListResponseModel>> GetRoomTypes()
    {
        try
        {
            var result = await _roomTypeRepository.GetRoomTypes();

            if (result.IsError) return CustomEntityResult<RoomTypeListResponseModel>.GenerateFailEntityResult(result.Result.RespCode, result.Result.RespDescription);

            var lst = result.Result.RoomTypeList.Select(x => new RoomTypeModel
            {
                RoomTypeId = x.RoomTypeId,
                RoomTypeName = x.RoomTypeName,
                Description = x.Description,
                RoomImg = x.RoomImg != null ? Convert.ToBase64String(x.RoomImg) : null,
                RoomImgMimeType = x.RoomImgMimeType,
                Price = x.Price,
            }).ToList();
            var response = new RoomTypeListResponseModel()
            {
                RoomTypeList = lst
            };
            return CustomEntityResult<RoomTypeListResponseModel>.GenerateSuccessEntityResult(response);
        } catch (Exception ex)
        {
            return CustomEntityResult<RoomTypeListResponseModel>.GenerateFailEntityResult(ResponseMessageConstants.RESPONSE_CODE_SERVERERROR, ex.Message + ex.InnerException);
        }
    }

    public async Task<CustomEntityResult<RoomTypeResponseModel>>GetRoomTypeById(Guid id)
    {
        try
        {
            var result = await _roomTypeRepository.GetRoomTypeById(id);
            if (result.IsError) return CustomEntityResult<RoomTypeResponseModel>.GenerateFailEntityResult(result.Result.RespCode, result.Result.RespDescription);

            var model = new RoomTypeModel
            {
                RoomTypeId = result.Result.RoomType.RoomTypeId,
                RoomTypeName = result.Result.RoomType.RoomTypeName,
                Description = result.Result.RoomType.Description,
                RoomImg = result.Result.RoomType.RoomImg != null ? Convert.ToBase64String(result.Result.RoomType.RoomImg) : null,
                RoomImgMimeType = result.Result.RoomType.RoomImgMimeType,
                Price = result.Result.RoomType.Price,
            };

            var response = new RoomTypeResponseModel()
            {
                RoomType = model,
            };

            return CustomEntityResult<RoomTypeResponseModel>.GenerateSuccessEntityResult(response);
        } catch(Exception ex)
        {
            return CustomEntityResult<RoomTypeResponseModel>.GenerateFailEntityResult(ResponseMessageConstants.RESPONSE_CODE_SERVERERROR, ex.Message + ex.InnerException);
        }
    }

    public async Task<CustomEntityResult<CreateRoomTypeResponseModel>> CreateRoomType(CreateRoomTypeRequestModel requestModel)
    {
        byte[]? imgBytes = null;

        if (!string.IsNullOrWhiteSpace(requestModel.RoomImg))
        {
            try
            {
                imgBytes = Convert.FromBase64String(requestModel.RoomImg);
            }
            catch (FormatException ex)
            {
                return CustomEntityResult<CreateRoomTypeResponseModel>.GenerateFailEntityResult(
                    "400",
                    "Invalid Base64 image data: " + ex.Message
                );
            }
        }

        try
        {
            var dto = new CreateRoomTypeRequestDto()
            {
                RoomTypeName = requestModel.RoomTypeName,
                Description = requestModel.Description,
                RoomImg = imgBytes,
                Price = requestModel.Price!.Value,
                RoomImgMimeType = requestModel.RoomImgMimeType,
            };

            var result = await _roomTypeRepository.CreateRoomType(dto);

            if (result.IsError)
            {
                return CustomEntityResult<CreateRoomTypeResponseModel>.GenerateFailEntityResult(
                    result.Result.RespCode,
                    result.Result.RespDescription
                );
            }

            var response = new CreateRoomTypeResponseModel()
            {
                RoomTypeId = result.Result.RoomTypeId
            };

            return CustomEntityResult<CreateRoomTypeResponseModel>.GenerateSuccessEntityResult(response);
        }
        catch (Exception ex)
        {
            return CustomEntityResult<CreateRoomTypeResponseModel>.GenerateFailEntityResult(
                ResponseMessageConstants.RESPONSE_CODE_SERVERERROR,
                ex.Message + (ex.InnerException != null ? " " + ex.InnerException.Message : "")
            );
        }
    }

    public async Task<CustomEntityResult<UpdateRoomTypeResponseModel>> UpdateRoomType(Guid id, UpdateRoomTypeRequestModel requestModel)
    {
        byte[]? imgBytes = null;
        if (!string.IsNullOrWhiteSpace(requestModel.RoomImg))
        {
            try
            {
                imgBytes = Convert.FromBase64String(requestModel.RoomImg);
            }
            catch (FormatException ex)
            {
                return CustomEntityResult<UpdateRoomTypeResponseModel>.GenerateFailEntityResult(
                    "400",
                    "Invalid Base64 image data: " + ex.Message
                );
            }
        }
        try
        {
            var dto = new UpdateRoomTypeRequestDto
            {
                RoomTypeName = requestModel.RoomTypeName,
                Description = requestModel.Description,
                RoomImg = imgBytes,
                RoomImgMimeType = requestModel.RoomImgMimeType,
                Price = requestModel.Price,
            };
            var result = await _roomTypeRepository.UpdateRoomType(id, dto);
            if (result.IsError) return CustomEntityResult<UpdateRoomTypeResponseModel>.GenerateFailEntityResult(result.Result.RespCode, result.Result.RespDescription);

            var response = new UpdateRoomTypeResponseModel
            {
                RoomTypeName = result.Result.RoomTypeName,
                Description = result.Result.Description,
                RoomImg = result.Result.RoomImg != null ? Convert.ToBase64String(result.Result.RoomImg) : null,
                RoomImgMimeType = result.Result.RoomImgMimeType,
                Price = result.Result.Price,
            };

            return CustomEntityResult<UpdateRoomTypeResponseModel>.GenerateSuccessEntityResult(response);
        } catch (Exception ex)
        {
            return CustomEntityResult<UpdateRoomTypeResponseModel>.GenerateFailEntityResult(ResponseMessageConstants.RESPONSE_CODE_SERVERERROR, ex.Message + ex.InnerException);
        }
    }

    public async Task<CustomEntityResult<BasedResponseModel>>DeleteRoomType (Guid id)
    {
        try
        {
            var result = await _roomTypeRepository.DeleteRoomType(id);
            if (result.IsError) return CustomEntityResult<BasedResponseModel>.GenerateFailEntityResult(result.Result.RespCode, result.Result.RespDescription);

            return CustomEntityResult<BasedResponseModel>.GenerateSuccessEntityResult(result.Result);
        } catch (Exception ex)
        {
            return CustomEntityResult<BasedResponseModel>.GenerateFailEntityResult(ResponseMessageConstants.RESPONSE_CODE_SERVERERROR, ex.Message + ex.InnerException);
        }
    }
}
