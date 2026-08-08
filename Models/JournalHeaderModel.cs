using System;

namespace AlRowad_ERP.Models
{
    public class JournalHeaderModel
    {
        public int JournalId { get; set; }
        public DateTime JournalDate { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
    }

    public class JournalDetailModel
    {
        public int JournalDetailId { get; set; }
        public int JournalId { get; set; }
        public int AccountId { get; set; }
        public decimal DebitAmount { get; set; }
        public decimal CreditAmount { get; set; }
    }
}