namespace AlRowad_ERP.Core.Models
{
    public class CustomerCurrencyDto
    {
        public int CurrencyId { get; set; }
        public bool IsActive { get; set; }
        public bool IsDefault { get; set; }
        public bool IsFrozen { get; set; }
    }
}