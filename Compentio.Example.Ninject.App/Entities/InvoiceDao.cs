using System;
using System.Collections.Generic;

namespace Compentio.Example.Ninject.App.Entities
{
    public class InvoiceDao
    {
        public Guid Id { get; set; }
        public string Number { get; set; }
        public DateTime InvDate { get; set; }
        public DateTime ShipDate { get; set; }
        public DateTime TaxDate { get; set; }
        public decimal NetValue { get; set; }
        public decimal TaxValue { get; set; }
        public decimal GrossValue { get; set; }
        public string Customer { get; set; }
        public IEnumerable<InvoiceItemDao> Items { get; set; }
    }
}