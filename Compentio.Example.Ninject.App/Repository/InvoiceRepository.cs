using Compentio.Example.Ninject.App.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Compentio.Example.Ninject.App.Repository
{
    [ExcludeFromCodeCoverage]
    public class InvoiceRepository : IInvoiceRepository
    {
        public async Task<InvoiceDao> CreateInvoice(InvoiceDao invoice)
        {
            return await Task.FromResult(_invoiceDao);
        }

        public async Task<InvoiceDao> GetInvoice(Guid invoiceId)
        {
            return await Task.FromResult(_invoiceDao);
        }

        public async Task<IEnumerable<InvoiceDao>> GetInvoices()
        {
            var invoiceList = new List<InvoiceDao>();
            invoiceList.Add(_invoiceDao);

            return await Task.FromResult(invoiceList);
        }

        public async Task<InvoiceDao> UpdateInvoice(InvoiceDao invoice)
        {
            return await Task.FromResult(_invoiceDao);
        }

        private readonly InvoiceDao _invoiceDao = new InvoiceDao()
        {
            Id = Guid.NewGuid(),
            Number = "InvoiceNumber",
            InvDate = DateTime.Now,
            ShipDate = DateTime.MinValue,
            TaxDate = DateTime.MaxValue,
            NetValue = 100.0m,
            TaxValue = 23.0m,
            GrossValue = 123.0m,
            Customer = "CustomerName",
            Items = new List<InvoiceItemDao>
            {
                new InvoiceItemDao()
                {
                    InvNum = "InvoiceNumber",
                    InvLine = 1,
                    Item = "Item",
                    Qty = 1.0m,
                    UnitPrice = 100.0m,
                    TaxRate = 23,
                    NetValue = 100.0m,
                    TaxValue = 23.0m,
                    GrossValue = 123.0m
                }
            }
        };
    }
}