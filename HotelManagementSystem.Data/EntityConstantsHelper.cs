namespace HotelManagementSystem.Data;

public class EntityConstantsHelper
{
    public static DateTime GetSingaporeLocalTime()
    {
        DateTime utcNow = DateTime.UtcNow;
        // Define the GMT+8 timezone
        TimeZoneInfo gmtPlus8 = TimeZoneInfo.FindSystemTimeZoneById("Asia/Singapore"); // or "Asia/Shanghai"
        // Convert the UTC time to GMT+8
        DateTime gmtPlus8Time = TimeZoneInfo.ConvertTimeFromUtc(utcNow, gmtPlus8);

        return gmtPlus8Time;
    }
    
    public static DateTime GetMyanmarLocalTime()
    {
        DateTime utcNow = DateTime.UtcNow;
        // Define the GMT+8 timezone
        TimeZoneInfo gmtPlus630 = TimeZoneInfo.FindSystemTimeZoneById("Asia/Yangon"); // or "Asia/Shanghai"
        // Convert the UTC time to GMT+8
        DateTime gmtPlus630Time = TimeZoneInfo.ConvertTimeFromUtc(utcNow, gmtPlus630);

        return gmtPlus630Time;
    }
}