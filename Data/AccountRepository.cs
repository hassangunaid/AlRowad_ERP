using AlRowad_ERP.Core.Constants;
using AlRowad_ERP.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace AlRowad_ERP.Data
{
    public static partial class AccountRepository
    {
        /// <summary>
        /// حفظ الحساب مع العملات المرافقة داخل معاملة موجودة
        /// </summary>
        public static async Task SaveAccountAsync(AccountEntity account, IList<AccountCurrencyDto> currencies, AlRowad_ERP.UI.Base.FormMode mode, SqlTransaction trans)
        {
            if (trans == null) throw new ArgumentNullException(nameof(trans), "Transaction must be provided by caller.");

            // 1. تحضير استعلام الرأس (INSERT أو UPDATE مع دعم RowVersion للتحقق من التضارب)
            string sqlHeader = mode == AlRowad_ERP.UI.Base.FormMode.Edit
                ? $@"UPDATE Accounts SET Acc_Name = @Acc_Name, Acc_Name_En = @Acc_Name_En, Parent_ID = @Parent_ID, Account_Level = @Account_Level, 
                        Acc_Type = @Acc_Type, Acc_Nature = @Acc_Nature, Report_Type = @Report_Type, Is_Stopped = @Is_Stopped,
                        {SystemConstants.AuditFields.UpdatedBy} = @Updated_By, {SystemConstants.AuditFields.UpdatedAt} = GETDATE()
                    WHERE Acc_ID = @Acc_ID AND RowVersion = @OldRowVersion"
                : $@"INSERT INTO Accounts (Acc_ID, Acc_Name, Acc_Name_En, Parent_ID, Account_Level, Acc_Type, Acc_Nature, Report_Type, Is_Stopped, {SystemConstants.AuditFields.CreatedBy}, {SystemConstants.AuditFields.CreatedAt})
                    VALUES (@Acc_ID, @Acc_Name, @Acc_Name_En, @Parent_ID, @Account_Level, @Acc_Type, @Acc_Nature, @Report_Type, @Is_Stopped, @Created_By, GETDATE())";

            var headerParams = new List<SqlParameter>
            {
                new SqlParameter("@Acc_ID", account.AccID),
                new SqlParameter("@Acc_Name", account.AccName ?? (object)DBNull.Value),
                new SqlParameter("@Acc_Name_En", string.IsNullOrWhiteSpace(account.AccNameEn) ? (object)DBNull.Value : account.AccNameEn),
                new SqlParameter("@Parent_ID", string.IsNullOrWhiteSpace(account.ParentID) ? (object)DBNull.Value : account.ParentID),
                new SqlParameter("@Account_Level", account.AccountLevel),
                new SqlParameter("@Acc_Type", account.AccType),
                new SqlParameter("@Acc_Nature", account.AccNature),
                new SqlParameter("@Report_Type", account.ReportType),
                new SqlParameter("@Is_Stopped", account.IsStopped)
            };

            if (mode == AlRowad_ERP.UI.Base.FormMode.New)
            {
                headerParams.Add(new SqlParameter("@Created_By", account.CreatedBy > 0 ? (object)account.CreatedBy : DBNull.Value));
            }
            else
            {
                headerParams.Add(new SqlParameter("@Updated_By", account.UpdatedBy > 0 ? (object)account.UpdatedBy : DBNull.Value));
                headerParams.Add(new SqlParameter("@OldRowVersion", System.Data.SqlDbType.Timestamp) { Value = account.RowVersion ?? (object)DBNull.Value });
            }

            int rowsAffected = await DatabaseHelper.ExecuteNonQueryAsync(sqlHeader, headerParams.ToArray(), trans);

            if (mode == AlRowad_ERP.UI.Base.FormMode.Edit && rowsAffected == 0)
            {
                throw new InvalidOperationException("تضارب بيانات: تم تعديل هذا السجل من قبل مستخدم آخر أثناء استعراضك له.");
            }

            // 2. تحديث جدول العملات: حذف ثم إدراج
            if (currencies != null)
            {
                await DatabaseHelper.ExecuteNonQueryAsync("DELETE FROM Account_Allowed_Currencies WHERE Acc_ID = @AccID", new[] { new System.Data.SqlClient.SqlParameter("@AccID", account.AccID) }, trans);

                string insertCur = "INSERT INTO Account_Allowed_Currencies (Acc_ID, Cur_ID, Is_Active, Is_Frozen, Is_Default) VALUES (@AccID, @CurID, @IsActive, @IsFrozen, @IsDefault)";

                foreach (var c in currencies)
                {
                    var p = new[] {
                        new System.Data.SqlClient.SqlParameter("@AccID", account.AccID),
                        new System.Data.SqlClient.SqlParameter("@CurID", c.CurID),
                        new System.Data.SqlClient.SqlParameter("@IsActive", c.IsActive),
                        new System.Data.SqlClient.SqlParameter("@IsFrozen", c.IsFrozen),
                        new System.Data.SqlClient.SqlParameter("@IsDefault", c.IsDefault)
                    };

                    await DatabaseHelper.ExecuteNonQueryAsync(insertCur, p, trans);
                }
            }

            // 3. تسجيل Audit عند التعديل
            if (mode == AlRowad_ERP.UI.Base.FormMode.Edit)
            {
                string oldValues = "تم الحفظ المسبق"; // placeholder — يمكن تحسينه لاحقاً لالتقاط القيم القديمة
                string newValues = $"الاسم: {account.AccName} | إيقاف: {account.IsStopped}";
                DatabaseHelper.LogAuditTransaction(trans, "Accounts", account.AccID, "UPDATE", oldValues, newValues, "تحديث الحساب");
            }
        }
    }
}
