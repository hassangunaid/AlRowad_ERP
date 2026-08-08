namespace AlRowad_ERP.Forms
{
    partial class Item_Balances
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
            System.Windows.Forms.Label balance_IDLabel;
            System.Windows.Forms.Label item_IDLabel;
            System.Windows.Forms.Label store_IDLabel;
            System.Windows.Forms.Label quantityLabel;
            this.alRowad_ERPDataSet = new AlRowad_ERP.AlRowad_ERPDataSet();
            this.item_BalancesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.item_BalancesTableAdapter = new AlRowad_ERP.AlRowad_ERPDataSetTableAdapters.Item_BalancesTableAdapter();
            this.tableAdapterManager = new AlRowad_ERP.AlRowad_ERPDataSetTableAdapters.TableAdapterManager();
            this.balance_IDTextBox = new System.Windows.Forms.TextBox();
            this.item_IDTextBox = new System.Windows.Forms.TextBox();
            this.store_IDTextBox = new System.Windows.Forms.TextBox();
            this.quantityTextBox = new System.Windows.Forms.TextBox();
            this.alRowadToolBar1 = new AlRowad_ERP.Controls.AlRowadToolBar();
            balance_IDLabel = new System.Windows.Forms.Label();
            item_IDLabel = new System.Windows.Forms.Label();
            store_IDLabel = new System.Windows.Forms.Label();
            quantityLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.alRowad_ERPDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.item_BalancesBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // alRowad_ERPDataSet
            // 
            this.alRowad_ERPDataSet.DataSetName = "AlRowad_ERPDataSet";
            this.alRowad_ERPDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // item_BalancesBindingSource
            // 
            this.item_BalancesBindingSource.DataMember = "Item_Balances";
            this.item_BalancesBindingSource.DataSource = this.alRowad_ERPDataSet;
            // 
            // item_BalancesTableAdapter
            // 
            this.item_BalancesTableAdapter.ClearBeforeFill = true;
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
            this.tableAdapterManager.Item_BalancesTableAdapter = this.item_BalancesTableAdapter;
            this.tableAdapterManager.ItemsTableAdapter = null;
            this.tableAdapterManager.Journal_DetailsTableAdapter = null;
            this.tableAdapterManager.Journal_HeaderTableAdapter = null;
            this.tableAdapterManager.Payment_MethodsTableAdapter = null;
            this.tableAdapterManager.Cash_VouchersTableAdapter = null;
            this.tableAdapterManager.StoresTableAdapter = null;
            this.tableAdapterManager.SuppliersTableAdapter = null;
            this.tableAdapterManager.System_ShortcutsTableAdapter = null;
            this.tableAdapterManager.UnitsTableAdapter = null;
            this.tableAdapterManager.UpdateOrder = AlRowad_ERP.AlRowad_ERPDataSetTableAdapters.TableAdapterManager.UpdateOrderOption.InsertUpdateDelete;
            // 
            // balance_IDLabel
            // 
            balance_IDLabel.AutoSize = true;
            balance_IDLabel.Location = new System.Drawing.Point(467, 139);
            balance_IDLabel.Name = "balance_IDLabel";
            balance_IDLabel.Size = new System.Drawing.Size(113, 25);
            balance_IDLabel.TabIndex = 1;
            balance_IDLabel.Text = "Balance ID:";
            // 
            // balance_IDTextBox
            // 
            this.balance_IDTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.item_BalancesBindingSource, "Balance_ID", true));
            this.balance_IDTextBox.Location = new System.Drawing.Point(586, 136);
            this.balance_IDTextBox.Name = "balance_IDTextBox";
            this.balance_IDTextBox.Size = new System.Drawing.Size(100, 30);
            this.balance_IDTextBox.TabIndex = 2;
            // 
            // item_IDLabel
            // 
            item_IDLabel.AutoSize = true;
            item_IDLabel.Location = new System.Drawing.Point(467, 175);
            item_IDLabel.Name = "item_IDLabel";
            item_IDLabel.Size = new System.Drawing.Size(79, 25);
            item_IDLabel.TabIndex = 3;
            item_IDLabel.Text = "Item ID:";
            // 
            // item_IDTextBox
            // 
            this.item_IDTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.item_BalancesBindingSource, "Item_ID", true));
            this.item_IDTextBox.Location = new System.Drawing.Point(586, 172);
            this.item_IDTextBox.Name = "item_IDTextBox";
            this.item_IDTextBox.Size = new System.Drawing.Size(100, 30);
            this.item_IDTextBox.TabIndex = 4;
            // 
            // store_IDLabel
            // 
            store_IDLabel.AutoSize = true;
            store_IDLabel.Location = new System.Drawing.Point(467, 211);
            store_IDLabel.Name = "store_IDLabel";
            store_IDLabel.Size = new System.Drawing.Size(89, 25);
            store_IDLabel.TabIndex = 5;
            store_IDLabel.Text = "Store ID:";
            // 
            // store_IDTextBox
            // 
            this.store_IDTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.item_BalancesBindingSource, "Store_ID", true));
            this.store_IDTextBox.Location = new System.Drawing.Point(586, 208);
            this.store_IDTextBox.Name = "store_IDTextBox";
            this.store_IDTextBox.Size = new System.Drawing.Size(100, 30);
            this.store_IDTextBox.TabIndex = 6;
            // 
            // quantityLabel
            // 
            quantityLabel.AutoSize = true;
            quantityLabel.Location = new System.Drawing.Point(467, 247);
            quantityLabel.Name = "quantityLabel";
            quantityLabel.Size = new System.Drawing.Size(91, 25);
            quantityLabel.TabIndex = 7;
            quantityLabel.Text = "Quantity:";
            // 
            // quantityTextBox
            // 
            this.quantityTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.item_BalancesBindingSource, "Quantity", true));
            this.quantityTextBox.Location = new System.Drawing.Point(586, 244);
            this.quantityTextBox.Name = "quantityTextBox";
            this.quantityTextBox.Size = new System.Drawing.Size(100, 30);
            this.quantityTextBox.TabIndex = 8;
            // 
            // alRowadToolBar1
            // 
            this.alRowadToolBar1.BackColor = System.Drawing.Color.Transparent;
            this.alRowadToolBar1.Cursor = System.Windows.Forms.Cursors.PanNW;
            this.alRowadToolBar1.Location = new System.Drawing.Point(70, 28);
            this.alRowadToolBar1.Name = "alRowadToolBar1";
            this.alRowadToolBar1.Size = new System.Drawing.Size(984, 53);
            this.alRowadToolBar1.TabIndex = 9;
            // 
            // Item_Balances
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1067, 592);
            this.Controls.Add(this.alRowadToolBar1);
            this.Controls.Add(balance_IDLabel);
            this.Controls.Add(this.balance_IDTextBox);
            this.Controls.Add(item_IDLabel);
            this.Controls.Add(this.item_IDTextBox);
            this.Controls.Add(store_IDLabel);
            this.Controls.Add(this.store_IDTextBox);
            this.Controls.Add(quantityLabel);
            this.Controls.Add(this.quantityTextBox);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Item_Balances";
            this.Text = "بيانات المخزون";
            this.Load += new System.EventHandler(this.Item_Balances_Load);
            ((System.ComponentModel.ISupportInitialize)(this.alRowad_ERPDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.item_BalancesBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private AlRowad_ERPDataSet alRowad_ERPDataSet;
        private System.Windows.Forms.BindingSource item_BalancesBindingSource;
        private AlRowad_ERPDataSetTableAdapters.Item_BalancesTableAdapter item_BalancesTableAdapter;
        private AlRowad_ERPDataSetTableAdapters.TableAdapterManager tableAdapterManager;
        private System.Windows.Forms.TextBox balance_IDTextBox;
        private System.Windows.Forms.TextBox item_IDTextBox;
        private System.Windows.Forms.TextBox store_IDTextBox;
        private System.Windows.Forms.TextBox quantityTextBox;
        private Controls.AlRowadToolBar alRowadToolBar1;
    }
}