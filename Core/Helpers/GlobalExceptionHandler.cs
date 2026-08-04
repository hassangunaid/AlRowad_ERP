using System;
using System.Threading;
using System.Windows.Forms;

namespace AlRowad_ERP.Core.Helpers
{
    public static class GlobalExceptionHandler
    {
        /// <summary>
        /// الدالة المركزية لربط محرك الأخطاء بالتطبيق
        /// </summary>
        public static void AttachGlobalErrorHandlers()
        {
            // 1. اصطياد أخطاء واجهة المستخدم (UI Thread Exceptions)
            Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

            // 2. اصطياد أخطاء العمليات الخلفية وغير المتزامنة (Async / Non-UI Threads)
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
        }

        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            HandleException(e.Exception, "UI Thread");
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception ex)
            {
                HandleException(ex, "Background Thread");
            }
        }

        /// <summary>
        /// المحرك الأساسي لمعالجة الخطأ
        /// </summary>
        private static void HandleException(Exception ex, string threadType)
        {
            // 1. تسجيل الخطأ بصمت في قاعدة البيانات (Audit & Logging)
            LogErrorToDatabase(ex, threadType);

            // 2. إظهار رسالة هادئة واحترافية للمحاسب تحمي تجربة المستخدم (UX)
            MessageBox.Show(
                "عذراً، حدث انقطاع في الاتصال أو خطأ فني غير متوقع. تم تسجيل المشكلة آلياً وجاري العمل على حلها.\n\n" +
                "بياناتك الموجودة على الشاشة آمنة. يرجى إعادة المحاولة.",
                "نظام الرواد ERP - حماية النظام",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }

        private static void LogErrorToDatabase(Exception ex, string threadType)
        {
            try
            {
                // التزاماً بالدستور: نحاول أولاً إرسال الخطأ إلى DatabaseHelper لحفظه في السيرفر
                DatabaseHelper.LogSystemError(ex.Message, ex.StackTrace, threadType);
            }
            catch (Exception fallbackEx)
            {
                // خطة الطوارئ (الدرع الأخير): 
                // في حال فشل الاتصال بقاعدة البيانات نهائياً، نكتب الخطأ في ملف نصي محلي لكي لا نفقد أثره
                try
                {
                    string logPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SystemLogs");
                    if (!System.IO.Directory.Exists(logPath))
                    {
                        System.IO.Directory.CreateDirectory(logPath);
                    }

                    string filePath = System.IO.Path.Combine(logPath, "FatalErrors.txt");
                    string logMessage = $"[{DateTime.Now}] Thread: {threadType}\n" +
                                        $"Main Error: {ex.Message}\n" +
                                        $"DB Fallback Error: {fallbackEx.Message}\n" +
                                        new string('-', 50) + "\n";

                    System.IO.File.AppendAllText(filePath, logMessage);
                }
                catch
                {
                    // الصمت التام: إذا فشل حتى حفظ الملف النصي، نبتلع الخطأ لمنع انهيار النظام أمام المستخدم
                }
            }
        }
    }
}