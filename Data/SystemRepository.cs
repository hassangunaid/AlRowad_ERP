using AlRowad_ERP.Core;
using AlRowad_ERP.Core.Helpers; // مسار DatabaseHelper
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace AlRowad_ERP.Data
{
    public class SystemRepository
    {
        /// <summary>
        /// جلب الرقم التسلسلي التالي بشكل لامتزامن وآمن
        /// </summary>
        public async Task<string> GetNextDocumentNumberAsync(string tableName, string columnName)
        {
            // ملاحظة: نستخدم String Interpolation هنا لأسماء الجداول لأن SQL Parameters لا تدعم تمرير أسماء الجداول أو الأعمدة.
            // يجب التأكد من تمرير ثوابت (SystemConstants) لهذه الدالة وليس نصوصاً من إدخال المستخدم لمنع SQL Injection.
            string query = $"SELECT ISNULL(MAX(CAST({columnName} AS INT)), 0) + 1 FROM {tableName} WHERE ISNUMERIC({columnName}) = 1";

            object result = await DatabaseHelper.ExecuteScalarAsync(query);

            return result != null && result != DBNull.Value ? Convert.ToInt32(result).ToString("D4") : "0001";
        }
    }
}