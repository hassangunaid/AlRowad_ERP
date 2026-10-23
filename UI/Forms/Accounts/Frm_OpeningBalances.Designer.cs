namespace AlRowad_ERP.UI.Forms.Accounts
{
    partial class Frm_OpeningBalances
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
            this.grpHeader = new System.Windows.Forms.GroupBox();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.lblDescription = new System.Windows.Forms.Label();
            this.dtpDocumentDate = new AlRowad_ERP.UI.Controls.AlRowadDateTextBox();
            this.lblDocDate = new System.Windows.Forms.Label();
            this.txtDocumentNo = new System.Windows.Forms.TextBox();
            this.lblDocNo = new System.Windows.Forms.Label();

            this.grpDetails = new System.Windows.Forms.GroupBox();
            this.dgvOpeningDetails = new AlRowad_ERP.UI.Controls.AlRowadAccountsGrid();
            this.Col_AccountId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col_AccountName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col_Currency = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.Col_ExchangeRate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col_ForeignDebit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col_ForeignCredit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col_Debit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col_Credit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col_Memo = new System.Windows.Forms.DataGridViewTextBoxColumn();

            this.pnlTotals = new System.Windows.Forms.Panel();
            this.lblTotalCreditVal = new System.Windows.Forms.Label();
            this.lblTotalDebitVal = new System.Windows.Forms.Label();
            this.lblCreditTitle = new System.Windows.Forms.Label();
            this.lblDebitTitle = new System.Windows.Forms.Label();

            this.settingsToolBar = new AlRowad_ERP.Controls.AlRowadSettingsToolBar();

            this.grpHeader.SuspendLayout();
            this.grpDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOpeningDetails)).BeginInit();
            this.pnlTotals.SuspendLayout();
            this.SuspendLayout();
            // 
            // settingsToolBar
            // 
            this.settingsToolBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.settingsToolBar.Location = new System.Drawing.Point(0, 0);
            this.settingsToolBar.Margin = new System.Windows.Forms.Padding(0);
            this.settingsToolBar.Name = "settingsToolBar";
            this.settingsToolBar.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.settingsToolBar.Size = new System.Drawing.Size(1250, 50);
            this.settingsToolBar.TabIndex = 0;
            // 
            // grpHeader
            // 
            this.grpHeader.Controls.Add(this.txtDescription);
            this.grpHeader.Controls.Add(this.lblDescription);
            this.grpHeader.Controls.Add(this.dtpDocumentDate);
            this.grpHeader.Controls.Add(this.lblDocDate);
            this.grpHeader.Controls.Add(this.txtDocumentNo);
            this.grpHeader.Controls.Add(this.lblDocNo);
            this.grpHeader.Location = new System.Drawing.Point(12, 60);
            this.grpHeader.Name = "grpHeader";
            this.grpHeader.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.grpHeader.Size = new System.Drawing.Size(1226, 110);
            this.grpHeader.TabIndex = 1;
            this.grpHeader.TabStop = false;
            this.grpHeader.Text = "بيانات القيد الافتتاحي (الرأس)";
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(20, 68);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(1075, 24);
            this.txtDescription.TabIndex = 5;
            this.txtDescription.Tag = "Description";
            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.lblDescription.Location = new System.Drawing.Point(1115, 71);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(51, 17);
            this.lblDescription.TabIndex = 4;
            this.lblDescription.Text = "البيان:";
            // 
            // dtpDocumentDate
            // 
            this.dtpDocumentDate.Location = new System.Drawing.Point(680, 28);
            this.dtpDocumentDate.Name = "dtpDocumentDate";
            this.dtpDocumentDate.Size = new System.Drawing.Size(150, 24);
            this.dtpDocumentDate.TabIndex = 3;
            this.dtpDocumentDate.Tag = "Document_Date";
            // 
            // lblDocDate
            // 
            this.lblDocDate.AutoSize = true;
            this.lblDocDate.Location = new System.Drawing.Point(850, 31);
            this.lblDocDate.Name = "lblDocDate";
            this.lblDocDate.Size = new System.Drawing.Size(83, 17);
            this.lblDocDate.TabIndex = 2;
            this.lblDocDate.Text = "تاريخ البداية:";
            // 
            // txtDocumentNo
            // 
            this.txtDocumentNo.Location = new System.Drawing.Point(78, 28);
            this.txtDocumentNo.Name = "txtDocumentNo";
            this.txtDocumentNo.ReadOnly = true;
            this.txtDocumentNo.Size = new System.Drawing.Size(150, 24);
            this.txtDocumentNo.TabIndex = 1;
            this.txtDocumentNo.Tag = "Document_No";
            // 
            // lblDocNo
            // 
            this.lblDocNo.AutoSize = true;
            this.lblDocNo.Location = new System.Drawing.Point(245, 31);
            this.lblDocNo.Name = "lblDocNo";
            this.lblDocNo.Size = new System.Drawing.Size(73, 17);
            this.lblDocNo.TabIndex = 0;
            this.lblDocNo.Text = "رقم القيد:";
            // 
            // grpDetails
            // 
            this.grpDetails.Controls.Add(this.dgvOpeningDetails);
            this.grpDetails.Location = new System.Drawing.Point(12, 180);
            this.grpDetails.Name = "grpDetails";
            this.grpDetails.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.grpDetails.Size = new System.Drawing.Size(1226, 390);
            this.grpDetails.TabIndex = 2;
            this.grpDetails.TabStop = false;
            this.grpDetails.Text = "تفاصيل الحسابات، العملات الأجنبية، والأرصدة الافتتاحية";
            // 
            // dgvOpeningDetails
            // 
            this.dgvOpeningDetails.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgvOpeningDetails.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvOpeningDetails.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Sunken;
            this.dgvOpeningDetails.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgvOpeningDetails.ColumnHeadersHeight = 34;
            this.dgvOpeningDetails.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Col_AccountId,
            this.Col_AccountName,
            this.Col_Currency,
            this.Col_ExchangeRate,
            this.Col_ForeignDebit,
            this.Col_ForeignCredit,
            this.Col_Debit,
            this.Col_Credit,
            this.Col_Memo});
            this.dgvOpeningDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvOpeningDetails.EnableHeadersVisualStyles = false;
            this.dgvOpeningDetails.Location = new System.Drawing.Point(3, 20);
            this.dgvOpeningDetails.Name = "dgvOpeningDetails";
            this.dgvOpeningDetails.RowHeadersVisible = false;
            this.dgvOpeningDetails.RowTemplate.Height = 29;
            this.dgvOpeningDetails.Size = new System.Drawing.Size(1220, 367);
            this.dgvOpeningDetails.TabIndex = 0;
            this.dgvOpeningDetails.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvOpeningDetails_CellValueChanged);
            this.dgvOpeningDetails.CurrentCellDirtyStateChanged += new System.EventHandler(this.dgvOpeningDetails_CurrentCellDirtyStateChanged);
            // 
            // Col_AccountId
            // 
            this.Col_AccountId.HeaderText = "رقم الحساب (F9)";
            this.Col_AccountId.MinimumWidth = 6;
            this.Col_AccountId.Name = "Col_AccountId";
            this.Col_AccountId.Width = 120;
            // 
            // Col_AccountName
            // 
            this.Col_AccountName.HeaderText = "اسم الحساب (F9)";
            this.Col_AccountName.MinimumWidth = 6;
            this.Col_AccountName.Name = "Col_AccountName";
            this.Col_AccountName.ReadOnly = true;
            this.Col_AccountName.Width = 220;
            // 
            // Col_Currency
            // 
            this.Col_Currency.HeaderText = "العملة";
            this.Col_Currency.MinimumWidth = 6;
            this.Col_Currency.Name = "Col_Currency";
            this.Col_Currency.Width = 110;
            // 
            // Col_ExchangeRate
            // 
            this.Col_ExchangeRate.HeaderText = "سعر الصرف";
            this.Col_ExchangeRate.MinimumWidth = 6;
            this.Col_ExchangeRate.Name = "Col_ExchangeRate";
            this.Col_ExchangeRate.Width = 100;
            // 
            // Col_ForeignDebit
            // 
            this.Col_ForeignDebit.HeaderText = "مدين أجنبي";
            this.Col_ForeignDebit.MinimumWidth = 6;
            this.Col_ForeignDebit.Name = "Col_ForeignDebit";
            this.Col_ForeignDebit.Width = 120;
            // 
            // Col_ForeignCredit
            // 
            this.Col_ForeignCredit.HeaderText = "دائن أجنبي";
            this.Col_ForeignCredit.MinimumWidth = 6;
            this.Col_ForeignCredit.Name = "Col_ForeignCredit";
            this.Col_ForeignCredit.Width = 120;
            // 
            // Col_Debit
            // 
            this.Col_Debit.HeaderText = "مدين محلي";
            this.Col_Debit.MinimumWidth = 6;
            this.Col_Debit.Name = "Col_Debit";
            this.Col_Debit.Width = 120;
            // 
            // Col_Credit
            // 
            this.Col_Credit.HeaderText = "دائن محلي";
            this.Col_Credit.MinimumWidth = 6;
            this.Col_Credit.Name = "Col_Credit";
            this.Col_Credit.Width = 120;
            // 
            // Col_Memo
            // 
            this.Col_Memo.HeaderText = "البيان / ملاحظات";
            this.Col_Memo.MinimumWidth = 6;
            this.Col_Memo.Name = "Col_Memo";
            this.Col_Memo.Width = 190;
            // 
            // pnlTotals
            // 
            this.pnlTotals.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlTotals.Controls.Add(this.lblTotalCreditVal);
            this.pnlTotals.Controls.Add(this.lblTotalDebitVal);
            this.pnlTotals.Controls.Add(this.lblCreditTitle);
            this.pnlTotals.Controls.Add(this.lblDebitTitle);
            this.pnlTotals.Location = new System.Drawing.Point(12, 580);
            this.pnlTotals.Name = "pnlTotals";
            this.pnlTotals.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.pnlTotals.Size = new System.Drawing.Size(1226, 50);
            this.pnlTotals.TabIndex = 3;
            // 
            // lblTotalCreditVal
            // 
            this.lblTotalCreditVal.AutoSize = true;
            this.lblTotalCreditVal.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.lblTotalCreditVal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(57)))), ((int)(((byte)(43)))));
            this.lblTotalCreditVal.Location = new System.Drawing.Point(200, 15);
            this.lblTotalCreditVal.Name = "lblTotalCreditVal";
            this.lblTotalCreditVal.Size = new System.Drawing.Size(43, 18);
            this.lblTotalCreditVal.TabIndex = 3;
            this.lblTotalCreditVal.Text = "0.00";
            // 
            // lblTotalDebitVal
            // 
            this.lblTotalDebitVal.AutoSize = true;
            this.lblTotalDebitVal.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.lblTotalDebitVal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(174)))), ((int)(((byte)(96)))));
            this.lblTotalDebitVal.Location = new System.Drawing.Point(750, 15);
            this.lblTotalDebitVal.Name = "lblTotalDebitVal";
            this.lblTotalDebitVal.Size = new System.Drawing.Size(43, 18);
            this.lblTotalDebitVal.TabIndex = 2;
            this.lblTotalDebitVal.Text = "0.00";
            // 
            // lblCreditTitle
            // 
            this.lblCreditTitle.AutoSize = true;
            this.lblCreditTitle.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.lblCreditTitle.Location = new System.Drawing.Point(320, 15);
            this.lblCreditTitle.Name = "lblCreditTitle";
            this.lblCreditTitle.Size = new System.Drawing.Size(95, 18);
            this.lblCreditTitle.TabIndex = 1;
            this.lblCreditTitle.Text = "إجمالي الدائن المحلي:";
            // 
            // lblDebitTitle
            // 
            this.lblDebitTitle.AutoSize = true;
            this.lblDebitTitle.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.lblDebitTitle.Location = new System.Drawing.Point(870, 15);
            this.lblDebitTitle.Name = "lblDebitTitle";
            this.lblDebitTitle.Size = new System.Drawing.Size(99, 18);
            this.lblDebitTitle.TabIndex = 0;
            this.lblDebitTitle.Text = "إجمالي المدين المحلي:";
            // 
            // Frm_OpeningBalances
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1250, 642);
            this.Controls.Add(this.pnlTotals);
            this.Controls.Add(this.grpDetails);
            this.Controls.Add(this.grpHeader);
            this.Controls.Add(this.settingsToolBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Frm_OpeningBalances";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "شاشة الأرصدة الافتتاحية والعملات الأجنبية المتعددة";
            this.grpHeader.ResumeLayout(false);
            this.grpHeader.PerformLayout();
            this.grpDetails.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvOpeningDetails)).EndInit();
            this.pnlTotals.ResumeLayout(false);
            this.pnlTotals.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private AlRowad_ERP.Controls.AlRowadSettingsToolBar settingsToolBar;
        private System.Windows.Forms.GroupBox grpHeader;
        private System.Windows.Forms.Label lblDocNo;
        private System.Windows.Forms.TextBox txtDocumentNo;
        private System.Windows.Forms.Label lblDocDate;
        private AlRowad_ERP.UI.Controls.AlRowadDateTextBox dtpDocumentDate;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.GroupBox grpDetails;
        private AlRowad_ERP.UI.Controls.AlRowadAccountsGrid dgvOpeningDetails;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_AccountId;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_AccountName;
        private System.Windows.Forms.DataGridViewComboBoxColumn Col_Currency;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_ExchangeRate;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_ForeignDebit;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_ForeignCredit;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_Debit;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_Credit;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_Memo;
        private System.Windows.Forms.Panel pnlTotals;
        private System.Windows.Forms.Label lblDebitTitle;
        private System.Windows.Forms.Label lblCreditTitle;
        private System.Windows.Forms.Label lblTotalDebitVal;
        private System.Windows.Forms.Label lblTotalCreditVal;
    }
}