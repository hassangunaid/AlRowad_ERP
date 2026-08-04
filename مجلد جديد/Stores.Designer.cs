namespace AlRowad_ERP.Forms
{
    partial class Stores
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
            System.Windows.Forms.Label store_IDLabel;
            System.Windows.Forms.Label store_NameLabel;
            System.Windows.Forms.Label store_LocationLabel;
            this.alRowad_ERPDataSet = new AlRowad_ERP.AlRowad_ERPDataSet();
            this.storesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.storesTableAdapter = new AlRowad_ERP.AlRowad_ERPDataSetTableAdapters.StoresTableAdapter();
            this.tableAdapterManager = new AlRowad_ERP.AlRowad_ERPDataSetTableAdapters.TableAdapterManager();
            this.storesDataGridView = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.store_IDTextBox = new System.Windows.Forms.TextBox();
            this.store_NameTextBox = new System.Windows.Forms.TextBox();
            this.store_LocationTextBox = new System.Windows.Forms.TextBox();
            this.item_BalancesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.item_BalancesTableAdapter = new AlRowad_ERP.AlRowad_ERPDataSetTableAdapters.Item_BalancesTableAdapter();
            store_IDLabel = new System.Windows.Forms.Label();
            store_NameLabel = new System.Windows.Forms.Label();
            store_LocationLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.alRowad_ERPDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.storesBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.storesDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.item_BalancesBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // alRowad_ERPDataSet
            // 
            this.alRowad_ERPDataSet.DataSetName = "AlRowad_ERPDataSet";
            this.alRowad_ERPDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // storesBindingSource
            // 
            this.storesBindingSource.DataMember = "Stores";
            this.storesBindingSource.DataSource = this.alRowad_ERPDataSet;
            // 
            // storesTableAdapter
            // 
            this.storesTableAdapter.ClearBeforeFill = true;
            // 
            // tableAdapterManager
            // 
            this.tableAdapterManager.AccountsTableAdapter = null;
            this.tableAdapterManager.BackupDataSetBeforeUpdate = false;
            this.tableAdapterManager.CurrenciesTableAdapter = null;
            this.tableAdapterManager.CustomersTableAdapter = null;
            this.tableAdapterManager.Doc_TypesTableAdapter = null;
            this.tableAdapterManager.Item_BalancesTableAdapter = this.item_BalancesTableAdapter;
            this.tableAdapterManager.ItemsTableAdapter = null;
            this.tableAdapterManager.Journal_HeaderTableAdapter = null;
            this.tableAdapterManager.Payment_MethodsTableAdapter = null;
            this.tableAdapterManager.StoresTableAdapter = this.storesTableAdapter;
            this.tableAdapterManager.SuppliersTableAdapter = null;
            this.tableAdapterManager.System_ShortcutsTableAdapter = null;
            this.tableAdapterManager.UnitsTableAdapter = null;
            this.tableAdapterManager.UpdateOrder = AlRowad_ERP.AlRowad_ERPDataSetTableAdapters.TableAdapterManager.UpdateOrderOption.InsertUpdateDelete;
            // 
            // storesDataGridView
            // 
            this.storesDataGridView.AutoGenerateColumns = false;
            this.storesDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.storesDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3});
            this.storesDataGridView.DataSource = this.storesBindingSource;
            this.storesDataGridView.Location = new System.Drawing.Point(171, 323);
            this.storesDataGridView.Name = "storesDataGridView";
            this.storesDataGridView.RowHeadersWidth = 62;
            this.storesDataGridView.RowTemplate.Height = 29;
            this.storesDataGridView.Size = new System.Drawing.Size(772, 220);
            this.storesDataGridView.TabIndex = 1;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "Store_ID";
            this.dataGridViewTextBoxColumn1.HeaderText = "Store_ID";
            this.dataGridViewTextBoxColumn1.MinimumWidth = 8;
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Width = 150;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "Store_Name";
            this.dataGridViewTextBoxColumn2.HeaderText = "Store_Name";
            this.dataGridViewTextBoxColumn2.MinimumWidth = 8;
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.Width = 150;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "Store_Location";
            this.dataGridViewTextBoxColumn3.HeaderText = "Store_Location";
            this.dataGridViewTextBoxColumn3.MinimumWidth = 8;
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.Width = 150;
            // 
            // store_IDLabel
            // 
            store_IDLabel.AutoSize = true;
            store_IDLabel.Location = new System.Drawing.Point(211, 140);
            store_IDLabel.Name = "store_IDLabel";
            store_IDLabel.Size = new System.Drawing.Size(89, 25);
            store_IDLabel.TabIndex = 2;
            store_IDLabel.Text = "Store ID:";
            // 
            // store_IDTextBox
            // 
            this.store_IDTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.storesBindingSource, "Store_ID", true));
            this.store_IDTextBox.Location = new System.Drawing.Point(356, 140);
            this.store_IDTextBox.Name = "store_IDTextBox";
            this.store_IDTextBox.Size = new System.Drawing.Size(463, 30);
            this.store_IDTextBox.TabIndex = 3;
            // 
            // store_NameLabel
            // 
            store_NameLabel.AutoSize = true;
            store_NameLabel.Location = new System.Drawing.Point(211, 176);
            store_NameLabel.Name = "store_NameLabel";
            store_NameLabel.Size = new System.Drawing.Size(122, 25);
            store_NameLabel.TabIndex = 4;
            store_NameLabel.Text = "Store Name:";
            // 
            // store_NameTextBox
            // 
            this.store_NameTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.storesBindingSource, "Store_Name", true));
            this.store_NameTextBox.Location = new System.Drawing.Point(356, 176);
            this.store_NameTextBox.Name = "store_NameTextBox";
            this.store_NameTextBox.Size = new System.Drawing.Size(463, 30);
            this.store_NameTextBox.TabIndex = 5;
            // 
            // store_LocationLabel
            // 
            store_LocationLabel.AutoSize = true;
            store_LocationLabel.Location = new System.Drawing.Point(211, 212);
            store_LocationLabel.Name = "store_LocationLabel";
            store_LocationLabel.Size = new System.Drawing.Size(144, 25);
            store_LocationLabel.TabIndex = 6;
            store_LocationLabel.Text = "Store Location:";
            // 
            // store_LocationTextBox
            // 
            this.store_LocationTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.storesBindingSource, "Store_Location", true));
            this.store_LocationTextBox.Location = new System.Drawing.Point(356, 212);
            this.store_LocationTextBox.Name = "store_LocationTextBox";
            this.store_LocationTextBox.Size = new System.Drawing.Size(463, 30);
            this.store_LocationTextBox.TabIndex = 7;
            // 
            // item_BalancesBindingSource
            // 
            this.item_BalancesBindingSource.DataMember = "FK_ItemBal_Store";
            this.item_BalancesBindingSource.DataSource = this.storesBindingSource;
            // 
            // item_BalancesTableAdapter
            // 
            this.item_BalancesTableAdapter.ClearBeforeFill = true;
            // 
            // Stores
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1269, 592);
            this.Controls.Add(store_IDLabel);
            this.Controls.Add(this.store_IDTextBox);
            this.Controls.Add(store_NameLabel);
            this.Controls.Add(this.store_NameTextBox);
            this.Controls.Add(store_LocationLabel);
            this.Controls.Add(this.store_LocationTextBox);
            this.Controls.Add(this.storesDataGridView);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Stores";
            this.Text = "بيانات المخازن";
            this.Load += new System.EventHandler(this.Stores_Load);
            ((System.ComponentModel.ISupportInitialize)(this.alRowad_ERPDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.storesBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.storesDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.item_BalancesBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private AlRowad_ERPDataSet alRowad_ERPDataSet;
        private System.Windows.Forms.BindingSource storesBindingSource;
        private AlRowad_ERPDataSetTableAdapters.StoresTableAdapter storesTableAdapter;
        private AlRowad_ERPDataSetTableAdapters.TableAdapterManager tableAdapterManager;
        private AlRowad_ERPDataSetTableAdapters.Item_BalancesTableAdapter item_BalancesTableAdapter;
        private System.Windows.Forms.DataGridView storesDataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.TextBox store_IDTextBox;
        private System.Windows.Forms.TextBox store_NameTextBox;
        private System.Windows.Forms.TextBox store_LocationTextBox;
        private System.Windows.Forms.BindingSource item_BalancesBindingSource;
    }
}