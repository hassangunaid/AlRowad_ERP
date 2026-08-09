using AlRowad_ERP.Core;
using AlRowad_ERP.Core.Constants;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AlRowad_ERP.Data
{
    public static class AccountRepository
    {
        public static DataTable GetAccountsTree()
        {
            return DatabaseHelper.GetTable("SELECT Acc_ID, Acc_Name, Parent_ID FROM Accounts ORDER BY Acc_ID");
        }

        public static DataTable GetAccountDetails(string accId)
        {
            // 🚀 تعديل معماري: جلب تفاصيل الحساب مع أسماء المستخدمين للرقابة
            string query = @"
                SELECT a.*, 
                       ISNULL(uc.Full_Name, uc.Username) AS CreatedByName,
                       ISNULL(uu.Full_Name, uu.Username) AS UpdatedByName
                FROM Accounts a
                LEFT JOIN Users uc ON a.Created_By = uc.User_ID
                LEFT JOIN Users uu ON a.Updated_By = uu.User_ID
                WHERE a.Acc_ID = @Acc_ID";

            return DatabaseHelper.ExecuteQuery(query, new[] { new SqlParameter("@Acc_ID", accId) });
        }
        /// <summary>
        /// الفحص السيادي: التأكد من خلو الحساب من أي ارتباطات مالية أو مرجعية قبل الحذف المنطقي
        /// </summary>
        private static async Task<bool> HasFinancialTransactionsAsync(string accId, SqlTransaction trans)
        {
            // الدستور: فحص شامل على مستوى قاعدة البيانات لكل الجداول التي قد تحتوي على الحساب
            // (يرجى التأكد من مطابقة أسماء الجداول لحالة قاعدة بياناتك الفعلية)
            string query = @"
                SELECT 
                    (SELECT COUNT(1) FROM Journal_Details WHERE Acc_ID = @AccID) +
                    (SELECT COUNT(1) FROM Invoice_Header WHERE Acc_ID = @AccID) +
                    (SELECT COUNT(1) FROM Receipt_Vouchers WHERE Acc_ID = @AccID) +
                    (SELECT COUNT(1) FROM Payment_Vouchers WHERE Acc_ID = @AccID)";

            object result = await DatabaseHelper.ExecuteScalarAsync(query, new[] { new SqlParameter("@AccID", accId) }, trans);

            return (result != null && Convert.ToInt32(result) > 0);
        }

        /// <summary>
        /// الحذف المنطقي الآمن (Soft Delete)
        /// </summary>
        public static async Task SoftDeleteAccountAsync(string accId, int currentUserId, SqlTransaction trans)
        {
            // 🌟 1. جدار الحماية الأول والأخير: لا يمكن تجاوزه أبداً
            bool hasTransactions = await HasFinancialTransactionsAsync(accId, trans);
            if (hasTransactions)
            {
                // رمي خطأ سيادي يوقف العملية تماماً ويجبر الشاشة على إظهاره للمستخدم
                throw new InvalidOperationException("حظر سيادي: يُمنع منعاً باتاً حذف أو إيقاف هذا الحساب لارتباطه بحركات مالية أو قيود محاسبية سابقة.");
            }

            // 🌟 2. التنفيذ في حال اجتياز الفحص الأمني (تحديث حالة الحساب فقط - Soft Delete)
            string updateAcc = $@"UPDATE Accounts SET Is_Stopped = 1, {SystemConstants.AuditFields.UpdatedBy} = @UserId, {SystemConstants.AuditFields.UpdatedAt} = GETDATE() WHERE Acc_ID = @AccID";

            await DatabaseHelper.ExecuteNonQueryAsync(updateAcc, new[] {
                new SqlParameter("@AccID", accId),
                new SqlParameter("@UserId", currentUserId)
            }, trans);
        }
        public static DataTable GetDropdownData(string tableName, string idCol, string nameCol)
        {
            return DatabaseHelper.GetTable($"SELECT {idCol}, {nameCol} FROM {tableName} ORDER BY {idCol}");
        }

        public static DataTable GetAccountCurrencies(string accId, string parentId, int level)
        {
            string query = "";
            SqlParameter[] p = null;

            if (level == 4)
            {
                query = @"
            SELECT C.Cur_ID, C.Cur_Name,
                   ISNULL(C.Is_Active, 0) AS Global_Active, /* 🚀 جلبنا حالة العملة السيادية */
                   ISNULL(A.Is_Active, 0) AS Is_Active,
                   ISNULL(A.Is_Frozen, 0) AS Is_Frozen,
                   ISNULL(A.Is_Default, 0) AS Is_Default
            FROM Currencies C
            LEFT JOIN Account_Allowed_Currencies A ON C.Cur_ID = A.Cur_ID AND A.Acc_ID = @Acc_ID";
                /* تم إزالة شرط WHERE C.Is_Active = 1 لتظهر دائماً */

                p = new[] { new SqlParameter("@Acc_ID", accId) };
            }
            else if (level == 5)
            {
                query = @"
            SELECT C.Cur_ID, C.Cur_Name,
                   ISNULL(C.Is_Active, 0) AS Global_Active, /* 🚀 جلبنا حالة العملة السيادية */
                   ISNULL(A.Is_Active, 0) AS Is_Active,
                   ISNULL(A.Is_Frozen, 0) AS Is_Frozen,
                   ISNULL(A.Is_Default, 0) AS Is_Default
            FROM Currencies C
            INNER JOIN Account_Allowed_Currencies P ON C.Cur_ID = P.Cur_ID AND P.Acc_ID = @ParentID AND P.Is_Active = 1
            LEFT JOIN Account_Allowed_Currencies A ON C.Cur_ID = A.Cur_ID AND A.Acc_ID = @Acc_ID";
                /* تم إزالة شرط WHERE C.Is_Active = 1 */

                p = new[] { new SqlParameter("@Acc_ID", accId), new SqlParameter("@ParentID", parentId) };
            }

            return DatabaseHelper.ExecuteQuery(query, p);
        }
        public static int CheckParentCurrenciesCount(string parentId)
        {
            string checkParentSql = "SELECT COUNT(*) FROM Account_Allowed_Currencies WHERE Acc_ID = @ParentID AND Is_Active = 1";
            return Convert.ToInt32(DatabaseHelper.ExecuteScalar(checkParentSql, new[] { new SqlParameter("@ParentID", parentId) }) ?? 0);
        }

        public static bool CanDeactivateCurrency(string accId, int curId, int level, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                string sqlTrans = "SELECT COUNT(*) FROM System_Ledger WHERE Acc_ID = @AccID AND Currency_ID = @CurID";
                int transCount = Convert.ToInt32(DatabaseHelper.ExecuteScalar(sqlTrans, new[] { new SqlParameter("@AccID", accId), new SqlParameter("@CurID", curId) }) ?? 0);

                if (transCount > 0)
                {
                    errorMessage = $"لا يمكن إلغاء تفعيل العملة لوجود ({transCount}) حركة مالية مسجلة عليها في دفتر الأستاذ.";
                    return false;
                }

                if (level == 4)
                {
                    string sqlChildren = @"
                        SELECT COUNT(*) FROM Account_Allowed_Currencies AAC 
                        INNER JOIN Accounts A ON AAC.Acc_ID = A.Acc_ID 
                        WHERE A.Parent_ID = @AccID AND AAC.Cur_ID = @CurID AND AAC.Is_Active = 1";
                    int childrenCount = Convert.ToInt32(DatabaseHelper.ExecuteScalar(sqlChildren, new[] { new SqlParameter("@AccID", accId), new SqlParameter("@CurID", curId) }) ?? 0);

                    if (childrenCount > 0)
                    {
                        errorMessage = $"مرفوض: يوجد ({childrenCount}) حسابات فرعية تعتمد على هذه العملة. قم بإلغائها من الأبناء أولاً.";
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = "خطأ في فحص سياسة إلغاء التفعيل: " + ex.Message;
                return false;
            }
        }

        public static string GetNextAccountID(string parentId)
        {
            try
            {
                string query = string.IsNullOrWhiteSpace(parentId)
                    ? "SELECT MAX(CAST(Acc_ID AS BIGINT)) FROM Accounts WHERE Parent_ID IS NULL OR Parent_ID = ''"
                    : "SELECT MAX(CAST(Acc_ID AS BIGINT)) FROM Accounts WHERE Parent_ID = @Parent_ID";
                SqlParameter[] p = string.IsNullOrWhiteSpace(parentId) ? null : new[] { new SqlParameter("@Parent_ID", parentId) };
                object result = DatabaseHelper.ExecuteScalar(query, p);

                if (result != null && result != DBNull.Value) return (Convert.ToInt64(result) + 1).ToString();
                return string.IsNullOrWhiteSpace(parentId) ? "1" : parentId + "01";
            }
            catch { return string.IsNullOrWhiteSpace(parentId) ? "1" : parentId + "01"; }
        }

        public static int GetAccountLevel(string accountId)
        {
            object result = DatabaseHelper.ExecuteScalar("SELECT Account_Level FROM Accounts WHERE Acc_ID = @Parent_ID", new[] { new SqlParameter("@Parent_ID", accountId) });
            return result != null ? Convert.ToInt32(result) : 0;
        }

        // دالة الحذف الموحدة التي تدير الـ Transaction الخاص بها بناءً على تعليق الملاحظة الثانية
        public static void DeleteAccount(string accountId)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        string checkSql = "SELECT COUNT(*) FROM Accounts WHERE Acc_ID LIKE @Pattern AND Acc_ID != @ParentID";
                        int childrenCount = Convert.ToInt32(DatabaseHelper.ExecuteScalar(checkSql, new[] { new SqlParameter("@Pattern", accountId + "%"), new SqlParameter("@ParentID", accountId) }, trans));

                        if (childrenCount > 0)
                            throw new Exception($"لا يمكن الحذف لوجود ({childrenCount}) حسابات تابعة له.");

                        DatabaseHelper.ExecuteNonQuery("DELETE FROM Account_Allowed_Currencies WHERE Acc_ID = @ParentID", new[] { new SqlParameter("@ParentID", accountId) }, trans);
                        DatabaseHelper.ExecuteNonQuery("DELETE FROM Accounts WHERE Acc_ID = @ParentID", new[] { new SqlParameter("@ParentID", accountId) }, trans);

                        trans.Commit();
                    }
                    catch (SqlException ex)
                    {
                        trans.Rollback();
                        if (ex.Number == 547) throw new Exception("الحساب مرتبط بحركات مالية ولا يمكن حذفه.");
                        throw;
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }
    }
}