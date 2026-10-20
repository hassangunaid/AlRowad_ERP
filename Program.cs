using System;
using System.Windows.Forms;
using AlRowad_ERP.Core;
using AlRowad_ERP.Core.Constants; // الدستور: استدعاء الثوابت المركزية لمنع النصوص الثابتة
using AlRowad_ERP.Core.Helpers;
using AlRowad_ERP.Forms;
using AlRowad_ERP.Infrastructure.Database;
using AlRowad_ERP.Infrastructure.Services;

namespace AlRowad_ERP.Core
{
    static class Program
    {
        /// <summary>
        /// نقطة الانطلاق الرئيسية لتطبيق الرواد ERP
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // =========================================================
            // 1. تفعيل الحارس الأخير (الاصطياد الشامل للأخطاء)
            // =========================================================
            // إجبار التطبيق على توجيه كافة الأخطاء إلى المعالج الخاص بنا لمنع رسائل الويندوز الافتراضية
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

            // اصطياد أخطاء واجهة المستخدم
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);

            // اصطياد الأخطاء العميقة وأخطاء المهام في الخلفية
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            // =========================================================
            // 2. منع ارتداد النوافذ واهتزاز الحجم في شاشات ويندوز الحديثة
            // =========================================================
            if (Environment.OSVersion.Version.Major >= 6)
            {
                SetProcessDPIAware();
            }

            // =========================================================
            // 3. فحص ومطابقة سلامة قاعدة البيانات تلقائياً (Schema Integrity Check)
            // =========================================================
            try
            {
                // فحص وإنشاء الجداول والحقول الرقابية النواقص بصمت قبل إقلاع المنظومة
                DatabasePatcher.EnsureSchemaIntegrityAsync().GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                DatabaseHelper.LogSystemError(ex.Message, ex.StackTrace, "Schema_Patcher_Startup");
                MessageBox.Show(
                    $"تعذر إتمام عملية الفحص التلقائي لسلامة قاعدة البيانات:\n{ex.Message}",
                    SystemConstants.Messages.SystemProtectionTitle,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            // =========================================================
            // 4. بوابة المصادقة (Security Gate)
            // =========================================================
            using (var loginForm = new LongUesr())
            {
                if (loginForm.ShowDialog() == DialogResult.OK)
                {
                    // =========================================================
                    // 5. إطلاق مجدول النسخ الاحتياطي التلقائي في الخلفية
                    // =========================================================
                    AutoBackupScheduler.Start();

                    // 6. بناء الشاشة الرئيسية وإدارتها داخل بيئة آمنة
                    MainForm frmMain = new MainForm();
                    try
                    {
                        // 7. تفعيل الرادار العائم (FormMonitor)
                        FormMonitor.راقب_الشاشة(frmMain);

                        // 8. عرض الشاشة الرئيسية بشكل منفصل
                        frmMain.Show();

                        // 9. تشغيل التطبيق في الذاكرة الحرة ليعمل محرك الترصيد بحرية
                        Application.Run();
                    }
                    finally
                    {
                        // هندسة الذاكرة: إيقاف مجدول الخلفية وتفريغ الموارد بنظافة تامة
                        AutoBackupScheduler.Stop();

                        if (frmMain != null && !frmMain.IsDisposed)
                        {
                            frmMain.Dispose();
                        }
                    }
                }
                else
                {
                    Application.Exit();
                }
            }
        }

        #region محرك الاصطياد والتسجيل المركزي (الحارس الأخير)

        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            HandleSystemException(e.Exception, "UI_Thread");
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = e.ExceptionObject as Exception;
            HandleSystemException(ex, "Non_UI_Thread");
        }

        static void HandleSystemException(Exception ex, string threadType)
        {
            if (ex == null) return;

            try
            {
                // 1. تسجيل الخطأ بصمت في قاعدة البيانات لتتم مراجعته من قبل المطورين
                DatabaseHelper.LogSystemError(ex.Message, ex.StackTrace, threadType);
            }
            catch
            {
                // إذا فشل الاتصال بقاعدة البيانات أثناء تسجيل الخطأ، نتجاهله كي لا يتسبب في خطأ مزدوج
            }

            // 2. تطبيق الدستور: منع النصوص الثابتة (Magic Strings)
            MessageBox.Show(
                SystemConstants.Messages.UnexpectedSystemError,
                SystemConstants.Messages.SystemProtectionTitle,
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }

        #endregion

        // استدعاء المنهج السيادي من الويندوز لمنع تصادم الـ Scaling
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();
    }
}