namespace AlRowad_ERP.Forms
{
    partial class Customers
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
            System.Windows.Forms.Label acc_IDLabel;
            System.Windows.Forms.Label cust_AddressLabel;
            System.Windows.Forms.Label cust_PhoneLabel;
            System.Windows.Forms.Label cust_NameLabel;
            System.Windows.Forms.Label cust_IDLabel;
            System.Windows.Forms.Label parent_IDLabel;
            System.Windows.Forms.Label label4;
            System.Windows.Forms.Label label3;
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Label label1;
            this.acc_ID = new System.Windows.Forms.TextBox();
            this.cust_Address = new System.Windows.Forms.TextBox();
            this.cust_Phone = new System.Windows.Forms.TextBox();
            this.cust_Name = new System.Windows.Forms.TextBox();
            this.cust_ID = new System.Windows.Forms.TextBox();
            this.alRowadToolBar = new AlRowad_ERP.Controls.AlRowadToolBar();
            this.cmb_parent_ID = new System.Windows.Forms.ComboBox();
            this.GrBox_currencies = new System.Windows.Forms.GroupBox();
            this.dgv_currencies = new System.Windows.Forms.DataGridView();
            this.Is_Active = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Cur_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Is_Default = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Is_Frozen = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Cur_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txt_UpdatedAt = new System.Windows.Forms.TextBox();
            this.txt_UpdatedBy = new System.Windows.Forms.TextBox();
            this.txt_CreatedBy = new System.Windows.Forms.TextBox();
            this.txt_CreatedAt = new System.Windows.Forms.TextBox();
            acc_IDLabel = new System.Windows.Forms.Label();
            cust_AddressLabel = new System.Windows.Forms.Label();
            cust_PhoneLabel = new System.Windows.Forms.Label();
            cust_NameLabel = new System.Windows.Forms.Label();
            cust_IDLabel = new System.Windows.Forms.Label();
            parent_IDLabel = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            this.GrBox_currencies.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_currencies)).BeginInit();
            this.SuspendLayout();
            // 
            // acc_IDLabel
            // 
            acc_IDLabel.AutoSize = true;
            acc_IDLabel.ForeColor = System.Drawing.Color.Linen;
            acc_IDLabel.Location = new System.Drawing.Point(603, 148);
            acc_IDLabel.Name = "acc_IDLabel";
            acc_IDLabel.Size = new System.Drawing.Size(124, 25);
            acc_IDLabel.TabIndex = 10;
            acc_IDLabel.Text = "رقم حساب العميل";
            // 
            // cust_AddressLabel
            // 
            cust_AddressLabel.AutoSize = true;
            cust_AddressLabel.ForeColor = System.Drawing.Color.Linen;
            cust_AddressLabel.Location = new System.Drawing.Point(202, 294);
            cust_AddressLabel.Name = "cust_AddressLabel";
            cust_AddressLabel.Size = new System.Drawing.Size(100, 25);
            cust_AddressLabel.TabIndex = 8;
            cust_AddressLabel.Text = "عنوان العميل ";
            // 
            // cust_PhoneLabel
            // 
            cust_PhoneLabel.AutoSize = true;
            cust_PhoneLabel.ForeColor = System.Drawing.Color.Linen;
            cust_PhoneLabel.Location = new System.Drawing.Point(202, 258);
            cust_PhoneLabel.Name = "cust_PhoneLabel";
            cust_PhoneLabel.Size = new System.Drawing.Size(72, 25);
            cust_PhoneLabel.TabIndex = 6;
            cust_PhoneLabel.Text = "رقم تلفون";
            // 
            // cust_NameLabel
            // 
            cust_NameLabel.AutoSize = true;
            cust_NameLabel.ForeColor = System.Drawing.Color.Linen;
            cust_NameLabel.Location = new System.Drawing.Point(202, 222);
            cust_NameLabel.Name = "cust_NameLabel";
            cust_NameLabel.Size = new System.Drawing.Size(78, 25);
            cust_NameLabel.TabIndex = 4;
            cust_NameLabel.Text = "اسم العيمل";
            // 
            // cust_IDLabel
            // 
            cust_IDLabel.AutoSize = true;
            cust_IDLabel.ForeColor = System.Drawing.Color.Linen;
            cust_IDLabel.Location = new System.Drawing.Point(202, 186);
            cust_IDLabel.Name = "cust_IDLabel";
            cust_IDLabel.Size = new System.Drawing.Size(78, 25);
            cust_IDLabel.TabIndex = 2;
            cust_IDLabel.Text = "رقم العميل";
            // 
            // parent_IDLabel
            // 
            parent_IDLabel.AutoSize = true;
            parent_IDLabel.ForeColor = System.Drawing.Color.Linen;
            parent_IDLabel.Location = new System.Drawing.Point(202, 148);
            parent_IDLabel.Name = "parent_IDLabel";
            parent_IDLabel.Size = new System.Drawing.Size(87, 25);
            parent_IDLabel.TabIndex = 21;
            parent_IDLabel.Text = "حساب الاب";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Cursor = System.Windows.Forms.Cursors.Default;
            label4.ForeColor = System.Drawing.Color.BlanchedAlmond;
            label4.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            label4.Location = new System.Drawing.Point(51, 683);
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
            label3.Location = new System.Drawing.Point(558, 639);
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
            label2.Location = new System.Drawing.Point(558, 683);
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
            label1.Location = new System.Drawing.Point(51, 639);
            label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            label1.Size = new System.Drawing.Size(108, 25);
            label1.TabIndex = 59;
            label1.Text = "تاريخ الانشاء :";
            label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // acc_ID
            // 
            this.acc_ID.Location = new System.Drawing.Point(733, 143);
            this.acc_ID.Name = "acc_ID";
            this.acc_ID.Size = new System.Drawing.Size(174, 30);
            this.acc_ID.TabIndex = 11;
            this.acc_ID.Tag = "Acc_ID";
            // 
            // cust_Address
            // 
            this.cust_Address.Location = new System.Drawing.Point(360, 291);
            this.cust_Address.Name = "cust_Address";
            this.cust_Address.Size = new System.Drawing.Size(266, 30);
            this.cust_Address.TabIndex = 9;
            this.cust_Address.Tag = "Cust_Address";
            // 
            // cust_Phone
            // 
            this.cust_Phone.Location = new System.Drawing.Point(360, 255);
            this.cust_Phone.Name = "cust_Phone";
            this.cust_Phone.Size = new System.Drawing.Size(216, 30);
            this.cust_Phone.TabIndex = 7;
            this.cust_Phone.Tag = "Cust_Phone";
            // 
            // cust_Name
            // 
            this.cust_Name.Location = new System.Drawing.Point(360, 219);
            this.cust_Name.Name = "cust_Name";
            this.cust_Name.Size = new System.Drawing.Size(303, 30);
            this.cust_Name.TabIndex = 5;
            this.cust_Name.Tag = "Cust_Name";
            // 
            // cust_ID
            // 
            this.cust_ID.Location = new System.Drawing.Point(360, 183);
            this.cust_ID.MaxLength = 20;
            this.cust_ID.Name = "cust_ID";
            this.cust_ID.Size = new System.Drawing.Size(94, 30);
            this.cust_ID.TabIndex = 3;
            this.cust_ID.Tag = "Cust_ID";
            // 
            // alRowadToolBar
            // 
            this.alRowadToolBar.BackColor = System.Drawing.Color.Transparent;
            this.alRowadToolBar.Cursor = System.Windows.Forms.Cursors.PanNW;
            this.alRowadToolBar.Location = new System.Drawing.Point(31, 39);
            this.alRowadToolBar.Margin = new System.Windows.Forms.Padding(4);
            this.alRowadToolBar.Name = "alRowadToolBar";
            this.alRowadToolBar.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.alRowadToolBar.Size = new System.Drawing.Size(1003, 52);
            this.alRowadToolBar.TabIndex = 21;
            // 
            // cmb_parent_ID
            // 
            this.cmb_parent_ID.FormattingEnabled = true;
            this.cmb_parent_ID.Location = new System.Drawing.Point(360, 144);
            this.cmb_parent_ID.Name = "cmb_parent_ID";
            this.cmb_parent_ID.Size = new System.Drawing.Size(188, 33);
            this.cmb_parent_ID.TabIndex = 22;
            this.cmb_parent_ID.SelectedIndexChanged += new System.EventHandler(this.cmb_parent_ID_SelectedIndexChanged);
            // 
            // GrBox_currencies
            // 
            this.GrBox_currencies.BackColor = System.Drawing.SystemColors.HighlightText;
            this.GrBox_currencies.Controls.Add(this.dgv_currencies);
            this.GrBox_currencies.Location = new System.Drawing.Point(78, 348);
            this.GrBox_currencies.Name = "GrBox_currencies";
            this.GrBox_currencies.Size = new System.Drawing.Size(881, 240);
            this.GrBox_currencies.TabIndex = 44;
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
            this.Is_Active.DataPropertyName = "Is_Active";
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
            this.Cur_Name.DataPropertyName = "Cur_Name";
            this.Cur_Name.HeaderText = "اسم العملة";
            this.Cur_Name.MinimumWidth = 8;
            this.Cur_Name.Name = "Cur_Name";
            // 
            // Is_Default
            // 
            this.Is_Default.DataPropertyName = "Is_Default";
            this.Is_Default.HeaderText = "العملة الافتراضية ";
            this.Is_Default.MinimumWidth = 8;
            this.Is_Default.Name = "Is_Default";
            this.Is_Default.Width = 150;
            // 
            // Is_Frozen
            // 
            this.Is_Frozen.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Is_Frozen.DataPropertyName = "Is_Frozen";
            this.Is_Frozen.HeaderText = "توقيف";
            this.Is_Frozen.MinimumWidth = 8;
            this.Is_Frozen.Name = "Is_Frozen";
            this.Is_Frozen.Width = 70;
            // 
            // Cur_ID
            // 
            this.Cur_ID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Cur_ID.DataPropertyName = "Cur_ID";
            this.Cur_ID.HeaderText = "رقم العملة";
            this.Cur_ID.MinimumWidth = 8;
            this.Cur_ID.Name = "Cur_ID";
            this.Cur_ID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Cur_ID.Visible = false;
            this.Cur_ID.Width = 150;
            // 
            // txt_UpdatedAt
            // 
            this.txt_UpdatedAt.Location = new System.Drawing.Point(195, 680);
            this.txt_UpdatedAt.Name = "txt_UpdatedAt";
            this.txt_UpdatedAt.Size = new System.Drawing.Size(336, 30);
            this.txt_UpdatedAt.TabIndex = 58;
            this.txt_UpdatedAt.Tag = "Updated_At";
            // 
            // txt_UpdatedBy
            // 
            this.txt_UpdatedBy.Location = new System.Drawing.Point(673, 680);
            this.txt_UpdatedBy.Name = "txt_UpdatedBy";
            this.txt_UpdatedBy.Size = new System.Drawing.Size(336, 30);
            this.txt_UpdatedBy.TabIndex = 57;
            this.txt_UpdatedBy.Tag = "Updated_By";
            // 
            // txt_CreatedBy
            // 
            this.txt_CreatedBy.Location = new System.Drawing.Point(673, 636);
            this.txt_CreatedBy.Name = "txt_CreatedBy";
            this.txt_CreatedBy.Size = new System.Drawing.Size(336, 30);
            this.txt_CreatedBy.TabIndex = 56;
            this.txt_CreatedBy.Tag = "Created_By";
            // 
            // txt_CreatedAt
            // 
            this.txt_CreatedAt.Location = new System.Drawing.Point(195, 636);
            this.txt_CreatedAt.Name = "txt_CreatedAt";
            this.txt_CreatedAt.Size = new System.Drawing.Size(336, 30);
            this.txt_CreatedAt.TabIndex = 55;
            this.txt_CreatedAt.Tag = "Created_At";
            // 
            // Customers
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
            this.Controls.Add(this.GrBox_currencies);
            this.Controls.Add(parent_IDLabel);
            this.Controls.Add(this.cmb_parent_ID);
            this.Controls.Add(this.alRowadToolBar);
            this.Controls.Add(cust_IDLabel);
            this.Controls.Add(this.cust_ID);
            this.Controls.Add(cust_NameLabel);
            this.Controls.Add(this.cust_Name);
            this.Controls.Add(cust_PhoneLabel);
            this.Controls.Add(this.cust_Phone);
            this.Controls.Add(cust_AddressLabel);
            this.Controls.Add(this.cust_Address);
            this.Controls.Add(acc_IDLabel);
            this.Controls.Add(this.acc_ID);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Customers";
            this.Text = "نهيئة العملاء";
            this.Load += new System.EventHandler(this.Customers_Load);
            this.GrBox_currencies.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_currencies)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox acc_ID;
        private System.Windows.Forms.TextBox cust_Address;
        private System.Windows.Forms.TextBox cust_Phone;
        private System.Windows.Forms.TextBox cust_Name;
        private System.Windows.Forms.TextBox cust_ID;
        private Controls.AlRowadToolBar alRowadToolBar;
        private System.Windows.Forms.ComboBox cmb_parent_ID;
        private System.Windows.Forms.GroupBox GrBox_currencies;
        private System.Windows.Forms.DataGridView dgv_currencies;
        private System.Windows.Forms.TextBox txt_UpdatedAt;
        private System.Windows.Forms.TextBox txt_UpdatedBy;
        private System.Windows.Forms.TextBox txt_CreatedBy;
        private System.Windows.Forms.TextBox txt_CreatedAt;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Is_Active;
        private System.Windows.Forms.DataGridViewTextBoxColumn Cur_Name;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Is_Default;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Is_Frozen;
        private System.Windows.Forms.DataGridViewTextBoxColumn Cur_ID;
    }
}