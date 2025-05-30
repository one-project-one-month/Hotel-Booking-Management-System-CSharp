using HotelManagementSystem.Data.Entities;
using HotelManagementSystem.Data.Models.Invoices;
using HotelManagementSystem.Service.Services.Interface;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using static System.Net.Mime.MediaTypeNames;

namespace HotelManagementSystem.Service.Services.Implementation;

public class InvoicePdfService : IInvoicePdfService
{
    //public async Task<byte[]> GenerateInvoicePdfAsync(Invoice invoice)
    //{
    //    return await Task.Run(() =>
    //    {
    //        return Document.Create(container =>
    //        {
    //            container.Page(page =>
    //            {
    //                page.Margin(40);

    //                page.Header().Text($"Hotel Invoice Code #{invoice.InvoiceCode}")
    //                    .FontSize(20).Bold().AlignCenter();

    //                page.Content().Column(col =>
    //                {
    //                    col.Spacing(10);
    //                    col.Item().Image();
    //                    col.Item().Text($"Guest NRC: {invoice.Guest?.Nrc ?? "N/A"}");
    //                    col.Item().Text($"Phone No: {invoice.Guest?.PhoneNo ?? "N/A"}");
    //                    col.Item().Text($"Check-In: {invoice.CheckInTime:dd MMM yyyy HH:mm}");
    //                    col.Item().Text($"Check-Out: {invoice.CheckOutTime:dd MMM yyyy HH:mm}");
    //                    col.Item().Text($"Nights: {(invoice.CheckOutTime - invoice.CheckInTime).Days}");
    //                    col.Item().Text($"Payment Type: {invoice.PaymentType ?? "N/A"}");

    //                    col.Item().LineHorizontal(1).LineColor(Colors.Grey.Medium);

    //                    col.Item().Text($"Deposit: {invoice.Deposite:C}").AlignRight();
    //                    col.Item().Text($"Extra Charges: {(invoice.ExtraCharges ?? 0):C}").AlignRight();

    //                    decimal subtotal = invoice.Deposite + (invoice.ExtraCharges ?? 0);
    //                    col.Item().Text($"Total Amount: {subtotal:C}")
    //                        .FontSize(14).Bold().AlignRight();

    //                    col.Item().LineHorizontal(1).LineColor(Colors.Grey.Medium);

    //                    // Terms and Conditions - bilingual
    //                    col.Item().PaddingTop(20).Text("Terms and Conditions / စည်းကမ်းသတ်မှတ်ချက်များ").FontSize(14).Bold();

    //                    col.Item().Text("• Please present this invoice at check-in.").FontSize(10);
    //                    col.Item().Text("• ကျေးဇူးပြု၍ ဤဘောင်ချာကို တည်းခိုမည့်အချိန် ပြသပေးပါရန်လိုအပ်ပါသည်။").FontSize(10);

    //                    col.Item().Text("• A valid government-issued ID or passport must be shown upon arrival.").FontSize(10);
    //                    col.Item().Text("• အစိုးရထုတ်ပြန်သော မှတ်ပုံတင်မူရင်း (သို့) နိုင်ငံကူးလက်မှတ် တင်ပြရန်လိုအပ်သည်။").FontSize(10);

    //                    col.Item().Text("• Check-in time is 2:00 PM and check-out time is 12:00 PM.").FontSize(10);
    //                    col.Item().Text("• Check-in ဝင်ခွင့်အချိန်မှာ နေ့လည် ၂ နာရီ၊ ထွက်ခွာရမည့်အချိန်မှာ နံနက် ၁၂ နာရီဖြစ်ပါသည်။").FontSize(10);

    //                    col.Item().Text("• Early check-in or late check-out may incur extra charges.").FontSize(10);
    //                    col.Item().Text("• စောစီစွာ check-in ဝင်လိုခြင်း သို့မဟုတ် နောက်ကျစွာ check-out ထွက်ခွာခြင်းအတွက် အပိုကြေးပေးဆောင်ရန် လိုအပ်နိုင်ပါသည်။").FontSize(10);

    //                    col.Item().Text("• All payments made are non-refundable unless stated otherwise.").FontSize(10);
    //                    col.Item().Text("• ငွေပေးချေမှုအားလုံးအတွက် ငွေပြန်အမ်းမပေးမယ်မဟုတ်ပါ။ ကြိုတင်ပြောကြားထားမှသာလျှင် ပြန်အမ်းပေးနိုင်ပါသည်။").FontSize(10);

