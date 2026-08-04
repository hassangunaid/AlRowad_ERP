using AlRowad_ERP.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace AlRowad_ERP.Forms
{
    public partial class Suppliers : BaseEntryForm // الوراثة الشرعية من النخاع الشوكي للنظام
    {
        protected override string PrimaryIdFieldName => "supp_IDTextBox";

        public Suppliers()
        {
            InitializeComponent();

            // ربط الأحداث البصرية لضمان تشغيل الشاشة بمرونة كاملة
            this.Load += new System.EventHandler(this.Suppliers_Load);
            this.dgv_currencies.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_currencies_CellContentClick);
        }

        private void Suppliers_Load(object sender, EventArgs e)
        {
            // شحن القوائم والعملات مسبقاً كمورد جديد لتظهر الواجهة ممتلئة ومستقرة
            تحميل_جميع_العملات_في_الجدول();
            تهيئة_قائمة_الحسابات_الرئيسية();

            // الالتزام بالدستور: وضع الاستعراض والقفل الافتراضي فور الفتح لحماية البيانات
            EntrySource = "Browse";
            SetState(false);
        }

        // 🎯 تطويع دالة التحكم بالـ State لتنصاع تماماً لقوانين الأب الصارمة
        protected override void SetState(bool editing)
        {
            // 1. دع الأب يقفل ويفتح كافة الأدوات النصية والمنسدلة تلقائياً حركياً
            base.SetState(editing);

            // 2. حظر حقل المعرف المفتاحي كود المورد ورقم الحساب من التعديل العشوائي بوضع التعديل
            supp_IDTextBox.ReadOnly = true;
            acc_IDTextBox.ReadOnly = true;

            if (EntrySource == "Edit" && editing)
            {
                supp_IDTextBox.Enabled = false;
                acc_IDTextBox.Enabled = false;
            }
            else
            {
                supp_IDTextBox.Enabled = editing;
                acc_IDTextBox.Enabled = editing;
            }

            // 3. التحكم بجداول الحركة وتفاصيل العملات وفق رغبة المستخدم الحالية
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
                // تصفية جلب الحسابات الرئيسية للموردين (الالتزامات المتداولة - الموردين)
                string query = @"SELECT Acc_ID, Acc_ID + ' - ' + Acc_Name AS Acc_Full_Name 
                                 FROM Accounts 
                                 WHERE Is_Stopped = 0 
                                   AND Acc_ID LIKE '2101%' 
                                   AND LEN(RTRIM(Acc_ID)) = 6
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
                MessageBox.Show("خطأ أثناء تصفية وتثبيت حسابات الموردين الرئيسية: " + ex.Message, "خطأ فني", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally { }
        }

        private string توليد_كود_المورد_تلقائيا()
        {
            string newCode = "0001";
            try
            {
                // 🎯 قراءة كود المورد كـ نصوص مع جلب القيمة الأعلى رقمياً برمجياً
                string query = "SELECT MAX(CAST(Supp_ID AS INT)) FROM Suppliers";

                using (SqlCommand cmd = new SqlCommand(query, DatabaseHelper.GetConnection()))
                {
                    object result = cmd.ExecuteScalar();
                    if (result != DBNull.Value && result != null)
                    {
                        int maxCode = Convert.ToInt32(result);
                        // 🎯 فرض التنسيق الرباعي الصارم (D4) لتوليد الأصفار (0002, 0003...) آلياً
                        newCode = (maxCode + 1).ToString("D4");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("خطأ فني في توليد كود المورد المنسق: " + ex.Message);
            }
            finally { }

            return newCode;
        }
        // ─── [دوال محرك الأزرار الموروثة من الأب الصارم] ───
        public override void OnNew()
        {
            EntrySource = "New";
            ClearForm(this);

            if (cmb_parent_ID != null) cmb_parent_ID.SelectedIndex = -1;

            تحميل_جميع_العملات_في_الجدول();
            supp_IDTextBox.Text = توليد_كود_المورد_تلقائيا();

            SetState(true);
            supp_NameTextBox.Focus();
        }

        public override void OnEdit()
        {
            if (string.IsNullOrWhiteSpace(supp_IDTextBox.Text))
            {
                MessageBox.Show("يرجى اختيار المورد المراد تعديله أولاً عن طريق شاشة البحث الموحدة.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            EntrySource = "Edit";
            SetState(true);
            supp_NameTextBox.Focus();
        }

        public override void OnCancel()
        {
            base.OnCancel(); // استدعاء حماية التراجع والصد الأمني للأب
            تحميل_جميع_العملات_في_الجدول();
        }

        public override void OnSave()
        {
            base.OnSave(); // تسليم الراية للأب ليقوم بالتحقق العام وحقن الحفظ المحمي
        }

        // 🎯 المحرك المركزي للحفظ والتعديل الفعلي المرتبط بقاعدة البيانات للموردين
        // 🎯 المحرك المركزي للحفظ والتعديل الفعلي للموردين
        protected override bool ExecuteSaveToDatabase()
        {
            // التحقق من الحقول الأساسية
            if (string.IsNullOrEmpty(supp_NameTextBox.Text) || string.IsNullOrEmpty(acc_IDTextBox.Text))
            {
                MessageBox.Show("تنبيه: يجب إدخال اسم المورد ورقم الحساب المالي.", "منع الحفظ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            SqlTransaction transaction = DatabaseHelper.GetConnection().BeginTransaction();

            try
            {
                if (EntrySource == "New")
                {
                    // 1. إضافة المورد للحسابات (الربط الشجري المحكم)
                    // ملاحظة: Acc_Nature = 2 (دائن للموردين)
                    string insertAccountQuery = @"INSERT INTO Accounts (Acc_ID, Acc_Name, Is_Stopped, Account_Level, Parent_ID, Acc_Type, Acc_Nature, Report_Type) 
                                         VALUES (@AccID, @AccName, 0, 5, @ParentID, 2, 2, 1)";

                    using (SqlCommand cmdAcc = new SqlCommand(insertAccountQuery, DatabaseHelper.GetConnection(), transaction))
                    {
                        cmdAcc.Parameters.AddWithValue("@AccID", acc_IDTextBox.Text.Trim());
                        cmdAcc.Parameters.Add("@AccName", SqlDbType.NVarChar).Value = "حساب المورد: " + supp_NameTextBox.Text.Trim();
                        cmdAcc.Parameters.AddWithValue("@ParentID", cmb_parent_ID.SelectedValue);
                        cmdAcc.Parameters.AddWithValue("@AccType", 2);
                        cmdAcc.Parameters.AddWithValue("@AccNature", 2);
                        cmdAcc.Parameters.AddWithValue("@ReportType", 1);
                        cmdAcc.ExecuteNonQuery();
                    }

                    // 2. إضافة بيانات كرت المورد
                    string supplierQuery = "INSERT INTO Suppliers (Supp_ID, Supp_Name, Supp_Phone, Supp_Address, Acc_ID) VALUES (@Code, @Name, @Phone, @Address, @AccNo)";
                    using (SqlCommand cmd = new SqlCommand(supplierQuery, DatabaseHelper.GetConnection(), transaction))
                    {
                        cmd.Parameters.AddWithValue("@Code", supp_IDTextBox.Text.Trim());
                        cmd.Parameters.Add("@Name", SqlDbType.NVarChar).Value = supp_NameTextBox.Text.Trim();
                        cmd.Parameters.AddWithValue("@Phone", supp_PhoneTextBox.Text.Trim());
                        cmd.Parameters.AddWithValue("@Address", supp_AddressTextBox.Text.Trim());
                        cmd.Parameters.AddWithValue("@AccNo", acc_IDTextBox.Text.Trim());
                        cmd.ExecuteNonQuery();
                    }
                }
                else if (EntrySource == "Edit")
                {
                    // تحديث الحساب المالي والكرت
                    DatabaseHelper.ExecuteNonQuery("UPDATE Accounts SET Acc_Name = N'حساب المورد: " + supp_NameTextBox.Text.Trim() + "' WHERE Acc_ID = '" + acc_IDTextBox.Text.Trim() + "'", transaction);
                    DatabaseHelper.ExecuteNonQuery("UPDATE Suppliers SET Supp_Name = N'" + supp_NameTextBox.Text.Trim() + "', Supp_Phone = '" + supp_PhoneTextBox.Text.Trim() + "' WHERE Supp_ID = '" + supp_IDTextBox.Text.Trim() + "'", transaction);
                }

                // 3. ربط العملات المتعددة (محرك المصارفة والعمليات)
                DatabaseHelper.ExecuteNonQuery("DELETE FROM Account_Allowed_Currencies WHERE Acc_ID = '" + acc_IDTextBox.Text.Trim() + "'", transaction);

                foreach (DataGridViewRow row in dgv_currencies.Rows)
                {
                    if (row.IsNewRow) continue;
                    string curId = row.Cells["Cur_ID"].Value?.ToString();
                    if (!string.IsNullOrEmpty(curId))
                    {
                        string curQuery = "INSERT INTO Account_Allowed_Currencies (Acc_ID, Cur_ID, Is_Active, Is_Default) VALUES (@AccID, @CurID, @IsActive, @IsDefault)";
                        using (SqlCommand cmdCur = new SqlCommand(curQuery, DatabaseHelper.GetConnection(), transaction))
                        {
                            cmdCur.Parameters.AddWithValue("@AccID", acc_IDTextBox.Text.Trim());
                            cmdCur.Parameters.AddWithValue("@CurID", curId);
                            cmdCur.Parameters.AddWithValue("@IsActive", Convert.ToBoolean(row.Cells["Is_Active"].Value));
                            cmdCur.Parameters.AddWithValue("@IsDefault", Convert.ToBoolean(row.Cells["Is_Default"].Value));
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
                MessageBox.Show($"خطأ حرج في الإنزال المحاسبي: {ex.Message}");
                return false;
            }
            finally { }
        }
        public override void OnDelete()
        {
            if (string.IsNullOrEmpty(supp_IDTextBox.Text) || EntrySource != "Browse")
            {
                MessageBox.Show("تنبيه: يجب استعراض أو اختيار المورد المراد حذفه أولاً.", "منع الحذف", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show($"هل أنت متأكد تماماً من رغبتك في حذف المورد المختار: ({supp_NameTextBox.Text}) وحسابه المالي الشجري نهائياً؟",
                                                  "تأكيد الحذف", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

            if (result == DialogResult.Yes)
            {
                ExecuteDelete(supp_IDTextBox.Text.Trim(), acc_IDTextBox.Text.Trim());
            }
        }

        private void ExecuteDelete(string supplierCode, string accountCode)
        {
            SqlTransaction transaction = DatabaseHelper.GetConnection().BeginTransaction();

            try
            {
                // 1. تنظيف جدول عملات الحساب الموحد
                string deleteCurrenciesQuery = "DELETE FROM Account_Allowed_Currencies WHERE Acc_ID = @AccID";
                using (SqlCommand cmdCur = new SqlCommand(deleteCurrenciesQuery, DatabaseHelper.GetConnection(), transaction))
                {
                    cmdCur.Parameters.AddWithValue("@AccID", accountCode.Trim());
                    cmdCur.ExecuteNonQuery();
                }

                // 2. حذف كرت بطاقة المورد
                string deleteSupplierQuery = "DELETE FROM Suppliers WHERE Supp_ID = @Code";
                using (SqlCommand cmdSupp = new SqlCommand(deleteSupplierQuery, DatabaseHelper.GetConnection(), transaction))
                {
                    cmdSupp.Parameters.AddWithValue("@Code", supplierCode);
                    cmdSupp.ExecuteNonQuery();
                }

                // 3. حذف الحساب المحاسبي من الشجرة 
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
                MessageBox.Show("تم حذف المورد وصلاحيات العملات والحساب المرتبط به بنجاح باهر من النظام.", "تأكيد الحذف", MessageBoxButtons.OK, MessageBoxIcon.Information);

                EntrySource = "Browse";
                SetState(false);
                تفريغ_خانات_الشاشة();
            }
            catch (SqlException ex)
            {
                transaction.Rollback();
                if (ex.Number == 547)
                {
                    MessageBox.Show("فشل الحذف: لا يمكن حذف هذا المورد لوجود معاملات مالية أو فواتير مشتريات مرتبطة به في النظام حماية للتقارير.",
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
                MessageBox.Show($"حدث خطأ عام أثناء الحذف التنفيذي: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally { }
        }

        private void تفريغ_خانات_الشاشة()
        {
            supp_IDTextBox.Clear();
            supp_NameTextBox.Clear();
            supp_PhoneTextBox.Clear();
            supp_AddressTextBox.Clear();
            acc_IDTextBox.Clear();
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
            catch (Exception ex) { System.Diagnostics.Debug.WriteLine("خطأ توليد رقم الحساب الشجري للمورد: " + ex.Message); }
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
                            // ضخ الترتيب الخماسي الآمن الصارم لحمايته من الـ DataError
                            dgv_currencies.Rows.Add(false, row["Cur_Name"].ToString(), false, false, row["Cur_ID"].ToString().Trim());
                        }
                    }
                }

                // تشغيل تلوين وقفل الخلايا الفرعية بالرمادي الباهت فور التحميل النظيف
                تهيئة_حالة_خلايا_الجدول_الافتراضية();
            }
            catch (Exception ex) { MessageBox.Show($"خطأ أثناء تحميل العملات في جدول الموردين: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error); }
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

                    row.Cells[2].ReadOnly = true;
                    row.Cells[3].ReadOnly = true;

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

                // التحكم الحي الفوري عند النقر على عمود التفعيل (العمود 0)
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

                // سياسة اختيار العملة الافتراضية الأحادية (العمود 2)
                if (e.ColumnIndex == 2)
                {
                    if (isDefault)
                    {
                        if (isFrozen)
                        {
                            dgv_currencies.Rows[e.RowIndex].Cells[2].Value = false;
                            dgv_currencies.RefreshEdit();
                            MessageBox.Show("محاسبياً: لا يمكن تعيين عملة موقوفة كعملة افتراضية للمورد.", "الرقابة البرمجية", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

                // سياسة عمود توقيف العملة (العمود 3)
                if (e.ColumnIndex == 3)
                {
                    if (isFrozen && isDefault)
                    {
                        dgv_currencies.Rows[e.RowIndex].Cells[3].Value = false;
                        dgv_currencies.RefreshEdit();
                        MessageBox.Show("محاسبياً: لا يمكن إيقاف العمل بالعملة الافتراضية المحددة للمورد.", "منع التضارب", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                }
            }
        }

        public override void OnSearch()
        {
            فتح_شاشة_البحث_عن_الموردين();
        }

        private void فتح_شاشة_البحث_عن_الموردين()
        {
            string sqlQuery = "SELECT Supp_ID AS [كود المورد], Supp_Name AS [اسم المورد], Supp_Phone AS [رقم الهاتف] FROM Suppliers ORDER BY Supp_ID";

            using (AlRowad_ERP.HelpForms.UniversalSearchForm searchForm = new AlRowad_ERP.HelpForms.UniversalSearchForm("البحث عن الموردين المسجلين", sqlQuery))
            {
                if (searchForm.ShowDialog() == DialogResult.OK)
                {
                    string selectedSupplierCode = searchForm.المعرف_المختار;
                    if (!string.IsNullOrEmpty(selectedSupplierCode))
                    {
                        جلب_بيانات_المورد_وتعبئتها(selectedSupplierCode);
                    }
                }
            }
        }

        private void جلب_بيانات_المورد_وتعبئتها(string supplierCode)
        {
            try
            {
                string query = "SELECT Supp_ID, Supp_Name, Supp_Phone, Supp_Address, Acc_ID FROM Suppliers WHERE Supp_ID = @Code";

                using (SqlCommand cmd = new SqlCommand(query, DatabaseHelper.GetConnection()))
                {
                    cmd.Parameters.AddWithValue("@Code", supplierCode.Trim());
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        if (dt.Rows.Count > 0)
                        {
                            DataRow row = dt.Rows[0];
                            supp_IDTextBox.Text = row["Supp_ID"].ToString();
                            supp_NameTextBox.Text = row["Supp_Name"].ToString();
                            supp_PhoneTextBox.Text = row["Supp_Phone"].ToString();
                            supp_AddressTextBox.Text = row["Supp_Address"].ToString();
                            acc_IDTextBox.Text = row["Acc_ID"].ToString();

                            if (cmb_parent_ID != null && acc_IDTextBox.Text.Length >= 6)
                            {
                                cmb_parent_ID.SelectedValue = acc_IDTextBox.Text.Substring(0, 6);
                            }

                            EntrySource = "Browse";
                            SetState(false); // قفل وتجميد الشاشة فوراً للتصفح الآمن والمستقر

                            // تشغيل محرك شحن العملات المخزنة بناءً على رقم الحساب المالي للمورد
                            string currentAssociatedAccount = acc_IDTextBox.Text.Trim();
                            تحميل_عملات_المورد_المخزنة(currentAssociatedAccount);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث خطأ أثناء جلب وشحن بيانات المورد المختار: " + ex.Message, "خطأ في النظام", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally { }
        }

        private void تحميل_عملات_المورد_المخزنة(string accountCode)
        {
            try
            {
                تحميل_جميع_العملات_في_الجدول();

                if (string.IsNullOrEmpty(accountCode)) return;

                // جلب الارتباطات الحقيقية للعملات من الجدول الموحد للحسابات بناءً على Acc_ID
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

                                string gridCurId = row.Cells[4].Value?.ToString().Trim();

                                if (gridCurId == dbCurId)
                                {
                                    row.Cells[0].Value = isActive;
                                    row.Cells[2].Value = isDefault;
                                    row.Cells[3].Value = isFrozen;
                                    break;
                                }
                            }
                        }
                    }
                }

                // تثبيت الحماية البصرية والتلوين الرمادي بعد الانتهاء من ضخ التعديلات
                تهيئة_حالة_خلايا_الجدول_الافتراضية();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("خطأ تتبع جلب وتوزيع عملات المورد: " + ex.Message);
            }
            finally
            {
                }
        }

        private void cmb_parent_ID_SelectedIndexChanged(object sender, EventArgs e)
        {
            // تحديث رقم الحساب تلقائياً عند تغيير الحساب الأب بوضع الإضافة
            if (EntrySource == "New" && cmb_parent_ID.SelectedValue != null)
            {
                string parentAccID = cmb_parent_ID.SelectedValue.ToString();
                acc_IDTextBox.Text = توليد_رقم_الحساب_الشجري(parentAccID);
            }
        }
    }
}