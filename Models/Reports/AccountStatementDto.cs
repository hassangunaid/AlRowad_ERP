using System;

namespace AlRowad_ERP.Models.Reports
{
    public class AccountStatementDto
    {
        // 🌟 بيانات صاحب الحساب المدمجة في الـ DTO
        public string AccountId { get; set; }
        public string AccountName { get; set; }

        // حقول الحركات المحاسبية الأساسية
        public int Source_ID { get; set; }
        public DateTime Trans_Date { get; set; }
        public string Description { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public string Post_Status { get; set; }
        public decimal Running_Balance { get; set; }
    }
}