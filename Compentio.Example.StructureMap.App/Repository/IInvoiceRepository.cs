using Compentio.Example.StructureMap.App.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Compentio.Example.StructureMap.App.Repository
{
    public interface IInvoiceRepository
    {
        Task<IEnumerable<InvoiceDao>> GetInvoices();

        Task<InvoiceDao> GetInvoice(Guid invoiceId);

        Task<InvoiceDao> CreateInvoice(InvoiceDao invoice);

        Task<InvoiceDao> UpdateInvoice(InvoiceDao invoice);
    }
}