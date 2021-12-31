using Compentio.Example.Ninject.App.Entities;
using Compentio.Example.Ninject.App.Mapper;
using Compentio.Example.Ninject.App.Repository;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Compentio.Example.Ninject.App.Services
{
    [ExcludeFromCodeCoverage]
    public class InvoiceService : IInvoiceService
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly InvoiceMapper _invoiceMapper;

        public InvoiceService(IInvoiceRepository invoiceRepository, InvoiceMapper invoiceMapper)
        {
            _invoiceRepository = invoiceRepository;
            _invoiceMapper = invoiceMapper;
        }

        public async Task<InvoiceDto> CreateInvoice(InvoiceDto invoice)
        {
            var invoiceDao = _invoiceMapper.MapInvoiceToDao(invoice);
            var invoiceResult = await _invoiceRepository.CreateInvoice(invoiceDao);
            return _invoiceMapper.MapInvoiceToDto(invoiceResult);
        }

        public async Task<InvoiceDto> GetInvoice(Guid invoiceId)
        {
            var invoiceResult = await _invoiceRepository.GetInvoice(invoiceId);
            return _invoiceMapper.MapInvoiceToDto(invoiceResult);
        }

        public async Task<IEnumerable<InvoiceDto>> GetInvoices()
        {
            var invoicesResult = await _invoiceRepository.GetInvoices();
            return invoicesResult.Select(invoiceDao => _invoiceMapper.MapInvoiceToDto(invoiceDao));
        }

        public async Task<InvoiceDto> UpdateInvoice(InvoiceDto invoice)
        {
            var invoiceDao = _invoiceMapper.MapInvoiceToDao(invoice);
            var updateResult = await _invoiceRepository.UpdateInvoice(invoiceDao);
            return _invoiceMapper.MapInvoiceToDto(updateResult);
        }
    }
}
