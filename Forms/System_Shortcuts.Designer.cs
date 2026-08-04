namespace AlRowad_ERP.Forms
{
    partial class System_Shortcuts
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
            System.Windows.Forms.Label operation_NameLabel;
            System.Windows.Forms.Label modifier_KeyLabel;
            System.Windows.Forms.Label primary_KeyLabel;
            this.alRowad_ERPDataSet = new AlRowad_ERP.AlRowad_ERPDataSet();
            this.system_ShortcutsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.system_ShortcutsTableAdapter = new AlRowad_ERP.AlRowad_ERPDataSetTableAdapters.System_ShortcutsTableAdapter();
            this.tableAdapterManager = new AlRowad_ERP.AlRowad_ERPDataSetTableAdapters.TableAdapterManager();
            this.operation_NameTextBox = new System.Windows.Forms.TextBox();
            this.modifier_KeyTextBox = new System.Windows.Forms.TextBox();
            this.primary_KeyTextBox = new System.Windows.Forms.TextBox();
            this.alRowadToolBar1 = new AlRowad_ERP.Controls.AlRowadToolBar();
            operation_NameLabel = new System.Windows.Forms.Label();
            modifier_KeyLabel = new System.Windows.Forms.Label();
            primary_KeyLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.alRowad_ERPDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.system_ShortcutsBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // alRowad_ERPDataSet
            // 
            this.alRowad_ERPDataSet.DataSetName = "AlRowad_ERPDataSet";
            this.alRowad_ERPDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // system_ShortcutsBindingSource
            // 
            this.system_ShortcutsBindingSource.DataMember = "System_Shortcuts";
            this.system_ShortcutsBindingSource.DataSource = this.alRowad_ERPDataSet;
            // 
            // system_ShortcutsTableAdapter
            // 
            this.system_ShortcutsTableAdapter.ClearBeforeFill = true;
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
            this.tableAdapterManager.Cash_VouchersTableAdapter = null;
            this.tableAdapterManager.StoresTableAdapter = null;
            this.tableAdapterManager.SuppliersTableAdapter = null;
            this.tableAdapterManager.System_ShortcutsTableAdapter = this.system_ShortcutsTableAdapter;
            this.tableAdapterManager.UnitsTableAdapter = null;
            this.tableAdapterManager.UpdateOrder = AlRowad_ERP.AlRowad_ERPDataSetTableAdapters.TableAdapterManager.UpdateOrderOption.InsertUpdateDelete;
            // 
            // operation_NameLabel
            // 
            operation_NameLabel.AutoSize = true;
            operation_NameLabel.Location = new System.Drawing.Point(365, 178);
            operation_NameLabel.Name = "operation_NameLabel";
            operation_NameLabel.Size = new System.Drawing.Size(161, 25);
            operation_NameLabel.TabIndex = 1;
            operation_NameLabel.Text = "Operation Name:";
            // 
            // operation_NameTextBox
            // 
            this.operation_NameTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.system_ShortcutsBindingSource, "Operation_Name", true));
            this.operation_NameTextBox.Location = new System.Drawing.Point(532, 175);
            this.operation_NameTextBox.Name = "operation_NameTextBox";
            this.operation_NameTextBox.Size = new System.Drawing.Size(100, 30);
            this.operation_NameTextBox.TabIndex = 2;
            // 
            // modifier_KeyLabel
            // 
            modifier_KeyLabel.AutoSize = true;
            modifier_KeyLabel.Location = new System.Drawing.Point(365, 214);
            modifier_KeyLabel.Name = "modifier_KeyLabel";
            modifier_KeyLabel.Size = new System.Drawing.Size(127, 25);
            modifier_KeyLabel.TabIndex = 3;
            modifier_KeyLabel.Text = "Modifier Key:";
            // 
            // modifier_KeyTextBox
            // 
            this.modifier_KeyTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.system_ShortcutsBindingSource, "Modifier_Key", true));
            this.modifier_KeyTextBox.Location = new System.Drawing.Point(532, 211);
            this.modifier_KeyTextBox.Name = "modifier_KeyTextBox";
            this.modifier_KeyTextBox.Size = new System.Drawing.Size(100, 30);
            this.modifier_KeyTextBox.TabIndex = 4;
            // 
            // primary_KeyLabel
            // 
            primary_KeyLabel.AutoSize = true;
            primary_KeyLabel.Location = new System.Drawing.Point(365, 250);
            primary_KeyLabel.Name = "primary_KeyLabel";
            primary_KeyLabel.Size = new System.Drawing.Size(124, 25);
            primary_KeyLabel.TabIndex = 5;
            primary_KeyLabel.Text = "Primary Key:";
            // 
            // primary_KeyTextBox
            // 
            this.primary_KeyTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.system_ShortcutsBindingSource, "Primary_Key", true));
            this.primary_KeyTextBox.Location = new System.Drawing.Point(532, 247);
            this.primary_KeyTextBox.Name = "primary_KeyTextBox";
            this.primary_KeyTextBox.Size = new System.Drawing.Size(100, 30);
            this.primary_KeyTextBox.TabIndex = 6;
            // 
            // alRowadToolBar1
            // 
            this.alRowadToolBar1.BackColor = System.Drawing.Color.Transparent;
            this.alRowadToolBar1.Cursor = System.Windows.Forms.Cursors.PanNW;
            this.alRowadToolBar1.Location = new System.Drawing.Point(59, 39);
            this.alRowadToolBar1.Name = "alRowadToolBar1";
            this.alRowadToolBar1.Size = new System.Drawing.Size(984, 53);
            this.alRowadToolBar1.TabIndex = 7;
            // 
            // System_Shortcuts
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1067, 592);
            this.Controls.Add(this.alRowadToolBar1);
            this.Controls.Add(operation_NameLabel);
            this.Controls.Add(this.operation_NameTextBox);
            this.Controls.Add(modifier_KeyLabel);
            this.Controls.Add(this.modifier_KeyTextBox);
            this.Controls.Add(primary_KeyLabel);
            this.Controls.Add(this.primary_KeyTextBox);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "System_Shortcuts";
            this.Text = "بيانات المخازن";
            this.Load += new System.EventHandler(this.System_Shortcuts_Load);
            ((System.ComponentModel.ISupportInitialize)(this.alRowad_ERPDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.system_ShortcutsBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private AlRowad_ERPDataSet alRowad_ERPDataSet;
        private System.Windows.Forms.BindingSource system_ShortcutsBindingSource;
        private AlRowad_ERPDataSetTableAdapters.System_ShortcutsTableAdapter system_ShortcutsTableAdapter;
        private AlRowad_ERPDataSetTableAdapters.TableAdapterManager tableAdapterManager;
        private System.Windows.Forms.TextBox operation_NameTextBox;
        private System.Windows.Forms.TextBox modifier_KeyTextBox;
        private System.Windows.Forms.TextBox primary_KeyTextBox;
        private Controls.AlRowadToolBar alRowadToolBar1;
    }
}