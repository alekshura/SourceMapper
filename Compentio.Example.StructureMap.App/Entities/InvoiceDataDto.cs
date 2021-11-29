using System;
using System.Collections.Generic;

namespace Compentio.Example.StructureMap.App.Entities
{
    public class InvoiceDataDto
    {
        public Guid InvoiceId { get; set; }
        public string InvoiceNumber { get; set; }
        public IEnumerable<InvoiceItemDto> ItemsData { get; set; }
    }
}