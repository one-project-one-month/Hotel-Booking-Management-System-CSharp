using HotelManagementSystem.Data.Data;
using HotelManagementSystem.Data.Models.Invoices;
using HotelManagementSystem.Service.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementSystem.Service.Repositories.Implementation;

public class InvoiceRepository : IInvoiceRepository
{
    private readonly HotelDbContext _context;

    public InvoiceRepository(HotelDbContext context)
    {
        _context = context;
    }

    public async Task<Invoice?> GetInvoiceDtoByIdAsync(Guid invoiceId)
    {
        var entity = await _context.TblInvoices
            .Include(i => i.Guest)
            .FirstOrDefaultAsync(i => i.InvoiceId == invoiceId);

        if (entity == null)
            return null;

        // Generate Invoice Code (last 4 chars of GUID) because we don`t have real invoice code logic yet
        string invoiceCode = entity.InvoiceId.ToString("N")[^4..].ToUpper();

        return new Invoice
        {
            InvoiceId = entity.InvoiceId,
            InvoiceCode = invoiceCode,
            GuestName = $"Guest {entity.Guest.Nrc}",
            CheckInDate = entity.CheckInTime,
            CheckOutDate = entity.CheckOutTime,
            Services = new List<HotelServiceItem>
            {
                new HotelServiceItem { Description = "Extra Charges", Price = entity.ExtraCharges ?? 0 },
                new HotelServiceItem { Description = "Deposit", Price = entity.Deposite }
            },
            // add some simple stubbed data for room
            RoomNumber = "204", // Stubbed — add room table/join later if needed
            RoomType = "Deluxe",
            PricePerNight = 150m,
            TaxRatePercent = 7.0m
        };
    }
}