    //                    col.Item().Text("• The hotel reserves the right to refuse service to anyone violating policies.").FontSize(10);
    //                    col.Item().Text("• ဟိုတယ်အနေဖြင့် စည်းကမ်းချက်များကိုမလိုက်နာသူအား ဝန်ဆောင်မှုပေးခြင်းကို ငြင်းဆိုနိုင်သည်။").FontSize(10);

    //                    col.Item().Text("• For assistance, contact support@hotel.com or call +95 923 456 7890.").FontSize(10);
    //                    col.Item().Text("• အကူအညီတစ်စုံတစ်ရာလိုအပ်ကပါ ဆက်သွယ်ရန် support@hotel.com သို့မဟုတ် +၉၅ ၉၂၃ ၄၅၆ ၇၈၉၀ ကို ဆက်သွယ်ပါ။").FontSize(10);

    //                    col.Item().PaddingTop(20);
    //                });

    //                page.Footer().AlignCenter().Text("Thank you for staying with us!")
    //                    .FontSize(10).Italic();
    //            });
    //        }).GeneratePdf();
    //    });
    //}

    // public async Task<byte[]> GenerateInvoicePdfAsync(Invoice invoice)
    // {
    //     return await Task.Run(() =>
    //     {
    //         return Document.Create(container =>
    //         {
    //             container.Page(page =>
    //             {
    //                 page.Margin(40);

    //                 page.Header().Row(row =>
    //                 {
    //                     row.RelativeItem().Column(col =>
    //                     {
    //                         col.Item().Text("Hotel Voucher").Bold().FontSize(20).FontColor(Colors.Green.Medium);
    //                         col.Item().Text("Voucher Status: PAID").Bold().FontSize(14).FontColor(Colors.Red.Medium);
    //                     });

    //                     // Logo section
    //                     var imagePath = "images/Black_and_Gold_Vintage_Luxury_Hotel_Logo.png";
    //                     if (File.Exists(imagePath))
    //                     {
    //                         using FileStream stream = File.OpenRead(imagePath);
    //                         row.ConstantItem(100).Height(80).Image(stream, ImageScaling.FitArea); 

    //                     }
    //                     else
    //                     {
    //                         row.ConstantItem(100).Height(80).Text("No Logo").FontSize(10).Italic();
    //                     }
    //                 });

    //                 page.Content().Column(content =>
    //                 {
    //                     content.Spacing(10);

    //                     // Booking info section //{invoice.Guest?.FullName}
    //                     content.Item().Background(Colors.Grey.Lighten3).Padding(5).Text(
    //                         $"ITINERARY: Hotel DC & D - Mg Mg Lwin , {invoice.CheckInTime:dd MMM yyyy HH:mm}")
    //                         .Bold().FontSize(12);

    //                     content.Item().Row(row =>
    //                     {
    //                         row.RelativeItem().Column(col =>
    //                         {
    //                             col.Item().Text($"Guest: { "Mg Mg Lwin" ?? "N/A"} (Passport {invoice.Guest?.Nrc ?? "N/A"})");
    //                             col.Item().Text($"Phone No: {invoice.Guest?.PhoneNo ?? "N/A"}");
    //                             col.Item().Text($"Check-In: {invoice.CheckInTime:dd MMM yyyy HH:mm}");
    //                             col.Item().Text($"Check-Out: {invoice.CheckOutTime:dd MMM yyyy HH:mm}");
    //                         });

    //                         row.RelativeItem().Column(col =>
    //                         {
    //                             col.Item().Text($"Nights: {(invoice.CheckOutTime - invoice.CheckInTime).Days}");
    //                             col.Item().Text($"Room Type: {invoice.RoomType ?? "Standard"}");
    //                             col.Item().Text($"Payment Type: {invoice.PaymentType ?? "N/A"}");
    //                             col.Item().Text($"Booking ID: {invoice.InvoiceCode}");
    //                         });
    //                     });

    //                     content.Item().PaddingTop(10).LineHorizontal(1).LineColor(Colors.Grey.Medium);

    //                     // Pricing
    //                     content.Item().Text("PRICING DETAILS").Bold().FontSize(12);
    //                     content.Item().Row(row =>
    //                     {
    //                         row.RelativeItem().Text($"Deposit: {invoice.Deposite:C}");
    //                         row.RelativeItem().Text($"Extra Charges: {(invoice.ExtraCharges ?? 0):C}");
    //                         row.RelativeItem().Text($"Total: {(invoice.Deposite + (invoice.ExtraCharges ?? 0)):C}").Bold();
    //                     });

