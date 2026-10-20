using System;
using System.ComponentModel;

namespace AlRowad_ERP.Models.Reports
{
    public class AccountStatementDto
    {
        // 1. حقول الشاشة والجدول الأساسية
        public int Source_ID { get; set; }
        public DateTime Trans_Date { get; set; }
        public string Description { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public decimal Running_Balance { get; set; }
        public string Currency_Name { get; set; } // يظهر في الشاشة لمعرفة عملة الحركة
        public string Post_Status { get; set; }

        // 2. حقول العملة والتجميع المحاسبي (مخفية عن الشبكة ومخصصة لمحرك RDLC)
        [Browsable(false)]
        public int Currency_ID { get; set; }

        [Browsable(false)]
        public string Sub_Currency_Name { get; set; } // مثل: فلس / سنت / هللة

        [Browsable(false)]
        public string AccountId { get; set; }

        [Browsable(false)]
        public string AccountName { get; set; }

        // 3. حقول خلاصة كل عملة على حدة (Per-Currency Summary)
        [Browsable(false)]
        public decimal Total_Debit { get; set; }

        [Browsable(false)]
        public decimal Total_Credit { get; set; }

        [Browsable(false)]
        public decimal Final_Balance { get; set; }

        [Browsable(false)]
        public string Balance_Type_Text { get; set; }

        [Browsable(false)]
        public string Amount_In_Words { get; set; }
    }
}