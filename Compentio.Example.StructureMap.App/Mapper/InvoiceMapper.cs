using Compentio.Example.StructureMap.App.Entities;
using Compentio.SourceMapper.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace Compentio.Example.StructureMap.App.Mapper
{
    [Mapper(ClassName = "ClassInvoiceMapper")]
    public abstract partial class InvoiceMapper
    {
        [Mapping(Source = nameof(InvoiceDao.Id), Target = nameof(InvoiceDto.InvoiceId))]
        [Mapping(Source = nameof(InvoiceDao.Number), Target = nameof(InvoiceDto.InvoiceNumber))]
        [Mapping(Source = nameof(InvoiceDao.InvDate), Target = nameof(InvoiceDto.InvoiceDate))]
        [Mapping(Source = nameof(InvoiceDao.NetValue), Target = nameof(InvoiceDto.NetAmount))]
        [Mapping(Source = nameof(InvoiceDao.TaxValue), Target = nameof(InvoiceDto.TaxAmount))]
        [Mapping(Source = nameof(InvoiceDao.GrossValue), Target = nameof(InvoiceDto.GrossAmount))]
        [Mapping(Source = nameof(InvoiceDao.Items), Target = nameof(InvoiceDto.Items), Expression = nameof(ConvertToItemsDto),
            InverseSource = nameof(InvoiceDto.Items), InverseTarget = nameof(InvoiceDao.Items), InverseExpression = nameof(ConvertToItemsDao))]
        [Mapping(CreateInverse = true, InverseMethodName = "MapInvoiceToDao")]
        public abstract InvoiceDto MapInvoiceToDto(InvoiceDao source);

        [Mapping(Source = nameof(InvoiceItemDao.InvNum), Target = nameof(InvoiceItemDto.InvoiceNumber))]
        [Mapping(Source = nameof(InvoiceItemDao.InvLine), Target = nameof(InvoiceItemDto.InvoiceLine))]
        [Mapping(Source = nameof(InvoiceItemDao.NetValue), Target = nameof(InvoiceItemDto.NetAmount))]
        [Mapping(Source = nameof(InvoiceItemDao.TaxValue), Target = nameof(InvoiceItemDto.TaxAmount))]
        [Mapping(Source = nameof(InvoiceItemDao.GrossValue), Target = nameof(InvoiceItemDto.GrossAmount))]
        [Mapping(CreateInverse = true, InverseMethodName = "MapInvoiceItemToDao")]
        public abstract InvoiceItemDto MapInvoiceItemToDto(InvoiceItemDao source);

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