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
            }
            catch { }

            // إبلاغ الأب بفتح الوضع الجديد
            ChangeFormMode(FormMode.New);
            item_NameTextBox?.Focus();
        }

        public override void OnEdit()
        {
            // فتح الحقول للتعديل
            ChangeFormMode(FormMode.Edit);
            item_NameTextBox?.Focus();
        }

       
        public override void OnCancel()
        {
            // استدعاء حماية التراجع
            base.OnCancel();

            try
            {
                ChangeFormMode(FormMode.View);
            }
            catch { }
        }
                      
        public override void OnDelete()
        {
            {
                MessageBox.Show("يرجى اختيار صنف لاستعراضه أولاً قبل محاولة حذفه.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show("هل أنت متأكد من حذف الصنف المحدد نهائياً؟", "تأكيد الحذف", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                try
                {
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