using System.Drawing;

namespace AlRowad_ERP.Core.Helpers
{
    public static class ColorHelper
    {
        public static Color FromHex(string hexCode, Color fallbackColor)
        {
            if (string.IsNullOrWhiteSpace(hexCode)) return fallbackColor;
            try { return ColorTranslator.FromHtml(hexCode); }
            catch { return fallbackColor; }
        }
    }
}