using HotelManagementSystem.Data.Data;
using HotelManagementSystem.Data.Entities;
using HotelManagementSystem.Data.Models.Invoices;
using HotelManagementSystem.Service.Repositories.Interface;
using HotelManagementSystem.Service.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementSystem.Service.Repositories.Implementation;

public class InvoiceRepository : IInvoiceRepository
{
    private readonly HotelDbContext _context;
    private readonly IInvoicePdfService _pdfService;

    public InvoiceRepository(HotelDbContext context, IInvoicePdfService pdfService)
    {
        _context = context;
        _pdfService = pdfService;
    }

    public async Task<Invoice?> GetInvoiceDtoByIdAsync(Guid invoiceId)
    {
        var entity = await _context.TblInvoices
            .Include(i => i.Guest)
            .FirstOrDefaultAsync(i => i.InvoiceId == invoiceId);

        if (entity == null)
            return null;

        // Convert entity to DTO
        return new Invoice
        {
            InvoiceId = entity.InvoiceId,
            InvoiceCode = entity.InvoiceId.ToString("N")[^4..].ToUpper(),
            CheckInTime = entity.CheckInTime,
            CheckOutTime = entity.CheckOutTime,
            Deposite = entity.Deposite,
            ExtraCharges = entity.ExtraCharges,
            TotalAmount = entity.TotalAmount,
            PaymentType = entity.PaymentType,
            Guest = new GuestInfo
            {
                Nrc = entity.Guest?.Nrc ?? "N/A",
                PhoneNo = entity.Guest?.PhoneNo ?? "N/A"
            }
        };
    }

    public async Task<byte[]> GenerateInvoicePdfByIdAsync(Guid invoiceId)
    {
        var invoice = await GetInvoiceDtoByIdAsync(invoiceId);

        if (invoice == null)
            throw new Exception("Invoice not found");

        return await _pdfService.GenerateInvoicePdfAsync(invoice);
    }

    public async Task<List<Invoice>> GetAllInvoicesAsync()
    {
        var invoiceEntities = await _context.TblInvoices
            .Include(i => i.Guest)
            .ToListAsync();

        var invoiceList = invoiceEntities.Select(i => new Invoice
        {
            InvoiceId = i.InvoiceId,
            InvoiceCode = i.InvoiceId.ToString("N")[^4..].ToUpper(), // last 4 chars of Guid string
            CheckInTime = i.CheckInTime,
            CheckOutTime = i.CheckOutTime,
            Deposite = i.Deposite,
            ExtraCharges = i.ExtraCharges,
            TotalAmount = i.TotalAmount,
            PaymentType = i.PaymentType,
            Guest = i.Guest == null
                ? null
                : new GuestInfo
                {
                    Nrc = i.Guest.Nrc ?? "N/A",
                    PhoneNo = i.Guest.PhoneNo ?? "N/A"
                }
        }).ToList();

        return invoiceList;
    }

}
