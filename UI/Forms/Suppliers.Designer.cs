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
            System.Windows.Forms.Label label5;
            this.txt_Supp_ID = new System.Windows.Forms.TextBox();
            this.txt_Supp_Name = new System.Windows.Forms.TextBox();
            this.txt_Supp_Phone = new System.Windows.Forms.TextBox();
            this.txt_Supp_Address = new System.Windows.Forms.TextBox();
            this.txt_Acc_ID = new System.Windows.Forms.TextBox();
            this.alRowadToolBar1 = new AlRowad_ERP.Controls.AlRowadToolBar();
            this.GrBox_currencies = new System.Windows.Forms.GroupBox();
            this.dgv_currencies = new System.Windows.Forms.DataGridView();
            this.Is_Active = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Cur_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Is_Default = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Is_Frozen = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Cur_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cmb_parent_ID = new System.Windows.Forms.ComboBox();
            this.txt_Updated_At = new System.Windows.Forms.TextBox();
            this.txt_UpdatedByName = new System.Windows.Forms.TextBox();
            this.txt_CreatedByName = new System.Windows.Forms.TextBox();
            this.txt_Created_At = new System.Windows.Forms.TextBox();
            this.chk_Is_Farmer = new System.Windows.Forms.CheckBox();
            supp_IDLabel = new System.Windows.Forms.Label();
            supp_NameLabel = new System.Windows.Forms.Label();
            supp_PhoneLabel = new System.Windows.Forms.Label();
            supp_AddressLabel = new System.Windows.Forms.Label();
            acc_IDLabel = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            label5 = new System.Windows.Forms.Label();
            this.GrBox_currencies.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_currencies)).BeginInit();
            this.SuspendLayout();
            // 
            // supp_IDLabel
            // 
            supp_IDLabel.AutoSize = true;
            supp_IDLabel.ForeColor = System.Drawing.SystemColors.ButtonFace;
            supp_IDLabel.Location = new System.Drawing.Point(145, 162);
            supp_IDLabel.Name = "supp_IDLabel";
            supp_IDLabel.Size = new System.Drawing.Size(80, 25);
            supp_IDLabel.TabIndex = 2;
            supp_IDLabel.Text = "رقم المورد";
            // 
            // supp_NameLabel
            // 
            supp_NameLabel.AutoSize = true;
            supp_NameLabel.ForeColor = System.Drawing.SystemColors.ButtonFace;
            supp_NameLabel.Location = new System.Drawing.Point(145, 206);
            supp_NameLabel.Name = "supp_NameLabel";
            supp_NameLabel.Size = new System.Drawing.Size(80, 25);
            supp_NameLabel.TabIndex = 4;
            supp_NameLabel.Text = "اسم المورد";
            // 
            // supp_PhoneLabel
            // 
            supp_PhoneLabel.AutoSize = true;
            supp_PhoneLabel.ForeColor = System.Drawing.SystemColors.ButtonFace;
            supp_PhoneLabel.Location = new System.Drawing.Point(145, 252);
            supp_PhoneLabel.Name = "supp_PhoneLabel";
            supp_PhoneLabel.Size = new System.Drawing.Size(80, 25);
            supp_PhoneLabel.TabIndex = 6;
            supp_PhoneLabel.Text = "رقم التلفون";
            // 
            // supp_AddressLabel
            // 
            supp_AddressLabel.AutoSize = true;
            supp_AddressLabel.ForeColor = System.Drawing.SystemColors.ButtonFace;
            supp_AddressLabel.Location = new System.Drawing.Point(145, 293);
            supp_AddressLabel.Name = "supp_AddressLabel";
            supp_AddressLabel.Size = new System.Drawing.Size(56, 25);
            supp_AddressLabel.TabIndex = 8;
            supp_AddressLabel.Text = "العنوان";
            // 
            // acc_IDLabel
            // 
            acc_IDLabel.AutoSize = true;
            acc_IDLabel.ForeColor = System.Drawing.SystemColors.ButtonFace;
            acc_IDLabel.Location = new System.Drawing.Point(462, 162);
            acc_IDLabel.Name = "acc_IDLabel";
            acc_IDLabel.Size = new System.Drawing.Size(88, 25);
            acc_IDLabel.TabIndex = 10;
            acc_IDLabel.Text = "رقم الحساب";
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
            // label5
            // 
            label5.AutoSize = true;
            label5.ForeColor = System.Drawing.SystemColors.ButtonFace;
            label5.Location = new System.Drawing.Point(145, 118);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(87, 25);
            label5.TabIndex = 65;
            label5.Text = "حساب الاب";
            // 
            // txt_Supp_ID
            // 
            this.txt_Supp_ID.Location = new System.Drawing.Point(250, 162);
            this.txt_Supp_ID.Name = "txt_Supp_ID";
            this.txt_Supp_ID.Size = new System.Drawing.Size(96, 30);
            this.txt_Supp_ID.TabIndex = 3;
            // 
            // txt_Supp_Name
            // 
            this.txt_Supp_Name.Location = new System.Drawing.Point(250, 203);
            this.txt_Supp_Name.Name = "txt_Supp_Name";
            this.txt_Supp_Name.Size = new System.Drawing.Size(311, 30);
            this.txt_Supp_Name.TabIndex = 5;
            // 
            // txt_Supp_Phone
            // 
            this.txt_Supp_Phone.Location = new System.Drawing.Point(250, 247);
            this.txt_Supp_Phone.Name = "txt_Supp_Phone";
            this.txt_Supp_Phone.Size = new System.Drawing.Size(180, 30);
            this.txt_Supp_Phone.TabIndex = 7;
            // 
            // txt_Supp_Address
            // 
            this.txt_Supp_Address.Location = new System.Drawing.Point(250, 290);
            this.txt_Supp_Address.Name = "txt_Supp_Address";
            this.txt_Supp_Address.Size = new System.Drawing.Size(392, 30);
            this.txt_Supp_Address.TabIndex = 9;
            // 
            // txt_Acc_ID
            // 
            this.txt_Acc_ID.Location = new System.Drawing.Point(557, 159);
            this.txt_Acc_ID.Name = "txt_Acc_ID";
            this.txt_Acc_ID.Size = new System.Drawing.Size(311, 30);
            this.txt_Acc_ID.TabIndex = 11;
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
            this.cmb_parent_ID.Location = new System.Drawing.Point(250, 115);
            this.cmb_parent_ID.Name = "cmb_parent_ID";
            this.cmb_parent_ID.Size = new System.Drawing.Size(160, 33);
            this.cmb_parent_ID.TabIndex = 46;
            // 
            // txt_Updated_At
            // 
            this.txt_Updated_At.Location = new System.Drawing.Point(202, 690);
            this.txt_Updated_At.Name = "txt_Updated_At";
            this.txt_Updated_At.Size = new System.Drawing.Size(336, 30);
            this.txt_Updated_At.TabIndex = 58;
            // 
            // txt_UpdatedByName
            // 
            this.txt_UpdatedByName.Location = new System.Drawing.Point(680, 690);
            this.txt_UpdatedByName.Name = "txt_UpdatedByName";
            this.txt_UpdatedByName.Size = new System.Drawing.Size(336, 30);
            this.txt_UpdatedByName.TabIndex = 57;
            // 
            // txt_CreatedByName
            // 
            this.txt_CreatedByName.Location = new System.Drawing.Point(680, 646);
            this.txt_CreatedByName.Name = "txt_CreatedByName";
            this.txt_CreatedByName.Size = new System.Drawing.Size(336, 30);
            this.txt_CreatedByName.TabIndex = 56;
            // 
            // txt_Created_At
            // 
            this.txt_Created_At.Location = new System.Drawing.Point(202, 646);
            this.txt_Created_At.Name = "txt_Created_At";
            this.txt_Created_At.Size = new System.Drawing.Size(336, 30);
            this.txt_Created_At.TabIndex = 55;
            // 
            // chk_Is_Farmer
            // 
            this.chk_Is_Farmer.AutoSize = true;
            this.chk_Is_Farmer.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.chk_Is_Farmer.Location = new System.Drawing.Point(833, 291);
            this.chk_Is_Farmer.Name = "chk_Is_Farmer";
            this.chk_Is_Farmer.Size = new System.Drawing.Size(127, 29);
            this.chk_Is_Farmer.TabIndex = 64;
            this.chk_Is_Farmer.Tag = "Is_Farmer";
            this.chk_Is_Farmer.Text = "حساب مزارع";
            this.chk_Is_Farmer.UseVisualStyleBackColor = true;
            // 
            // Suppliers
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.MediumBlue;
            this.ClientSize = new System.Drawing.Size(1067, 744);
            this.Controls.Add(label5);
            this.Controls.Add(this.chk_Is_Farmer);
            this.Controls.Add(label4);
            this.Controls.Add(label3);
            this.Controls.Add(label2);
            this.Controls.Add(label1);
            this.Controls.Add(this.txt_Updated_At);
            this.Controls.Add(this.txt_UpdatedByName);
            this.Controls.Add(this.txt_CreatedByName);
            this.Controls.Add(this.txt_Created_At);
            this.Controls.Add(this.cmb_parent_ID);
            this.Controls.Add(this.GrBox_currencies);
            this.Controls.Add(this.alRowadToolBar1);
            this.Controls.Add(supp_IDLabel);
            this.Controls.Add(this.txt_Supp_ID);
            this.Controls.Add(supp_NameLabel);
            this.Controls.Add(this.txt_Supp_Name);
            this.Controls.Add(supp_PhoneLabel);
            this.Controls.Add(this.txt_Supp_Phone);
            this.Controls.Add(supp_AddressLabel);
            this.Controls.Add(this.txt_Supp_Address);
            this.Controls.Add(acc_IDLabel);
            this.Controls.Add(this.txt_Acc_ID);
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
        private System.Windows.Forms.TextBox txt_Supp_ID;
        private System.Windows.Forms.TextBox txt_Supp_Name;
        private System.Windows.Forms.TextBox txt_Supp_Phone;
        private System.Windows.Forms.TextBox txt_Supp_Address;
        private System.Windows.Forms.TextBox txt_Acc_ID;
        private Controls.AlRowadToolBar alRowadToolBar1;
        private System.Windows.Forms.GroupBox GrBox_currencies;
        private System.Windows.Forms.DataGridView dgv_currencies;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Is_Active;
        private System.Windows.Forms.DataGridViewTextBoxColumn Cur_Name;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Is_Default;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Is_Frozen;
        private System.Windows.Forms.DataGridViewTextBoxColumn Cur_ID;
        private System.Windows.Forms.ComboBox cmb_parent_ID;
        private System.Windows.Forms.TextBox txt_Updated_At;
        private System.Windows.Forms.TextBox txt_UpdatedByName;
        private System.Windows.Forms.TextBox txt_CreatedByName;
        private System.Windows.Forms.TextBox txt_Created_At;
        private System.Windows.Forms.CheckBox chk_Is_Farmer;
    }
}