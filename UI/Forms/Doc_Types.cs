using AlRowad_ERP.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AlRowad_ERP.UI.Base;
using AlRowad_ERP.Core.Constants;



namespace AlRowad_ERP.Forms
{
    public partial class Doc_Types : BaseEntryForm
    {

        public Doc_Types()
        {
            InitializeComponent();
            this.Load += Doc_Types_Load;
            PrimaryIdFieldName = "docType_IDTextBox";
        }

        private void Doc_Types_Load(object sender, EventArgs e)
        {
            try
            {

                // الدستور: وضع الاستعراض المحمي فور الفتح
                ChangeFormMode(FormMode.View);
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ أثناء تحميل بيانات أنواع المستندات: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #region محرك إدارة الحالات الخاص بالشاشة (امتداد الأب)

        protected override void LockControls(Control parent, bool isReadOnly)
        {
            // 1. استدعاء الأب ليقوم بقفل الأدوات النصية تلقائياً
            base.LockControls(parent, isReadOnly);

            // 2. حماية حقل المعرف المفتاحي لكي يفتح فقط في وضع الإضافة
            var idTextBox = this.Controls.Find("docType_IDTextBox", true).FirstOrDefault() as TextBox;
            if (idTextBox != null)
            {
                idTextBox.ReadOnly = (CurrentMode != FormMode.New);
            }

            // 3. منع التصفح العشوائي في شبكة البيانات (إن وجدت) أثناء وضع الإدخال أو التعديل
            var grid = this.Controls.Find("doc_TypesDataGridView", true).FirstOrDefault() as DataGridView;
            if (grid != null)
            {
                grid.Enabled = isReadOnly;
            }
        }

        #endregion

        #region العمليات السيادية (CRUD) المربوطة بالأب

        // 🎯 الالتزام بالدستور: زر الإضافة
        public override void OnNew()
        {
            try
            {
                // استخدام AddNew بدلاً من ClearForm للحفاظ على سلامة الربط مع الداتا سيت

                // توليد الرقم التلقائي
                var tb = this.Controls.Find("docType_IDTextBox", true).FirstOrDefault() as TextBox;

                // إبلاغ الأب بفتح الشاشة في وضع جديد
                ChangeFormMode(FormMode.New);

                // وضع المؤشر في حقل اسم المستند (افترضنا أن اسمه doc_NameTextBox)
                var nameTb = this.Controls.Find("doc_NameTextBox", true).FirstOrDefault() as TextBox;
                if (nameTb != null) nameTb.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ أثناء التحضير للإضافة: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 🎯 الالتزام بالدستور: زر التعديل
        public override void OnEdit()
        {

            // فتح الحقول للتعديل على المستند الحالي
            ChangeFormMode(FormMode.Edit);
        }
        /*
        // 🎯 الالتزام بالدستور: محرك الحفظ الفعلي المربوط بالأب (F5)
        protected override bool ExecuteSaveToDatabase(SqlTransaction trans)
        {
            // 1. التحقق المبدئي من الحقول الإجبارية (قم بتعديل أسماء مربعات النص حسب الموجود لديك في التصميم)
            if (string.IsNullOrWhiteSpace(doc_Type_IDTextBox.Text) || string.IsNullOrWhiteSpace(doc_NameTextBox.Text))
            {
                MessageBox.Show("يجب إدخال رقم ونوع المستند.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            try
            {
                // 2. صياغة الاستعلام مع حقول التدقيق الزمنية والمستخدم
                string sqlQuery = (CurrentMode == FormMode.Edit)
                    ? @"UPDATE Doc_Types 
                        SET Doc_Name = @Doc_Name, Module_Name = @Module_Name, 
                            Updated_By = @Updated_By, Updated_At = @Updated_At 
                        WHERE Doc_Type_ID = @Doc_Type_ID"
                    : @"INSERT INTO Doc_Types 
                        (Doc_Type_ID, Doc_Name, Module_Name, Created_By, Created_At) 
                        VALUES (@Doc_Type_ID, @Doc_Name, @Module_Name, @Created_By, @Created_At)";

                // 3. تجهيز البارامترات الأساسية باستخدام List
                var pHeader = new System.Collections.Generic.List<SqlParameter>
                {
                    new SqlParameter("@Doc_Type_ID", doc_Type_IDTextBox.Text.Trim()),
                    new SqlParameter("@Doc_Name", doc_NameTextBox.Text.Trim()),
                    new SqlParameter("@Module_Name", string.IsNullOrWhiteSpace(module_NameTextBox.Text) ? (object)DBNull.Value : module_NameTextBox.Text.Trim())
                };

                // 4. حقن بيانات المستخدم والوقت أوتوماتيكياً من الجلسة
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
                    string oldValues = "تم الحفظ المسبق"; // مستقبلاً يمكن جلب JSON من הذاكرة المؤقتة
                    string newValues = $"الاسم: {doc_NameTextBox.Text.Trim()} | الموديول: {module_NameTextBox.Text.Trim()}";
                    DatabaseHelper.LogAuditTransaction(trans, "Doc_Types", doc_Type_IDTextBox.Text.Trim(), "UPDATE", oldValues, newValues, "تعديل أنواع المستندات");
                }

                // إرجاع نجاح للـ BaseEntryForm ليقوم بعمل Commit
                return true;
            }
            catch (Exception ex)
            {
                // رمي الخطأ ليتم التقاطه والتراجع (Rollback) في الـ BaseEntryForm
                throw new Exception($"خطأ أثناء حفظ نوع المستند: {ex.Message}");
            }
        }
                              */
        // 🎯 الالتزام بالدستور: زر التراجع (Esc)
        public override void OnCancel()
        {
            if (MessageBox.Show("هل أنت متأكد من إلغاء العملية والتراجع؟", "تأكيد", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                ChangeFormMode(FormMode.View);
            }
        }

        // 🎯 الالتزام بالدستور: زر الحذف (F4)
        public override void OnDelete()
        {
            {
                MessageBox.Show("يرجى اختيار نوع مستند واستعراضه قبل محاولة حذفه.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show("هل تريد حذف نوع المستند المحدد نهائياً؟", "تأكيد الحذف", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    MessageBox.Show("تم حذف نوع المستند بنجاح.", "تم", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (System.Data.SqlClient.SqlException)
                {
                    // اصطياد أخطاء الارتباط (Foreign Key)
                    MessageBox.Show("لا يمكن حذف هذا المستند لارتباطه بعمليات أخرى مسجلة في النظام.", "حماية النظام", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("فشل الحذف: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        #endregion
    }
}