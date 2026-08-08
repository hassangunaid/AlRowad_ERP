using AlRowad_ERP.Core;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks; // 🌟 إضافة مكتبة التزامن
using System.Windows.Forms;
using AlRowad_ERP.Models;
using AlRowad_ERP.Data;
using AlRowad_ERP.UI.Base;
using AlRowad_ERP.Core.Constants;

namespace AlRowad_ERP.Forms
{
    public partial class ReceiptVoucher : BaseEntryForm
    {
        private bool isHandlingCurrencyChange = false;
        private int currentVoucherId = 0;
        private bool isUpdatingRadioBoxes = false;
        private byte[] _currentRowVersion;

        public ReceiptVoucher()
        {
            InitializeComponent();
            this.Load += ReceiptVoucher_Load;
            PrimaryIdFieldName = "voucher_NoTextBox";
            this.MainTableName = "Cash_Vouchers"; // 🌟 تصحيح اسم الجدول الرئيسي
        }

        // 🌟 التحميل اللامتزامن
        private async void ReceiptVoucher_Load(object sender, EventArgs e)
        {
            grid_Details.DataError += DataGridView1_DataError;
            grid_Details.CellDoubleClick += grid_Details_CellDoubleClick;
            grid_Details.RowsRemoved += grid_Details_RowsRemoved;

            rbtnCash.CheckedChanged += async (s, ev) => await rbtnCash_CheckedChangedAsync(s, ev);
            rbtnBank.CheckedChanged += async (s, ev) => await rbtnBank_CheckedChangedAsync(s, ev);

            await SetupControlsAsync();
            await SetupCurrencyGridColumnAsync();

            grid_Details.EditingControlShowing += grid_Details_EditingControlShowing;
            AttachHeaderEvents();
            grid_Details.CellValueChanged += grid_Details_CellValueChanged;
            amountTextBox.TextChanged += amountTextBox_TextChanged;
            grid_Details.CellEndEdit += grid_Details_CellEndEdit;

            if (amount_ForeignTextBox != null)
                amount_ForeignTextBox.TextChanged += amount_ForeignTextBox_TextChanged;

            AttachContextMenuToGrid(grid_Details);

            ChangeFormMode(FormMode.View);
        }

        #region التجاوب مع أحداث الشاشة الأب (BaseEntryForm)
        protected override void OnGridRowDeleted(DataGridView grid)
        {
            if (grid == grid_Details)
            {
                CalculateGridTotals();
            }
        }

        // 🌟 التحديث اللامتزامن
        protected override async void RefreshData()
        {
            try
            {
                if (currentVoucherId > 0)
                {
                    await LoadVoucherDataAsync(currentVoucherId);
                }
            }
            catch { }
        }
        #endregion

        #region إعدادات الشاشة الأساسية (Async)
        private async Task SetupControlsAsync()
        {
            voucher_DateDateTimePicker.DateValue = DateTime.Now;
            exchange_RateTextBox.Text = "1.0000";
            amount_ForeignTextBox.Text = "0.00";

            grid_Details.AutoGenerateColumns = false;
            UIHelper.FormatGridColumns(grid_Details, NumericCategory.AccountingAmount, "Amount_Credit", "Amount_Foreign");
            UIHelper.FormatGridColumn(grid_Details, "Exchange_Rate", NumericCategory.ExchangeRate);

            rbtnCash.Checked = true;
            await LoadBoxAccountsAsync(false);

            if (doc_Type_IDComboBox != null)
            {
                doc_Type_IDComboBox.DataSource = await DatabaseHelper.GetTableAsync("SELECT Doc_Type_ID, Doc_Name FROM Doc_Types WHERE Doc_Type_ID = 1");
                doc_Type_IDComboBox.DisplayMember = "Doc_Name";
                doc_Type_IDComboBox.ValueMember = "Doc_Type_ID";
                doc_Type_IDComboBox.Enabled = false;
            }
        }

        private async Task SetupCurrencyGridColumnAsync()
        {
            DataTable dtCurrencies = await DatabaseHelper.GetTableAsync("SELECT Cur_ID, Cur_Name FROM Currencies WHERE Is_Active = 1");
            DataGridViewComboBoxColumn col = (DataGridViewComboBoxColumn)grid_Details.Columns["Cur_ID"];
            col.DataSource = dtCurrencies;
            col.DisplayMember = "Cur_Name";
            col.ValueMember = "Cur_ID";
        }

        private void AttachHeaderEvents()
        {
            DetachHeaderEvents();
            box_Acc_IDComboBox.SelectedIndexChanged += async (s, e) => await box_Acc_IDComboBox_SelectedIndexChangedAsync(s, e);
            cur_IDComboBox.SelectedIndexChanged += cur_IDComboBox_SelectedIndexChanged;
            box_Acc_IDComboBox.Validating += Box_Acc_IDComboBox_Validating;
        }

        private void DetachHeaderEvents()
        {
            // إزالة الربط لتجنب التكرار
            box_Acc_IDComboBox.SelectedIndexChanged -= async (s, e) => await box_Acc_IDComboBox_SelectedIndexChangedAsync(s, e);
            cur_IDComboBox.SelectedIndexChanged -= cur_IDComboBox_SelectedIndexChanged;
            box_Acc_IDComboBox.Validating -= Box_Acc_IDComboBox_Validating;
        }
        #endregion

        #region محرك إدارة الحالات الخاص بالشاشة
        protected override void LockControls(Control parent, bool isReadOnly)
        {
            base.LockControls(parent, isReadOnly);

            grid_Details.ReadOnly = isReadOnly;
            grid_Details.AllowUserToAddRows = !isReadOnly;
            grid_Details.AllowUserToDeleteRows = !isReadOnly;
            voucher_NoTextBox.ReadOnly = true;
            exchange_RateTextBox.ReadOnly = true;

            rbtnCash.Enabled = !isReadOnly;
            rbtnBank.Enabled = !isReadOnly;

            if (this.CurrentDocumentStatus == DocumentStatus.Posted)
            {
                grid_Details.ReadOnly = true;
                grid_Details.AllowUserToAddRows = false;
                grid_Details.AllowUserToDeleteRows = false;
                amountTextBox.ReadOnly = true;
                box_Acc_IDComboBox.Enabled = false;
                cur_IDComboBox.Enabled = false;
                notesTextBox.ReadOnly = true;
                rbtnCash.Enabled = false;
                rbtnBank.Enabled = false;
            }
        }
        #endregion

