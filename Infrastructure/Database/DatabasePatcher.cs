using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using AlRowad_ERP.Core;
using AlRowad_ERP.Core.Constants;

namespace AlRowad_ERP.Infrastructure.Database
{
    public static class DatabasePatcher
    {
        public static async Task EnsureSchemaIntegrityAsync()
        {
            using (var conn = await DatabaseHelper.GetConnectionAsync())
            {
                // 1. إنشاء جدول سجل التحديثات
                string checkPatchHistory = @"
                    IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = '__SchemaVersions')
                    BEGIN
                        CREATE TABLE __SchemaVersions (
                            Version_Id INT IDENTITY(1,1) PRIMARY KEY,
                            Patch_Name NVARCHAR(150) NOT NULL UNIQUE,
                            Applied_At DATETIME2 NOT NULL DEFAULT GETDATE()
                        );
                    END";
                using (var cmd = new SqlCommand(checkPatchHistory, conn))
                {
                    await cmd.ExecuteNonQueryAsync();
                }

                // 2. إنشاء جدول إعدادات النسخ الاحتياطي
                await EnsureTableExistsAsync(conn, "System_Backup_Settings", $@"
                    Setting_Id INT IDENTITY(1,1) PRIMARY KEY,
                    Backup_Path NVARCHAR(500) NOT NULL,
                    Auto_Backup_Enabled BIT NOT NULL DEFAULT 1,
                    Backup_Interval_Hours INT NOT NULL DEFAULT 24,
                    Last_Backup_Date DATETIME NULL,
                    Max_Backup_Files INT NOT NULL DEFAULT 7,
                    {SystemConstants.AuditFields.CreatedBy} INT NOT NULL DEFAULT 1,
                    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE()
                ");

                // 3. ضمان الحقول الرقابية الأساسية
                await EnsureAuditColumnsAsync(conn, "System_Backup_Settings");
                await EnsureColumnExistsAsync(conn, "System_Backup_Settings", "Cloud_Backup_Enabled", "BIT NOT NULL DEFAULT 0");
                await EnsureColumnExistsAsync(conn, "System_Backup_Settings", "Cloud_Backup_Path", "NVARCHAR(500) NULL");
            }
        }

        public static async Task EnsureTableExistsAsync(SqlConnection conn, string tableName, string columnsDefinition)
        {
            string sql = $@"
                IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = '{tableName}')
                BEGIN
                    CREATE TABLE {tableName} (
                        {columnsDefinition}
                    );
                END";

            using (var cmd = new SqlCommand(sql, conn))
            {
                await cmd.ExecuteNonQueryAsync();
            }
        }

        public static async Task EnsureColumnExistsAsync(SqlConnection conn, string tableName, string columnName, string columnDefinition)
        {
            string sql = $@"
                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('{tableName}') AND name = '{columnName}')
                BEGIN
                    ALTER TABLE {tableName} ADD {columnName} {columnDefinition};
                END";

            using (var cmd = new SqlCommand(sql, conn))
            {
                await cmd.ExecuteNonQueryAsync();
            }
        }

        private static async Task EnsureAuditColumnsAsync(SqlConnection conn, string tableName)
        {
            await EnsureColumnExistsAsync(conn, tableName, SystemConstants.AuditFields.IsDeleted, "BIT NOT NULL DEFAULT 0");
            await EnsureColumnExistsAsync(conn, tableName, "Modified_At", "DATETIME2 NULL");
            await EnsureColumnExistsAsync(conn, tableName, "Modified_By", "INT NULL");
        }
    }
}