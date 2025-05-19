using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementSystem.Service.Exceptions
{
    public class UserNotFoundException(string email) : Exception($"User with {email} cannot be found");
}
