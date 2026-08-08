using System.Windows.Forms;

namespace AlRowad_ERP.Core // 🛠️ النطاق الصحيح والمعتمد داخل مجلد الأساسيات
{
    public static class FormMonitor
    {
        public static void راقب_الشاشة(Form frm)
        {
            if (frm == null) return;

            // منع تكرار الاشتراك في الحدث لتجنب تسرب الذاكرة
            frm.FormClosed -= Frm_FormClosed;
            frm.FormClosed += Frm_FormClosed;
        }

        private static void Frm_FormClosed(object sender, FormClosedEventArgs e)
        {
            // إذا أغلقت آخر شاشة في الذاكرة يموت البرنامج فوراً وينتهي تعليق الفيجوال ستوديو
            if (Application.OpenForms.Count == 0)
            {
                // استخدام ExitThread لإنهاء حلقة Application.Run() العائمة بأمان تام
                Application.ExitThread();
            }
        }
    }
}