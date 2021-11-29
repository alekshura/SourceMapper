using System;
using System.Collections.Generic;

namespace Compentio.Example.StructureMap.App.Entities
{
    public class InvoiceDataDao
    {
        public Guid Id { get; set; }
        public string Number { get; set; }
        public IEnumerable<InvoiceItemDao> Items { get; set; }
    }
}