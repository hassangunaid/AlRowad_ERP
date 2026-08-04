using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace AlRowad_ERP.Core
{
    public static class CurrencyHelper
    {
        public static decimal GetExchangeRate(int curID)
        {
            SqlParameter[] pars = { new SqlParameter("@Cur_ID", curID) };
            object res = DatabaseHelper.ExecuteScalar("SELECT Exchange_Rate FROM Currencies WHERE Cur_ID = @Cur_ID", pars);
            return res != null ? Convert.ToDecimal(res) : 1.0m;
        }

        public static void FillCurrencyComboBox(ComboBox cmb)
        {
            cmb.DataSource = DatabaseHelper.GetTable("SELECT Cur_ID, Cur_Name FROM Currencies WHERE Is_Active = 1");
            cmb.DisplayMember = "Cur_Name";
            cmb.ValueMember = "Cur_ID";
        }
        // دالة التحقق مما إذا كانت العملة مجمدة لحساب مالي معين (الصندوق/البنك/العميل)
        public static bool IsCurrencyFrozenForAccount(string accId, int curId)
        {
            string query = @"SELECT ISNULL(Is_Frozen, 0) 
                             FROM Account_Allowed_Currencies 
                             WHERE Acc_ID = @AccID AND Cur_ID = @CurID";

            SqlParameter[] pars = {
                new SqlParameter("@AccID", accId.Trim()),
                new SqlParameter("@CurID", curId)
            };

            object result = DatabaseHelper.ExecuteScalar(query, pars);

            // إذا وجد نتيجة يعيد حالتها، وإذا لم يجد يعتبرها غير مجمدة (أو يمكنك عكس المنطق حسب حاجة النظام)
            return result != null && Convert.ToBoolean(result);
        }
        public static bool IsCurrencyActive(int curId)
        {
            SqlParameter[] pars = { new SqlParameter("@Cur_ID", curId) };
            object result = DatabaseHelper.ExecuteScalar("SELECT Is_Active FROM Currencies WHERE Cur_ID = @Cur_ID", pars);
            return result != null && Convert.ToBoolean(result);
        }

        // دالة لجلب معرف العملة المحلية للنظام
        public static int GetLocalCurrencyId()
        {
            object res = DatabaseHelper.ExecuteScalar("SELECT TOP 1 Cur_ID FROM Currencies WHERE Is_Local_Currency = 1", null);
            return res != null ? Convert.ToInt32(res) : 1;
        }
        // دالة الفحص المركزي المعدلة لتشمل (سيادة النظام، الحساب المباشر، والحساب الأب)
        public static (bool IsGloballyActive, bool IsAccountFrozen, bool IsParentFrozen) CheckCurrencyTransactionStatus(string accId, int curId)
        {
            try
            {
                // 🚀 الاستعلام المعماري الجديد: يجلب حالة العملة للحساب، وللأب في نفس اللحظة
                string sql = @"
            SELECT 
                ISNULL(C.Is_Active, 0) AS Global_Active,
                ISNULL(AAC.Is_Frozen, 0) AS Account_Frozen,
                ISNULL(PAAC.Is_Frozen, 0) AS Parent_Frozen
            FROM Accounts A
            INNER JOIN Currencies C ON C.Cur_ID = @CurID
            LEFT JOIN Account_Allowed_Currencies AAC ON AAC.Acc_ID = A.Acc_ID AND AAC.Cur_ID = C.Cur_ID
            LEFT JOIN Account_Allowed_Currencies PAAC ON PAAC.Acc_ID = A.Parent_ID AND PAAC.Cur_ID = C.Cur_ID
            WHERE A.Acc_ID = @AccID";

                DataTable dt = DatabaseHelper.ExecuteQuery(sql, new[] {
            new SqlParameter("@AccID", accId),
            new SqlParameter("@CurID", curId)
        });

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    return (
                        Convert.ToBoolean(row["Global_Active"]),
                        Convert.ToBoolean(row["Account_Frozen"]),
                        Convert.ToBoolean(row["Parent_Frozen"])
                    );
                }
            }
            catch (Exception ex)
            {
                // تسجيل الخطأ مستقبلاً
            }

            // الإغلاق الأمني (Fail-Safe): إذا فشل الاستعلام، نعتبر العملة مجمدة للجميع لحماية النظام
            return (false, true, true);
        }
        // دالة لجلب معرف العملة الافتراضية (التفضيلية) للنظام
        public static int GetBaseCurrencyId()
        {
            object res = DatabaseHelper.ExecuteScalar("SELECT TOP 1 Cur_ID FROM Currencies WHERE Is_Base_Currency = 1", null);
            return res != null ? Convert.ToInt32(res) : GetLocalCurrencyId();
        }
        // في CurrencyHelper.cs
        // أضف هذه الدالة داخل كلاس CurrencyHelper
        public static bool IsExchangeRateValid(int curId, decimal enteredRate, out string errorMessage)
        {
            errorMessage = string.Empty;

            // استدعاء الحدود من قاعدة البيانات (يُفضل لاحقاً جعلها مكيشة Caching لسرعة الأداء)
            string sql = "SELECT Min_Exchange_Rate, Max_Exchange_Rate FROM Currencies WHERE Cur_ID = @CurID";
            SqlParameter[] p = { new SqlParameter("@CurID", curId) };
            DataTable dt = DatabaseHelper.ExecuteQuery(sql, p);

            if (dt.Rows.Count > 0)
            {
                decimal minRate = Convert.ToDecimal(dt.Rows[0]["Min_Exchange_Rate"]);
                decimal maxRate = Convert.ToDecimal(dt.Rows[0]["Max_Exchange_Rate"]);

                // حماية العملة المحلية: إذا كان الحد الأدنى والأعلى 1، والسعر المدخل ليس 1
                if (minRate == 1 && maxRate == 1 && enteredRate != 1)
                {
                    errorMessage = "عذراً! لا يمكن تغيير سعر صرف العملة المحلية. يجب أن يظل دائماً 1.0000";
                    return false;
                }

                // حماية العملات الأجنبية: النطاق
                if (enteredRate < minRate || enteredRate > maxRate)
                {
                    errorMessage = $"سعر الصرف المدخل ({enteredRate}) غير منطقي! \nيجب أن يكون السعر بين [{minRate}] و [{maxRate}].";
                    return false;
                }
            }
            return true;
        }
        public static DataTable GetAllowedCurrenciesForAccount(string accId)
        {
            // حماية مبكرة: إذا كان الحساب فارغاً، أعد جدولاً فارغاً فوراً دون استدعاء قاعدة البيانات
            if (string.IsNullOrWhiteSpace(accId))
            {
                return new DataTable(); // يمنع الـ Timeout الناتج عن الاستعلامات الميتة
            }

            string query = @"SELECT c.Cur_ID, c.Cur_Name 
                     FROM Currencies c
                     INNER JOIN Account_Allowed_Currencies a ON c.Cur_ID = a.Cur_ID
                     WHERE a.Acc_ID = @AccID AND c.Is_Active = 1";

            SqlParameter[] pars = { new SqlParameter("@AccID", accId.Trim()) };
            return DatabaseHelper.ExecuteQuery(query, pars);
        }        // المحرك المركزي لترحيل الحركات مدعوماً بالتوثيق المصدري (ACID Compliant)
        public static void PostToLedger(string accID, decimal debit, decimal credit, int curID, decimal rate, string notes, int docTypeId, string sourceDocNo, SqlTransaction trans)
        {
            string sql = @"INSERT INTO System_Ledger (Acc_ID, Debit_Local, Credit_Local, Currency_ID, Exchange_Rate, Notes, Post_Date, Doc_Type_ID, Source_Doc_No) 
                   VALUES (@AccID, @Debit, @Credit, @Cur, @Rate, @Notes, GETDATE(), @DocType, @SourceNo)";

            SqlParameter[] pars = {
        new SqlParameter("@AccID", accID),
        new SqlParameter("@Debit", debit * rate),
        new SqlParameter("@Credit", credit * rate),
        new SqlParameter("@Cur", curID),
        new SqlParameter("@Rate", rate),
        new SqlParameter("@Notes", notes ?? ""),
        new SqlParameter("@DocType", docTypeId),
        new SqlParameter("@SourceNo", sourceDocNo)
    };

            DatabaseHelper.ExecuteNonQuery(sql, pars, trans);
        }
    }
}