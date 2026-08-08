namespace AlRowad_ERP.Forms
{
    partial class Suppliers
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
            System.Windows.Forms.Label supp_IDLabel;
            System.Windows.Forms.Label supp_NameLabel;
            System.Windows.Forms.Label supp_PhoneLabel;
            System.Windows.Forms.Label supp_AddressLabel;
            System.Windows.Forms.Label acc_IDLabel;
            System.Windows.Forms.Label label4;
            System.Windows.Forms.Label label3;
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Label label1;
            this.supp_IDTextBox = new System.Windows.Forms.TextBox();
            this.supp_NameTextBox = new System.Windows.Forms.TextBox();
            this.supp_PhoneTextBox = new System.Windows.Forms.TextBox();
            this.supp_AddressTextBox = new System.Windows.Forms.TextBox();
            this.acc_IDTextBox = new System.Windows.Forms.TextBox();
            this.alRowadToolBar1 = new AlRowad_ERP.Controls.AlRowadToolBar();
            this.GrBox_currencies = new System.Windows.Forms.GroupBox();
            this.dgv_currencies = new System.Windows.Forms.DataGridView();
            this.Is_Active = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Cur_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Is_Default = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Is_Frozen = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Cur_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cmb_parent_ID = new System.Windows.Forms.ComboBox();
            this.txt_UpdatedAt = new System.Windows.Forms.TextBox();
            this.txt_UpdatedBy = new System.Windows.Forms.TextBox();
            this.txt_CreatedBy = new System.Windows.Forms.TextBox();
            this.txt_CreatedAt = new System.Windows.Forms.TextBox();
            supp_IDLabel = new System.Windows.Forms.Label();
            supp_NameLabel = new System.Windows.Forms.Label();
            supp_PhoneLabel = new System.Windows.Forms.Label();
            supp_AddressLabel = new System.Windows.Forms.Label();
            acc_IDLabel = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            this.GrBox_currencies.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_currencies)).BeginInit();
            this.SuspendLayout();
            // 
            // supp_IDLabel
            // 
            supp_IDLabel.AutoSize = true;
            supp_IDLabel.Location = new System.Drawing.Point(395, 152);
            supp_IDLabel.Name = "supp_IDLabel";
            supp_IDLabel.Size = new System.Drawing.Size(89, 25);
            supp_IDLabel.TabIndex = 2;
            supp_IDLabel.Text = "Supp ID:";
            // 
            // supp_NameLabel
            // 
            supp_NameLabel.AutoSize = true;
            supp_NameLabel.Location = new System.Drawing.Point(395, 188);
            supp_NameLabel.Name = "supp_NameLabel";
            supp_NameLabel.Size = new System.Drawing.Size(122, 25);
            supp_NameLabel.TabIndex = 4;
            supp_NameLabel.Text = "Supp Name:";
            // 
            // supp_PhoneLabel
            // 
            supp_PhoneLabel.AutoSize = true;
            supp_PhoneLabel.Location = new System.Drawing.Point(395, 224);
            supp_PhoneLabel.Name = "supp_PhoneLabel";
            supp_PhoneLabel.Size = new System.Drawing.Size(127, 25);
            supp_PhoneLabel.TabIndex = 6;
            supp_PhoneLabel.Text = "Supp Phone:";
            // 
            // supp_AddressLabel
            // 
            supp_AddressLabel.AutoSize = true;
            supp_AddressLabel.Location = new System.Drawing.Point(395, 260);
            supp_AddressLabel.Name = "supp_AddressLabel";
            supp_AddressLabel.Size = new System.Drawing.Size(143, 25);
            supp_AddressLabel.TabIndex = 8;
            supp_AddressLabel.Text = "Supp Address:";
            // 
            // acc_IDLabel
            // 
            acc_IDLabel.AutoSize = true;
            acc_IDLabel.Location = new System.Drawing.Point(395, 296);
            acc_IDLabel.Name = "acc_IDLabel";
            acc_IDLabel.Size = new System.Drawing.Size(76, 25);
            acc_IDLabel.TabIndex = 10;
            acc_IDLabel.Text = "Acc ID:";
            // 
            // supp_IDTextBox
            // 
            this.supp_IDTextBox.Location = new System.Drawing.Point(544, 149);
            this.supp_IDTextBox.Name = "supp_IDTextBox";
            this.supp_IDTextBox.Size = new System.Drawing.Size(311, 30);
            this.supp_IDTextBox.TabIndex = 3;
            // 
            // supp_NameTextBox
            // 
            this.supp_NameTextBox.Location = new System.Drawing.Point(544, 185);
            this.supp_NameTextBox.Name = "supp_NameTextBox";
            this.supp_NameTextBox.Size = new System.Drawing.Size(311, 30);
            this.supp_NameTextBox.TabIndex = 5;
            // 
            // supp_PhoneTextBox
            // 
            this.supp_PhoneTextBox.Location = new System.Drawing.Point(544, 221);
            this.supp_PhoneTextBox.Name = "supp_PhoneTextBox";
            this.supp_PhoneTextBox.Size = new System.Drawing.Size(311, 30);
            this.supp_PhoneTextBox.TabIndex = 7;
            // 
            // supp_AddressTextBox
            // 
            this.supp_AddressTextBox.Location = new System.Drawing.Point(544, 257);
            this.supp_AddressTextBox.Name = "supp_AddressTextBox";
            this.supp_AddressTextBox.Size = new System.Drawing.Size(311, 30);
            this.supp_AddressTextBox.TabIndex = 9;
            // 
            // acc_IDTextBox
            // 
            this.acc_IDTextBox.Location = new System.Drawing.Point(544, 293);
            this.acc_IDTextBox.Name = "acc_IDTextBox";
            this.acc_IDTextBox.Size = new System.Drawing.Size(311, 30);
            this.acc_IDTextBox.TabIndex = 11;
            // 
            // alRowadToolBar1
            // 
            this.alRowadToolBar1.BackColor = System.Drawing.Color.Transparent;
            this.alRowadToolBar1.Cursor = System.Windows.Forms.Cursors.PanNW;
            this.alRowadToolBar1.Location = new System.Drawing.Point(27, 23);
            this.alRowadToolBar1.Name = "alRowadToolBar1";
            this.alRowadToolBar1.Size = new System.Drawing.Size(1014, 53);
            this.alRowadToolBar1.TabIndex = 12;
            // 
            // GrBox_currencies
            // 
            this.GrBox_currencies.BackColor = System.Drawing.SystemColors.HighlightText;
            this.GrBox_currencies.Controls.Add(this.dgv_currencies);
            this.GrBox_currencies.Location = new System.Drawing.Point(79, 340);
            this.GrBox_currencies.Name = "GrBox_currencies";
            this.GrBox_currencies.Size = new System.Drawing.Size(881, 240);
            this.GrBox_currencies.TabIndex = 45;
            this.GrBox_currencies.TabStop = false;
            this.GrBox_currencies.Text = "العملات";
            // 
            // dgv_currencies
            // 
            this.dgv_currencies.AllowUserToAddRows = false;
            this.dgv_currencies.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_currencies.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Is_Active,
            this.Cur_Name,
            this.Is_Default,
            this.Is_Frozen,
            this.Cur_ID});
            this.dgv_currencies.Location = new System.Drawing.Point(255, 27);
            this.dgv_currencies.Name = "dgv_currencies";
            this.dgv_currencies.RowHeadersVisible = false;
            this.dgv_currencies.RowHeadersWidth = 62;
            this.dgv_currencies.RowTemplate.Height = 29;
            this.dgv_currencies.Size = new System.Drawing.Size(607, 207);
            this.dgv_currencies.TabIndex = 1;
            // 
            // Is_Active
            // 
            this.Is_Active.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Is_Active.HeaderText = "▣";
            this.Is_Active.MinimumWidth = 8;
            this.Is_Active.Name = "Is_Active";
            this.Is_Active.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Is_Active.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.Is_Active.Width = 70;
            // 
            // Cur_Name
            // 
            this.Cur_Name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Cur_Name.HeaderText = "اسم العملة";
            this.Cur_Name.MinimumWidth = 8;
            this.Cur_Name.Name = "Cur_Name";
            // 
            // Is_Default
            // 
            this.Is_Default.HeaderText = "العملة الافتراضية ";
            this.Is_Default.MinimumWidth = 8;
            this.Is_Default.Name = "Is_Default";
            this.Is_Default.Width = 150;
            // 
            // Is_Frozen
            // 
            this.Is_Frozen.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Is_Frozen.HeaderText = "توقيف";
            this.Is_Frozen.MinimumWidth = 8;
            this.Is_Frozen.Name = "Is_Frozen";
            this.Is_Frozen.Width = 70;
            // 
            // Cur_ID
            // 
            this.Cur_ID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Cur_ID.HeaderText = "رقم العملة";
            this.Cur_ID.MinimumWidth = 8;
            this.Cur_ID.Name = "Cur_ID";
            this.Cur_ID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Cur_ID.Visible = false;
            this.Cur_ID.Width = 150;
            // 
            // cmb_parent_ID
            // 
            this.cmb_parent_ID.FormattingEnabled = true;
            this.cmb_parent_ID.Location = new System.Drawing.Point(544, 110);
            this.cmb_parent_ID.Name = "cmb_parent_ID";
            this.cmb_parent_ID.Size = new System.Drawing.Size(244, 33);
            this.cmb_parent_ID.TabIndex = 46;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Cursor = System.Windows.Forms.Cursors.Default;
            label4.ForeColor = System.Drawing.Color.BlanchedAlmond;
            label4.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            label4.Location = new System.Drawing.Point(58, 693);
            label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            label4.Size = new System.Drawing.Size(107, 25);
            label4.TabIndex = 62;
            label4.Text = "تاريخ التعديل :";
            label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Cursor = System.Windows.Forms.Cursors.Default;
            label3.ForeColor = System.Drawing.Color.BlanchedAlmond;
            label3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            label3.Location = new System.Drawing.Point(565, 649);
            label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            label3.Size = new System.Drawing.Size(105, 25);
            label3.TabIndex = 61;
            label3.Text = "المستـــخــدم : ";
            label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Cursor = System.Windows.Forms.Cursors.Default;
            label2.ForeColor = System.Drawing.Color.BlanchedAlmond;
            label2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            label2.Location = new System.Drawing.Point(565, 693);
            label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            label2.Size = new System.Drawing.Size(105, 25);
            label2.TabIndex = 60;
            label2.Text = "المستـــخــدم : ";
            label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Cursor = System.Windows.Forms.Cursors.Default;
            label1.ForeColor = System.Drawing.Color.BlanchedAlmond;
            label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            label1.Location = new System.Drawing.Point(58, 649);
            label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            label1.Size = new System.Drawing.Size(108, 25);
            label1.TabIndex = 59;
            label1.Text = "تاريخ الانشاء :";
            label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txt_UpdatedAt
            // 
            this.txt_UpdatedAt.Location = new System.Drawing.Point(202, 690);
            this.txt_UpdatedAt.Name = "txt_UpdatedAt";
            this.txt_UpdatedAt.Size = new System.Drawing.Size(336, 30);
            this.txt_UpdatedAt.TabIndex = 58;
            // 
            // txt_UpdatedBy
            // 
            this.txt_UpdatedBy.Location = new System.Drawing.Point(680, 690);
            this.txt_UpdatedBy.Name = "txt_UpdatedBy";
            this.txt_UpdatedBy.Size = new System.Drawing.Size(336, 30);
            this.txt_UpdatedBy.TabIndex = 57;
            // 
            // txt_CreatedBy
            // 
            this.txt_CreatedBy.Location = new System.Drawing.Point(680, 646);
            this.txt_CreatedBy.Name = "txt_CreatedBy";
            this.txt_CreatedBy.Size = new System.Drawing.Size(336, 30);
            this.txt_CreatedBy.TabIndex = 56;
            // 
            // txt_CreatedAt
            // 
            this.txt_CreatedAt.Location = new System.Drawing.Point(202, 646);
            this.txt_CreatedAt.Name = "txt_CreatedAt";
            this.txt_CreatedAt.Size = new System.Drawing.Size(336, 30);
            this.txt_CreatedAt.TabIndex = 55;
            // 
            // Suppliers
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.MediumBlue;
            this.ClientSize = new System.Drawing.Size(1067, 744);
            this.Controls.Add(label4);
            this.Controls.Add(label3);
            this.Controls.Add(label2);
            this.Controls.Add(label1);
            this.Controls.Add(this.txt_UpdatedAt);
            this.Controls.Add(this.txt_UpdatedBy);
            this.Controls.Add(this.txt_CreatedBy);
            this.Controls.Add(this.txt_CreatedAt);
            this.Controls.Add(this.cmb_parent_ID);
            this.Controls.Add(this.GrBox_currencies);
            this.Controls.Add(this.alRowadToolBar1);
            this.Controls.Add(supp_IDLabel);
            this.Controls.Add(this.supp_IDTextBox);
            this.Controls.Add(supp_NameLabel);
            this.Controls.Add(this.supp_NameTextBox);
            this.Controls.Add(supp_PhoneLabel);
            this.Controls.Add(this.supp_PhoneTextBox);
            this.Controls.Add(supp_AddressLabel);
            this.Controls.Add(this.supp_AddressTextBox);
            this.Controls.Add(acc_IDLabel);
            this.Controls.Add(this.acc_IDTextBox);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Suppliers";
            this.Text = "بيانات الموردين";
            this.Load += new System.EventHandler(this.Suppliers_Load);
            this.GrBox_currencies.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_currencies)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox supp_IDTextBox;
        private System.Windows.Forms.TextBox supp_NameTextBox;
        private System.Windows.Forms.TextBox supp_PhoneTextBox;
        private System.Windows.Forms.TextBox supp_AddressTextBox;
        private System.Windows.Forms.TextBox acc_IDTextBox;
        private Controls.AlRowadToolBar alRowadToolBar1;
        private System.Windows.Forms.GroupBox GrBox_currencies;
        private System.Windows.Forms.DataGridView dgv_currencies;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Is_Active;
        private System.Windows.Forms.DataGridViewTextBoxColumn Cur_Name;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Is_Default;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Is_Frozen;
        private System.Windows.Forms.DataGridViewTextBoxColumn Cur_ID;
        private System.Windows.Forms.ComboBox cmb_parent_ID;
        private System.Windows.Forms.TextBox txt_UpdatedAt;
        private System.Windows.Forms.TextBox txt_UpdatedBy;
        private System.Windows.Forms.TextBox txt_CreatedBy;
        private System.Windows.Forms.TextBox txt_CreatedAt;
    }
}