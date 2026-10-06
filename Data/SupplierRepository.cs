using AlRowad_ERP.Core;
using AlRowad_ERP.Core.Constants;
using AlRowad_ERP.Core.Helpers;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace AlRowad_ERP.Data
{
    public class SupplierRepository
    {
        public async Task<DataTable> GetMainAccountsAsync()
        {
            string query = @"SELECT Acc_ID, Acc_ID + ' - ' + Acc_Name AS Acc_Full_Name 
                             FROM Accounts 
                             WHERE Is_Stopped = 0 AND Acc_ID LIKE '2101%' AND LEN(RTRIM(Acc_ID)) = 6
                             ORDER BY Acc_ID";
            return await DatabaseHelper.GetTableAsync(query);
        }

        public string GetNextSupplierId()
        {
            string query = "SELECT MAX(CAST(Supp_ID AS INT)) FROM Suppliers";
            object result = DatabaseHelper.ExecuteScalar(query);
            if (result != DBNull.Value && result != null)
                return (Convert.ToInt32(result) + 1).ToString("D4");
            return "0001";
        }

        public string GenerateChildAccountId(string parentAccId)
        {
            if (string.IsNullOrEmpty(parentAccId)) return "";
            try
            {
                string query = "SELECT MAX(CAST(Acc_ID AS BIGINT)) FROM Accounts WHERE Parent_ID = @ParentID";
                object result = DatabaseHelper.ExecuteScalar(query, new[] { new SqlParameter("@ParentID", parentAccId.Trim()) });
                if (result != null && result != DBNull.Value) return (Convert.ToInt64(result) + 1).ToString();
                return parentAccId.Trim() + "0001";
            }
            catch { return parentAccId.Trim() + "0001"; }
        }

        public string GetSearchQuery()
        {
            return "SELECT Supp_ID AS [كود المورد], Supp_Name AS [اسم المورد], Supp_Phone AS [رقم الهاتف] FROM Suppliers WHERE Is_Deleted = 0 ORDER BY Supp_ID";
        }

        public async Task<DataTable> GetSupplierDataAsync(string supplierId)
        {
            string query = $@"
                SELECT s.*, 
                       ISNULL(uc.Full_Name, uc.Username) AS CreatedByName,
                       ISNULL(uu.Full_Name, uu.Username) AS UpdatedByName
                FROM Suppliers s
                LEFT JOIN Users uc ON s.{SystemConstants.AuditFields.CreatedBy} = uc.User_ID
                LEFT JOIN Users uu ON s.{SystemConstants.AuditFields.UpdatedBy} = uu.User_ID
                WHERE s.Supp_ID = @Code AND s.Is_Deleted = 0";
            return await DatabaseHelper.GetTableAsync(query, new[] { new SqlParameter("@Code", supplierId.Trim()) });
        }

        public async Task<DataTable> GetSupplierCurrenciesAsync(string accountId)
        {
            string query = "SELECT RTRIM(Cur_ID) AS Cur_ID, Is_Default, ISNULL(Is_Frozen, 0) AS Is_Frozen, ISNULL(Is_Active, 0) AS Is_Active FROM Account_Allowed_Currencies WHERE Acc_ID = @AccID";
            return await DatabaseHelper.GetTableAsync(query, new[] { new SqlParameter("@AccID", accountId.Trim()) });
        }

        public async Task<bool> SaveSupplierTransactionAsync(
            string suppId, string suppName, string phone, string address, string accId,
            object parentAccId, bool isFarmer, byte[] oldRowVersion,
            DataTable currenciesTable, int userId, bool isNew, SqlTransaction trans)
        {
            if (isNew)
            {
                string accSql = $@"INSERT INTO Accounts 
                                (Acc_ID, Acc_Name, Is_Stopped, Account_Level, Parent_ID, Acc_Type, Acc_Nature, Report_Type, {SystemConstants.AuditFields.CreatedBy}, {SystemConstants.AuditFields.CreatedAt}) 
                                VALUES (@AccID, @AccName, 0, 5, @ParentID, 1, 1, 0, @UserId, GETDATE())";
                await DatabaseHelper.ExecuteNonQueryAsync(accSql, new[] {
                    new SqlParameter("@AccID", accId), new SqlParameter("@AccName", suppName),
                    new SqlParameter("@ParentID", parentAccId ?? (object)DBNull.Value), new SqlParameter("@UserId", userId)
                }, trans);

                string suppSql = $@"INSERT INTO Suppliers 
                                  (Supp_ID, Supp_Name, Supp_Phone, Supp_Address, Acc_ID, Is_Farmer, {SystemConstants.AuditFields.CreatedBy}, {SystemConstants.AuditFields.CreatedAt}) 
                                  VALUES (@ID, @Name, @Phone, @Address, @AccID, @IsFarmer, @UserId, GETDATE())";
                await DatabaseHelper.ExecuteNonQueryAsync(suppSql, new[] {
                    new SqlParameter("@ID", suppId), new SqlParameter("@Name", suppName),
                    new SqlParameter("@Phone", phone), new SqlParameter("@Address", address),
                    new SqlParameter("@AccID", accId), new SqlParameter("@IsFarmer", isFarmer), new SqlParameter("@UserId", userId)
                }, trans);
            }
            else
            {
                string updateAcc = $@"UPDATE Accounts SET Acc_Name = @Name, {SystemConstants.AuditFields.UpdatedBy} = @UserId, {SystemConstants.AuditFields.UpdatedAt} = GETDATE() WHERE Acc_ID = @AccID";
                await DatabaseHelper.ExecuteNonQueryAsync(updateAcc, new[] {
                    new SqlParameter("@Name", suppName), new SqlParameter("@AccID", accId), new SqlParameter("@UserId", userId)
                }, trans);

                string updateSupp = $@"UPDATE Suppliers 
                                      SET Supp_Name = @Name, Supp_Phone = @Phone, Supp_Address = @Address, Is_Farmer = @IsFarmer,
                                          {SystemConstants.AuditFields.UpdatedBy} = @UserId, {SystemConstants.AuditFields.UpdatedAt} = GETDATE() 
                                      WHERE Supp_ID = @ID AND RowVersion = @OldRowVersion";

                int rowsAffected = await DatabaseHelper.ExecuteNonQueryAsync(updateSupp, new[] {
                    new SqlParameter("@Name", suppName), new SqlParameter("@Phone", phone),
                    new SqlParameter("@Address", address), new SqlParameter("@IsFarmer", isFarmer),
                    new SqlParameter("@ID", suppId), new SqlParameter("@UserId", userId),
                    new SqlParameter("@OldRowVersion", SqlDbType.Timestamp) { Value = oldRowVersion ?? (object)DBNull.Value }
                }, trans);

                if (rowsAffected == 0) throw new InvalidOperationException("تضارب بيانات: تم تعديل بيانات هذا المورد من قبل مستخدم آخر أثناء استعراضك لها.");
            }

            string delCurSql = "DELETE FROM Account_Allowed_Currencies WHERE Acc_ID = @AccID";
            await DatabaseHelper.ExecuteNonQueryAsync(delCurSql, new[] { new SqlParameter("@AccID", accId) }, trans);

            if (currenciesTable != null && currenciesTable.Rows.Count > 0)
            {
                string curSql = "INSERT INTO Account_Allowed_Currencies (Acc_ID, Cur_ID, Is_Active, Is_Frozen, Is_Default) VALUES (@AccID, @CurID, @IsActive, @IsFrozen, @IsDefault)";
                foreach (DataRow row in currenciesTable.Rows)
                {
                    await DatabaseHelper.ExecuteNonQueryAsync(curSql, new[] {
                        new SqlParameter("@AccID", accId), new SqlParameter("@CurID", row["Cur_ID"].ToString()),
                        new SqlParameter("@IsActive", Convert.ToBoolean(row["Is_Active"])),
                        new SqlParameter("@IsFrozen", Convert.ToBoolean(row["Is_Frozen"])),
                        new SqlParameter("@IsDefault", Convert.ToBoolean(row["Is_Default"]))
                    }, trans);
                }
            }
            return true;
        }

        public async Task<bool> ExecuteDeleteAsync(string suppId, string accId, int userId, SqlTransaction trans)
        {
            string updateSupp = $@"UPDATE Suppliers SET {SystemConstants.Columns.Is_Deleted} = 1, {SystemConstants.AuditFields.DeletedBy} = @UserId, {SystemConstants.AuditFields.DeletedAt} = GETDATE() WHERE Supp_ID = @SuppID";
            await DatabaseHelper.ExecuteNonQueryAsync(updateSupp, new[] { new SqlParameter("@SuppID", suppId.Trim()), new SqlParameter("@UserId", userId) }, trans);

            if (!string.IsNullOrWhiteSpace(accId))
            {
                string updateAcc = $@"UPDATE Accounts SET Is_Stopped = 1, {SystemConstants.AuditFields.UpdatedBy} = @UserId, {SystemConstants.AuditFields.UpdatedAt} = GETDATE() WHERE Acc_ID = @AccID";
                await DatabaseHelper.ExecuteNonQueryAsync(updateAcc, new[] { new SqlParameter("@AccID", accId.Trim()), new SqlParameter("@UserId", userId) }, trans);
            }
            return true;
        }
    }
}