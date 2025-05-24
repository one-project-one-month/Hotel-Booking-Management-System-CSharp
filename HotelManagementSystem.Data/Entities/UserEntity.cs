using System.ComponentModel.DataAnnotations.Schema;

namespace HotelManagementSystem.Data.Entities;

[Table(("User"))]
public class UserEntity : BasedEntity
{
    public Guid UserId { get; set; }

    public string UserName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public Guid RoleId { get; set; }

    public byte[]? ProfileImg { get; set; }

    public int? Points { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? RefreshToken { get; set; }

    public DateTime? TokenExpireAt { get; set; }

    public string? ForgetPasswordOtp { get; set; }

    public DateTime? OtpExpireAt { get; set; }

    public string? Password { get; set; }

    public virtual RoleEntity Role { get; set; } = null!;

    public virtual ICollection<BookingEntity> BookingEntities { get; set; } = new List<BookingEntity>();

    public virtual ICollection<GuestEntity> GuestEntities { get; set; } = new List<GuestEntity>();
}