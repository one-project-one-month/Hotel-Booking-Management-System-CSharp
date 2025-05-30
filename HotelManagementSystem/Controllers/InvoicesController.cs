using HotelManagementSystem.Data.Models.Invoices;
using HotelManagementSystem.Helpers;
using HotelManagementSystem.Service.Repositories.Interface;
using HotelManagementSystem.Service.Services.Implementation;
using HotelManagementSystem.Service.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sprache;

namespace HotelManagementSystem.Controllers;

[AllowAnonymous]
[Route("api/[controller]")]
[ApiController]
public class InvoicesController : ControllerBase
{
    private readonly IInvoicePdfService _pdfService;
    private readonly IInvoiceRepository _invoiceRepository;
    //private readonly IUserService _service;

    public InvoicesController(IInvoicePdfService pdfService, IInvoiceRepository invoiceRepository)
    {
        _pdfService = pdfService;
        _invoiceRepository = invoiceRepository;
        //_service = service;
    }

    //[HttpGet("download/{invoiceId}/test")]
    //public async Task<IActionResult> DownloadInvoiceTest(int invoiceId)
    //{
    //    // Simple test data without DB
    //    var invoice = new Invoice
    //    {
    //        InvoiceId = "123456789012345678901234567890123456",
    //        GuestName = "John Smith",
    //        CheckInDate = new DateTime(2025, 5, 25),
    //        CheckOutDate = new DateTime(2025, 5, 28),
    //        RoomNumber = "204",
    //        RoomType = "Deluxe",
    //        PricePerNight = 150m,
    //        TaxRatePercent = 7.0m,
    //        Services = new List<HotelServiceItem>
    //    {
    //        new HotelServiceItem { Description = "Breakfast Buffet", Price = 30m },
    //        new HotelServiceItem { Description = "Minibar", Price = 20m }
    //    }
    //    };

    //    var pdfBytes = await _pdfService.GenerateInvoicePdfAsync(invoice); // await the async call

    //    return File(pdfBytes, "application/pdf", $"HotelInvoice_{invoiceId}.pdf");
    //}

    [HttpGet("download/{invoiceId}")]
    //[Route("SeedRole")]
    public async Task<IActionResult> DownloadInvoice(Guid invoiceId)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        try
        {
            var invoice = await _invoiceRepository.GetInvoiceDtoByIdAsync(invoiceId);

            if (invoice == null)
                return NotFound();

            var pdfBytes = await _pdfService.GenerateInvoicePdfAsync(invoice);

            //var result = await _service.SeedRole();

            //return !result.IsError ? APIHelper.GenerateSuccessResponse() : APIHelper.GenerateFailResponse(result.Result);

            return File(pdfBytes, "application/pdf", $"HotelInvoice_{invoice.InvoiceCode}.pdf");
        }
        catch (Exception ex)
        {
            var message = ex.Message;
            return BadRequest(500);
        }

    }

}


//[HttpGet("{fileName}")]
//public async Task<IActionResult> DownloadFile(string fileName)
//{
//    // Required to add StoredFileName column in database
//    //var uploadResult = await _context.TblInvoices.FirstOrDefaultAsync(u => u.StoredFileName.Equals(fileName));

//    // For testing purposes, replace with actual database call when needed
//    var uploadResult = "apple.txt"; 

//    var path = Path.Combine(_env.ContentRootPath, "uploads", fileName);

//    var memory = new MemoryStream();
//    using (var stream = new FileStream(path, FileMode.Open))
//    {
//        await stream.CopyToAsync(memory);
//    }
//    memory.Position = 0;

//    // Requied to add ContentType column in database
//    //return File(memory, uploadResult.ContentType, Path.GetFileName(path));

//    // Replace "text/plain" with the actual content type if needed
//    return File(memory, "text/plain", Path.GetFileName(path)); 
//}

// Open when we want to upload files later and it's not working with the current fontend setup

//[HttpPost]
//public async Task<ActionResult<List<UploadResult>>> UploadFile(List<IFormFile> files)
//{
//    List<UploadResult> uploadResults = new List<UploadResult>();

//    foreach (var file in files)
//    {
//        var uploadResult = new UploadResult();
//        string trustedFileNameForFileStorage;
//        var untrustedFileName = file.FileName;
//        uploadResult.FileName = untrustedFileName;
//        //var trustedFileNameForDisplay = WebUtility.HtmlEncode(untrustedFileName);

//        trustedFileNameForFileStorage = Path.GetRandomFileName();
//        var path = Path.Combine(_env.ContentRootPath, "uploads", trustedFileNameForFileStorage);

//        await using FileStream fs = new(path, FileMode.Create);
//        await file.CopyToAsync(fs);

//        uploadResult.StoredFileName = trustedFileNameForFileStorage;
//        uploadResult.ContentType = file.ContentType;
//        uploadResults.Add(uploadResult);

//        _context.Uploads.Add(uploadResult);
//    }

//    await _context.SaveChangesAsync();

//    return Ok(uploadResults);
//}




