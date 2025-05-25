using Microsoft.AspNetCore.Http;

namespace HotelManagementSystem.Data.Models.User
{
    public class CreateUserProfileModel
    {
    }
    public class CreateUserProfileRequestModel
    {
        public string UserName { get; set; }
        public IFormFile ProfileImg { get; set; } 
    }
    public class  CreateUserResponseModel : BasedResponseModel
    {
        
    }
}
