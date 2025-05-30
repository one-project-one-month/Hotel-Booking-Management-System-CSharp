using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementSystem.Service.Exceptions
{
    public class InvalidUserIdException(string message): Exception(message);
}
