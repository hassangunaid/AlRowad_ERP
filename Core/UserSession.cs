using System;

namespace AlRowad_ERP.Core
{
    public static class UserSession
    {
        // بيانات المستخدم الأساسية
        public static int UserId { get; set; } = 1; // الافتراضي 1 (مدير النظام) للبيئة التطويرية
        public static string Username { get; set; } = "admin";
        public static string FullName { get; set; } = "مدير النظام العام";
        public static bool IsSuperAdmin { get; set; } = true;
        public static DateTime LoginTime { get; set; } = DateTime.Now;

        // دالة مركزية للتحقق من الصلاحيات لاحقاً
        public static bool HasPermission(string permissionCode)
        {
            // المستخدم رقم 1 (أو SuperAdmin) يكسر أي قاعدة قفل أو صلاحية
            if (IsSuperAdmin || UserId == 1)
                return true;

            // هنا سيتم استدعاء منطق الصلاحيات العادية لباقي المستخدمين لاحقاً
            return false;
        }

        // تفريغ الجلسة عند تسجيل الخروج
        public static void ClearSession()
        {
            UserId = 0;
            Username = string.Empty;
            FullName = string.Empty;
            IsSuperAdmin = false;
        }
    }
}