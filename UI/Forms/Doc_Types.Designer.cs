namespace AlRowad_ERP.Forms
{
    partial class Doc_Types
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
            System.Windows.Forms.Label doc_Type_IDLabel;
            System.Windows.Forms.Label doc_NameLabel;
            System.Windows.Forms.Label module_NameLabel;
            this.alRowad_ERPDataSet = new AlRowad_ERP.AlRowad_ERPDataSet();
            this.doc_TypesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.doc_TypesTableAdapter = new AlRowad_ERP.AlRowad_ERPDataSetTableAdapters.Doc_TypesTableAdapter();
            this.tableAdapterManager = new AlRowad_ERP.AlRowad_ERPDataSetTableAdapters.TableAdapterManager();
            this.doc_Type_IDTextBox = new System.Windows.Forms.TextBox();
            this.doc_NameTextBox = new System.Windows.Forms.TextBox();
            this.module_NameTextBox = new System.Windows.Forms.TextBox();
            this.alRowadToolBar1 = new AlRowad_ERP.Controls.AlRowadToolBar();
            doc_Type_IDLabel = new System.Windows.Forms.Label();
            doc_NameLabel = new System.Windows.Forms.Label();
            module_NameLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.alRowad_ERPDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.doc_TypesBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // alRowad_ERPDataSet
            // 
            this.alRowad_ERPDataSet.DataSetName = "AlRowad_ERPDataSet";
            this.alRowad_ERPDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // doc_TypesBindingSource
            // 
            this.doc_TypesBindingSource.DataMember = "Doc_Types";
            this.doc_TypesBindingSource.DataSource = this.alRowad_ERPDataSet;
            // 
            // doc_TypesTableAdapter
            // 
            this.doc_TypesTableAdapter.ClearBeforeFill = true;
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
            this.tableAdapterManager.Doc_TypesTableAdapter = this.doc_TypesTableAdapter;
            this.tableAdapterManager.Invoice_DetailsTableAdapter = null;
            this.tableAdapterManager.Invoice_HeaderTableAdapter = null;
            this.tableAdapterManager.Item_BalancesTableAdapter = null;
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
            // doc_Type_IDLabel
            // 
            doc_Type_IDLabel.AutoSize = true;
            doc_Type_IDLabel.Location = new System.Drawing.Point(436, 154);
            doc_Type_IDLabel.Name = "doc_Type_IDLabel";
            doc_Type_IDLabel.Size = new System.Drawing.Size(127, 25);
            doc_Type_IDLabel.TabIndex = 0;
            doc_Type_IDLabel.Text = "Doc Type ID:";
            // 
            // doc_Type_IDTextBox
            // 
            this.doc_Type_IDTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.doc_TypesBindingSource, "Doc_Type_ID", true));
            this.doc_Type_IDTextBox.Location = new System.Drawing.Point(582, 151);
            this.doc_Type_IDTextBox.Name = "doc_Type_IDTextBox";
            this.doc_Type_IDTextBox.Size = new System.Drawing.Size(100, 30);
            this.doc_Type_IDTextBox.TabIndex = 1;
            // 
            // doc_NameLabel
            // 
            doc_NameLabel.AutoSize = true;
            doc_NameLabel.Location = new System.Drawing.Point(436, 190);
            doc_NameLabel.Name = "doc_NameLabel";
            doc_NameLabel.Size = new System.Drawing.Size(110, 25);
            doc_NameLabel.TabIndex = 2;
            doc_NameLabel.Text = "Doc Name:";
            // 
            // doc_NameTextBox
            // 
            this.doc_NameTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.doc_TypesBindingSource, "Doc_Name", true));
            this.doc_NameTextBox.Location = new System.Drawing.Point(582, 187);
            this.doc_NameTextBox.Name = "doc_NameTextBox";
            this.doc_NameTextBox.Size = new System.Drawing.Size(100, 30);
            this.doc_NameTextBox.TabIndex = 3;
            // 
            // module_NameLabel
            // 
            module_NameLabel.AutoSize = true;
            module_NameLabel.Location = new System.Drawing.Point(436, 226);
            module_NameLabel.Name = "module_NameLabel";
            module_NameLabel.Size = new System.Drawing.Size(140, 25);
            module_NameLabel.TabIndex = 4;
            module_NameLabel.Text = "Module Name:";
            // 
            // module_NameTextBox
            // 
            this.module_NameTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.doc_TypesBindingSource, "Module_Name", true));
            this.module_NameTextBox.Location = new System.Drawing.Point(582, 223);
            this.module_NameTextBox.Name = "module_NameTextBox";
            this.module_NameTextBox.Size = new System.Drawing.Size(100, 30);
            this.module_NameTextBox.TabIndex = 5;
            // 
            // alRowadToolBar1
            // 
            this.alRowadToolBar1.BackColor = System.Drawing.Color.Transparent;
            this.alRowadToolBar1.Cursor = System.Windows.Forms.Cursors.PanNW;
            this.alRowadToolBar1.Location = new System.Drawing.Point(35, 47);
            this.alRowadToolBar1.Name = "alRowadToolBar1";
            this.alRowadToolBar1.Size = new System.Drawing.Size(984, 53);
            this.alRowadToolBar1.TabIndex = 6;
            // 
            // Doc_Types
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1067, 592);
            this.Controls.Add(this.alRowadToolBar1);
            this.Controls.Add(doc_Type_IDLabel);
            this.Controls.Add(this.doc_Type_IDTextBox);
            this.Controls.Add(doc_NameLabel);
            this.Controls.Add(this.doc_NameTextBox);
            this.Controls.Add(module_NameLabel);
            this.Controls.Add(this.module_NameTextBox);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Doc_Types";
            this.Text = "انواع المستندات";
            this.Load += new System.EventHandler(this.Doc_Types_Load);
            ((System.ComponentModel.ISupportInitialize)(this.alRowad_ERPDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.doc_TypesBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private AlRowad_ERPDataSet alRowad_ERPDataSet;
        private System.Windows.Forms.BindingSource doc_TypesBindingSource;
        private AlRowad_ERPDataSetTableAdapters.Doc_TypesTableAdapter doc_TypesTableAdapter;
        private AlRowad_ERPDataSetTableAdapters.TableAdapterManager tableAdapterManager;
        private System.Windows.Forms.TextBox doc_Type_IDTextBox;
        private System.Windows.Forms.TextBox doc_NameTextBox;
        private System.Windows.Forms.TextBox module_NameTextBox;
        private Controls.AlRowadToolBar alRowadToolBar1;
    }
}