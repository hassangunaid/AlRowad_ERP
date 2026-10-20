using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using AlRowad_ERP.Core;
using AlRowad_ERP.Core.Constants;
using AlRowad_ERP.Infrastructure.Services;
using AlRowad_ERP.UI.Base;

namespace AlRowad_ERP.UI.Forms.Settings
{
    public partial class FrmBackupSettings : BaseEntryForm
    {
        private int _currentSettingId = 0;

        public FrmBackupSettings()
        {
            InitializeComponent();
            PrimaryIdFieldName = "txt_BackupPath";
            MainTableName = "System_Backup_Settings";
        }

        protected override async void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            // الفئة الأم BaseEntryForm اكتشفت settingsToolBar وربطت أحداثه تلقائياً
            await LoadConfigurationAsync();
        }

        #region تجاوزات BaseEntryForm السيادية وفق الدستور

        public override void OnNew()
        {
            ChangeFormMode(FormMode.Edit);
        }

        public override void OnEdit()
        {
            ChangeFormMode(FormMode.Edit);
        }

        public override void OnCancel()
        {
            _ = LoadConfigurationAsync();
            ChangeFormMode(FormMode.RecordSelected);
        }

        public override void OnDelete()
        {
            MessageBox.Show(
                "إعدادات المنظومة الأساسية محمية برمجياً ولا يمكن حذفها نهائياً.",
                "قيد سيادي",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }

        protected override async Task<bool> ExecuteSaveToDatabaseAsync(SqlTransaction transaction)
        {
            if (string.IsNullOrWhiteSpace(txt_BackupPath.Text))
            {
                MessageBox.Show("يجب تحديد مسار محلي صالح للنسخ الاحتياطي.", "التحقق من البيانات", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_BackupPath.Focus();
                return false;
            }

            if (chkCloudBackup.Checked && string.IsNullOrWhiteSpace(txt_CloudPath.Text))
            {
                MessageBox.Show("لقد قمت بتفعيل النسخ السحابي/الشبكي، يرجى تحديد مسار مجلد درايف أو المجلد المشترك.", "التحقق من البيانات", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_CloudPath.Focus();
                return false;
            }

            string query = _currentSettingId == 0
                ? $@"INSERT INTO System_Backup_Settings 
                    (Backup_Path, Auto_Backup_Enabled, Backup_Interval_Hours, Max_Backup_Files, Cloud_Backup_Enabled, Cloud_Backup_Path, {SystemConstants.AuditFields.CreatedBy}) 
                    VALUES (@Path, @Enabled, @Interval, @MaxFiles, @CloudEnabled, @CloudPath, @UserId);
                    SELECT SCOPE_IDENTITY();"
                : @"UPDATE System_Backup_Settings 
                    SET Backup_Path = @Path, 
                        Auto_Backup_Enabled = @Enabled, 
                        Backup_Interval_Hours = @Interval, 
                        Max_Backup_Files = @MaxFiles, 
                        Cloud_Backup_Enabled = @CloudEnabled,
                        Cloud_Backup_Path = @CloudPath,
                        Modified_At = GETDATE(), 
                        Modified_By = @UserId 
                    WHERE Setting_Id = @Id;";

            using (var cmd = new SqlCommand(query, transaction.Connection, transaction))
            {
                cmd.Parameters.Add("@Path", SqlDbType.NVarChar, 500).Value = txt_BackupPath.Text.Trim();
                cmd.Parameters.Add("@Enabled", SqlDbType.Bit).Value = chkAutoBackup.Checked;
                cmd.Parameters.Add("@Interval", SqlDbType.Int).Value = (int)numIntervalHours.Value;
                cmd.Parameters.Add("@MaxFiles", SqlDbType.Int).Value = (int)numMaxFiles.Value;
                cmd.Parameters.Add("@CloudEnabled", SqlDbType.Bit).Value = chkCloudBackup.Checked;
                cmd.Parameters.Add("@CloudPath", SqlDbType.NVarChar, 500).Value = (object)txt_CloudPath.Text.Trim() ?? DBNull.Value;
                cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = this.CurrentUserId;

                if (_currentSettingId > 0)
                {
                    cmd.Parameters.Add("@Id", SqlDbType.Int).Value = _currentSettingId;
                    await cmd.ExecuteNonQueryAsync();
                }
                else
                {
                    object newId = await cmd.ExecuteScalarAsync();
                    if (newId != null && newId != DBNull.Value)
                    {
                        _currentSettingId = Convert.ToInt32(newId);
                    }
                }
            }

            return true;
        }

        protected override void LockControls(Control parent, bool isReadOnly)
        {
            base.LockControls(parent, isReadOnly);

            txt_BackupPath.ReadOnly = true;
            btnBrowse.Enabled = !isReadOnly;
            chkAutoBackup.Enabled = !isReadOnly;
            numIntervalHours.Enabled = !isReadOnly;
            numMaxFiles.Enabled = !isReadOnly;

            chkCloudBackup.Enabled = !isReadOnly;
            btnBrowseCloud.Enabled = !isReadOnly && chkCloudBackup.Checked;
            txt_CloudPath.ReadOnly = true;

            // زر النسخ المباشر متاح في وضع التصفح فقط
            btnBackupNow.Enabled = isReadOnly;
        }

        #endregion

        #region العمليات والتحميل

        private async Task LoadConfigurationAsync()
        {
            try
            {
                string query = $"SELECT TOP 1 * FROM System_Backup_Settings WHERE {SystemConstants.AuditFields.IsDeleted} = 0";
                DataTable dt = await DatabaseHelper.GetTableAsync(query);

                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    _currentSettingId = Convert.ToInt32(row["Setting_Id"]);
                    txt_BackupPath.Text = row["Backup_Path"].ToString();
                    chkAutoBackup.Checked = Convert.ToBoolean(row["Auto_Backup_Enabled"]);
                    numIntervalHours.Value = Convert.ToInt32(row["Backup_Interval_Hours"]);
                    numMaxFiles.Value = Convert.ToInt32(row["Max_Backup_Files"]);

                    chkCloudBackup.Checked = row.Table.Columns.Contains("Cloud_Backup_Enabled") && row["Cloud_Backup_Enabled"] != DBNull.Value && Convert.ToBoolean(row["Cloud_Backup_Enabled"]);
                    txt_CloudPath.Text = row.Table.Columns.Contains("Cloud_Backup_Path") ? row["Cloud_Backup_Path"]?.ToString() : "";

                    var lastDate = row["Last_Backup_Date"];
                    lblLastBackup.Text = lastDate != DBNull.Value
                        ? $"آخر عملية نسخ: {Convert.ToDateTime(lastDate):yyyy-MM-dd HH:mm}"
                        : "آخر عملية نسخ: لم تتم بعد";

                    ChangeFormMode(FormMode.RecordSelected);
                }
                else
                {
                    _currentSettingId = 0;
                    txt_BackupPath.Text = @"D:\AlRowad_Backups";
                    txt_CloudPath.Text = @"G:\My Drive\AlRowad_Backups";
                    chkAutoBackup.Checked = true;
                    chkCloudBackup.Checked = false;
                    numIntervalHours.Value = 24;
                    numMaxFiles.Value = 7;
                    lblLastBackup.Text = "آخر عملية نسخ: لا يوجد سجل سابق";

                    ChangeFormMode(FormMode.New);
                }
            }
            catch (Exception ex)
            {
                DatabaseHelper.LogSystemError(ex.Message, ex.StackTrace, "LoadBackupConfig");
                MessageBox.Show($"خطأ أثناء استرجاع إعدادات المنظومة: {ex.Message}", "خطأ شبكة/بيانات", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ChkCloudBackup_CheckedChanged(object sender, EventArgs e)
        {
            btnBrowseCloud.Enabled = (CurrentMode == FormMode.Edit || CurrentMode == FormMode.New) && chkCloudBackup.Checked;
        }

        private void BtnBrowse_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                fbd.Description = "اختر المجلد المحلي على القرص لحفظ النسخ الاحتياطية";
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    txt_BackupPath.Text = fbd.SelectedPath;
                }
            }
        }

        private void BtnBrowseCloud_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                fbd.Description = "اختر مجلد مزامنة Google Drive / OneDrive أو المسار الشبكي المشترك";
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    txt_CloudPath.Text = fbd.SelectedPath;
                }
            }
        }

        private async void BtnBackupNow_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txt_BackupPath.Text))
            {
                MessageBox.Show("يرجى تحديد المسار المحلي للنسخ أولاً.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                ShowLoading();
                btnBackupNow.Enabled = false;

                using (var conn = await DatabaseHelper.GetConnectionAsync())
                {
                    bool completed = await BackupEngine.ExecuteBackupAsync(
                        conn.ConnectionString,
                        txt_BackupPath.Text,
                        (int)numMaxFiles.Value,
                        chkCloudBackup.Checked,
                        txt_CloudPath.Text);

                    if (completed)
                    {
                        string updateQuery = "UPDATE System_Backup_Settings SET Last_Backup_Date = GETDATE() WHERE Setting_Id = @Id";
                        SqlParameter[] parameters = { new SqlParameter("@Id", _currentSettingId) };
                        await DatabaseHelper.ExecuteNonQueryAsync(updateQuery, parameters);

                        lblLastBackup.Text = $"آخر عملية نسخ: {DateTime.Now:yyyy-MM-dd HH:mm}";
                        MessageBox.Show("اكتملت عملية النسخ الاحتياطي والمزامنة السحابية بنجاح.", "إشعار نظام", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                DatabaseHelper.LogSystemError(ex.Message, ex.StackTrace, "ManualBackupExecution");
                MessageBox.Show($"فشل إجراء النسخ الاحتياطي: {ex.Message}", "خطأ حرج", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                HideLoading();
                btnBackupNow.Enabled = true;
            }
        }

        #endregion
    }
}