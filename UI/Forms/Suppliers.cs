using AlRowad_ERP.Core;
using AlRowad_ERP.Core.Constants;
using AlRowad_ERP.Data;
using AlRowad_ERP.UI.Base;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AlRowad_ERP.Forms
{
    public partial class Suppliers : BaseEntryForm
    {
        private byte[] _currentRowVersion;
        private readonly SupplierRepository _supplierRepo;

        public Suppliers()
        {
            InitializeComponent();

            // 🌟 الربط الدستوري لمحرك Auto-Bind
            PrimaryIdFieldName = "txt_Supp_ID";
            this.MainTableName = SystemConstants.Tables.Suppliers;

            // الفحص الآلي للارتباطات المرجعية
            this.DeleteDependencies.Add("Purchase_Invoices", "Vendor_ID");
            this.DeleteDependencies.Add("Purchase_Orders", "Vendor_ID");

            _supplierRepo = new SupplierRepository();

            this.Load += Suppliers_Load;
            this.dgv_currencies.CurrentCellDirtyStateChanged += dgv_currencies_CurrentCellDirtyStateChanged;
            this.dgv_currencies.CellValueChanged += dgv_currencies_CellValueChanged;
        }
        private async void Suppliers_Load(object sender, EventArgs e)
        {
            try
            {
                dgv_currencies.Rows.Clear();
                await تهيئة_قائمة_الحسابات_الرئيسية_Async();
                ChangeFormMode(FormMode.View);
            }
            catch (Exception ex) { LogError(ex); }
        }

        #region إدارة الحالة السيادية
        protected override void LockControls(Control parent, bool isReadOnly)
        {
            base.LockControls(parent, isReadOnly);

            if (txt_Supp_ID != null) txt_Supp_ID.ReadOnly = (CurrentMode != FormMode.New);
            if (txt_Acc_ID != null) txt_Acc_ID.ReadOnly = (CurrentMode != FormMode.New);

            if (dgv_currencies != null)
            {
                dgv_currencies.Enabled = !isReadOnly;
                dgv_currencies.ReadOnly = isReadOnly;
                dgv_currencies.AllowUserToAddRows = false;
                dgv_currencies.AllowUserToDeleteRows = false;

                if (!isReadOnly) تهيئة_حالة_خلايا_الجدول_الافتراضية();
            }
        }
        #endregion

        #region عمليات الربط اللامتزامن
        private async Task تهيئة_قائمة_الحسابات_الرئيسية_Async()
        {
            try
            {
                DataTable dt = await _supplierRepo.GetMainAccountsAsync();
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
            catch (Exception ex) { MessageBox.Show("خطأ تهيئة حسابات الموردين: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        protected override string GetNextId()
        {
            try { return _supplierRepo.GetNextSupplierId(); }
            catch { return "0001"; }
        }

        public override void OnNew()
        {
            base.OnNew();
            if (cmb_parent_ID != null) cmb_parent_ID.SelectedIndex = -1;
            if (chk_Is_Farmer != null) chk_Is_Farmer.Checked = false;
            txt_Acc_ID.Clear();
            dgv_currencies.Rows.Clear();
            txt_Supp_Name.Focus();
        }

        public override void OnEdit()
        {
            if (string.IsNullOrWhiteSpace(txt_Supp_ID.Text))
            {
                MessageBox.Show("يرجى اختيار المورد أولاً.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            base.OnEdit();
            txt_Supp_Name.Focus();
        }

        public override void OnCancel()
        {
            base.OnCancel();
            if (CurrentMode == FormMode.New || (CurrentMode == FormMode.View && string.IsNullOrWhiteSpace(txt_Supp_ID.Text)))
            {
                dgv_currencies.Rows.Clear();
            }
        }
        #endregion

        #region الحفظ والحذف السيادي (ACID Transactions)
        protected override async Task<bool> ExecuteSaveToDatabaseAsync(SqlTransaction trans)
        {
            if (string.IsNullOrWhiteSpace(txt_Supp_Name.Text) || string.IsNullOrWhiteSpace(txt_Acc_ID.Text))
            {
                MessageBox.Show("يجب إدخال اسم المورد ورقم الحساب المالي.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!ValidateCurrenciesRules()) return false;

            try
            {
                bool isNew = (CurrentMode == FormMode.New);
                DataTable dtCurrencies = GetCurrenciesFromGrid();
                bool isFarmer = chk_Is_Farmer != null && chk_Is_Farmer.Checked;

                // 🌟 تمرير رقم المستخدم (ID) حصراً كما ينص الدستور
                int currentUserId = this.CurrentUserId > 0 ? this.CurrentUserId : UserSession.UserId;

                await _supplierRepo.SaveSupplierTransactionAsync(
                    txt_Supp_ID.Text.Trim(), txt_Supp_Name.Text.Trim(),
                    txt_Supp_Phone.Text.Trim(), txt_Supp_Address.Text.Trim(),
                    txt_Acc_ID.Text.Trim(), cmb_parent_ID.SelectedValue,
                    isFarmer, _currentRowVersion, dtCurrencies, currentUserId, isNew, trans);

                if (!isNew)
                {
                    string newValues = $"الاسم: {txt_Supp_Name.Text.Trim()} | مزارع: {isFarmer}";
                    DatabaseHelper.LogAuditTransaction(trans, SystemConstants.Tables.Suppliers, txt_Supp_ID.Text.Trim(), "UPDATE", "مسبق", newValues, "تعديل مورد");
                }

                // 🌟 الإجراء الدستوري: نعيد true فوراً!
                // الفئة الأب (BaseEntryForm) ستتولى عمل Commit بأمان تام.
                // لتحديث الواجهة وعرض أسماء المستخدمين الجديدة، نعتمد على استدعاء جلب البيانات 
                // الذي يحدث عادة بعد الحفظ، أو عند قيام المستخدم بالبحث (Refresh).
                return true;
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "تضارب التزامن", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }
        }
            protected override async Task<bool> ExecuteDeleteFromDatabaseAsync(SqlTransaction transaction)
        {
            return await _supplierRepo.ExecuteDeleteAsync(txt_Supp_ID.Text.Trim(), txt_Acc_ID.Text.Trim(), UserSession.UserId, transaction);
        }

        protected override async Task<bool> ValidateDependenciesBeforeDeleteAsync()
        {
            string vendorId = txt_Supp_ID.Text.Trim();
            if (await IsRecordUsedInTableAsync("Purchase_Invoices", "Vendor_ID", vendorId) ||
                await IsRecordUsedInTableAsync("Purchase_Orders", "Vendor_ID", vendorId))
            {
                MessageBox.Show("منع أمني: المورد مرتبط بعمليات سابقة ولا يمكن حذفه.", "ارتباط مرجعي", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }
            return true;
        }
        #endregion

        #region محرك البحث (F9) وعرض البيانات الموحد
        public override async void OnSearch()
        {
            using (var searchForm = new AlRowad_ERP.HelpForms.UniversalSearchForm("البحث عن الموردين", _supplierRepo.GetSearchQuery()))
            {
                if (searchForm.ShowDialog() == DialogResult.OK && !string.IsNullOrEmpty(searchForm.المعرف_المختار))
                {
                    await جلب_بيانات_المورد_وتعبئتها_Async(searchForm.المعرف_المختار);
                }
            }
        }

        protected override async void RefreshData()
        {
            if (!string.IsNullOrWhiteSpace(txt_Supp_ID.Text))
                await جلب_بيانات_المورد_وتعبئتها_Async(txt_Supp_ID.Text.Trim());
        }

        private async Task جلب_بيانات_المورد_وتعبئتها_Async(string supplierCode)
        {
            try
            {
                DataTable dt = await _supplierRepo.GetSupplierDataAsync(supplierCode);
                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    if (dt.Columns.Contains("RowVersion") && row["RowVersion"] != DBNull.Value)
                        _currentRowVersion = (byte[])row["RowVersion"];

                    AutoBindRecord(row); // 🌟 محرك الـ Auto-Bind سيربط Is_Farmer آلياً عبر خاصية Tag

                    if (cmb_parent_ID != null && txt_Acc_ID.Text.Length >= 6)
                        cmb_parent_ID.SelectedValue = txt_Acc_ID.Text.Substring(0, 6);

                    ChangeFormMode(FormMode.RecordSelected);
                    await تحميل_عملات_المورد_المخزنة_Async(txt_Acc_ID.Text.Trim(), cmb_parent_ID.SelectedValue?.ToString() ?? "");
                }
            }
            catch (Exception ex) { MessageBox.Show("خطأ جلب بيانات المورد: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }
        #endregion

        #region إدارة العملات المعقدة (منزوعة SQL)
        private DataTable GetCurrenciesFromGrid()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Is_Active", typeof(bool));
            dt.Columns.Add("Is_Frozen", typeof(bool));
            dt.Columns.Add("Is_Default", typeof(bool));
            dt.Columns.Add("Cur_ID", typeof(string));

            if (dgv_currencies != null)
            {
                foreach (DataGridViewRow row in dgv_currencies.Rows)
                {
                    if (row.IsNewRow || row.Cells[4].Value == null) continue;
                    bool act = Convert.ToBoolean(row.Cells[0].Value);
                    bool frz = Convert.ToBoolean(row.Cells[3].Value);
                    if (act || frz)
                    {
                        dt.Rows.Add(act, frz, Convert.ToBoolean(row.Cells[2].Value), row.Cells[4].Value.ToString());
                    }
                }
            }
            return dt;
        }

        private bool ValidateCurrenciesRules()
        {
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
                        MessageBox.Show("لا يمكن للعملة أن تكون 'افتراضية' و'مجمدة' معاً.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return false;
                    }
                    if (act || frz)
                    {
                        hasSelectedCurrency = true;
                        if (isDef) hasDefaultCurrency = true;
                    }
                }
                if (!hasSelectedCurrency || !hasDefaultCurrency)
                {
                    MessageBox.Show("يجب تفعيل عملة وتحديد عملة افتراضية.", "منع الحفظ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }
            return true;
        }

        private async Task تحميل_عملات_المورد_بناء_على_الأب_Async(string accId, string parentId)
        {
            try
            {
                dgv_currencies.CellValueChanged -= dgv_currencies_CellValueChanged;
                string cleanAccId = string.IsNullOrWhiteSpace(accId) ? "0" : accId.Trim();
                string cleanParentId = string.IsNullOrWhiteSpace(parentId) ? "0" : parentId.Trim();

                DataTable dt = await Task.Run(() => AccountRepository.GetAccountCurrencies(cleanAccId, cleanParentId, 5));
                dgv_currencies.Rows.Clear();

                foreach (DataRow row in dt.Rows)
                {
                    int rowIndex = dgv_currencies.Rows.Add(Convert.ToBoolean(row["Is_Active"]), row["Cur_Name"].ToString(), Convert.ToBoolean(row["Is_Default"]), Convert.ToBoolean(row["Is_Frozen"]), row["Cur_ID"]);
                    dgv_currencies.Rows[rowIndex].Tag = Convert.ToBoolean(row["Global_Active"]);
                }
                تهيئة_حالة_خلايا_الجدول_الافتراضية();
            }
            finally { dgv_currencies.CellValueChanged += dgv_currencies_CellValueChanged; }
        }

        private async Task تحميل_عملات_المورد_المخزنة_Async(string accountCode, string parentCode)
        {
            await تحميل_عملات_المورد_بناء_على_الأب_Async(accountCode, parentCode);
            if (string.IsNullOrEmpty(accountCode)) return;

            DataTable dt = await _supplierRepo.GetSupplierCurrenciesAsync(accountCode);
            foreach (DataRow dbRow in dt.Rows)
            {
                string dbCurId = dbRow["Cur_ID"].ToString().Trim();
                foreach (DataGridViewRow gridRow in dgv_currencies.Rows)
                {
                    if (gridRow.IsNewRow) continue;
                    if (gridRow.Cells[4].Value?.ToString().Trim() == dbCurId)
                    {
                        gridRow.Cells[0].Value = Convert.ToBoolean(dbRow["Is_Active"]);
                        gridRow.Cells[2].Value = Convert.ToBoolean(dbRow["Is_Default"]);
                        gridRow.Cells[3].Value = Convert.ToBoolean(dbRow["Is_Frozen"]);
                        break;
                    }
                }
            }
            تهيئة_حالة_خلايا_الجدول_الافتراضية();
        }

        private async void cmb_parent_ID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CurrentMode == FormMode.New && cmb_parent_ID.SelectedValue != null)
            {
                string parentAccID = cmb_parent_ID.SelectedValue.ToString();
                txt_Acc_ID.Text = _supplierRepo.GenerateChildAccountId(parentAccID);
                await تحميل_عملات_المورد_بناء_على_الأب_Async(txt_Acc_ID.Text.Trim(), parentAccID);
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
                bool isGloballyActive = Convert.ToBoolean(row.Tag ?? true);

                if (e.ColumnIndex == 0)
                {
                    if (isActive && !isGloballyActive)
                    {
                        MessageBox.Show("العملة موقوفة سيادياً من الإدارة.", "منع", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        row.Cells[0].Value = false;
                        return;
                    }
                    row.Cells[2].ReadOnly = !isActive;
                    row.Cells[3].ReadOnly = !isActive;
                    row.DefaultCellStyle.BackColor = isActive ? System.Drawing.Color.White : System.Drawing.Color.FromArgb(240, 240, 240);
                    if (!isActive) { row.Cells[2].Value = false; row.Cells[3].Value = false; }
                }
                else if (e.ColumnIndex == 2 && isDefault)
                {
                    if (!isActive) { row.Cells[0].Value = true; row.Cells[2].ReadOnly = false; }
                    if (isFrozen) row.Cells[3].Value = false;
                    foreach (DataGridViewRow r in dgv_currencies.Rows)
                        if (r.Index != e.RowIndex && !r.IsNewRow) r.Cells[2].Value = false;
                }
            }
            finally { dgv_currencies.CellValueChanged += dgv_currencies_CellValueChanged; }
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
                    if (!isActive) { row.Cells[2].Value = false; row.Cells[3].Value = false; }
                }
            }
            finally { dgv_currencies.CellValueChanged += dgv_currencies_CellValueChanged; }
        }
        #endregion
    }
}