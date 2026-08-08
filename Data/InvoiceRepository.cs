using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using AlRowad_ERP.Core;
using AlRowad_ERP.Models;
using AlRowad_ERP.Core.Constants;

namespace AlRowad_ERP.Data
{
    public class InvoiceRepository
    {
        public async Task<int> InsertInvoiceAsync(InvoiceModel invoice, string userId, SqlTransaction transaction)
        {
            string query = $@"
                INSERT INTO Invoices (Invoice_Date, Customer_ID, Store_ID, Net_Total, {SystemConstants.AuditFields.CreatedBy}, Created_Date) 
                OUTPUT INSERTED.Invoice_ID 
                VALUES (@Date, @CustId, @StoreId, @NetTotal, @UserId, GETDATE())";

            SqlParameter[] parameters = {
                new SqlParameter("@Date", invoice.InvoiceDate),
                new SqlParameter("@CustId", invoice.CustomerId),
                new SqlParameter("@StoreId", invoice.StoreId),
                new SqlParameter("@NetTotal", invoice.NetTotal),
                new SqlParameter("@UserId", userId)
            };

            object result = await DatabaseHelper.ExecuteScalarAsync(query, parameters, transaction);
            return Convert.ToInt32(result);
        }

        public async Task InsertInvoiceDetailAsync(InvoiceDetailModel detail, SqlTransaction transaction)
        {
            string query = @"
                INSERT INTO Invoice_Details (Invoice_ID, Item_ID, Quantity, Price) 
                VALUES (@InvId, @ItemId, @Qty, @Price)";

            SqlParameter[] parameters = {
                new SqlParameter("@InvId", detail.InvoiceId),
                new SqlParameter("@ItemId", detail.ItemId),
                new SqlParameter("@Qty", detail.Quantity),
                new SqlParameter("@Price", detail.Price)
            };

            await DatabaseHelper.ExecuteNonQueryAsync(query, parameters, transaction);
        }
    }
}