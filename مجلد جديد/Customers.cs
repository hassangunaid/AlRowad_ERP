using AlRowad_ERP.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace AlRowad_ERP.Forms
{
    public partial class Customers : BaseEntryForm // الوراثة الشرعية من النخاع الشوكي للنظام
    {
        protected override string PrimaryIdFieldName => "cust_ID";

        public Customers()
        {
            InitializeComponent();

            // ربط الأحداث البصرية لضمان تشغيل الشاشة بمرونة
            this.Load += new System.EventHandler(this.Customers_Load);
            this.dgv_currencies.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_currencies_CellContentClick);
        }

        private void Customers_Load(object sender, EventArgs e)
        {
            // شحن القوائم والعملات مسبقاً كحساب جديد لتظهر الواجهة ممتلئة
            تحميل_جميع_العملات_في_الجدول();
            تهيئة_قائمة_الحسابات_الرئيسية();

            // الالتزام بالدستور: وضع الاستعراض والقفل الافتراضي فور الفتح
            EntrySource = "Browse";
            SetState(false);
        }

        // 🎯 تطويع دالة التحكم بالـ State لتنصاع تماماً لقوانين الأب الصارمة
        protected override void SetState(bool editing)
        {
            // 1. دع الأب يقفل ويفتح كافة الأدوات النصية والمنسدلة تلقائياً حيوياً
            base.SetState(editing);

            // 2. حظر حقل المعرف المفتاحي كود العميل ورقم الحساب من التعديل بوضع التعديل
            cust_ID.ReadOnly = true;
            acc_ID.ReadOnly = true;

            if (EntrySource == "Edit" && editing)
            {
                cust_ID.Enabled = false;
                acc_ID.Enabled = false;
            }
            else
            {
                cust_ID.Enabled = editing;
                acc_ID.Enabled = editing;
            }

            // 3. التحكم بجداول الحركة وتفاصيل العملات وفق رغبة المستخدم
            dgv_currencies.Enabled = editing;
            if (editing)
            {
                تهيئة_حالة_خلايا_الجدول_الافتراضية();
            }
        }

        private void تهيئة_قائمة_الحسابات_الرئيسية()
        {
            try
            {
                string query = @"SELECT Acc_ID, Acc_ID + ' - ' + Acc_Name AS Acc_Full_Name 
                                 FROM Accounts 
                                 WHERE Is_Stopped = 0 
                                   AND Acc_ID LIKE '1102%' 
                                   AND LEN(RTRIM(Acc_ID)) = 6
                                   AND Acc_ID != '110202'
                                 ORDER BY Acc_ID";

                using (SqlCommand cmd = new SqlCommand(query, DatabaseHelper.GetConnection()))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        if (cmb_parent_ID != null)
                        {
                            cmb_parent_ID.SelectedIndexChanged -= cmb_parent_ID_SelectedIndexChanged;

                            cmb_parent_ID.DataSource = dt;
                            cmb_parent_ID.DisplayMember = "Acc_Full_Name";
                            cmb_parent_ID.ValueMember = "Acc_ID";
                            cmb_parent_ID.SelectedIndex = -1;

                            cmb_parent_ID.SelectedIndexChanged += cmb_parent_ID_SelectedIndexChanged;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ أثناء تصفية وتثبيت حسابات العملاء: " + ex.Message, "خطأ فني", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally { }
        }

        private string توليد_كود_العميل_تلقائيا()
        {
            string newCode = "0001";
            try
            {
                string query = "SELECT MAX(CAST(Cust_ID AS INT)) FROM Customers";
                using (SqlCommand cmd = new SqlCommand(query, DatabaseHelper.GetConnection()))
                {
                    object result = cmd.ExecuteScalar();
                    if (result != DBNull.Value && result != null)
                    {
                        int maxCode = Convert.ToInt32(result);
                        newCode = (maxCode + 1).ToString("D4");
                    }
                }
            }
            catch (Exception ex) { System.Diagnostics.Debug.WriteLine("خطأ توليد كود العميل: " + ex.Message); }
            finally { }
            return newCode;
        }

        // ─── [دوال محرك الأزرار الموروثة من الأب] ───
        public override void OnNew()
        {
            EntrySource = "New";
            ClearForm(this);

            if (cmb_parent_ID != null) cmb_parent_ID.SelectedIndex = -1;

            تحميل_جميع_العملات_في_الجدول();
            cust_ID.Text = توليد_كود_العميل_تلقائيا();

            SetState(true);
            cust_Name.Focus();
        }

        public override void OnEdit()
        {
            if (string.IsNullOrWhiteSpace(cust_ID.Text))
            {
                MessageBox.Show("يرجى اختيار العميل المراد تعديله أولاً عن طريق شاشة البحث.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            EntrySource = "Edit";
            SetState(true);
            cust_Name.Focus();
        }

        public override void OnCancel()
        {
            base.OnCancel(); // استدعاء حماية التراجع الموحدة للأب
            تحميل_جميع_العملات_في_الجدول();
        }

        public override void OnSave()
        {
            // استدعاء دالة الأب السيادية التي ستحتضن دالة الـ ExecuteSaveToDatabase تلقائياً
            base.OnSave();
        }

        // 🎯 المحرك المركزي للحفظ والتعديل الفعلي المرتبط بقاعدة البيانات
        protected override bool ExecuteSaveToDatabase()
        {
            if (string.IsNullOrEmpty(cust_Name.Text) || string.IsNullOrEmpty(acc_ID.Text))
            {
                MessageBox.Show("تنبيه: يجب إدخال اسم العميل ورقم الحساب المالي المرتبط.", "منع الحفظ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            bool hasDefaultCurrency = false;
            foreach (DataGridViewRow row in dgv_currencies.Rows)
            {
                if (row.IsNewRow) continue;
                if (row.Cells[2].Value != null && Convert.ToBoolean(row.Cells[2].Value))
                {
                    hasDefaultCurrency = true;
                    break;
                }
            }

            if (!hasDefaultCurrency)
            {
                MessageBox.Show("تنبيه: يجب تحديد عملة افتراضية واحدة للعميل.", "منع الحفظ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            SqlTransaction transaction = DatabaseHelper.GetConnection().BeginTransaction();

            try
            {
                if (EntrySource == "New")
                {
                    // 🎯 فرض القيم المحاسبية الثابتة للعميل: Acc_Type=2 (فرعي)، Acc_Nature=1 (مدين)، Report_Type=1 (ميزانية)
                    string insertAccountQuery = @"INSERT INTO Accounts (Acc_ID, Acc_Name, Is_Stopped, Account_Level, Parent_ID, Acc_Type, Acc_Nature, Report_Type) 
                                         VALUES (@AccID, @AccName, 0, 5, @ParentID, 2, 1, 1)";

                    using (SqlCommand cmdAcc = new SqlCommand(insertAccountQuery, DatabaseHelper.GetConnection(), transaction))
                    {
                        cmdAcc.Parameters.AddWithValue("@AccID", acc_ID.Text.Trim());
                        cmdAcc.Parameters.Add("@AccName", SqlDbType.NVarChar).Value = "حساب العميل: " + cust_Name.Text.Trim();
                        cmdAcc.Parameters.AddWithValue("@ParentID", cmb_parent_ID.SelectedValue);
                        cmdAcc.ExecuteNonQuery();
                    }

                    string customerQuery = "INSERT INTO Customers (Cust_ID, Cust_Name, Cust_Phone, Cust_Address, Acc_ID) VALUES (@Code, @Name, @Phone, @Address, @AccNo)";
                    using (SqlCommand cmd = new SqlCommand(customerQuery, DatabaseHelper.GetConnection(), transaction))
                    {
                        cmd.Parameters.AddWithValue("@Code", cust_ID.Text.Trim());
                        cmd.Parameters.Add("@Name", SqlDbType.NVarChar).Value = cust_Name.Text.Trim();
                        cmd.Parameters.AddWithValue("@Phone", cust_Phone.Text.Trim());
                        cmd.Parameters.AddWithValue("@Address", cust_Address.Text.Trim());
                        cmd.Parameters.AddWithValue("@AccNo", acc_ID.Text.Trim());
                        cmd.ExecuteNonQuery();
                    }
                }
                else if (EntrySource == "Edit")
                {
                    // تحديث بيانات الحساب والعميل
                    string updateAccountQuery = "UPDATE Accounts SET Acc_Name = @AccName WHERE Acc_ID = @AccID";
                    using (SqlCommand cmdAcc = new SqlCommand(updateAccountQuery, DatabaseHelper.GetConnection(), transaction))
                    {
                        cmdAcc.Parameters.AddWithValue("@AccID", acc_ID.Text.Trim());
                        cmdAcc.Parameters.Add("@AccName", SqlDbType.NVarChar).Value = "حساب العميل: " + cust_Name.Text.Trim();
                        cmdAcc.ExecuteNonQuery();
                    }

                    string updateCustomerQuery = "UPDATE Customers SET Cust_Name = @Name, Cust_Phone = @Phone, Cust_Address = @Address WHERE Cust_ID = @Code";
                    using (SqlCommand cmd = new SqlCommand(updateCustomerQuery, DatabaseHelper.GetConnection(), transaction))
                    {
                        cmd.Parameters.AddWithValue("@Code", cust_ID.Text.Trim());
                        cmd.Parameters.Add("@Name", SqlDbType.NVarChar).Value = cust_Name.Text.Trim();
                        cmd.Parameters.AddWithValue("@Phone", cust_Phone.Text.Trim());
                        cmd.Parameters.AddWithValue("@Address", cust_Address.Text.Trim());
                        cmd.ExecuteNonQuery();
                    }
                }

                // مسح وحفظ العملات المسموحة بناءً على الحساب المالي الموحد
                string deleteCurrenciesQuery = "DELETE FROM Account_Allowed_Currencies WHERE Acc_ID = @AccID";
                using (SqlCommand cmdDel = new SqlCommand(deleteCurrenciesQuery, DatabaseHelper.GetConnection(), transaction))
                {
                    cmdDel.Parameters.AddWithValue("@AccID", acc_ID.Text.Trim());
                    cmdDel.ExecuteNonQuery();
                }

                string currencyQuery = "INSERT INTO Account_Allowed_Currencies (Acc_ID, Cur_ID, Is_Active, Is_Frozen, Is_Default) VALUES (@AccID, @CurID, @IsActive, @IsFrozen, @IsDefault)";
                foreach (DataGridViewRow row in dgv_currencies.Rows)
                {
                    if (row.IsNewRow) continue;
                    bool isActive = Convert.ToBoolean(row.Cells[0].Value);
                    bool isDefault = Convert.ToBoolean(row.Cells[2].Value);
                    bool isFrozen = Convert.ToBoolean(row.Cells[3].Value);
                    string curId = row.Cells[4].Value?.ToString();

                    if ((isActive || isDefault || isFrozen) && !string.IsNullOrEmpty(curId))
                    {
                        using (SqlCommand cmdCur = new SqlCommand(currencyQuery, DatabaseHelper.GetConnection(), transaction))
                        {
                            cmdCur.Parameters.AddWithValue("@AccID", acc_ID.Text.Trim());
                            cmdCur.Parameters.AddWithValue("@CurID", Convert.ToInt32(curId.Trim()));
                            cmdCur.Parameters.AddWithValue("@IsActive", isActive);
                            cmdCur.Parameters.AddWithValue("@IsFrozen", isFrozen);
                            cmdCur.Parameters.AddWithValue("@IsDefault", isDefault);
                            cmdCur.ExecuteNonQuery();
                        }
                    }
                }

                transaction.Commit();
                EntrySource = "Browse";
                SetState(false);
                return true;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                MessageBox.Show($"حدث خطأ حرج: {ex.Message}", "خطأ نظام", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            finally { }
        }
        public override void OnDelete()
        {
            if (string.IsNullOrEmpty(cust_ID.Text) || EntrySource != "Browse")
            {
                MessageBox.Show("تنبيه: يجب استعراض أو اختيار العميل المراد حذفه أولاً.", "منع الحذف", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show($"هل أنت متأكد تماماً من رغبتك في حذف العميل المختار: ({cust_Name.Text}) وحسابه المالي المرتبط نهائياً؟",
                                                  "تأكيد الحذف", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

            if (result == DialogResult.Yes)
            {
                ExecuteDelete(cust_ID.Text.Trim(), acc_ID.Text.Trim());
            }
        }

        // 🎯 دالة الحذف التنفيذية المكتملة لتكامل شريط الأدوات القياسي
        private void ExecuteDelete(string customerCode, string accountCode)
        {
            SqlTransaction transaction = DatabaseHelper.GetConnection().BeginTransaction();

            try
            {
                // ابحث عن هذا السطر داخل دالة ExecuteDelete واستبدله كالتالي:
                string deleteCurrenciesQuery = "DELETE FROM Account_Allowed_Currencies WHERE Acc_ID = @AccID";
                using (SqlCommand cmdCur = new SqlCommand(deleteCurrenciesQuery, DatabaseHelper.GetConnection(), transaction))
                {
                    cmdCur.Parameters.AddWithValue("@AccID", accountCode.Trim()); // الربط البرمجي بكود الحساب المالي الممرر للدالة
                    cmdCur.ExecuteNonQuery();
                }
                string deleteCustomerQuery = "DELETE FROM Customers WHERE Cust_ID = @Code";
                using (SqlCommand cmdCust = new SqlCommand(deleteCustomerQuery, DatabaseHelper.GetConnection(), transaction))
                {
                    cmdCust.Parameters.AddWithValue("@Code", customerCode);
                    cmdCust.ExecuteNonQuery();
                }

                if (!string.IsNullOrEmpty(accountCode))
                {
                    string deleteAccountQuery = "DELETE FROM Accounts WHERE Acc_ID = @AccID";
                    using (SqlCommand cmdAcc = new SqlCommand(deleteAccountQuery, DatabaseHelper.GetConnection(), transaction))
                    {
                        cmdAcc.Parameters.AddWithValue("@AccID", accountCode);
                        cmdAcc.ExecuteNonQuery();
                    }
                }

                transaction.Commit();
                MessageBox.Show("تم حذف العميل وصلاحيات العملات والحساب المرتبط به بنجاح تام من النظام.", "تأكيد الحذف", MessageBoxButtons.OK, MessageBoxIcon.Information);

                EntrySource = "Browse";
                SetState(false);
                تفريغ_خانات_الشاشة();
            }
            catch (SqlException ex)
            {
                transaction.Rollback();
                if (ex.Number == 547)
                {
                    MessageBox.Show("فشل الحذف: لا يمكن حذف هذا العميل لوجود معاملات مالية مرتبطة به في النظام. يمكنك إيقاف الحساب بدلاً من حذفه.",
                                    "خطأ محاسبي أمني", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                else
                {
                    MessageBox.Show($"حدث خطأ غير متوقع أثناء الحذف من قاعدة البيانات: {ex.Message}", "خطأ نظام", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                MessageBox.Show($"حدث خطأ عام: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally { }
        }

        private void تفريغ_خانات_الشاشة()
        {
            cust_ID.Clear();
            cust_Name.Clear();
            cust_Phone.Clear();
            cust_Address.Clear();
            acc_ID.Clear();
            if (cmb_parent_ID != null) cmb_parent_ID.SelectedIndex = -1;
            dgv_currencies.Rows.Clear();
            تحميل_جميع_العملات_في_الجدول();
        }

        private string توليد_رقم_الحساب_الشجري(string parentAccID)
        {
            if (string.IsNullOrEmpty(parentAccID)) return "";
            string newAccID = parentAccID.Trim() + "0001";
            try
            {
                string query = @"SELECT MAX(CAST(SUBSTRING(Acc_ID, 7, 4) AS INT)) 
                                 FROM Accounts 
                                 WHERE Acc_ID LIKE @ParentPattern 
                                   AND LEN(RTRIM(Acc_ID)) = 10";

                using (SqlCommand cmd = new SqlCommand(query, DatabaseHelper.GetConnection()))
                {
                    cmd.Parameters.AddWithValue("@ParentPattern", parentAccID.Trim() + "%");
                    object result = cmd.ExecuteScalar();

                    if (result != DBNull.Value && result != null)
                    {
                        int maxSubCode = Convert.ToInt32(result);
                        newAccID = parentAccID.Trim() + (maxSubCode + 1).ToString("D4");
                    }
                }
            }
            catch (Exception ex) { System.Diagnostics.Debug.WriteLine("خطأ توليد رقم الحساب الشجري: " + ex.Message); }
            finally { }
            return newAccID;
        }

        private void تحميل_جميع_العملات_في_الجدول()
        {
            try
            {
                string query = "SELECT Cur_ID, Cur_Name FROM Currencies WHERE Is_Active = 1";
                using (SqlCommand cmd = new SqlCommand(query, DatabaseHelper.GetConnection()))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        dgv_currencies.Rows.Clear();
                        foreach (DataRow row in dt.Rows)
                        {
                            // 🎯 الترتيب الصحيح لشاشتك: 
                            // 0: تفعيل | 1: اسم العملة | 2: الافتراضية | 3: توقيف العملة | 4: كود العملة المخفي
                            dgv_currencies.Rows.Add(false, row["Cur_Name"].ToString(), false, false, row["Cur_ID"].ToString().Trim());
                        }
                    }
                }

                // تشغيل القفل والتلوين الرمادي فوراً للعملات غير المختارة
                تهيئة_حالة_خلايا_الجدول_الافتراضية();
            }
            catch (Exception ex) { MessageBox.Show($"خطأ أثناء تحميل العملات بالترتيب الفعلي: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            finally { }
        }

        private void تهيئة_حالة_خلايا_الجدول_الافتراضية()
        {
            foreach (DataGridViewRow row in dgv_currencies.Rows)
            {
                if (row.IsNewRow) continue;

                if (row.Cells[0].Value == null) row.Cells[0].Value = false;
                if (row.Cells[2].Value == null) row.Cells[2].Value = false;
                if (row.Cells[3].Value == null) row.Cells[3].Value = false;

                bool isActive = Convert.ToBoolean(row.Cells[0].Value);

                if (!isActive)
                {
                    row.Cells[2].Value = false;
                    row.Cells[3].Value = false;

                    row.Cells[2].ReadOnly = true; // قفل الافتراضي
                    row.Cells[3].ReadOnly = true; // قفل التوقيف

                    row.Cells[2].Style.BackColor = System.Drawing.Color.LightGray;
                    row.Cells[3].Style.BackColor = System.Drawing.Color.LightGray;
                }
                else
                {
                    row.Cells[2].ReadOnly = false;
                    row.Cells[3].ReadOnly = false;

                    row.Cells[2].Style.BackColor = System.Drawing.Color.White;
                    row.Cells[3].Style.BackColor = System.Drawing.Color.White;
                }
            }
            dgv_currencies.Refresh();
        }

        private void dgv_currencies_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                dgv_currencies.CommitEdit(DataGridViewDataErrorContexts.Commit);

                bool isActive = Convert.ToBoolean(dgv_currencies.Rows[e.RowIndex].Cells[0].Value);
                bool isDefault = Convert.ToBoolean(dgv_currencies.Rows[e.RowIndex].Cells[2].Value);
                bool isFrozen = Convert.ToBoolean(dgv_currencies.Rows[e.RowIndex].Cells[3].Value);

                // ❌ التحكم الحي عند النقر على العمود الأول (التفعيل)
                if (e.ColumnIndex == 0)
                {
                    if (isActive)
                    {
                        dgv_currencies.Rows[e.RowIndex].Cells[2].ReadOnly = false;
                        dgv_currencies.Rows[e.RowIndex].Cells[3].ReadOnly = false;
                        dgv_currencies.Rows[e.RowIndex].Cells[2].Style.BackColor = System.Drawing.Color.White;
                        dgv_currencies.Rows[e.RowIndex].Cells[3].Style.BackColor = System.Drawing.Color.White;
                    }
                    else
                    {
                        dgv_currencies.Rows[e.RowIndex].Cells[2].Value = false;
                        dgv_currencies.Rows[e.RowIndex].Cells[3].Value = false;
                        dgv_currencies.Rows[e.RowIndex].Cells[2].ReadOnly = true;
                        dgv_currencies.Rows[e.RowIndex].Cells[3].ReadOnly = true;
                        dgv_currencies.Rows[e.RowIndex].Cells[2].Style.BackColor = System.Drawing.Color.LightGray;
                        dgv_currencies.Rows[e.RowIndex].Cells[3].Style.BackColor = System.Drawing.Color.LightGray;
                    }
                    dgv_currencies.RefreshEdit();
                    return;
                }

                // حماية النقرات العشوائية وهي مقفلة
                if (!isActive && (e.ColumnIndex == 2 || e.ColumnIndex == 3))
                {
                    dgv_currencies.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = false;
                    dgv_currencies.RefreshEdit();
                    return;
                }

                // 🌟 سياسة اختيار العملة الافتراضية الأحادية (عمود 2)
                if (e.ColumnIndex == 2)
                {
                    if (isDefault)
                    {
                        if (isFrozen)
                        {
                            dgv_currencies.Rows[e.RowIndex].Cells[2].Value = false;
                            dgv_currencies.RefreshEdit();
                            MessageBox.Show("محاسبياً: لا يمكن تعيين عملة موقوفة كعملة افتراضية للعميل.", "الرقابة البرمجية", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        dgv_currencies.Rows[e.RowIndex].Cells[0].Value = true;

                        foreach (DataGridViewRow row in dgv_currencies.Rows)
                        {
                            if (row.Index != e.RowIndex && !row.IsNewRow)
                            {
                                row.Cells[2].Value = false;
                            }
                        }
                        dgv_currencies.RefreshEdit();
                    }
                }

                // ❄️ سياسة عمود "توقيف" (العمود رقم 3)
                if (e.ColumnIndex == 3)
                {
                    if (isFrozen && isDefault)
                    {
                        dgv_currencies.Rows[e.RowIndex].Cells[3].Value = false;
                        dgv_currencies.RefreshEdit();
                        MessageBox.Show("محاسبياً: لا يمكن إيقاف العمل بالعملة الافتراضية للعميل.", "منع التضارب", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                }
            }
        }

        public override void OnSearch()
        {
            فتح_شاشة_البحث_عن_العملاء();
        }

        private void فتح_شاشة_البحث_عن_العملاء()
        {
            string sqlQuery = "SELECT Cust_ID AS [كود العميل], Cust_Name AS [اسم العميل], Cust_Phone AS [رقم الهاتف] FROM Customers ORDER BY Cust_ID";

            using (AlRowad_ERP.HelpForms.UniversalSearchForm searchForm = new AlRowad_ERP.HelpForms.UniversalSearchForm("البحث عن العملاء المتوفرين", sqlQuery))
            {
                if (searchForm.ShowDialog() == DialogResult.OK)
                {
                    string selectedCustomerCode = searchForm.المعرف_المختار;
                    if (!string.IsNullOrEmpty(selectedCustomerCode))
                    {
                        جلب_بيانات_العميل_وتعبئتها(selectedCustomerCode);
                    }
                }
            }
        }

        private void جلب_بيانات_العميل_وتعبئتها(string customerCode)
        {
            try
            {
                string query = "SELECT Cust_ID, Cust_Name, Cust_Phone, Cust_Address, Acc_ID FROM Customers WHERE Cust_ID = @Code";

                using (SqlCommand cmd = new SqlCommand(query, DatabaseHelper.GetConnection()))
                {
                    cmd.Parameters.AddWithValue("@Code", customerCode.Trim());
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        if (dt.Rows.Count > 0)
                        {
                            DataRow row = dt.Rows[0];
                            cust_ID.Text = row["Cust_ID"].ToString();
                            cust_Name.Text = row["Cust_Name"].ToString();
                            cust_Phone.Text = row["Cust_Phone"].ToString();
                            cust_Address.Text = row["Cust_Address"].ToString();
                            acc_ID.Text = row["Acc_ID"].ToString();

                            if (cmb_parent_ID != null && acc_ID.Text.Length >= 6)
                            {
                                cmb_parent_ID.SelectedValue = acc_ID.Text.Substring(0, 6);
                            }

                            EntrySource = "Browse";
                            SetState(false); // تجميد وقفل الواجهة فوراً للتصفح الآمن

                            // 🎯 التصحيح الجوهري: نمرر رقم الحساب المالي للعميل بدلاً من كود العميل لفك الفلترة الموحدة
                            string currentAssociatedAccount = acc_ID.Text.Trim();
                            تحميل_عملات_العميل_المخزنة(currentAssociatedAccount);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث خطأ أثناء جلب بيانات العميل المختار: " + ex.Message, "خطأ في النظام", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally { }
        }
        private void تحميل_عملات_العميل_المخزنة(string accountCode)
        {
            try
            {
                // 1. شحن جدول العملات بالكامل كوضع نظيف (كلها غير محددة)
                تحميل_جميع_العملات_في_الجدول();

                if (string.IsNullOrEmpty(accountCode)) return;

                // 2. 🎯 التصحيح الهيكلي: الفلترة بـ Acc_ID وقراءة الحقل Is_Active الموحد لجدول الحسابات
                string query = "SELECT RTRIM(Cur_ID) AS Cur_ID, Is_Default, ISNULL(Is_Frozen, 0) AS Is_Frozen, ISNULL(Is_Active, 0) AS Is_Active FROM Account_Allowed_Currencies WHERE Acc_ID = @AccID";

                using (SqlCommand cmd = new SqlCommand(query, DatabaseHelper.GetConnection()))
                {
                    cmd.Parameters.AddWithValue("@AccID", accountCode.Trim());
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string dbCurId = reader["Cur_ID"].ToString().Trim();
                            bool isActive = Convert.ToBoolean(reader["Is_Active"]);
                            bool isDefault = Convert.ToBoolean(reader["Is_Default"]);
                            bool isFrozen = Convert.ToBoolean(reader["Is_Frozen"]);

                            foreach (DataGridViewRow row in dgv_currencies.Rows)
                            {
                                if (row.IsNewRow) continue;

                                // كود العملة يقع في الخلية رقم 4 بحسب هيكلية شاشتك الفعليّة
                                string gridCurId = row.Cells[4].Value?.ToString().Trim();

                                if (gridCurId == dbCurId)
                                {
                                    // شحن الحالات الحقيقية المتكاملة من السيرفر
                                    row.Cells[0].Value = isActive;   // تفعيل العملة لفتح القيود البصرية للسطر
                                    row.Cells[2].Value = isDefault;  // العملة الافتراضية
                                    row.Cells[3].Value = isFrozen;   // توقيف العملة
                                    break;
                                }
                            }
                        }
                    }
                }

                // 3. تشغيل الحماية البصرية والتلوين الرمادي بعد الانتهاء من ضخ البيانات الحقيقية
                تهيئة_حالة_خلايا_الجدول_الافتراضية();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("خطأ تتبع شحن تفاصيل عملات العميل: " + ex.Message);
            }
            finally
            {
                }
        }
        private void cmb_parent_ID_SelectedIndexChanged(object sender, EventArgs e)
        {
            // تحديث رقم الحساب تلقائياً عند تغيير الحساب الأب
            if (EntrySource == "New" && cmb_parent_ID.SelectedValue != null)
            {
                string parentAccID = cmb_parent_ID.SelectedValue.ToString();
                acc_ID.Text = توليد_رقم_الحساب_الشجري(parentAccID);
            }
        }
    }
}