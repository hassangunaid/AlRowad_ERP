namespace AlRowad_ERP.UI.Reports
{
    partial class Frm_AccountStatement
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

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnLoadData = new System.Windows.Forms.Button();
            this.txtAccountId = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtAccountName = new System.Windows.Forms.TextBox();
            this.dtpFrom = new AlRowad_ERP.Controls.AlRowadDateTextBox();
            this.dtpTo = new AlRowad_ERP.Controls.AlRowadDateTextBox();
            this.chkIncludeUnposted = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.dgvStatement = new AlRowad_ERP.UI.Controls.AlRowadDataGridView();
            this.btnPrintOfficial = new System.Windows.Forms.Button();
            this.btnPrintGrid = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStatement)).BeginInit();
            this.SuspendLayout();
            // 
            // btnLoadData
            // 
            this.btnLoadData.Location = new System.Drawing.Point(774, 160);
            this.btnLoadData.Name = "btnLoadData";
            this.btnLoadData.Size = new System.Drawing.Size(75, 51);
            this.btnLoadData.TabIndex = 0;
            this.btnLoadData.Text = "تحديث";
            this.btnLoadData.UseVisualStyleBackColor = true;
            // 
            // txtAccountId
            // 
            this.txtAccountId.Location = new System.Drawing.Point(854, 124);
            this.txtAccountId.Name = "txtAccountId";
            this.txtAccountId.Size = new System.Drawing.Size(190, 30);
            this.txtAccountId.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1050, 124);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 25);
            this.label1.TabIndex = 2;
            this.label1.Text = "رقم الحساب";
            // 
            // txtAccountName
            // 
            this.txtAccountName.Location = new System.Drawing.Point(476, 124);
            this.txtAccountName.Name = "txtAccountName";
            this.txtAccountName.Size = new System.Drawing.Size(375, 30);
            this.txtAccountName.TabIndex = 3;
            // 
            // dtpFrom
            // 
            this.dtpFrom.DateValue = new System.DateTime(2026, 10, 9, 0, 0, 0, 0);
            this.dtpFrom.Location = new System.Drawing.Point(827, 53);
            this.dtpFrom.MaxLength = 10;
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(150, 30);
            this.dtpFrom.TabIndex = 4;
            this.dtpFrom.Text = "09/10/2026";
            this.dtpFrom.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // dtpTo
            // 
            this.dtpTo.DateValue = new System.DateTime(2026, 10, 9, 0, 0, 0, 0);
            this.dtpTo.Location = new System.Drawing.Point(599, 53);
            this.dtpTo.MaxLength = 10;
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(164, 30);
            this.dtpTo.TabIndex = 5;
            this.dtpTo.Text = "09/10/2026";
            this.dtpTo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // chkIncludeUnposted
            // 
            this.chkIncludeUnposted.AutoSize = true;
            this.chkIncludeUnposted.Location = new System.Drawing.Point(343, 55);
            this.chkIncludeUnposted.Name = "chkIncludeUnposted";
            this.chkIncludeUnposted.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.chkIncludeUnposted.Size = new System.Drawing.Size(198, 29);
            this.chkIncludeUnposted.TabIndex = 6;
            this.chkIncludeUnposted.Text = "كشف حساب قبل الترحيل";
            this.chkIncludeUnposted.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1003, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 25);
            this.label2.TabIndex = 7;
            this.label2.Text = "من تاريخ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(769, 55);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(32, 25);
            this.label3.TabIndex = 8;
            this.label3.Text = "الى";
            // 
            // dgvStatement
            // 
            this.dgvStatement.AllowUserToAddRows = false;
            this.dgvStatement.AllowUserToDeleteRows = false;
            this.dgvStatement.AllowUserToOrderColumns = true;
            this.dgvStatement.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgvStatement.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvStatement.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Sunken;
            this.dgvStatement.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvStatement.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvStatement.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvStatement.EnableHeadersVisualStyles = false;
            this.dgvStatement.Location = new System.Drawing.Point(13, 239);
            this.dgvStatement.Name = "dgvStatement";
            this.dgvStatement.ReadOnly = true;
            this.dgvStatement.RowHeadersVisible = false;
            this.dgvStatement.RowHeadersWidth = 62;
            this.dgvStatement.RowTemplate.Height = 29;
            this.dgvStatement.Size = new System.Drawing.Size(1543, 658);
            this.dgvStatement.TabIndex = 9;
            // 
            // btnPrintOfficial
            // 
            this.btnPrintOfficial.Location = new System.Drawing.Point(726, 939);
            this.btnPrintOfficial.Name = "btnPrintOfficial";
            this.btnPrintOfficial.Size = new System.Drawing.Size(75, 51);
            this.btnPrintOfficial.TabIndex = 10;
            this.btnPrintOfficial.Text = "طباعة";
            this.btnPrintOfficial.UseVisualStyleBackColor = true;
            // 
            // btnPrintGrid
            // 
            this.btnPrintGrid.Location = new System.Drawing.Point(432, 939);
            this.btnPrintGrid.Name = "btnPrintGrid";
            this.btnPrintGrid.Size = new System.Drawing.Size(166, 51);
            this.btnPrintGrid.TabIndex = 11;
            this.btnPrintGrid.Text = "طباعة الشبكة";
            this.btnPrintGrid.UseVisualStyleBackColor = true;
            // 
            // Frm_AccountStatement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1578, 1050);
            this.Controls.Add(this.btnPrintGrid);
            this.Controls.Add(this.btnPrintOfficial);
            this.Controls.Add(this.dgvStatement);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.chkIncludeUnposted);
            this.Controls.Add(this.dtpTo);
            this.Controls.Add(this.dtpFrom);
            this.Controls.Add(this.txtAccountName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtAccountId);
            this.Controls.Add(this.btnLoadData);
            this.Name = "Frm_AccountStatement";
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.RightToLeftLayout = false;
            this.Text = "تقرير كشف حساب";
            ((System.ComponentModel.ISupportInitialize)(this.dgvStatement)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnLoadData;
        private System.Windows.Forms.TextBox txtAccountId;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtAccountName;
        private AlRowad_ERP.Controls.AlRowadDateTextBox dtpFrom;
        private AlRowad_ERP.Controls.AlRowadDateTextBox dtpTo;
        private System.Windows.Forms.CheckBox chkIncludeUnposted;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private Controls.AlRowadDataGridView dgvStatement;
        private System.Windows.Forms.Button btnPrintOfficial;
        private System.Windows.Forms.Button btnPrintGrid;
    }
}