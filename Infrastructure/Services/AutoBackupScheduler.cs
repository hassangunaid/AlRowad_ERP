using AlRowad_ERP.Core;
using AlRowad_ERP.Core.Constants;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace AlRowad_ERP.Infrastructure.Services
{
    public static class AutoBackupScheduler
    {
        private static Timer _timer;
        private static bool _isProcessing = false;

        public static void Start()
        {
            // يبدأ المؤقت بعد دقيقتين من إقلاع النظام، ثم يفحص كل ساعة
            _timer = new Timer(async _ => await CheckAndExecuteBackupAsync(), null, TimeSpan.FromMinutes(2), TimeSpan.FromHours(1));
        }

        public static void Stop()
        {
            _timer?.Change(Timeout.Infinite, 0);
            _timer?.Dispose();
        }

        private static async Task CheckAndExecuteBackupAsync()
        {
            if (_isProcessing) return;
            _isProcessing = true;

            try
            {
                string query = $"SELECT TOP 1 * FROM System_Backup_Settings WHERE {SystemConstants.AuditFields.IsDeleted} = 0";
                DataTable dt = await DatabaseHelper.GetTableAsync(query);

                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    bool isAutoBackupEnabled = Convert.ToBoolean(row["Auto_Backup_Enabled"]);
                    if (!isAutoBackupEnabled) return;

                    string localPath = row["Backup_Path"]?.ToString();
                    int intervalHours = Convert.ToInt32(row["Backup_Interval_Hours"]);
                    int maxFiles = Convert.ToInt32(row["Max_Backup_Files"]);
                    int settingId = Convert.ToInt32(row["Setting_Id"]);

                    // قراءة إعدادات النسخ السحابي / الشبكي (Google Drive / OneDrive / Network)
                    bool isCloudEnabled = row.Table.Columns.Contains("Cloud_Backup_Enabled")
                        && row["Cloud_Backup_Enabled"] != DBNull.Value
                        && Convert.ToBoolean(row["Cloud_Backup_Enabled"]);

                    string cloudPath = row.Table.Columns.Contains("Cloud_Backup_Path")
                        ? row["Cloud_Backup_Path"]?.ToString()
                        : string.Empty;

                    DateTime? lastBackup = row["Last_Backup_Date"] != DBNull.Value
                        ? Convert.ToDateTime(row["Last_Backup_Date"])
                        : (DateTime?)null;

                    // التحقق مما إذا استحق موعد النسخ الدوري
                    if (!lastBackup.HasValue || (DateTime.Now - lastBackup.Value).TotalHours >= intervalHours)
                    {
                        using (var conn = await DatabaseHelper.GetConnectionAsync())
                        {
                            // استدعاء محرك النسخ المزدوج (محلي + سحابي/شبكي) مع إدارة الحذف التلقائي
                            bool success = await BackupEngine.ExecuteBackupAsync(
                                conn.ConnectionString,
                                localPath,
                                maxFiles,
                                isCloudEnabled,
                                cloudPath);

                            if (success)
                            {
                                string updateSql = "UPDATE System_Backup_Settings SET Last_Backup_Date = GETDATE() WHERE Setting_Id = @Id";
                                SqlParameter[] p = { new SqlParameter("@Id", settingId) };
                                await DatabaseHelper.ExecuteNonQueryAsync(updateSql, p);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DatabaseHelper.LogSystemError(ex.Message, ex.StackTrace, "AutoBackup_Background");
            }
            finally
            {
                _isProcessing = false;
            }
        }
    }
}