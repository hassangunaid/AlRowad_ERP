using AlRowad_ERP.Core;
using AlRowad_ERP.Core.Constants;
using AlRowad_ERP.Core.Helpers;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace AlRowad_ERP.Data
{
    public class AgencySettingsRepository
    {
        public async Task<DataTable> GetAllAgenciesAsync()
        {
            string query = $@"
                SELECT A.{SystemConstants.Columns.Agency_ID}, 
                       A.{SystemConstants.Columns.Agency_Name}, 
                       A.{SystemConstants.Columns.Farmer_Commission_Percent},
                       A.{SystemConstants.Columns.Buyer_Fee_Per_Package},
                       (ISNULL(C.Full_Name, C.Username) + ISNULL(' | ' + FORMAT(A.{SystemConstants.Columns.Created_At}, 'yyyy/MM/dd hh:mm tt'), '')) AS CreatedInfo,
                       (ISNULL(M.Full_Name, M.Username) + ISNULL(' | ' + FORMAT(A.{SystemConstants.Columns.Updated_At}, 'yyyy/MM/dd hh:mm tt'), '')) AS UpdatedInfo
                FROM {SystemConstants.Tables.Agency_Settings} A
                LEFT JOIN Users C ON A.{SystemConstants.Columns.Created_By} = C.User_ID
                LEFT JOIN Users M ON A.{SystemConstants.Columns.Updated_By} = M.User_ID";
            return await DatabaseHelper.GetTableAsync(query, null);
        }

        public async Task<DataTable> GetAgencyByIdAsync(int agencyId)
        {
            string query = $@"
                SELECT A.*, 
                       Acc1.{SystemConstants.Columns.Acc_Name} AS FarmerAccName,
                       Acc2.{SystemConstants.Columns.Acc_Name} AS BuyerAccName,
                       Acc3.{SystemConstants.Columns.Acc_Name} AS OfficeAccName,
                       ISNULL(C.Full_Name, C.Username) AS CreatedByName,
                       ISNULL(M.Full_Name, M.Username) AS UpdatedByName
                FROM {SystemConstants.Tables.Agency_Settings} A
                LEFT JOIN {SystemConstants.Tables.Accounts} Acc1 ON A.{SystemConstants.Columns.Acc_Farmer_Commission} = Acc1.{SystemConstants.Columns.Acc_ID}
                LEFT JOIN {SystemConstants.Tables.Accounts} Acc2 ON A.{SystemConstants.Columns.Acc_Buyer_Fee} = Acc2.{SystemConstants.Columns.Acc_ID}
                LEFT JOIN {SystemConstants.Tables.Accounts} Acc3 ON A.{SystemConstants.Columns.Acc_Additional_Discount} = Acc3.{SystemConstants.Columns.Acc_ID}
                LEFT JOIN Users C ON A.{SystemConstants.Columns.Created_By} = C.User_ID
                LEFT JOIN Users M ON A.{SystemConstants.Columns.Updated_By} = M.User_ID
                WHERE A.{SystemConstants.Columns.Agency_ID} = @ID";

            return await DatabaseHelper.GetTableAsync(query, new[] { new SqlParameter("@ID", agencyId) });
        }

        public async Task<bool> CheckIfAgencyHasMovementsAsync(int agencyId)
        {
            string query = $@"
                SELECT COUNT(1) 
                FROM Auction_Invoices 
                WHERE {SystemConstants.Columns.Agency_ID} = @ID AND {SystemConstants.Columns.Is_Deleted} = 0";
            try
            {
                object result = await DatabaseHelper.ExecuteScalarAsync(query, new[] { new SqlParameter("@ID", agencyId) });
                return Convert.ToInt32(result) > 0;
            }
            catch { return false; }
        }

        public async Task<int> SaveAgencySettingsAsync(int agencyId, string agencyName, string engAgencyName,
            string phone1, string phone2, string address, string engAddress, string notes, string engNotes,
            decimal farmerComm, decimal buyerFee, decimal officeFee, bool allowOverride,
            string accFarmer, string accBuyer, string accOffice,
            int userId, bool isNew, SqlTransaction trans)
        {
            string sqlQuery;
            SqlCommand cmd = new SqlCommand { Connection = trans.Connection, Transaction = trans };

            if (isNew)
            {
                sqlQuery = $@"INSERT INTO {SystemConstants.Tables.Agency_Settings} 
                              ({SystemConstants.Columns.Agency_Name}, {SystemConstants.Columns.Eng_Agency_Name},
                               {SystemConstants.Columns.Agency_Phone1}, {SystemConstants.Columns.Agency_Phone2}, 
                               {SystemConstants.Columns.Agency_Address}, {SystemConstants.Columns.Eng_Agency_Address},
                               {SystemConstants.Columns.Agency_Notes}, {SystemConstants.Columns.Eng_Agency_Notes},
                               {SystemConstants.Columns.Farmer_Commission_Percent}, {SystemConstants.Columns.Buyer_Fee_Per_Package}, {SystemConstants.Columns.Office_Service_Fee}, {SystemConstants.Columns.Allow_Override_In_Invoice},
                               {SystemConstants.Columns.Acc_Farmer_Commission}, {SystemConstants.Columns.Acc_Buyer_Fee}, {SystemConstants.Columns.Acc_Additional_Discount},
                               {SystemConstants.Columns.Created_By}, {SystemConstants.Columns.Created_At}) 
                              VALUES (@Name, @EngName, @Phone1, @Phone2, @Address, @EngAddress, @Notes, @EngNotes,
                                      @FarmerComm, @BuyerFee, @OfficeFee, @AllowOverride, 
                                      @AccFarmer, @AccBuyer, @AccOffice, 
                                      @UserId, GETDATE());
                              SELECT SCOPE_IDENTITY();";
            }
            else
            {
                sqlQuery = $@"UPDATE {SystemConstants.Tables.Agency_Settings} 
                              SET {SystemConstants.Columns.Agency_Name} = @Name, 
                                  {SystemConstants.Columns.Eng_Agency_Name} = @EngName,
                                  {SystemConstants.Columns.Agency_Phone1} = @Phone1, 
                                  {SystemConstants.Columns.Agency_Phone2} = @Phone2, 
                                  {SystemConstants.Columns.Agency_Address} = @Address, 
                                  {SystemConstants.Columns.Eng_Agency_Address} = @EngAddress,
                                  {SystemConstants.Columns.Agency_Notes} = @Notes, 
                                  {SystemConstants.Columns.Eng_Agency_Notes} = @EngNotes,
                                  {SystemConstants.Columns.Farmer_Commission_Percent} = @FarmerComm, 
                                  {SystemConstants.Columns.Buyer_Fee_Per_Package} = @BuyerFee, 
                                  {SystemConstants.Columns.Office_Service_Fee} = @OfficeFee,
                                  {SystemConstants.Columns.Allow_Override_In_Invoice} = @AllowOverride,
                                  {SystemConstants.Columns.Acc_Farmer_Commission} = @AccFarmer,
                                  {SystemConstants.Columns.Acc_Buyer_Fee} = @AccBuyer,
                                  {SystemConstants.Columns.Acc_Additional_Discount} = @AccOffice,
                                  {SystemConstants.Columns.Updated_By} = @UserId, 
                                  {SystemConstants.Columns.Updated_At} = GETDATE() 
                              WHERE {SystemConstants.Columns.Agency_ID} = @ID";
                cmd.Parameters.AddWithValue("@ID", agencyId);
            }

            cmd.CommandText = sqlQuery;
            cmd.Parameters.AddWithValue("@Name", agencyName);
            cmd.Parameters.AddWithValue("@EngName", string.IsNullOrWhiteSpace(engAgencyName) ? (object)DBNull.Value : engAgencyName);
            cmd.Parameters.AddWithValue("@Phone1", string.IsNullOrWhiteSpace(phone1) ? (object)DBNull.Value : phone1);
            cmd.Parameters.AddWithValue("@Phone2", string.IsNullOrWhiteSpace(phone2) ? (object)DBNull.Value : phone2);
            cmd.Parameters.AddWithValue("@Address", string.IsNullOrWhiteSpace(address) ? (object)DBNull.Value : address);
            cmd.Parameters.AddWithValue("@EngAddress", string.IsNullOrWhiteSpace(engAddress) ? (object)DBNull.Value : engAddress);
            cmd.Parameters.AddWithValue("@Notes", string.IsNullOrWhiteSpace(notes) ? (object)DBNull.Value : notes);
            cmd.Parameters.AddWithValue("@EngNotes", string.IsNullOrWhiteSpace(engNotes) ? (object)DBNull.Value : engNotes);

            cmd.Parameters.AddWithValue("@FarmerComm", farmerComm);
            cmd.Parameters.AddWithValue("@BuyerFee", buyerFee);
            cmd.Parameters.AddWithValue("@OfficeFee", officeFee);
            cmd.Parameters.AddWithValue("@AllowOverride", allowOverride);

            cmd.Parameters.AddWithValue("@AccFarmer", string.IsNullOrWhiteSpace(accFarmer) ? (object)DBNull.Value : accFarmer);
            cmd.Parameters.AddWithValue("@AccBuyer", string.IsNullOrWhiteSpace(accBuyer) ? (object)DBNull.Value : accBuyer);
            cmd.Parameters.AddWithValue("@AccOffice", string.IsNullOrWhiteSpace(accOffice) ? (object)DBNull.Value : accOffice);

            cmd.Parameters.AddWithValue("@UserId", userId);

            if (isNew) return Convert.ToInt32(await cmd.ExecuteScalarAsync());

            await cmd.ExecuteNonQueryAsync();
            return agencyId;
        }
        // دالة لجلب الرقم القادم للوكالة (الترقيم التلقائي)
        public async Task<int> GetNextAgencyIdAsync()
        {
            string query = $"SELECT ISNULL(MAX({SystemConstants.Columns.Agency_ID}), 0) + 1 FROM {SystemConstants.Tables.Agency_Settings}";
            try
            {
                object result = await DatabaseHelper.ExecuteScalarAsync(query, null);
                return Convert.ToInt32(result);
            }
            catch
            {
                return 1; // في حال كان الجدول فارغاً تماماً
            }
        }
    }
}