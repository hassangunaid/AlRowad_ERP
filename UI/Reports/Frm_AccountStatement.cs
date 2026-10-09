using AlRowad_ERP.Core;
using AlRowad_ERP.Data.Reports;
using AlRowad_ERP.HelpForms;
using AlRowad_ERP.Models.Reports;
using AlRowad_ERP.UI.Base; // مسار BaseForm الخاص بك
using AlRowad_ERP.UI.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace AlRowad_ERP.UI.Reports
{
    // الالتزام بالهيكلية الوراثية
    public partial class Frm_AccountStatement : BaseEntryForm
    {
        private readonly ReportAccountRepository _reportRepo;

        public Frm_AccountStatement()
        {
            InitializeComponent();
            _reportRepo = new ReportAccountRepository();

            // تهيئة الجريد ليكون مرناً للمستخدم (النوع الأول)
            if (dgvStatement != null)
            {
                dgvStatement.AllowUserToOrderColumns = true;
                dgvStatement.ReadOnly = true;
                dgvStatement.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            }
        }

        // =========================================================
        // تجاوز حالة الشاشة عند التحميل (إلغاء قفل الكلاس الأب)
        // =========================================================
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // فك القفل عن الشاشة لأنها شاشة استعلام (تقرير) وليست إدخال
            LockControls(this, false);

            // 1. تفعيل خيار "كشف حساب قبل الترحيل" بشكل افتراضي
            if (chkIncludeUnposted != null) chkIncludeUnposted.Checked = true;

            // إعادة قفل الحقول التي يجب أن تبقى محمية
            if (txtAccountName != null) txtAccountName.ReadOnly = true;
            if (dgvStatement != null) dgvStatement.ReadOnly = true;

            // التركيز على حقل رقم الحساب لتسهيل بدء العمل
            if (txtAccountId != null) txtAccountId.Focus();
            // 2. ربط حدث المفاتيح (F9 و Enter) برمجياً من الكود مباشرة كما طلبت
            if (txtAccountId != null)
            {
                txtAccountId.KeyDown += txtAccountId_KeyDown;
            }
            if (btnLoadData != null)
            {
                btnLoadData.Click += btnLoadData_Click;
            }
            if (dtpFrom != null)
            {
                try
                {
                    // استعلام نظيف لجلب بداية الفترة المالية الحالية المفتوحة التي يقع فيها تاريخ اليوم
                    string query = "SELECT TOP 1 Start_Date FROM Financial_Periods WHERE Is_Closed = 0 AND @CurrentDate BETWEEN Start_Date AND End_Date";

                    // استخدام DatabaseHelper حسب الدستور (تفويض المهام)
                    // تجهيز مصفوفة البارامترات حسب معيار DatabaseHelper
                    SqlParameter[] parameters = {
    new SqlParameter("@CurrentDate", SqlDbType.DateTime) { Value = DateTime.Now.Date }
};

                    var startDateObj = DatabaseHelper.ExecuteScalar(query, parameters);
                    if (startDateObj != null && startDateObj != DBNull.Value)
                    {
                        dtpFrom.DateValue = Convert.ToDateTime(startDateObj);
                    }
                    else
                    {
                        // قيمة احتياطية (Fallback) في حال نسي مدير النظام تعريف فترة مالية للعام الجديد
                        dtpFrom.DateValue = new DateTime(DateTime.Now.Year, 1, 1);
                    }
                }
                catch (Exception ex)
                {
                    DatabaseHelper.LogSystemError(ex.Message, ex.StackTrace, "Load_Financial_Period");
                    dtpFrom.DateValue = new DateTime(DateTime.Now.Year, 1, 1);
                }
            }
        }
        // =========================================================
        // حدث الضغط على المفاتيح (استدعاء دليل الحسابات)
        // =========================================================
        private void txtAccountId_KeyDown(object sender, KeyEventArgs e)
        {
            // تم تفويض F9 للدالة السيادية OnF9Pressed
            // نتعامل هنا فقط مع مفتاح Enter للانتقال للحقل التالي
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                SendKeys.Send("{TAB}");
            }
        }
        // =========================================================
        // التوافق مع الدستور: استخدام العملية السيادية الموروثة من الكلاس الأب
        // =========================================================
        // =========================================================
        // التوافق مع الدستور: استخدام العملية السيادية الموروثة من الكلاس الأب
        // =========================================================
        internal override void OnF9Pressed()
        {
            base.OnF9Pressed();

            // جلب الأداة النشطة الفعلية (حتى لو كانت داخل GroupBox أو Panel)
            Control currentActiveControl = GetActiveControlDeep(this);

            // التحقق من أن المؤشر يقف حالياً على حقل رقم الحساب
            if (currentActiveControl != null && currentActiveControl.Name == "txtAccountId")
            {
                string accountQuery = "SELECT Acc_ID AS [رقم الحساب], Acc_Name AS [اسم الحساب] FROM Accounts";

                using (var searchForm = new AlRowad_ERP.HelpForms.UniversalSearchForm("دليل الحسابات", accountQuery))
                {
                    if (searchForm.ShowDialog() == DialogResult.OK)
                    {
                        txtAccountId.Text = searchForm.المعرف_المختار;
                        txtAccountName.Text = searchForm.الاسم_المختار;

                        // نقل التركيز آلياً إلى حقل التاريخ لتسريع عمل المستخدم
                        if (dtpFrom != null) dtpFrom.Focus();
                    }
                }
            }
        }

        // =========================================================
        // 🚀 التقاط ضغطات المفاتيح بالقوة قبل الكلاس الأب (BaseEntryForm)
        // =========================================================
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            // 1. التحقق من مفتاح F9
            if (keyData == Keys.F9)
            {
                // جلب الأداة النشطة الفعلية
                Control currentActiveControl = GetActiveControlDeep(this);

                // إذا كنا نقف على مربع الحساب، نفذ الاستدعاء وأوقف الإرسال للأب
                if (currentActiveControl != null && currentActiveControl.Name == "txtAccountId")
                {
                    string accountQuery = "SELECT Acc_ID AS [رقم الحساب], Acc_Name AS [اسم الحساب] FROM Accounts";

                    using (var searchForm = new AlRowad_ERP.HelpForms.UniversalSearchForm("دليل الحسابات", accountQuery))
                    {
                        if (searchForm.ShowDialog() == DialogResult.OK)
                        {
                            txtAccountId.Text = searchForm.المعرف_المختار;
                            txtAccountName.Text = searchForm.الاسم_المختار;

                            if (dtpFrom != null) dtpFrom.Focus();
                        }
                    }
                    return true; // 🚨 هذا السطر مهم جداً: يعني "أنا تعاملت مع المفتاح، لا ترسله للأب"
                }
            }

            // 2. التحقق من مفتاح Enter للتنقل
            if (keyData == Keys.Enter)
            {
                Control currentActiveControl = GetActiveControlDeep(this);
                if (currentActiveControl != null && currentActiveControl.Name == "txtAccountId")
                {
                    SendKeys.Send("{TAB}");
                    return true; // 🚨 إيقاف الإرسال للأب
                }
            }

            // إذا لم يكن F9 ولم يكن Enter على حقل الحساب، مرر المفتاح للكلاس الأب ليتعامل معه كالمعتاد
            return base.ProcessCmdKey(ref msg, keyData);
        }

        // دالة مساعدة للوصول للأداة النشطة الحقيقية
        private Control GetActiveControlDeep(ContainerControl container)
        {
            Control activeControl = container.ActiveControl;
            while (activeControl is ContainerControl nestedContainer)
            {
                activeControl = nestedContainer.ActiveControl;
            }
            return activeControl;
        }
        // تطبيق السلاسة والأداء العالي عبر async
        private async void btnLoadData_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtAccountId.Text))
            {
                MessageBox.Show("يرجى اختيار الحساب المراد عرض كشفه.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // تطبيق إدارة الحالة المركزية (الدستور): قفل الشاشة لمنع الإدخال المزدوج أثناء التحميل
            LockControls(this, true);
            try
            {
                var data = await _reportRepo.GetStatementAsync(
                    txtAccountId.Text,
                    dtpFrom.DateValue, // استخدام الخاصية الصحيحة لأداتك المخصصة
                    dtpTo.DateValue,   // استخدام الخاصية الصحيحة لأداتك المخصصة
                    chkIncludeUnposted.Checked);

                // عرض البيانات في الجريد المرن ليتمكن المستخدم من ترتيبها كما يشاء
                dgvStatement.DataSource = data;

                if (data.Count == 0)
                {
                    MessageBox.Show("لا توجد حركات مالية لهذا الحساب في الفترة المحددة.", "معلومة", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"حدث خطأ أثناء جلب البيانات: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // فتح الأدوات بعد انتهاء التحميل
                LockControls(this, false);
                // إبقاء هذه الحقول للقراءة فقط بعد فك القفل
                if (txtAccountName != null) txtAccountName.ReadOnly = true;
                if (dgvStatement != null) dgvStatement.ReadOnly = true;
            }
        }

        // طباعة الجريد المرن (ما يراه المستخدم على الشاشة - ترتيب وتجميع)
        private void btnPrintGrid_Click(object sender, EventArgs e)
        {
            MessageBox.Show("سيتم طباعة الشبكة بنفس الترتيب الذي تراه الآن.");
        }

        // طباعة القالب الرسمي المحاسبي (RDLC شبيه الـ Access)
        private void btnPrintOfficial_Click(object sender, EventArgs e)
        {
            var currentData = dgvStatement.DataSource as List<AccountStatementDto>;
            if (currentData == null || currentData.Count == 0)
            {
                MessageBox.Show("لا توجد بيانات لطباعتها. يرجى استدعاء البيانات أولاً.");
                return;
            }

            MessageBox.Show("سيتم تمرير البيانات إلى قالب كشف الحساب الرسمي RDLC.");
        }
    }
}