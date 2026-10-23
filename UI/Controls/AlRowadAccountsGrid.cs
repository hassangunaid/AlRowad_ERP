using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows.Forms;
using AlRowad_ERP.Core;
using AlRowad_ERP.Core.Constants;
using AlRowad_ERP.HelpForms;

namespace AlRowad_ERP.UI.Controls
{
    /// <summary>
    /// شبكة مخصصة لشاشات الحسابات والمالية (تدعم F9 للبحث المفلتر والتحقق اللحظي من الحساب)
    /// </summary>
    public class AlRowadAccountsGrid : AlRowadDataGridView
    {
        public AlRowadAccountsGrid()
        {
            this.OnF9Pressed += AlRowadAccountsGrid_OnF9Pressed;
        }

        protected override void OnColumnAdded(DataGridViewColumnEventArgs e)
        {
            base.OnColumnAdded(e);

            string colName = e.Column.Name.ToLower();
            // أتمتة التنسيق المحاسبي لأعمدة المدين والدائن وسعر الصرف
            if (colName.Contains("debit") || colName.Contains("credit") || colName.Contains("rate"))
            {
                e.Column.DefaultCellStyle.Format = "N2";
                e.Column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
        }

        protected override void OnEditingControlShowing(DataGridViewEditingControlShowingEventArgs e)
        {
            base.OnEditingControlShowing(e);

            if (this.CurrentCell == null) return;
            string colName = this.Columns[this.CurrentCell.ColumnIndex].Name.ToLower();

            if (colName.Contains("debit") || colName.Contains("credit") || colName.Contains("rate"))
            {
                if (e.Control is TextBox txt)
                {
                    txt.KeyPress -= NumericAccountCell_KeyPress;
                    txt.KeyPress += NumericAccountCell_KeyPress;
                }
            }
            else if (e.Control is TextBox txt)
            {
                txt.KeyPress -= NumericAccountCell_KeyPress;
            }
        }

        private void NumericAccountCell_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
                e.Handled = true;

            if (sender is TextBox txt && e.KeyChar == '.' && txt.Text.IndexOf('.') > -1)
                e.Handled = true;
        }

        private void AlRowadAccountsGrid_OnF9Pressed(object sender, DataGridViewCellEventArgs e)
        {
            if (this.ReadOnly || e.RowIndex < 0 || e.ColumnIndex < 0) return;

            string colName = this.Columns[e.ColumnIndex].Name;

            if (colName == "Col_AccountId" || colName == "Col_AccountName")
            {
                string initialFilter = this.Rows[e.RowIndex].Cells[e.ColumnIndex].EditedFormattedValue?.ToString()?.Trim() ?? string.Empty;
                OpenAccountSearchDialog(this.Rows[e.RowIndex], initialFilter);
            }
        }

        private void OpenAccountSearchDialog(DataGridViewRow row, string initialFilter)
        {
            try
            {
                string query = $"SELECT Account_Id AS [رقم الحساب], Account_Name AS [اسم الحساب] FROM Accounts WHERE {SystemConstants.AuditFields.IsDeleted} = 0";

                using (var searchForm = new UniversalSearchForm("دليل الحسابات - اختيار حساب", query))
                {
                    if (!string.IsNullOrEmpty(initialFilter))
                    {
                        searchForm.Shown += (s, args) =>
                        {
                            searchForm.txt_البحث_الفوري.Text = initialFilter;
                            searchForm.txt_البحث_الفوري.SelectionStart = initialFilter.Length;
                        };
                    }

                    if (searchForm.ShowDialog(this.FindForm()) == DialogResult.OK)
                    {
                        if (!string.IsNullOrEmpty(searchForm.المعرف_المختار))
                        {
                            row.Cells["Col_AccountId"].Value = searchForm.المعرف_المختار;
                            row.Cells["Col_AccountName"].Value = searchForm.الاسم_المختار;

                            if (this.Columns.Contains("Col_Currency"))
                            {
                                this.CurrentCell = row.Cells["Col_Currency"];
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DatabaseHelper.LogSystemError(ex.Message, ex.StackTrace, "AlRowadAccountsGrid_F9_Search");
                MessageBox.Show("حدث خطأ أثناء البحث عن الحساب: " + ex.Message, "خطأ فني", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected override async void OnCellValidating(DataGridViewCellValidatingEventArgs e)
        {
            base.OnCellValidating(e);

            if (this.ReadOnly || e.RowIndex < 0 || this.Rows[e.RowIndex].IsNewRow) return;

            if (this.Columns[e.ColumnIndex].Name == "Col_AccountId")
            {
                string inputAccountId = e.FormattedValue?.ToString()?.Trim();
                bool isValid = await ValidateAccountIdCellAsync(e.RowIndex, inputAccountId);

                if (!isValid)
                {
                    e.Cancel = true;
                }
            }
        }

        public async Task<bool> ValidateAccountIdCellAsync(int rowIndex, string accountId)
        {
            if (string.IsNullOrWhiteSpace(accountId))
            {
                this.Rows[rowIndex].Cells["Col_AccountName"].Value = string.Empty;
                return true;
            }

            try
            {
                string query = $"SELECT Account_Name FROM Accounts WHERE Account_Id = @AccId AND {SystemConstants.AuditFields.IsDeleted} = 0";
                SqlParameter[] p = { new SqlParameter("@AccId", accountId.Trim()) };
                object result = await DatabaseHelper.ExecuteScalarAsync(query, p);

                if (result != null && result != DBNull.Value)
                {
                    this.Rows[rowIndex].Cells["Col_AccountName"].Value = result.ToString();
                    return true;
                }
                else
                {
                    MessageBox.Show("رقم الحساب المدخل غير موجود في الدليل المحاسبي المعتمد.", "خطأ حساب", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.Rows[rowIndex].Cells["Col_AccountName"].Value = string.Empty;
                    return false;
                }
            }
            catch (Exception ex)
            {
                DatabaseHelper.LogSystemError(ex.Message, ex.StackTrace, "ValidateAccountIdCellAsync");
                return false;
            }
        }
    }
}