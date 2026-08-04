using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace AlRowad_ERP.Core
{
    public static class DatabaseHelper
    {
        // ط§ط³طھط®ط¯ط§ظ… ط³ظ„ط³ظ„ط© ط§ظ„ط§طھطµط§ظ„ ط§ظ„ظ…ط±ظƒط²ظٹط© ظ…ظ† ط§ظ„ط¯ط³طھظˆط±
        private static string GetConnectionString()
        {
            return @"Server=DESKTOP-U2PUV95\SQLEXPRESS;Database=AlRowad_ERP;Trusted_Connection=True;MultipleActiveResultSets=True;";
        }

        private static SqlConnection connection = new SqlConnection(GetConnectionString());

        public static void OpenConnection()
        {
            if (connection.State != ConnectionState.Open) connection.Open();
        }

        public static void CloseConnection()
        {
            if (connection.State != ConnectionState.Closed) connection.Close();
        }

        public static SqlConnection GetConnection()
        {
            return connection;
        }

        // ط§ظ„ط¯ط§ظ„ط© ط§ظ„ظ…ظˆط­ط¯ط© ظ„ظ„طھظ†ظپظٹط° (ط¥ط¶ط§ظپط©/طھط¹ط¯ظٹظ„/ط­ط°ظپ) ظ…ط¹ ط¯ط¹ظ… ط§ظ„ظ€ Transaction
        public static void ExecuteNonQuery(string query, SqlTransaction transaction = null)
        {
            using (SqlCommand cmd = new SqlCommand(query, GetConnection()))
            {
                if (transaction != null) cmd.Transaction = transaction;
                OpenConnection();
                cmd.ExecuteNonQuery();
            }
        }

        // ط§ظ„ط¯ط§ظ„ط© ط§ظ„ظ…ظˆط­ط¯ط© ظ„ط¬ظ„ط¨ ط§ظ„ط¨ظٹط§ظ†ط§طھ (ظ„ط´ط¬ط±ط© ط§ظ„ط­ط³ط§ط¨ط§طھ ظˆط§ظ„ط¨ط­ط«)
        public static DataTable GetTable(string query)
        {
            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand(query, GetConnection()))
            {
                OpenConnection();
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }
            }
            return dt;
        }

        // ط¯ط§ظ„ط© طھظˆظ„ظٹط¯ ط§ظ„ط£ظƒظˆط§ط¯ ط§ظ„ظ…ظ†ط³ظ‚ط© (0001, 0002...)
        public static string GetNextCode(string tableName, string columnName)
        {
            string query = $"SELECT ISNULL(MAX(CAST({columnName} AS INT)), 0) + 1 FROM {tableName} WHERE ISNUMERIC({columnName}) = 1";
            try
            {
                OpenConnection();
                using (SqlCommand cmd = new SqlCommand(query, GetConnection()))
                {
                    object result = cmd.ExecuteScalar();
                    return result != null ? Convert.ToInt32(result).ToString("D4") : "0001";
                }
            }
            catch { return "0001"; }
            finally { CloseConnection(); }
        }
    }
}