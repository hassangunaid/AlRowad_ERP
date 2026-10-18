using AlRowad_ERP.Core;
using AlRowad_ERP.Core.Constants;
using AlRowad_ERP.Data;
using AlRowad_ERP.Models.Reports;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace AlRowad_ERP.Data.Reports
{
    /// <summary>
    /// طبقة بيانات تقارير الحسابات - متوافقة تماماً مع دستور الرواد ERP
    /// </summary>
    public class ReportAccountRepository
    {
        /// <summary>
        /// جلب كشف الحساب غير المتزامن مع تضمين ترويسة الحساب (رقم واسم الحساب) لكل سجل
        /// </summary>
        public async Task<List<AccountStatementDto>> GetStatementAsync(string accountId, DateTime fromDate, DateTime toDate, bool includeUnposted)
        {
            var statementList = new List<AccountStatementDto>();

            using (var connection = DatabaseHelper.GetConnection())
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                // 1. جلب اسم ورقم الحساب أولاً لضمان توفرها في الترويسة (Clean Architecture)
                string accountInfoQuery = "SELECT Acc_ID, Acc_Name FROM Accounts WHERE Acc_ID = @AccID";
                var accountInfo = await connection.QueryFirstOrDefaultAsync<dynamic>(
                    accountInfoQuery,
                    new { AccID = accountId });

                string accIdVal = accountInfo?.Acc_ID ?? accountId;
                string accNameVal = accountInfo?.Acc_Name ?? string.Empty;

                // 2. تحديد الإجراء المخزن بمنع الـ Magic Strings عبر SystemConstants
                string spName = includeUnposted
                    ? SystemConstants.StoredProcedures.GetUnpostedAccountStatement
                    : SystemConstants.StoredProcedures.GetAccountStatement;

                var parameters = new DynamicParameters();
                parameters.Add("@AccID", accountId, DbType.String);
                parameters.Add("@FromDate", fromDate, DbType.DateTime);
                parameters.Add("@ToDate", toDate, DbType.DateTime);

                // 3. جلب الحركات المحاسبية غير المتزامن للحفاظ على سلاسة واجهة المستخدم (المادة 3 من الدستور)
                var result = await connection.QueryAsync<AccountStatementDto>(
                    spName,
                    parameters,
                    commandType: CommandType.StoredProcedure);

                if (result != null)
                {
                    statementList = result.ToList();

                    // حقن رقم واسم الحساب في كل سطر ليتطابق مع حقول التقرير (RDLC Fields)
                    foreach (var item in statementList)
                    {
                        item.AccountId = accIdVal;
                        item.AccountName = accNameVal;
                    }
                }
            }

            return statementList;
        }

        /// <summary>
        /// استعلام دليل الحسابات لشاشة البحث (F9)
        /// </summary>
        public string GetAccountsLookupQuery()
        {
            return "SELECT Acc_ID AS [رقم الحساب], Acc_Name AS [اسم الحساب] FROM Accounts";
        }

        /// <summary>
        /// جلب تاريخ بداية الفترة المالية المفتوحة الحالية
        /// </summary>
        public DateTime GetCurrentPeriodStartDate()
        {
            string query = "SELECT TOP 1 Start_Date FROM Financial_Periods WHERE Is_Closed = 0 AND @CurrentDate BETWEEN Start_Date AND End_Date";
            SqlParameter[] parameters = {
                new SqlParameter("@CurrentDate", SqlDbType.DateTime) { Value = DateTime.Now.Date }
            };

            var startDateObj = DatabaseHelper.ExecuteScalar(query, parameters);
            if (startDateObj != null && startDateObj != DBNull.Value)
                return Convert.ToDateTime(startDateObj);

            // قيمة افتراضية احتياطية في حال عدم وجود فترة معرفة
            return new DateTime(DateTime.Now.Year, 1, 1);
        }
    }
}