using AlRowad_ERP.Core;
using AlRowad_ERP.Core.Constants;
using AlRowad_ERP.Core.Helpers;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows.Forms; // تمت الإضافة للسماح بإظهار رسالة التنبيه البسيطة

namespace AlRowad_ERP.Data
{
    public class ShipmentReceiptRepository
    {
        public async Task<string> GenerateNextShipmentCodeAsync()
        {
            // استعلام SQL يعتمد على الدالة TRY_CAST لتجنب أخطاء التحويل 
            // وجلب أعلى رقم تسلسلي، ثم إضافة 1. إذا كان الجدول فارغاً سيبدأ من 1.
            string query = @"
        SELECT ISNULL(MAX(TRY_CAST(Shipment_Code AS INT)), 0) + 1 
        FROM Shipment_Receipt_Headers 
        WHERE TRY_CAST(Shipment_Code AS INT) IS NOT NULL";

            try
            {
                // استخدام DatabaseHelper حسب دستور النظام المعماري
                object result = await DatabaseHelper.ExecuteScalarAsync(query, null);

                if (result != null && result != DBNull.Value)
                {
                    return result.ToString();
                }
                return "1";
            }
            catch (Exception ex)
            {
                // 1. تسجيل الخطأ بصمت في قاعدة البيانات عبر المسجل المركزي (للمطورين)
                GlobalExceptionHandler.LogError(ex, "ShipmentReceiptRepository.GenerateNextShipmentCode");

                // 2. إظهار رسالة تنبيه بسيطة لا تعيق عمل المستخدم (كما طلبت)
                MessageBox.Show("حدث خطأ طفيف أثناء جلب التسلسل التلقائي للمستند. سيتم استخدام رقم افتراضي مؤقتاً.", "تنبيه نظام الرواد", MessageBoxButtons.OK, MessageBoxIcon.Information);

                return "1"; // رقم افتراضي في حالة الفشل لضمان استمرار العمل
            }
        }

