using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using AlRowad_ERP.Data;
using AlRowad_ERP.Core;
using AlRowad_ERP.Models;
using AlRowad_ERP.UI.Base;

namespace AlRowad_ERP.Forms
{
    public partial class AccountsForm : BaseEntryForm
    {
        // ... (بقية الملف) ...

        // ربط ExecuteSaveToDatabaseAsync مع SaveAccountAsync في المستودع
        protected override async Task<bool> ExecuteSaveToDatabaseAsync(SqlTransaction trans)
        {
            // 1) التحققات القائمة بالفعل
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
                        MessageBox.Show($"منع الحفظ: تناقض منطقي في العملة. لا يمكن أن تكون 'افتراضية' و'مجمدة' معاً.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                // 2) بناء AccountEntity من الحقول
                var account = new AccountEntity
                {
                    AccID = acc_ID.Text.Trim(),
                    AccName = acc_Name.Text.Trim(),
                    AccNameEn = acc_Name_En.Text.Trim(),
                    ParentID = string.IsNullOrWhiteSpace(parent_ID.Text) ? null : parent_ID.Text.Trim(),
                    AccountLevel = CurrentAccountLevel,
                    AccType = int.TryParse(acc_Type.SelectedValue?.ToString(), out int tVal) ? tVal : 0,
                    AccNature = int.TryParse(acc_Nature.SelectedValue?.ToString(), out int nVal) ? nVal : 0,
                    ReportType = int.TryParse(report_Type?.SelectedValue?.ToString(), out int rVal) ? rVal : 0,
                    IsStopped = is_Stopped.Checked,
                    RowVersion = _currentRowVersion,
                    UpdatedBy = UserSession.UserId,
                    CreatedBy = UserSession.UserId
                };

                // 3) بناء قائمة العملات من dgv_currencies
                var currencies = new List<AccountCurrencyDto>();
                foreach (DataGridViewRow row in dgv_currencies.Rows)
                {
                    if (row.IsNewRow || row.Cells[4].Value == null) continue;
                    currencies.Add(new AccountCurrencyDto
                    {
                        CurID = Convert.ToInt32(row.Cells[4].Value),
                        CurName = row.Cells[1].Value?.ToString(),
                        IsActive = Convert.ToBoolean(row.Cells[0].Value ?? false),
                        IsDefault = Convert.ToBoolean(row.Cells[2].Value ?? false),
                        IsFrozen = Convert.ToBoolean(row.Cells[3].Value ?? false)
                    });
                }

                // 4) استدعاء المستودع داخل المعاملة الممررة
                await AccountRepository.SaveAccountAsync(account, currencies, CurrentMode, trans);

                // 5) في حال النجاح يتم اعادة القيمة true ليدل على نجاح الحفظ
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

        // ... (بقية الملف) ...
    }
}
