using AlRowad_ERP.Core;
using AlRowad_ERP.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Threading.Tasks; // 🌟 إضافة مكتبة التزامن
using System.Windows.Forms;
using AlRowad_ERP.UI.Base;
using AlRowad_ERP.Core.Constants;


namespace AlRowad_ERP.Forms
{
    public partial class AccountsForm : BaseEntryForm
    {
        private byte[] _currentRowVersion;
        private DataTable dtAccountsChart = new DataTable();

        private int CurrentAccountLevel
        {
            get { return int.TryParse(account_Level.Text, out int level) ? level : 1; }
        }

        public AccountsForm()
        {
            InitializeComponent();
            this.Load += AccountsForm_Load;
            PrimaryIdFieldName = "acc_ID";
            this.MainTableName = "Accounts";

            if (this.treeAccounts != null)
            {
                this.treeAccounts.NodeMouseDoubleClick += treeAccounts_NodeMouseDoubleClick;
                treeAccounts.BeforeExpand += treeAccounts_BeforeExpand;
                treeAccounts.BeforeCollapse += treeAccounts_BeforeCollapse;
                this.treeAccounts.BeforeExpand += TreeAccounts_BeforeExpandCollapse;
                this.treeAccounts.BeforeCollapse += TreeAccounts_BeforeExpandCollapse;
                this.dgv_currencies.CurrentCellDirtyStateChanged += dgv_currencies_CurrentCellDirtyStateChanged;
            }
        }

        private void TreeAccounts_BeforeExpandCollapse(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Action == TreeViewAction.ByMouse) e.Cancel = true;
        }

