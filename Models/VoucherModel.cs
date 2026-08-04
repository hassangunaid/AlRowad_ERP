using System;
using System.Collections.Generic;

namespace AlRowad_ERP.Models
{
    public class CashVoucherHeader
    {
        public byte[] RowVersion { get; set; }
        public int VoucherID { get; set; }
        public string VoucherNo { get; set; }
        public DateTime VoucherDate { get; set; }
        public int DocTypeID { get; set; }
        public string BoxAccID { get; set; }
        public decimal Amount { get; set; }
        public decimal AmountForeign { get; set; }
        public int CurID { get; set; }
        public decimal ExchangeRate { get; set; }
        public string Notes { get; set; }
        public int IsPosted { get; set; }

        // 🚀 حقول الرقابة والتدقيق (Audit Trail) الدستورية
        public int CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public DateTime? CreatedAt { get; set; }

        public int UpdatedBy { get; set; }
        public string UpdatedByName { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public List<CashVoucherDetail> Details { get; set; } = new List<CashVoucherDetail>();
    }

    public class CashVoucherDetail
    {
        public string AccID { get; set; }
        public string AccName { get; set; }
        public decimal AmountCredit { get; set; }
        public decimal AmountDebit { get; set; }
        public int CurrencyID { get; set; }
        public decimal ExchangeRate { get; set; }
        public decimal AmountForeign { get; set; }
        public string Notes { get; set; }
    }
}