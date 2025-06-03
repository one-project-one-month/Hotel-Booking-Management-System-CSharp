using HotelManagementSystem.Data;
using HotelManagementSystem.Data.Models;

namespace HotelManagementSystem.Helpers;

public class APIHelper
{
    public static BasedResponseModel GenerateSuccessResponse(BasedResponseModel model)
    {
        if (model == null)
            model = new BasedResponseModel();
        model.RespCode = ResponseMessageConstants.RESPONSE_CODE_SUCCESS;
        model.RespDescription = ResponseMessageConstants.RESPONSE_MESSAGE_SUCCESS;

        return model;
    }

    public static BasedResponseModel GenerateFailResponse(BasedResponseModel model)
    {
        BasedResponseModel bmodel = new BasedResponseModel()
        {
            RespCode = model.RespCode,
            RespDescription = model.RespDescription
        };

        return bmodel;
    }
}