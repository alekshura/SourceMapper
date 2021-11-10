using Compentio.Example.StructureMap.App.Entities;
using Compentio.SourceMapper.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace Compentio.Example.StructureMap.App.Mapper
{
    [Mapper(ClassName = "ClassInvoiceMapper")]
    public abstract class InvoiceMapper
    {
        [Mapping(Source = nameof(InvoiceDao.Id), Target = nameof(InvoiceDto.InvoiceId))]
        [Mapping(Source = nameof(InvoiceDao.Number), Target = nameof(InvoiceDto.InvoiceNumber))]
        [Mapping(Source = nameof(InvoiceDao.InvDate), Target = nameof(InvoiceDto.InvoiceDate))]
        [Mapping(Source = nameof(InvoiceDao.NetValue), Target = nameof(InvoiceDto.NetAmount))]
        [Mapping(Source = nameof(InvoiceDao.TaxValue), Target = nameof(InvoiceDto.TaxAmount))]
        [Mapping(Source = nameof(InvoiceDao.GrossValue), Target = nameof(InvoiceDto.GrossAmount))]
        [Mapping(Source = nameof(InvoiceDao.Items), Target = nameof(InvoiceDto.Items), Expression = nameof(ConvertToItemsDto))]
        public abstract InvoiceDto MapInvoiceToDto(InvoiceDao source);

        [Mapping(Source = nameof(InvoiceDto.InvoiceId), Target = nameof(InvoiceDao.Id))]
        [Mapping(Source = nameof(InvoiceDto.InvoiceNumber), Target = nameof(InvoiceDao.Number))]
        [Mapping(Source = nameof(InvoiceDto.InvoiceDate), Target = nameof(InvoiceDao.InvDate))]
        [Mapping(Source = nameof(InvoiceDto.NetAmount), Target = nameof(InvoiceDao.NetValue))]
        [Mapping(Source = nameof(InvoiceDto.TaxAmount), Target = nameof(InvoiceDao.TaxValue))]
        [Mapping(Source = nameof(InvoiceDto.GrossAmount), Target = nameof(InvoiceDao.GrossValue))]
        [Mapping(Source = nameof(InvoiceDto.Items), Target = nameof(InvoiceDao.Items), Expression = nameof(ConvertToItemsDao))]
        public abstract InvoiceDao MapInvoiceToDao(InvoiceDto source);

        [Mapping(Source = nameof(InvoiceItemDao.InvNum), Target = nameof(InvoiceItemDto.InvoiceNumber))]
        [Mapping(Source = nameof(InvoiceItemDao.InvLine), Target = nameof(InvoiceItemDto.InvoiceLine))]
        [Mapping(Source = nameof(InvoiceItemDao.NetValue), Target = nameof(InvoiceItemDto.NetAmount))]
        [Mapping(Source = nameof(InvoiceItemDao.TaxValue), Target = nameof(InvoiceItemDto.TaxAmount))]
        [Mapping(Source = nameof(InvoiceItemDao.GrossValue), Target = nameof(InvoiceItemDto.GrossAmount))]
        public abstract InvoiceItemDto MapInvoiceItemToDto(InvoiceItemDao source);

        [Mapping(Source = nameof(InvoiceItemDto.InvoiceNumber), Target = nameof(InvoiceItemDao.InvNum))]
        [Mapping(Source = nameof(InvoiceItemDto.InvoiceLine), Target = nameof(InvoiceItemDao.InvLine))]
        [Mapping(Source = nameof(InvoiceItemDto.NetAmount), Target = nameof(InvoiceItemDao.NetValue))]
        [Mapping(Source = nameof(InvoiceItemDto.TaxAmount), Target = nameof(InvoiceItemDao.TaxValue))]
        [Mapping(Source = nameof(InvoiceItemDto.GrossAmount), Target = nameof(InvoiceItemDao.GrossValue))]
        public abstract InvoiceItemDao MapInvoiceItemToDao(InvoiceItemDto source);

        protected IEnumerable<InvoiceItemDao> ConvertToItemsDao(IEnumerable<InvoiceItemDto> items)
        {
            return items.Select(i => MapInvoiceItemToDao(i)).AsEnumerable();
        }

        protected IEnumerable<InvoiceItemDto> ConvertToItemsDto(IEnumerable<InvoiceItemDao> items)
        {
            return items.Select(i => MapInvoiceItemToDto(i)).AsEnumerable();
        }
    }
}