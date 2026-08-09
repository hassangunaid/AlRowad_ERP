using System;
using System.Linq;
using System.Windows.Forms;
using AlRowad_ERP.Core;
using AlRowad_ERP.UI.Base;
using AlRowad_ERP.Core.Constants;


namespace AlRowad_ERP.Forms
{
    public partial class Stores : BaseEntryForm
    {

        public Stores()
        {
        }

        private void Stores_Load(object sender, EventArgs e)
        {
            try
            {
                TryFillTableAdapter("item_BalancesTableAdapter", "Item_Balances");
                TryFillTableAdapter("storesTableAdapter", "Stores");

                // الدستور: وضع الاستعراض المحمي فور الفتح
                ChangeFormMode(FormMode.View);
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ أثناء تحميل بيانات المخازن: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #region محرك إدارة الحالات (امتداد الأب)

        protected override void LockControls(Control parent, bool isReadOnly)
        {
            // 1. استدعاء الأب ليقفل ويفتح الأدوات القياسية
            base.LockControls(parent, isReadOnly);

            // 2. قفل حماية حقل المعرف (يفتح فقط في وضع الإضافة)
            var idTb = FindControlByName(this, "store_IDTextBox") as TextBox;
            if (idTb != null) idTb.ReadOnly = (CurrentMode != FormMode.New);

            // 3. قفل جدول المخازن في وضع التعديل/الإضافة لتركيز المستخدم
            var dgv = FindControlByName(this, "storesDataGridView") as DataGridView;
            if (dgv != null) dgv.Enabled = isReadOnly;
        }

        #endregion

        private void StoresDataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || CurrentMode != FormMode.View) return;

            try
            {
                var dgv = sender as DataGridView;
                if (dgv == null) return;

                DataGridViewRow row = dgv.Rows[e.RowIndex];
                SetTextBoxValue("store_IDTextBox", row.Cells["Store_ID"]?.Value?.ToString());
                SetTextBoxValue("store_NameTextBox", row.Cells["Store_Name"]?.Value?.ToString());
                SetTextBoxValue("store_LocationTextBox", row.Cells["Store_Location"]?.Value?.ToString());

                // إبلاغ الأب بوضع الاستعراض بعد اختيار السجل
                ChangeFormMode(FormMode.View);
            }
            catch (Exception ex)
            {
                MessageBox.Show("فشل في تعبئة بيانات المخزن: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public override void OnNew()
        {
            base.OnNew(); // تنظيف الحقول عبر الأب

            try
            {
            }
            catch { }

            ChangeFormMode(FormMode.New);
            var tb = FindControlByName(this, "store_NameTextBox") as TextBox;
            tb?.Focus();
        }

        public override void OnEdit()
        {
            if (string.IsNullOrWhiteSpace(GetTextBoxValue("store_IDTextBox"))) return;
            ChangeFormMode(FormMode.Edit);
        }


        public override void OnCancel()
        {
            base.OnCancel();
            try
            {
                TryFillTableAdapter("storesTableAdapter", "Stores");
                ChangeFormMode(FormMode.View);
            }
            catch { }
        }

        public override void OnDelete()
        {
            var idVal = GetTextBoxValue("store_IDTextBox");
            if (string.IsNullOrWhiteSpace(idVal) || CurrentMode != FormMode.View)
            {
                MessageBox.Show("يرجى اختيار السجل المراد حذفه أولاً.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("هل أنت متأكد من حذف المخزن المحدد؟", "تأكيد الحذف", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    TryRemoveCurrentBindingSource("storesBindingSource");
                    TryUpdateTableAdapterManager();
                    MessageBox.Show("تم حذف السجل بنجاح.", "تم", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ChangeFormMode(FormMode.View);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("فشل الحذف: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // --- Helper methods (كما هي) ---
        private Control FindControlByName(Control parent, string name) { /* ... */ return null; } // احتفظ بـ implementation الخاص بك
        private void SetTextBoxValue(string name, string value) { /* ... */ }
        private string GetTextBoxValue(string name) { /* ... */ return null; }
        private void TryFillTableAdapter(string adapterFieldName, string dataTablePropertyName) { /* ... */ }
        private void TryEndEditBindingSource(string bindingSourceFieldName) { /* ... */ }
        private void TryRemoveCurrentBindingSource(string bindingSourceFieldName) { /* ... */ }
        private void TryUpdateTableAdapterManager() { /* ... */ }
    }
}