        public async Task<int> SaveShipmentReceiptAsync(
            int shipmentId, string shipmentCode, DateTime receiptDate,
            string driverName, string vehicleNumber, string driverPhone, string notes,
            decimal totalEstimated, DataTable dtDetails, int userId, bool isNew, SqlTransaction trans)
        {
            SqlCommand cmd = new SqlCommand { Connection = trans.Connection, Transaction = trans };
            int currentShipmentId = shipmentId;

            // حفظ الرأس (Master)
            if (isNew)
            {
                cmd.CommandText = $@"
                    INSERT INTO {SystemConstants.Tables.Shipment_Receipt_Headers} 
                    ({SystemConstants.Columns.Shipment_Code}, {SystemConstants.Columns.Receipt_Date}, {SystemConstants.Columns.Driver_Name}, 
                     {SystemConstants.Columns.Vehicle_Number}, {SystemConstants.Columns.Driver_Phone}, Status_Code, 
                     {SystemConstants.Columns.Total_Estimated_Value}, {SystemConstants.Columns.Notes}, {SystemConstants.Columns.Created_By}, {SystemConstants.Columns.Created_At}) 
                    VALUES 
                    (@Code, @Date, @Driver, @Vehicle, @Phone, 'ESTIMATED', @TotalEst, @Notes, @UserId, GETDATE());
                    SELECT SCOPE_IDENTITY();";
            }
            else
            {
                cmd.CommandText = $@"
                    UPDATE {SystemConstants.Tables.Shipment_Receipt_Headers} 
                    SET {SystemConstants.Columns.Receipt_Date} = @Date, 
                        {SystemConstants.Columns.Driver_Name} = @Driver, 
                        {SystemConstants.Columns.Vehicle_Number} = @Vehicle, 
                        {SystemConstants.Columns.Driver_Phone} = @Phone, 
                        {SystemConstants.Columns.Total_Estimated_Value} = @TotalEst, 
                        {SystemConstants.Columns.Notes} = @Notes, 
                        {SystemConstants.Columns.Updated_By} = @UserId, 
                        {SystemConstants.Columns.Updated_At} = GETDATE()
                    WHERE {SystemConstants.Columns.Shipment_ID} = @ShipmentID";
                cmd.Parameters.AddWithValue("@ShipmentID", shipmentId);
            }

            cmd.Parameters.AddWithValue("@Code", shipmentCode);
            cmd.Parameters.AddWithValue("@Date", receiptDate);
            cmd.Parameters.AddWithValue("@Driver", string.IsNullOrWhiteSpace(driverName) ? (object)DBNull.Value : driverName);
            cmd.Parameters.AddWithValue("@Vehicle", string.IsNullOrWhiteSpace(vehicleNumber) ? (object)DBNull.Value : vehicleNumber);
            cmd.Parameters.AddWithValue("@Phone", string.IsNullOrWhiteSpace(driverPhone) ? (object)DBNull.Value : driverPhone);
            cmd.Parameters.AddWithValue("@TotalEst", totalEstimated);
            cmd.Parameters.AddWithValue("@Notes", string.IsNullOrWhiteSpace(notes) ? (object)DBNull.Value : notes);
            cmd.Parameters.AddWithValue("@UserId", userId);

            if (isNew)
            {
                currentShipmentId = Convert.ToInt32(await cmd.ExecuteScalarAsync());
            }
            else
            {
                await cmd.ExecuteNonQueryAsync();
                cmd.CommandText = $"DELETE FROM {SystemConstants.Tables.Shipment_Receipt_Details} WHERE {SystemConstants.Columns.Shipment_ID} = @ShipmentID";
                await cmd.ExecuteNonQueryAsync();
            }

            // حفظ التفاصيل (Details) مع تضمين الخصم الكمي
            if (dtDetails != null && dtDetails.Rows.Count > 0)
            {
                cmd.CommandText = $@"
                    INSERT INTO {SystemConstants.Tables.Shipment_Receipt_Details} 
                    ({SystemConstants.Columns.Shipment_ID}, {SystemConstants.Columns.Farmer_ID}, {SystemConstants.Columns.Item_ID}, 
                     {SystemConstants.Columns.Unit_ID}, {SystemConstants.Columns.Quantity}, 
                     {SystemConstants.Columns.Estimated_Discount}, {SystemConstants.Columns.Estimated_Price}, {SystemConstants.Columns.Notes})
                    VALUES 
                    (@H_ShipmentID, @D_FarmerID, @D_ItemID, @D_UnitID, @D_Qty, @D_EstDisc, @D_EstPrice, @D_Notes)";

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@H_ShipmentID", currentShipmentId);
                cmd.Parameters.Add("@D_FarmerID", SqlDbType.NVarChar);
                cmd.Parameters.Add("@D_ItemID", SqlDbType.Int);
                cmd.Parameters.Add("@D_UnitID", SqlDbType.Int);
                cmd.Parameters.Add("@D_Qty", SqlDbType.Decimal);
                cmd.Parameters.Add("@D_EstDisc", SqlDbType.Decimal);
                cmd.Parameters.Add("@D_EstPrice", SqlDbType.Decimal);
                cmd.Parameters.Add("@D_Notes", SqlDbType.NVarChar);

                foreach (DataRow row in dtDetails.Rows)
                {
                    cmd.Parameters["@D_FarmerID"].Value = row[SystemConstants.Columns.Farmer_ID].ToString();
                    cmd.Parameters["@D_ItemID"].Value = Convert.ToInt32(row[SystemConstants.Columns.Item_ID]);
                    cmd.Parameters["@D_UnitID"].Value = Convert.ToInt32(row[SystemConstants.Columns.Unit_ID]);
                    cmd.Parameters["@D_Qty"].Value = Convert.ToDecimal(row[SystemConstants.Columns.Quantity]);
                    cmd.Parameters["@D_EstDisc"].Value = Convert.ToDecimal(row[SystemConstants.Columns.Estimated_Discount]);
                    cmd.Parameters["@D_EstPrice"].Value = Convert.ToDecimal(row[SystemConstants.Columns.Estimated_Price]);
                    cmd.Parameters["@D_Notes"].Value = row[SystemConstants.Columns.Notes].ToString();

                    await cmd.ExecuteNonQueryAsync();
                }
            }
            return currentShipmentId;
        }
        public DataTable GetUnitsDataTable()
        {
            string query = "SELECT Unit_ID, Unit_Name FROM Units WHERE Is_Deleted = 0";
            return DatabaseHelper.ExecuteQuery(query, null);
        }
        public async Task<DataTable> GetUnitsDataTableAsync()
        {
            string query = "SELECT Unit_ID, Unit_Name FROM Units WHERE Is_Deleted = 0";
            return await DatabaseHelper.GetTableAsync(query);
        }
    }
}