using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows.Forms;
using AlRowad_ERP.Data;
using AlRowad_ERP.Core;

namespace AlRowad_ERP.Forms
{
    public partial class AccountsForm : BaseEntryForm
    {
        public override async void OnDelete()
        {
            if (string.IsNullOrEmpty(acc_ID.Text) || CurrentMode != FormMode.View && CurrentMode != FormMode.RecordSelected)
            {
                MessageBox.Show("يجب استعراض الحساب المراد حذفه أولاً.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning); return;
            }

            if (MessageBox.Show($"هل أنت متأكد من حذف الحساب: ({acc_Name.Text})؟", "تأكيد", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                // استخدام ExecuteTransactionAsync المركزي لضمان ACID
                try
                {
                    await DatabaseHelper.ExecuteTransactionAsync(async (trans) =>
                    {
                        // تنفيذ الحذف داخل نفس المعاملة
                        await AccountRepository.DeleteAccountAsync(acc_ID.Text.Trim(), UserSession.UserId, trans);

                        // تسجيل الحذف في سجل التدقيق
                        DatabaseHelper.LogAuditTransaction(trans, "Accounts", acc_ID.Text.Trim(), "DELETE", "تم الحذف المسبق", $"حذف الحساب: {acc_Name.Text}", "حذف حساب");
                    });

                    MessageBox.Show("تم حذف الحساب بنجاح.", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    acc_ID.Clear(); acc_Name.Clear();
                    ChangeFormMode(FormMode.View);
                    await BuildAccountsTreeStructureAsync();
                }
                catch (InvalidOperationException ex)
                {
                    MessageBox.Show(ex.Message, "منع الحذف", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "حماية النظام", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
        }
    }
}
