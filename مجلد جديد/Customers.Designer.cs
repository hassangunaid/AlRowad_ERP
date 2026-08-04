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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.Label acc_IDLabel;
            System.Windows.Forms.Label cust_AddressLabel;
            System.Windows.Forms.Label cust_PhoneLabel;
            System.Windows.Forms.Label cust_NameLabel;
            System.Windows.Forms.Label cust_IDLabel;
            System.Windows.Forms.Label parent_IDLabel;
            this.alRowad_ERPDataSet = new AlRowad_ERP.AlRowad_ERPDataSet();
            this.customersBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.customersTableAdapter = new AlRowad_ERP.AlRowad_ERPDataSetTableAdapters.CustomersTableAdapter();
            this.tableAdapterManager = new AlRowad_ERP.AlRowad_ERPDataSetTableAdapters.TableAdapterManager();
            this.accountsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.accountsTableAdapter = new AlRowad_ERP.AlRowad_ERPDataSetTableAdapters.AccountsTableAdapter();
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
            acc_IDLabel = new System.Windows.Forms.Label();
            cust_AddressLabel = new System.Windows.Forms.Label();
            cust_PhoneLabel = new System.Windows.Forms.Label();
            cust_NameLabel = new System.Windows.Forms.Label();
            cust_IDLabel = new System.Windows.Forms.Label();
            parent_IDLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.alRowad_ERPDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.customersBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.accountsBindingSource)).BeginInit();
            this.GrBox_currencies.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_currencies)).BeginInit();
            this.SuspendLayout();
            // 
            // acc_IDLabel
            // 
            acc_IDLabel.AutoSize = true;
            acc_IDLabel.Location = new System.Drawing.Point(317, 297);
            acc_IDLabel.Name = "acc_IDLabel";
            acc_IDLabel.Size = new System.Drawing.Size(124, 25);
            acc_IDLabel.TabIndex = 10;
            acc_IDLabel.Text = "رقم حساب العميل";
            // 
            // cust_AddressLabel
            // 
            cust_AddressLabel.AutoSize = true;
            cust_AddressLabel.Location = new System.Drawing.Point(317, 261);
            cust_AddressLabel.Name = "cust_AddressLabel";
            cust_AddressLabel.Size = new System.Drawing.Size(100, 25);
            cust_AddressLabel.TabIndex = 8;
            cust_AddressLabel.Text = "عنوان العميل ";
            // 
            // cust_PhoneLabel
            // 
            cust_PhoneLabel.AutoSize = true;
            cust_PhoneLabel.Location = new System.Drawing.Point(317, 225);
            cust_PhoneLabel.Name = "cust_PhoneLabel";
            cust_PhoneLabel.Size = new System.Drawing.Size(72, 25);
            cust_PhoneLabel.TabIndex = 6;
            cust_PhoneLabel.Text = "رقم تلفون";
            // 
            // cust_NameLabel
            // 
            cust_NameLabel.AutoSize = true;
            cust_NameLabel.Location = new System.Drawing.Point(317, 189);
            cust_NameLabel.Name = "cust_NameLabel";
            cust_NameLabel.Size = new System.Drawing.Size(78, 25);
            cust_NameLabel.TabIndex = 4;
            cust_NameLabel.Text = "اسم العيمل";
            // 
            // cust_IDLabel
            // 
            cust_IDLabel.AutoSize = true;
            cust_IDLabel.Location = new System.Drawing.Point(317, 153);
            cust_IDLabel.Name = "cust_IDLabel";
            cust_IDLabel.Size = new System.Drawing.Size(78, 25);
            cust_IDLabel.TabIndex = 2;
            cust_IDLabel.Text = "رقم العميل";
            // 
            // parent_IDLabel
            // 
            parent_IDLabel.AutoSize = true;
            parent_IDLabel.Location = new System.Drawing.Point(317, 115);
            parent_IDLabel.Name = "parent_IDLabel";
            parent_IDLabel.Size = new System.Drawing.Size(99, 25);
            parent_IDLabel.TabIndex = 21;
            parent_IDLabel.Text = "Parent ID:";
            // 
            // alRowad_ERPDataSet
            // 
            this.alRowad_ERPDataSet.DataSetName = "AlRowad_ERPDataSet";
            this.alRowad_ERPDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // customersBindingSource
            // 
            this.customersBindingSource.DataMember = "Customers";
            this.customersBindingSource.DataSource = this.alRowad_ERPDataSet;
            // 
            // customersTableAdapter
            // 
            this.customersTableAdapter.ClearBeforeFill = true;
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
            this.tableAdapterManager.CustomersTableAdapter = this.customersTableAdapter;
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
            this.tableAdapterManager.SuppliersTableAdapter = null;
            this.tableAdapterManager.System_ShortcutsTableAdapter = null;
            this.tableAdapterManager.UnitsTableAdapter = null;
            this.tableAdapterManager.UpdateOrder = AlRowad_ERP.AlRowad_ERPDataSetTableAdapters.TableAdapterManager.UpdateOrderOption.InsertUpdateDelete;
            // 
            // accountsBindingSource
            // 
            this.accountsBindingSource.DataMember = "Accounts";
            this.accountsBindingSource.DataSource = this.alRowad_ERPDataSet;
            // 
            // accountsTableAdapter
            // 
            this.accountsTableAdapter.ClearBeforeFill = true;
            // 
            // acc_ID
            // 
            this.acc_ID.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.customersBindingSource, "Acc_ID", true));
            this.acc_ID.Location = new System.Drawing.Point(475, 294);
            this.acc_ID.Name = "acc_ID";
            this.acc_ID.Size = new System.Drawing.Size(406, 30);
            this.acc_ID.TabIndex = 11;
            // 
            // cust_Address
            // 
            this.cust_Address.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.customersBindingSource, "Cust_Address", true));
            this.cust_Address.Location = new System.Drawing.Point(475, 258);
            this.cust_Address.Name = "cust_Address";
            this.cust_Address.Size = new System.Drawing.Size(406, 30);
            this.cust_Address.TabIndex = 9;
            // 
            // cust_Phone
            // 
            this.cust_Phone.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.customersBindingSource, "Cust_Phone", true));
            this.cust_Phone.Location = new System.Drawing.Point(475, 222);
            this.cust_Phone.Name = "cust_Phone";
            this.cust_Phone.Size = new System.Drawing.Size(406, 30);
            this.cust_Phone.TabIndex = 7;
            // 
            // cust_Name
            // 
            this.cust_Name.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.customersBindingSource, "Cust_Name", true));
            this.cust_Name.Location = new System.Drawing.Point(475, 186);
            this.cust_Name.Name = "cust_Name";
            this.cust_Name.Size = new System.Drawing.Size(406, 30);
            this.cust_Name.TabIndex = 5;
            // 
            // cust_ID
            // 
            this.cust_ID.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.customersBindingSource, "Cust_ID", true));
            this.cust_ID.Location = new System.Drawing.Point(475, 150);
            this.cust_ID.MaxLength = 20;
            this.cust_ID.Name = "cust_ID";
            this.cust_ID.Size = new System.Drawing.Size(406, 30);
            this.cust_ID.TabIndex = 3;
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
            this.cmb_parent_ID.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.accountsBindingSource, "Parent_ID", true));
            this.cmb_parent_ID.FormattingEnabled = true;
            this.cmb_parent_ID.Location = new System.Drawing.Point(475, 111);
            this.cmb_parent_ID.Name = "cmb_parent_ID";
            this.cmb_parent_ID.Size = new System.Drawing.Size(244, 33);
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
            // Customers
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1067, 600);
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
            ((System.ComponentModel.ISupportInitialize)(this.alRowad_ERPDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.customersBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.accountsBindingSource)).EndInit();
            this.GrBox_currencies.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_currencies)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private AlRowad_ERPDataSet alRowad_ERPDataSet;
        private System.Windows.Forms.BindingSource customersBindingSource;
        private AlRowad_ERPDataSetTableAdapters.CustomersTableAdapter customersTableAdapter;
        private AlRowad_ERPDataSetTableAdapters.TableAdapterManager tableAdapterManager;
        private System.Windows.Forms.BindingSource accountsBindingSource;
        private AlRowad_ERPDataSetTableAdapters.AccountsTableAdapter accountsTableAdapter;
        private System.Windows.Forms.TextBox acc_ID;
        private System.Windows.Forms.TextBox cust_Address;
        private System.Windows.Forms.TextBox cust_Phone;
        private System.Windows.Forms.TextBox cust_Name;
        private System.Windows.Forms.TextBox cust_ID;
        private Controls.AlRowadToolBar alRowadToolBar;
        private System.Windows.Forms.ComboBox cmb_parent_ID;
        private System.Windows.Forms.GroupBox GrBox_currencies;
        private System.Windows.Forms.DataGridView dgv_currencies;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Is_Active;
        private System.Windows.Forms.DataGridViewTextBoxColumn Cur_Name;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Is_Default;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Is_Frozen;
        private System.Windows.Forms.DataGridViewTextBoxColumn Cur_ID;
    }
}