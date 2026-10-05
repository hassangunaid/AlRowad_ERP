using AlRowad_ERP.Core;
using AlRowad_ERP.Core.Constants;
using AlRowad_ERP.Core.Helpers;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace AlRowad_ERP.Data
{
    public class DriverRepository
    {
        // الحفظ الدستوري المحمي بالـ Transaction
        public async Task<int> SaveDriverAsync(
            int driverId, string driverName, string phone, string license,
            string vehicleType, string notes, int userId, bool isNew, SqlTransaction trans)
        {
            SqlCommand cmd = new SqlCommand { Connection = trans.Connection, Transaction = trans };

            if (isNew)
            {
                cmd.CommandText = $@"
                    INSERT INTO {SystemConstants.Tables.Drivers} 
                    ({SystemConstants.Columns.Driver_Name}, {SystemConstants.Columns.Phone_Number}, 
                     {SystemConstants.Columns.License_Number}, {SystemConstants.Columns.Vehicle_Type}, 
                     {SystemConstants.Columns.Notes}, {SystemConstants.Columns.Created_By}, {SystemConstants.Columns.Created_At}) 
                    VALUES 
                    (@Name, @Phone, @License, @VehicleType, @Notes, @UserId, GETDATE());
                    SELECT SCOPE_IDENTITY();";
            }
            else
            {
                cmd.CommandText = $@"
                    UPDATE {SystemConstants.Tables.Drivers} 
                    SET {SystemConstants.Columns.Driver_Name} = @Name, 
                        {SystemConstants.Columns.Phone_Number} = @Phone, 
                        {SystemConstants.Columns.License_Number} = @License, 
                        {SystemConstants.Columns.Vehicle_Type} = @VehicleType, 
                        {SystemConstants.Columns.Notes} = @Notes, 
                        {SystemConstants.Columns.Updated_By} = @UserId, 
                        {SystemConstants.Columns.Updated_At} = GETDATE()
                    WHERE {SystemConstants.Columns.Driver_ID} = @DriverID AND {SystemConstants.Columns.Is_Deleted} = 0";
                cmd.Parameters.AddWithValue("@DriverID", driverId);
            }

            cmd.Parameters.AddWithValue("@Name", driverName.Trim());
            cmd.Parameters.AddWithValue("@Phone", string.IsNullOrWhiteSpace(phone) ? (object)DBNull.Value : phone.Trim());
            cmd.Parameters.AddWithValue("@License", string.IsNullOrWhiteSpace(license) ? (object)DBNull.Value : license.Trim());
            cmd.Parameters.AddWithValue("@VehicleType", string.IsNullOrWhiteSpace(vehicleType) ? (object)DBNull.Value : vehicleType.Trim());
            cmd.Parameters.AddWithValue("@Notes", string.IsNullOrWhiteSpace(notes) ? (object)DBNull.Value : notes.Trim());
            cmd.Parameters.AddWithValue("@UserId", userId);

            if (isNew)
            {
                return Convert.ToInt32(await cmd.ExecuteScalarAsync());
            }
            else
            {
                await cmd.ExecuteNonQueryAsync();
                return driverId;
            }
        }

        // الحذف المنطقي الدستوري (Soft Delete)
        public async Task ExecuteDeleteAsync(int driverId, int userId, SqlTransaction trans)
        {
            string query = $@"
                UPDATE {SystemConstants.Tables.Drivers} 
                SET {SystemConstants.Columns.Is_Deleted} = 1, 
                    {SystemConstants.Columns.Updated_By} = @UserId, 
                    {SystemConstants.Columns.Updated_At} = GETDATE() 
                WHERE {SystemConstants.Columns.Driver_ID} = @DriverID";

            SqlCommand cmd = new SqlCommand(query, trans.Connection, trans);
            cmd.Parameters.AddWithValue("@DriverID", driverId);
            cmd.Parameters.AddWithValue("@UserId", userId);
            await cmd.ExecuteNonQueryAsync();
        }
        // جلب بيانات السائق (تطبيقاً لفصل المهام)
        public async Task<DataTable> GetDriverAsync(int driverId)
        {
            string query = $@"
                SELECT * FROM {SystemConstants.Tables.Drivers} 
                WHERE {SystemConstants.Columns.Driver_ID} = @DriverID 
                AND Is_Deleted = 0";

            return await DatabaseHelper.GetTableAsync(query, new[] { new SqlParameter("@DriverID", driverId) });
        }
        // جلب استعلام محرك البحث (F9)
        public string GetSearchQuery()
        {
            return $@"
                SELECT {SystemConstants.Columns.Driver_ID} AS [رقم السائق], 
                       {SystemConstants.Columns.Driver_Name} AS [اسم السائق], 
                       {SystemConstants.Columns.Phone_Number} AS [رقم الهاتف] 
                FROM {SystemConstants.Tables.Drivers} 
                WHERE {SystemConstants.Columns.Is_Deleted} = 0";
        }
    }
}