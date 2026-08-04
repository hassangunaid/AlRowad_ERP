using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace AlRowad_ERP.Core
{
    // كلاس ثابت، مستقل، ومركزي بالكامل لإدارة العملات والمصارفة
    public static class CurrencyHelper
    {
        /// <summary>
        /// جلب سعر الصرف الفوري والأحدث لعملة معينة مقارنة بالعملة المحلية للنظام
        /// </summary>
        public static decimal GetExchangeRate(string currencyCode)
        {
            try
            {
                if (string.IsNullOrEmpty(currencyCode)) return 1.00m;

                string query = "SELECT TOP 1 سعر_الصرف FROM CurrenciesExchange WHERE كود_العملة = @كود ORDER BY تاريخ_التحديث DESC";
                SqlParameter[] parameters = { new SqlParameter("@كود", currencyCode) };

                // استخدام ExecuteQuery المتاحة في DatabaseHelper لديك لضمان التوافق التام
                DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters);

                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0]["سعر_الصرف"] != DBNull.Value)
                {
                    if (decimal.TryParse(dt.Rows[0]["سعر_الصرف"].ToString(), out decimal result))
                    {
                        return result <= 0 ? 1.00m : result;
                    }
                }
                return 1.00m;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"خطأ في جلب سعر المصارفة للعملة {currencyCode}: {ex.Message}", "خطأ في النظام", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 1.00m;
            }
        }

        /// <summary>
        /// احتساب المعادل بالعملة المحلية بناءً على المبلغ الأجنبي وسعر المصارفة
        /// </summary>
        public static decimal ConvertToLocal(decimal foreignAmount, decimal exchangeRate)
        {
            if (exchangeRate <= 0) exchangeRate = 1.00m;
            return foreignAmount * exchangeRate;
        }

        /// <summary>
        /// تعبئة أي أداة ComboBox بالعملات المتاحة في النظام لتوحيد الجلب من الشاشات
        /// </summary>
        public static void FillCurrencyComboBox(ComboBox cmb)
        {
            try
            {
                string query = "SELECT كود_العملة, اسم_العملة FROM Currencies ORDER BY اسم_العملة ASC";
                DataTable dt = DatabaseHelper.ExecuteQuery(query);

                cmb.DataSource = dt;
                cmb.DisplayMember = "اسم_العملة";
                cmb.ValueMember = "كود_العملة";
                cmb.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"خطأ أثناء تعبئة قائمة العملات: {ex.Message}", "خطأ للنظام", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}