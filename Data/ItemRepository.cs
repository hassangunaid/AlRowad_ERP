using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using AlRowad_ERP.Core;
using AlRowad_ERP.Core.Constants;

namespace AlRowad_ERP.Data
{
    public class ItemRepository
    {
        public async Task<decimal> GetItemBalanceAsync(int itemId, int storeId, SqlTransaction transaction)
        {
            string query = "SELECT ISNULL(Current_Balance, 0) FROM Item_Balances WHERE Item_ID = @ItemId AND Store_ID = @StoreId";
            SqlParameter[] parameters = {
                new SqlParameter("@ItemId", itemId),
                new SqlParameter("@StoreId", storeId)
            };

            object result = await DatabaseHelper.ExecuteScalarAsync(query, parameters, transaction);
            return result != null ? Convert.ToDecimal(result) : 0m;
        }

        public async Task UpdateQuantityAsync(int itemId, int storeId, decimal quantity, string userId, string auditField, SqlTransaction transaction)
        {
            // الدستور: استخدام الحقول الرقابية من الثوابت (SystemConstants)
            string query = $@"
                UPDATE Item_Balances 
                SET Current_Balance = Current_Balance + @Qty,
                    {auditField} = @UserId, 
                    Last_Updated = GETDATE()
                WHERE Item_ID = @ItemId AND Store_ID = @StoreId";

            SqlParameter[] parameters = {
                new SqlParameter("@Qty", quantity),
                new SqlParameter("@UserId", userId),
                new SqlParameter("@ItemId", itemId),
                new SqlParameter("@StoreId", storeId)
            };

            int rowsAffected = await DatabaseHelper.ExecuteNonQueryAsync(query, parameters, transaction);

            // إذا لم يكن الصنف موجوداً في هذا المخزن من قبل، نقوم بإدراجه
            if (rowsAffected == 0)
            {
                string insertQuery = $@"
                    INSERT INTO Item_Balances (Item_ID, Store_ID, Current_Balance, {SystemConstants.AuditFields.CreatedBy}, Created_Date)
                    VALUES (@ItemId, @StoreId, @Qty, @UserId, GETDATE())";
                await DatabaseHelper.ExecuteNonQueryAsync(insertQuery, parameters, transaction);
            }
        }
    }
}