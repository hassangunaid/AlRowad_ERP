using AlRowad_ERP.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks; // 🌟 إضافة دعم التزامن
using System.Windows.Forms;
using AlRowad_ERP.Data;
using AlRowad_ERP.UI.Base;
using AlRowad_ERP.Core.Constants;

namespace AlRowad_ERP.Forms
{
    public partial class Suppliers : BaseEntryForm
    {
        private byte[] _currentRowVersion;

        public Suppliers()
        {
            InitializeComponent();
            PrimaryIdFieldName = "supp_IDTextBox";
            this.MainTableName = "Suppliers";

            // 🌟 الفحص الآلي للارتباطات المرجعية
            this.DeleteDependencies.Add("Purchase_Invoices", "Vendor_ID");
            this.DeleteDependencies.Add("Purchase_Orders", "Vendor_ID");

            this.Load += Suppliers_Load;
            this.dgv_currencies.CurrentCellDirtyStateChanged += dgv_currencies_CurrentCellDirtyStateChanged;
            this.dgv_currencies.CellValueChanged += dgv_currencies_CellValueChanged;
        }

        // 🌟 تحميل غير متزامن
        private async void Suppliers_Load(object sender, EventArgs e)
        {
            dgv_currencies.Rows.Clear();
            await تهيئة_قائمة_الحسابات_الرئيسية_Async();

            ChangeFormMode(FormMode.View);
        }

        protected override void LockControls(Control parent, bool isReadOnly)
        {
            base.LockControls(parent, isReadOnly);

            if (supp_IDTextBox != null)
            {
                supp_IDTextBox.ReadOnly = (CurrentMode != FormMode.New);
            }
            if (acc_IDTextBox != null)
            {
                acc_IDTextBox.ReadOnly = (CurrentMode != FormMode.New);
            }

            if (dgv_currencies != null)
            {
                dgv_currencies.Enabled = !isReadOnly;
                dgv_currencies.ReadOnly = isReadOnly;
                dgv_currencies.AllowUserToAddRows = false;
                dgv_currencies.AllowUserToDeleteRows = false;

                if (!isReadOnly)
                {
                    تهيئة_حالة_خلايا_الجدول_الافتراضية();
                }
            }
        }

