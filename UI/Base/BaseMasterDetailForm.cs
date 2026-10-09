using AlRowad_ERP.Core.Constants;
using AlRowad_ERP.UI.Controls; // مسار الأداة المخصصة للجريد
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AlRowad_ERP.UI.Base
{
    // الفئة ترث من الشاشة الأساسية وتخصص لشاشات الرأس والتفاصيل
    public class BaseMasterDetailForm : BaseEntryForm
    {
        // مرجع للجريد الرئيسي في الشاشة ليتم التحكم به مركزياً
        public AlRowadDataGridView MainDetailsGrid { get; set; }

        public BaseMasterDetailForm()
        {
            // المُشيد الافتراضي
        }

        // ==========================================
        // 1. التحكم المركزي في حالة الشاشة (State Management)
        // ==========================================
        protected override void LockControls(Control parent, bool isReadOnly)
        {
            // استدعاء قفل الرأس من الكلاس الأب
            base.LockControls(parent, isReadOnly);

            // قفل شبكة التفاصيل آلياً
            if (MainDetailsGrid != null)
            {
                MainDetailsGrid.ReadOnly = isReadOnly;
                MainDetailsGrid.AllowUserToAddRows = !isReadOnly;
                MainDetailsGrid.AllowUserToDeleteRows = !isReadOnly;

                // حماية الأعمدة الاستعلامية والحسابية آلياً بناءً على أسمائها
                foreach (DataGridViewColumn col in MainDetailsGrid.Columns)
                {
                    string colName = col.Name.ToLower();
                    if (colName.Contains("serial") || colName.Contains("name") ||
                        colName.Contains("total") || colName.Contains("balance"))
                    {
                        col.ReadOnly = true;
                    }
                }
            }
        }

        // ==========================================
        // 2. أتمتة تهيئة الشاشة لعملية جديدة
        // ==========================================
        public override void OnNew()
        {
            base.OnNew();
            if (MainDetailsGrid != null)
            {
                MainDetailsGrid.Rows.Clear(); // تفريغ الشبكة تلقائياً
            }
        }

        // ==========================================
        // 3. تأمين سلامة البيانات (ACID Transactions)
        // ==========================================
        // تم تغليف عملية الحفظ بمسار إجباري يمنع تشتت الكود
        protected override async Task<bool> ExecuteSaveToDatabaseAsync(SqlTransaction trans)
        {
            // 1. التحقق الأساسي (Validation)
            if (!ValidateMasterData()) return false;
            if (!ValidateDetailsData()) return false;

            // 2. حفظ الرأس (Master)
            int headerId = await SaveHeaderAsync(trans);
            if (headerId <= 0) return false; // إذا فشل الرأس، سيتكفل BaseEntryForm بعمل Rollback

            // 3. حفظ التفاصيل (Details)
            bool detailsSaved = await SaveDetailsAsync(headerId, trans);
            if (!detailsSaved) return false; // إذا فشلت التفاصيل، سيتكفل BaseEntryForm بعمل Rollback

            return true; // تمت العملية بنجاح كامل
        }

        // ==========================================
        // 4. دوال الفاتورة (يتم تجاوزها Override في الشاشات الأبناء)
        // ==========================================

        protected virtual bool ValidateMasterData()
        {
            return true;
        }

        protected virtual bool ValidateDetailsData()
        {
            // التحقق الافتراضي: منع حفظ فاتورة بدون تفاصيل
            if (MainDetailsGrid == null || MainDetailsGrid.Rows.Count == 0 ||
               (MainDetailsGrid.Rows.Count == 1 && MainDetailsGrid.Rows[0].IsNewRow))
            {
                MessageBox.Show("يجب إدخال بيانات في شبكة التفاصيل.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        // إجبار المبرمج على فصل كود حفظ الرأس عن التفاصيل
        protected virtual Task<int> SaveHeaderAsync(SqlTransaction trans)
        {
            throw new NotImplementedException("يجب برمجة دالة حفظ الرأس في الشاشة الابن");
        }

        protected virtual Task<bool> SaveDetailsAsync(int headerId, SqlTransaction trans)
        {
            throw new NotImplementedException("يجب برمجة دالة حفظ التفاصيل في الشاشة الابن");
        }
    }
}