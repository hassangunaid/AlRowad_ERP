namespace AlRowad_ERP.Models
{
    public class CompanyInfoDto
    {
        public int Company_ID { get; set; }
        public string Company_Name_Ar { get; set; }
        public string Company_Name_En { get; set; }
        public string Tax_Number { get; set; }
        public string Commercial_Record { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public byte[] Logo { get; set; }
        public string Company_Activity_Desc { get; set; }
    }
}