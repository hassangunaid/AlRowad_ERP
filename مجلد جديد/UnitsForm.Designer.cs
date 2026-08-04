using System;
using System.Windows.Forms;

namespace AlRowad_ERP.Forms
{
    partial class UnitsForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.Label unit_IDLabel;
            System.Windows.Forms.Label unit_NameLabel;
            System.Windows.Forms.Label conversion_FactorLabel;
            this.alRowad_ERPDataSet = new AlRowad_ERP.AlRowad_ERPDataSet();
            this.unitsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.unitsTableAdapter = new AlRowad_ERP.AlRowad_ERPDataSetTableAdapters.UnitsTableAdapter();
            this.tableAdapterManager = new AlRowad_ERP.AlRowad_ERPDataSetTableAdapters.TableAdapterManager();
            this.unit_IDTextBox = new System.Windows.Forms.TextBox();
            this.unit_NameTextBox = new System.Windows.Forms.TextBox();
            this.conversion_FactorTextBox = new System.Windows.Forms.TextBox();
            this.unitsDataGridView = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.alRowadToolBar1 = new AlRowad_ERP.Controls.AlRowadToolBar();
            unit_IDLabel = new System.Windows.Forms.Label();
            unit_NameLabel = new System.Windows.Forms.Label();
            conversion_FactorLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.alRowad_ERPDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.unitsBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.unitsDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // unit_IDLabel
            // 
            unit_IDLabel.AutoSize = true;
            unit_IDLabel.Location = new System.Drawing.Point(336, 178);
            unit_IDLabel.Name = "unit_IDLabel";
            unit_IDLabel.Size = new System.Drawing.Size(76, 25);
            unit_IDLabel.TabIndex = 1;
            unit_IDLabel.Text = "Unit ID:";
            // 
            // unit_NameLabel
            // 
            unit_NameLabel.AutoSize = true;
            unit_NameLabel.Location = new System.Drawing.Point(336, 214);
            unit_NameLabel.Name = "unit_NameLabel";
            unit_NameLabel.Size = new System.Drawing.Size(109, 25);
            unit_NameLabel.TabIndex = 3;
            unit_NameLabel.Text = "Unit Name:";
            // 
            // conversion_FactorLabel
            // 
            conversion_FactorLabel.AutoSize = true;
            conversion_FactorLabel.Location = new System.Drawing.Point(336, 250);
            conversion_FactorLabel.Name = "conversion_FactorLabel";
            conversion_FactorLabel.Size = new System.Drawing.Size(178, 25);
            conversion_FactorLabel.TabIndex = 5;
            conversion_FactorLabel.Text = "Conversion Factor:";
            // 
            // alRowad_ERPDataSet
            // 
            this.alRowad_ERPDataSet.DataSetName = "AlRowad_ERPDataSet";
            this.alRowad_ERPDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // unitsBindingSource
            // 
            this.unitsBindingSource.DataMember = "Units";
            this.unitsBindingSource.DataSource = this.alRowad_ERPDataSet;
            // 
            // unitsTableAdapter
            // 
            this.unitsTableAdapter.ClearBeforeFill = true;
            // 
            // tableAdapterManager
            // 
            this.tableAdapterManager.AccountsTableAdapter = null;
            this.tableAdapterManager.BackupDataSetBeforeUpdate = false;
            this.tableAdapterManager.CurrenciesTableAdapter = null;
            this.tableAdapterManager.CustomersTableAdapter = null;
            this.tableAdapterManager.Doc_TypesTableAdapter = null;
            this.tableAdapterManager.Item_BalancesTableAdapter = null;
            this.tableAdapterManager.ItemsTableAdapter = null;
            this.tableAdapterManager.Journal_HeaderTableAdapter = null;
            this.tableAdapterManager.Payment_MethodsTableAdapter = null;
            this.tableAdapterManager.StoresTableAdapter = null;
            this.tableAdapterManager.SuppliersTableAdapter = null;
            this.tableAdapterManager.System_ShortcutsTableAdapter = null;
            this.tableAdapterManager.UnitsTableAdapter = this.unitsTableAdapter;
            this.tableAdapterManager.UpdateOrder = AlRowad_ERP.AlRowad_ERPDataSetTableAdapters.TableAdapterManager.UpdateOrderOption.InsertUpdateDelete;
            // 
            // unit_IDTextBox
            // 
            this.unit_IDTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.unitsBindingSource, "Unit_ID", true));
            this.unit_IDTextBox.Location = new System.Drawing.Point(551, 178);
            this.unit_IDTextBox.Name = "unit_IDTextBox";
            this.unit_IDTextBox.Size = new System.Drawing.Size(257, 30);
            this.unit_IDTextBox.TabIndex = 2;
            // 
            // unit_NameTextBox
            // 
            this.unit_NameTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.unitsBindingSource, "Unit_Name", true));
            this.unit_NameTextBox.Location = new System.Drawing.Point(551, 214);
            this.unit_NameTextBox.Name = "unit_NameTextBox";
            this.unit_NameTextBox.Size = new System.Drawing.Size(257, 30);
            this.unit_NameTextBox.TabIndex = 4;
            // 
            // conversion_FactorTextBox
            // 
            this.conversion_FactorTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.unitsBindingSource, "Conversion_Factor", true));
            this.conversion_FactorTextBox.Location = new System.Drawing.Point(551, 250);
            this.conversion_FactorTextBox.Name = "conversion_FactorTextBox";
            this.conversion_FactorTextBox.Size = new System.Drawing.Size(257, 30);
            this.conversion_FactorTextBox.TabIndex = 6;
            // 
            // unitsDataGridView
            // 
            this.unitsDataGridView.AutoGenerateColumns = false;
            this.unitsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.unitsDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3});
            this.unitsDataGridView.DataSource = this.unitsBindingSource;
            this.unitsDataGridView.Location = new System.Drawing.Point(96, 338);
            this.unitsDataGridView.Name = "unitsDataGridView";
            this.unitsDataGridView.RowHeadersWidth = 62;
            this.unitsDataGridView.RowTemplate.Height = 29;
            this.unitsDataGridView.Size = new System.Drawing.Size(969, 420);
            this.unitsDataGridView.TabIndex = 7;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "Unit_ID";
            this.dataGridViewTextBoxColumn1.HeaderText = "Unit_ID";
            this.dataGridViewTextBoxColumn1.MinimumWidth = 8;
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Width = 150;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "Unit_Name";
            this.dataGridViewTextBoxColumn2.HeaderText = "Unit_Name";
            this.dataGridViewTextBoxColumn2.MinimumWidth = 8;
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.Width = 150;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "Conversion_Factor";
            this.dataGridViewTextBoxColumn3.HeaderText = "Conversion_Factor";
            this.dataGridViewTextBoxColumn3.MinimumWidth = 8;
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.Width = 150;
            // 
            // alRowadToolBar1
            // 
            this.alRowadToolBar1.Location = new System.Drawing.Point(68, 52);
            this.alRowadToolBar1.Name = "alRowadToolBar1";
            this.alRowadToolBar1.Size = new System.Drawing.Size(1014, 53);
            this.alRowadToolBar1.TabIndex = 8;
            this.alRowadToolBar1.Load += new System.EventHandler(this.alRowadToolBar);
            // 
            // UnitsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(1924, 1050);
            this.Controls.Add(this.alRowadToolBar1);
            this.Controls.Add(this.unitsDataGridView);
            this.Controls.Add(unit_IDLabel);
            this.Controls.Add(this.unit_IDTextBox);
            this.Controls.Add(unit_NameLabel);
            this.Controls.Add(this.unit_NameTextBox);
            this.Controls.Add(conversion_FactorLabel);
            this.Controls.Add(this.conversion_FactorTextBox);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "UnitsForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "تعريف الوحدات";
            this.Load += new System.EventHandler(this.UnitsForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.alRowad_ERPDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.unitsBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.unitsDataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private AlRowad_ERPDataSet alRowad_ERPDataSet;
        private System.Windows.Forms.BindingSource unitsBindingSource;
        private AlRowad_ERPDataSetTableAdapters.UnitsTableAdapter unitsTableAdapter;
        private AlRowad_ERPDataSetTableAdapters.TableAdapterManager tableAdapterManager;
        private System.Windows.Forms.TextBox unit_IDTextBox;
        private System.Windows.Forms.TextBox unit_NameTextBox;
        private System.Windows.Forms.TextBox conversion_FactorTextBox;
        private System.Windows.Forms.DataGridView unitsDataGridView;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private Controls.AlRowadToolBar alRowadToolBar1;
    }
}