        #region إدارة أزرار الراديو والصندوق/البنك (Async)
        private async Task LoadBoxAccountsAsync(bool isBank)
        {
            string filterPrefix = isBank ? "110102%" : "110101%";
            string query = $"SELECT Acc_ID, Acc_Name FROM Accounts WHERE Acc_ID LIKE '{filterPrefix}'";

            box_Acc_IDComboBox.DataSource = await DatabaseHelper.GetTableAsync(query);
            box_Acc_IDComboBox.DisplayMember = "Acc_Name";
            box_Acc_IDComboBox.ValueMember = "Acc_ID";
            box_Acc_IDComboBox.SelectedIndex = -1;
        }

        private async Task rbtnCash_CheckedChangedAsync(object sender, EventArgs e)
        {
            if (isUpdatingRadioBoxes) return;

            if (rbtnCash.Checked)
            {
                isUpdatingRadioBoxes = true;
                rbtnBank.Checked = false;
                isUpdatingRadioBoxes = false;

                await LoadBoxAccountsAsync(false);
                box_Acc_IDComboBox.SelectedIndex = -1;
                cur_IDComboBox.DataSource = null;
            }
            else if (!rbtnBank.Checked)
            {
                isUpdatingRadioBoxes = true;
                rbtnCash.Checked = true;
                isUpdatingRadioBoxes = false;
            }
        }

        private async Task rbtnBank_CheckedChangedAsync(object sender, EventArgs e)
        {
            if (isUpdatingRadioBoxes) return;

            if (rbtnBank.Checked)
            {
                isUpdatingRadioBoxes = true;
                rbtnCash.Checked = false;
                isUpdatingRadioBoxes = false;

                await LoadBoxAccountsAsync(true);
                box_Acc_IDComboBox.SelectedIndex = -1;
                cur_IDComboBox.DataSource = null;
            }
            else if (!rbtnCash.Checked)
            {
                isUpdatingRadioBoxes = true;
                rbtnBank.Checked = true;
                isUpdatingRadioBoxes = false;
            }
        }
        #endregion

        #region الفحص المشترك لعملة حساب الرأس والتفاعل اللحظي
        private bool ValidateHeaderCurrency()
        {
            if (box_Acc_IDComboBox.SelectedValue == null || box_Acc_IDComboBox.SelectedValue is DataRowView ||
                cur_IDComboBox.SelectedValue == null || cur_IDComboBox.SelectedValue is DataRowView)
                return true;

            string accId = box_Acc_IDComboBox.SelectedValue.ToString();
            int curId = Convert.ToInt32(cur_IDComboBox.SelectedValue);

            var status = CurrencyHelper.CheckCurrencyTransactionStatus(accId, curId);

            if (!status.IsGloballyActive || status.IsAccountFrozen || status.IsParentFrozen)
            {
                string msg = "";
                if (!status.IsGloballyActive)
                    msg = "إدارة النظام أوقفت التعامل مع هذه العملة سيادياً.\nلا يمكن إصدار حركات جديدة بها.";
                else if (status.IsParentFrozen)
                    msg = "عذراً! هذه العملة مجمدة إدارياً من الحساب الأب، لذلك تم إيقاف التعامل بها لجميع الحسابات الفرعية.";
                else if (status.IsAccountFrozen)
                    msg = "عذراً! العملة المختارة مجمدة لهذا الحساب (الصندوق/البنك) ولا يمكن عمل حركات عليها.";

                MessageBox.Show(msg, "تجميد إداري / سيادي", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }

            return true;
        }

        private void Box_Acc_IDComboBox_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!ValidateHeaderCurrency())
            {
                e.Cancel = true;
                box_Acc_IDComboBox.SelectedIndex = -1;
            }
        }

        private async Task box_Acc_IDComboBox_SelectedIndexChangedAsync(object sender, EventArgs e)
        {
            if (box_Acc_IDComboBox.SelectedValue == null || box_Acc_IDComboBox.SelectedValue is DataRowView)
            {
                cur_IDComboBox.DataSource = null;
                return;
            }

            if (!ValidateHeaderCurrency()) return;

            string accId = box_Acc_IDComboBox.SelectedValue.ToString();
            await LoadHeaderAccountCurrenciesAsync(accId);
        }

