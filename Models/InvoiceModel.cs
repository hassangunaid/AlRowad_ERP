using System;

namespace AlRowad_ERP.Models
{
    public class InvoiceModel
    {
        public int InvoiceId { get; set; }
        public DateTime InvoiceDate { get; set; }
        public int CustomerId { get; set; }
        public int StoreId { get; set; }
        public decimal NetTotal { get; set; }
    }

    public class InvoiceDetailModel
    {
        public int InvoiceDetailId { get; set; }
        public int InvoiceId { get; set; }
        public int ItemId { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
    }
}