        // 🌟 تعبئة الحسابات بدون تجميد الشاشة
        private async Task تهيئة_قائمة_الحسابات_الرئيسية_Async()
        {
            try
            {
                string query = @"SELECT Acc_ID, Acc_ID + ' - ' + Acc_Name AS Acc_Full_Name 
                                 FROM Accounts 
                                 WHERE Is_Stopped = 0 
                                   AND Acc_ID LIKE '2101%' 
                                   AND LEN(RTRIM(Acc_ID)) = 6
                                 ORDER BY Acc_ID";

                DataTable dt = await DatabaseHelper.GetTableAsync(query);

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
            catch (Exception ex)
            {
                MessageBox.Show("خطأ أثناء تصفية وتثبيت حسابات الموردين الرئيسية: " + ex.Message, "خطأ فني", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected override string GetNextId()
        {
            string newCode = "0001";
            try
            {
                string query = "SELECT MAX(CAST(Supp_ID AS INT)) FROM Suppliers";
                object result = DatabaseHelper.ExecuteScalar(query);

                if (result != DBNull.Value && result != null)
                {
                    int maxCode = Convert.ToInt32(result);
                    newCode = (maxCode + 1).ToString("D4");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("خطأ فني في توليد كود المورد المنسق: " + ex.Message);
            }
            return newCode;
        }

        public override void OnNew()
        {
            base.OnNew(); // سيقوم بتفريغ الشاشة واستدعاء GetNextId 

            if (cmb_parent_ID != null) cmb_parent_ID.SelectedIndex = -1;

            acc_IDTextBox.Clear();
            dgv_currencies.Rows.Clear();
            supp_NameTextBox.Focus();
        }

        public override void OnEdit()
        {
            if (string.IsNullOrWhiteSpace(supp_IDTextBox.Text))
            {
                MessageBox.Show("يرجى اختيار المورد المراد تعديله أولاً عن طريق شاشة البحث الموحدة.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            base.OnEdit();
            supp_NameTextBox.Focus();
        }

        public override void OnCancel()
        {
            base.OnCancel();

            if (CurrentMode == FormMode.New || (CurrentMode == FormMode.View && string.IsNullOrWhiteSpace(supp_IDTextBox.Text)))
            {
                dgv_currencies.Rows.Clear();
            }
        }

        // 🌟 الحفظ اللامتزامن باستخدام الـ Transactions والثوابت
        protected override async Task<bool> ExecuteSaveToDatabaseAsync(SqlTransaction trans)
        {
            if (string.IsNullOrWhiteSpace(supp_NameTextBox.Text) || string.IsNullOrWhiteSpace(acc_IDTextBox.Text))
            {
                MessageBox.Show("يجب إدخال اسم المورد ورقم الحساب المالي.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            bool hasSelectedCurrency = false;
            bool hasDefaultCurrency = false;

            if (dgv_currencies != null)
            {
                foreach (DataGridViewRow row in dgv_currencies.Rows)
                {
                    if (row.IsNewRow || row.Cells[4].Value == null) continue;

                    bool act = Convert.ToBoolean(row.Cells[0].Value);
                    bool isDef = Convert.ToBoolean(row.Cells[2].Value);
                    bool frz = Convert.ToBoolean(row.Cells[3].Value);

                    if (isDef && frz)
                    {
                        MessageBox.Show($"منع الحفظ: اكتشاف تناقض منطقي!\nلا يمكن للعملة ({row.Cells[1].Value}) أن تكون 'افتراضية' وفي نفس الوقت 'مجمدة'.",
                                        "حماية قواعد الأعمال", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return false;
                    }

                    if (act || frz)
                    {
                        hasSelectedCurrency = true;
                        if (isDef) hasDefaultCurrency = true;
                    }
                }

                if (!hasSelectedCurrency)
                {
                    MessageBox.Show("لا يمكن حفظ المورد دون تفعيل عملة واحدة له على الأقل.", "منع الحفظ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                if (!hasDefaultCurrency)
                {
                    MessageBox.Show("يجب تحديد عملة واحدة لتكون العملة الافتراضية لهذا المورد.", "منع الحفظ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }

            try
            {
                if (CurrentMode == FormMode.New)
                {
                    string accSql = $@"INSERT INTO Accounts 
                                    (Acc_ID, Acc_Name, Is_Stopped, Account_Level, Parent_ID, Acc_Type, Acc_Nature, Report_Type, {SystemConstants.AuditFields.CreatedBy}, {SystemConstants.AuditFields.CreatedAt}) 
                                    VALUES (@AccID, @AccName, 0, 5, @ParentID, 2, 2, 1, @Created_By, @Created_At)";

                    var pAcc = new List<SqlParameter> {
                        new SqlParameter("@AccID", acc_IDTextBox.Text.Trim()),
                        new SqlParameter("@AccName", supp_NameTextBox.Text.Trim()),
                        new SqlParameter("@ParentID", cmb_parent_ID.SelectedValue ?? (object)DBNull.Value),
                        new SqlParameter("@Created_By", UserSession.UserId),
                        new SqlParameter("@Created_At", DateTime.Now)
                    };
                    await DatabaseHelper.ExecuteNonQueryAsync(accSql, pAcc.ToArray(), trans);

                    string suppSql = $@"INSERT INTO Suppliers 
                                      (Supp_ID, Supp_Name, Supp_Phone, Supp_Address, Acc_ID, {SystemConstants.AuditFields.CreatedBy}, {SystemConstants.AuditFields.CreatedAt}) 
                                      VALUES (@ID, @Name, @Phone, @Address, @AccID, @Created_By, @Created_At)";

                    var pSupp = new List<SqlParameter> {
                        new SqlParameter("@ID", supp_IDTextBox.Text.Trim()),
                        new SqlParameter("@Name", supp_NameTextBox.Text.Trim()),
                        new SqlParameter("@Phone", supp_PhoneTextBox.Text.Trim()),
                        new SqlParameter("@Address", supp_AddressTextBox.Text.Trim()),
                        new SqlParameter("@AccID", acc_IDTextBox.Text.Trim()),
                        new SqlParameter("@Created_By", UserSession.UserId),
                        new SqlParameter("@Created_At", DateTime.Now)
                    };
                    await DatabaseHelper.ExecuteNonQueryAsync(suppSql, pSupp.ToArray(), trans);
                }
                else if (CurrentMode == FormMode.Edit)
                {
                    string updateAcc = $@"UPDATE Accounts 
                                         SET Acc_Name = @Name, {SystemConstants.AuditFields.UpdatedBy}  = @Updated_By,  {SystemConstants.AuditFields.UpdatedAt} = @Updated_At 
                                         WHERE Acc_ID = @AccID";

                    var pUpdAcc = new List<SqlParameter> {
                        new SqlParameter("@Name", supp_NameTextBox.Text.Trim()),
                        new SqlParameter("@AccID", acc_IDTextBox.Text.Trim()),
                        new SqlParameter("@Updated_By", UserSession.UserId),
                        new SqlParameter("@Updated_At", DateTime.Now)
                    };
                    await DatabaseHelper.ExecuteNonQueryAsync(updateAcc, pUpdAcc.ToArray(), trans);

                    // 🚀 تحديث المورد مع شرط التزامن الصارم (RowVersion)
                    string updateSupp = $@"UPDATE Suppliers 
                                          SET Supp_Name = @Name, Supp_Phone = @Phone, Supp_Address = @Address,
                                              {SystemConstants.AuditFields.UpdatedBy}  = @Updated_By,  {SystemConstants.AuditFields.UpdatedAt} = @Updated_At 
                                          WHERE Supp_ID = @ID AND RowVersion = @OldRowVersion";

                    var pUpdSupp = new List<SqlParameter> {
                        new SqlParameter("@Name", supp_NameTextBox.Text.Trim()),
                        new SqlParameter("@Phone", supp_PhoneTextBox.Text.Trim()),
                        new SqlParameter("@Address", supp_AddressTextBox.Text.Trim()),
                        new SqlParameter("@ID", supp_IDTextBox.Text.Trim()),
                        new SqlParameter("@Updated_By", UserSession.UserId),
                        new SqlParameter("@Updated_At", DateTime.Now),
                        new SqlParameter("@OldRowVersion", SqlDbType.Timestamp) { Value = _currentRowVersion ?? (object)DBNull.Value }
                    };

                    int rowsAffected = await DatabaseHelper.ExecuteNonQueryAsync(updateSupp, pUpdSupp.ToArray(), trans);

                    if (rowsAffected == 0)
                    {
                        throw new InvalidOperationException("تضارب بيانات: تم تعديل بيانات هذا المورد من قبل مستخدم آخر أثناء استعراضك لها. يرجى تحديث الشاشة والمحاولة مجدداً.");
                    }

                    string newValues = $"الاسم: {supp_NameTextBox.Text.Trim()} | الهاتف: {supp_PhoneTextBox.Text.Trim()} | العنوان: {supp_AddressTextBox.Text.Trim()}";
                    DatabaseHelper.LogAuditTransaction(trans, "Suppliers", supp_IDTextBox.Text.Trim(), "UPDATE", "تم الحفظ المسبق", newValues, "تعديل بيانات المورد والحساب المالي");
                }

                string delCurSql = "DELETE FROM Account_Allowed_Currencies WHERE Acc_ID = @AccID";
                await DatabaseHelper.ExecuteNonQueryAsync(delCurSql, new[] { new SqlParameter("@AccID", acc_IDTextBox.Text.Trim()) }, trans);

                if (dgv_currencies != null)
                {
                    string curSql = "INSERT INTO Account_Allowed_Currencies (Acc_ID, Cur_ID, Is_Active, Is_Frozen, Is_Default) VALUES (@AccID, @CurID, @IsActive, @IsFrozen, @IsDefault)";
                    foreach (DataGridViewRow row in dgv_currencies.Rows)
                    {
                        if (row.IsNewRow || row.Cells[4].Value == null) continue;

                        bool act = Convert.ToBoolean(row.Cells[0].Value);
                        bool isDef = Convert.ToBoolean(row.Cells[2].Value);
                        bool frz = Convert.ToBoolean(row.Cells[3].Value);

                        if (act || frz)
                        {
                            SqlParameter[] pCur = {
                                new SqlParameter("@AccID", acc_IDTextBox.Text.Trim()),
                                new SqlParameter("@CurID", row.Cells[4].Value.ToString()),
                                new SqlParameter("@IsActive", act),
                                new SqlParameter("@IsFrozen", frz),
                                new SqlParameter("@IsDefault", isDef)
                            };
                            await DatabaseHelper.ExecuteNonQueryAsync(curSql, pCur, trans);
                        }
                    }
                }

                return true;
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "تضارب في التزامن", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("فشل أثناء تنفيذ الأوامر: " + ex.Message);
            }
        }

        // 🌟 الحذف اللامتزامن المنطقي المزدوج (المورد + الحساب)
        protected override async Task<bool> ExecuteDeleteFromDatabaseAsync(SqlTransaction transaction)
        {
            string updateSupp = $@"UPDATE Suppliers SET {SystemConstants.Columns.Is_Deleted} = 1, {SystemConstants.AuditFields.DeletedBy} = @UserId, {SystemConstants.AuditFields.DeletedAt} = GETDATE() WHERE Supp_ID = @SuppID";
            await DatabaseHelper.ExecuteNonQueryAsync(updateSupp, new[] {
                new SqlParameter("@SuppID", supp_IDTextBox.Text.Trim()), new SqlParameter("@UserId", UserSession.UserId)
            }, transaction);

            if (!string.IsNullOrWhiteSpace(acc_IDTextBox.Text))
            {
                string updateAcc = $@"UPDATE Accounts SET Is_Stopped = 1, {SystemConstants.AuditFields.UpdatedBy} = @UserId, {SystemConstants.AuditFields.UpdatedAt} = GETDATE() WHERE Acc_ID = @AccID";
                await DatabaseHelper.ExecuteNonQueryAsync(updateAcc, new[] {
                    new SqlParameter("@AccID", acc_IDTextBox.Text.Trim()), new SqlParameter("@UserId", UserSession.UserId)
                }, transaction);
            }
            return true;
        }

        // 🌟 التأكد من المراجع قبل الحذف المنطقي
        protected override async Task<bool> ValidateDependenciesBeforeDeleteAsync()
        {
            string vendorId = supp_IDTextBox.Text;

            if (await IsRecordUsedInTableAsync("Purchase_Invoices", "Vendor_ID", vendorId))
            {
                MessageBox.Show("منع أمني: لا يمكن حذف بطاقة المورد لوجود فواتير مشتريات مرتبطة به.", "ارتباط مرجعي", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }

            if (await IsRecordUsedInTableAsync("Purchase_Orders", "Vendor_ID", vendorId))
            {
                MessageBox.Show("منع أمني: المورد مرتبط بأوامر شراء سابقة.", "ارتباط مرجعي", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }

            return true;
        }

        private string توليد_رقم_الحساب_الشجري(string parentAccID)
        {
            if (string.IsNullOrEmpty(parentAccID)) return "";
            try
            {
                string query = "SELECT MAX(CAST(Acc_ID AS BIGINT)) FROM Accounts WHERE Parent_ID = @ParentID";
                SqlParameter[] p = { new SqlParameter("@ParentID", parentAccID.Trim()) };
                object result = DatabaseHelper.ExecuteScalar(query, p);

                if (result != null && result != DBNull.Value)
                {
                    long lastId = Convert.ToInt64(result);
                    return (lastId + 1).ToString();
                }

                return parentAccID.Trim() + "0001";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("خطأ توليد رقم الحساب الشجري للمورد: " + ex.Message);
                return parentAccID.Trim() + "0001";
            }
        }

        #region دوال العمليات الحسابية والتهيئة

        private async Task تحميل_عملات_المورد_بناء_على_الأب_Async(string accId, string parentId)
        {
            try
            {
                dgv_currencies.CellValueChanged -= dgv_currencies_CellValueChanged;

                string cleanAccId = string.IsNullOrWhiteSpace(accId) ? "0" : accId.Trim();
                string cleanParentId = string.IsNullOrWhiteSpace(parentId) ? "0" : parentId.Trim();

                DataTable dt = await Task.Run(() => AccountRepository.GetAccountCurrencies(cleanAccId, cleanParentId, 5));
                dgv_currencies.Rows.Clear();

                if (dt.Rows.Count == 0 && CurrentMode == FormMode.New && !string.IsNullOrWhiteSpace(cleanParentId) && cleanParentId != "0")
                {
                    int parentCurCount = await Task.Run(() => AccountRepository.CheckParentCurrenciesCount(cleanParentId));
                    if (parentCurCount == 0)
                    {
                        MessageBox.Show("تنبيه أمني: الحساب الرئيسي (الأب) المحدد لا يملك أي عملات مفعلة!\nقم باستعراض الحساب الأب في دليل الحسابات وحفظ عملاته أولاً.", "سياسة العملات المتاحة", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }

                foreach (DataRow row in dt.Rows)
                {
                    int rowIndex = dgv_currencies.Rows.Add(
                        Convert.ToBoolean(row["Is_Active"]),
                        row["Cur_Name"].ToString(),
                        Convert.ToBoolean(row["Is_Default"]),
                        Convert.ToBoolean(row["Is_Frozen"]),
                        row["Cur_ID"]
                    );

                    dgv_currencies.Rows[rowIndex].Tag = Convert.ToBoolean(row["Global_Active"]);
                }

                تهيئة_حالة_خلايا_الجدول_الافتراضية();
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ أثناء بناء جدول العملات للمورد: " + ex.Message, "خطأ هيكلي", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                dgv_currencies.CellValueChanged += dgv_currencies_CellValueChanged;
            }
        }

        private void dgv_currencies_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgv_currencies.IsCurrentCellDirty && CurrentMode != FormMode.View)
            {
                dgv_currencies.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void dgv_currencies_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (CurrentMode == FormMode.View || e.RowIndex < 0 || e.ColumnIndex < 0) return;

            var row = dgv_currencies.Rows[e.RowIndex];
            dgv_currencies.CellValueChanged -= dgv_currencies_CellValueChanged;

            try
            {
                bool isActive = Convert.ToBoolean(row.Cells[0].Value ?? false);
                bool isDefault = Convert.ToBoolean(row.Cells[2].Value ?? false);
                bool isFrozen = Convert.ToBoolean(row.Cells[3].Value ?? false);
                int curId = Convert.ToInt32(row.Cells[4].Value ?? 0);
                bool isGloballyActive = Convert.ToBoolean(row.Tag ?? true);

                if (e.ColumnIndex == 0)
                {
                    if (isActive)
                    {
                        if (!isGloballyActive)
                        {
                            MessageBox.Show("إدارة النظام أوقفت التعامل مع هذه العملة على مستوى النظام بأكمله.\nلا يمكن تفعيلها لهذا المورد لحين السماح بها من الإدارة.",
                                            "حظر سيادي", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            row.Cells[0].Value = false;
                            return;
                        }

                        row.Cells[2].ReadOnly = false;
                        row.Cells[3].ReadOnly = false;
                        row.DefaultCellStyle.BackColor = System.Drawing.Color.White;
                        row.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                    }
                    else
                    {
                        if (CurrentMode != FormMode.New && !string.IsNullOrWhiteSpace(acc_IDTextBox.Text))
                        {
                            if (!AccountRepository.CanDeactivateCurrency(acc_IDTextBox.Text.Trim(), curId, 5, out string errorMsg))
                            {
                                MessageBox.Show(errorMsg, "رقابة النظام المالي", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                row.Cells[0].Value = true;
                                return;
                            }
                        }

                        row.Cells[2].ReadOnly = true;
                        row.Cells[3].ReadOnly = true;
                        row.Cells[2].Value = false;
                        row.Cells[3].Value = false;
                        row.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(240, 240, 240);
                        row.DefaultCellStyle.ForeColor = System.Drawing.Color.Gray;
                    }
                }
                else if (e.ColumnIndex == 2 && isDefault)
                {
                    if (!isActive)
                    {
                        row.Cells[0].Value = true;
                        row.Cells[2].ReadOnly = false;
                        row.Cells[3].ReadOnly = false;
                        row.DefaultCellStyle.BackColor = System.Drawing.Color.White;
                        row.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                    }

                    if (isFrozen) row.Cells[3].Value = false;

                    foreach (DataGridViewRow r in dgv_currencies.Rows)
                    {
                        if (r.Index != e.RowIndex && !r.IsNewRow)
                            r.Cells[2].Value = false;
                    }
                }
                else if (e.ColumnIndex == 3 && isFrozen && isDefault)
                {
                    row.Cells[3].Value = false;
                    MessageBox.Show("لا يمكن تجميد العملة الافتراضية للمورد.", "حماية النظام", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            finally
            {
                dgv_currencies.CellValueChanged += dgv_currencies_CellValueChanged;
            }
        }

        private void تهيئة_حالة_خلايا_الجدول_الافتراضية()
        {
            if (dgv_currencies == null) return;
            dgv_currencies.CellValueChanged -= dgv_currencies_CellValueChanged;

            try
            {
                foreach (DataGridViewRow row in dgv_currencies.Rows)
                {
                    if (row.IsNewRow) continue;

                    bool isActive = Convert.ToBoolean(row.Cells[0].Value ?? false);

                    row.Cells[2].ReadOnly = !isActive;
                    row.Cells[3].ReadOnly = !isActive;

                    row.DefaultCellStyle.BackColor = isActive ? System.Drawing.Color.White : System.Drawing.Color.FromArgb(240, 240, 240);
                    row.DefaultCellStyle.ForeColor = isActive ? System.Drawing.Color.Black : System.Drawing.Color.Gray;

                    if (!isActive)
                    {
                        row.Cells[2].Value = false;
                        row.Cells[3].Value = false;
                    }
                }
            }
            finally
            {
                dgv_currencies.CellValueChanged += dgv_currencies_CellValueChanged;
            }
        }

        public override async void OnSearch()
        {
            string sqlQuery = "SELECT Supp_ID AS [كود المورد], Supp_Name AS [اسم المورد], Supp_Phone AS [رقم الهاتف] FROM Suppliers ORDER BY Supp_ID";

            using (AlRowad_ERP.HelpForms.UniversalSearchForm searchForm = new AlRowad_ERP.HelpForms.UniversalSearchForm("البحث عن الموردين المسجلين", sqlQuery))
            {
                if (searchForm.ShowDialog() == DialogResult.OK)
                {
                    string selectedSupplierCode = searchForm.المعرف_المختار;
                    if (!string.IsNullOrEmpty(selectedSupplierCode))
                    {
                        await جلب_بيانات_المورد_وتعبئتها_Async(selectedSupplierCode);
                    }
                }
            }
        }

        protected override async void RefreshData()
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(supp_IDTextBox.Text))
                {
                    await جلب_بيانات_المورد_وتعبئتها_Async(supp_IDTextBox.Text.Trim());
                }
            }
            catch { }
        }

        private async Task جلب_بيانات_المورد_وتعبئتها_Async(string supplierCode)
        {
            try
            {
                string query = $@"
                    SELECT s.Supp_ID, s.Supp_Name, s.Supp_Phone, s.Supp_Address, s.Acc_ID, s.RowVersion,
                           s.{SystemConstants.AuditFields.CreatedBy}, s.{SystemConstants.AuditFields.CreatedAt}, s.{SystemConstants.AuditFields.UpdatedBy}, s.{SystemConstants.AuditFields.UpdatedAt},
                           ISNULL(uc.Full_Name, uc.Username) AS CreatedByName,
                           ISNULL(uu.Full_Name, uu.Username) AS UpdatedByName
                    FROM Suppliers s
                    LEFT JOIN Users uc ON s.{SystemConstants.AuditFields.CreatedBy} = uc.User_ID
                    LEFT JOIN Users uu ON s.{SystemConstants.AuditFields.UpdatedBy} = uu.User_ID
                    WHERE s.Supp_ID = @Code";

                SqlParameter[] p = { new SqlParameter("@Code", supplierCode.Trim()) };
                DataTable dt = await DatabaseHelper.GetTableAsync(query, p);

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];

                    if (dt.Columns.Contains("RowVersion") && row["RowVersion"] != DBNull.Value)
                    {
                        _currentRowVersion = (byte[])row["RowVersion"];
                    }

                    AutoBindRecord(row); // 👈 الاعتماد على Caching في BaseEntryForm للربط

                    if (cmb_parent_ID != null && acc_IDTextBox.Text.Length >= 6)
                    {
                        cmb_parent_ID.SelectedValue = acc_IDTextBox.Text.Substring(0, 6);
                    }

                    ChangeFormMode(FormMode.RecordSelected);

                    string currentAssociatedAccount = acc_IDTextBox.Text.Trim();
                    string parentAccount = cmb_parent_ID.SelectedValue?.ToString() ?? "";

                    await تحميل_عملات_المورد_المخزنة_Async(currentAssociatedAccount, parentAccount);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث خطأ أثناء جلب وشحن بيانات المورد المختار: " + ex.Message, "خطأ في النظام", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task تحميل_عملات_المورد_المخزنة_Async(string accountCode, string parentCode)
        {
            try
            {
                await تحميل_عملات_المورد_بناء_على_الأب_Async(accountCode, parentCode);

                if (string.IsNullOrEmpty(accountCode)) return;

                string query = "SELECT RTRIM(Cur_ID) AS Cur_ID, Is_Default, ISNULL(Is_Frozen, 0) AS Is_Frozen, ISNULL(Is_Active, 0) AS Is_Active FROM Account_Allowed_Currencies WHERE Acc_ID = @AccID";
                DataTable dt = await DatabaseHelper.GetTableAsync(query, new[] { new SqlParameter("@AccID", accountCode.Trim()) });

                foreach (DataRow dbRow in dt.Rows)
                {
                    string dbCurId = dbRow["Cur_ID"].ToString().Trim();
                    bool isActive = Convert.ToBoolean(dbRow["Is_Active"]);
                    bool isDefault = Convert.ToBoolean(dbRow["Is_Default"]);
                    bool isFrozen = Convert.ToBoolean(dbRow["Is_Frozen"]);

                    foreach (DataGridViewRow gridRow in dgv_currencies.Rows)
                    {
                        if (gridRow.IsNewRow) continue;
                        if (gridRow.Cells[4].Value?.ToString().Trim() == dbCurId)
                        {
                            gridRow.Cells[0].Value = isActive;
                            gridRow.Cells[2].Value = isDefault;
                            gridRow.Cells[3].Value = isFrozen;
                            break;
                        }
                    }
                }
                تهيئة_حالة_خلايا_الجدول_الافتراضية();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("خطأ تتبع جلب وتوزيع عملات المورد: " + ex.Message);
            }
        }

        private async void cmb_parent_ID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CurrentMode == FormMode.New && cmb_parent_ID.SelectedValue != null)
            {
                string parentAccID = cmb_parent_ID.SelectedValue.ToString();
                acc_IDTextBox.Text = توليد_رقم_الحساب_الشجري(parentAccID);
                await تحميل_عملات_المورد_بناء_على_الأب_Async(acc_IDTextBox.Text.Trim(), parentAccID);
            }
        }

        #endregion
    }
}