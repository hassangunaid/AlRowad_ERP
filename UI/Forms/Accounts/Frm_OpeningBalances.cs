using AlRowad_ERP.Core;
using AlRowad_ERP.Core.Constants;
using AlRowad_ERP.UI.Base;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AlRowad_ERP.UI.Forms.Accounts
{
    public partial class Frm_OpeningBalances : BaseEntryForm
    {
        private bool _isInternalCalculation = false;

        public Frm_OpeningBalances()
        {
            InitializeComponent();
            PrimaryIdFieldName = "txtDocumentNo";
            MainTableName = "Journal_Header";
        }

        protected override async void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            await LoadCurrenciesIntoGridAsync();
            OnNew(); // فتح الشاشة في وضع الإدخال جاهزة للعمل
        }

        private async Task LoadCurrenciesIntoGridAsync()
        {
            try
            {
                string query = $"SELECT Currency_Id, Currency_Name FROM Currencies WHERE {SystemConstants.AuditFields.IsDeleted} = 0";
                DataTable dtCurrencies = await DatabaseHelper.GetTableAsync(query);

                if (dtCurrencies != null && dtCurrencies.Rows.Count > 0)
                {
                    Col_Currency.DataSource = dtCurrencies;
                    Col_Currency.DisplayMember = "Currency_Name";
                    Col_Currency.ValueMember = "Currency_Id";
                }
            }
            catch (Exception ex)
            {
                DatabaseHelper.LogSystemError(ex.Message, ex.StackTrace, "LoadCurrencies_OpeningBalances");
            }
        }

        #region محرك الحساب الفوري والمقارنة اللحظية (Real-time Calculation)

        private void dgvOpeningDetails_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgvOpeningDetails.IsCurrentCellDirty)
            {
                dgvOpeningDetails.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void dgvOpeningDetails_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (_isInternalCalculation || e.RowIndex < 0) return;

            try
            {
                _isInternalCalculation = true;
                var row = dgvOpeningDetails.Rows[e.RowIndex];

                if (e.ColumnIndex == dgvOpeningDetails.Columns["Col_ForeignDebit"].Index ||
                    e.ColumnIndex == dgvOpeningDetails.Columns["Col_ForeignCredit"].Index ||
                    e.ColumnIndex == dgvOpeningDetails.Columns["Col_ExchangeRate"].Index)
                {
                    decimal rate = ParseDecimalSafe(row.Cells["Col_ExchangeRate"].Value, 1.0m);
                    if (rate <= 0) rate = 1.0m;

                    decimal fDebit = ParseDecimalSafe(row.Cells["Col_ForeignDebit"].Value, 0m);
                    decimal fCredit = ParseDecimalSafe(row.Cells["Col_ForeignCredit"].Value, 0m);

                    if (e.ColumnIndex == dgvOpeningDetails.Columns["Col_ForeignDebit"].Index && fDebit > 0)
                    {
                        row.Cells["Col_ForeignCredit"].Value = 0m;
                        row.Cells["Col_Debit"].Value = Math.Round(fDebit * rate, 4);
                        row.Cells["Col_Credit"].Value = 0m;
                    }
                    else if (e.ColumnIndex == dgvOpeningDetails.Columns["Col_ForeignCredit"].Index && fCredit > 0)
                    {
                        row.Cells["Col_ForeignDebit"].Value = 0m;
                        row.Cells["Col_Credit"].Value = Math.Round(fCredit * rate, 4);
                        row.Cells["Col_Debit"].Value = 0m;
                    }
                    else if (e.ColumnIndex == dgvOpeningDetails.Columns["Col_ExchangeRate"].Index)
                    {
                        if (fDebit > 0) row.Cells["Col_Debit"].Value = Math.Round(fDebit * rate, 4);
                        if (fCredit > 0) row.Cells["Col_Credit"].Value = Math.Round(fCredit * rate, 4);
                    }
                }

                CalculateAndDisplayTotals();
            }
            finally
            {
                _isInternalCalculation = false;
            }
        }

        private decimal ParseDecimalSafe(object val, decimal defaultValue)
        {
            if (val == null || val == DBNull.Value) return defaultValue;
            return decimal.TryParse(val.ToString(), out decimal result) ? result : defaultValue;
        }

        private void CalculateAndDisplayTotals()
        {
            decimal totalDebit = 0;
            decimal totalCredit = 0;

            foreach (DataGridViewRow row in dgvOpeningDetails.Rows)
            {
                if (row.IsNewRow) continue;

                totalDebit += ParseDecimalSafe(row.Cells["Col_Debit"]?.Value, 0m);
                totalCredit += ParseDecimalSafe(row.Cells["Col_Credit"]?.Value, 0m);
            }

            lblTotalDebitVal.Text = totalDebit.ToString("N2");
            lblTotalCreditVal.Text = totalCredit.ToString("N2");
        }

        #endregion

        #region تجاوزات BaseEntryForm السيادية وفق الدستور

        public override void OnNew()
        {
            base.OnNew();
            dtpDocumentDate.DateValue = DateTime.Now;
            txtDescription.Text = "قيد الافتتاح والأرصدة المرحلة للفترة المالية والعملات المتعددة";
            dgvOpeningDetails.Rows.Clear();
            CalculateAndDisplayTotals();
            ChangeFormMode(FormMode.New);
        }

        public override void OnEdit()
        {
            if (this.CurrentDocumentStatus == DocumentStatus.Posted)
            {
                MessageBox.Show("لا يمكن تعديل قيد افتتاحي معتمد ومرحل.", "حظر أمني", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            ChangeFormMode(FormMode.Edit);
        }

        public override void OnCancel()
        {
            base.OnCancel();
        }

        public override void OnDelete()
        {
            base.OnDelete();
        }

        protected override bool ValidateFields(Control parent)
        {
            if (dgvOpeningDetails == null || dgvOpeningDetails.Rows.Count <= 1)
            {
                MessageBox.Show("يجب إدخال حسابات في جدول الأرصدة الافتتاحية.", "تحقق أمني", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            decimal totalDebit = 0;
            decimal totalCredit = 0;

            foreach (DataGridViewRow row in dgvOpeningDetails.Rows)
            {
                if (row.IsNewRow) continue;

                if (string.IsNullOrWhiteSpace(row.Cells["Col_AccountId"]?.Value?.ToString()))
                {
                    MessageBox.Show("يوجد سطر بدون رقم حساب، يرجى استكماله أو حذفه.", "تحقق أمني", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                totalDebit += ParseDecimalSafe(row.Cells["Col_Debit"]?.Value, 0m);
                totalCredit += ParseDecimalSafe(row.Cells["Col_Credit"]?.Value, 0m);
            }

            if (totalDebit == 0 && totalCredit == 0)
            {
                MessageBox.Show("لا يمكن حفظ قيد افتتاحي بقيم صفرية.", "خطأ محاسبي", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (Math.Abs(totalDebit - totalCredit) > 0.001m)
            {
                MessageBox.Show($"القيد غير متوازن محلياً!\nإجمالي المدين المحلي: {totalDebit:N2}\nإجمالي الدائن المحلي: {totalCredit:N2}", "خطأ محاسبي", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }

            return true;
        }

        /// <summary>
        /// الحفظ الذري المتكامل بمعاملة ACID واحدة (الرأس + التفاصيل + الأستاذ العام System_Ledger)
        /// </summary>
        protected override async Task<bool> ExecuteSaveToDatabaseAsync(SqlTransaction transaction)
        {
            string headerQuery = $@"
                INSERT INTO Journal_Header 
                (Document_No, Document_Date, Description, Doc_Type_Id, Total_Amount, Status, {SystemConstants.AuditFields.CreatedBy}, Created_At)
                VALUES 
                (@DocNo, @DocDate, @Desc, @DocTypeId, @Amount, @Status, @UserId, GETDATE());
                SELECT SCOPE_IDENTITY();";

            long headerId = 0;
            using (var cmdHeader = new SqlCommand(headerQuery, transaction.Connection, transaction))
            {
                cmdHeader.Parameters.Add("@DocNo", SqlDbType.NVarChar).Value = txtDocumentNo.Text.Trim();
                cmdHeader.Parameters.Add("@DocDate", SqlDbType.DateTime).Value = dtpDocumentDate.DateValue;
                cmdHeader.Parameters.Add("@Desc", SqlDbType.NVarChar).Value = txtDescription.Text.Trim();
                cmdHeader.Parameters.Add("@DocTypeId", SqlDbType.Int).Value = 1; // 1 = قيد افتتاحي
                cmdHeader.Parameters.Add("@Amount", SqlDbType.Decimal).Value = CalculateTotalAmount();
                cmdHeader.Parameters.Add("@Status", SqlDbType.Int).Value = (int)DocumentStatus.Posted;
                cmdHeader.Parameters.Add("@UserId", SqlDbType.Int).Value = this.CurrentUserId;

                object result = await cmdHeader.ExecuteScalarAsync();
                headerId = Convert.ToInt64(result);
            }

            foreach (DataGridViewRow row in     dgvOpeningDetails.Rows)
            {
                if (row.IsNewRow) continue;

                int accountId = Convert.ToInt32(row.Cells["Col_AccountId"].Value);
                int currencyId = Convert.ToInt32(row.Cells["Col_Currency"]?.Value ?? 1);
                decimal exchangeRate = ParseDecimalSafe(row.Cells["Col_ExchangeRate"]?.Value, 1.0m);
                decimal foreignDebit = ParseDecimalSafe(row.Cells["Col_ForeignDebit"]?.Value, 0m);
                decimal foreignCredit = ParseDecimalSafe(row.Cells["Col_ForeignCredit"]?.Value, 0m);
                decimal debit = ParseDecimalSafe(row.Cells["Col_Debit"]?.Value, 0m);
                decimal credit = ParseDecimalSafe(row.Cells["Col_Credit"]?.Value, 0m);
                string memo = row.Cells["Col_Memo"]?.Value?.ToString() ?? string.Empty;

                string detailQuery = @"
                    INSERT INTO Journal_Details 
                    (Journal_Id, Account_Id, Currency_Id, Exchange_Rate, Foreign_Debit, Foreign_Credit, Debit, Credit, Description)
                    VALUES 
                    (@JournalId, @AccountId, @CurrencyId, @Rate, @FDebit, @FCredit, @Debit, @Credit, @Memo);";

                using (var cmdDetail = new SqlCommand(detailQuery, transaction.Connection, transaction))
                {
                    cmdDetail.Parameters.Add("@JournalId", SqlDbType.BigInt).Value = headerId;
                    cmdDetail.Parameters.Add("@AccountId", SqlDbType.Int).Value = accountId;
                    cmdDetail.Parameters.Add("@CurrencyId", SqlDbType.Int).Value = currencyId;
                    cmdDetail.Parameters.Add("@Rate", SqlDbType.Decimal).Value = exchangeRate;
                    cmdDetail.Parameters.Add("@FDebit", SqlDbType.Decimal).Value = foreignDebit;
                    cmdDetail.Parameters.Add("@FCredit", SqlDbType.Decimal).Value = foreignCredit;
                    cmdDetail.Parameters.Add("@Debit", SqlDbType.Decimal).Value = debit;
                    cmdDetail.Parameters.Add("@Credit", SqlDbType.Decimal).Value = credit;
                    cmdDetail.Parameters.Add("@Memo", SqlDbType.NVarChar).Value = memo;

                    await cmdDetail.ExecuteNonQueryAsync();
                }

                string ledgerQuery = @"
                    INSERT INTO System_Ledger 
                    (Account_Id, Document_Id, Document_Date, Debit, Credit, Description, Is_Posted)
                    VALUES 
                    (@AccountId, @DocId, @DocDate, @Debit, @Credit, @Desc, 1);";

                using (var cmdLedger = new SqlCommand(ledgerQuery, transaction.Connection, transaction))
                {
                    cmdLedger.Parameters.Add("@AccountId", SqlDbType.Int).Value = accountId;
                    cmdLedger.Parameters.Add("@DocId", SqlDbType.BigInt).Value = headerId;
                    cmdLedger.Parameters.Add("@DocDate", SqlDbType.DateTime).Value = dtpDocumentDate.DateValue;
                    cmdLedger.Parameters.Add("@Debit", SqlDbType.Decimal).Value = debit;
                    cmdLedger.Parameters.Add("@Credit", SqlDbType.Decimal).Value = credit;
                    cmdLedger.Parameters.Add("@Desc", SqlDbType.NVarChar).Value = "رصيد افتتاحي عملة متعددة: " + memo;

                    await cmdLedger.ExecuteNonQueryAsync();
                }
            }

            return true;
        }

        protected override void LockControls(Control parent, bool isReadOnly)
        {
            base.LockControls(parent, isReadOnly);

            txtDocumentNo.ReadOnly = true;
            txtDescription.ReadOnly = isReadOnly;
            dtpDocumentDate.Enabled = !isReadOnly;
            dgvOpeningDetails.ReadOnly = isReadOnly;
        }

        #endregion

        private decimal CalculateTotalAmount()
        {
            decimal total = 0;
            foreach (DataGridViewRow row in dgvOpeningDetails.Rows)
            {
                if (row.IsNewRow) continue;
                total += ParseDecimalSafe(row.Cells["Col_Debit"]?.Value, 0m);
            }
            return total;
        }
    }
}