        // 🌟 جعل حدث التحميل غير متزامن لعدم تجميد الشاشة أثناء بناء الشجرة
        private async void AccountsForm_Load(object sender, EventArgs e)
        {
            try
            {
                treeAccounts.RightToLeft = RightToLeft.Yes;
                treeAccounts.RightToLeftLayout = true;

                await Task.Run(() => تهيئة_القوائم_المنسدلة_للحسابات()); // جلب القوائم في الخلفية
                await BuildAccountsTreeStructureAsync(); // بناء الشجرة في الخلفية

                ChangeFormMode(FormMode.View);
                this.parent_ID.TextChanged += parent_ID_TextChanged;
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ أثناء تحميل بيانات الشاشة: " + ex.Message, "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #region محرك إدارة الحالات (الامتثال لـ BaseEntryForm)
        protected override void LockControls(Control parent, bool isReadOnly)
        {
            base.LockControls(parent, isReadOnly);

            if (dgv_currencies != null)
            {
                dgv_currencies.AllowUserToAddRows = false;
                dgv_currencies.AllowUserToDeleteRows = false;
                dgv_currencies.Enabled = true;
                dgv_currencies.ReadOnly = isReadOnly;

                if (!isReadOnly && CurrentAccountLevel != 4 && CurrentAccountLevel != 5)
                    dgv_currencies.ReadOnly = true;
            }

            if (treeAccounts != null) treeAccounts.Enabled = isReadOnly;
            if (acc_ID != null) acc_ID.ReadOnly = (CurrentMode != FormMode.New);
        }
        #endregion

        #region بناء الشجرة والقوائم (محرك غير متزامن)

        // 🌟 بناء الشجرة بدون تجميد واجهة المستخدم
        private async Task BuildAccountsTreeStructureAsync()
        {
            if (treeAccounts == null) return;
            try
            {
                // جلب البيانات في خيط منفصل (Background Thread)
                dtAccountsChart = await Task.Run(() => AccountRepository.GetAccountsTree());
                if (dtAccountsChart == null || dtAccountsChart.Rows.Count == 0) return;

                // التحديث يجب أن يتم على خيط الواجهة الأساسي
                treeAccounts.BeginUpdate();
                treeAccounts.Nodes.Clear();

                DataView dvRoot = new DataView(dtAccountsChart);
                dvRoot.RowFilter = "Parent_ID IS NULL OR Parent_ID = '' OR Parent_ID = '0' OR Parent_ID = 'root'";

                foreach (DataRowView rowView in dvRoot)
                {
                    TreeNode rootNode = new TreeNode
                    {
                        Tag = rowView["Acc_ID"].ToString().Trim(),
                        Text = rowView["Acc_ID"].ToString().Trim() + " - " + rowView["Acc_Name"].ToString().Trim()
                    };
                    treeAccounts.Nodes.Add(rootNode);
                    PopulateSubAccounts(rootNode, dtAccountsChart);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ في الشجرة المحاسبية: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                treeAccounts.EndUpdate();
            }
        }

        private void PopulateSubAccounts(TreeNode parentNode, DataTable dtSource)
        {
            string parentID = parentNode.Tag.ToString().Trim();
            DataView dvChildren = new DataView(dtSource);
            dvChildren.RowFilter = $"Parent_ID = '{parentID}'";

            foreach (DataRowView rowView in dvChildren)
            {
                TreeNode childNode = new TreeNode
                {
                    Tag = rowView["Acc_ID"].ToString().Trim(),
                    Text = rowView["Acc_ID"].ToString().Trim() + " - " + rowView["Acc_Name"].ToString().Trim()
                };
                parentNode.Nodes.Add(childNode);
                PopulateSubAccounts(childNode, dtSource);
            }
        }

        private void تهيئة_القوائم_المنسدلة_للحسابات()
        {
            acc_Type.Invoke((MethodInvoker)delegate {
                acc_Type.DataSource = AccountRepository.GetDropdownData("Account_Types", "Type_ID", "Type_Name");
                acc_Type.DisplayMember = "Type_Name"; acc_Type.ValueMember = "Type_ID";
            });

            acc_Nature.Invoke((MethodInvoker)delegate {
                acc_Nature.DataSource = AccountRepository.GetDropdownData("Account_Natures", "Nature_ID", "Nature_Name");
                acc_Nature.DisplayMember = "Nature_Name"; acc_Nature.ValueMember = "Nature_ID";
            });

            if (report_Type != null)
            {
                report_Type.Invoke((MethodInvoker)delegate {
                    report_Type.DataSource = AccountRepository.GetDropdownData("Account_Reports", "Report_ID", "Report_Name");
                    report_Type.DisplayMember = "Report_Name"; report_Type.ValueMember = "Report_ID";
                });
            }
        }

        private async void treeAccounts_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node == null || string.IsNullOrEmpty(e.Node.Tag?.ToString())) return;

            if (CurrentMode != FormMode.View && CurrentMode != FormMode.RecordSelected)
            {
                if (MessageBox.Show("أنت في وضع الإدخال/التعديل حالياً. هل تريد التراجع واستعراض هذا الحساب؟", "تأكيد التراجع", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    OnCancel();
                else
                    return;
            }

            // 🌟 استدعاء الجلب اللامتزامن
            await LoadAccountDetailsDataAsync(e.Node.Tag.ToString().Trim());
        }

        private void treeAccounts_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Action == TreeViewAction.ByMouse) e.Cancel = true;
        }

        private void treeAccounts_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Action == TreeViewAction.ByMouse) e.Cancel = true;
        }

        // 🌟 جلب تفاصيل الحساب بشكل غير متزامن
        private async Task LoadAccountDetailsDataAsync(string accId)
        {
            try
            {
                DataTable dtDetails = await Task.Run(() => AccountRepository.GetAccountDetails(accId));

                if (dtDetails.Rows.Count > 0)
                {
                    ChangeFormMode(FormMode.View);
                    DataRow row = dtDetails.Rows[0];

                    if (dtDetails.Columns.Contains("RowVersion") && row["RowVersion"] != DBNull.Value)
                    {
                        _currentRowVersion = (byte[])row["RowVersion"];
                    }

                    acc_ID.Text = row["Acc_ID"].ToString();
                    acc_Name.Text = row["Acc_Name"].ToString();
                    acc_Name_En.Text = row["Acc_Name_En"] != DBNull.Value ? row["Acc_Name_En"].ToString() : "";

                    parent_ID.TextChanged -= parent_ID_TextChanged;
                    parent_ID.Text = row["Parent_ID"] != DBNull.Value ? row["Parent_ID"].ToString() : "";
                    parent_ID.TextChanged += parent_ID_TextChanged;

                    account_Level.Text = row["Account_Level"].ToString();

                    if (acc_Type != null) acc_Type.SelectedValue = row["Acc_Type"];
                    if (acc_Nature != null) acc_Nature.SelectedValue = row["Acc_Nature"];
                    if (report_Type != null) report_Type.SelectedValue = row["Report_Type"];
                    is_Stopped.Checked = Convert.ToBoolean(row["Is_Stopped"]);

                    // استخدام المحرك الآلي الداخلي للأب بدلاً من التكرار اليدوي
                    txt_CreatedBy.Text = FormatUserInfo(row["Created_By"], row["CreatedByName"]);
                    txt_CreatedAt.Text = row["Created_At"] != DBNull.Value ? Convert.ToDateTime(row["Created_At"]).ToString("yyyy/MM/dd hh:mm tt") : "";
                    txt_UpdatedBy.Text = FormatUserInfo(row["Updated_By"], row["UpdatedByName"]);
                    txt_UpdatedAt.Text = row["Updated_At"] != DBNull.Value ? Convert.ToDateTime(row["Updated_At"]).ToString("yyyy/MM/dd hh:mm tt") : "";

                    if (CurrentAccountLevel == 4 || CurrentAccountLevel == 5)
                    {
                        await تحميل_عملات_الحساب_بالجدول_Async(accId, parent_ID.Text.Trim(), CurrentAccountLevel);
                    }
                    else
                    {
                        dgv_currencies.CellValueChanged -= dgv_currencies_CellValueChanged;
                        dgv_currencies.Rows.Clear();
                        dgv_currencies.CellValueChanged += dgv_currencies_CellValueChanged;
                    }

                    ChangeFormMode(FormMode.RecordSelected);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ في جلب بيانات الحساب: " + ex.Message, "خطأ هيكلي", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        // 🌟 استدعاء أوتوماتيكي من BaseEntryForm فور نجاح الـ Commit
        protected override async void RefreshData()
        {
            try
            {
                await BuildAccountsTreeStructureAsync();

                if (!string.IsNullOrWhiteSpace(acc_ID.Text))
                {
                    await LoadAccountDetailsDataAsync(acc_ID.Text.Trim());
                }
            }
            catch { }
        }

        #region محرك الدستور المالي لجدول العملات

        // 🌟 جلب العملات بشكل لامتزامن لعدم تجميد الشاشة
        private async Task تحميل_عملات_الحساب_بالجدول_Async(string accId, string parentId, int level)
        {
            try
            {
                dgv_currencies.CellValueChanged -= dgv_currencies_CellValueChanged;

                string cleanAccId = string.IsNullOrWhiteSpace(accId) ? "0" : accId.Trim();
                string cleanParentId = string.IsNullOrWhiteSpace(parentId) ? "0" : parentId.Trim();

                DataTable dt = await Task.Run(() => AccountRepository.GetAccountCurrencies(cleanAccId, cleanParentId, level));
                dgv_currencies.Rows.Clear();

                if (level == 5 && dt.Rows.Count == 0 && CurrentMode == FormMode.New)
                {
                    int parentCurCount = await Task.Run(() => AccountRepository.CheckParentCurrenciesCount(cleanParentId));
                    if (parentCurCount == 0)
                    {
                        MessageBox.Show("تنبيه أمني: الحساب الأب المحدد لا يملك أي عملات مفعلة!\nقم باستعراض الأب وحفظ عملاته أولاً.", "سياسة العملات المتاحة", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                SyncCurrencyGridUI();
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

        private void SyncCurrencyGridUI()
        {
            if (dgv_currencies == null || dgv_currencies.Rows.Count == 0) return;
            bool isGridReadOnly = dgv_currencies.ReadOnly;

            foreach (DataGridViewRow row in dgv_currencies.Rows)
            {
                if (row.IsNewRow) continue;
                bool isActive = Convert.ToBoolean(row.Cells[0].Value ?? false);

                if (!isGridReadOnly)
                {
                    row.Cells[2].ReadOnly = !isActive;
                    row.Cells[3].ReadOnly = !isActive;
                }
                row.DefaultCellStyle.BackColor = isActive ? Color.White : Color.FromArgb(240, 240, 240);
            }
        }

        private void dgv_currencies_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgv_currencies.IsCurrentCellDirty && CurrentMode != FormMode.View)
                dgv_currencies.CommitEdit(DataGridViewDataErrorContexts.Commit);
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
                            MessageBox.Show("إدارة النظام أوقفت التعامل مع هذه العملة على مستوى النظام بأكمله.\nلا يمكن تفعيلها لهذا الحساب لحين السماح بها من قِبل الإدارة العليا.",
                                            "حظر سيادي - نظام الرواد", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            row.Cells[0].Value = false;
                            return;
                        }

                        row.Cells[2].ReadOnly = false;
                        row.Cells[3].ReadOnly = false;
                        row.DefaultCellStyle.BackColor = Color.White;
                    }
                    else
                    {
                        if (CurrentMode != FormMode.New && !AccountRepository.CanDeactivateCurrency(acc_ID.Text.Trim(), curId, CurrentAccountLevel, out string errorMsg))
                        {
                            MessageBox.Show(errorMsg, "رقابة النظام المالي", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            row.Cells[0].Value = true;
                            return;
                        }

                        row.Cells[2].ReadOnly = true; row.Cells[2].Value = false;
                        row.Cells[3].ReadOnly = true; row.Cells[3].Value = false;
                        row.DefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);
                    }
                }
                else if (e.ColumnIndex == 2 && isDefault)
                {
                    if (!isActive)
                    {
                        row.Cells[0].Value = true;
                        row.Cells[2].ReadOnly = false;
                        row.Cells[3].ReadOnly = false;
                        row.DefaultCellStyle.BackColor = Color.White;
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
                    MessageBox.Show("قواعد الأعمال: لا يمكن تجميد العملة الافتراضية للنظام.", "منع التجميد", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            finally
            {
                dgv_currencies.CellValueChanged += dgv_currencies_CellValueChanged;
            }
        }
        #endregion

        #region العمليات السيادية (CRUD)

        public override async void OnNew()
        {
            if (treeAccounts.SelectedNode == null)
            {
                MessageBox.Show("يرجى تحديد الحساب الأب من الشجرة أولاً لإضافة حساب فرعي.", "توجيه إجباري", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            base.OnNew();

            int selectedNodeLevel = treeAccounts.SelectedNode.Level + 1;
            string parentIdValue = (selectedNodeLevel >= 5)
                ? (treeAccounts.SelectedNode.Parent != null ? treeAccounts.SelectedNode.Parent.Tag.ToString() : parent_ID.Text.Trim())
                : treeAccounts.SelectedNode.Tag.ToString();

            int calculatedLevel = (selectedNodeLevel >= 5) ? 5 : selectedNodeLevel + 1;

            parent_ID.TextChanged -= parent_ID_TextChanged;
            parent_ID.Text = parentIdValue;
            parent_ID.TextChanged += parent_ID_TextChanged;

            account_Level.Text = calculatedLevel.ToString();

            if (!string.IsNullOrWhiteSpace(parentIdValue))
                acc_ID.Text = await Task.Run(() => AccountRepository.GetNextAccountID(parentIdValue));

            acc_Type.SelectedIndex = 0; acc_Nature.SelectedIndex = 0;
            if (report_Type != null) report_Type.SelectedIndex = 0;
            is_Stopped.Checked = false;

            if (CurrentAccountLevel == 4 || CurrentAccountLevel == 5)
                await تحميل_عملات_الحساب_بالجدول_Async(acc_ID.Text.Trim(), parentIdValue, CurrentAccountLevel);
            else
            {
                dgv_currencies.CellValueChanged -= dgv_currencies_CellValueChanged;
                dgv_currencies.Rows.Clear();
                dgv_currencies.CellValueChanged += dgv_currencies_CellValueChanged;
            }

            ChangeFormMode(FormMode.New);
            acc_Name.Focus();
        }

        protected override async void OnAddFrom()
        {
            if (string.IsNullOrWhiteSpace(acc_ID.Text))
            {
                MessageBox.Show("يرجى اختيار الحساب المراد النسخ منه من الشجرة أولاً.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning); return;
            }
            ChangeFormMode(FormMode.New);
            acc_ID.Text = await Task.Run(() => AccountRepository.GetNextAccountID(parent_ID.Text.Trim()));
            acc_Name.Text += " - نسخة";
            acc_Name.Focus(); acc_Name.SelectAll();
        }

        public override void OnEdit()
        {
            if (string.IsNullOrWhiteSpace(acc_ID.Text))
            {
                MessageBox.Show("يرجى اختيار الحساب المراد تعديله من الشجرة أولاً.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning); return;
            }
            ChangeFormMode(FormMode.Edit);
            if (CurrentAccountLevel == 4 || CurrentAccountLevel == 5) SyncCurrencyGridUI();
            acc_Name.Focus();
        }

        public override void OnSave()
        {
            if (string.IsNullOrWhiteSpace(acc_ID.Text) || string.IsNullOrWhiteSpace(acc_Name.Text))
            {
                MessageBox.Show("يرجى ملء الحقول السيادية للحساب قبل الحفظ.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning); return;
            }
            base.OnSave();
        }

        // 🌟 تحويل دالة الحفظ لتعمل بشكل لامتزامن (Async) مع الالتزام بالـ SystemConstants
        protected override async Task<bool> ExecuteSaveToDatabaseAsync(SqlTransaction trans)
        {
            if (CurrentAccountLevel == 4 || CurrentAccountLevel == 5)
            {
                bool hasSelectedCurrency = false, hasDefaultCurrency = false;
                foreach (DataGridViewRow row in dgv_currencies.Rows)
                {
                    if (row.IsNewRow) continue;
                    bool act = Convert.ToBoolean(row.Cells[0].Value ?? false);
                    bool isDef = Convert.ToBoolean(row.Cells[2].Value ?? false);
                    bool frz = Convert.ToBoolean(row.Cells[3].Value ?? false);

                    if (isDef && frz)
                    {
                        MessageBox.Show($"منع الحفظ: تناقض منطقي في العملة. لا يمكن أن تكون 'افتراضية' و'مجمدة' معاً.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return false;
                    }
                    if (act || frz) { hasSelectedCurrency = true; if (isDef) hasDefaultCurrency = true; }
                }

                if (!hasSelectedCurrency || !hasDefaultCurrency)
                {
                    MessageBox.Show("يرجى تفعيل عملة واحدة على الأقل وتحديد العملة الافتراضية.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }

            try
            {
                // 🌟 استخدام الثوابت المركزية (No Magic Strings)
                string sqlQuery = (CurrentMode == FormMode.Edit)
                    ? $@"UPDATE Accounts SET Acc_Name = @Acc_Name, Acc_Name_En = @Acc_Name_En, Parent_ID = @Parent_ID, Account_Level = @Account_Level, 
                        Acc_Type = @Acc_Type, Acc_Nature = @Acc_Nature, Report_Type = @Report_Type, Is_Stopped = @Is_Stopped,
                        {SystemConstants.UpdatedBy} = @Updated_By, {SystemConstants.UpdatedAt} = @Updated_At 
                        WHERE Acc_ID = @Acc_ID AND RowVersion = @OldRowVersion"
                    : $@"INSERT INTO Accounts (Acc_ID, Acc_Name, Acc_Name_En, Parent_ID, Account_Level, Acc_Type, Acc_Nature, Report_Type, Is_Stopped, {SystemConstants.CreatedBy}, {SystemConstants.CreatedAt}) 
                        VALUES (@Acc_ID, @Acc_Name, @Acc_Name_En, @Parent_ID, @Account_Level, @Acc_Type, @Acc_Nature, @Report_Type, @Is_Stopped, @Created_By, @Created_At)";

                int.TryParse(acc_Type.SelectedValue?.ToString(), out int typeVal);
                int.TryParse(acc_Nature.SelectedValue?.ToString(), out int natureVal);
                int.TryParse(report_Type?.SelectedValue?.ToString(), out int reportVal);

                var pHeader = new List<SqlParameter>
                {
                    new SqlParameter("@Acc_ID", acc_ID.Text.Trim()),
                    new SqlParameter("@Acc_Name", acc_Name.Text.Trim()),
                    new SqlParameter("@Acc_Name_En", string.IsNullOrWhiteSpace(acc_Name_En.Text) ? (object)DBNull.Value : acc_Name_En.Text.Trim()),
                    new SqlParameter("@Account_Level", CurrentAccountLevel),
                    new SqlParameter("@Acc_Type", typeVal),
                    new SqlParameter("@Acc_Nature", natureVal),
                    new SqlParameter("@Report_Type", reportVal),
                    new SqlParameter("@Is_Stopped", is_Stopped.Checked),
                    new SqlParameter("@Parent_ID", string.IsNullOrWhiteSpace(parent_ID.Text) ? (object)DBNull.Value : parent_ID.Text.Trim())
                };

                if (CurrentMode == FormMode.New)
                {
                    pHeader.Add(new SqlParameter("@Created_By", UserSession.UserId));
                    pHeader.Add(new SqlParameter("@Created_At", DateTime.Now));
                }
                else if (CurrentMode == FormMode.Edit)
                {
                    pHeader.Add(new SqlParameter("@Updated_By", UserSession.UserId));
                    pHeader.Add(new SqlParameter("@Updated_At", DateTime.Now));
                    pHeader.Add(new SqlParameter("@OldRowVersion", SqlDbType.Timestamp) { Value = _currentRowVersion ?? (object)DBNull.Value });
                }

                // 🌟 تنفيذ لامتزامن (Async)
                int rowsAffected = await DatabaseHelper.ExecuteNonQueryAsync(sqlQuery, pHeader.ToArray(), trans);

                if (CurrentMode == FormMode.Edit && rowsAffected == 0)
                {
                    throw new InvalidOperationException("تضارب بيانات: تم تعديل بيانات هذا الحساب من قبل مستخدم آخر أثناء استعراضك له. يرجى تحديث الشجرة والمحاولة مجدداً.");
                }

                if (CurrentMode == FormMode.Edit)
                {
                    string oldValues = "تم الحفظ المسبق";
                    string newValues = $"الاسم الجديد: {acc_Name.Text.Trim()} | إيقاف: {is_Stopped.Checked}";
                    DatabaseHelper.LogAuditTransaction(trans, "Accounts", acc_ID.Text.Trim(), "UPDATE", oldValues, newValues, "تعديل تفاصيل الحساب");
                }

                if (CurrentAccountLevel == 4 || CurrentAccountLevel == 5)
                {
                    await DatabaseHelper.ExecuteNonQueryAsync("DELETE FROM Account_Allowed_Currencies WHERE Acc_ID = @AccID", new[] { new SqlParameter("@AccID", acc_ID.Text.Trim()) }, trans);
                    string insertCur = @"INSERT INTO Account_Allowed_Currencies (Acc_ID, Cur_ID, Is_Active, Is_Frozen, Is_Default) VALUES (@AccID, @CurrID, @IsActive, @IsFrozen, @IsDefault)";

                    foreach (DataGridViewRow row in dgv_currencies.Rows)
                    {
                        if (row.IsNewRow || row.Cells[4].Value == null) continue;
                        bool act = Convert.ToBoolean(row.Cells[0].Value ?? false);
                        bool isDef = Convert.ToBoolean(row.Cells[2].Value ?? false);
                        bool frz = Convert.ToBoolean(row.Cells[3].Value ?? false);

                        if (act || frz)
                        {
                            SqlParameter[] pCur = {
                                new SqlParameter("@AccID", acc_ID.Text.Trim()),
                                new SqlParameter("@CurrID", row.Cells[4].Value.ToString()),
                                new SqlParameter("@IsActive", act),
                                new SqlParameter("@IsFrozen", frz),
                                new SqlParameter("@IsDefault", isDef)
                            };
                            await DatabaseHelper.ExecuteNonQueryAsync(insertCur, pCur, trans);
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
                throw new Exception($"خطأ في عملية الحفظ: {ex.Message}");
            }
        }

        // 🌟 حذف مخصص يلتزم بـ async 
        public override async void OnDelete()
        {
            if (string.IsNullOrEmpty(acc_ID.Text) || CurrentMode != FormMode.View && CurrentMode != FormMode.RecordSelected)
            {
                MessageBox.Show("يجب استعراض الحساب المراد حذفه أولاً.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning); return;
            }

            if (MessageBox.Show($"هل أنت متأكد من حذف الحساب: ({acc_Name.Text})؟", "تأكيد", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    // الحذف يتم بداخل الـ Repository
                    await Task.Run(() => AccountRepository.DeleteAccount(acc_ID.Text.Trim()));
                    MessageBox.Show("تم حذف الحساب بنجاح.", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    acc_ID.Clear(); acc_Name.Clear();
                    ChangeFormMode(FormMode.View);
                    await BuildAccountsTreeStructureAsync();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "حماية النظام", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
        }

        private async void parent_ID_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(parent_ID.Text)) return;
            try
            {
                int parentLevel = await Task.Run(() => AccountRepository.GetAccountLevel(parent_ID.Text.Trim()));
                if (parentLevel > 0)
                {
                    int calculatedLevel = parentLevel + 1;
                    account_Level.Text = calculatedLevel.ToString();

                    if ((calculatedLevel == 4 || calculatedLevel == 5) && CurrentMode == FormMode.New)
                        await تحميل_عملات_الحساب_بالجدول_Async(acc_ID.Text.Trim(), parent_ID.Text.Trim(), calculatedLevel);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ في قراءة مستوى الحساب الأب: " + ex.Message, "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // 🌟 الترقية إلى النسخة اللامتزامنة للفحص المرجعي قبل الحذف (Async)
        protected override async Task<bool> ValidateDependenciesBeforeDeleteAsync()
        {
            string accountId = acc_ID.Text;

            if (await IsRecordUsedInTableAsync("System_Ledger", "Acc_ID", accountId))
            {
                MessageBox.Show("منع أمني: لا يمكن حذف هذا الحساب لارتباطه بقيود وحركات مالية مسجلة في دفتر الأستاذ. يمكنك إيقاف تفعيله فقط.",
                                "ارتباط مالي", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }

            if (await IsRecordUsedInTableAsync("Cash_Voucher_Header", "Box_Acc_ID", accountId))
            {
                MessageBox.Show("منع أمني: الحساب مستخدم كصندوق/بنك في سندات قبض سابقة.",
                                "ارتباط مالي", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }

            return true;
        }

        private void btnCollapseAll_Click(object sender, EventArgs e) { if (treeAccounts != null) treeAccounts.CollapseAll(); }
        private void btnExpandAll_Click(object sender, EventArgs e) { if (treeAccounts != null) treeAccounts.ExpandAll(); }
        private async void Refresh_Click(object sender, EventArgs e) { await BuildAccountsTreeStructureAsync(); }
        #endregion
    }
}