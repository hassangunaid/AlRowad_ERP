using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks; // 🌟 دعم التزامن
using AlRowad_ERP.Core;
using AlRowad_ERP.Models;
using AlRowad_ERP.UI.Base;
using AlRowad_ERP.Core.Constants;



namespace AlRowad_ERP.Data { 

    public static class VoucherRepository
    {
        // 🌟 توليد رقم السند بشكل لا متزامن
        public static async Task<string> GetNewVoucherNoAsync(int docTypeId)
        {
            string query = $"SELECT ISNULL(MAX(CAST(Voucher_No AS INT)), 0) + 1 FROM Cash_Vouchers WHERE ISNUMERIC(Voucher_No) = 1 AND Doc_Type_ID = {docTypeId}";
            object res = await DatabaseHelper.ExecuteScalarAsync(query);
            return res != null ? res.ToString().PadLeft(4, '0') : "0001";
        }

        public static string GetVouchersSearchQuery(int docTypeId)
        {
            return $"SELECT Voucher_ID, Voucher_No AS [رقم السند], Voucher_Date AS [التاريخ], Amount AS [المبلغ] FROM Cash_Vouchers WHERE Doc_Type_ID = {docTypeId}";
        }

        // 🌟 الحفظ اللامتزامن المدعوم بالـ Transactions والثوابت
        public static async Task<int> SaveVoucherAsync(CashVoucherHeader voucher, FormMode mode, SqlTransaction trans)
        {
            int savedVoucherId = voucher.VoucherID;

            if (mode == FormMode.Edit)
            {
                string sqlUpdate = $@"UPDATE Cash_Vouchers 
                                     SET Voucher_Date = @Date, Box_Acc_ID = @BoxAcc, Amount = @Amount, 
                                         Amount_Foreign = @AmountForeign, Cur_ID = @CurID, 
                                         Exchange_Rate = @ExchRate, Notes = @Notes, Is_Posted = @IsPosted,
                                         {SystemConstants.AuditFields.UpdatedBy} = @UpdatedBy, {SystemConstants.AuditFields.UpdatedAt} = GETDATE()
                                     WHERE Voucher_ID = @VoucherID AND RowVersion = @OldRowVersion";

                SqlParameter[] pUpd = {
                    new SqlParameter("@VoucherID", voucher.VoucherID),
                    new SqlParameter("@Date", voucher.VoucherDate),
                    new SqlParameter("@BoxAcc", voucher.BoxAccID),
                    new SqlParameter("@Amount", voucher.Amount),
                    new SqlParameter("@AmountForeign", voucher.AmountForeign > 0 ? (object)voucher.AmountForeign : DBNull.Value),
                    new SqlParameter("@CurID", voucher.CurID),
                    new SqlParameter("@ExchRate", voucher.ExchangeRate),
                    new SqlParameter("@Notes", string.IsNullOrWhiteSpace(voucher.Notes) ? (object)DBNull.Value : voucher.Notes),
                    new SqlParameter("@IsPosted", voucher.IsPosted),
                    new SqlParameter("@UpdatedBy", voucher.UpdatedBy > 0 ? (object)voucher.UpdatedBy : DBNull.Value),
                    new SqlParameter("@OldRowVersion", SqlDbType.Timestamp) { Value = voucher.RowVersion ?? (object)DBNull.Value }
                };

                int rowsAffected = await DatabaseHelper.ExecuteNonQueryAsync(sqlUpdate, pUpd, trans);

                if (rowsAffected == 0)
                {
                    throw new InvalidOperationException("تضارب مالي خطير: تم تعديل هذا السند من قبل محاسب آخر أثناء استعراضك له. تم إيقاف عملية الحفظ لحماية القيود المحاسبية. يرجى التراجع وتحديث الشاشة.");
                }

                await DatabaseHelper.ExecuteNonQueryAsync("DELETE FROM Cash_Voucher_Details WHERE Voucher_ID = @VoucherID", new[] { new SqlParameter("@VoucherID", voucher.VoucherID) }, trans);
                await DatabaseHelper.ExecuteNonQueryAsync("DELETE FROM Journal_Details WHERE Journal_ID IN (SELECT Journal_ID FROM Journal_Entries WHERE Reference_No = @No AND Entry_Type = @Type)", new[] { new SqlParameter("@No", voucher.VoucherNo), new SqlParameter("@Type", voucher.DocTypeID) }, trans);
                await DatabaseHelper.ExecuteNonQueryAsync("DELETE FROM Journal_Entries WHERE Reference_No = @No AND Entry_Type = @Type", new[] { new SqlParameter("@No", voucher.VoucherNo), new SqlParameter("@Type", voucher.DocTypeID) }, trans);
            }
            else if (mode == FormMode.New)
            {
                string sqlInsert = $@"INSERT INTO Cash_Vouchers 
                                     (Voucher_No, Voucher_Date, Doc_Type_ID, Box_Acc_ID, Amount, Amount_Foreign, Cur_ID, Exchange_Rate, Notes, Is_Posted, {SystemConstants.CreatedBy}, {SystemConstants.CreatedAt}) 
                                     VALUES 
                                     (@No, @Date, @DocType, @BoxAcc, @Amount, @AmountForeign, @CurID, @ExchRate, @Notes, @IsPosted, @CreatedBy, GETDATE()); 
                                     SELECT SCOPE_IDENTITY();";

                SqlParameter[] pIns = {
                    new SqlParameter("@No", voucher.VoucherNo),
                    new SqlParameter("@Date", voucher.VoucherDate),
                    new SqlParameter("@DocType", voucher.DocTypeID),
                    new SqlParameter("@BoxAcc", voucher.BoxAccID),
                    new SqlParameter("@Amount", voucher.Amount),
                    new SqlParameter("@AmountForeign", voucher.AmountForeign > 0 ? (object)voucher.AmountForeign : DBNull.Value),
                    new SqlParameter("@CurID", voucher.CurID),
                    new SqlParameter("@ExchRate", voucher.ExchangeRate),
                    new SqlParameter("@Notes", string.IsNullOrWhiteSpace(voucher.Notes) ? (object)DBNull.Value : voucher.Notes),
                    new SqlParameter("@IsPosted", voucher.IsPosted),
                    new SqlParameter("@CreatedBy", voucher.CreatedBy > 0 ? (object)voucher.CreatedBy : DBNull.Value)
                };

                object res = await DatabaseHelper.ExecuteScalarAsync(sqlInsert, pIns, trans);
                savedVoucherId = Convert.ToInt32(res);
            }

            string sqlDetail = @"INSERT INTO Cash_Voucher_Details 
                                 (Voucher_ID, Acc_ID, Amount_Credit, Amount_Debit, Currency_ID, Exchange_Rate, Amount_Foreign, Notes) 
                                 VALUES 
                                 (@VoucherID, @AccID, @Credit, @Debit, @CurrencyID, @ExchangeRate, @AmountForeign, @Notes)";

            foreach (var det in voucher.Details)
            {
                if (det.AmountCredit <= 0 && det.AmountDebit <= 0)
                {
                    throw new Exception($"خطأ محاسبي: لا يمكن تمرير الحساب [{det.AccID}] بقيمة صفرية إلى قاعدة البيانات.");
                }

                SqlParameter[] pDet = {
                    new SqlParameter("@VoucherID", savedVoucherId),
                    new SqlParameter("@AccID", det.AccID),
                    new SqlParameter("@Credit", det.AmountCredit),
                    new SqlParameter("@Debit", det.AmountDebit),
                    new SqlParameter("@CurrencyID", det.CurrencyID),
                    new SqlParameter("@ExchangeRate", det.ExchangeRate),
                    new SqlParameter("@AmountForeign", det.AmountForeign > 0 ? (object)det.AmountForeign : DBNull.Value),
                    new SqlParameter("@Notes", string.IsNullOrWhiteSpace(det.Notes) ? (object)DBNull.Value : det.Notes)
                };

                await DatabaseHelper.ExecuteNonQueryAsync(sqlDetail, pDet, trans);
            }

            return savedVoucherId;
        }

