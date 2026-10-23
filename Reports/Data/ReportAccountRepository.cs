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
    public class ReportAccountRepository
    {
        /// <summary>
        /// جلب كشف الحساب بدعم تعدد العملات (عملة محددة أو كافة العملات) مع تفقيط كل عملة باسمها
        /// </summary>
        public async Task<List<AccountStatementDto>> GetStatementAsync(
            string accountId,
            DateTime fromDate,
            DateTime toDate,
            bool includeUnposted,
            int? currencyId = null)
        {
            var statementList = new List<AccountStatementDto>();

            using (var connection = DatabaseHelper.GetConnection())
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                // 1. جلب بيانات ترويسة الحساب
                string accountInfoQuery = "SELECT Acc_ID, Acc_Name FROM Accounts WHERE Acc_ID = @AccID";
                var accountInfo = await connection.QueryFirstOrDefaultAsync<dynamic>(
                    accountInfoQuery,
                    new { AccID = accountId });

                string accIdVal = accountInfo?.Acc_ID ?? accountId;
                string accNameVal = accountInfo?.Acc_Name ?? string.Empty;

                // 2. تحديد الإجراء المخزن عبر SystemConstants (منع Magic Strings)
                string spName = includeUnposted
                    ? SystemConstants.StoredProcedures.GetUnpostedAccountStatement
                    : SystemConstants.StoredProcedures.GetAccountStatement;

                var parameters = new DynamicParameters();
                parameters.Add("@AccID", accountId, DbType.String);
                parameters.Add("@FromDate", fromDate, DbType.DateTime);
                parameters.Add("@ToDate", toDate, DbType.DateTime);

                // تمرير رقم العملة (NULL يجلب كافة العملات في الإجراء المخزن)
                parameters.Add("@CurrencyID", (currencyId.HasValue && currencyId.Value > 0) ? currencyId.Value : (int?)null, DbType.Int32);

                // 3. تنفيذ الاستعلام غير المتزامن
                var result = await connection.QueryAsync<AccountStatementDto>(
                    spName,
                    parameters,
                    commandType: CommandType.StoredProcedure);

                if (result != null)
                {
                    statementList = result.ToList();

                    // 4. التجميع المحاسبي حسب العملة (يمنع خلط عملات مختلفة في إجمالي واحد)
                    var currencyGroups = statementList.GroupBy(x => new { x.Currency_ID, x.Currency_Name });

                    foreach (var group in currencyGroups)
                    {
                        string mainCurrName = !string.IsNullOrWhiteSpace(group.Key.Currency_Name)
                            ? group.Key.Currency_Name
                            : "ريال";

                        string subCurrName = group.FirstOrDefault()?.Sub_Currency_Name;
                        if (string.IsNullOrWhiteSpace(subCurrName))
                            subCurrName = GetDefaultSubCurrency(mainCurrName);

                        decimal grpDebit = group.Sum(x => x.Debit);
                        decimal grpCredit = group.Sum(x => x.Credit);
                        decimal netBalance = grpDebit - grpCredit;
                        decimal absBalance = Math.Abs(netBalance);

                        string balanceTypeText = netBalance > 0
                            ? "مدين - عليكم"
                            : (netBalance < 0 ? "دائن - لكم" : "متزن");

                        // 🌟 تفقيط المبلغ مدمجاً باسم العملة الخاصة بهذه المجموعة
                        string amountInWords = CurrencyTafqeetHelper.ConvertToWords(absBalance, mainCurrName, subCurrName);

                        // إعادة ضبط الرصيد التراكمي وحقن الخلاصة لكل عملة بشكل مستقل
                        decimal runningBal = 0;
                        foreach (var item in group)
                        {
                            runningBal += (item.Debit - item.Credit);
                            item.Running_Balance = runningBal;

                            item.AccountId = accIdVal;
                            item.AccountName = accNameVal;
                            item.Currency_Name = mainCurrName;
                            item.Total_Debit = grpDebit;
                            item.Total_Credit = grpCredit;
                            item.Final_Balance = absBalance;
                            item.Balance_Type_Text = balanceTypeText;
                            item.Amount_In_Words = amountInWords;
                        }
                    }
                }
            }

            return statementList;
        }

        /// <summary>
        /// جلب قائمة العملات من جدول Currencies (Cur_ID, Cur_Name) مع خيار كافة العملات
        /// </summary>
        public async Task<DataTable> GetCurrenciesLookupAsync()
        {
            string query = @"
        SELECT 0 AS Currency_ID, N'--- كافة العملات ---' AS Currency_Name
        UNION ALL
        SELECT Cur_ID AS Currency_ID, Cur_Name AS Currency_Name 
        FROM dbo.Currencies 
        WHERE Is_Active = 1 AND ISNULL(Is_Deleted, 0) = 0";

            return await Task.Run(() => DatabaseHelper.ExecuteQuery(query, null));
        }
        private string GetDefaultSubCurrency(string mainCurrency)
        {
            if (mainCurrency.Contains("دولار")) return "سنت";
            if (mainCurrency.Contains("سعودي")) return "هللة";
            return "فلس";
        }

        public string GetAccountsLookupQuery()
        {
            return "SELECT Acc_ID AS [رقم الحساب], Acc_Name AS [اسم الحساب] FROM Accounts";
        }

        public DateTime GetCurrentPeriodStartDate()
        {
            string query = "SELECT TOP 1 Start_Date FROM Financial_Periods WHERE Is_Closed = 0 AND @CurrentDate BETWEEN Start_Date AND End_Date";
            SqlParameter[] parameters = {
                new SqlParameter("@CurrentDate", SqlDbType.DateTime) { Value = DateTime.Now.Date }
            };

            var startDateObj = DatabaseHelper.ExecuteScalar(query, parameters);
            if (startDateObj != null && startDateObj != DBNull.Value)
                return Convert.ToDateTime(startDateObj);

            return new DateTime(DateTime.Now.Year, 1, 1);
        }
    }
}