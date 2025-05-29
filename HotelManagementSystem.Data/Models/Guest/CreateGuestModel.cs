namespace HotelManagementSystem.Data.Models.Guest
{
    internal class CreateGuestModel
    {
    }
    public class CreateGuestRequestModel
    {
        public Guid? UserId { get; set; }
        public string Nrc { get; set; } = null!;
        public string PhoneNo { get; set; } = null!;
    }
    public class  CreateGuestResponseModel : BasedResponseModel
    {
        
    }
}
