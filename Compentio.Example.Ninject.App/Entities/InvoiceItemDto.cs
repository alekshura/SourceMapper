namespace Compentio.Example.Ninject.App.Entities
{
    public class InvoiceItemDto
    {
        public string InvoiceNumber { get; set; }
        public int InvoiceLine { get; set; }
        public string Item { get; set; }
        public decimal Qty { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal NetAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal GrossAmount { get; set; }
        public int TaxRate { get; set; }
    }
}