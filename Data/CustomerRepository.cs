using AlRowad_ERP.Core;
using AlRowad_ERP.Core.Constants;
using AlRowad_ERP.Core.Helpers;
using AlRowad_ERP.Core.Models;
using AlRowad_ERP.UI.Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace AlRowad_ERP.Data
{
    public static class CustomerRepository
    {
        // 🌟 1. جلب بيانات العميل
        public static async Task<DataTable> GetCustomerDetailsAsync(string customerCode)
        {
            string query = $@"
                SELECT c.Cust_ID, c.Cust_Name, c.Cust_Phone, c.Cust_Address, c.Acc_ID, c.RowVersion,
                       c.{SystemConstants.AuditFields.CreatedBy}, c.{SystemConstants.AuditFields.CreatedAt}, 
                       c.{SystemConstants.AuditFields.UpdatedBy}, c.{SystemConstants.AuditFields.UpdatedAt},
                       ISNULL(uc.Full_Name, uc.Username) AS CreatedByName,
                       ISNULL(uu.Full_Name, uu.Username) AS UpdatedByName
                FROM Customers c
                LEFT JOIN Users uc ON c.{SystemConstants.AuditFields.CreatedBy} = uc.User_ID
                LEFT JOIN Users uu ON c.{SystemConstants.AuditFields.UpdatedBy} = uu.User_ID
                WHERE c.Cust_ID = @Code";

            return await DatabaseHelper.GetTableAsync(query, new[] { new SqlParameter("@Code", customerCode.Trim()) });
        }

        // 🌟 2. الحفظ اللامتزامن مع دعم الـ ACID Transaction
        public static async Task SaveCustomerTransactionAsync(
            FormMode mode, string custId, string custName, string custPhone, string custAddress,
            string accId, object parentId, byte[] rowVersion, int currentUserId,
            List<CustomerCurrencyDto> currencies, SqlTransaction trans)
        {
            if (mode == FormMode.New)
            {
                // إدراج الحساب المالي
                string accSql = $@"INSERT INTO Accounts (Acc_ID, Acc_Name, Is_Stopped, Account_Level, Parent_ID, Acc_Type, Acc_Nature, Report_Type, {SystemConstants.AuditFields.CreatedBy}, {SystemConstants.AuditFields.CreatedAt}) 
                                   VALUES (@AccID, @AccName, 0, 5, @ParentID, 1, 1, 1, @Created_By, @Created_At)";
                await DatabaseHelper.ExecuteNonQueryAsync(accSql, new[] {
                    new SqlParameter("@AccID", accId), new SqlParameter("@AccName", custName),
                    new SqlParameter("@ParentID", parentId ?? (object)DBNull.Value),
                    new SqlParameter("@Created_By", currentUserId), new SqlParameter("@Created_At", DateTime.Now)
                }, trans);

                // إدراج بيانات العميل
                string custSql = $@"INSERT INTO Customers (Cust_ID, Cust_Name, Cust_Phone, Cust_Address, Acc_ID, {SystemConstants.AuditFields.CreatedBy}, {SystemConstants.AuditFields.CreatedAt}) 
                                    VALUES (@Code, @Name, @Phone, @Address, @AccNo, @Created_By, @Created_At)";
                await DatabaseHelper.ExecuteNonQueryAsync(custSql, new[] {
                    new SqlParameter("@Code", custId), new SqlParameter("@Name", custName),
                    new SqlParameter("@Phone", custPhone), new SqlParameter("@Address", custAddress),
                    new SqlParameter("@AccNo", accId), new SqlParameter("@Created_By", currentUserId),
                    new SqlParameter("@Created_At", DateTime.Now)
                }, trans);
            }
            else if (mode == FormMode.Edit)
            {
                // تحديث الحساب المالي
                string updateAcc = $@"UPDATE Accounts SET Acc_Name = @Name, {SystemConstants.AuditFields.UpdatedBy} = @Updated_By, {SystemConstants.AuditFields.UpdatedAt} = @Updated_At WHERE Acc_ID = @AccID";
                await DatabaseHelper.ExecuteNonQueryAsync(updateAcc, new[] {
                    new SqlParameter("@Name", custName), new SqlParameter("@AccID", accId),
                    new SqlParameter("@Updated_By", currentUserId), new SqlParameter("@Updated_At", DateTime.Now)
                }, trans);

                // تحديث بيانات العميل مع فحص تضارب التزامن (Concurrency)
                string updateCust = $@"UPDATE Customers SET Cust_Name = @Name, Cust_Phone = @Phone, Cust_Address = @Address, {SystemConstants.AuditFields.UpdatedBy} = @Updated_By, {SystemConstants.AuditFields.UpdatedAt} = @Updated_At 
                                       WHERE Cust_ID = @Code AND RowVersion = @OldRowVersion";

                int rowsAffected = await DatabaseHelper.ExecuteNonQueryAsync(updateCust, new[] {
                    new SqlParameter("@Name", custName), new SqlParameter("@Phone", custPhone),
                    new SqlParameter("@Address", custAddress), new SqlParameter("@Code", custId),
                    new SqlParameter("@Updated_By", currentUserId), new SqlParameter("@Updated_At", DateTime.Now),
                    new SqlParameter("@OldRowVersion", SqlDbType.Timestamp) { Value = rowVersion ?? (object)DBNull.Value }
                }, trans);

                if (rowsAffected == 0) throw new InvalidOperationException("تضارب بيانات: تم تعديل بيانات هذا العميل من قبل مستخدم آخر أثناء استعراضك لها.");

                DatabaseHelper.LogAuditTransaction(trans, "Customers", custId, "UPDATE", "تم الحفظ المسبق", $"الاسم: {custName}", "تعديل عميل");
            }

            // تحديث عملات العميل
            await DatabaseHelper.ExecuteNonQueryAsync("DELETE FROM Account_Allowed_Currencies WHERE Acc_ID = @AccID", new[] { new SqlParameter("@AccID", accId) }, trans);
            string curSql = "INSERT INTO Account_Allowed_Currencies (Acc_ID, Cur_ID, Is_Default, Is_Frozen, Is_Active) VALUES (@AccID, @CurID, @IsDefault, @IsFrozen, 1)";
            foreach (var cur in currencies)
            {
                if (cur.IsActive)
                {
                    await DatabaseHelper.ExecuteNonQueryAsync(curSql, new[] {
                        new SqlParameter("@AccID", accId), new SqlParameter("@CurID", cur.CurrencyId),
                        new SqlParameter("@IsDefault", cur.IsDefault), new SqlParameter("@IsFrozen", cur.IsFrozen)
                    }, trans);
                }
            }
        }

        // 🌟 3. الحذف المنطقي (Soft Delete)
        // 🌟 الحذف المنطقي (Soft Delete) المتوافق مع مبدأ المسؤولية الفردية (SRP)
        public static async Task SoftDeleteCustomerAsync(string custId, int currentUserId, SqlTransaction trans)
        {
            // الدستور: استخدام SystemConstants لمنع الثوابت النصية، وتمرير الـ trans لضمان الـ ACID
            string updateCust = $@"
                UPDATE Customers 
                SET {SystemConstants.AuditFields.IsDeleted} = 1, 
                    {SystemConstants.AuditFields.DeletedBy} = @UserId, 
                    {SystemConstants.AuditFields.DeletedAt} = GETDATE() 
                WHERE Cust_ID = @CustID";

            await DatabaseHelper.ExecuteNonQueryAsync(updateCust, new[] {
                new SqlParameter("@CustID", custId),
                new SqlParameter("@UserId", currentUserId)
            }, trans);

            // 💡 ملاحظة معمارية: تم حذف كود إيقاف الحساب من هنا، 
            // لأن AccountRepository أصبح هو المسؤول الوحيد عن إدارة حالة الحسابات.
        }
    }
}