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
            System.Windows.Forms.Label cur_IDLabel;
            System.Windows.Forms.Label cur_NameLabel;
            System.Windows.Forms.Label cur_SymbolLabel;
            System.Windows.Forms.Label label1;
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Label label4;
            System.Windows.Forms.Label label3;
            System.Windows.Forms.Label label5;
            System.Windows.Forms.Label label6;
            this.iconToolStripButton1 = new FontAwesome.Sharp.IconToolStripButton();
            this.currenciesDataGridView = new System.Windows.Forms.DataGridView();
            this.cur_IDTextBox = new System.Windows.Forms.TextBox();
            this.cur_NameTextBox = new System.Windows.Forms.TextBox();
            this.exchange_RateTextBox = new System.Windows.Forms.TextBox();
            this.cur_SymbolTextBox = new System.Windows.Forms.TextBox();
            this.is_ActiveCheckBox = new System.Windows.Forms.CheckBox();
            this.alRowadToolBar1 = new AlRowad_ERP.Controls.AlRowadToolBar();
            this.iconToolStripButton2 = new FontAwesome.Sharp.IconToolStripButton();
            this.max_RateTextBox = new System.Windows.Forms.TextBox();
            this.min_RateTextBox = new System.Windows.Forms.TextBox();
            this.chk_IsBaseCurrency = new System.Windows.Forms.CheckBox();
            this.chk_IsLocalCurrency = new System.Windows.Forms.CheckBox();
            this.txt_UpdatedAt = new System.Windows.Forms.TextBox();
            this.txt_UpdatedBy = new System.Windows.Forms.TextBox();
            this.txt_CreatedBy = new System.Windows.Forms.TextBox();
            this.txt_CreatedAt = new System.Windows.Forms.TextBox();
            this.Cur_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Exchange_Rate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Min_Exchange_Rate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Max_Exchange_Rate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Change_Date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Modified_By_Col = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Cur_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            cur_IDLabel = new System.Windows.Forms.Label();
            cur_NameLabel = new System.Windows.Forms.Label();
            cur_SymbolLabel = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label5 = new System.Windows.Forms.Label();
            label6 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.currenciesDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // cur_IDLabel
            // 
            cur_IDLabel.AutoSize = true;
            cur_IDLabel.Location = new System.Drawing.Point(129, 171);
            cur_IDLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            cur_IDLabel.Name = "cur_IDLabel";
            cur_IDLabel.Size = new System.Drawing.Size(81, 25);
            cur_IDLabel.TabIndex = 1;
            cur_IDLabel.Text = "رقم العملة:";
            // 
            // cur_NameLabel
            // 
            cur_NameLabel.AutoSize = true;
            cur_NameLabel.Location = new System.Drawing.Point(129, 214);
            cur_NameLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            cur_NameLabel.Name = "cur_NameLabel";
            cur_NameLabel.Size = new System.Drawing.Size(81, 25);
            cur_NameLabel.TabIndex = 3;
            cur_NameLabel.Text = "اسم العملة:";
            // 
            // cur_SymbolLabel
            // 
            cur_SymbolLabel.AutoSize = true;
            cur_SymbolLabel.Location = new System.Drawing.Point(123, 255);
            cur_SymbolLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            cur_SymbolLabel.Name = "cur_SymbolLabel";
            cur_SymbolLabel.Size = new System.Drawing.Size(87, 25);
            cur_SymbolLabel.TabIndex = 7;
            cur_SymbolLabel.Text = "رمز العملة:";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(119, 334);
            label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(91, 25);
            label1.TabIndex = 14;
            label1.Text = "الحد الاعلى:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(122, 295);
            label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(88, 25);
            label2.TabIndex = 15;
            label2.Text = "الحد الادني:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Cursor = System.Windows.Forms.Cursors.Default;
            label4.ForeColor = System.Drawing.Color.BlanchedAlmond;
            label4.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            label4.Location = new System.Drawing.Point(228, 900);
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
            label3.Location = new System.Drawing.Point(735, 856);
            label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            label3.Size = new System.Drawing.Size(105, 25);
            label3.TabIndex = 61;
            label3.Text = "المستـــخــدم : ";
            label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Cursor = System.Windows.Forms.Cursors.Default;
            label5.ForeColor = System.Drawing.Color.BlanchedAlmond;
            label5.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            label5.Location = new System.Drawing.Point(735, 900);
            label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label5.Name = "label5";
            label5.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            label5.Size = new System.Drawing.Size(105, 25);
            label5.TabIndex = 60;
            label5.Text = "المستـــخــدم : ";
            label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Cursor = System.Windows.Forms.Cursors.Default;
            label6.ForeColor = System.Drawing.Color.BlanchedAlmond;
            label6.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            label6.Location = new System.Drawing.Point(228, 856);
            label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label6.Name = "label6";
            label6.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            label6.Size = new System.Drawing.Size(108, 25);
            label6.TabIndex = 59;
            label6.Text = "تاريخ الانشاء :";
            label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
            // currenciesDataGridView
            // 
            this.currenciesDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.currenciesDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Cur_Name,
            this.Exchange_Rate,
            this.Min_Exchange_Rate,
            this.Max_Exchange_Rate,
            this.Change_Date,
            this.Modified_By_Col,
            this.Cur_ID});
            this.currenciesDataGridView.EnableHeadersVisualStyles = false;
            this.currenciesDataGridView.Location = new System.Drawing.Point(19, 395);
            this.currenciesDataGridView.Margin = new System.Windows.Forms.Padding(4);
            this.currenciesDataGridView.Name = "currenciesDataGridView";
            this.currenciesDataGridView.RowHeadersVisible = false;
            this.currenciesDataGridView.RowHeadersWidth = 62;
            this.currenciesDataGridView.RowTemplate.Height = 29;
            this.currenciesDataGridView.Size = new System.Drawing.Size(1231, 357);
            this.currenciesDataGridView.TabIndex = 0;
            // 
            // cur_IDTextBox
            // 
            this.cur_IDTextBox.Location = new System.Drawing.Point(221, 167);
            this.cur_IDTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.cur_IDTextBox.Name = "cur_IDTextBox";
            this.cur_IDTextBox.Size = new System.Drawing.Size(79, 30);
            this.cur_IDTextBox.TabIndex = 2;
            // 
            // cur_NameTextBox
            // 
            this.cur_NameTextBox.Location = new System.Drawing.Point(221, 211);
            this.cur_NameTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.cur_NameTextBox.Name = "cur_NameTextBox";
            this.cur_NameTextBox.Size = new System.Drawing.Size(117, 30);
            this.cur_NameTextBox.TabIndex = 4;
            // 
            // exchange_RateTextBox
            // 
            this.exchange_RateTextBox.Location = new System.Drawing.Point(422, 211);
            this.exchange_RateTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.exchange_RateTextBox.Name = "exchange_RateTextBox";
            this.exchange_RateTextBox.Size = new System.Drawing.Size(301, 30);
            this.exchange_RateTextBox.TabIndex = 6;
            // 
            // cur_SymbolTextBox
            // 
            this.cur_SymbolTextBox.Location = new System.Drawing.Point(221, 252);
            this.cur_SymbolTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.cur_SymbolTextBox.Name = "cur_SymbolTextBox";
            this.cur_SymbolTextBox.Size = new System.Drawing.Size(301, 30);
            this.cur_SymbolTextBox.TabIndex = 8;
            // 
            // is_ActiveCheckBox
            // 
            this.is_ActiveCheckBox.Location = new System.Drawing.Point(584, 171);
            this.is_ActiveCheckBox.Margin = new System.Windows.Forms.Padding(4);
            this.is_ActiveCheckBox.Name = "is_ActiveCheckBox";
            this.is_ActiveCheckBox.Size = new System.Drawing.Size(139, 32);
            this.is_ActiveCheckBox.TabIndex = 10;
            this.is_ActiveCheckBox.Text = "ايقاف";
            this.is_ActiveCheckBox.UseVisualStyleBackColor = true;
            // 
            // alRowadToolBar1
            // 
            this.alRowadToolBar1.BackColor = System.Drawing.Color.Transparent;
            this.alRowadToolBar1.Cursor = System.Windows.Forms.Cursors.PanNW;
            this.alRowadToolBar1.Location = new System.Drawing.Point(2, 64);
            this.alRowadToolBar1.Margin = new System.Windows.Forms.Padding(4);
            this.alRowadToolBar1.Name = "alRowadToolBar1";
            this.alRowadToolBar1.Size = new System.Drawing.Size(1232, 55);
            this.alRowadToolBar1.TabIndex = 11;
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
            // max_RateTextBox
            // 
            this.max_RateTextBox.Location = new System.Drawing.Point(221, 332);
            this.max_RateTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.max_RateTextBox.Name = "max_RateTextBox";
            this.max_RateTextBox.Size = new System.Drawing.Size(301, 30);
            this.max_RateTextBox.TabIndex = 12;
            // 
            // min_RateTextBox
            // 
            this.min_RateTextBox.Location = new System.Drawing.Point(221, 292);
            this.min_RateTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.min_RateTextBox.Name = "min_RateTextBox";
            this.min_RateTextBox.Size = new System.Drawing.Size(301, 30);
            this.min_RateTextBox.TabIndex = 13;
            // 
            // chk_IsBaseCurrency
            // 
            this.chk_IsBaseCurrency.Location = new System.Drawing.Point(356, 168);
            this.chk_IsBaseCurrency.Margin = new System.Windows.Forms.Padding(4);
            this.chk_IsBaseCurrency.Name = "chk_IsBaseCurrency";
            this.chk_IsBaseCurrency.Size = new System.Drawing.Size(166, 32);
            this.chk_IsBaseCurrency.TabIndex = 16;
            this.chk_IsBaseCurrency.Text = "العملة الافتراضية";
            this.chk_IsBaseCurrency.UseVisualStyleBackColor = true;
            // 
            // chk_IsLocalCurrency
            // 
            this.chk_IsLocalCurrency.Location = new System.Drawing.Point(570, 295);
            this.chk_IsLocalCurrency.Margin = new System.Windows.Forms.Padding(4);
            this.chk_IsLocalCurrency.Name = "chk_IsLocalCurrency";
            this.chk_IsLocalCurrency.Size = new System.Drawing.Size(166, 32);
            this.chk_IsLocalCurrency.TabIndex = 17;
            this.chk_IsLocalCurrency.Text = "محلية";
            this.chk_IsLocalCurrency.UseVisualStyleBackColor = true;
            // 
            // txt_UpdatedAt
            // 
            this.txt_UpdatedAt.Location = new System.Drawing.Point(372, 897);
            this.txt_UpdatedAt.Name = "txt_UpdatedAt";
            this.txt_UpdatedAt.Size = new System.Drawing.Size(336, 30);
            this.txt_UpdatedAt.TabIndex = 58;
            // 
            // txt_UpdatedBy
            // 
            this.txt_UpdatedBy.Location = new System.Drawing.Point(850, 897);
            this.txt_UpdatedBy.Name = "txt_UpdatedBy";
            this.txt_UpdatedBy.Size = new System.Drawing.Size(336, 30);
            this.txt_UpdatedBy.TabIndex = 57;
            // 
            // txt_CreatedBy
            // 
            this.txt_CreatedBy.Location = new System.Drawing.Point(850, 853);
            this.txt_CreatedBy.Name = "txt_CreatedBy";
            this.txt_CreatedBy.Size = new System.Drawing.Size(336, 30);
            this.txt_CreatedBy.TabIndex = 56;
            // 
            // txt_CreatedAt
            // 
            this.txt_CreatedAt.Location = new System.Drawing.Point(372, 853);
            this.txt_CreatedAt.Name = "txt_CreatedAt";
            this.txt_CreatedAt.Size = new System.Drawing.Size(336, 30);
            this.txt_CreatedAt.TabIndex = 55;
            // 
            // Cur_Name
            // 
            this.Cur_Name.HeaderText = "اسم العملة";
            this.Cur_Name.MinimumWidth = 8;
            this.Cur_Name.Name = "Cur_Name";
            this.Cur_Name.Width = 150;
            // 
            // Exchange_Rate
            // 
            this.Exchange_Rate.HeaderText = "سعر الصرف";
            this.Exchange_Rate.MinimumWidth = 8;
            this.Exchange_Rate.Name = "Exchange_Rate";
            this.Exchange_Rate.Width = 150;
            // 
            // Min_Exchange_Rate
            // 
            this.Min_Exchange_Rate.HeaderText = "الحد الادنى لسعر الصرف";
            this.Min_Exchange_Rate.MinimumWidth = 8;
            this.Min_Exchange_Rate.Name = "Min_Exchange_Rate";
            this.Min_Exchange_Rate.Width = 150;
            // 
            // Max_Exchange_Rate
            // 
            this.Max_Exchange_Rate.HeaderText = "الحد الاعلى لسعر الصرف";
            this.Max_Exchange_Rate.MinimumWidth = 8;
            this.Max_Exchange_Rate.Name = "Max_Exchange_Rate";
            this.Max_Exchange_Rate.Width = 150;
            // 
            // Change_Date
            // 
            this.Change_Date.HeaderText = "تاريخ التعديل";
            this.Change_Date.MinimumWidth = 8;
            this.Change_Date.Name = "Change_Date";
            this.Change_Date.Width = 150;
            // 
            // Modified_By_Col
            // 
            this.Modified_By_Col.DataPropertyName = "ModifiedBy";
            this.Modified_By_Col.HeaderText = "اسم المستخدم";
            this.Modified_By_Col.MinimumWidth = 8;
            this.Modified_By_Col.Name = "Modified_By_Col";
            this.Modified_By_Col.Width = 150;
            // 
            // Cur_ID
            // 
            this.Cur_ID.HeaderText = "رقم العملة";
            this.Cur_ID.MinimumWidth = 8;
            this.Cur_ID.Name = "Cur_ID";
            this.Cur_ID.Visible = false;
            this.Cur_ID.Width = 150;
            // 
            // Currencies
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.MediumBlue;
            this.ClientSize = new System.Drawing.Size(1304, 979);
            this.Controls.Add(label4);
            this.Controls.Add(label3);
            this.Controls.Add(label5);
            this.Controls.Add(label6);
            this.Controls.Add(this.txt_UpdatedAt);
            this.Controls.Add(this.txt_UpdatedBy);
            this.Controls.Add(this.txt_CreatedBy);
            this.Controls.Add(this.txt_CreatedAt);
            this.Controls.Add(this.chk_IsLocalCurrency);
            this.Controls.Add(this.chk_IsBaseCurrency);
            this.Controls.Add(label2);
            this.Controls.Add(label1);
            this.Controls.Add(this.min_RateTextBox);
            this.Controls.Add(this.max_RateTextBox);
            this.Controls.Add(this.alRowadToolBar1);
            this.Controls.Add(cur_IDLabel);
            this.Controls.Add(this.cur_IDTextBox);
            this.Controls.Add(cur_NameLabel);
            this.Controls.Add(this.cur_NameTextBox);
            this.Controls.Add(this.exchange_RateTextBox);
            this.Controls.Add(cur_SymbolLabel);
            this.Controls.Add(this.cur_SymbolTextBox);
            this.Controls.Add(this.is_ActiveCheckBox);
            this.Controls.Add(this.currenciesDataGridView);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Currencies";
            this.Text = "تهيئة العملات";
            this.Load += new System.EventHandler(this.CurrenciesForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.currenciesDataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private FontAwesome.Sharp.IconToolStripButton iconToolStripButton1;
        private System.Windows.Forms.DataGridView currenciesDataGridView;
        private System.Windows.Forms.TextBox cur_IDTextBox;
        private System.Windows.Forms.TextBox cur_NameTextBox;
        private System.Windows.Forms.TextBox exchange_RateTextBox;
        private System.Windows.Forms.TextBox cur_SymbolTextBox;
        private System.Windows.Forms.CheckBox is_ActiveCheckBox;
        private Controls.AlRowadToolBar alRowadToolBar1;
        private FontAwesome.Sharp.IconToolStripButton iconToolStripButton2;
        private System.Windows.Forms.TextBox max_RateTextBox;
        private System.Windows.Forms.TextBox min_RateTextBox;
        private System.Windows.Forms.CheckBox chk_IsBaseCurrency;
        private System.Windows.Forms.CheckBox chk_IsLocalCurrency;
        private System.Windows.Forms.TextBox txt_UpdatedAt;
        private System.Windows.Forms.TextBox txt_UpdatedBy;
        private System.Windows.Forms.TextBox txt_CreatedBy;
        private System.Windows.Forms.TextBox txt_CreatedAt;
        private System.Windows.Forms.DataGridViewTextBoxColumn Cur_Name;
        private System.Windows.Forms.DataGridViewTextBoxColumn Exchange_Rate;
        private System.Windows.Forms.DataGridViewTextBoxColumn Min_Exchange_Rate;
        private System.Windows.Forms.DataGridViewTextBoxColumn Max_Exchange_Rate;
        private System.Windows.Forms.DataGridViewTextBoxColumn Change_Date;
        private System.Windows.Forms.DataGridViewTextBoxColumn Modified_By_Col;
        private System.Windows.Forms.DataGridViewTextBoxColumn Cur_ID;
    }
}