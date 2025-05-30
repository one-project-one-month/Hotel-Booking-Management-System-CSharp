using HotelManagementSystem.Data.Entities;
using HotelManagementSystem.Data.Models.Invoices;

namespace HotelManagementSystem.Service.Services.Interface;

public interface IInvoicePdfService
{
    Task<byte[]> GenerateInvoicePdfAsync(Invoice invoice);
}