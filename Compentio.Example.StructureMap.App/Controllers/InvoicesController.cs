using Compentio.Example.StructureMap.App.Entities;
using Compentio.Example.StructureMap.App.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Compentio.Example.StructureMap.App.Controllers
{
    [ExcludeFromCodeCoverage]
    [Route("api/[controller]")]
    [ApiController]
    public class InvoicesController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;

        public InvoicesController(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        [HttpGet]
        public IEnumerable<InvoiceDto> Get()
        {
            var invoices = _invoiceService.GetInvoices();

            return invoices.Result;
        }
    }
}