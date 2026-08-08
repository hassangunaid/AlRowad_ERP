using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using AlRowad_ERP.Core;
using AlRowad_ERP.Models;
using AlRowad_ERP.Core.Constants;

namespace AlRowad_ERP.Data
{
    public class JournalRepository
    {
        public async Task<int> InsertHeaderAsync(JournalHeaderModel header, SqlTransaction transaction)
        {
            string query = $@"
                INSERT INTO Journal_Header (Journal_Date, Description, {SystemConstants.AuditFields.CreatedBy}, Created_Date) 
                OUTPUT INSERTED.Journal_ID 
                VALUES (@Date, @Desc, @UserId, GETDATE())";

            SqlParameter[] parameters = {
                new SqlParameter("@Date", header.JournalDate),
                new SqlParameter("@Desc", header.Description ?? (object)DBNull.Value),
                new SqlParameter("@UserId", header.CreatedBy ?? (object)DBNull.Value)
            };

            object result = await DatabaseHelper.ExecuteScalarAsync(query, parameters, transaction);
            return Convert.ToInt32(result);
        }

        public async Task InsertDetailAsync(JournalDetailModel detail, SqlTransaction transaction)
        {
            string query = @"
                INSERT INTO Journal_Details (Journal_ID, Account_ID, Debit_Amount, Credit_Amount) 
                VALUES (@JournalId, @AccountId, @Debit, @Credit)";

            SqlParameter[] parameters = {
                new SqlParameter("@JournalId", detail.JournalId),
                new SqlParameter("@AccountId", detail.AccountId),
                new SqlParameter("@Debit", detail.DebitAmount),
                new SqlParameter("@Credit", detail.CreditAmount)
            };

            await DatabaseHelper.ExecuteNonQueryAsync(query, parameters, transaction);
        }
    }
}