using AlRowad_ERP.Core;
using AlRowad_ERP.Core.Constants;   // بافتراض أن DatabaseHelper موجود هنا
using AlRowad_ERP.Data;       // بافتراض أن SystemConstants موجود هنا
using System;
using System.Data.SqlClient; // ضروري لاستخدام SqlParameter
using System.Threading.Tasks;
public class FinancialPeriodRepository
{
    public async Task<bool> IsPeriodOpenAsync(DateTime transactionDate, SqlTransaction transaction = null)
    {
        // استعلام نظيف يعتمد على الثوابت الدستورية
        string query = $@"
            SELECT COUNT(1) 
            FROM {SystemConstants.FinancialPeriods} 
            WHERE @TransDate BETWEEN Start_Date AND End_Date 
            AND {SystemConstants.Is_Closed} = 0";

        // بناء المعاملات بالشكل الذي يقبله DatabaseHelper الخاص بك
        SqlParameter[] parameters = new SqlParameter[]
        {
            new SqlParameter("@TransDate", transactionDate)
        };

        // استدعاء الدالة غير العامة (Non-generic) وإرسال الترانزاكشن إن وجد
        object result = await DatabaseHelper.ExecuteScalarAsync(query, parameters, transaction);

        // التحقق من النتيجة وتحويلها بأمان إلى رقم صحيح
        int count = (result != null && result != DBNull.Value) ? Convert.ToInt32(result) : 0;

        return count > 0;
    }
}