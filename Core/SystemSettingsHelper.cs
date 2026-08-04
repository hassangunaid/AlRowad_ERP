

namespace AlRowad_ERP.Core
{
    // فئات الحقول الرقمية في النظام
    public enum NumericCategory
    {
        AccountingAmount,   // المبالغ المحاسبية (محلي، أجنبي)
        ExchangeRate,       // أسعار الصرف
        InventoryPrice,     // أسعار الأصناف
        InventoryQuantity   // الكميات المخزنية

    }

    public static class SystemSettingsHelper
    {
        // القيم الافتراضية (حتى يتم تحميلها من قاعدة البيانات)
        public static int AccountingDecimals { get; set; } = 2;
        public static int ExchangeRateDecimals { get; set; } = 4;
        public static int PriceDecimals { get; set; } = 2;
        public static int QuantityDecimals { get; set; } = 2;

        // تُستدعى هذه الدالة مرة واحدة فقط عند تسجيل الدخول للنظام (في Program.cs أو MainForm)
        public static void LoadSettings()
        {

            // هنا سيتم جلب الإعدادات من جدول System_Settings في قاعدة البيانات
            // AccountingDecimals = Convert.ToInt32(DatabaseHelper.ExecuteScalar("SELECT Value FROM Settings WHERE Key = 'Acc_Decimals'"));
            // ... وهكذا لبقية الإعدادات
        }
    }
}