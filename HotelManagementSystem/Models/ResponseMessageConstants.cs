namespace HotelManagementSystem.Models;

public class ResponseMessageConstants
{
    #region CODE

    public static string RESPONSE_CODE_SUCCESS = "200";
    
    public static string RESPONSE_CODE_NOTFOUND = "404";
    public static string RESPONSE_CODE_BADREQUEST = "400";
    public static string RESPONSE_CODE_DUPLICATE = "409";

    public static string RESPONSE_CODE_SERVERERROR = "500";
    
    public static string RESPONSE_CODE_REQUIRED = "003";
    
    #endregion

    #region MESSAGE

    public static string RESPONSE_MESSAGE_SUCCESS = "Success!";
    
    public static string RESPONSE_MESSAGE_NOTFOUND = "not found!";
    public static string RESPONSE_MESSAGE_BADREQUEST_NOTMATCH = "does not match!";
    public static string RESPONSE_MESSAGE_DUPLICATE = " is duplicate!";
    
    public static string RESPONSE_CODE_MESSAGE = "Server error!";
    
    public static string RESPONSE_MESSAGE_REQUIRED = " is required!";

    #endregion
}