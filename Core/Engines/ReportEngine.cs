using System;
using System.Data;
using System.Data.SqlClient;

namespace AlRowad_ERP.Core
{
    public static class ReportEngine
    {
        /// <summary>
        /// استخراج كشف حساب ديناميكي حسب العملة المختارة
        /// </summary>
        public static DataTable GetAccountStatement(string accID, int currencyID, DateTime fromDate, DateTime toDate)
        {
            // نستخدم القيمة الأجنبية إذا كانت العملة أجنبية، والمحلية إذا كانت العملة هي عملة النظام
            string query = @"
                SELECT 
                    Notes AS البيان,
                    Post_Date AS التاريخ,
                    Amount_FC AS [المبلغ بالعملة المختارة],
                    (Amount_Local) AS [المقابل بالعملة المحلية]
                FROM System_Ledger
                WHERE Acc_ID = @AccID 
                  AND Currency_ID = @CurID
                  AND Post_Date BETWEEN @From AND @To";

            SqlParameter[] pars = {
                new SqlParameter("@AccID", accID),
                new SqlParameter("@CurID", currencyID),
                new SqlParameter("@From", fromDate),
                new SqlParameter("@To", toDate)
            };

            return DatabaseHelper.ExecuteQuery(query, pars);
        }
    }
}