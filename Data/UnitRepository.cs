using AlRowad_ERP.Core;
using AlRowad_ERP.Core.Constants;
using AlRowad_ERP.Core.Helpers;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace AlRowad_ERP.Data
{
    public class UnitRepository
    {
        // 1. جلب كافة الوحدات للجدول (مع أسماء المضيف والمعدل)
        public async Task<DataTable> GetAllActiveUnitsAsync()
        {
            // تم دمج اسم المستخدم مع التاريخ في SQL مباشرة بأداء عالي وتنسيق واضح
            string query = $@"
                SELECT U.{SystemConstants.Columns.Unit_ID}, 
                       U.{SystemConstants.Columns.Unit_Name}, 
                       U.{SystemConstants.Columns.Conversion_Factor},
                       (ISNULL(C.Full_Name, C.Username) + ISNULL(' | ' + FORMAT(U.{SystemConstants.Columns.Created_At}, 'yyyy/MM/dd hh:mm tt'), '')) AS CreatedInfo,
                       (ISNULL(M.Full_Name, M.Username) + ISNULL(' | ' + FORMAT(U.{SystemConstants.Columns.Updated_At}, 'yyyy/MM/dd hh:mm tt'), '')) AS UpdatedInfo
                FROM {SystemConstants.Tables.Units} U
                LEFT JOIN Users C ON U.{SystemConstants.Columns.Created_By} = C.User_ID
                LEFT JOIN Users M ON U.{SystemConstants.Columns.Updated_By} = M.User_ID
                WHERE U.{SystemConstants.Columns.Is_Deleted} = 0";

            return await DatabaseHelper.GetTableAsync(query, null);
        }
        
        // 2. جلب وحدة محددة (للبحث أو عند النقر على الجدول)
        public async Task<DataTable> GetUnitByIdAsync(int unitId)
        {
            // استدعاء U.* يجلب ضمناً تواريخ الإنشاء والتعديل المطلوبة لحقول الرقابة
            string query = $@"
                SELECT U.*, 
                       ISNULL(C.Full_Name, C.Username) AS CreatedByName,
                       ISNULL(M.Full_Name, M.Username) AS UpdatedByName
                FROM {SystemConstants.Tables.Units} U
                LEFT JOIN Users C ON U.{SystemConstants.Columns.Created_By} = C.User_ID
                LEFT JOIN Users M ON U.{SystemConstants.Columns.Updated_By} = M.User_ID
                WHERE U.{SystemConstants.Columns.Unit_ID} = @ID AND U.{SystemConstants.Columns.Is_Deleted} = 0";

            return await DatabaseHelper.GetTableAsync(query, new[] { new SqlParameter("@ID", unitId) });
        }
        // 🌟 تعديل جوهري: الدالة الآن تُرجع رقم السجل (int) بدلاً من bool
        public async Task<int> SaveUnitAsync(int unitId, string unitName, decimal conversionFactor, int userId, bool isNew, SqlTransaction trans)
        {
            string sqlQuery;
            SqlCommand cmd = new SqlCommand { Connection = trans.Connection, Transaction = trans };

            if (isNew)
            {
                sqlQuery = $@"INSERT INTO {SystemConstants.Tables.Units} 
                              ({SystemConstants.Columns.Unit_Name}, {SystemConstants.Columns.Conversion_Factor}, 
                               {SystemConstants.Columns.Created_By}, {SystemConstants.Columns.Created_At}, {SystemConstants.Columns.Is_Deleted}) 
                              VALUES (@Name, @Conv, @UserId, GETDATE(), 0);
                              SELECT SCOPE_IDENTITY();"; // جلب الرقم المولد تلقائياً
            }
            else
            {
                sqlQuery = $@"UPDATE {SystemConstants.Tables.Units} 
                              SET {SystemConstants.Columns.Unit_Name} = @Name, 
                                  {SystemConstants.Columns.Conversion_Factor} = @Conv, 
                                  {SystemConstants.Columns.Updated_By} = @UserId, 
                                  {SystemConstants.Columns.Updated_At} = GETDATE() 
                              WHERE {SystemConstants.Columns.Unit_ID} = @ID";
                cmd.Parameters.AddWithValue("@ID", unitId);
            }

            cmd.CommandText = sqlQuery;
            cmd.Parameters.AddWithValue("@Name", unitName);
            cmd.Parameters.AddWithValue("@Conv", conversionFactor);
            cmd.Parameters.AddWithValue("@UserId", userId);

            if (isNew)
            {
                var insertedId = await cmd.ExecuteScalarAsync();
                unitId = Convert.ToInt32(insertedId); // تعيين الرقم الجديد
            }
            else
            {
                await cmd.ExecuteNonQueryAsync();
            }

            return unitId; // إرجاع الرقم المعماري
        }

        public async Task<bool> DeleteUnitAsync(int unitId, int userId, SqlTransaction trans)
        {
            string query = $@"UPDATE {SystemConstants.Tables.Units} 
                              SET {SystemConstants.Columns.Is_Deleted} = 1, 
                                  {SystemConstants.AuditFields.DeletedBy} = @UserId, 
                                  {SystemConstants.AuditFields.DeletedAt} = GETDATE() 
                              WHERE {SystemConstants.Columns.Unit_ID} = @ID";

            SqlCommand cmd = new SqlCommand(query, trans.Connection, trans);
            cmd.Parameters.AddWithValue("@ID", unitId);
            cmd.Parameters.AddWithValue("@UserId", userId);

            return await cmd.ExecuteNonQueryAsync() > 0;
        }
    }
}