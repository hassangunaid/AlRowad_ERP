using AlRowad_ERP.Core;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;
using AlRowad_ERP.UI.Base;
using AlRowad_ERP.Core.Constants;

namespace AlRowad_ERP.Forms
{
    public partial class UnitsForm : BaseEntryForm
    {

        public UnitsForm()
        {
            InitializeComponent();
            PrimaryIdFieldName = "unit_IDTextBox";
        }

        private void UnitsForm_Load(object sender, EventArgs e)
        {
            try
            {
                this.unitsTableAdapter.Fill(this.alRowad_ERPDataSet.Units);

                // الانتقال لوضع الاستعراض المحمي فور الفتح
                ChangeFormMode(FormMode.View);

                if (this.unitsDataGridView != null)
                {
                    this.unitsDataGridView.CellDoubleClick += UnitsDataGridView_CellDoubleClick;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ أثناء تحميل بيانات الوحدات: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #region محرك إدارة الحالات (امتداد الأب)

        protected override void LockControls(Control parent, bool isReadOnly)
        {
            // استدعاء الأب ليقفل الحقول النصية والكومبو بوكس
            base.LockControls(parent, isReadOnly);

            // قفل جدول الوحدات أثناء التحرير
            if (unitsDataGridView != null)
                unitsDataGridView.Enabled = isReadOnly;
        }

        #endregion

        private void UnitsDataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || CurrentMode != FormMode.View) return;

            try
            {
                DataGridViewRow row = unitsDataGridView.Rows[e.RowIndex];
                unit_IDTextBox.Text = row.Cells["Unit_ID"].Value?.ToString();
                unit_NameTextBox.Text = row.Cells["Unit_Name"].Value?.ToString();

                // الانتقال لوضع الاستعراض بعد اختيار السجل
                ChangeFormMode(FormMode.View);
            }
            catch (Exception ex)
            {
                MessageBox.Show("فشل في تعبئة بيانات الوحدة: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public override void OnNew()
        {
            base.OnNew(); // تفريغ الحقول عبر الأب

            try
            {
                string next = DatabaseHelper.GetNextCode("Units", "Unit_ID");
                unit_IDTextBox.Text = next;
                unit_NameTextBox.Focus();

                ChangeFormMode(FormMode.New);
            }
            catch { }
        }

        public override void OnEdit()
        {
            if (string.IsNullOrWhiteSpace(unit_IDTextBox.Text)) return;
            ChangeFormMode(FormMode.Edit);
            unit_NameTextBox.Focus();
        }
          /*
        protected override bool ExecuteSaveToDatabase(SqlTransaction trans)
        {
            // 1. التحقق المبدئي من الحقول الإجبارية (يرجى مطابقة أسماء الأدوات مع ما هو موجود في واجهة التصميم)
            if (string.IsNullOrWhiteSpace(unit_NameTextBox.Text))
            {
                MessageBox.Show("يجب إدخال اسم الوحدة.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            try
            {
                // 2. صياغة الاستعلام مع حقول التدقيق (ملاحظة: Unit_ID هو IDENTITY ولا يتم إدراجه يدوياً)
                string sqlQuery = (CurrentMode == FormMode.Edit)
                    ? @"UPDATE Units 
                        SET Unit_Name = @Unit_Name, Conversion_Factor = @Conversion_Factor, 
                            Updated_By = @Updated_By, Updated_At = @Updated_At 
                        WHERE Unit_ID = @Unit_ID"
                    : @"INSERT INTO Units 
                        (Unit_Name, Conversion_Factor, Created_By, Created_At) 
                        VALUES (@Unit_Name, @Conversion_Factor, @Created_By, @Created_At)";

                // معالجة القيم الرقمية بأمان
                decimal.TryParse(conversion_FactorTextBox.Text, out decimal conversionFactor);
                if (conversionFactor <= 0) conversionFactor = 1; // القيمة الافتراضية لمعامل التحويل هي 1

                // 3. تجهيز البارامترات الأساسية
                var pHeader = new System.Collections.Generic.List<SqlParameter>
                {
                    new SqlParameter("@Unit_Name", unit_NameTextBox.Text.Trim()),
                    new SqlParameter("@Conversion_Factor", conversionFactor)
                };

                // إضافة مفتاح السجل في حالة التعديل فقط
                if (CurrentMode == FormMode.Edit)
                {
                    pHeader.Add(new SqlParameter("@Unit_ID", unit_IDTextBox.Text.Trim()));
                }

                // 4. حقن بيانات المستخدم والوقت أوتوماتيكياً (Audit Trail)
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

                // 5. التنفيذ الآمن تحت مظلة المعاملة المركزية (SqlTransaction)
                DatabaseHelper.ExecuteNonQuery(sqlQuery, pHeader.ToArray(), trans);

                // 6. تسجيل الحركة في الجدول الرقابي عند التعديل
                if (CurrentMode == FormMode.Edit)
                {
                    string oldValues = "تم الحفظ المسبق"; // مستقبلاً يمكن استخراج النسخة من الذاكرة
                    string newValues = $"الاسم: {unit_NameTextBox.Text.Trim()} | معامل التحويل: {conversionFactor}";
                    DatabaseHelper.LogAuditTransaction(trans, "Units", unit_IDTextBox.Text.Trim(), "UPDATE", oldValues, newValues, "تعديل وحدة قياس");
                }

                return true;
            }
            catch (Exception ex)
            {
                // رمي الخطأ ليتم التقاطه وتنفيذ Rollback بأمان في الـ BaseEntryForm
                throw new Exception($"خطأ أثناء حفظ بيانات الوحدة: {ex.Message}");
            }
        }        */

        // 7. يمكنك عمل Override لدالة RefreshData من الـ BaseEntryForm لتحديث الشجرة أو الجدول بعد الحفظ الناجح
        protected override void RefreshData()
        {
            try
            {
                // تحديث البيانات في الشاشة (مثلاً DataGridView أو غيره) بدلاً من tableAdapter القديم
                // مثال: dgvUnits.DataSource = DatabaseHelper.GetTable("SELECT * FROM Units");
            }
            catch { }
        }

        public override void OnCancel()
        {
            base.OnCancel();
            try
            {
                this.unitsTableAdapter.Fill(this.alRowad_ERPDataSet.Units);
                ChangeFormMode(FormMode.View);
            }
            catch { }
        }

        public override void OnDelete()
        {
            if (CurrentMode != FormMode.View || unitsBindingSource.Current == null)
            {
                MessageBox.Show("يرجى اختيار وحدة لاستعراضها أولاً قبل حذفها.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show("هل أنت متأكد من حذف الوحدة المحددة؟", "تأكيد الحذف", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                try
                {
                    this.unitsBindingSource.RemoveCurrent();
                    this.tableAdapterManager.UpdateAll(this.alRowad_ERPDataSet);
                    MessageBox.Show("تم حذف السجل بنجاح.", "تم", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ChangeFormMode(FormMode.View);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("فشل الحذف: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}