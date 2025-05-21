namespace HotelManagementSystem.Service.Helpers.SMTP
{
    public interface ISmtpService
    {
        public Task SentPasswordOTPAsync(string toEmail, string subject, string body);
    }
}
