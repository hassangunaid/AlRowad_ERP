using System;
using System.Windows.Forms;
using AlRowad_ERP.Core;

namespace AlRowad_ERP.Core
{
    /// <summary>
    /// المحرك المركزي لإدارة وتوحيد خصائص واجهات المستخدم في نظام الرواد
    /// </summary>
    public static class UIHelper
    {
        /// <summary>
        /// دالة سيادية لضبط التنسيق العشري لأي عمود داخل الجريد بناءً على إعدادات النظام
        /// </summary>
        /// <param name="grid">شبكة البيانات</param>
        /// <param name="columnName">اسم العمود برمجياً</param>
        /// <param name="category">فئة الحقل (محاسبي، مخزني، صرف)</param>
        public static void FormatGridColumn(DataGridView grid, string columnName, NumericCategory category)
        {
            // حماية من الأخطاء إذا لم يكن العمود موجوداً
            if (grid.Columns[columnName] == null) return;

            int decimals = 2; // القيمة الافتراضية

            // قراءة الخانات من الذاكرة المركزية للإعدادات
            switch (category)
            {
                case NumericCategory.AccountingAmount:
                    decimals = SystemSettingsHelper.AccountingDecimals;
                    break;
                case NumericCategory.ExchangeRate:
                    decimals = SystemSettingsHelper.ExchangeRateDecimals;
                    break;
                case NumericCategory.InventoryPrice:
                    decimals = SystemSettingsHelper.PriceDecimals;
                    break;
                case NumericCategory.InventoryQuantity:
                    decimals = SystemSettingsHelper.QuantityDecimals;
                    break;
            }

            // تطبيق الدستور المرئي على العمود
            grid.Columns[columnName].DefaultCellStyle.Format = "N" + decimals;
            grid.Columns[columnName].ValueType = typeof(decimal); // إجبار الجريد على التعامل مع القيمة كرقم

            // محاذاة الأرقام للوسط أو اليسار (حسب المعيار المحاسبي المفضل)
            grid.Columns[columnName].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        /// <summary>
        /// دالة مساعدة لتنسيق عدة أعمدة من نفس الفئة دفعة واحدة (توفير للأسطر)
        /// </summary>
        public static void FormatGridColumns(DataGridView grid, NumericCategory category, params string[] columnNames)
        {
            foreach (string colName in columnNames)
            {
                FormatGridColumn(grid, colName, category);
            }
        }
    }
}       