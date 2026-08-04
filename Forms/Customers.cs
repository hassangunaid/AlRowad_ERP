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
    public partial class Customers : BaseEntryForm
    {
        private byte[] _currentRowVersion;

        public Customers()
        {
            InitializeComponent();
            this.Load += Customers_Load;

            PrimaryIdFieldName = "cust_ID";
            this.MainTableName = "Customers";

            this.dgv_currencies.CurrentCellDirtyStateChanged += dgv_currencies_CurrentCellDirtyStateChanged;
            this.dgv_currencies.CellValueChanged += dgv_currencies_CellValueChanged;
        }

        // 🌟 تحميل غير متزامن
        private async void Customers_Load(object sender, EventArgs e)
        {
            await تهيئة_قائمة_الحسابات_الرئيسية_Async();
            ChangeFormMode(FormMode.View);
        }

        protected override void LockControls(Control parent, bool isReadOnly)
        {
            base.LockControls(parent, isReadOnly);

            if (cust_ID != null) cust_ID.ReadOnly = (CurrentMode != FormMode.New);
            if (acc_ID != null) acc_ID.ReadOnly = (CurrentMode != FormMode.New);

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

        protected override string GetNextId()
        {
            string newCode = "0001";
            try
            {
                string query = "SELECT MAX(CAST(ISNULL(Cust_ID, 0) AS INT)) FROM Customers WHERE ISNUMERIC(Cust_ID) = 1";
                object result = DatabaseHelper.ExecuteScalar(query);

                if (result != DBNull.Value && result != null)
                {
                    int maxCode = Convert.ToInt32(result);
                    newCode = (maxCode + 1).ToString("D4");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("خطأ توليد كود العميل: " + ex.Message);
            }
            return newCode;
        }

        public override void OnNew()
        {
            base.OnNew();

            acc_ID.Clear();
            if (cmb_parent_ID != null) cmb_parent_ID.SelectedIndex = -1;
            dgv_currencies.Rows.Clear();
            cust_Name.Focus();
        }

        public override void OnEdit()
        {
            base.OnEdit();
            cust_Name.Focus();
        }

        public override void OnCancel()
        {
            base.OnCancel();
            if (CurrentMode == FormMode.New || (CurrentMode == FormMode.View && string.IsNullOrWhiteSpace(cust_ID.Text)))
            {
                dgv_currencies.Rows.Clear();
            }
        }

        // 🌟 جلب سريع ولا متزامن للبيانات
        private async Task جلب_بيانات_العميل_وتعبئتها_Async(string customerCode)
        {
            try
            {
                string query = $@"
                    SELECT c.Cust_ID, c.Cust_Name, c.Cust_Phone, c.Cust_Address, c.Acc_ID, c.RowVersion,
                           c.{SystemConstants.CreatedBy}, c.{SystemConstants.CreatedAt}, c.{SystemConstants.UpdatedBy}, c.{SystemConstants.UpdatedAt},
                           ISNULL(uc.Full_Name, uc.Username) AS CreatedByName,
                           ISNULL(uu.Full_Name, uu.Username) AS UpdatedByName
                    FROM Customers c
                    LEFT JOIN Users uc ON c.{SystemConstants.CreatedBy} = uc.User_ID
                    LEFT JOIN Users uu ON c.{SystemConstants.UpdatedBy} = uu.User_ID
                    WHERE c.Cust_ID = @Code";

                DataTable dt = await DatabaseHelper.GetTableAsync(query, new[] { new SqlParameter("@Code", customerCode.Trim()) });

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];

                    if (dt.Columns.Contains("RowVersion") && row["RowVersion"] != DBNull.Value)
                    {
                        _currentRowVersion = (byte[])row["RowVersion"];
                    }

                    AutoBindRecord(row); // 👈 استخدام محرك الذاكرة المؤقتة (Caching) الجديد في BaseEntryForm

                    string currentAssociatedAccount = row["Acc_ID"].ToString().Trim();
                    if (cmb_parent_ID != null && currentAssociatedAccount.Length >= 6)
                    {
                        cmb_parent_ID.SelectedValue = currentAssociatedAccount.Substring(0, 6);
                    }

                    string parentAccount = cmb_parent_ID.SelectedValue?.ToString() ?? "";
                    await تحميل_عملات_العميل_المخزنة_Async(currentAssociatedAccount, parentAccount);

                    ChangeFormMode(FormMode.RecordSelected);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث خطأ أثناء جلب بيانات العميل المختار: " + ex.Message, "خطأ في النظام", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 🌟 الحفظ اللامتزامن باستخدام الـ Transactions
        protected override async Task<bool> ExecuteSaveToDatabaseAsync(SqlTransaction trans)
        {
            if (string.IsNullOrWhiteSpace(cust_Name.Text) || string.IsNullOrWhiteSpace(acc_ID.Text))
            {
                MessageBox.Show("يجب إدخال اسم العميل ورقم الحساب المالي.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            bool hasActiveCurrency = false, hasDefaultCurrency = false;
            foreach (DataGridViewRow row in dgv_currencies.Rows)
            {
                if (row.IsNewRow) continue;
                if (Convert.ToBoolean(row.Cells[0].Value ?? false)) hasActiveCurrency = true;
                if (Convert.ToBoolean(row.Cells[2].Value ?? false)) hasDefaultCurrency = true;
            }

            if (!hasActiveCurrency || !hasDefaultCurrency)
            {
                MessageBox.Show("يجب تفعيل عملة واحدة على الأقل وتحديد العملة الافتراضية.", "تنبيه قواعد العمل", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            try
            {
                if (CurrentMode == FormMode.New)
                {
                    string accSql = $@"INSERT INTO Accounts (Acc_ID, Acc_Name, Is_Stopped, Account_Level, Parent_ID, Acc_Type, Acc_Nature, Report_Type, {SystemConstants.CreatedBy}, {SystemConstants.CreatedAt}) 
                                       VALUES (@AccID, @AccName, 0, 5, @ParentID, 1, 1, 1, @Created_By, @Created_At)";
                    await DatabaseHelper.ExecuteNonQueryAsync(accSql, new[] {
                        new SqlParameter("@AccID", acc_ID.Text.Trim()), new SqlParameter("@AccName", cust_Name.Text.Trim()),
                        new SqlParameter("@ParentID", cmb_parent_ID.SelectedValue ?? (object)DBNull.Value),
                        new SqlParameter("@Created_By", UserSession.UserId), new SqlParameter("@Created_At", DateTime.Now)
                    }, trans);

                    string custSql = $@"INSERT INTO Customers (Cust_ID, Cust_Name, Cust_Phone, Cust_Address, Acc_ID, {SystemConstants.CreatedBy}, {SystemConstants.CreatedAt}) 
                                        VALUES (@Code, @Name, @Phone, @Address, @AccNo, @Created_By, @Created_At)";
                    await DatabaseHelper.ExecuteNonQueryAsync(custSql, new[] {
                        new SqlParameter("@Code", cust_ID.Text.Trim()), new SqlParameter("@Name", cust_Name.Text.Trim()),
                        new SqlParameter("@Phone", cust_Phone.Text.Trim()), new SqlParameter("@Address", cust_Address.Text.Trim()),
                        new SqlParameter("@AccNo", acc_ID.Text.Trim()), new SqlParameter("@Created_By", UserSession.UserId),
                        new SqlParameter("@Created_At", DateTime.Now)
                    }, trans);
                }
                else if (CurrentMode == FormMode.Edit)
                {
                    string updateAcc = $@"UPDATE Accounts SET Acc_Name = @Name, {SystemConstants.UpdatedBy} = @Updated_By, {SystemConstants.UpdatedAt} = @Updated_At WHERE Acc_ID = @AccID";
                    await DatabaseHelper.ExecuteNonQueryAsync(updateAcc, new[] {
                        new SqlParameter("@Name", cust_Name.Text.Trim()), new SqlParameter("@AccID", acc_ID.Text.Trim()),
                        new SqlParameter("@Updated_By", UserSession.UserId), new SqlParameter("@Updated_At", DateTime.Now)
                    }, trans);

                    string updateCust = $@"UPDATE Customers SET Cust_Name = @Name, Cust_Phone = @Phone, Cust_Address = @Address, {SystemConstants.UpdatedBy} = @Updated_By, {SystemConstants.UpdatedAt} = @Updated_At 
                                           WHERE Cust_ID = @Code AND RowVersion = @OldRowVersion";

                    int rowsAffected = await DatabaseHelper.ExecuteNonQueryAsync(updateCust, new[] {
                        new SqlParameter("@Name", cust_Name.Text.Trim()), new SqlParameter("@Phone", cust_Phone.Text.Trim()),
                        new SqlParameter("@Address", cust_Address.Text.Trim()), new SqlParameter("@Code", cust_ID.Text.Trim()),
                        new SqlParameter("@Updated_By", UserSession.UserId), new SqlParameter("@Updated_At", DateTime.Now),
                        new SqlParameter("@OldRowVersion", SqlDbType.Timestamp) { Value = _currentRowVersion ?? (object)DBNull.Value }
                    }, trans);

                    if (rowsAffected == 0) throw new InvalidOperationException("تضارب بيانات: تم تعديل بيانات هذا العميل من قبل مستخدم آخر أثناء استعراضك لها.");

                    DatabaseHelper.LogAuditTransaction(trans, "Customers", cust_ID.Text.Trim(), "UPDATE", "تم الحفظ المسبق", $"الاسم: {cust_Name.Text.Trim()}", "تعديل عميل");
                }

                await DatabaseHelper.ExecuteNonQueryAsync("DELETE FROM Account_Allowed_Currencies WHERE Acc_ID = @AccID", new[] { new SqlParameter("@AccID", acc_ID.Text.Trim()) }, trans);
                string curSql = "INSERT INTO Account_Allowed_Currencies (Acc_ID, Cur_ID, Is_Default, Is_Frozen, Is_Active) VALUES (@AccID, @CurID, @IsDefault, @IsFrozen, 1)";
                foreach (DataGridViewRow row in dgv_currencies.Rows)
                {
                    if (!row.IsNewRow && Convert.ToBoolean(row.Cells[0].Value))
                    {
                        await DatabaseHelper.ExecuteNonQueryAsync(curSql, new[] {
                            new SqlParameter("@AccID", acc_ID.Text.Trim()), new SqlParameter("@CurID", row.Cells[4].Value.ToString()),
                            new SqlParameter("@IsDefault", row.Cells[2].Value ?? false), new SqlParameter("@IsFrozen", row.Cells[3].Value ?? false)
                        }, trans);
                    }
                }
                return true;
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "تضارب في التزامن", MessageBoxButtons.OK, MessageBoxIcon.Stop); return false;
            }
            catch (Exception ex)
            {
                throw new Exception($"خطأ أثناء الحفظ: {ex.Message}");
            }
        }

        // 🌟 الحذف اللامتزامن المنطقي (Soft Delete)
        protected override async Task<bool> ExecuteDeleteFromDatabaseAsync(SqlTransaction transaction)
        {
            string updateCust = $@"UPDATE Customers SET {SystemConstants.IsDeleted} = 1, {SystemConstants.DeletedBy} = @UserId, {SystemConstants.DeletedAt} = GETDATE() WHERE Cust_ID = @CustID";
            await DatabaseHelper.ExecuteNonQueryAsync(updateCust, new[] {
                new SqlParameter("@CustID", cust_ID.Text.Trim()), new SqlParameter("@UserId", UserSession.UserId)
            }, transaction);

            if (!string.IsNullOrWhiteSpace(acc_ID.Text))
            {
                string updateAcc = $@"UPDATE Accounts SET Is_Stopped = 1, {SystemConstants.UpdatedBy} = @UserId, {SystemConstants.UpdatedAt} = GETDATE() WHERE Acc_ID = @AccID";
                await DatabaseHelper.ExecuteNonQueryAsync(updateAcc, new[] {
                    new SqlParameter("@AccID", acc_ID.Text.Trim()), new SqlParameter("@UserId", UserSession.UserId)
                }, transaction);
            }
            return true;
        }

        #region دوال العمليات الحسابية والتهيئة 

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
                            MessageBox.Show("إدارة النظام أوقفت التعامل مع هذه العملة على مستوى النظام بأكمله.\nلا يمكن تفعيلها لهذا العميل لحين السماح بها من الإدارة.",
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
                        if (CurrentMode != FormMode.New && !string.IsNullOrWhiteSpace(acc_ID.Text))
                        {
                            if (!AccountRepository.CanDeactivateCurrency(acc_ID.Text.Trim(), curId, 5, out string errorMsg))
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
                    MessageBox.Show("لا يمكن تجميد العملة الافتراضية للعميل.", "حماية النظام", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        // 🌟 تعبئة الحسابات بدون تجميد
        private async Task تهيئة_قائمة_الحسابات_الرئيسية_Async()
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
                MessageBox.Show("خطأ أثناء تصفية وتثبيت حسابات العملاء: " + ex.Message, "خطأ فني", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task تحميل_عملات_العميل_بناء_على_الأب_Async(string accId, string parentId)
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
                MessageBox.Show("خطأ أثناء بناء جدول العملات: " + ex.Message, "خطأ هيكلي", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                dgv_currencies.CellValueChanged += dgv_currencies_CellValueChanged;
            }
        }

        private async Task تحميل_عملات_العميل_المخزنة_Async(string accountCode, string parentCode)
        {
            try
            {
                await تحميل_عملات_العميل_بناء_على_الأب_Async(accountCode, parentCode);

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
                MessageBox.Show("خطأ أثناء جلب تفاصيل عملات العميل: " + ex.Message, "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        public override async void OnSearch()
        {
            string sqlQuery = "SELECT Cust_ID AS [كود العميل], Cust_Name AS [اسم العميل], Cust_Phone AS [رقم الهاتف] FROM Customers ORDER BY Cust_ID";
            using (AlRowad_ERP.HelpForms.UniversalSearchForm searchForm = new AlRowad_ERP.HelpForms.UniversalSearchForm("البحث عن العملاء", sqlQuery))
            {
                if (searchForm.ShowDialog() == DialogResult.OK && !string.IsNullOrEmpty(searchForm.المعرف_المختار))
                {
                    await جلب_بيانات_العميل_وتعبئتها_Async(searchForm.المعرف_المختار);
                }
            }
        }

        protected override async void RefreshData()
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(cust_ID.Text)) await جلب_بيانات_العميل_وتعبئتها_Async(cust_ID.Text.Trim());
            }
            catch { }
        }

        private string توليد_رقم_الحساب_الشجري(string parentAccID)
        {
            if (string.IsNullOrEmpty(parentAccID)) return "";
            string newAccID = parentAccID.Trim() + "0001";
            try
            {
                string query = @"SELECT MAX(CAST(RIGHT(Acc_ID, 4) AS INT)) FROM Accounts WHERE Acc_ID LIKE @ParentPattern AND LEN(Acc_ID) = LEN(@ParentID) + 4";
                object result = DatabaseHelper.ExecuteScalar(query, new[] { new SqlParameter("@ParentID", parentAccID.Trim()), new SqlParameter("@ParentPattern", parentAccID.Trim() + "%") });
                if (result != DBNull.Value && result != null) newAccID = parentAccID.Trim() + (Convert.ToInt32(result) + 1).ToString("D4");
            }
            catch (Exception ex) { System.Diagnostics.Debug.WriteLine(ex.Message); }
            return newAccID;
        }

        private async void cmb_parent_ID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CurrentMode == FormMode.New && cmb_parent_ID.SelectedValue != null)
            {
                string parentAccID = cmb_parent_ID.SelectedValue.ToString();
                acc_ID.Text = توليد_رقم_الحساب_الشجري(parentAccID);
                await تحميل_عملات_العميل_بناء_على_الأب_Async(acc_ID.Text.Trim(), parentAccID);
            }
        }

        // 🌟 التأكد من المراجع قبل الحذف المنطقي
        protected override async Task<bool> ValidateDependenciesBeforeDeleteAsync()
        {
            string customerId = cust_ID.Text;

            if (await IsRecordUsedInTableAsync("Invoice_Header", "Cust_ID", customerId))
            {
                MessageBox.Show("منع أمني: لا يمكن حذف العميل لارتباطه بفواتير مبيعات سابقة.", "ارتباط مالي", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }

         /*  if (await IsRecordUsedInTableAsync("Sales_Quotations", "Customer_ID", customerId))
            {
                MessageBox.Show("منع أمني: العميل مرتبط بعروض أسعار مسجلة.", "ارتباط مرجعي", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }
             */
            return true;
        }
    }
}