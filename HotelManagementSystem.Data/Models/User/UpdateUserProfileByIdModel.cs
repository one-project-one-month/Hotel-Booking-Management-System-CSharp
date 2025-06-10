using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementSystem.Data.Models.User
{
    internal class UpdateUserProfileByIdModel
    {
    }
    public class UpdateUserProfileByIdRequestModel
    {
        public string? UserName { get; set; }
        public string? Address { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string? ProfileImg { get; set; }
    }
    public class UpdateUserProfileByIdResponseModel : BasedResponseModel
    {
    }
}
