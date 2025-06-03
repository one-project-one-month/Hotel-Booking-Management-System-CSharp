using System.ComponentModel.DataAnnotations.Schema;
namespace HotelManagement.DbService.Entities;
public class BasedEntity
{
    [Column("Id")]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [Column("CreatedUserId")]
    public string CreatedUserId { get; set; } = "1";

    [Column("CreatedDateTime")]
    public DateTime CreatedDateTime { get; set; } = DateTime.UtcNow;

    [Column("UpdatedUserId")]
    public string UpdatedUserId { get; set; } = "1";

    [Column("UpdatedDateTime")]
    public DateTime UpdatedDateTime { get; set; } = DateTime.UtcNow;
}