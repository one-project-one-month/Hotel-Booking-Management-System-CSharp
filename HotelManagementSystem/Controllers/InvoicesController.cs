using HotelManagementSystem.Data.Models.Invoices;
using HotelManagementSystem.Helpers;
using HotelManagementSystem.Service.Repositories.Interface;
using HotelManagementSystem.Service.Services.Implementation;
using HotelManagementSystem.Service.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementSystem.Controllers;

[AllowAnonymous]
[Route("api/[controller]")]
[ApiController]
public class InvoicesController : ControllerBase
{
    private readonly IInvoicePdfService _pdfService;
    private readonly IInvoiceRepository _invoiceRepository;

    public InvoicesController(IInvoicePdfService pdfService, IInvoiceRepository invoiceRepository)
    {
        _pdfService = pdfService;
        _invoiceRepository = invoiceRepository;
    }

    [HttpGet("download/{invoiceId}")]
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
                return NotFound("Invoice not found");

            var pdfBytes = await _pdfService.GenerateInvoicePdfAsync(invoice);

            return File(pdfBytes, "application/pdf", $"HotelInvoice_{invoice.InvoiceCode}.pdf");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}
