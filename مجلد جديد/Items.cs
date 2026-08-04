using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AlRowad_ERP.Core;

namespace AlRowad_ERP.Forms
{
    public partial class Items : BaseEntryForm
    {
        public Items()
        {
            InitializeComponent();
            SetupFormDesign();
        }

        private void SetupFormDesign()
        {
            this.Text = "شاشة بيانات الاصناف";

            // قفل الحقول عند فتح الشاشة لأول مرة بناءً على دستور الرواد
            SetState(false);
        }

        private void ItemsForm_Load(object sender, EventArgs e)
        {
            // تحميل البيانات من قاعدة البيانات
            // TODO: إضافة كود تحميل البيانات هنا
            this.itemsTableAdapter.Fill(alRowad_ERPDataSet.Items);

            item_IDTextBox.ReadOnly = true;
            ClearFields();

            if (itemsDataGridView != null)
            {
                itemsDataGridView.CellDoubleClick += itemsDataGridView_celldoubleclick;


            }
        }
        private void itemsDataGridView_celldoubleclick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                SetState(true);
                DataGridViewRow row = itemsDataGridView.Rows[e.RowIndex];
                item_IDTextBox.Text = row.Cells
                  ["Item_ID"].Value?.ToString();
                item_NameTextBox.Text = row.Cells
                  ["Item_Name"].Value?.ToString();
                base_Unit_IDTextBox.Text = row.Cells
                  ["base_Unit_ID"].Value?.ToString();
                default_PriceTextBox.Text = row.Cells
                  ["default_Price"].Value?.ToString();

            }
        }
        private void ClearFields()
        {
            item_IDTextBox.Text = "";
            item_NameTextBox.Text = "";
            default_PriceTextBox.Text = "";
            // استبدل Item_NameTextBox باسم حقل اسم الوحدة الحقيقي لديك
            // أي حقول أخرى ضعها هنا مثل: txtNotes.Text = "";
        }

        public override void OnCancel()
        {
            try
            {
                // تفريغ الحقول وإعادة تحميل البيانات لتطهير الشاشة
                ClearFields();
                this.itemsTableAdapter.Fill(this.alRowad_ERPDataSet.Items);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"خطأ في إلغاء التعديلات: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            base.OnCancel(); // يتراجع عن التعديلات ويقفل الحقول
        }

        private void itemsBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.itemsBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.alRowad_ERPDataSet);

        }


        public override void OnNew()
        {
            base.OnNew(); // يفتح الحقول ويهيئ الأزرار

            try
            {
                // 1. تفريغ الحقول أولاً لتجهيزها للإدخال الجديد
                ClearFields();

                // 2. الوصول إلى الـ DataTable المحملة بالبيانات
                var table = this.alRowad_ERPDataSet.Items;

                // 3. حساب الرقم التلقائي القادم (أعلى رقم في العمود + 1)
                int nextId = 1;
                object maxId = table.Compute("MAX(Item_ID)", "");

                if (maxId != DBNull.Value && maxId != null)
                {
                    nextId = Convert.ToInt32(maxId) + 1;
                }

                // 4. إظهار الرقم التلقائي الجديد فوراً في الـ TextBox المقفل
                item_IDTextBox.Text = nextId.ToString();

                // 5. نقل التركيز إلى حقل الاسم لتبدأ الكتابة مباشرة
                item_NameTextBox.Focus();
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
                DataRow[] existingRows = table.Select($"Item_ID = {item_IDTextBox.Text}");

                if (existingRows.Length > 0)
                {
                    // حالة تعديل سجل موجود مسبقاً
                    existingRows[0]["Item_Name"] = item_NameTextBox.Text;
                }
                else
                {
                    // حالة إضافة سجل جديد تماماً
                    DataRow newRow = table.NewRow();
                    newRow["Item_ID"] = Convert.ToInt32(item_IDTextBox.Text);
                    newRow["Item_Name"] = item_NameTextBox.Text;
                    newRow["base_Unit_ID"] = base_Unit_IDTextBox.Text;
                    newRow["default_Price"] = default_PriceTextBox.Text;
                    table.Rows.Add(newRow);
                }

                // حفظ البيانات في قاعدة البيانات
                this.tableAdapterManager.UpdateAll(this.alRowad_ERPDataSet);

                // إعادة تحميل البيانات لتحديث الجدول
                this.itemsTableAdapter.Fill(this.alRowad_ERPDataSet.Items);

                MessageBox.Show("تم حفظ البيانات بنجاح!", "تأكيد", MessageBoxButtons.OK, MessageBoxIcon.Information);

                ClearFields(); // تفريغ الحقول بعد الحفظ بنجاح
                base.OnSave(); // يقفل الحقول ويعيد الأزرار لحالتها
            }
            catch (Exception ex)
            {
                MessageBox.Show($"خطأ في حفظ البيانات: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}


