using Compentio.Example.Ninject.App.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Compentio.Example.Ninject.App.Controllers
{
    [ExcludeFromCodeCoverage]
    public class HomeController : Controller
    {
        private readonly IInvoiceService _invoiceService;

        public HomeController(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        public IActionResult Index()
        {
            var invoiceTask = _invoiceService.GetInvoices();
            var invoice = invoiceTask.Result.First();

            return View(invoice);
        }
    }
}