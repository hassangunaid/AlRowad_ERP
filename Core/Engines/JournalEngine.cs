using AlRowad_ERP.Core;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks; // 🌟

namespace AlRowad_ERP.Core
{
    public static class JournalEngine
    {
        // 🌟 التحديث الدستوري: تحويل الدالة إلى لامتزامنة (Async) لضمان عدم تجميد النظام أثناء الترحيل
        public static async Task<(bool isSuccess, string errorMessage)> PostEntryAsync(DateTime date, string desc, string refNo, int entryType, DataTable dtItems, SqlTransaction trans = null)
        {
            // 1. الفحص المحاسبي الصارم
            decimal totalDebit = 0;
            decimal totalCredit = 0;

            foreach (DataRow row in dtItems.Rows)
            {
                totalDebit += Convert.ToDecimal(row["Debit"] == DBNull.Value ? 0 : row["Debit"]);
                totalCredit += Convert.ToDecimal(row["Credit"] == DBNull.Value ? 0 : row["Credit"]);
            }

            if (totalDebit == 0 && totalCredit == 0)
                return (false, "رفض أمني: لا يمكن ترحيل قيد محاسبي صفري.");

            if (Math.Round(totalDebit, 4) != Math.Round(totalCredit, 4))
                return (false, $"رفض محاسبي: القيد غير متزن. إجمالي المدين ({totalDebit}) لا يساوي إجمالي الدائن ({totalCredit}).");

            bool isNewTrans = (trans == null);
            SqlConnection localConn = null;
            SqlTransaction localTrans = trans;

            try
            {
                if (isNewTrans)
                {
                    localConn = DatabaseHelper.GetConnection();
                    await localConn.OpenAsync();
                    localTrans = localConn.BeginTransaction();
                }

                // 2. إدخال رأس القيد
                string entryQuery = @"INSERT INTO Journal_Entries (Entry_Date, Description, Reference_No, Entry_Type) 
                                      OUTPUT INSERTED.Journal_ID 
                                      VALUES (@Date, @Desc, @Ref, @Type)";

                SqlParameter[] pEntry = {
                    new SqlParameter("@Date", date),
                    new SqlParameter("@Desc", desc),
                    new SqlParameter("@Ref", refNo),
                    new SqlParameter("@Type", entryType)
                };

                // 🌟 استخدام المحرك اللامتزامن الجديد
                object result = await DatabaseHelper.ExecuteScalarAsync(entryQuery, pEntry, localTrans);
                int journalId = Convert.ToInt32(result);

                // 3. إدخال تفاصيل القيد
                string detailQuery = @"INSERT INTO Journal_Details 
                                       (Journal_ID, Acc_ID, Debit, Credit, Cur_ID, Exchange_Rate, Foreign_Debit, Foreign_Credit) 
                                       VALUES 
                                       (@JournalID, @AccID, @Debit, @Credit, @CurID, @ExRate, @FDebit, @FCredit)";

                foreach (DataRow row in dtItems.Rows)
                {
                    SqlParameter[] pDetail = {
                        new SqlParameter("@JournalID", journalId),
                        new SqlParameter("@AccID", row["Acc_ID"].ToString().Trim()),
                        new SqlParameter("@Debit", row["Debit"]),
                        new SqlParameter("@Credit", row["Credit"]),
                        new SqlParameter("@CurID", row["Cur_ID"].ToString().Trim()),
                        new SqlParameter("@ExRate", row["Exchange_Rate"]),
                        new SqlParameter("@FDebit", row["Foreign_Debit"] == DBNull.Value ? 0 : row["Foreign_Debit"]),
                        new SqlParameter("@FCredit", row["Foreign_Credit"] == DBNull.Value ? 0 : row["Foreign_Credit"])
                    };
                    await DatabaseHelper.ExecuteNonQueryAsync(detailQuery, pDetail, localTrans);
                }

                if (isNewTrans) localTrans.Commit();
                return (true, string.Empty);
            }
            catch (Exception ex)
            {
                if (isNewTrans) localTrans?.Rollback();
                return (false, "خطأ في محرك القيود: " + ex.Message);
            }
            finally
            {
                if (isNewTrans && localConn != null)
                {
                    localConn.Close();
                    localConn.Dispose();
                }
            }
        }
    }
}