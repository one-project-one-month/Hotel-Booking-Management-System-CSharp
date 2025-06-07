using HotelManagementSystem.Data.Models.Invoices;
using HotelManagementSystem.Service.Repositories.Interface;
using HotelManagementSystem.Service.Services.Implementation;

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

    [HttpGet("download/id/{invoiceId}")]
    public async Task<IActionResult> DownloadInvoiceById(Guid invoiceId)
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

    [HttpGet("download/{invoiceCode}")]
    public async Task<IActionResult> DownloadInvoiceByCode(string invoiceCode)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var invoice = await _invoiceRepository.GetInvoiceDtoByCodeAsync(invoiceCode);

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


    [HttpGet("all")]
    public async Task<ActionResult<List<Invoice>>> GetAllInvoicesAsync()
    {
        try
        {
            var invoices = await _invoiceRepository.GetAllInvoicesAsync();
            
            return Ok(invoices);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error retrieving invoices: {ex.Message}");
        }
    }


}
