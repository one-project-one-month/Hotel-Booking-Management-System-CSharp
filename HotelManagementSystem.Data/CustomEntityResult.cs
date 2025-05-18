using HotelManagementSystem.Data.Models;
using HotelManagementSystem.Data.Models.User;

namespace HotelManagementSystem.Data;

public class CustomEntityResult<TResult> where TResult : BasedResponseModel, new()
{
    // public string RespCode { get; set; }
    // public string RespDescription { get; set; }
    public TResult Result { get; set; }
    public bool IsError { get; set; }

    public static CustomEntityResult<TResult> GenerateFailEntityResult(string errorCode, string errorDesc)
    {
        CustomEntityResult<TResult> res = new CustomEntityResult<TResult>
        {
            IsError = true,
        };
        res.Result ??= new TResult
        {
            RespCode = errorCode,
            RespDescription = errorDesc
        };
        return res;
    }

    public static CustomEntityResult<TResult> GenerateSuccessEntityResult(TResult entityResult)
    {
        CustomEntityResult<TResult> res = new CustomEntityResult<TResult>
        {
            IsError = false,
            Result = entityResult
        };
        return res;
    }
}