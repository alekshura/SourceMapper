namespace Compentio.Example.StructureMap.App.Entities
{
    public class InvoiceItemDao
    {
        public string InvNum { get; set; }
        public int InvLine { get; set; }
        public string Item { get; set; }
        public decimal Qty { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal NetValue { get; set; }
        public decimal TaxValue { get; set; }
        public decimal GrossValue { get; set; }
        public int TaxRate { get; set; }
    }
}