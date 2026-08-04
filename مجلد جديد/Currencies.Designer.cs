namespace AlRowad_ERP.Forms
{
    partial class Currencies
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
            System.Windows.Forms.Label cur_IDLabel;
            System.Windows.Forms.Label cur_NameLabel;
            System.Windows.Forms.Label exchange_RateLabel;
            System.Windows.Forms.Label cur_SymbolLabel;
            System.Windows.Forms.Label cur_CodeLabel;
            this.cur_IDTextBox = new System.Windows.Forms.TextBox();
            this.cur_NameTextBox = new System.Windows.Forms.TextBox();
            this.exchange_RateTextBox = new System.Windows.Forms.TextBox();
            this.cur_SymbolTextBox = new System.Windows.Forms.TextBox();
            this.cur_CodeTextBox = new System.Windows.Forms.TextBox();
            this.alRowad_ERPDataSet = new AlRowad_ERP.AlRowad_ERPDataSet();
            this.currenciesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.currenciesTableAdapter = new AlRowad_ERP.AlRowad_ERPDataSetTableAdapters.CurrenciesTableAdapter();
            this.tableAdapterManager = new AlRowad_ERP.AlRowad_ERPDataSetTableAdapters.TableAdapterManager();
            this.currenciesDataGridView = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.iconToolStripButton1 = new FontAwesome.Sharp.IconToolStripButton();
            this.iconToolStripButton2 = new FontAwesome.Sharp.IconToolStripButton();
            cur_IDLabel = new System.Windows.Forms.Label();
            cur_NameLabel = new System.Windows.Forms.Label();
            exchange_RateLabel = new System.Windows.Forms.Label();
            cur_SymbolLabel = new System.Windows.Forms.Label();
            cur_CodeLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.alRowad_ERPDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.currenciesBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.currenciesDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // cur_IDLabel
            // 
            cur_IDLabel.AutoSize = true;
            cur_IDLabel.Location = new System.Drawing.Point(271, 196);
            cur_IDLabel.Name = "cur_IDLabel";
            cur_IDLabel.Size = new System.Drawing.Size(62, 19);
            cur_IDLabel.TabIndex = 13;
            cur_IDLabel.Text = "Cur ID:";
            // 
            // cur_NameLabel
            // 
            cur_NameLabel.AutoSize = true;
            cur_NameLabel.Location = new System.Drawing.Point(271, 229);
            cur_NameLabel.Name = "cur_NameLabel";
            cur_NameLabel.Size = new System.Drawing.Size(86, 19);
            cur_NameLabel.TabIndex = 15;
            cur_NameLabel.Text = "Cur Name:";
            // 
            // exchange_RateLabel
            // 
            exchange_RateLabel.AutoSize = true;
            exchange_RateLabel.Location = new System.Drawing.Point(271, 262);
            exchange_RateLabel.Name = "exchange_RateLabel";
            exchange_RateLabel.Size = new System.Drawing.Size(118, 19);
            exchange_RateLabel.TabIndex = 17;
            exchange_RateLabel.Text = "Exchange Rate:";
            // 
            // cur_SymbolLabel
            // 
            cur_SymbolLabel.AutoSize = true;
            cur_SymbolLabel.Location = new System.Drawing.Point(271, 295);
            cur_SymbolLabel.Name = "cur_SymbolLabel";
            cur_SymbolLabel.Size = new System.Drawing.Size(98, 19);
            cur_SymbolLabel.TabIndex = 19;
            cur_SymbolLabel.Text = "Cur Symbol:";
            // 
            // cur_CodeLabel
            // 
            cur_CodeLabel.AutoSize = true;
            cur_CodeLabel.Location = new System.Drawing.Point(271, 328);
            cur_CodeLabel.Name = "cur_CodeLabel";
            cur_CodeLabel.Size = new System.Drawing.Size(81, 19);
            cur_CodeLabel.TabIndex = 21;
            cur_CodeLabel.Text = "Cur Code:";
            // 
            // cur_IDTextBox
            // 
            this.cur_IDTextBox.Location = new System.Drawing.Point(398, 188);
            this.cur_IDTextBox.Name = "cur_IDTextBox";
            this.cur_IDTextBox.Size = new System.Drawing.Size(304, 27);
            this.cur_IDTextBox.TabIndex = 14;
            // 
            // cur_NameTextBox
            // 
            this.cur_NameTextBox.Location = new System.Drawing.Point(398, 221);
            this.cur_NameTextBox.Name = "cur_NameTextBox";
            this.cur_NameTextBox.Size = new System.Drawing.Size(304, 27);
            this.cur_NameTextBox.TabIndex = 16;
            // 
            // exchange_RateTextBox
            // 
            this.exchange_RateTextBox.Location = new System.Drawing.Point(398, 254);
            this.exchange_RateTextBox.Name = "exchange_RateTextBox";
            this.exchange_RateTextBox.Size = new System.Drawing.Size(304, 27);
            this.exchange_RateTextBox.TabIndex = 18;
            // 
            // cur_SymbolTextBox
            // 
            this.cur_SymbolTextBox.Location = new System.Drawing.Point(398, 287);
            this.cur_SymbolTextBox.Name = "cur_SymbolTextBox";
            this.cur_SymbolTextBox.Size = new System.Drawing.Size(304, 27);
            this.cur_SymbolTextBox.TabIndex = 20;
            // 
            // cur_CodeTextBox
            // 
            this.cur_CodeTextBox.Location = new System.Drawing.Point(398, 320);
            this.cur_CodeTextBox.Name = "cur_CodeTextBox";
            this.cur_CodeTextBox.Size = new System.Drawing.Size(304, 27);
            this.cur_CodeTextBox.TabIndex = 22;
            // 
            // alRowad_ERPDataSet
            // 
            this.alRowad_ERPDataSet.DataSetName = "AlRowad_ERPDataSet";
            this.alRowad_ERPDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // currenciesBindingSource
            // 
            this.currenciesBindingSource.DataMember = "Currencies";
            this.currenciesBindingSource.DataSource = this.alRowad_ERPDataSet;
            // 
            // currenciesTableAdapter
            // 
            this.currenciesTableAdapter.ClearBeforeFill = true;
            // 
            // tableAdapterManager
            // 
            this.tableAdapterManager.AccountsTableAdapter = null;
            this.tableAdapterManager.AccountsTableAdapter = null;
            this.tableAdapterManager.BackupDataSetBeforeUpdate = false;
            this.tableAdapterManager.CurrenciesTableAdapter = this.currenciesTableAdapter;
            this.tableAdapterManager.CustomersTableAdapter = null;
            this.tableAdapterManager.Doc_TypesTableAdapter = null;
            this.tableAdapterManager.Item_BalancesTableAdapter = null;
            this.tableAdapterManager.ItemsTableAdapter = null;
            this.tableAdapterManager.Journal_HeaderTableAdapter = null;
            this.tableAdapterManager.Payment_MethodsTableAdapter = null;
            this.tableAdapterManager.StoresTableAdapter = null;
            this.tableAdapterManager.SuppliersTableAdapter = null;
            this.tableAdapterManager.System_ShortcutsTableAdapter = null;
            this.tableAdapterManager.UnitsTableAdapter = null;
            this.tableAdapterManager.UpdateOrder = AlRowad_ERP.AlRowad_ERPDataSetTableAdapters.TableAdapterManager.UpdateOrderOption.InsertUpdateDelete;
            // 
            // currenciesDataGridView
            // 
            this.currenciesDataGridView.AutoGenerateColumns = false;
            this.currenciesDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.currenciesDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn4,
            this.dataGridViewTextBoxColumn5});
            this.currenciesDataGridView.DataSource = this.currenciesBindingSource;
            this.currenciesDataGridView.Location = new System.Drawing.Point(150, 384);
            this.currenciesDataGridView.Name = "currenciesDataGridView";
            this.currenciesDataGridView.RowHeadersWidth = 62;
            this.currenciesDataGridView.RowTemplate.Height = 29;
            this.currenciesDataGridView.Size = new System.Drawing.Size(632, 279);
            this.currenciesDataGridView.TabIndex = 23;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "Cur_ID";
            this.dataGridViewTextBoxColumn1.HeaderText = "Cur_ID";
            this.dataGridViewTextBoxColumn1.MinimumWidth = 8;
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Width = 150;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "Cur_Name";
            this.dataGridViewTextBoxColumn2.HeaderText = "Cur_Name";
            this.dataGridViewTextBoxColumn2.MinimumWidth = 8;
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.Width = 150;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "Exchange_Rate";
            this.dataGridViewTextBoxColumn3.HeaderText = "Exchange_Rate";
            this.dataGridViewTextBoxColumn3.MinimumWidth = 8;
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.Width = 150;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.DataPropertyName = "Cur_Symbol";
            this.dataGridViewTextBoxColumn4.HeaderText = "Cur_Symbol";
            this.dataGridViewTextBoxColumn4.MinimumWidth = 8;
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.Width = 150;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.DataPropertyName = "Cur_Code";
            this.dataGridViewTextBoxColumn5.HeaderText = "Cur_Code";
            this.dataGridViewTextBoxColumn5.MinimumWidth = 8;
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.Width = 150;
            // 
            // iconToolStripButton1
            // 
            this.iconToolStripButton1.IconChar = FontAwesome.Sharp.IconChar.None;
            this.iconToolStripButton1.IconColor = System.Drawing.Color.Black;
            this.iconToolStripButton1.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.iconToolStripButton1.Name = "iconToolStripButton1";
            this.iconToolStripButton1.Size = new System.Drawing.Size(23, 23);
            this.iconToolStripButton1.Text = "iconToolStripButton1";
            // 
            // iconToolStripButton2
            // 
            this.iconToolStripButton2.IconChar = FontAwesome.Sharp.IconChar.None;
            this.iconToolStripButton2.IconColor = System.Drawing.Color.Black;
            this.iconToolStripButton2.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.iconToolStripButton2.Name = "iconToolStripButton2";
            this.iconToolStripButton2.Size = new System.Drawing.Size(23, 23);
            this.iconToolStripButton2.Text = "iconToolStripButton2";
            // 
            // Currencies
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(978, 744);
            this.Controls.Add(this.currenciesDataGridView);
            this.Controls.Add(cur_IDLabel);
            this.Controls.Add(this.cur_IDTextBox);
            this.Controls.Add(cur_NameLabel);
            this.Controls.Add(this.cur_NameTextBox);
            this.Controls.Add(exchange_RateLabel);
            this.Controls.Add(this.exchange_RateTextBox);
            this.Controls.Add(cur_SymbolLabel);
            this.Controls.Add(this.cur_SymbolTextBox);
            this.Controls.Add(cur_CodeLabel);
            this.Controls.Add(this.cur_CodeTextBox);
            this.Name = "Currencies";
            this.Text = "تهيئة العملات";
            this.Load += new System.EventHandler(this.CurrenciesForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.alRowad_ERPDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.currenciesBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.currenciesDataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox cur_IDTextBox;
        private System.Windows.Forms.TextBox cur_NameTextBox;
        private System.Windows.Forms.TextBox exchange_RateTextBox;
        private System.Windows.Forms.TextBox cur_SymbolTextBox;
        private System.Windows.Forms.TextBox cur_CodeTextBox;
        private AlRowad_ERPDataSet alRowad_ERPDataSet;
        private System.Windows.Forms.BindingSource currenciesBindingSource;
        private AlRowad_ERPDataSetTableAdapters.CurrenciesTableAdapter currenciesTableAdapter;
        private AlRowad_ERPDataSetTableAdapters.TableAdapterManager tableAdapterManager;
        private System.Windows.Forms.DataGridView currenciesDataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private FontAwesome.Sharp.IconToolStripButton iconToolStripButton1;
        private FontAwesome.Sharp.IconToolStripButton iconToolStripButton2;
    }
}