using HotelManagementSystem.Data.Entities;
using HotelManagementSystem.Data.Models.Invoices;
using HotelManagementSystem.Service.Services.Interface;
using QuestPDF.Fluent;
using QuestPDF.Helpers;

namespace HotelManagementSystem.Service.Services.Implementation;

public class InvoicePdfService : IInvoicePdfService
{
    public async Task<byte[]> GenerateInvoicePdfAsync(Invoice invoice)
    {
        return await Task.Run(() =>
        {
            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(40);

                    page.Header().Text($"Hotel Invoice Code #{invoice.InvoiceCode}")
                        .FontSize(20).Bold().AlignCenter();

                    page.Content().Column(col =>
                    {
                        col.Spacing(10);
                        col.Item().Text($"Guest NRC: {invoice.Guest?.Nrc ?? "N/A"}");
                        col.Item().Text($"Phone No: {invoice.Guest?.PhoneNo ?? "N/A"}");
                        col.Item().Text($"Check-In: {invoice.CheckInTime:dd MMM yyyy HH:mm}");
                        col.Item().Text($"Check-Out: {invoice.CheckOutTime:dd MMM yyyy HH:mm}");
                        col.Item().Text($"Nights: {(invoice.CheckOutTime - invoice.CheckInTime).Days}");
                        col.Item().Text($"Payment Type: {invoice.PaymentType ?? "N/A"}");

                        col.Item().LineHorizontal(1).LineColor(Colors.Grey.Medium);

                        col.Item().Text($"Deposit: {invoice.Deposite:C}").AlignRight();
                        col.Item().Text($"Extra Charges: {(invoice.ExtraCharges ?? 0):C}")
                            .AlignRight();

                        decimal subtotal = invoice.Deposite + (invoice.ExtraCharges ?? 0);
                        //col.Item().Text($"Subtotal: {subtotal:C}").AlignRight();

                        col.Item().Text($"Total Amount: {subtotal:C}")
                            .FontSize(14).Bold().AlignRight();

                        col.Item().LineHorizontal(1).LineColor(Colors.Grey.Medium);
                    });

                    page.Footer().AlignCenter().Text("Thank you for staying with us!")
                        .FontSize(10).Italic();
                });
            }).GeneratePdf();
        });
    }

}

