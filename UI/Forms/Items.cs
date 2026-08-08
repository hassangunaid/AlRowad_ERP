using AlRowad_ERP.Core;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;
using AlRowad_ERP.UI.Base;
using AlRowad_ERP.Core.Constants;


namespace AlRowad_ERP.Forms
{
    public partial class Items : BaseEntryForm
    {

        public Items()
        {
            InitializeComponent();
            this.Load += Items_Load;
            PrimaryIdFieldName = "item_IDTextBox";
            if (itemsDataGridView != null) itemsDataGridView.CellDoubleClick += ItemsDataGridView_CellDoubleClick;
        }

        private void Items_Load(object sender, EventArgs e)
        {
            try
            {
                this.itemsTableAdapter.Fill(this.alRowad_ERPDataSet.Items);

                // الانتقال لوضع الاستعراض الآمن فوراً
                ChangeFormMode(FormMode.View);
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ أثناء تحميل بيانات الاصناف: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #region محرك إدارة الحالات (امتداد الأب)

        protected override void LockControls(Control parent, bool isReadOnly)
        {
            // استدعاء الأب لقفل الحقول النصية والكومبو بوكس
            base.LockControls(parent, isReadOnly);

            // قفل حماية حقل المعرف (يُفتح فقط في وضع الإضافة)
            if (item_IDTextBox != null)
                item_IDTextBox.ReadOnly = (CurrentMode != FormMode.New);

            // قفل جدول البيانات في حالة التحرير لمنع التنقل العشوائي
            if (itemsDataGridView != null)
                itemsDataGridView.Enabled = isReadOnly;
        }

        #endregion

        #region العمليات السيادية (CRUD Engine)

        public override void OnNew()
        {
            // تفريغ البيانات عبر الأب
            base.OnNew();

            try
            {
                string next = DatabaseHelper.GetNextCode("Items", "Item_ID");
                if (!string.IsNullOrEmpty(next) && item_IDTextBox != null)
                    item_IDTextBox.Text = next;
            }
            catch { }

            // إبلاغ الأب بفتح الوضع الجديد
            ChangeFormMode(FormMode.New);
            item_NameTextBox?.Focus();
        }

        public override void OnEdit()
        {
            if (itemsBindingSource.Current == null) return;

            // فتح الحقول للتعديل
            ChangeFormMode(FormMode.Edit);
            item_NameTextBox?.Focus();
        }

        /*
        protected override bool ExecuteSaveToDatabase(SqlTransaction trans)
        {
            // 1. التحقق المبدئي من الحقول الإجبارية (يرجى مطابقة أسماء الـ TextBoxes مع تصميم الشاشة لديك)
            if (string.IsNullOrWhiteSpace(item_NameTextBox.Text))
            {
                MessageBox.Show("يجب إدخال اسم الصنف.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            try
            {
                // 2. صياغة الاستعلام مع حقول التدقيق الزمنية والمستخدم
                // ملاحظة: Item_ID في القاعدة هو IDENTITY لذلك لا نمرره في جملة INSERT
                string sqlQuery = (CurrentMode == FormMode.Edit)
                    ? @"UPDATE Items 
                        SET Item_Name = @Item_Name, Base_Unit_ID = @Base_Unit_ID, Default_Price = @Default_Price,
                            Updated_By = @Updated_By, Updated_At = @Updated_At 
                        WHERE Item_ID = @Item_ID"
                    : @"INSERT INTO Items 
                        (Item_Name, Base_Unit_ID, Default_Price, Created_By, Created_At) 
                        VALUES (@Item_Name, @Base_Unit_ID, @Default_Price, @Created_By, @Created_At)";

                // تجهيز المتغيرات العددية لتجنب أخطاء التحويل
                int.TryParse(base_Unit_IDComboBox?.SelectedValue?.ToString(), out int baseUnitId);
                decimal.TryParse(default_PriceTextBox.Text, out decimal defaultPrice);

                // 3. تجهيز البارامترات الأساسية باستخدام List
                var pHeader = new System.Collections.Generic.List<SqlParameter>
                {
                    new SqlParameter("@Item_Name", item_NameTextBox.Text.Trim()),
                    new SqlParameter("@Base_Unit_ID", baseUnitId > 0 ? (object)baseUnitId : DBNull.Value),
                    new SqlParameter("@Default_Price", defaultPrice)
                };

                // إضافة شرط الـ ID فقط في حالة التعديل
                if (CurrentMode == FormMode.Edit)
                {
                    pHeader.Add(new SqlParameter("@Item_ID", item_IDTextBox.Text.Trim()));
                }

                // 4. حقن بيانات المستخدم والوقت أوتوماتيكياً من الجلسة للتدقيق
                if (CurrentMode == FormMode.New)
                {
                    pHeader.Add(new SqlParameter("@Created_By", UserSession.UserId));
                    pHeader.Add(new SqlParameter("@Created_At", DateTime.Now));
                }
                else if (CurrentMode == FormMode.Edit)
                {
                    pHeader.Add(new SqlParameter("@Updated_By", UserSession.UserId));
                    pHeader.Add(new SqlParameter("@Updated_At", DateTime.Now));
                }

                // 5. تنفيذ الاستعلام وتمرير معاملة الـ Transaction
                DatabaseHelper.ExecuteNonQuery(sqlQuery, pHeader.ToArray(), trans);

                // 6. تسجيل الحركة في الجدول الرقابي عند التعديل (Audit Trail)
                if (CurrentMode == FormMode.Edit)
                {
                    string oldValues = "تم الحفظ المسبق"; // مستقبلاً يمكن جلب JSON من الذاكرة المؤقتة للنموذج
                    string newValues = $"الاسم: {item_NameTextBox.Text.Trim()} | السعر: {defaultPrice}";
                    DatabaseHelper.LogAuditTransaction(trans, "Items", item_IDTextBox.Text.Trim(), "UPDATE", oldValues, newValues, "تعديل بيانات الصنف");
                }

                // إرجاع نجاح للـ BaseEntryForm ليقوم بعمل Commit آمن
                return true;
            }
            catch (Exception ex)
            {
                // رمي الخطأ ليتم التقاطه والتراجع (Rollback) بشكل آمن في الـ BaseEntryForm
                throw new Exception($"خطأ أثناء حفظ الصنف: {ex.Message}");
            }
        }
        public override void OnCancel()
        {
            // استدعاء حماية التراجع
            base.OnCancel();

            try
            {
                this.itemsTableAdapter.Fill(this.alRowad_ERPDataSet.Items);
                ChangeFormMode(FormMode.View);
            }
            catch { }
        }
                      */
        public override void OnDelete()
        {
            if (CurrentMode != FormMode.View || itemsBindingSource.Current == null)
            {
                MessageBox.Show("يرجى اختيار صنف لاستعراضه أولاً قبل محاولة حذفه.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show("هل أنت متأكد من حذف الصنف المحدد نهائياً؟", "تأكيد الحذف", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                try
                {
                    this.itemsBindingSource.RemoveCurrent();
                    this.tableAdapterManager.UpdateAll(this.alRowad_ERPDataSet);
                    MessageBox.Show("تم حذف السجل بنجاح.", "تم", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ChangeFormMode(FormMode.View);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("فشل الحذف لوجود ارتباطات مالية بهذا الصنف: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ItemsDataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || CurrentMode != FormMode.View) return;

            // الانتقال لوضع الاستعراض المحمي بعد اختيار الصنف
            ChangeFormMode(FormMode.View);
        }

        #endregion
    }
}