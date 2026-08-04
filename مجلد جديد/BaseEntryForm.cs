using AlRowad_ERP.Controls; // تأكد من مطابقة مسار الأداة لديك
using AlRowad_ERP.HelpForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace AlRowad_ERP.Core
{
    public class BaseEntryForm : BaseForm // أو BaseForm إذا كان لديك كلاس أساسي آخر
    {
        protected AlRowadToolBar MainToolBar { get; private set; }
        protected string EntrySource { get; set; } = "None";
        private Dictionary<string, string> _originalValues = new Dictionary<string, string>();
        private string nextId;

        public BaseEntryForm()
        {
            this.KeyPreview = true;
            this.Load += BaseEntryForm_Load;
        }
        // يُكتب هذا الكود داخل كلاس الأب BaseEntryForm
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            // عند ضغط F2 يتم استدعاء دالة الإضافة المورّثة
            if (keyData == Keys.F2)
            {
                OnNew();
                return true;
            }

            // عند ضغط F5 يتم استدعاء دالة الحفظ المورّثة
            if (keyData == Keys.F5)
            {
                OnSave();
                return true;
            }

            // عند ضغط F4 يتم استدعاء دالة الحذف المورّثة
            if (keyData == Keys.F4)
            {
                OnDelete();
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }
        protected void SaveAllowedCurrencies(int accId, CheckedListBox clbCurrencies)
        {
            // حظر المصمم لمنع انهيار شاشة الحسابات
            if (this.DesignMode || System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime)
            {
                return;
            }

            if (clbCurrencies == null) return;

            try
            {
                string deleteOldCurrencies = "DELETE FROM Account_Allowed_Currencies WHERE Acc_ID = @Acc_ID";
                using (SqlCommand cmdDel = new SqlCommand(deleteOldCurrencies, DatabaseHelper.GetConnection()))
                {
                    cmdDel.Parameters.AddWithValue("@Acc_ID", accId);
                    cmdDel.ExecuteNonQuery();
                }

                string insertCurrencies = "INSERT INTO Account_Allowed_Currencies (Acc_ID, Cur_ID) VALUES (@Acc_ID, @Cur_ID)";

                foreach (var item in clbCurrencies.CheckedItems)
                {
                    int currentCurId = 0;
                    if (item is DataRowView checkedItem)
                    {
                        currentCurId = Convert.ToInt32(checkedItem["Cur_ID"]);
                    }
                    else
                    {
                        continue;
                    }

                    using (SqlCommand cmdIns = new SqlCommand(insertCurrencies, DatabaseHelper.GetConnection()))
                    {
                        cmdIns.Parameters.AddWithValue("@Acc_ID", accId);
                        cmdIns.Parameters.AddWithValue("@Cur_ID", currentCurId);
                        cmdIns.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ في الأب أثناء حفظ العملات المسموحة: " + ex.Message, "خطأ في النظام", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                }
        }

        // باقي دوال كلاس الأب الأخرى تظل كما هي دون تغيير...
    
        private void BaseEntryForm_Load(object sender, EventArgs e)
        {
            MainToolBar = FindToolBar(this);
            if (MainToolBar != null)
            {
                MainToolBar.btn_AddFrom.Click += (s, ev) => OnAddFrom();
                MainToolBar.btn_New.Click += (s, ev) => OnNew();
                MainToolBar.btn_Edit.Click += (s, ev) => OnEdit();
                MainToolBar.btn_Save.Click += (s, ev) => OnSave();
                MainToolBar.btn_Cancel.Click += (s, ev) => OnCancel();
                MainToolBar.btn_Search.Click += (s, ev) => OnSearch();
                MainToolBar.btn_Print.Click += (s, ev) => OnPrint();
                MainToolBar.btn_Delete.Click += (s, ev) => OnDelete();
                MainToolBar.btn_Close.Click += (s, ev) => OnClose();
            }
            SetState(false);
        }

        private AlRowadToolBar FindToolBar(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                if (c is AlRowadToolBar toolBar) return toolBar;
                if (c.HasChildren)
                {
                    var found = FindToolBar(c);
                    if (found != null) return found;
                }
            }
            return null;
        }

        // --- محرك التحقق الإلزامي ---
        protected virtual bool ValidateFields(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                if (c.Tag != null && c.Tag.ToString() == "Required")
                {
                    if (c is TextBox txt && string.IsNullOrWhiteSpace(txt.Text))
                    {
                        MessageBox.Show($"حقل {txt.Name.Replace("txt_", "")} مطلوب.", "تنبيه الرواد", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txt.Focus();
                        return false;
                    }
                }
                if (c.HasChildren && !ValidateFields(c)) return false;
            }
            return true;
        }

        public virtual void OnNew()

        {
            EntrySource = "New"; ClearForm(this);
            string nextid = Getnextid();
            var idField = this.Controls.Find("txt_ID", true).FirstOrDefault() as TextBox;
            if (idField != null) idField.Text = nextId;
            SetState(true);
        }

        private string Getnextid()
        {
            return "0";
        }

        public virtual void OnEdit() { EntrySource = "Edit"; CaptureOriginalValues(this); SetState(true); }
        // 1. الدالة الافتراضية التي ستقوم كل شاشة فرعية بكتابة كود الحفظ الفعلي بداخلها
        // جعلناها ترجع true إذا نجح الحفظ في قاعدة البيانات و false إذا فشل
        protected virtual bool ExecuteSaveToDatabase()
        {
            return true;
        }


        // 2. 🚀 دالة الحفظ المركزية السيادية في الأب (تحتوي على الحماية ورسالة الخطأ لجميع الشاشات)


        // 🚀 دالة الحفظ المركزية السيادية في الأب (تحتوي على حماية الـ SQL لجميع الشاشات)
        public virtual void OnSave()
        {
            // 1. أولاً: فحص الحقول الإلزامية العامة في الشاشة
            if (!ValidateFields(this)) return;

            try
            {
                // 2. ثانياً: استدعاء كود الحفظ الفعلي الخاص بالشاشة الفرعية
                bool isSavedSuccessfully = ExecuteSaveToDatabase();

                // 3. إذا نجح الحفظ وبدون استثناءات، نقوم بإرجاع حالة الشاشة لوضع الاستعراض
                if (isSavedSuccessfully)
                {
                    // قمنا بنقل رسالة النجاح إلى هنا لتصبح موحدة
                    MessageBox.Show("تم حفظ البيانات بنجاح تام.", "تأكيد النظام", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    EntrySource = "None";
                    _originalValues.Clear();
                    SetState(false);
                }
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                // 🛑 هنا اصطياد الخطأ الصارم في الأب ليطبق على كامل النظام تلقائياً!
                if (ex.Number == 2627 || ex.Number == 2601)
                {
                    MessageBox.Show("عذراً، هذا الاسم أو المعرّف مسجل مسبقاً في النظام!\nقاعدة البيانات تمنع التكرار لضمان سلامة البيانات والتقارير المالية.",
                                    "تنبيه منع التكرار", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show($"حدث خطأ في قاعدة البيانات أثناء الحفظ:\n{ex.Message}", "خطأ في النظام", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"حدث خطأ عام غير متوقع:\n{ex.Message}", "خطأ حرج", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        protected virtual void LoadDataById(string id)
        {
            EntrySource = id;
        }

        public virtual void OnCancel()
        {
            // 🛑 إضافة رسالة التحذير هنا في الأب لتطبق على كامل النظام تلقائياً
            DialogResult result = MessageBox.Show(
                "هل أنت متأكد من إلغاء كافة التغييرات الحالية والتراجع؟",
                "تأكيد التراجع",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2 // خيار "لا" هو الافتراضي لحماية البيانات
            );

            // إذا اختار المستخدم "لا" نوقف التراجع فوراً ونحافظ على ما كتبه
            if (result == DialogResult.No) return;

            // إذا اختار "نعم" ينفذ النظام منطق الأب الافتراضي المقنن
            if (EntrySource == "New" || EntrySource == "AddFrom")
            {
                ClearForm(this);
            }
            else if (EntrySource == "Edit")
            {
                RestoreOriginalValues(this);
            }

            EntrySource = "None";
            SetState(false);
        }
        //حالة الازرار 
        protected virtual string PrimaryIdFieldName => "";

        protected virtual void SetState(bool editing)
        {
            bool isEmpty = IsPrimaryIdEmpty(this);
            if (MainToolBar != null)
            {
                MainToolBar.btn_New.Enabled = !editing;
                MainToolBar.btn_Edit.Enabled = !isEmpty;
                MainToolBar.btn_Save.Enabled = editing;
                MainToolBar.btn_Cancel.Enabled = editing;
                MainToolBar.btn_Delete.Enabled = !editing && !isEmpty;
                MainToolBar.btn_Search.Enabled = !editing;
                MainToolBar.btn_AddFrom.Enabled = !editing && !isEmpty;
            }
            LockControls(this, editing);
        }

        // تحديث دالة قفل العناصر لتصبح متداخلة (Recursive) وتستثني شريط الأدوات
        private void LockControls(Control parent, bool editing)
        {
            foreach (Control c in parent.Controls)
            {
                // تخطي شريط الأدوات الرئيسي حتى لا يقفل أزراره!
                if (c == MainToolBar) continue;

                // قفل وفتح أدوات الإدخال بناءً على حالة التعديل
                if (c is TextBox || c is ComboBox || c is CheckBox || c is DateTimePicker || c is DataGridView || c is CheckedListBox)
                {
                    c.Enabled = editing;
                }

                // الغوص بشكل متداخل وعميق داخل الحاويات والألواح (GroupBox, Panel, TabControl)
                if (c.HasChildren)
                {
                    LockControls(c, editing);
                }
            }
        }
        private bool IsPrimaryIdEmpty(Control parent)
        {
            TextBox idField = null;
            if (!string.IsNullOrEmpty(PrimaryIdFieldName))
            {
                idField = FindControlByName(parent, PrimaryIdFieldName) as TextBox;
            }

            if (idField == null)
            {
                idField = FindTextBoxEndingWithId(parent);
            }
            return idField == null ? true : (string.IsNullOrWhiteSpace(idField.Text) || idField.Text.Trim() == "0");
        }
        private Control FindControlByName(Control parent, string name)
        {
            foreach (Control c in parent.Controls)
            {
                if (c.Name.Equals(name, StringComparison.OrdinalIgnoreCase)) return c;
                if (c.HasChildren)
                {
                    var found = FindControlByName(c, name);
                    if (found != null) return found;
                }
            }
            return null;
        }
        private TextBox FindTextBoxEndingWithId(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                if (c is TextBox txt && txt.Name.EndsWith("_ID", StringComparison.OrdinalIgnoreCase))
                {
                    return txt;
                }
                if (c.HasChildren)
                {
                    var found = FindTextBoxEndingWithId(c);
                    if (found != null) return found;
                }
            }
            return null;
        }

        // داخل ملف Core/BaseEntryForm.cs
        protected void ClearForm(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                if (c is TextBox t) t.Clear();
                if (c is ComboBox cb) cb.SelectedIndex = -1;
                if (c is CheckBox chk) chk.Checked = false;
                if (c.HasChildren) ClearForm(c);
            }
        }
        private void CaptureOriginalValues(Control parent) { /* ... منطق الحفظ في القاموس ... */ }
        private void RestoreOriginalValues(Control parent) { /* ... منطق الاستعادة ... */ }

        public virtual void OnAddFrom()
        {



        }
        public virtual void OnDelete() { }
        public virtual void OnSearch()
        {
            // يفتح شاشة البحث الشاملة - يمكن تخصيصها في الفئات المشتقة
            try
            {
                UniversalSearchForm searchForm = new UniversalSearchForm("البحث", "");
                searchForm.ShowDialog(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ في فتح نافذة البحث: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public virtual void OnPrint() { }
        public virtual void OnClose()
        {
            // 🎯 فحص جودة البيانات وحالة الشاشة قبل المغادرة (دستور الرواد الأمني)
            if (EntrySource == "New" || EntrySource == "Edit" || EntrySource == "AddFrom")
            {
                // حظر الإغلاق الفوري وإجبار المستخدم على إنهاء الحفظ أو التراجع
                MessageBox.Show("تنبيه أمني: لا يمكنك مغادرة الشاشة حتى اكتمال الإجراءات الحالية!\n" +
                                "يرجى حفظ البيانات أولاً (F5) أو الضغط على زر التراجع لإلغاء العمليات.",
                                "حظر مغادرة الشاشة", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 🌟 في حال كانت الشاشة في وضع الاستعراض الصافي (Browse Mode) ولم يجرِ أي إدخال
            DialogResult result = MessageBox.Show("هل تود مغادرة الشاشة وإغلاق النافذة الحالية؟",
                                                  "تأكيد الخروج",
                                                  MessageBoxButtons.YesNo,
                                                  MessageBoxIcon.Question,
                                                  MessageBoxDefaultButton.Button2); // خيار "لا" هو الافتراضي للأمان

            if (result == DialogResult.Yes)
            {
                this.Close(); // الإغلاق الآمن للنافذة
            }
        }
        /// <summary>
        /// 1. فحص مستوى العملة (العام): التحقق من أن العملة نشطة في النظام ككل
        /// </summary>
        protected bool التحقق_من_نشاط_العملة_عام(int curId)
        {
            if (this.DesignMode || System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime) return true;

            string query = "SELECT Is_Active FROM Currencies WHERE Cur_ID = @Cur_ID";
            try
            {
                using (SqlCommand cmd = new SqlCommand(query, DatabaseHelper.GetConnection()))
                {
                    cmd.Parameters.AddWithValue("@Cur_ID", curId);
                    object result = cmd.ExecuteScalar();
                    return result != null && Convert.ToBoolean(result);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"خطأ في فحص نشاط العملة العام: {ex.Message}");
                return false;
            }
            finally { }
        }

        /// <summary>
        /// 2. فحص مستوى الحساب (الخاص): التحقق من أن العملة مسموحة ونشطة لهذا الحساب بالذات
        /// </summary>
        protected bool التحقق_من_نشاط_عملة_الحساب(int accId, int curId)
        {
            if (this.DesignMode || System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime) return true;

            // الاستعلام يفحص وجود الارتباط وأن حقل النشاط المشترك يساوي 1
            string query = @"SELECT COUNT(1) 
                             FROM Account_Allowed_Currencies 
                             WHERE Acc_ID = @Acc_ID AND Cur_ID = @Cur_ID AND Is_Active = 1";
            try
            {
                using (SqlCommand cmd = new SqlCommand(query, DatabaseHelper.GetConnection()))
                {
                    cmd.Parameters.AddWithValue("@Acc_ID", accId);
                    cmd.Parameters.AddWithValue("@Cur_ID", curId);
                    return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"خطأ في فحص نشاط عملة الحساب: {ex.Message}");
                return false;
            }
            finally { }
        }
    }
}
    
