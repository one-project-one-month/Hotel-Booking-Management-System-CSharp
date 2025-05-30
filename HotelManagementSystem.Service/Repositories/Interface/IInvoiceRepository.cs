using HotelManagementSystem.Data.Models.Invoices;

namespace HotelManagementSystem.Service.Repositories.Interface;

public interface IInvoiceRepository
{
    Task<Invoice?> GetInvoiceDtoByIdAsync(Guid invoiceId);
}
