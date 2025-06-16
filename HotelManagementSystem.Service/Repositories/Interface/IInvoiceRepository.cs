using HotelManagementSystem.Data.Models.Invoices;

namespace HotelManagementSystem.Service.Repositories.Interface;

public interface IInvoiceRepository
{
    Task<Invoice?> GetInvoiceDtoByIdAsync(Guid invoiceId);
    Task<byte[]> GenerateInvoicePdfByIdAsync(Guid invoiceId);
    Task<Invoice?> GetInvoiceDtoByCodeAsync(string invoiceCode);
    Task<byte[]> GenerateInvoicePdfByCodeAsync(string invoiceCode);

    Task<List<Invoice>> GetAllInvoicesAsync();
}
