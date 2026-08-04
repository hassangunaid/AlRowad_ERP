using System.Collections.Generic;

namespace AlRowad_ERP.Core.Entities
{
    public class ThemeEntity
    {
        public int ThemeID { get; set; }
        public string ThemeName { get; set; }
        public bool IsDarkMode { get; set; }
        public bool IsDefault { get; set; }
        public Dictionary<string, string> Tokens { get; set; } = new Dictionary<string, string>();
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public bool IsPredefined { get; set; }
    }

    public class ThemeTokenEntity
    {
        public int TokenID { get; set; }
        public int ThemeID { get; set; } // الربط مع الـ Master
        public string TokenKey { get; set; }
        public string TokenValue { get; set; }
    }
}