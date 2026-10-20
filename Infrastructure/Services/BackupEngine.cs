using System;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AlRowad_ERP.Infrastructure.Services
{
    public static class BackupEngine
    {
        public static async Task<bool> ExecuteBackupAsync(
            string connectionString,
            string destinationFolder,
            int maxFilesToKeep,
            bool enableCloud = false,
            string cloudFolder = "")
        {
            if (!Directory.Exists(destinationFolder))
            {
                Directory.CreateDirectory(destinationFolder);
            }

            var builder = new SqlConnectionStringBuilder(connectionString);
            string databaseName = builder.InitialCatalog;
            string fileName = $"{databaseName}_Backup_{DateTime.Now:yyyyMMdd_HHmmss}.bak";
            string localFullPath = Path.Combine(destinationFolder, fileName);

            // 1. إجراء النسخ الاحتياطي عبر SQL Server مع الضغط
            string sql = $"BACKUP DATABASE [{databaseName}] TO DISK = @Path WITH FORMAT, INIT, COMPRESSION;";

            using (var conn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@Path", localFullPath);
                cmd.CommandTimeout = 300;
                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }

            // 2. تنظيف النسخ القديمة في المسار المحلي
            CleanOldBackups(destinationFolder, $"{databaseName}_Backup_*.bak", maxFilesToKeep);

            // 3. المزامنة مع المجلد السحابي / الشبكي (إن تم تفعيله)
            if (enableCloud && !string.IsNullOrWhiteSpace(cloudFolder) && Directory.Exists(cloudFolder))
            {
                try
                {
                    string cloudFullPath = Path.Combine(cloudFolder, fileName);
                    // نسخ غير متزامن للملف إلى مسار درايف
                    await Task.Run(() => File.Copy(localFullPath, cloudFullPath, true));
                    // تنظيف النسخ القديمة داخل درايف أيضاً للحفاظ على المساحة
                    CleanOldBackups(cloudFolder, $"{databaseName}_Backup_*.bak", maxFilesToKeep);
                }
                catch (Exception ex)
                {
                    // تسجيل الخطأ دون كسر النسخة المحلية الناجحة
                    AlRowad_ERP.Core.DatabaseHelper.LogSystemError(ex.Message, ex.StackTrace, "CloudBackup_Sync");
                }
            }

            return true;
        }

        private static void CleanOldBackups(string folder, string searchPattern, int maxFilesToKeep)
        {
            try
            {
                var dir = new DirectoryInfo(folder);
                var files = dir.GetFiles(searchPattern).OrderByDescending(f => f.CreationTime).ToList();

                if (files.Count > maxFilesToKeep)
                {
                    foreach (var file in files.Skip(maxFilesToKeep))
                    {
                        file.Delete();
                    }
                }
            }
            catch
            {
                // تجاهل أخطاء الصلاحيات في حذف ملف قديم
            }
        }
    }
}