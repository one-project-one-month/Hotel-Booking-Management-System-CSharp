using HotelManagementSystem.Data.Models.Invoices;
using HotelManagementSystem.Service.Services.Interface;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

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

                    page.Header().Text($"Hotel Invoice #{invoice.InvoiceId}")
                        .FontSize(20).Bold().AlignCenter();

                    page.Content().Column(col =>
                    {
                        col.Spacing(10);

                        col.Item().Text($"Guest: {invoice.GuestName}");
                        col.Item().Text($"Room: {invoice.RoomNumber} ({invoice.RoomType})");
                        col.Item().Text($"Check-In: {invoice.CheckInDate:dd MMM yyyy}");
                        col.Item().Text($"Check-Out: {invoice.CheckOutDate:dd MMM yyyy}");
                        col.Item().Text($"Nights: {(invoice.CheckOutDate - invoice.CheckInDate).Days}");

                        decimal roomTotal = invoice.PricePerNight * (invoice.CheckOutDate - invoice.CheckInDate).Days;

                        col.Item().Text($"Room Price per Night: {invoice.PricePerNight:C}");
                        col.Item().Text($"Room Total: {roomTotal:C}");

                        col.Item().LineHorizontal(1).LineColor(Colors.Grey.Medium);

                        col.Item().Text("Additional Services").FontSize(14).Bold();

                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(cols =>
                            {
                                cols.RelativeColumn();
                                cols.ConstantColumn(100);
                            });

                            table.Header(header =>
                            {
                                header.Cell().Text("Description").Bold();
                                header.Cell().Text("Price").AlignRight().Bold();
                            });

                            foreach (var service in invoice.Services)
                            {
                                table.Cell().Text(service.Description);
                                table.Cell().Text($"{service.Price:C}").AlignRight();
                            }
                        });

                        decimal servicesTotal = invoice.Services.Sum(x => x.Price);
                        decimal subtotal = roomTotal + servicesTotal;
                        decimal taxAmount = subtotal * invoice.TaxRatePercent / 100;
                        decimal total = subtotal + taxAmount;

                        col.Item().LineHorizontal(1).LineColor(Colors.Grey.Medium);

                        col.Item().Text($"Subtotal: {subtotal:C}").AlignRight();
                        col.Item().Text($"Tax ({invoice.TaxRatePercent}%): {taxAmount:C}").AlignRight();
                        col.Item().Text($"Total: {total:C}").FontSize(14).Bold().AlignRight();
                    });

                    page.Footer().AlignCenter().Text("Thank you for staying with us!")
                        .FontSize(10).Italic();
                });
            }).GeneratePdf();
        });
    }
}

