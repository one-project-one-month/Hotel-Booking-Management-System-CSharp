using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementSystem.Service
{
    public static class DevCode
    {

        /// you can use this method to check if a string is null or empty, including whitespace 
        /// You can directly use it in your code like this:
        /// for example: model.RomType.isNullOrEmptyCustom()

        public static bool isNullOrEmptyCustom(this string str)
        {
            if (string.IsNullOrEmpty(str) || string.IsNullOrEmpty(str.Trim()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static string ToByteArray(this string str)
        {
            if (str != null)
            {
                var dataParts = str.Split(',');
                var mimeTypePart = dataParts[0];
                var base64Data = dataParts[1];
                var mimeType = mimeTypePart.Split(':')[1].Split(';')[0];
                byte[] imageBytes;
                imageBytes = Convert.FromBase64String(base64Data);
                return mimeType;
            }
            return null;
        }

        public static bool IsRoomAvailable(this string roomStatus)
        {
            if (roomStatus == "Available")
            {
                return true;
            }
            else if (roomStatus == "Occupied" || roomStatus == "Maintenance")
            {
                return false;
            }
            else
            {
                throw new ArgumentException("Invalid room status");
            }
        }

        public static decimal CalculateEarlyCheckInFee(this StayLog stayLog)
        {
            TimeSpan actualTime = stayLog.ActualCheckIn.TimeOfDay;
            if (actualTime < HotelPolicy.CheckInTime)
            {
                if (actualTime < new TimeSpan(6, 0, 0)) // before 6:00 AM
                    return stayLog.RoomRatePerNight;
                else if (actualTime < new TimeSpan(10, 0, 0)) // 6 AM - 10 AM
                    return stayLog.RoomRatePerNight / 2;
            }
            return 0;
        }

        public static decimal CalculateLateCheckOutFee(this StayLog stayLog)
        {
            TimeSpan actualTime = stayLog.ActualCheckOut.TimeOfDay;
            if (actualTime > HotelPolicy.CheckOutTime)
            {
                if (actualTime <= HotelPolicy.LateCheckoutHalfChargeLimit)
                    return stayLog.RoomRatePerNight / 2;
                else
                    return stayLog.RoomRatePerNight;
            }
            return 0;
        }
    }

    public static class HotelPolicy
    {
        public static readonly TimeSpan CheckInTime = new TimeSpan(14, 0, 0);   // 2:00 PM
        public static readonly TimeSpan CheckOutTime = new TimeSpan(12, 0, 0);  // 12:00 PM

        //Define penalty windows
        public static readonly TimeSpan LateCheckoutHalfChargeLimit = new TimeSpan(18, 0, 0); // 6:00 PM
    }

    public class StayLog
    {
        public DateTime ActualCheckIn { get; set; }
        public DateTime ActualCheckOut { get; set; }
        public decimal RoomRatePerNight { get; set; }
    }
}
