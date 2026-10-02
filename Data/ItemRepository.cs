using AlRowad_ERP.Core;
using AlRowad_ERP.Core.Constants;
using AlRowad_ERP.Core.Helpers;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace AlRowad_ERP.Data
{
    public class ItemRepository
    {
        // =========================================================================
        // القسم الأول: عمليات شاشة بطاقة الصنف (CRUD Operations)
        // =========================================================================

        public async Task<DataTable> GetAllActiveItemsAsync()
        {
            string query = $@"
                SELECT I.{SystemConstants.Columns.Item_ID}, 
                       I.{SystemConstants.Columns.Item_Name}, 
                       U.{SystemConstants.Columns.Unit_Name} AS Base_Unit_Name,
                       I.{SystemConstants.Columns.Default_Price},
                       (ISNULL(C.Full_Name, C.Username) + ISNULL(' | ' + FORMAT(I.{SystemConstants.Columns.Created_At}, 'yyyy/MM/dd hh:mm tt'), '')) AS CreatedInfo,
                       (ISNULL(M.Full_Name, M.Username) + ISNULL(' | ' + FORMAT(I.{SystemConstants.Columns.Updated_At}, 'yyyy/MM/dd hh:mm tt'), '')) AS UpdatedInfo
                FROM {SystemConstants.Tables.Items} I
                LEFT JOIN {SystemConstants.Tables.Units} U ON I.{SystemConstants.Columns.Base_Unit_ID} = U.{SystemConstants.Columns.Unit_ID}
                LEFT JOIN Users C ON I.{SystemConstants.Columns.Created_By} = C.User_ID
                LEFT JOIN Users M ON I.{SystemConstants.Columns.Updated_By} = M.User_ID
                WHERE I.{SystemConstants.Columns.Is_Deleted} = 0";

            return await DatabaseHelper.GetTableAsync(query, null);
        }

        public async Task<DataTable> GetItemByIdAsync(int itemId)
        {
            string query = $@"
                SELECT I.*, 
                       ISNULL(C.Full_Name, C.Username) AS CreatedByName,
                       ISNULL(M.Full_Name, M.Username) AS UpdatedByName
                FROM {SystemConstants.Tables.Items} I
                LEFT JOIN Users C ON I.{SystemConstants.Columns.Created_By} = C.User_ID
                LEFT JOIN Users M ON I.{SystemConstants.Columns.Updated_By} = M.User_ID
                WHERE I.{SystemConstants.Columns.Item_ID} = @ID AND I.{SystemConstants.Columns.Is_Deleted} = 0";

            return await DatabaseHelper.GetTableAsync(query, new[] { new SqlParameter("@ID", itemId) });
        }

        public async Task<DataTable> GetUnitsForDropdownAsync()
        {
            string query = $"SELECT {SystemConstants.Columns.Unit_ID}, {SystemConstants.Columns.Unit_Name} FROM {SystemConstants.Tables.Units} WHERE {SystemConstants.Columns.Is_Deleted} = 0";
            return await DatabaseHelper.GetTableAsync(query, null);
        }

        public async Task<int> SaveItemAsync(int itemId, string itemName, int baseUnitId, decimal defaultPrice, int userId, bool isNew, SqlTransaction trans)
        {
            string sqlQuery;
            SqlCommand cmd = new SqlCommand { Connection = trans.Connection, Transaction = trans };

            if (isNew)
            {
                sqlQuery = $@"INSERT INTO {SystemConstants.Tables.Items} 
                              ({SystemConstants.Columns.Item_Name}, {SystemConstants.Columns.Base_Unit_ID}, {SystemConstants.Columns.Default_Price}, 
                               {SystemConstants.Columns.Created_By}, {SystemConstants.Columns.Created_At}, {SystemConstants.Columns.Is_Deleted}) 
                              VALUES (@Name, @UnitID, @Price, @UserId, GETDATE(), 0);
                              SELECT SCOPE_IDENTITY();";
            }
            else
            {
                sqlQuery = $@"UPDATE {SystemConstants.Tables.Items} 
                              SET {SystemConstants.Columns.Item_Name} = @Name, 
                                  {SystemConstants.Columns.Base_Unit_ID} = @UnitID, 
                                  {SystemConstants.Columns.Default_Price} = @Price, 
                                  {SystemConstants.Columns.Updated_By} = @UserId, 
                                  {SystemConstants.Columns.Updated_At} = GETDATE() 
                              WHERE {SystemConstants.Columns.Item_ID} = @ID";
                cmd.Parameters.AddWithValue("@ID", itemId);
            }

            cmd.CommandText = sqlQuery;
            cmd.Parameters.AddWithValue("@Name", itemName);
            cmd.Parameters.AddWithValue("@UnitID", baseUnitId);
            cmd.Parameters.AddWithValue("@Price", defaultPrice);
            cmd.Parameters.AddWithValue("@UserId", userId);

            if (isNew)
            {
                var insertedId = await cmd.ExecuteScalarAsync();
                itemId = Convert.ToInt32(insertedId);
            }
            else
            {
                await cmd.ExecuteNonQueryAsync();
            }

            return itemId;
        }

        public async Task<bool> DeleteItemAsync(int itemId, int userId, SqlTransaction trans)
        {
            string query = $@"UPDATE {SystemConstants.Tables.Items} 
                              SET {SystemConstants.Columns.Is_Deleted} = 1, 
                                  {SystemConstants.AuditFields.DeletedBy} = @UserId, 
                                  {SystemConstants.AuditFields.DeletedAt} = GETDATE() 
                              WHERE {SystemConstants.Columns.Item_ID} = @ID";

            SqlCommand cmd = new SqlCommand(query, trans.Connection, trans);
            cmd.Parameters.AddWithValue("@ID", itemId);
            cmd.Parameters.AddWithValue("@UserId", userId);

            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        // =========================================================================
        // القسم الثاني: المعالجات المخزنية اللحظية (تحديث الأرصدة)
        // =========================================================================

        public async Task<decimal> GetItemBalanceAsync(int itemId, int storeId, SqlTransaction transaction = null)
        {
            string query = $@"SELECT ISNULL({SystemConstants.Columns.Quantity}, 0) 
                              FROM {SystemConstants.Tables.Item_Balances} 
                              WHERE {SystemConstants.Columns.Item_ID} = @ItemId AND {SystemConstants.Columns.Store_ID} = @StoreId";

            SqlParameter[] parameters = {
                new SqlParameter("@ItemId", itemId),
                new SqlParameter("@StoreId", storeId)
            };

            object result;

            // تنفيذ آمن مع أو بدون Transaction
            if (transaction != null)
            {
                result = await DatabaseHelper.ExecuteScalarAsync(query, parameters, transaction);
            }
            else
            {
                result = await DatabaseHelper.ExecuteScalarAsync(query, parameters);
            }

            return (result != null && result != DBNull.Value) ? Convert.ToDecimal(result) : 0m;
        }

        // =========================================================================
        // القسم الثاني: المعالجات المخزنية اللحظية (تحديث الأرصدة)
        // (تم تطبيق مبدأ التوافق الرجعي Adapter Pattern لخدمة الشاشات القديمة)
        // =========================================================================

        public async Task UpdateQuantityAsync(int itemId, int storeId, decimal quantity, string userId, string auditField, SqlTransaction transaction)
        {
            // 1. التوفيق (Reconciliation): تحويل رقم المستخدم النصي القادم من الشاشات القديمة إلى رقم صحيح لقاعدة البيانات
            int parsedUserId = 0;
            int.TryParse(userId, out parsedUserId);

            // 2. التوفيق الأمني: التأكد من أن الحقل الممرر ليس حقنة خبيثة (SQL Injection) بل حقل رقابي معتمد
            string safeAuditField = SystemConstants.Columns.Updated_By; // الوضع الافتراضي
            if (auditField == "Created_By" || auditField == SystemConstants.Columns.Created_By)
            {
                safeAuditField = SystemConstants.Columns.Created_By;
            }

            // 3. بناء الاستعلام المعماري (لا توجد ثوابت نصية عشوائية للجداول)
            string query = $@"
                UPDATE {SystemConstants.Tables.Item_Balances} 
                SET {SystemConstants.Columns.Quantity} = ISNULL({SystemConstants.Columns.Quantity}, 0) + @Qty,
                    {safeAuditField} = @UserId, 
                    {SystemConstants.Columns.Updated_At} = GETDATE()
                WHERE {SystemConstants.Columns.Item_ID} = @ItemId AND {SystemConstants.Columns.Store_ID} = @StoreId";

            SqlParameter[] parameters = {
                new SqlParameter("@Qty", quantity),
                new SqlParameter("@UserId", parsedUserId), // تمرير الرقم الصحيح بعد التحويل
                new SqlParameter("@ItemId", itemId),
                new SqlParameter("@StoreId", storeId)
            };

            int rowsAffected = await DatabaseHelper.ExecuteNonQueryAsync(query, parameters, transaction);

            // 4. الإدراج التلقائي في حال لم يكن الصنف موجوداً في المخزن
            if (rowsAffected == 0)
            {
                string insertQuery = $@"
                    INSERT INTO {SystemConstants.Tables.Item_Balances} 
                    ({SystemConstants.Columns.Item_ID}, {SystemConstants.Columns.Store_ID}, {SystemConstants.Columns.Quantity}, {SystemConstants.Columns.Created_By}, {SystemConstants.Columns.Created_At})
                    VALUES (@ItemId, @StoreId, @Qty, @UserId, GETDATE())";

                await DatabaseHelper.ExecuteNonQueryAsync(insertQuery, parameters, transaction);
            }
        }
    }
}