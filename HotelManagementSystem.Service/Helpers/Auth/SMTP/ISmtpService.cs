using HotelManagementSystem.Data.Models.User;
using HotelManagementSystem.Data;

namespace HotelManagementSystem.Service.Helpers.Auth.SMTP
{
    public interface ISmtpService
    {
        public Task<CustomEntityResult<ForgotPasswordResponseModel>> SentPasswordOTPAsync(string toEmail, string subject, string body);
    }
}
