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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.Label supp_IDLabel;
            System.Windows.Forms.Label supp_NameLabel;
            System.Windows.Forms.Label supp_PhoneLabel;
            System.Windows.Forms.Label supp_AddressLabel;
            System.Windows.Forms.Label acc_IDLabel;
            this.alRowad_ERPDataSet = new AlRowad_ERP.AlRowad_ERPDataSet();
            this.suppliersBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.suppliersTableAdapter = new AlRowad_ERP.AlRowad_ERPDataSetTableAdapters.SuppliersTableAdapter();
            this.tableAdapterManager = new AlRowad_ERP.AlRowad_ERPDataSetTableAdapters.TableAdapterManager();
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
            supp_IDLabel = new System.Windows.Forms.Label();
            supp_NameLabel = new System.Windows.Forms.Label();
            supp_PhoneLabel = new System.Windows.Forms.Label();
            supp_AddressLabel = new System.Windows.Forms.Label();
            acc_IDLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.alRowad_ERPDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.suppliersBindingSource)).BeginInit();
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
            // alRowad_ERPDataSet
            // 
            this.alRowad_ERPDataSet.DataSetName = "AlRowad_ERPDataSet";
            this.alRowad_ERPDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // suppliersBindingSource
            // 
            this.suppliersBindingSource.DataMember = "Suppliers";
            this.suppliersBindingSource.DataSource = this.alRowad_ERPDataSet;
            // 
            // suppliersTableAdapter
            // 
            this.suppliersTableAdapter.ClearBeforeFill = true;
            // 
            // tableAdapterManager
            // 
            this.tableAdapterManager.Account_Allowed_CurrenciesTableAdapter = null;
            this.tableAdapterManager.Account_NaturesTableAdapter = null;
            this.tableAdapterManager.Account_ReportsTableAdapter = null;
            this.tableAdapterManager.Account_TypesTableAdapter = null;
            this.tableAdapterManager.AccountsTableAdapter = null;
            this.tableAdapterManager.BackupDataSetBeforeUpdate = false;
            this.tableAdapterManager.CurrenciesTableAdapter = null;
            this.tableAdapterManager.CustomersTableAdapter = null;
            this.tableAdapterManager.Doc_TypesTableAdapter = null;
            this.tableAdapterManager.Invoice_DetailsTableAdapter = null;
            this.tableAdapterManager.Invoice_HeaderTableAdapter = null;
            this.tableAdapterManager.Item_BalancesTableAdapter = null;
            this.tableAdapterManager.ItemsTableAdapter = null;
            this.tableAdapterManager.Journal_DetailsTableAdapter = null;
            this.tableAdapterManager.Journal_HeaderTableAdapter = null;
            this.tableAdapterManager.Payment_MethodsTableAdapter = null;
            this.tableAdapterManager.Payment_VouchersTableAdapter = null;
            this.tableAdapterManager.StoresTableAdapter = null;
            this.tableAdapterManager.SuppliersTableAdapter = this.suppliersTableAdapter;
            this.tableAdapterManager.System_ShortcutsTableAdapter = null;
            this.tableAdapterManager.UnitsTableAdapter = null;
            this.tableAdapterManager.UpdateOrder = AlRowad_ERP.AlRowad_ERPDataSetTableAdapters.TableAdapterManager.UpdateOrderOption.InsertUpdateDelete;
            // 
            // supp_IDTextBox
            // 
            this.supp_IDTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.suppliersBindingSource, "Supp_ID", true));
            this.supp_IDTextBox.Location = new System.Drawing.Point(544, 149);
            this.supp_IDTextBox.Name = "supp_IDTextBox";
            this.supp_IDTextBox.Size = new System.Drawing.Size(311, 30);
            this.supp_IDTextBox.TabIndex = 3;
            // 
            // supp_NameTextBox
            // 
            this.supp_NameTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.suppliersBindingSource, "Supp_Name", true));
            this.supp_NameTextBox.Location = new System.Drawing.Point(544, 185);
            this.supp_NameTextBox.Name = "supp_NameTextBox";
            this.supp_NameTextBox.Size = new System.Drawing.Size(311, 30);
            this.supp_NameTextBox.TabIndex = 5;
            // 
            // supp_PhoneTextBox
            // 
            this.supp_PhoneTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.suppliersBindingSource, "Supp_Phone", true));
            this.supp_PhoneTextBox.Location = new System.Drawing.Point(544, 221);
            this.supp_PhoneTextBox.Name = "supp_PhoneTextBox";
            this.supp_PhoneTextBox.Size = new System.Drawing.Size(311, 30);
            this.supp_PhoneTextBox.TabIndex = 7;
            // 
            // supp_AddressTextBox
            // 
            this.supp_AddressTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.suppliersBindingSource, "Supp_Address", true));
            this.supp_AddressTextBox.Location = new System.Drawing.Point(544, 257);
            this.supp_AddressTextBox.Name = "supp_AddressTextBox";
            this.supp_AddressTextBox.Size = new System.Drawing.Size(311, 30);
            this.supp_AddressTextBox.TabIndex = 9;
            // 
            // acc_IDTextBox
            // 
            this.acc_IDTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.suppliersBindingSource, "Acc_ID", true));
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
            // Suppliers
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1067, 592);
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
            ((System.ComponentModel.ISupportInitialize)(this.alRowad_ERPDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.suppliersBindingSource)).EndInit();
            this.GrBox_currencies.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_currencies)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private AlRowad_ERPDataSet alRowad_ERPDataSet;
        private System.Windows.Forms.BindingSource suppliersBindingSource;
        private AlRowad_ERPDataSetTableAdapters.SuppliersTableAdapter suppliersTableAdapter;
        private AlRowad_ERPDataSetTableAdapters.TableAdapterManager tableAdapterManager;
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
    }
}