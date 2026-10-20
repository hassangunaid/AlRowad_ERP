namespace AlRowad_ERP.UI.Forms.Settings
{
    partial class FrmBackupSettings
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.grpBackupConfig = new System.Windows.Forms.GroupBox();
            this.lblFilesUnit = new System.Windows.Forms.Label();
            this.numMaxFiles = new System.Windows.Forms.NumericUpDown();
            this.lblFiles = new System.Windows.Forms.Label();
            this.lblHoursUnit = new System.Windows.Forms.Label();
            this.numIntervalHours = new System.Windows.Forms.NumericUpDown();
            this.lblInterval = new System.Windows.Forms.Label();
            this.chkAutoBackup = new System.Windows.Forms.CheckBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.txt_BackupPath = new System.Windows.Forms.TextBox();
            this.lblPath = new System.Windows.Forms.Label();
            this.grpCloud = new System.Windows.Forms.GroupBox();
            this.btnBrowseCloud = new System.Windows.Forms.Button();
            this.txt_CloudPath = new System.Windows.Forms.TextBox();
            this.lblCloudPath = new System.Windows.Forms.Label();
            this.chkCloudBackup = new System.Windows.Forms.CheckBox();
            this.grpStatus = new System.Windows.Forms.GroupBox();
            this.lblLastBackup = new System.Windows.Forms.Label();
            this.btnBackupNow = new System.Windows.Forms.Button();
            this.settingsToolBar = new AlRowad_ERP.Controls.AlRowadSettingsToolBar();
            this.grpBackupConfig.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxFiles)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numIntervalHours)).BeginInit();
            this.grpCloud.SuspendLayout();
            this.grpStatus.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpBackupConfig
            // 
            this.grpBackupConfig.Controls.Add(this.lblFilesUnit);
            this.grpBackupConfig.Controls.Add(this.numMaxFiles);
            this.grpBackupConfig.Controls.Add(this.lblFiles);
            this.grpBackupConfig.Controls.Add(this.lblHoursUnit);
            this.grpBackupConfig.Controls.Add(this.numIntervalHours);
            this.grpBackupConfig.Controls.Add(this.lblInterval);
            this.grpBackupConfig.Controls.Add(this.chkAutoBackup);
            this.grpBackupConfig.Controls.Add(this.btnBrowse);
            this.grpBackupConfig.Controls.Add(this.txt_BackupPath);
            this.grpBackupConfig.Controls.Add(this.lblPath);
            this.grpBackupConfig.Location = new System.Drawing.Point(21, 94);
            this.grpBackupConfig.Margin = new System.Windows.Forms.Padding(5);
            this.grpBackupConfig.Name = "grpBackupConfig";
            this.grpBackupConfig.Padding = new System.Windows.Forms.Padding(5);
            this.grpBackupConfig.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.grpBackupConfig.Size = new System.Drawing.Size(1046, 273);
            this.grpBackupConfig.TabIndex = 0;
            this.grpBackupConfig.TabStop = false;
            this.grpBackupConfig.Text = "إعدادات المسار المحلي والجدولة";
            // 
            // lblFilesUnit
            // 
            this.lblFilesUnit.AutoSize = true;
            this.lblFilesUnit.Location = new System.Drawing.Point(497, 219);
            this.lblFilesUnit.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblFilesUnit.Name = "lblFilesUnit";
            this.lblFilesUnit.Size = new System.Drawing.Size(66, 25);
            this.lblFilesUnit.TabIndex = 9;
            this.lblFilesUnit.Text = "نسخ فقط";
            // 
            // numMaxFiles
            // 
            this.numMaxFiles.Location = new System.Drawing.Point(626, 216);
            this.numMaxFiles.Margin = new System.Windows.Forms.Padding(5);
            this.numMaxFiles.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.numMaxFiles.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numMaxFiles.Name = "numMaxFiles";
            this.numMaxFiles.Size = new System.Drawing.Size(137, 30);
            this.numMaxFiles.TabIndex = 8;
            this.numMaxFiles.Value = new decimal(new int[] {
            7,
            0,
            0,
            0});
            // 
            // lblFiles
            // 
            this.lblFiles.AutoSize = true;
            this.lblFiles.Location = new System.Drawing.Point(797, 219);
            this.lblFiles.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblFiles.Name = "lblFiles";
            this.lblFiles.Size = new System.Drawing.Size(118, 25);
            this.lblFiles.TabIndex = 7;
            this.lblFiles.Text = "الاحتفاظ بأحدث:";
            // 
            // lblHoursUnit
            // 
            this.lblHoursUnit.AutoSize = true;
            this.lblHoursUnit.Location = new System.Drawing.Point(540, 162);
            this.lblHoursUnit.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblHoursUnit.Name = "lblHoursUnit";
            this.lblHoursUnit.Size = new System.Drawing.Size(47, 25);
            this.lblHoursUnit.TabIndex = 6;
            this.lblHoursUnit.Text = "ساعة";
            // 
            // numIntervalHours
            // 
            this.numIntervalHours.Location = new System.Drawing.Point(626, 159);
            this.numIntervalHours.Margin = new System.Windows.Forms.Padding(5);
            this.numIntervalHours.Maximum = new decimal(new int[] {
            72,
            0,
            0,
            0});
            this.numIntervalHours.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numIntervalHours.Name = "numIntervalHours";
            this.numIntervalHours.Size = new System.Drawing.Size(137, 30);
            this.numIntervalHours.TabIndex = 5;
            this.numIntervalHours.Value = new decimal(new int[] {
            24,
            0,
            0,
            0});
            // 
            // lblInterval
            // 
            this.lblInterval.AutoSize = true;
            this.lblInterval.Location = new System.Drawing.Point(797, 162);
            this.lblInterval.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblInterval.Name = "lblInterval";
            this.lblInterval.Size = new System.Drawing.Size(134, 25);
            this.lblInterval.TabIndex = 4;
            this.lblInterval.Text = "التكرار الزمني كل:";
            // 
            // chkAutoBackup
            // 
            this.chkAutoBackup.AutoSize = true;
            this.chkAutoBackup.Location = new System.Drawing.Point(609, 106);
            this.chkAutoBackup.Margin = new System.Windows.Forms.Padding(5);
            this.chkAutoBackup.Name = "chkAutoBackup";
            this.chkAutoBackup.Size = new System.Drawing.Size(281, 29);
            this.chkAutoBackup.TabIndex = 3;
            this.chkAutoBackup.Text = "تفعيل النسخ الاحتياطي التلقائي الدوري";
            this.chkAutoBackup.UseVisualStyleBackColor = true;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(26, 42);
            this.btnBrowse.Margin = new System.Windows.Forms.Padding(5);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(146, 44);
            this.btnBrowse.TabIndex = 2;
            this.btnBrowse.Text = "استعراض...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.BtnBrowse_Click);
            // 
            // txt_BackupPath
            // 
            this.txt_BackupPath.Location = new System.Drawing.Point(189, 45);
            this.txt_BackupPath.Margin = new System.Windows.Forms.Padding(5);
            this.txt_BackupPath.Name = "txt_BackupPath";
            this.txt_BackupPath.ReadOnly = true;
            this.txt_BackupPath.Size = new System.Drawing.Size(666, 30);
            this.txt_BackupPath.TabIndex = 1;
            // 
            // lblPath
            // 
            this.lblPath.AutoSize = true;
            this.lblPath.Location = new System.Drawing.Point(874, 50);
            this.lblPath.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblPath.Name = "lblPath";
            this.lblPath.Size = new System.Drawing.Size(108, 25);
            this.lblPath.TabIndex = 0;
            this.lblPath.Text = "المسار المحلي:";
            // 
            // grpCloud
            // 
            this.grpCloud.Controls.Add(this.btnBrowseCloud);
            this.grpCloud.Controls.Add(this.txt_CloudPath);
            this.grpCloud.Controls.Add(this.lblCloudPath);
            this.grpCloud.Controls.Add(this.chkCloudBackup);
            this.grpCloud.Location = new System.Drawing.Point(21, 383);
            this.grpCloud.Margin = new System.Windows.Forms.Padding(5);
            this.grpCloud.Name = "grpCloud";
            this.grpCloud.Padding = new System.Windows.Forms.Padding(5);
            this.grpCloud.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.grpCloud.Size = new System.Drawing.Size(1046, 180);
            this.grpCloud.TabIndex = 1;
            this.grpCloud.TabStop = false;
            this.grpCloud.Text = "المزامنة السحابية / الشبكية (Google Drive / OneDrive / Network)";
            // 
            // btnBrowseCloud
            // 
            this.btnBrowseCloud.Location = new System.Drawing.Point(26, 105);
            this.btnBrowseCloud.Margin = new System.Windows.Forms.Padding(5);
            this.btnBrowseCloud.Name = "btnBrowseCloud";
            this.btnBrowseCloud.Size = new System.Drawing.Size(146, 44);
            this.btnBrowseCloud.TabIndex = 3;
            this.btnBrowseCloud.Text = "استعراض...";
            this.btnBrowseCloud.UseVisualStyleBackColor = true;
            this.btnBrowseCloud.Click += new System.EventHandler(this.BtnBrowseCloud_Click);
            // 
            // txt_CloudPath
            // 
            this.txt_CloudPath.Location = new System.Drawing.Point(189, 108);
            this.txt_CloudPath.Margin = new System.Windows.Forms.Padding(5);
            this.txt_CloudPath.Name = "txt_CloudPath";
            this.txt_CloudPath.ReadOnly = true;
            this.txt_CloudPath.Size = new System.Drawing.Size(666, 30);
            this.txt_CloudPath.TabIndex = 2;
            // 
            // lblCloudPath
            // 
            this.lblCloudPath.AutoSize = true;
            this.lblCloudPath.Location = new System.Drawing.Point(874, 112);
            this.lblCloudPath.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblCloudPath.Name = "lblCloudPath";
            this.lblCloudPath.Size = new System.Drawing.Size(105, 25);
            this.lblCloudPath.TabIndex = 1;
            this.lblCloudPath.Text = "مسار السحابة:";
            // 
            // chkCloudBackup
            // 
            this.chkCloudBackup.AutoSize = true;
            this.chkCloudBackup.Location = new System.Drawing.Point(531, 47);
            this.chkCloudBackup.Margin = new System.Windows.Forms.Padding(5);
            this.chkCloudBackup.Name = "chkCloudBackup";
            this.chkCloudBackup.Size = new System.Drawing.Size(363, 29);
            this.chkCloudBackup.TabIndex = 0;
            this.chkCloudBackup.Text = "تفعيل مزامنة نسخة إضافية إلى مسار سحابي/شبكي";
            this.chkCloudBackup.UseVisualStyleBackColor = true;
            this.chkCloudBackup.CheckedChanged += new System.EventHandler(this.ChkCloudBackup_CheckedChanged);
            // 
            // grpStatus
            // 
            this.grpStatus.Controls.Add(this.lblLastBackup);
            this.grpStatus.Controls.Add(this.btnBackupNow);
            this.grpStatus.Location = new System.Drawing.Point(21, 578);
            this.grpStatus.Margin = new System.Windows.Forms.Padding(5);
            this.grpStatus.Name = "grpStatus";
            this.grpStatus.Padding = new System.Windows.Forms.Padding(5);
            this.grpStatus.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.grpStatus.Size = new System.Drawing.Size(1046, 125);
            this.grpStatus.TabIndex = 2;
            this.grpStatus.TabStop = false;
            this.grpStatus.Text = "الحالة والتشغيل المباشر";
            // 
            // lblLastBackup
            // 
            this.lblLastBackup.AutoSize = true;
            this.lblLastBackup.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLastBackup.Location = new System.Drawing.Point(360, 55);
            this.lblLastBackup.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblLastBackup.Name = "lblLastBackup";
            this.lblLastBackup.Size = new System.Drawing.Size(262, 22);
            this.lblLastBackup.TabIndex = 0;
            this.lblLastBackup.Text = "آخر عملية نسخ: لا يوجد بيانات";
            // 
            // btnBackupNow
            // 
            this.btnBackupNow.Location = new System.Drawing.Point(26, 39);
            this.btnBackupNow.Margin = new System.Windows.Forms.Padding(5);
            this.btnBackupNow.Name = "btnBackupNow";
            this.btnBackupNow.Size = new System.Drawing.Size(283, 59);
            this.btnBackupNow.TabIndex = 1;
            this.btnBackupNow.Text = "أخذ نسخة يدوية فوراً";
            this.btnBackupNow.UseVisualStyleBackColor = true;
            this.btnBackupNow.Click += new System.EventHandler(this.BtnBackupNow_Click);
            // 
            // settingsToolBar
            // 
            this.settingsToolBar.Location = new System.Drawing.Point(81, 14);
            this.settingsToolBar.Margin = new System.Windows.Forms.Padding(5);
            this.settingsToolBar.Name = "settingsToolBar";
            this.settingsToolBar.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.settingsToolBar.Size = new System.Drawing.Size(890, 56);
            this.settingsToolBar.TabIndex = 3;
            // 
            // FrmBackupSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1087, 727);
            this.Controls.Add(this.settingsToolBar);
            this.Controls.Add(this.grpStatus);
            this.Controls.Add(this.grpCloud);
            this.Controls.Add(this.grpBackupConfig);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmBackupSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "إعدادات النسخ الاحتياطي التلقائي والسحابي";
            this.grpBackupConfig.ResumeLayout(false);
            this.grpBackupConfig.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxFiles)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numIntervalHours)).EndInit();
            this.grpCloud.ResumeLayout(false);
            this.grpCloud.PerformLayout();
            this.grpStatus.ResumeLayout(false);
            this.grpStatus.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpBackupConfig;
        private System.Windows.Forms.Label lblPath;
        private System.Windows.Forms.TextBox txt_BackupPath;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.CheckBox chkAutoBackup;
        private System.Windows.Forms.Label lblInterval;
        private System.Windows.Forms.NumericUpDown numIntervalHours;
        private System.Windows.Forms.Label lblHoursUnit;
        private System.Windows.Forms.Label lblFiles;
        private System.Windows.Forms.NumericUpDown numMaxFiles;
        private System.Windows.Forms.Label lblFilesUnit;
        private System.Windows.Forms.GroupBox grpCloud;
        private System.Windows.Forms.CheckBox chkCloudBackup;
        private System.Windows.Forms.Label lblCloudPath;
        private System.Windows.Forms.TextBox txt_CloudPath;
        private System.Windows.Forms.Button btnBrowseCloud;
        private System.Windows.Forms.GroupBox grpStatus;
        private System.Windows.Forms.Label lblLastBackup;
        private System.Windows.Forms.Button btnBackupNow;
        private AlRowad_ERP.Controls.AlRowadSettingsToolBar settingsToolBar;
    }
}