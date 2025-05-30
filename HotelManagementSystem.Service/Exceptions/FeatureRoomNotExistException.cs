using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementSystem.Service.Exceptions
{
    public class FeatureRoomNotExistException(string message) : Exception(message);
}
