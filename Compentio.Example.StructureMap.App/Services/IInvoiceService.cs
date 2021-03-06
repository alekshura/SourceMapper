using Compentio.Example.StructureMap.App.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Compentio.Example.StructureMap.App.Services
{
    public interface IInvoiceService
    {
        Task<IEnumerable<InvoiceDto>> GetInvoices();

        Task<InvoiceDto> GetInvoice(Guid invoiceId);

        Task<InvoiceDto> CreateInvoice(InvoiceDto invoice);

        Task<InvoiceDto> UpdateInvoice(InvoiceDto invoice);
    }
}