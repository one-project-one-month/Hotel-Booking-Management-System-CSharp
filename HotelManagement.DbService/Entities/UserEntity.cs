using System.ComponentModel.DataAnnotations.Schema;
namespace HotelManagement.DbService.Entities;
[Table("User")]
public class UserEntity : BasedEntity
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string PhoneNo { get; set; }
    public string Passowrd { get; set; }
    public int Role { get; set; }
    public int ProfilePhoto { get; set; }
    public int Points { get; set; }
    public int Coupon { get; set; }
}