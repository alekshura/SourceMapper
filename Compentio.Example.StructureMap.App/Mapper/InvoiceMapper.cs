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
        [Mapping(Source = nameof(InvoiceDao.Items), Target = nameof(InvoiceDto.Items), Expression = nameof(ConvertToItems))]
        [InverseMapping(InverseMethodName = "MapInvoiceToDao")]
        public abstract InvoiceDto MapInvoiceToDto(InvoiceDao source);

        [Mapping(Source = nameof(InvoiceItemDao.InvNum), Target = nameof(InvoiceItemDto.InvoiceNumber))]
        [Mapping(Source = nameof(InvoiceItemDao.InvLine), Target = nameof(InvoiceItemDto.InvoiceLine))]
        [Mapping(Source = nameof(InvoiceItemDao.NetValue), Target = nameof(InvoiceItemDto.NetAmount))]
        [Mapping(Source = nameof(InvoiceItemDao.TaxValue), Target = nameof(InvoiceItemDto.TaxAmount))]
        [Mapping(Source = nameof(InvoiceItemDao.GrossValue), Target = nameof(InvoiceItemDto.GrossAmount))]
        [InverseMapping(InverseMethodName = "MapInvoiceItemToDao")]
        public abstract InvoiceItemDto MapInvoiceItemToDto(InvoiceItemDao source);

        protected internal IEnumerable<InvoiceItemDto> ConvertToItems(IEnumerable<InvoiceItemDao> items)
        {
            return items.Select(i => MapInvoiceItemToDto(i)).AsEnumerable();
        }
    }

    [Mapper(ClassName = "ClassInvoiceDataMapper", UseMapper = nameof(InvoiceMapper))]
    public abstract partial class InvoiceDataMapper
    {
        [Mapping(Source = nameof(InvoiceDataDao.Id), Target = nameof(InvoiceDataDto.InvoiceId))]
        [Mapping(Source = nameof(InvoiceDataDao.Number), Target = nameof(InvoiceDataDto.InvoiceNumber))]
        [Mapping(Source = nameof(InvoiceDataDao.Items), Target = nameof(InvoiceDataDto.ItemsData), Expression = nameof(InvoiceMapper.ConvertToItems))]
        public abstract InvoiceDataDto MapToDto(InvoiceDataDao source);
    }
}