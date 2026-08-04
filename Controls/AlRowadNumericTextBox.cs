using System;
using System.ComponentModel;
using System.Windows.Forms;
using AlRowad_ERP.Core;

namespace AlRowad_ERP.Controls
{
    public class AlRowadNumericTextBox : TextBox
    {
        // إضافة خاصية تظهر في نافذة الخصائص (Properties) في واجهة التصميم
        [Category("AlRowad Properties")]
        [Description("تحديد فئة الحقل لربطه بعدد الخانات العشرية من إعدادات النظام.")]
        public NumericCategory FormatCategory { get; set; } = NumericCategory.AccountingAmount;

        public AlRowadNumericTextBox()
        {
            // المحاذاة الافتراضية للأرقام تكون لليسار أو الوسط في الأنظمة المحاسبية
            this.TextAlign = HorizontalAlignment.Center;
            this.Text = "0.00";
        }

        // 1. الدستور الأمني: منع إدخال أي شيء غير الأرقام والفاصلة العشرية
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);

            // السماح بالأرقام، وزر المسح (Backspace)، والفاصلة العشرية فقط
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }

            // منع كتابة الفاصلة العشرية أكثر من مرة
            if (e.KeyChar == '.' && (this.Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        // 2. تجربة المستخدم: تحديد الكل عند الدخول للحقل لتسهيل الكتابة فوقه
        protected override void OnEnter(EventArgs e)
        {
            base.OnEnter(e);
            this.SelectAll();
        }

        // 3. الدستور المرئي: تطبيق التنسيق والربط بالإعدادات عند الخروج من الحقل
        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);
            ApplyFormatting();
        }

        public void ApplyFormatting()
        {
            if (string.IsNullOrWhiteSpace(this.Text))
            {
                this.Text = "0";
            }

            // جلب عدد الخانات من إعدادات النظام بناءً على نوع الحقل
            int decimals = 2;
            switch (FormatCategory)
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

            string formatString = "N" + decimals;

            if (decimal.TryParse(this.Text, out decimal parsedValue))
            {
                this.Text = parsedValue.ToString(formatString);
            }
            else
            {
                this.Text = 0m.ToString(formatString);
            }
        }

        // 4. خاصية برمجية مساعدة: لإرجاع القيمة الرقمية جاهزة لدوال الحفظ بدون تحويلات
        [Browsable(false)] // لا تظهر في واجهة التصميم
        public decimal DecimalValue
        {
            get
            {
                if (decimal.TryParse(this.Text, out decimal val)) return val;
                return 0m;
            }
        }
    }
}