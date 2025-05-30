using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementSystem.Data.Models.Invoices;

public class Invoice
{
    public string InvoiceCode { get; set; } = default!;
    public Guid InvoiceId { get; set; }
    public string GuestName { get; set; } = default!;
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
    public string RoomNumber { get; set; } = "N/A"; // Stubbed: You can connect room entity if needed
    public string RoomType { get; set; } = "Standard"; // Stubbed
    public decimal PricePerNight { get; set; } = 100m; // Stubbed or calculate from other data
    public decimal TaxRatePercent { get; set; } = 7.0m;

    public List<HotelServiceItem> Services { get; set; } = new();
}


public class HotelServiceItem
{
    public string Description { get; set; }
    public decimal Price { get; set; }
}

