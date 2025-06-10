using System;
using System.Collections.Generic;
using System.Linq;
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
        {   if(str != null)
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
    }
}