        // 🌟 الحذف اللامتزامن
        public static async Task DeleteVoucherAsync(int voucherId, string voucherNo, int docTypeId, SqlTransaction trans)
        {
            await DatabaseHelper.ExecuteNonQueryAsync("DELETE FROM Journal_Details WHERE Journal_ID IN (SELECT Journal_ID FROM Journal_Entries WHERE Reference_No = @No AND Entry_Type = @Type)", new[] { new SqlParameter("@No", voucherNo), new SqlParameter("@Type", docTypeId) }, trans);
            await DatabaseHelper.ExecuteNonQueryAsync("DELETE FROM Journal_Entries WHERE Reference_No = @No AND Entry_Type = @Type", new[] { new SqlParameter("@No", voucherNo), new SqlParameter("@Type", docTypeId) }, trans);
            await DatabaseHelper.ExecuteNonQueryAsync("DELETE FROM Cash_Voucher_Details WHERE Voucher_ID = @VoucherID", new[] { new SqlParameter("@VoucherID", voucherId) }, trans);
            await DatabaseHelper.ExecuteNonQueryAsync("DELETE FROM Cash_Vouchers WHERE Voucher_ID = @VoucherID", new[] { new SqlParameter("@VoucherID", voucherId) }, trans);
        }

        // 🌟 جلب كائن السند بشكل لا متزامن
        public static async Task<CashVoucherHeader> GetVoucherAsync(int voucherId)
        {
            string queryHeader = $@"
                SELECT v.*, 
                       ISNULL(uc.Full_Name, uc.Username) AS CreatedByName,
                       ISNULL(uu.Full_Name, uu.Username) AS UpdatedByName
                FROM Cash_Vouchers v
                LEFT JOIN Users uc ON v.{SystemConstants.CreatedBy} = uc.User_ID
                LEFT JOIN Users uu ON v.{SystemConstants.UpdatedBy} = uu.User_ID
                WHERE v.Voucher_ID = @Vid";

            DataTable dtHeader = await DatabaseHelper.GetTableAsync(queryHeader, new[] { new SqlParameter("@Vid", voucherId) });
            if (dtHeader.Rows.Count == 0) return null;

            DataRow r = dtHeader.Rows[0];
            CashVoucherHeader voucher = new CashVoucherHeader
            {
                VoucherID = voucherId,
                VoucherNo = r["Voucher_No"].ToString(),
                VoucherDate = Convert.ToDateTime(r["Voucher_Date"]),
                DocTypeID = Convert.ToInt32(r["Doc_Type_ID"]),
                BoxAccID = r["Box_Acc_ID"].ToString(),
                Amount = Convert.ToDecimal(r["Amount"]),
                AmountForeign = r["Amount_Foreign"] != DBNull.Value ? Convert.ToDecimal(r["Amount_Foreign"]) : 0,
                CurID = Convert.ToInt32(r["Cur_ID"]),
                ExchangeRate = Convert.ToDecimal(r["Exchange_Rate"]),
                Notes = r["Notes"].ToString(),
                IsPosted = (dtHeader.Columns.Contains("Is_Posted") && r["Is_Posted"] != DBNull.Value) ? Convert.ToInt32(Convert.ToBoolean(r["Is_Posted"])) : 0,

                RowVersion = (dtHeader.Columns.Contains("RowVersion") && r["RowVersion"] != DBNull.Value) ? (byte[])r["RowVersion"] : null,

                CreatedBy = r[SystemConstants.CreatedBy] != DBNull.Value ? Convert.ToInt32(r[SystemConstants.CreatedBy]) : 0,
                CreatedByName = r["CreatedByName"] != DBNull.Value ? r["CreatedByName"].ToString() : "",
                CreatedAt = r[SystemConstants.CreatedAt] != DBNull.Value ? (DateTime?)Convert.ToDateTime(r[SystemConstants.CreatedAt]) : null,

                UpdatedBy = r[SystemConstants.UpdatedBy] != DBNull.Value ? Convert.ToInt32(r[SystemConstants.UpdatedBy]) : 0,
                UpdatedByName = r["UpdatedByName"] != DBNull.Value ? r["UpdatedByName"].ToString() : "",
                UpdatedAt = r[SystemConstants.UpdatedAt] != DBNull.Value ? (DateTime?)Convert.ToDateTime(r[SystemConstants.UpdatedAt]) : null
            };

            DataTable dtDetails = await DatabaseHelper.GetTableAsync("SELECT D.*, A.Acc_Name FROM Cash_Voucher_Details D INNER JOIN Accounts A ON D.Acc_ID = A.Acc_ID WHERE D.Voucher_ID = @Vid", new[] { new SqlParameter("@Vid", voucherId) });
            foreach (DataRow rowDet in dtDetails.Rows)
            {
                voucher.Details.Add(new CashVoucherDetail
                {
                    AccID = rowDet["Acc_ID"].ToString(),
                    AccName = rowDet["Acc_Name"].ToString(),
                    AmountCredit = Convert.ToDecimal(rowDet["Amount_Credit"]),
                    AmountDebit = Convert.ToDecimal(rowDet["Amount_Debit"]),
                    CurrencyID = Convert.ToInt32(rowDet["Currency_ID"]),
                    ExchangeRate = Convert.ToDecimal(rowDet["Exchange_Rate"]),
                    AmountForeign = rowDet["Amount_Foreign"] != DBNull.Value ? Convert.ToDecimal(rowDet["Amount_Foreign"]) : 0,
                    Notes = rowDet["Notes"].ToString()
                });
            }
            return voucher;
        }
    }
}