        private async Task LoadHeaderAccountCurrenciesAsync(string accId)
        {
            try
            {
                string curQuery = @"SELECT C.Cur_ID, C.Cur_Name, AAC.Is_Default 
                                    FROM Currencies C 
                                    INNER JOIN Account_Allowed_Currencies AAC ON C.Cur_ID = AAC.Cur_ID 
                                    WHERE AAC.Acc_ID = @AccID";

                DataTable dtAllowedCur = await DatabaseHelper.GetTableAsync(curQuery, new[] { new SqlParameter("@AccID", accId) });

                if (dtAllowedCur != null && dtAllowedCur.Rows.Count > 0)
                {
                    cur_IDComboBox.SelectedIndexChanged -= cur_IDComboBox_SelectedIndexChanged;

                    cur_IDComboBox.DataSource = dtAllowedCur;
                    cur_IDComboBox.DisplayMember = "Cur_Name";
                    cur_IDComboBox.ValueMember = "Cur_ID";

                    int defaultCurId = 0;
                    DataRow[] defaultRows = dtAllowedCur.Select("Is_Default = 1 OR Is_Default = 'True'");

                    if (defaultRows.Length > 0)
                        defaultCurId = Convert.ToInt32(defaultRows[0]["Cur_ID"]);
                    else
                        defaultCurId = Convert.ToInt32(dtAllowedCur.Rows[0]["Cur_ID"]);

                    cur_IDComboBox.SelectedValue = defaultCurId;

                    cur_IDComboBox.SelectedIndexChanged += cur_IDComboBox_SelectedIndexChanged;
                    TriggerCurrencyChangeUpdate();
                }
                else
                {
                    MessageBox.Show("لا توجد عملات مرتبطة بهذا الصندوق/البنك في الدليل المحاسبي!", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cur_IDComboBox.DataSource = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ أثناء جلب عملات الصندوق/البنك: " + ex.Message, "نظام الرواد", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cur_IDComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            TriggerCurrencyChangeUpdate();
        }

        private void TriggerCurrencyChangeUpdate()
        {
            if (cur_IDComboBox.SelectedValue == null || cur_IDComboBox.SelectedValue is DataRowView) return;

            if (!ValidateHeaderCurrency())
            {
                cur_IDComboBox.SelectedIndex = -1;
                exchange_RateTextBox.Text = "0.0000";
                return;
            }

            try
            {
                int curId = Convert.ToInt32(cur_IDComboBox.SelectedValue);
                decimal rate = CurrencyHelper.GetExchangeRate(curId);

                exchange_RateTextBox.Text = rate.ToString("F4");

                if (rate == 1.0000m)
                {
                    if (amount_ForeignTextBox != null) { amount_ForeignTextBox.Enabled = false; amount_ForeignTextBox.Text = "0.00"; }
                    if (CurrentMode != FormMode.View) amountTextBox.ReadOnly = false;
                }
                else
                {
                    if (amount_ForeignTextBox != null) amount_ForeignTextBox.Enabled = true;
                    amountTextBox.ReadOnly = true;
                }

                decimal foreignAmount = 0;
                decimal.TryParse(amount_ForeignTextBox.Text, out foreignAmount);
                if (foreignAmount > 0)
                {
                    isHandlingCurrencyChange = true;
                    amountTextBox.Text = (foreignAmount * rate).ToString("F2");
                    isHandlingCurrencyChange = false;
                    CalculateGridTotals();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ في تحديث سعر الصرف: " + ex.Message);
            }
        }

        private void amount_ForeignTextBox_TextChanged(object sender, EventArgs e)
        {
            if (isHandlingCurrencyChange || CurrentMode == FormMode.View) return;

            decimal foreignAmount = 0;
            decimal rate = 0;

            decimal.TryParse(amount_ForeignTextBox.Text, out foreignAmount);
            decimal.TryParse(exchange_RateTextBox.Text, out rate);

            isHandlingCurrencyChange = true;
            amountTextBox.Text = (foreignAmount * rate).ToString("F2");
            isHandlingCurrencyChange = false;
            CalculateGridTotals();
        }
        #endregion

        #region محرك الحساب اللحظي للإجماليات والتوازن
        private void CalculateGridTotals()
        {
            decimal totalCredit = 0;
            try
            {
                var creditCol = grid_Details.Columns["Amount_Credit"];
                if (creditCol == null) return;

                foreach (DataGridViewRow row in grid_Details.Rows)
                {
                    if (row.IsNewRow) continue;
                    totalCredit += Convert.ToDecimal(row.Cells[creditCol.Index].Value ?? 0);
                }

                if (total_grid != null)
                {
                    total_grid.Text = totalCredit.ToString("N2");
                }

                decimal headerAmount = 0;
                decimal.TryParse(amountTextBox.Text, out headerAmount);

                decimal difference = headerAmount - totalCredit;

                if (txt_Difference != null)
                {
                    txt_Difference.Text = difference.ToString("N2");
                    txt_Difference.ForeColor = difference == 0 ? System.Drawing.Color.Green : System.Drawing.Color.Red;
                }
            }
            catch { }
        }

        private void amountTextBox_TextChanged(object sender, EventArgs e)
        {
            CalculateGridTotals();
        }

        private void grid_Details_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            CalculateGridTotals();
        }
        #endregion

        #region معالجة خلايا الجريد والبحث المنسدل
        private async Task<bool> ApplyAccountToRowAsync(int rowIndex, string accId)
        {
            try
            {
                string sql = "SELECT Acc_ID, Acc_Name FROM Accounts WHERE Acc_ID = @AccID";
                DataTable dtAcc = await DatabaseHelper.GetTableAsync(sql, new[] { new SqlParameter("@AccID", accId) });

                if (dtAcc.Rows.Count > 0)
                {
                    grid_Details.Rows[rowIndex].Cells["Acc_ID"].Value = dtAcc.Rows[0]["Acc_ID"];
                    grid_Details.Rows[rowIndex].Cells["Acc_Name"].Value = dtAcc.Rows[0]["Acc_Name"];

                    string curQuery = @"SELECT C.Cur_ID, C.Cur_Name, AAC.Is_Default, AAC.Is_Active AS Account_Cur_Active
                                        FROM Currencies C 
                                        INNER JOIN Account_Allowed_Currencies AAC ON C.Cur_ID = AAC.Cur_ID 
                                        WHERE AAC.Acc_ID = @AccID AND C.Is_Active = 1";

                    DataTable dtAllowedCur = await DatabaseHelper.GetTableAsync(curQuery, new[] { new SqlParameter("@AccID", accId) });

                    if (dtAllowedCur != null && dtAllowedCur.Rows.Count > 0)
                    {
                        DataGridViewComboBoxCell comboCell = (DataGridViewComboBoxCell)grid_Details.Rows[rowIndex].Cells["Cur_ID"];
                        comboCell.DataSource = dtAllowedCur;
                        comboCell.DisplayMember = "Cur_Name";
                        comboCell.ValueMember = "Cur_ID";

                        int defaultCurId = 0;
                        DataRow[] defaultRows = dtAllowedCur.Select("Is_Default = 1 OR Is_Default = 'True'");

                        if (defaultRows.Length > 0)
                            defaultCurId = Convert.ToInt32(defaultRows[0]["Cur_ID"]);
                        else
                            defaultCurId = Convert.ToInt32(dtAllowedCur.Rows[0]["Cur_ID"]);

                        comboCell.Value = defaultCurId;
                        grid_Details.Rows[rowIndex].Cells["Exchange_Rate"].Value = CurrencyHelper.GetExchangeRate(defaultCurId).ToString("F4");

                        تحديث_حالة_حقل_العملة_الأجنبية(grid_Details.Rows[rowIndex]);
                        CalculateGridTotals();

                        return true;
                    }
                    else
                    {
                        MessageBox.Show("لا توجد عملات مرتبطة بهذا الحساب!", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ أثناء جلب بيانات الحساب: " + ex.Message, "نظام الرواد", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private async void OpenAccountSearch(int rowIndex, bool isCustomersOnly = false)
        {
            try
            {
                string query = isCustomersOnly
                    ? "SELECT Acc_ID, Acc_Name FROM Accounts WHERE Acc_Type = 2"
                    : "SELECT Acc_ID, Acc_Name FROM Accounts WHERE Acc_Type = 1";

                string formTitle = isCustomersOnly ? "بحث عن العملاء (F7)" : "بحث عن كافة الحسابات (F9)";

                using (var search = new AlRowad_ERP.HelpForms.UniversalSearchForm(formTitle, query))
                {
                    if (search.ShowDialog() == DialogResult.OK)
                    {
                        string selectedAcc = search.المعرف_المختار;
                        if (!string.IsNullOrWhiteSpace(selectedAcc))
                        {
                            await ApplyAccountToRowAsync(rowIndex, selectedAcc);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ في شاشة البحث: " + ex.Message, "نظام الرواد", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void grid_Details_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (CurrentMode == FormMode.View) return;

            if (e.RowIndex >= 0 && grid_Details.Columns[e.ColumnIndex].Name == "Acc_ID")
            {
                var cellValue = grid_Details.Rows[e.RowIndex].Cells["Acc_ID"].Value;

                if (cellValue != null && !string.IsNullOrWhiteSpace(cellValue.ToString()))
                {
                    string enteredAccId = cellValue.ToString().Trim();
                    bool success = await ApplyAccountToRowAsync(e.RowIndex, enteredAccId);

                    if (!success)
                    {
                        MessageBox.Show("رقم الحساب المدخل غير صحيح أو غير مسجل في الدليل المحاسبي.", "إدخال خاطئ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        grid_Details.Rows[e.RowIndex].Cells["Acc_ID"].Value = DBNull.Value;
                        grid_Details.Rows[e.RowIndex].Cells["Acc_Name"].Value = DBNull.Value;
                    }
                }
            }
        }

        private void تحديث_حالة_حقل_العملة_الأجنبية(DataGridViewRow row)
        {
            var foreignCol = grid_Details.Columns["Amount_Foreign"];
            var rateCol = grid_Details.Columns["Exchange_Rate"];
            if (foreignCol == null || rateCol == null) return;

            decimal rate = Convert.ToDecimal(row.Cells[rateCol.Index].Value ?? 1);

            if (rate == 1.0000m)
            {
                row.Cells[foreignCol.Index].ReadOnly = true;
                row.Cells[foreignCol.Index].Style.BackColor = System.Drawing.Color.LightGray;
                row.Cells[foreignCol.Index].Value = "0.00";

                row.Cells[rateCol.Index].ReadOnly = true;
                row.Cells[rateCol.Index].Style.BackColor = System.Drawing.Color.LightGray;
            }
            else
            {
                row.Cells[foreignCol.Index].ReadOnly = false;
                row.Cells[foreignCol.Index].Style.BackColor = System.Drawing.Color.White;

                row.Cells[rateCol.Index].ReadOnly = false;
                row.Cells[rateCol.Index].Style.BackColor = System.Drawing.Color.White;
            }
        }

        private void grid_Details_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || CurrentMode == FormMode.View || isHandlingCurrencyChange)
                return;

            var row = grid_Details.Rows[e.RowIndex];

            if (grid_Details.Columns["Amount_Credit"] == null ||
                grid_Details.Columns["Amount_Foreign"] == null ||
                grid_Details.Columns["Exchange_Rate"] == null ||
                grid_Details.Columns["Cur_ID"] == null ||
                grid_Details.Columns["Acc_Name"] == null)
                return;

            int colIdx = e.ColumnIndex;
            var creditCol = grid_Details.Columns["Amount_Credit"];
            var foreignCol = grid_Details.Columns["Amount_Foreign"];
            var rateCol = grid_Details.Columns["Exchange_Rate"];
            var curCol = grid_Details.Columns["Cur_ID"];

            try
            {
                decimal.TryParse(row.Cells[rateCol.Index].Value?.ToString(), out decimal rate);
                rate = rate <= 0 ? 1 : rate;

                if (colIdx == foreignCol.Index)
                {
                    decimal.TryParse(row.Cells[foreignCol.Index].Value?.ToString(), out decimal fAmount);

                    isHandlingCurrencyChange = true;
                    row.Cells[creditCol.Index].Value = (fAmount * rate).ToString("F2");
                    CalculateGridTotals();
                }
                else if (colIdx == creditCol.Index)
                {
                    decimal.TryParse(row.Cells[creditCol.Index].Value?.ToString(), out decimal localAmount);

                    isHandlingCurrencyChange = true;
                    if (rate != 1.0000m)
                    {
                        row.Cells[foreignCol.Index].Value = (localAmount / rate).ToString("F2");
                    }
                    CalculateGridTotals();
                }
                else if (colIdx == curCol.Index)
                {
                    var comboCell = row.Cells[curCol.Index] as DataGridViewComboBoxCell;
                    if (comboCell?.Value == null || row.Cells["Acc_ID"].Value == null) return;

                    string accId = row.Cells["Acc_ID"].Value.ToString();
                    int selectedCurId = Convert.ToInt32(comboCell.Value);
                    string accName = row.Cells["Acc_Name"].Value?.ToString() ?? "غير محدد";

                    var status = CurrencyHelper.CheckCurrencyTransactionStatus(accId, selectedCurId);

                    if (!status.IsGloballyActive || status.IsAccountFrozen || status.IsParentFrozen)
                    {
                        string msg = "";
                        if (!status.IsGloballyActive)
                            msg = "العملة المختارة موقوفة من قبل إدارة النظام سيادياً ولا يمكن استخدامها.";
                        else if (status.IsParentFrozen)
                            msg = $"عذراً! العملة مجمدة من الحساب الأب للحساب [{accName}]، ولا يمكن التعامل بها.";
                        else if (status.IsAccountFrozen)
                            msg = $"عذراً! هذه العملة مجمدة للحساب [{accName}] حالياً.";

                        MessageBox.Show(msg, "تنبيه النظام المالي", MessageBoxButtons.OK, MessageBoxIcon.Stop);

                        isHandlingCurrencyChange = true;
                        grid_Details.CancelEdit();
                        comboCell.Value = DBNull.Value;
                        row.Cells[rateCol.Index].Value = "0.0000";
                        row.Cells[creditCol.Index].Value = "0.00";
                        row.Cells[foreignCol.Index].Value = "0.00";

                        تحديث_حالة_حقل_العملة_الأجنبية(row);
                    }
                    else
                    {
                        decimal newRate = CurrencyHelper.GetExchangeRate(selectedCurId);

                        isHandlingCurrencyChange = true;
                        row.Cells[rateCol.Index].Value = newRate.ToString("F4");

                        تحديث_حالة_حقل_العملة_الأجنبية(row);

                        decimal.TryParse(row.Cells[creditCol.Index].Value?.ToString(), out decimal currentLocal);

                        if (newRate != 1.0000m && newRate > 0)
                            row.Cells[foreignCol.Index].Value = (currentLocal / newRate).ToString("F2");
                        else
                            row.Cells[foreignCol.Index].Value = "0.00";
                    }
                    CalculateGridTotals();
                }
                else if (colIdx == rateCol.Index)
                {
                    int selectedCurId = Convert.ToInt32(row.Cells[curCol.Index].Value ?? 0);
                    decimal.TryParse(row.Cells[rateCol.Index].Value?.ToString(), out decimal enteredRate);

                    if (!CurrencyHelper.IsExchangeRateValid(selectedCurId, enteredRate, out string errorMsg))
                    {
                        MessageBox.Show(errorMsg, "رقابة سعر الصرف", MessageBoxButtons.OK, MessageBoxIcon.Stop);

                        isHandlingCurrencyChange = true;
                        row.Cells[rateCol.Index].Value = CurrencyHelper.GetExchangeRate(selectedCurId).ToString("F4");
                        return;
                    }

                    تحديث_حالة_حقل_العملة_الأجنبية(row);

                    decimal.TryParse(row.Cells[foreignCol.Index].Value?.ToString(), out decimal fAmount);

                    isHandlingCurrencyChange = true;
                    if (enteredRate != 1.0000m && fAmount > 0)
                    {
                        row.Cells[creditCol.Index].Value = (fAmount * enteredRate).ToString("F2");
                    }
                    CalculateGridTotals();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ أثناء معالجة العمليات الحسابية: " + ex.Message, "نظام الرواد", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                isHandlingCurrencyChange = false;
            }
        }

        private void grid_Details_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (grid_Details.Columns[e.ColumnIndex].Name == "Cur_ID")
            {
                if (e.FormattedValue == null || string.IsNullOrEmpty(e.FormattedValue.ToString())) return;
            }
        }

        private void grid_Details_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (CurrentMode == FormMode.View) return;
            if (e.RowIndex >= 0 && (grid_Details.Columns[e.ColumnIndex].Name == "Acc_ID" || grid_Details.Columns[e.ColumnIndex].Name == "Acc_Name"))
            {
                OpenAccountSearch(e.RowIndex, false);
            }
        }

        private void grid_Details_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (grid_Details.IsCurrentCellDirty && grid_Details.CurrentCell.ColumnIndex == grid_Details.Columns["Cur_ID"].Index)
            {
                grid_Details.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void DataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
            e.Cancel = true;
        }

        private void grid_Details_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (grid_Details.CurrentCell == null) return;

            string colName = grid_Details.Columns[grid_Details.CurrentCell.ColumnIndex].Name;

            if (colName == "Amount_Credit" || colName == "Amount_Foreign" || colName == "Exchange_Rate")
            {
                TextBox txt = e.Control as TextBox;
                if (txt != null)
                {
                    txt.KeyPress -= NumericGridCell_KeyPress;
                    txt.KeyPress += NumericGridCell_KeyPress;
                }
            }
        }

        private void NumericGridCell_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }

            TextBox txt = sender as TextBox;
            if (txt != null && e.KeyChar == '.' && txt.Text.IndexOf('.') > -1)
            {
                e.Handled = true;
            }
        }
        #endregion

        #region الاختصارات السيادية الموروثة
        internal override void OnF9Pressed()
        {
            if (grid_Details.CurrentCell != null)
            {
                string colName = grid_Details.Columns[grid_Details.CurrentCell.ColumnIndex].Name;
                if (colName == "Acc_ID" || colName == "Acc_Name")
                {
                    OpenAccountSearch(grid_Details.CurrentCell.RowIndex, false);
                }
            }
        }

        internal override void OnF7Pressed()
        {
            if (grid_Details.CurrentCell != null)
            {
                string colName = grid_Details.Columns[grid_Details.CurrentCell.ColumnIndex].Name;
                if (colName == "Acc_ID" || colName == "Acc_Name")
                {
                    OpenAccountSearch(grid_Details.CurrentCell.RowIndex, true);
                }
            }
        }

        internal override void OnF3Pressed()
        {
            if (grid_Details.CurrentCell == null) return;

            int currentRow = grid_Details.CurrentCell.RowIndex;
            var notesCol = grid_Details.Columns["Notes"];

            if (notesCol != null)
            {
                if (currentRow > 0)
                {
                    var prevNote = grid_Details.Rows[currentRow - 1].Cells[notesCol.Index].Value;
                    grid_Details.Rows[currentRow].Cells[notesCol.Index].Value = prevNote;
                }
                else if (currentRow == 0 && notesTextBox != null)
                {
                    grid_Details.Rows[currentRow].Cells[notesCol.Index].Value = notesTextBox.Text;
                }
            }
        }

        internal override void OnF2Pressed()
        {
            if (grid_Details.CurrentCell == null) return;

            int currentRow = grid_Details.CurrentCell.RowIndex;

            if (currentRow > 0)
            {
                DataGridViewRow prevRow = grid_Details.Rows[currentRow - 1];
                DataGridViewRow currRow = grid_Details.Rows[currentRow];

                currRow.Cells["Acc_ID"].Value = prevRow.Cells["Acc_ID"].Value;
                currRow.Cells["Acc_Name"].Value = prevRow.Cells["Acc_Name"].Value;
                currRow.Cells["Cur_ID"].Value = prevRow.Cells["Cur_ID"].Value;
                currRow.Cells["Exchange_Rate"].Value = prevRow.Cells["Exchange_Rate"].Value;

                var notesCol = grid_Details.Columns["Notes"];
                if (notesCol != null) currRow.Cells[notesCol.Index].Value = prevRow.Cells[notesCol.Index].Value;

                if (grid_Details.Columns["Amount_Credit"] != null) currRow.Cells["Amount_Credit"].Value = "0.00";
                if (grid_Details.Columns["Amount_Foreign"] != null) currRow.Cells["Amount_Foreign"].Value = "0.00";

                تحديث_حالة_حقل_العملة_الأجنبية(currRow);
            }
        }
        #endregion

        #region محركات العمليات السيادية والاتصال اللحظي الموحد (CRUD) - 🌟 Async

        public override async void OnNew()
        {
            currentVoucherId = 0;
            DetachHeaderEvents();
            ClearFormFields(this);
            voucher_NoTextBox.Text = await VoucherRepository.GetNewVoucherNoAsync(1);

            rbtnCash.Checked = true;
            await LoadBoxAccountsAsync(false);

            if (box_Acc_IDComboBox != null) box_Acc_IDComboBox.SelectedIndex = -1;
            if (cur_IDComboBox != null) cur_IDComboBox.SelectedIndex = -1;
            if (txt_Difference != null) { txt_Difference.Text = "0.00"; txt_Difference.ForeColor = System.Drawing.Color.Green; }

            exchange_RateTextBox.Text = "1.0000";
            amountTextBox.Text = "0.00";
            if (amount_ForeignTextBox != null) { amount_ForeignTextBox.Text = "0.00"; amount_ForeignTextBox.Enabled = false; }

            notesTextBox.Clear();
            voucher_DateDateTimePicker.DateValue = DateTime.Now;
            grid_Details.Rows.Clear();
            if (total_grid != null) total_grid.Text = "0.00";

            AttachHeaderEvents();
            ChangeFormMode(FormMode.New);
        }

        public override void OnEdit()
        {
            if (currentVoucherId <= 0 || CurrentMode == FormMode.New)
            {
                MessageBox.Show("يرجى استعراض سند محفوظ مسبقاً للتمكن من تعديله.", "منع التعديل", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (this.CurrentDocumentStatus == DocumentStatus.Posted)
            {
                MessageBox.Show("لا يمكن تعديل سند تم ترحيله محاسبياً. يرجى فك الترحيل أولاً.", "تنبيه النظام", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            ChangeFormMode(FormMode.Edit);
        }

        private DataTable PrepareJournalDataTable(CashVoucherHeader voucher)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Acc_ID", typeof(string));
            dt.Columns.Add("Debit", typeof(decimal));
            dt.Columns.Add("Credit", typeof(decimal));
            dt.Columns.Add("Cur_ID", typeof(int));
            dt.Columns.Add("Exchange_Rate", typeof(decimal));
            dt.Columns.Add("Foreign_Debit", typeof(decimal));
            dt.Columns.Add("Foreign_Credit", typeof(decimal));

            dt.Rows.Add(voucher.BoxAccID, voucher.Amount, 0m, voucher.CurID, voucher.ExchangeRate, voucher.AmountForeign, 0m);

            foreach (var det in voucher.Details)
            {
                dt.Rows.Add(det.AccID, 0m, det.AmountCredit, det.CurrencyID, det.ExchangeRate, 0m, det.AmountForeign);
            }
            return dt;
        }

        // 🌟 الحفظ اللامتزامن مع الـ Transactions
        protected override async Task<bool> ExecuteSaveToDatabaseAsync(SqlTransaction trans)
        {
            if (!ValidateFinancialPeriodBeforeSave(voucher_DateDateTimePicker.DateValue))
            {
                return false;
            }

            if (box_Acc_IDComboBox.SelectedValue == null || cur_IDComboBox.SelectedValue == null)
            {
                MessageBox.Show("لا يمكن الحفظ! يرجى التأكد من اختيار حساب الصندوق/البنك وعملة السند.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            decimal headerAmount = 0;
            decimal.TryParse(amountTextBox.Text, out headerAmount);
            decimal gridTotalCredit = 0;

            foreach (DataGridViewRow row in grid_Details.Rows)
            {
                if (row.IsNewRow || row.Cells["Acc_ID"].Value == null) continue;
                gridTotalCredit += Convert.ToDecimal(row.Cells["Amount_Credit"].Value ?? 0);
            }

            if (headerAmount != gridTotalCredit)
            {
                MessageBox.Show($"خطأ محاسبي: السند غير متزن ماليًا!\nالفارق: {(headerAmount - gridTotalCredit):N2}", "منع الترحيل المحاسبي", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }

            foreach (DataGridViewRow row in grid_Details.Rows)
            {
                if (row.IsNewRow || row.Cells["Acc_ID"].Value == null || string.IsNullOrWhiteSpace(row.Cells["Acc_ID"].Value.ToString()))
                    continue;

                decimal amountCredit = Convert.ToDecimal(row.Cells["Amount_Credit"].Value ?? 0);
                if (amountCredit <= 0)
                {
                    MessageBox.Show($"الحساب [{row.Cells["Acc_Name"].Value}] مسجل بقيمة صفرية!\nيجب إدخال مبلغ صحيح أو حذف السطر.", "منع الحفظ", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return false;
                }
            }

            try
            {
                CashVoucherHeader voucher = new CashVoucherHeader
                {
                    VoucherID = currentVoucherId,
                    VoucherNo = voucher_NoTextBox.Text,
                    VoucherDate = voucher_DateDateTimePicker.DateValue,
                    DocTypeID = 1,
                    BoxAccID = box_Acc_IDComboBox.SelectedValue.ToString(),
                    Amount = headerAmount,
                    AmountForeign = string.IsNullOrEmpty(amount_ForeignTextBox.Text) ? 0 : Convert.ToDecimal(amount_ForeignTextBox.Text),
                    CurID = Convert.ToInt32(cur_IDComboBox.SelectedValue),
                    ExchangeRate = Convert.ToDecimal(exchange_RateTextBox.Text),
                    Notes = notesTextBox.Text ?? "",
                    IsPosted = (this.CurrentDocumentStatus == DocumentStatus.Posted) ? 1 : 0,

                    CreatedBy = (CurrentMode == FormMode.New) ? UserSession.UserId : 0,
                    UpdatedBy = (CurrentMode == FormMode.Edit) ? UserSession.UserId : 0,

                    RowVersion = _currentRowVersion
                };

                foreach (DataGridViewRow row in grid_Details.Rows)
                {
                    if (row.IsNewRow || row.Cells["Acc_ID"].Value == null || string.IsNullOrWhiteSpace(row.Cells["Acc_ID"].Value.ToString())) continue;

                    decimal lineForeign = 0;
                    var foreignCol = grid_Details.Columns["Amount_Foreign"];
                    if (foreignCol != null && row.Cells[foreignCol.Index].Value != null)
                        decimal.TryParse(row.Cells[foreignCol.Index].Value.ToString(), out lineForeign);

                    voucher.Details.Add(new CashVoucherDetail
                    {
                        AccID = row.Cells["Acc_ID"].Value.ToString(),
                        AccName = row.Cells["Acc_Name"].Value?.ToString() ?? "",
                        AmountCredit = Convert.ToDecimal(row.Cells["Amount_Credit"].Value ?? 0),
                        AmountDebit = 0,
                        CurrencyID = Convert.ToInt32(row.Cells["Cur_ID"].Value),
                        ExchangeRate = Convert.ToDecimal(row.Cells["Exchange_Rate"].Value ?? 1),
                        AmountForeign = lineForeign,
                        Notes = row.Cells["Notes"].Value?.ToString() ?? ""
                    });
                }

                // 🌟 استخدام الدوال اللامتزامنة
                currentVoucherId = await VoucherRepository.SaveVoucherAsync(voucher, CurrentMode, trans);

                DataTable dtJournal = PrepareJournalDataTable(voucher);
                var (isSuccess, entryError) = await JournalEngine.PostEntryAsync(voucher.VoucherDate, string.IsNullOrWhiteSpace(voucher.Notes) ? "سند قبض" : voucher.Notes, voucher.VoucherNo, voucher.DocTypeID, dtJournal, trans);
                if (!isSuccess)
                    throw new Exception(entryError);

                if (CurrentMode == FormMode.Edit)
                {
                    string oldValues = "تم الحفظ المسبق";
                    string newValues = $"تعديل سند قبض رقم: {voucher.VoucherNo} | المبلغ: {voucher.Amount} | الصندوق/البنك: {box_Acc_IDComboBox.Text}";
                    DatabaseHelper.LogAuditTransaction(trans, "Cash_Vouchers", voucher.VoucherID.ToString(), "UPDATE", oldValues, newValues, "تعديل سند قبض");
                }

                return true;
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "تضارب مالي في التزامن", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception($"خطأ أثناء حفظ السند المالي: {ex.Message}");
            }
        }

        // 🌟 الحذف اللامتزامن
        protected override async Task<bool> ExecuteDeleteFromDatabaseAsync(SqlTransaction trans)
        {
            if (currentVoucherId <= 0 || CurrentMode == FormMode.New)
            {
                MessageBox.Show("لا يوجد سند محفوظ للحذف.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (this.CurrentDocumentStatus == DocumentStatus.Posted)
            {
                MessageBox.Show("لا يمكن حذف سند مرحل. يرجى فك الترحيل أولاً.", "منع الحذف", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }

            int docTypeId = 1;
            await VoucherRepository.DeleteVoucherAsync(currentVoucherId, voucher_NoTextBox.Text, docTypeId, trans);
            return true;
        }

        public override async void OnSearch()
        {
            try
            {
                using (var search = new AlRowad_ERP.HelpForms.UniversalSearchForm("بحث عن سندات القبض", VoucherRepository.GetVouchersSearchQuery(1)))
                {
                    if (search.ShowDialog() == DialogResult.OK)
                    {
                        string selectedValue = search.المعرف_المختار;
                        if (!string.IsNullOrWhiteSpace(selectedValue))
                        {
                            await LoadVoucherDataAsync(Convert.ToInt32(selectedValue));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ أثناء البحث: " + ex.Message, "نظام الرواد", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 🌟 الجلب اللامتزامن
        private async Task LoadVoucherDataAsync(int voucherId)
        {
            CashVoucherHeader voucher = await VoucherRepository.GetVoucherAsync(voucherId);
            if (voucher == null) return;

            _currentRowVersion = voucher.RowVersion;
            currentVoucherId = voucher.VoucherID;

            bool isPeriodLocked = IsPeriodClosed(voucher.VoucherDate);

            if (voucher.IsPosted == 1 || isPeriodLocked)
            {
                this.CurrentDocumentStatus = DocumentStatus.Posted;
            }
            else
            {
                this.CurrentDocumentStatus = DocumentStatus.Pending;
            }

            DetachHeaderEvents();

            voucher_NoTextBox.Text = voucher.VoucherNo;
            voucher_DateDateTimePicker.DateValue = voucher.VoucherDate;

            isUpdatingRadioBoxes = true;
            if (voucher.BoxAccID.StartsWith("110102"))
            {
                rbtnBank.Checked = true;
                rbtnCash.Checked = false;
                await LoadBoxAccountsAsync(true);
            }
            else
            {
                rbtnCash.Checked = true;
                rbtnBank.Checked = false;
                await LoadBoxAccountsAsync(false);
            }
            isUpdatingRadioBoxes = false;

            box_Acc_IDComboBox.SelectedValue = voucher.BoxAccID;
            await LoadHeaderAccountCurrenciesAsync(voucher.BoxAccID);
            cur_IDComboBox.SelectedValue = voucher.CurID;

            exchange_RateTextBox.Text = voucher.ExchangeRate.ToString("F4");
            amountTextBox.Text = voucher.Amount.ToString("F2");
            if (amount_ForeignTextBox != null) amount_ForeignTextBox.Text = voucher.AmountForeign.ToString("F2");
            notesTextBox.Text = voucher.Notes;

            grid_Details.Rows.Clear();

            foreach (var det in voucher.Details)
            {
                int rowIndex = grid_Details.Rows.Add();
                var rowG = grid_Details.Rows[rowIndex];

                rowG.Cells["Acc_ID"].Value = det.AccID;
                rowG.Cells["Acc_Name"].Value = det.AccName;
                rowG.Cells["Amount_Credit"].Value = det.AmountCredit.ToString("F2");

                DataTable dtAllowedCur = await Task.Run(() => CurrencyHelper.GetAllowedCurrenciesForAccount(det.AccID));
                DataGridViewComboBoxCell comboCell = (DataGridViewComboBoxCell)rowG.Cells["Cur_ID"];
                comboCell.DataSource = dtAllowedCur;
                comboCell.DisplayMember = "Cur_Name";
                comboCell.ValueMember = "Cur_ID";
                comboCell.Value = det.CurrencyID;

                rowG.Cells["Exchange_Rate"].Value = det.ExchangeRate.ToString("F4");
                rowG.Cells["Notes"].Value = det.Notes;

                var foreignCol = grid_Details.Columns["Amount_Foreign"];
                if (foreignCol != null) rowG.Cells[foreignCol.Index].Value = det.AmountForeign.ToString("F2");

                تحديث_حالة_حقل_العملة_الأجنبية(rowG);
            }

            if (this.Controls.ContainsKey("txt_CreatedBy"))
                txt_CreatedBy.Text = FormatUserInfo(voucher.CreatedBy, voucher.CreatedByName);
            if (this.Controls.ContainsKey("txt_CreatedAt"))
                txt_CreatedAt.Text = voucher.CreatedAt.HasValue ? voucher.CreatedAt.Value.ToString("yyyy/MM/dd  hh:mm tt") : "";

            if (this.Controls.ContainsKey("txt_UpdatedBy"))
                txt_UpdatedBy.Text = FormatUserInfo(voucher.UpdatedBy, voucher.UpdatedByName);
            if (this.Controls.ContainsKey("txt_UpdatedAt"))
                txt_UpdatedAt.Text = voucher.UpdatedAt.HasValue ? voucher.UpdatedAt.Value.ToString("yyyy/MM/dd  hh:mm tt") : "";

            CalculateGridTotals();
            AttachHeaderEvents();

            ChangeFormMode(FormMode.RecordSelected);

            if (isPeriodLocked && !UserSession.IsSuperAdmin)
            {
                MessageBox.Show("تنبيه أمني: هذا السند يقع ضمن فترة محاسبية مقفلة، لذلك تم عرضه في وضع القراءة فقط لحماية الدفاتر.",
                                "فترة مقفلة", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public override async void OnCancel()
        {
            if (MessageBox.Show("هل أنت متأكد من التراجع وإلغاء التعديلات الحالية؟", "تنبيه التراجع", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (currentVoucherId > 0)
                {
                    await LoadVoucherDataAsync(currentVoucherId);
                }
                else
                {
                    ClearFormFields(this);
                    grid_Details.Rows.Clear();
                    if (txt_Difference != null) { txt_Difference.Text = "0.00"; txt_Difference.ForeColor = System.Drawing.Color.Green; }
                    if (total_grid != null) total_grid.Text = "0.00";

                    ChangeFormMode(FormMode.View);
                }
            }
        }
        #endregion

        private string FormatUserInfo(object userIdObj, object userNameObj)
        {
            if (userIdObj == null || userIdObj == DBNull.Value || Convert.ToInt32(userIdObj) == 0) return "";
            string name = (userNameObj != null && userNameObj != DBNull.Value) ? userNameObj.ToString() : "مجهول";
            return $"{name} || {userIdObj}";
        }

        protected override async Task<bool> ValidateDependenciesBeforeDeleteAsync()
        {
            string voucherNo = voucher_NoTextBox.Text;

            if (await IsRecordUsedInTableAsync("Invoice_Allocations", "Voucher_No", voucherNo))
            {
                MessageBox.Show("منع أمني ومحاسبي: لا يمكن حذف هذا السند لأنه تم استخدامه في تسوية أو تسديد فواتير سابقة. يرجى إلغاء التسوية أولاً.", "ارتباط مالي", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }

            return true;
        }
    }
}