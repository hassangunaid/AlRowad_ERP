using AlRowad_ERP.Core;
using AlRowad_ERP.Core.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows.Forms;
using AlRowad_ERP.UI.Base;
using AlRowad_ERP.Core.Constants;

namespace AlRowad_ERP.Data

{
    public static class ThemeRepository 
    {
        public static async Task<ThemeEntity> GetDefaultThemeAsync()
        {
            // تطبيق قاعدة سلامة البيانات والفصل المعماري: التعامل حصراً عبر DatabaseHelper
            using (var connection = DatabaseHelper.GetConnection())
            {
                await connection.OpenAsync();

                // استعلام جلب الثيم الافتراضي النشط
                string query = "SELECT TOP 1 * FROM Sys_Themes WHERE Is_Default = 1 AND Is_Active = 1";

                using (var command = new SqlCommand(query, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new ThemeEntity
                            {
                                ThemeID = Convert.ToInt32(reader["ThemeID"]),
                                ThemeName = reader["ThemeName"].ToString(),
                                // قم بتعبئة باقي خصائص الثيم هنا حسب حقول قاعدة البيانات
                            };
                        }
                    }
                }
            }
            return null; // في حال عدم وجود ثيم افتراضي
        }

        public static async Task<bool> SoftDeleteAsync(int themeId, int userId, SqlTransaction transaction)
        {
            // بناء استعلام نظيف خالٍ من الـ Magic Strings
            string query = $@"
            UPDATE [Themes] 
            SET {SystemConstants.AuditFields.IsDeleted} = 1, 
                {SystemConstants.AuditFields.DeletedBy} = @UserId, 
                {SystemConstants.AuditFields.DeletedAt} = GETDATE() 
            WHERE Theme_ID = @ID";

            SqlParameter[] parameters = {
            new SqlParameter("@ID", themeId),
            new SqlParameter("@UserId", userId)
        };

            // تنفيذ الحذف ضمن سياق الـ ACID Transaction الممرر من الواجهة
            int rowsAffected = await DatabaseHelper.ExecuteNonQueryAsync(query, parameters, transaction);

            return rowsAffected > 0;
        }

        // 1. دالة جلب الثيمات الجاهزة
        public static async Task<List<ThemeEntity>> GetPredefinedThemesAsync()
        {
            var list = new List<ThemeEntity>();
            // هنا يتم استدعاء DatabaseHelper لجلب البيانات (IsPredefined = 1)
            // نضع قائمة فارغة مؤقتاً لاكتمال البناء (Compile)
            return await Task.FromResult(list);
        }

        // 2. دالة الحفظ ككتلة واحدة (ACID Transaction - Master/Details)
        public static async Task<bool> SaveThemeWithTokensAsync(ThemeEntity master, List<ThemeTokenEntity> details, FormMode mode)
        {
            // بناءً على الدستور، يجب استخدام Transaction لضمان سلامة البيانات
            /*
             * مثال هيكلي لكيفية عملها مع DatabaseHelper:
             * 
             * using (var connection = DatabaseHelper.GetConnection())
             * {
             *     await connection.OpenAsync();
             *     using (var transaction = connection.BeginTransaction())
             *     {
             *         try
             *         {
             *             // 1. حفظ جدول Sys_Themes (Master) وأخذ الـ Scope_Identity()
             *             // 2. ربط الـ ID الجديد بكافة عناصر قائمة details
             *             // 3. حفظ جدول Sys_Theme_Tokens (Details)
             *             
             *             transaction.Commit();
             *             return true;
             *         }
             *         catch
             *         {
             *             transaction.Rollback();
             *             throw;
             *         }
             *     }
             * }
             */
            return await Task.FromResult(true); // قيمة مؤقتة حتى نربطها فعلياً بـ DatabaseHelper
        }
    }
}
