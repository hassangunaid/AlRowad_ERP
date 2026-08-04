namespace AlRowadERP.UI.Themes
{
    partial class Frm_ThemeManager
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
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
            this.lblPredefinedTheme = new System.Windows.Forms.Label();
            this.cmbPredefinedThemes = new System.Windows.Forms.ComboBox();
            this.btnApplyPredefinedTheme = new System.Windows.Forms.Button();
            this.lblFontFamily = new System.Windows.Forms.Label();
            this.cmbFontFamily = new System.Windows.Forms.ComboBox();
            this.lblFontSize = new System.Windows.Forms.Label();
            this.numFontSize = new System.Windows.Forms.NumericUpDown();
            this.tcThemeSettings = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.txtThemeName = new System.Windows.Forms.TextBox();
            this.chkIsActive = new System.Windows.Forms.CheckBox();
            this.txtThemeID = new System.Windows.Forms.TextBox();
            this.IsPredefined = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.numFontSize)).BeginInit();
            this.tcThemeSettings.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblPredefinedTheme
            // 
            this.lblPredefinedTheme.Location = new System.Drawing.Point(0, 0);
            this.lblPredefinedTheme.Name = "lblPredefinedTheme";
            this.lblPredefinedTheme.Size = new System.Drawing.Size(100, 23);
            this.lblPredefinedTheme.TabIndex = 0;
            // 
            // cmbPredefinedThemes
            // 
            this.cmbPredefinedThemes.Location = new System.Drawing.Point(0, 0);
            this.cmbPredefinedThemes.Name = "cmbPredefinedThemes";
            this.cmbPredefinedThemes.Size = new System.Drawing.Size(121, 27);
            this.cmbPredefinedThemes.TabIndex = 0;
            // 
            // btnApplyPredefinedTheme
            // 
            this.btnApplyPredefinedTheme.Location = new System.Drawing.Point(0, 0);
            this.btnApplyPredefinedTheme.Name = "btnApplyPredefinedTheme";
            this.btnApplyPredefinedTheme.Size = new System.Drawing.Size(75, 23);
            this.btnApplyPredefinedTheme.TabIndex = 0;
            // 
            // lblFontFamily
            // 
            this.lblFontFamily.Location = new System.Drawing.Point(0, 0);
            this.lblFontFamily.Name = "lblFontFamily";
            this.lblFontFamily.Size = new System.Drawing.Size(100, 23);
            this.lblFontFamily.TabIndex = 0;
            // 
            // cmbFontFamily
            // 
            this.cmbFontFamily.Location = new System.Drawing.Point(0, 0);
            this.cmbFontFamily.Name = "cmbFontFamily";
            this.cmbFontFamily.Size = new System.Drawing.Size(121, 27);
            this.cmbFontFamily.TabIndex = 0;
            // 
            // lblFontSize
            // 
            this.lblFontSize.Location = new System.Drawing.Point(0, 0);
            this.lblFontSize.Name = "lblFontSize";
            this.lblFontSize.Size = new System.Drawing.Size(100, 23);
            this.lblFontSize.TabIndex = 0;
            // 
            // numFontSize
            // 
            this.numFontSize.Location = new System.Drawing.Point(0, 0);
            this.numFontSize.Name = "numFontSize";
            this.numFontSize.Size = new System.Drawing.Size(120, 27);
            this.numFontSize.TabIndex = 0;
            // 
            // tcThemeSettings
            // 
            this.tcThemeSettings.Controls.Add(this.tabPage1);
            this.tcThemeSettings.Controls.Add(this.tabPage2);
            this.tcThemeSettings.Location = new System.Drawing.Point(12, 12);
            this.tcThemeSettings.Name = "tcThemeSettings";
            this.tcThemeSettings.RightToLeftLayout = true;
            this.tcThemeSettings.SelectedIndex = 0;
            this.tcThemeSettings.Size = new System.Drawing.Size(964, 700);
            this.tcThemeSettings.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.IsPredefined);
            this.tabPage1.Controls.Add(this.txtThemeID);
            this.tabPage1.Controls.Add(this.chkIsActive);
            this.tabPage1.Controls.Add(this.txtThemeName);
            this.tabPage1.Location = new System.Drawing.Point(4, 34);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(956, 662);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "الثيمات";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 34);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(956, 662);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "اعدادات الخط";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // txtThemeName
            // 
            this.txtThemeName.Location = new System.Drawing.Point(688, 57);
            this.txtThemeName.Name = "txtThemeName";
            this.txtThemeName.Size = new System.Drawing.Size(100, 30);
            this.txtThemeName.TabIndex = 0;
            // 
            // chkIsActive
            // 
            this.chkIsActive.AutoSize = true;
            this.chkIsActive.Location = new System.Drawing.Point(391, 93);
            this.chkIsActive.Name = "chkIsActive";
            this.chkIsActive.Size = new System.Drawing.Size(135, 29);
            this.chkIsActive.TabIndex = 1;
            this.chkIsActive.Text = "checkBox1";
            this.chkIsActive.UseVisualStyleBackColor = true;
            // 
            // txtThemeID
            // 
            this.txtThemeID.Location = new System.Drawing.Point(428, 316);
            this.txtThemeID.Name = "txtThemeID";
            this.txtThemeID.Size = new System.Drawing.Size(100, 30);
            this.txtThemeID.TabIndex = 2;
            // 
            // IsPredefined
            // 
            this.IsPredefined.AutoSize = true;
            this.IsPredefined.Location = new System.Drawing.Point(599, 212);
            this.IsPredefined.Name = "IsPredefined";
            this.IsPredefined.Size = new System.Drawing.Size(135, 29);
            this.IsPredefined.TabIndex = 3;
            this.IsPredefined.Text = "checkBox1";
            this.IsPredefined.UseVisualStyleBackColor = true;
            // 
            // Frm_ThemeManager
            // 
            this.ClientSize = new System.Drawing.Size(978, 744);
            this.Controls.Add(this.tcThemeSettings);
            this.Name = "Frm_ThemeManager";
            this.Text = "إدارة المظهر والثيمات";
            this.Load += new System.EventHandler(this.Frm_ThemeManager_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numFontSize)).EndInit();
            this.tcThemeSettings.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label lblPredefinedTheme;
        private System.Windows.Forms.ComboBox cmbPredefinedThemes;
        private System.Windows.Forms.Button btnApplyPredefinedTheme;
        private System.Windows.Forms.Label lblFontFamily;
        private System.Windows.Forms.ComboBox cmbFontFamily;
        private System.Windows.Forms.Label lblFontSize;
        private System.Windows.Forms.NumericUpDown numFontSize;
        private System.Windows.Forms.TabControl tcThemeSettings;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TextBox txtThemeName;
        private System.Windows.Forms.CheckBox chkIsActive;
        private System.Windows.Forms.TextBox txtThemeID;
        private System.Windows.Forms.CheckBox IsPredefined;
    }
}