using System;
using System.Collections.Generic;

namespace Compentio.Example.StructureMap.App.Entities
{
    public class InvoiceDto
    {
        public Guid InvoiceId { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime ShipDate { get; set; }
        public DateTime TaxDate { get; set; }
        public decimal NetAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal GrossAmount { get; set; }
        public string Customer { get; set; }
        public IEnumerable<InvoiceItemDto> Items { get; set; }
    }
}