using AlRowad_ERP.Core;
using AlRowad_ERP.Data; // الاعتماد الحصري على DatabaseHelper وفقاً للدستور
using AlRowad_ERP.Models.Reports;
using Dapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AlRowad_ERP.Repositories
{
    public class ReportRepository
    {
        /// <summary>
        /// جلب بيانات كشف الحساب بشكل غير متزامن (Asynchronous) لضمان عدم تجميد واجهة المستخدم
        /// </summary>
        public async Task<IEnumerable<AccountStatementDto>> GetAccountStatementAsync(int accountId)
        {
            // استخدام الطريقة الكلاسيكية الآمنة لفتح وإغلاق الاتصال المتوافقة مع جميع إصدارات C#
            using (var connection = DatabaseHelper.GetConnection())
            {
                string query = @"
                    SELECT 
                        Source_ID, 
                        Trans_Date, 
                        Description, 
                        Debit, 
                        Credit, 
                        Post_Status, 
                        Running_Balance 
                    FROM System_Ledger 
                    WHERE Account_ID = @AccountId 
                    ORDER BY Trans_Date, ID";

                var result = await connection.QueryAsync<AccountStatementDto>(query, new { AccountId = accountId });
                return result;
            }
        }
    }
}