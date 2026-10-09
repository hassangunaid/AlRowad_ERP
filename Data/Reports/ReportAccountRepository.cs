using AlRowad_ERP.Core.Constants;
using AlRowad_ERP.Data; // مسار كلاس DatabaseHelper الخاص بك
using AlRowad_ERP.Models.Reports;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using AlRowad_ERP.Core;

namespace AlRowad_ERP.Data.Reports
{
    public class ReportAccountRepository
    {
        public async Task<List<AccountStatementDto>> GetStatementAsync(string accountId, DateTime fromDate, DateTime toDate, bool includeUnposted)
        {
            // استخدام DatabaseHelper حسب الدستور

            using (var connection = DatabaseHelper.GetConnection())
            {
                // منع الـ Magic Strings
                string spName = includeUnposted
                    ? SystemConstants.StoredProcedures.GetUnpostedAccountStatement
                    : SystemConstants.StoredProcedures.GetAccountStatement;

                var parameters = new DynamicParameters();
                parameters.Add("@AccID", accountId, DbType.String);
                parameters.Add("@FromDate", fromDate, DbType.DateTime);
                parameters.Add("@ToDate", toDate, DbType.DateTime);

                // استدعاء غير متزامن للحفاظ على سلاسة واجهة المستخدم (الدستور)
                var result = await connection.QueryAsync<AccountStatementDto>(
                    spName,
                    parameters,
                    commandType: CommandType.StoredProcedure);

                return result.ToList();
            }
        }
    }
}