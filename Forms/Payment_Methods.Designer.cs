namespace AlRowad_ERP.Forms
{
    partial class Payment_Methods
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
            System.Windows.Forms.Label method_IDLabel;
            System.Windows.Forms.Label method_NameLabel;
            this.alRowad_ERPDataSet = new AlRowad_ERP.AlRowad_ERPDataSet();
            this.payment_MethodsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.payment_MethodsTableAdapter = new AlRowad_ERP.AlRowad_ERPDataSetTableAdapters.Payment_MethodsTableAdapter();
            this.tableAdapterManager = new AlRowad_ERP.AlRowad_ERPDataSetTableAdapters.TableAdapterManager();
            this.method_IDTextBox = new System.Windows.Forms.TextBox();
            this.method_NameTextBox = new System.Windows.Forms.TextBox();
            this.alRowadToolBar1 = new AlRowad_ERP.Controls.AlRowadToolBar();
            method_IDLabel = new System.Windows.Forms.Label();
            method_NameLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.alRowad_ERPDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.payment_MethodsBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // alRowad_ERPDataSet
            // 
            this.alRowad_ERPDataSet.DataSetName = "AlRowad_ERPDataSet";
            this.alRowad_ERPDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // payment_MethodsBindingSource
            // 
            this.payment_MethodsBindingSource.DataMember = "Payment_Methods";
            this.payment_MethodsBindingSource.DataSource = this.alRowad_ERPDataSet;
            // 
            // payment_MethodsTableAdapter
            // 
            this.payment_MethodsTableAdapter.ClearBeforeFill = true;
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
            this.tableAdapterManager.Payment_MethodsTableAdapter = this.payment_MethodsTableAdapter;
            this.tableAdapterManager.Cash_VouchersTableAdapter = null;
            this.tableAdapterManager.StoresTableAdapter = null;
            this.tableAdapterManager.SuppliersTableAdapter = null;
            this.tableAdapterManager.System_ShortcutsTableAdapter = null;
            this.tableAdapterManager.UnitsTableAdapter = null;
            this.tableAdapterManager.UpdateOrder = AlRowad_ERP.AlRowad_ERPDataSetTableAdapters.TableAdapterManager.UpdateOrderOption.InsertUpdateDelete;
            // 
            // method_IDLabel
            // 
            method_IDLabel.AutoSize = true;
            method_IDLabel.Location = new System.Drawing.Point(382, 221);
            method_IDLabel.Name = "method_IDLabel";
            method_IDLabel.Size = new System.Drawing.Size(108, 25);
            method_IDLabel.TabIndex = 1;
            method_IDLabel.Text = "Method ID:";
            // 
            // method_IDTextBox
            // 
            this.method_IDTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.payment_MethodsBindingSource, "Method_ID", true));
            this.method_IDTextBox.Location = new System.Drawing.Point(529, 218);
            this.method_IDTextBox.Name = "method_IDTextBox";
            this.method_IDTextBox.Size = new System.Drawing.Size(100, 30);
            this.method_IDTextBox.TabIndex = 2;
            // 
            // method_NameLabel
            // 
            method_NameLabel.AutoSize = true;
            method_NameLabel.Location = new System.Drawing.Point(382, 257);
            method_NameLabel.Name = "method_NameLabel";
            method_NameLabel.Size = new System.Drawing.Size(141, 25);
            method_NameLabel.TabIndex = 3;
            method_NameLabel.Text = "Method Name:";
            // 
            // method_NameTextBox
            // 
            this.method_NameTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.payment_MethodsBindingSource, "Method_Name", true));
            this.method_NameTextBox.Location = new System.Drawing.Point(529, 254);
            this.method_NameTextBox.Name = "method_NameTextBox";
            this.method_NameTextBox.Size = new System.Drawing.Size(100, 30);
            this.method_NameTextBox.TabIndex = 4;
            // 
            // alRowadToolBar1
            // 
            this.alRowadToolBar1.BackColor = System.Drawing.Color.Transparent;
            this.alRowadToolBar1.Cursor = System.Windows.Forms.Cursors.PanNW;
            this.alRowadToolBar1.Location = new System.Drawing.Point(60, 24);
            this.alRowadToolBar1.Name = "alRowadToolBar1";
            this.alRowadToolBar1.Size = new System.Drawing.Size(984, 53);
            this.alRowadToolBar1.TabIndex = 5;
            // 
            // Payment_Methods
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1067, 592);
            this.Controls.Add(this.alRowadToolBar1);
            this.Controls.Add(method_IDLabel);
            this.Controls.Add(this.method_IDTextBox);
            this.Controls.Add(method_NameLabel);
            this.Controls.Add(this.method_NameTextBox);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Payment_Methods";
            this.Text = "طرق الدفع";
            this.Load += new System.EventHandler(this.Payment_Methods_Load);
            ((System.ComponentModel.ISupportInitialize)(this.alRowad_ERPDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.payment_MethodsBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private AlRowad_ERPDataSet alRowad_ERPDataSet;
        private System.Windows.Forms.BindingSource payment_MethodsBindingSource;
        private AlRowad_ERPDataSetTableAdapters.Payment_MethodsTableAdapter payment_MethodsTableAdapter;
        private AlRowad_ERPDataSetTableAdapters.TableAdapterManager tableAdapterManager;
        private System.Windows.Forms.TextBox method_IDTextBox;
        private System.Windows.Forms.TextBox method_NameTextBox;
        private Controls.AlRowadToolBar alRowadToolBar1;
    }
}