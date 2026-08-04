using AlRowad_ERP;
using AlRowad_ERP.Core.Helpers;
using AlRowad_ERP.Forms; // تأكد من أن LongUesr موجودة هنا
using System;
using System.Windows.Forms;

namespace AlRowad_ERP.Core
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // 1. تشغيل أنظمة الحماية ومعالجة الأخطاء المركزية
            GlobalExceptionHandler.AttachGlobalErrorHandlers();

            // 2. منع ارتداد النوافذ واهتزاز الحجم في شاشات ويندوز الحديثة
            if (Environment.OSVersion.Version.Major >= 6)
            {
                SetProcessDPIAware();
            }

            // 3. بوابة المصادقة (Security Gate)
            using (var loginForm = new LongUesr())
            {
                if (loginForm.ShowDialog() == DialogResult.OK)
                {
                    // 4. بناء الشاشة الرئيسية
                    MainForm frmMain = new MainForm();

                    // 5. تفعيل الرادار العائم الخاص بك (FormMonitor)
                    FormMonitor.راقب_الشاشة(frmMain);

                    // 6. عرض الشاشة الرئيسية بشكل منفصل
                    frmMain.Show();

                    // 7. استعادة السياسة: تشغيل التطبيق في الذاكرة الحرة ليعمل محرك الترصيد الخاص بك بحرية
                    Application.Run();
                }
                else
                {
                    Application.Exit();
                }
            }
        }

        // استدعاء المنهج السيادي من الويندوز لمنع تصادم الـ Scaling
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();
    }
}