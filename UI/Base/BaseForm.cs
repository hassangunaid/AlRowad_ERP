using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel; // مطلوب لـ LicenseManager
using AlRowad_ERP.Core;

namespace AlRowad_ERP.Core
{
    public class BaseForm : Form
    {
        protected Label lblShortcutBar;
        public BaseForm()
        {
            
            this.Resize += BaseForm_Resize;
            // إعدادات واجهة المستخدم الافتراضية للرواد (RTL والخط القياسي)
            this.RightToLeft = RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.Font = new Font("Cairo", 10F);

            // ==================================================
            // دستور الرواد المعماري: سلوك مجلدات وتطبيقات ويندوز
            // ==================================================

            // 1. جعل النافذة تفتح في موقع شلالي متزحزح تلقائياً لمنع تطابق وتراكم النوافذ
            this.StartPosition = FormStartPosition.WindowsDefaultLocation;

            // 2. إظهار أيقونة مستقلة لكل نافذة في شريط مهام الويندوز (Taskbar) للتنقل الحر بـ Alt+Tab
            this.ShowInTaskbar = true;
        }

        // هذا الحدث يعمل تلقائياً في كل مرة يقوم فيها المستخدم بتكبير، تصغير، أو سحب الشاشة
        private void BaseForm_Resize(object sender, EventArgs e)
        {
            AdjustBarScale();
        }

        // الدالة السحرية لحساب أبعاد الشريط وحجم الخط ديناميكياً
        private void AdjustBarScale()
        {
            // التأكد من أن أداة الشريط موجودة ولها أبعاد حقيقية لمنع أخطاء الحسابات
            if (lblShortcutBar == null || this.Width <= 0 || this.Height <= 0) return;

            // =========================================================
            // 1. ضبط الارتفاع (الطول الرأسي) للشريط نسبياً
            // =========================================================
            int calculatedHeight = (int)(this.Height * 0.05);
            lblShortcutBar.Height = Math.Max(38, Math.Min(calculatedHeight, 55));

            // =========================================================
            // 2. الحساب الذكي لحجم الخط ليتناسب مع عرض الشاشة تماماً
            // =========================================================
            string textToMeasure = "الاضافة (F6)   |   التعديل (F5)   |   التراجع (F4)   |   الحفظ (F10)   |   الحذف (Ctrl+D)   |   البحث (F9)   |   قائمة العملاء (F7)   |   قائمة الموردين (F8)";
            lblShortcutBar.Text = textToMeasure;

            // نبدأ بأكبر حجم خط مسموح به ومريح للعين
            float startingFontSize = 14f;
            Font testFont = new Font("Segoe UI", startingFontSize, FontStyle.Bold);

            // نترك مسافة أمان (Margin) على أطراف الشاشة لكي لا يلتصق النص بالحواف
            int availableWidth = this.Width - 40;

            // استخدام كائن الجرافيكس لقياس الطول الفعلي للنص بالبكسل
            using (Graphics g = lblShortcutBar.CreateGraphics())
            {
                // قياس طول النص الحالي بناءً على الخط الافتراضي
                SizeF textSize = g.MeasureString(textToMeasure, testFont);

                // حلقة ذكية: طالما أن طول النص بالبكسل أكبر من عرض الشاشة، قم بتصغير الخط بمقدار 0.5
                while (textSize.Width > availableWidth && startingFontSize > 8f)
                {
                    startingFontSize -= 0.5f; // تصغير الخط تدريجياً
                    testFont.Dispose(); // التخلص من الخط القديم لتوفير الذاكرة
                    testFont = new Font("Segoe UI", startingFontSize, FontStyle.Bold);

                    // إعادة قياس النص بالحجم الجديد
                    textSize = g.MeasureString(textToMeasure, testFont);
                }
            }

            // تطبيق الخط النهائي المحسوب بدقة والذي يضمن احتواء النص بنسبة 100%
            lblShortcutBar.Font = testFont;
        }


        // 3. فرض التركيز: إجبار النافذة الحرة لتكون دائماً في المقدمة فور تفعيلها
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            // 💡 حظر الكود أثناء التصميم لحماية المصمم المرئي
            if (this.DesignMode || LicenseManager.UsageMode == LicenseUsageMode.Designtime) return;

            this.Activate();       // تفعيل الشاشة
            this.BringToFront();   // جلبها للأمام فوق الشاشة الرئيسية
        }

        protected void ClearFormFields(Control container = null)
        {
            if (container == null) container = this;

            foreach (Control ctrl in container.Controls)
            {
                // 1. تفريغ صناديق النصوص
                if (ctrl is TextBox txt)
                {
                    txt.Clear();
                }
                // 2. إلغاء تحديد صناديق الاختيار
                else if (ctrl is CheckBox chk)
                {
                    chk.Checked = false;
                }
                // 3. إعادة القوائم المنسدلة للوضع الافتراضي الفارغ
                else if (ctrl is ComboBox cmb)
                {
                    if (cmb.Items.Count > 0) cmb.SelectedIndex = -1;
                    else cmb.Text = string.Empty;
                }
                // 4. تصفير الخيارات الدائرية
                else if (ctrl is RadioButton rdo)
                {
                    rdo.Checked = false;
                }
                // 5. الغوص داخل الحاويات والألواح (GroupBox, Panel, TableLayoutPanel) لتطهير ما بداخلها
                else if (ctrl.HasChildren)
                {
                    ClearFormFields(ctrl);
                }
            }
        }

        // معالج الإغلاق الآمن لتحرير موارد قاعدة البيانات
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // 💡 حظر استدعاء قاعدة البيانات أثناء التصميم لمنع انهيار الـ Designer
            if (this.DesignMode || LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            {
                base.OnFormClosing(e);
                return;
            }

            try
            {
                }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"خطأ عند إغلاق الاتصال: {ex.Message}");
            }
            base.OnFormClosing(e);
        }
    }
}