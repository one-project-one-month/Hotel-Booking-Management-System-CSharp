using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementSystem.Data.Models.User
{
    public class GetUserProfileByIdModel
    {

    }
    public class GetUserProfileByIdRequestModel
    {
    }
    public class GetUserProfileByIdResponseModel : BasedResponseModel
    {
        public string? UserName { get; set; } 
        public string? Address { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public byte[]? ProfileImg { get; set; }
        public string? ProfileImgMimeType { get; set; }
    }
}
