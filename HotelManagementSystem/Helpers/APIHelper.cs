namespace HotelManagementSystem.Helpers;

public class APIHelper
{
    public static T GenerateSuccessResponse<T>(T model) where T : BasedResponseModel, new()
    {
        if (model == null)
            model = new T();
        model.RespCode = ResponseMessageConstants.RESPONSE_CODE_SUCCESS;
        model.RespDescription = ResponseMessageConstants.RESPONSE_MESSAGE_SUCCESS;
        return model;
    }

    public static T GenerateFailResponse<T>(T model) where T : BasedResponseModel, new()
    {
        var bmodel = new T
        {
            RespCode = model.RespCode,
            RespDescription = model.RespDescription
        };
        return bmodel;
    }
}