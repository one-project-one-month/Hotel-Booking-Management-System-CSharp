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

    }
}