    //                     // Notes Section (English & Burmese)
    //                     content.Item().PaddingTop(10).Background(Colors.Grey.Lighten3).Padding(5)
    //                         .Text("INFORMATION / သတင်းအချက်အလက်").FontSize(12).Bold();

    //                     content.Item().Text("* Please present this voucher at the reception during check-in.")
    //                         .FontSize(10);
    //                     content.Item().Text("* ကျေးဇူးပြု၍ ဤဘောင်ချာကို တည်းခိုမည့်အချိန် reception တွင်ပြသပေးပါရန်လိုအပ်ပါသည်။").FontSize(10);

    //                     content.Item().Text("* A valid passport or NRC is required.")
    //                         .FontSize(10);
    //                     content.Item().Text("* မှတ်ပုံတင်မူရင်း (သို့) နိုင်ငံကူးလက်မှတ် အားတင်ပြရန် လိုအပ်ပါသည်။")
    //                         .FontSize(10);

    //                     content.Item().Text("* Check-in time: 2:00 PM | Check-out time: 12:00 PM")
    //                         .FontSize(10);
    //                     content.Item().Text("* Check-in ဝင်ခွင့်အချိန်မှာ - နေ့လည် ၂ နာရီ | Check-out ထွက်ခွာရမည့်အချိန်မှာ - နံနက် ၁၂ နာရီ")
    //                         .FontSize(10);

    //                     content.Item().Text("• Early check-in or late check-out may incur extra charges.")
    //                     .FontSize(10);
    //                     content.Item().Text("• စောစီစွာ check-in ဝင်လိုခြင်း သို့မဟုတ် နောက်ကျစွာ check-out ထွက်ခွာခြင်းအတွက် အပိုကြေးပေးဆောင်ရန် လိုအပ်နိုင်ပါသည်။")
    //                     .FontSize(10);

    //                     content.Item().Text("* All payments made are non-refundable for any reasons.")
    //                         .FontSize(10);
    //                     content.Item().Text("* မည်သည့်အကြောင်းပြချက်နဲမှ ငွေပြန်အမ်းပေးမည်မဟုတ်ပါ။")
    //                         .FontSize(10);

    //                     content.Item().Text("* The hotel reserves the right to refuse service to anyone violating policies.")
    //                     .FontSize(10);
    //                     content.Item().Text("* ဟိုတယ်အနေဖြင့် စည်းကမ်းချက်များကိုမလိုက်နာသူအား ဝန်ဆောင်မှုပေးခြင်းကို ငြင်းဆိုနိုင်သည်။")
    //                     .FontSize(10);

    //                     content.Item().Text("* For assistance, contact support@hotel.com or call +95 923 456 7890.")
    //                     .FontSize(10);
    //                     content.Item().Text("* အကူအညီတစ်စုံတစ်ရာလိုအပ်ကပါ ဆက်သွယ်ရန် support@hotel.com သို့မဟုတ် +၉၅ ၉၂၃ ၄၅၆ ၇၈၉၀ ကို ဆက်သွယ်ပါ။")
    //                     .FontSize(10);
    //                 });

    //                 page.Footer().AlignCenter().Text("Thank you for choosing our hotel! / ကျွန်ုပ်တို့ဟိုတယ်ကို ရွေးချယ်ထားခဲ့သည့်အတွက် ကျေးဇူးတင်ပါသည်။")
    //                     .FontSize(10).Italic();
    //             });
    //         }).GeneratePdf();
    //     });
    // }

