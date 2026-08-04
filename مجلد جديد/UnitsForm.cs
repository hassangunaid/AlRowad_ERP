using System;
using System.Windows.Forms;
using System.Data; // تم إضافتها لدعم DataRowView و DataView
using AlRowad_ERP.Core;

namespace AlRowad_ERP.Forms
{
    public partial class UnitsForm : BaseEntryForm
    {
        public UnitsForm()
        {
            InitializeComponent();
            SetupFormDesign();
        }

        private void SetupFormDesign()
        {
            this.Text = "شاشة تعريف وحدات القياس";

            // قفل الحقول عند فتح الشاشة لأول مرة بناءً على دستور الرواد
            SetState(false);
        }

        private void UnitsForm_Load(object sender, EventArgs e)
        {
            // 1. تعبئة البيانات إلى الـ DataSet لكي تظهر في الجدول كالمعتاد
            this.unitsTableAdapter.Fill(this.alRowad_ERPDataSet.Units);

            unit_IDTextBox.ReadOnly = true;

            // 2. تفريغ الحقول عند أول فتح للشاشة لمنع ارتكازها على السجل الأول
            ClearFields();

            // 3. ربط حدث النقر المزدوج للجدول برمجياً (تأكد من اسم الـ DataGridView لديك، هنا افتراضاً اسمه unitsDataGridView)
            // إذا كنت قد ربطت الحدث من شاشة التصميم، يمكنك حذف السطر التالي
            if (this.unitsDataGridView != null)
            {
                this.unitsDataGridView.CellDoubleClick += UnitsDataGridView_CellDoubleClick;
            }
        }

        // دالة مساعدة لتفريغ مربعات النص بالكامل
        private void ClearFields()
        {
            unit_IDTextBox.Text = "";
            unit_NameTextBox.Text = ""; // استبدل unit_NameTextBox باسم حقل اسم الوحدة الحقيقي لديك
            // أي حقول أخرى ضعها هنا مثل: txtNotes.Text = "";
        }

        // حدث النقر المزدوج لنقل البيانات يدوياً من الجدول إلى الحقول
        private void UnitsDataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // التأكد من أن النقر تم على صف بيانات حقيقي وليس العناوين
            if (e.RowIndex >= 0)
            {
                // فتح الحقول برمجياً للسماح بالتعديل (تفعيل عناصر الشاشة بناءً على نظام الـ BaseForm الخاص بك)
                SetState(true);

                // الحصول على الصف المنقور عليه
                DataGridViewRow row = unitsDataGridView.Rows[e.RowIndex];

                // نقل القيم برمجياً (تأكد من مطابقة أسماء الأعمدة داخل الـ DataGridView)
                unit_IDTextBox.Text = row.Cells["Unit_ID"].Value?.ToString();
                unit_NameTextBox.Text = row.Cells["Unit_Name"].Value?.ToString(); // استبدل الاسم بما يناسب جدولك
            }
        }

        // إعطاء أوامر حقيقية لأزرار الأب عند الضغط عليها
        public override void OnNew()
        {
            base.OnNew(); // يفتح الحقول ويهيئ الأزرار

            try
            {
                // 1. تفريغ الحقول أولاً لتجهيزها للإدخال الجديد
                ClearFields();

                // 2. الوصول إلى الـ DataTable المحملة بالبيانات
                var table = this.alRowad_ERPDataSet.Units;

                // 3. حساب الرقم التلقائي القادم (أعلى رقم في العمود + 1)
                int nextId = 1;
                object maxId = table.Compute("MAX(Unit_ID)", "");

                if (maxId != DBNull.Value && maxId != null)
                {
                    nextId = Convert.ToInt32(maxId) + 1;
                }

                // 4. إظهار الرقم التلقائي الجديد فوراً في الـ TextBox المقفل
                unit_IDTextBox.Text = nextId.ToString();

                // 5. نقل التركيز إلى حقل الاسم لتبدأ الكتابة مباشرة
                unit_NameTextBox.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"خطأ في إنشاء سجل جديد: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                base.OnCancel();
            }
        }

        public override void OnSave()
        {
            try
            {
                // بما أننا ألغينا الـ Binding التلقائي، سنقوم بإنشاء الصف وحفظه يدوياً في الـ DataTable
                var table = this.alRowad_ERPDataSet.Units;

                // التحقق هل هو سجل جديد تماماً أم تعديل لسجل حالي؟
                DataRow[] existingRows = table.Select($"Unit_ID = {unit_IDTextBox.Text}");

                if (existingRows.Length > 0)
                {
                    // حالة تعديل سجل موجود مسبقاً
                    existingRows[0]["Unit_Name"] = unit_NameTextBox.Text;
                }
                else
                {
                    // حالة إضافة سجل جديد تماماً
                    DataRow newRow = table.NewRow();
                    newRow["Unit_ID"] = Convert.ToInt32(unit_IDTextBox.Text);
                    newRow["Unit_Name"] = unit_NameTextBox.Text;
                    newRow["conversion_Factor"] = conversion_FactorTextBox.Text;
                    table.Rows.Add(newRow);
                }

                // حفظ البيانات في قاعدة البيانات
                this.tableAdapterManager.UpdateAll(this.alRowad_ERPDataSet);

                // إعادة تحميل البيانات لتحديث الجدول
                this.unitsTableAdapter.Fill(this.alRowad_ERPDataSet.Units);

                MessageBox.Show("تم حفظ البيانات بنجاح!", "تأكيد", MessageBoxButtons.OK, MessageBoxIcon.Information);

                ClearFields(); // تفريغ الحقول بعد الحفظ بنجاح
                base.OnSave(); // يقفل الحقول ويعيد الأزرار لحالتها
            }
            catch (Exception ex)
            {
                MessageBox.Show($"خطأ في حفظ البيانات: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public override void OnCancel()
        {
            try
            {
                // تفريغ الحقول وإعادة تحميل البيانات لتطهير الشاشة
                ClearFields();
                this.unitsTableAdapter.Fill(this.alRowad_ERPDataSet.Units);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"خطأ في إلغاء التعديلات: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            base.OnCancel(); // يتراجع عن التعديلات ويقفل الحقول
        }

        private void unitsBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.OnSave(); // توجيه زر شريط الأدوات الافتراضي ليعمل مع نظام الـ Save المعدل لدينا
        }

        // معالج حدث شريط التحكم
        private void alRowadToolBar(object sender, EventArgs e)
        {
            // لا يوجد أي عملية مطلوبة عند تحميل شريط التحكم
            // يتم ربط الأزرار تلقائياً مع الـ Overrides للفئة الأساسية BaseEntryForm
        }
    }
}
