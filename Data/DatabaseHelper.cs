using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;

namespace AlRowad_ERP.Core
{
    public static class DatabaseHelper
    {
        #region إعدادات الاتصال
        private static string GetConnectionString()
        {
            // قراءة سلسلة الاتصال من ملف الإعدادات (App.config / machine.config)
            // الاسم الموحد المستخدم في المشروع
            string connectionStringName = "AlRowad_ERP.Properties.Settings.AlRowad_ERPConnectionString";

            // حاول قراءة من مقاطع connectionStrings أولاً
            string cs = ConfigurationManager.ConnectionStrings[connectionStringName]?.ConnectionString;

            // إذا لم تتوفر هناك، حاول قراءة من AppSettings بعنوان أبسط لدعم بيئات التطوير
            if (string.IsNullOrWhiteSpace(cs))
            {
                cs = ConfigurationManager.AppSettings["AlRowad_ERPConnectionString"];
            }

            // كحل مؤقت فقط (fallback) — لا تستخدم في الإنتاج، سيُرمى استثناء إذا لم تُعرّف السلسلة
            if (string.IsNullOrWhiteSpace(cs))
            {
                throw new InvalidOperationException($"سلسلة الاتصال للمفتاح '{connectionStringName}' غير معرّفة في App.config أو AppSettings.");
            }

            return cs;
        }

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(GetConnectionString());
        }
        #endregion

        #region العمليات اللامتزامنة (Async Operations) - الدستور المتقدم

        public static async Task<int> ExecuteNonQueryAsync(string query, SqlParameter[] parameters = null, SqlTransaction transaction = null)
        {
            using (SqlCommand cmd = new SqlCommand(query))
            {
                if (parameters != null) cmd.Parameters.AddRange(parameters);
                cmd.CommandTimeout = 60;

                if (transaction != null)
                {
                    cmd.Connection = transaction.Connection;
                    cmd.Transaction = transaction;
                    return await cmd.ExecuteNonQueryAsync();
                }
                else
                {
                    using (SqlConnection conn = GetConnection())
                    {
                        cmd.Connection = conn;
                        await conn.OpenAsync();
                        return await cmd.ExecuteNonQueryAsync();
                    }
                }
            }
        }

        public static async Task<object> ExecuteScalarAsync(string query, SqlParameter[] parameters = null, SqlTransaction transaction = null)
        {
            using (SqlCommand cmd = new SqlCommand(query))
            {
                if (parameters != null) cmd.Parameters.AddRange(parameters);
                cmd.CommandTimeout = 60;

                if (transaction != null)
                {
                    cmd.Connection = transaction.Connection;
                    cmd.Transaction = transaction;
                    return await cmd.ExecuteScalarAsync();
                }
                else
                {
                    using (SqlConnection conn = GetConnection())
                    {
                        cmd.Connection = conn;
                        await conn.OpenAsync();
                        return await cmd.ExecuteScalarAsync();
                    }
                }
            }
        }

