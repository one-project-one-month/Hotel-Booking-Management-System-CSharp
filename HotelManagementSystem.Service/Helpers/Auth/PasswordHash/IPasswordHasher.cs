namespace HotelManagementSystem.Service.Helpers.Auth.PasswordHash
{
    public interface IPasswordHasher
    {
        string HashPassword(string password);
        bool VerifyPassword(string hashedPassword, string providedPassword);
    }
}