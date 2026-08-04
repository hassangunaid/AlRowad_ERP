using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace AlRowad_ERP.Controls
{
    public class AlRowadDateTextBox : TextBox
    {
        private DateTime _currentValidDate = DateTime.Today;

        public AlRowadDateTextBox()
        {
            this.TextAlign = HorizontalAlignment.Center;
            this.Text = _currentValidDate.ToString("dd/MM/yyyy");
            this.MaxLength = 10; // أقصى طول لتاريخ 10/10/2022
        }

        // 1. الدستور الأمني: السماح بالأرقام والشرطات والفواصل فقط
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);

            // السماح بالأرقام، وزر المسح (Backspace)، والشرطة، والفاصلة المائلة
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '-' && e.KeyChar != '/')
            {
                e.Handled = true; // رفض أي حرف أو رمز آخر
            }
        }

        // 2. تجربة المستخدم: تحديد الكل عند الدخول لسرعة الكتابة
        protected override void OnEnter(EventArgs e)
        {
            base.OnEnter(e);
            this.SelectAll();
        }

        // 3. المحرك الذكي: تحويل النص المدخل إلى تاريخ قياسي عند الخروج
        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);
            FormatDateInput();
        }

        private void FormatDateInput()
        {
            string input = this.Text.Trim();

            if (string.IsNullOrWhiteSpace(input))
            {
                this.Text = _currentValidDate.ToString("dd/MM/yyyy");
                return;
            }

            // معالجة حالة إدخال الأرقام متصلة (مثل: 01012022 أو 1012022)
            if (!input.Contains("/") && !input.Contains("-"))
            {
                if (input.Length == 8) // 01012022
                {
                    input = input.Insert(4, "/").Insert(2, "/"); // يصبح 01/01/2022
                }
                else if (input.Length == 6) // 010122 (تلقائياً للقرن الحالي)
                {
                    input = input.Insert(4, "/").Insert(2, "/"); // يصبح 01/01/22
                }
                else if (input.Length == 7) // 1012022 (تاريخ بدون صفر البداية)
                {
                    input = "0" + input.Insert(3, "/").Insert(1, "/");
                }
            }

            // استبدال الشرطات بفواصل مائلة لتوحيد التحليل
            input = input.Replace('-', '/');

            // قائمة التنسيقات التي يقبلها النظام بمرونة
            string[] formats = {
                "d/M/yyyy", "dd/MM/yyyy",
                "d/M/yy", "dd/MM/yy",
                "d/MM/yyyy", "dd/M/yyyy"
            };

            // محاولة التحليل (Parsing)
            if (DateTime.TryParseExact(input, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
            {
                _currentValidDate = parsedDate; // حفظ التاريخ كقيمة صالحة
                this.Text = parsedDate.ToString("dd/MM/yyyy");
                this.BackColor = SystemColors.Window; // إعادة اللون الطبيعي
            }
            else
            {
                // إذا أدخل المستخدم تاريخاً خرافياً (مثل 35/13/2022)
                MessageBox.Show("صيغة التاريخ غير صحيحة. يرجى إدخال تاريخ صالح.", "تنبيه النظام", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Text = _currentValidDate.ToString("dd/MM/yyyy"); // استرجاع آخر تاريخ صالح
                this.Focus();
            }
        }

        // 4. خاصية برمجية مساعدة: لاستخدامها في أوامر الحفظ (SqlTransaction)
        [Browsable(false)]
        public DateTime DateValue
        {
            get { return _currentValidDate; }
            set
            {
                _currentValidDate = value;
                this.Text = value.ToString("dd/MM/yyyy");
            }
        }
    }
}