        public static async Task<DataTable> GetTableAsync(string query, SqlParameter[] parameters = null, SqlTransaction transaction = null)
        {
            DataTable dt = new DataTable();
            SqlConnection conn = transaction != null ? transaction.Connection : GetConnection();

            try
            {
                if (conn.State != ConnectionState.Open) await conn.OpenAsync();

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (transaction != null) cmd.Transaction = transaction;
                    if (parameters != null) cmd.Parameters.AddRange(parameters);
                    cmd.CommandTimeout = 60;

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        dt.Load(reader);
                    }
                }
            }
            finally
            {
                if (transaction == null && conn != null && conn.State == ConnectionState.Open)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
            return dt;
        }

        #endregion

        #region العمليات المتزامنة (القديمة للتوافقية) والتدقيق

        public static int ExecuteNonQuery(string query, SqlParameter[] parameters = null, SqlTransaction transaction = null)
        {
            using (SqlCommand cmd = new SqlCommand(query))
            {
                if (parameters != null) cmd.Parameters.AddRange(parameters);
                cmd.CommandTimeout = 60;

                if (transaction != null)
                {
                    cmd.Connection = transaction.Connection;
                    cmd.Transaction = transaction;
                    return cmd.ExecuteNonQuery();
                }
                else
                {
                    using (SqlConnection conn = GetConnection())
                    {
                        cmd.Connection = conn;
                        conn.Open();
                        return cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        public static object ExecuteScalar(string query, SqlParameter[] parameters = null, SqlTransaction transaction = null)
        {
            using (SqlCommand cmd = new SqlCommand(query))
            {
                if (parameters != null) cmd.Parameters.AddRange(parameters);
                cmd.CommandTimeout = 60;

                if (transaction != null)
                {
                    cmd.Connection = transaction.Connection;
                    cmd.Transaction = transaction;
                    return cmd.ExecuteScalar();
                }
                else
                {
                    using (SqlConnection conn = GetConnection())
                    {
                        cmd.Connection = conn;
                        conn.Open();
                        return cmd.ExecuteScalar();
                    }
                }
            }
        }

        public static DataTable GetTable(string query, SqlParameter[] parameters = null, SqlTransaction transaction = null)
        {
            DataTable dt = new DataTable();
            SqlConnection conn = transaction != null ? transaction.Connection : GetConnection();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (transaction != null) cmd.Transaction = transaction;
                    if (parameters != null) cmd.Parameters.AddRange(parameters);
                    cmd.CommandTimeout = 60;
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }
            finally
            {
                if (transaction == null && conn != null && conn.State == ConnectionState.Open)
                {
                    conn.Close(); conn.Dispose();
                }
            }
            return dt;
        }

        internal static DataTable ExecuteQuery(string query) => GetTable(query);
        internal static DataTable ExecuteQuery(string query, SqlParameter[] parameters) => GetTable(query, parameters);

        public static void LogAuditTransaction(SqlTransaction transaction, string tableName, string recordId, string actionType, string oldValues, string newValues, string notes = "")
        {
            string query = @"INSERT INTO System_Audit_Log 
                            (Table_Name, Record_ID, Action_Type, User_ID, Action_Timestamp, Old_Values, New_Values, Notes) 
                             VALUES 
                            (@Table_Name, @Record_ID, @Action_Type, @User_ID, GETDATE(), @Old_Values, @New_Values, @Notes)";

            using (SqlCommand cmd = new SqlCommand(query, transaction.Connection, transaction))
            {
                cmd.Parameters.AddWithValue("@Table_Name", tableName);
                cmd.Parameters.AddWithValue("@Record_ID", recordId);
                cmd.Parameters.AddWithValue("@Action_Type", actionType);
                cmd.Parameters.AddWithValue("@User_ID", UserSession.UserId);
                cmd.Parameters.AddWithValue("@Old_Values", string.IsNullOrEmpty(oldValues) ? (object)DBNull.Value : oldValues);
                cmd.Parameters.AddWithValue("@New_Values", string.IsNullOrEmpty(newValues) ? (object)DBNull.Value : newValues);
                cmd.Parameters.AddWithValue("@Notes", string.IsNullOrEmpty(notes) ? (object)DBNull.Value : notes);
                cmd.ExecuteNonQuery();
            }
        }

        #endregion

        /// <summary>
        /// دالة مركزية لإنشاء وفتح الاتصال بقاعدة البيانات بشكل غير متزامن
        /// </summary>
        // الكود الدستوري المحدث داخل DatabaseHelper.cs
        public static async Task<SqlConnection> GetConnectionAsync()
        {
            // الدستور: قراءة نص الاتصال من ملف الإعدادات بالاسم الفعلي المُولد
            string connectionStringName = "AlRowad_ERP.Properties.Settings.AlRowad_ERPConnectionString";
            string connectionString = ConfigurationManager.ConnectionStrings[connectionStringName]?.ConnectionString;

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new System.InvalidOperationException($"نص الاتصال '{connectionStringName}' غير متاح أو غير معرّف في إعدادات النظام.");
            }

            var connection = new SqlConnection(connectionString);

            try
            {
                await connection.OpenAsync();
            }
            catch (SqlException ex)
            {
                connection.Dispose();
                throw new System.Exception("تعذر إنشاء اتصال بقاعدة البيانات. يرجى التحقق من إعدادات الخادم.", ex);
            }

            return connection;
        }
        public static async Task<bool> ExecuteTransactionAsync(Func<SqlTransaction, Task> action)
        {
            using (var connection = await GetConnectionAsync()) // بافتراض أن لديك دالة GetConnectionAsync
            {
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        await action(transaction); // تنفيذ العمليات (Master-Details)
                        transaction.Commit();      // الحفظ ككتلة واحدة
                        return true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();    // التراجع الكامل عند الخطأ (ACID)
                                                   // يفضل هنا إضافة سطر لتسجيل الخطأ في سجلات النظام (Log Error)
                        throw; // إعادة رمي الخطأ لتعرف سبب المشكلة أثناء التطوير
                    }
                }
            }
        }
        /// <summary>
        /// الدالة المركزية لتسجيل الأخطاء العميقة بصمت في قاعدة البيانات
        /// تُستدعى من GlobalExceptionHandler
        /// </summary>
        public static void LogSystemError(string errorMessage, string stackTrace, string threadType)
        {
            try
            {
                string query = @"INSERT INTO System_Error_Logs 
                                (ErrorMessage, StackTrace, ThreadType, User_ID, Created_Date) 
                                 VALUES 
                                (@ErrorMessage, @StackTrace, @ThreadType, @UserId, GETDATE())";

                SqlParameter[] parameters = {
                    new SqlParameter("@ErrorMessage", string.IsNullOrEmpty(errorMessage) ? (object)DBNull.Value : errorMessage),
                    new SqlParameter("@StackTrace", string.IsNullOrEmpty(stackTrace) ? (object)DBNull.Value : stackTrace),
                    new SqlParameter("@ThreadType", string.IsNullOrEmpty(threadType) ? (object)DBNull.Value : threadType),
                    // تطبيق الدستور: استخدام SystemConstants / UserSession بدلاً من القيم الثابتة
                    new SqlParameter("@UserId", UserSession.UserId > 0 ? UserSession.UserId : (object)DBNull.Value)
                };

                // استخدام دالة التنفيذ المتزامنة لضمان سرعة التسجيل قبل انهيار التطبيق
                ExecuteNonQuery(query, parameters);
            }
            catch (Exception fallbackEx)
            {
                // الخطة ب (Fallback): إذا كان الخطأ الأصلي هو انقطاع السيرفر، فلن نتمكن من الحفظ في قاعدة البيانات.
                // لذلك نحفظ الخط�� في ملف نصي (Text File) داخل مجلد النظام.
                try
                {
                    string logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SystemLogs");
                    if (!Directory.Exists(logDirectory))
                    {
                        Directory.CreateDirectory(logDirectory);
                    }

                    string logFilePath = Path.Combine(logDirectory, "FatalErrors_Log.txt");
                    string logMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Thread: {threadType}\n" +
                                        $"Error: {errorMessage}\n" +
                                        $"StackTrace: {stackTrace}\n" +
                                        $"Fallback Error (DB Fail): {fallbackEx.Message}\n" +
                                        new string('-', 50) + "\n";

                    File.AppendAllText(logFilePath, logMessage);
                }
                catch
                {
                    // يتم تجاهل الأخطاء هنا لمنع الدخول في حلقة لانهائية من الأخطاء
                }
            }
        }
    }
}