    public async Task<byte[]> GenerateInvoicePdfAsync(Invoice invoice)
    {
        return await Task.Run(() =>
        {
            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    // Set page properties
                    page.Size(PageSizes.A4);
                    page.Margin(50);
                    page.DefaultTextStyle(x => x.FontSize(10));
                    
                    // HEADER SECTION
                    page.Header().Padding(5).Column(header =>
                    {
                        // Header with logo and hotel info
                        header.Item().Row(row =>
                        {
                            // Hotel name and info - increased relative size
                            row.RelativeItem(3).Column(col =>
                            {
                                col.Item().Text("LUXURY HOTEL & RESORT")
                                    .FontSize(20).Bold().FontColor(Colors.Blue.Darken3);
                                col.Item().Text("Your Home Away From Home")
                                    .FontSize(10).Italic().FontColor(Colors.Grey.Medium);
                                col.Item().PaddingTop(5).Text("123 Peaceful Street, Yangon, Myanmar")
                                    .FontSize(8).FontColor(Colors.Grey.Medium);
                                col.Item().Text("Tel: +95 1 234 5678 | Email: info@luxuryhotel.com")
                                    .FontSize(8).FontColor(Colors.Grey.Medium);
                            });

                            // Logo - reduced size
                            var imagePath = "images/Black_and_Gold_Vintage_Luxury_Hotel_Logo.png";
                            if (File.Exists(imagePath))
                            {
                                using FileStream stream = File.OpenRead(imagePath);
                                // Reduced relative size and added height constraint
                                row.RelativeItem(1).AlignRight().Height(60).Image(stream, ImageScaling.FitArea);
                            }
                            else
                            {
                                row.RelativeItem(1).AlignRight().Text("LOGO")
                                    .FontSize(20).Bold().FontColor(Colors.Grey.Medium);
                            }
                        });

                        // INVOICE title with status
                        header.Item().PaddingTop(10).LineHorizontal(1).LineColor(Colors.Grey.Medium);
                        header.Item().PaddingVertical(10).Background(Colors.Blue.Lighten5).Padding(5).Row(row =>
                        {
                            row.RelativeItem().Text("HOTEL INVOICE")
                                .FontSize(18).Bold().FontColor(Colors.Blue.Darken3);
                            row.RelativeItem().AlignRight().Text("PAID")
                                .FontSize(16).Bold().FontColor(Colors.Green.Darken1);
                        });
                    });

                    // CONTENT SECTION
                    page.Content().PaddingVertical(10).Column(content =>
                    {
                        // Booking reference section
                        content.Item().Background(Colors.Grey.Lighten4).Padding(10).Row(row =>
                        {
                            row.RelativeItem(3).Text($"BOOKING REFERENCE: {invoice.InvoiceCode}")
                                .Bold().FontSize(11);
                            row.RelativeItem(2).AlignRight().Text($"Date: {DateTime.Now:dd MMM yyyy}")
                                .FontSize(11);
                        });

                        // Guest information section
                        content.Item().PaddingTop(15).Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(1);
                                columns.RelativeColumn(1);
                            });

                            // Guest details
                            table.Cell().Text("GUEST INFORMATION").Bold().FontSize(12).FontColor(Colors.Blue.Darken3);
                            table.Cell().Text("STAY DETAILS").Bold().FontSize(12).FontColor(Colors.Blue.Darken3);

                            table.Cell().Column(col =>
                            {
                                col.Item().PaddingTop(5).Text($"Name: {"Mg Mg Lwin" ?? "N/A"}").Bold();
                                col.Item().Text($"ID/Passport: {invoice.Guest?.Nrc ?? "N/A"}");
                                col.Item().Text($"Phone: {invoice.Guest?.PhoneNo ?? "N/A"}");
                                col.Item().Text($"Email: {invoice.Guest?.Email ?? "guest@example.com"}");
                            });

                            table.Cell().Column(col =>
                            {
                                col.Item().PaddingTop(5).Text($"Check-In: {invoice.CheckInTime:dd MMM yyyy, HH:mm}").Bold();
                                col.Item().Text($"Check-Out: {invoice.CheckOutTime:dd MMM yyyy, HH:mm}").Bold();
                                col.Item().Text($"Nights: {(invoice.CheckOutTime - invoice.CheckInTime).Days}");
                                col.Item().Text($"Room Type: {invoice.RoomType ?? "Standard"}");
                                col.Item().Text($"Payment Method: {invoice.PaymentType ?? "N/A"}");
                            });
                        });

                        // Pricing section with box border
                        content.Item().PaddingTop(20).Element(container =>
                        {
                            container.Background(Colors.White)
                                .Border(1)
                                .BorderColor(Colors.Grey.Lighten2)
                                .Padding(1)
                                .Table(table =>
                                {
                                    table.ColumnsDefinition(columns =>
                                    {
                                        columns.RelativeColumn(3);
                                        columns.RelativeColumn(1);
                                    });

                                    // Header
                                    table.Cell().ColumnSpan(2).Background(Colors.Blue.Lighten4)
                                        .Padding(8).Text("PRICING DETAILS").Bold().FontSize(12);

                                    // Deposit
                                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2)
                                        .Padding(8).Text("Deposit");
                                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2)
                                        .Padding(8).AlignRight().Text($"{invoice.Deposite:C}");

                                    // Extra charges
                                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2)
                                        .Padding(8).Text("Extra Charges");
                                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2)
                                        .Padding(8).AlignRight().Text($"{(invoice.ExtraCharges ?? 0):C}");

                                    // Total
                                    table.Cell().Background(Colors.Grey.Lighten3)
                                        .Padding(8).Text("Total Amount").Bold();
                                    table.Cell().Background(Colors.Grey.Lighten3)
                                        .Padding(8).AlignRight().Text($"{(invoice.Deposite + (invoice.ExtraCharges ?? 0)):C}")
                                        .Bold().FontSize(12);
                                });
                        });

                        // Information section
                        content.Item().PaddingTop(20).Element(container =>
                        {
                            container.Border(1)
                                .BorderColor(Colors.Grey.Lighten2)
                                .Column(col =>
                                {
                                    col.Item().Background(Colors.Grey.Lighten4).Padding(10)
                                        .Text("IMPORTANT INFORMATION / အရေးကြီးသတင်းအချက်အလက်")
                                        .Bold().FontSize(10).FontColor(Colors.Blue.Darken3);

                                    col.Item().Padding(8).Row(row =>
                                    {
                                        // English information
                                        row.RelativeItem().Column(col =>
                                        {
                                            col.Item().Text("• Please present this voucher at check-in.").FontSize(9);
                                            col.Item().Text("• Valid ID/passport is required.").FontSize(9);
                                            col.Item().Text("• Check-in: 2:00 PM, Check-out: 12:00 PM.").FontSize(9);
                                            col.Item().Text("• Early check-in/late check-out may incur extra fees.").FontSize(9);
                                            col.Item().Text("• All payments are non-refundable for any reasons.").FontSize(9);
                                        });

                                        // Burmese information
                                        row.RelativeItem().Column(col =>
                                        {
                                            col.Item().Text("• ကျေးဇူးပြု၍ ဤဘောင်ချာကို တည်းခိုမည့်အချိန် ပြသပေးပါရန်။").FontSize(8);
                                            col.Item().Text("• မှတ်ပုံတင်/နိုင်ငံကူးလက်မှတ် လိုအပ်ပါသည်။").FontSize(8);
                                            col.Item().Text("• Check-in: နေ့လည် ၂ နာရီ, Check-out: နံနက် ၁၂ နာရီ။").FontSize(8);
                                            col.Item().Text("• စောစီးစွာ check-in ဝင်ခြင်း (သို့) နောက်ကျ check-out ထွက်ခြင်း အတွက် အပိုအခကြေးငွေ ပေးရမည်။").FontSize(8);
                                            col.Item().Text("• မည့်သည့် အကြောင်းနဲမျှ ငွေပြန်အမ်းမပေးပါ။").FontSize(8   );
                                        });
                                    });

                                    // Contact information
                                    col.Item().BorderTop(1).BorderColor(Colors.Grey.Lighten2).Padding(10).Row(row =>
                                    {
                                        row.RelativeItem().Text("For assistance: +95 923 456 7890 | support@luxuryhotel.com")
                                            .FontSize(9).FontColor(Colors.Grey.Medium);
                                    });
                                });
                        });

                        // Cancellation policy
                        content.Item().PaddingTop(15).Background(Colors.Red.Lighten5).Border(1)
                            .BorderColor(Colors.Red.Medium).Padding(10).Text("Cancellation Policy: Non-refundable")
                            .FontColor(Colors.Red.Darken1).FontSize(10).Bold();
                    });

                    // FOOTER SECTION
                    page.Footer().Column(footer =>
                    {
                        footer.Item().LineHorizontal(1).LineColor(Colors.Grey.Lighten2);
                        footer.Item().PaddingTop(10).Row(row =>
                        {
                            row.RelativeItem().Column(col =>
                            {
                                col.Item().Text("Thank you for choosing our hotel!")
                                    .FontSize(10).Bold().FontColor(Colors.Blue.Darken3);
                                col.Item().Text("ကျွန်ုပ်တို့ဟိုတယ်ကို ရွေးချယ်ထားခဲ့သည့်အတွက် ကျေးဇူးတင်ပါသည်။")
                                    .FontSize(9).FontColor(Colors.Grey.Medium);
                            });

                            row.RelativeItem().AlignRight().Text($"Page 1 of 1")
                                .FontSize(9).FontColor(Colors.Grey.Medium);
                        });
                    });
                });
            }).GeneratePdf();
        });
    }
}
