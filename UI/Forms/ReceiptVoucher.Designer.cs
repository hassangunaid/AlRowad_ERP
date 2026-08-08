namespace AlRowad_ERP.Forms
{
    partial class ReceiptVoucher
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
            System.Windows.Forms.Label voucher_NoLabel;
            System.Windows.Forms.Label voucher_DateLabel;
            System.Windows.Forms.Label doc_Type_IDLabel;
            System.Windows.Forms.Label box_Acc_IDLabel;
            System.Windows.Forms.Label amountLabel;
            System.Windows.Forms.Label cur_IDLabel;
            System.Windows.Forms.Label notesLabel;
            System.Windows.Forms.Label is_PostedLabel;
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.Label label4;
            System.Windows.Forms.Label label3;
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Label label5;
            this.split_RV = new System.Windows.Forms.SplitContainer();
            this.rbtnCash = new System.Windows.Forms.CheckBox();
            this.rbtnBank = new System.Windows.Forms.CheckBox();
            this.voucher_DateDateTimePicker = new AlRowad_ERP.Controls.AlRowadDateTextBox();
            this.amount_ForeignTextBox = new AlRowad_ERP.Controls.AlRowadNumericTextBox();
            this.exchange_RateTextBox = new AlRowad_ERP.Controls.AlRowadNumericTextBox();
            this.amountTextBox = new AlRowad_ERP.Controls.AlRowadNumericTextBox();
            this.voucher_NoTextBox = new System.Windows.Forms.TextBox();
            this.doc_Type_IDComboBox = new System.Windows.Forms.ComboBox();
            this.box_Acc_IDComboBox = new System.Windows.Forms.ComboBox();
            this.cur_IDComboBox = new System.Windows.Forms.ComboBox();
            this.notesTextBox = new System.Windows.Forms.TextBox();
            this.is_PostedCheckBox = new System.Windows.Forms.CheckBox();
            this.grid_Details = new System.Windows.Forms.DataGridView();
            this.Acc_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Acc_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Notes = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Cur_ID = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.Exchange_Rate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Amount_Foreign = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Amount_Credit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Cost_Center_ID = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.AlRowadToolBar = new AlRowad_ERP.Controls.AlRowadToolBar();
            this.label1 = new System.Windows.Forms.Label();
            this.total_grid = new System.Windows.Forms.TextBox();
            this.txt_Difference = new System.Windows.Forms.TextBox();
            this.txt_UpdatedAt = new System.Windows.Forms.TextBox();
            this.txt_UpdatedBy = new System.Windows.Forms.TextBox();
            this.txt_CreatedBy = new System.Windows.Forms.TextBox();
            this.txt_CreatedAt = new System.Windows.Forms.TextBox();
            voucher_NoLabel = new System.Windows.Forms.Label();
            voucher_DateLabel = new System.Windows.Forms.Label();
            doc_Type_IDLabel = new System.Windows.Forms.Label();
            box_Acc_IDLabel = new System.Windows.Forms.Label();
            amountLabel = new System.Windows.Forms.Label();
            cur_IDLabel = new System.Windows.Forms.Label();
            notesLabel = new System.Windows.Forms.Label();
            is_PostedLabel = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.split_RV)).BeginInit();
            this.split_RV.Panel1.SuspendLayout();
            this.split_RV.Panel2.SuspendLayout();
            this.split_RV.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid_Details)).BeginInit();
            this.SuspendLayout();
            // 
            // voucher_NoLabel
            // 
            voucher_NoLabel.AutoSize = true;
            voucher_NoLabel.Location = new System.Drawing.Point(1103, 15);
            voucher_NoLabel.Name = "voucher_NoLabel";
            voucher_NoLabel.Size = new System.Drawing.Size(76, 25);
            voucher_NoLabel.TabIndex = 2;
            voucher_NoLabel.Text = "رقم السند:";
            // 
            // voucher_DateLabel
            // 
            voucher_DateLabel.AutoSize = true;
            voucher_DateLabel.Location = new System.Drawing.Point(404, 12);
            voucher_DateLabel.Name = "voucher_DateLabel";
            voucher_DateLabel.Size = new System.Drawing.Size(70, 25);
            voucher_DateLabel.TabIndex = 4;
            voucher_DateLabel.Text = "التـاريـخ:";
            // 
            // doc_Type_IDLabel
            // 
            doc_Type_IDLabel.AutoSize = true;
            doc_Type_IDLabel.Location = new System.Drawing.Point(839, 15);
            doc_Type_IDLabel.Name = "doc_Type_IDLabel";
            doc_Type_IDLabel.Size = new System.Drawing.Size(78, 25);
            doc_Type_IDLabel.TabIndex = 6;
            doc_Type_IDLabel.Text = "نوع السند:";
            // 
            // box_Acc_IDLabel
            // 
            box_Acc_IDLabel.AutoSize = true;
            box_Acc_IDLabel.Location = new System.Drawing.Point(1103, 64);
            box_Acc_IDLabel.Name = "box_Acc_IDLabel";
            box_Acc_IDLabel.Size = new System.Drawing.Size(121, 25);
            box_Acc_IDLabel.TabIndex = 8;
            box_Acc_IDLabel.Text = "الصندوق / البنك:";
            // 
            // amountLabel
            // 
            amountLabel.AutoSize = true;
            amountLabel.Location = new System.Drawing.Point(1103, 107);
            amountLabel.Name = "amountLabel";
            amountLabel.Size = new System.Drawing.Size(52, 25);
            amountLabel.TabIndex = 12;
            amountLabel.Text = "المبلغ:";
            // 
            // cur_IDLabel
            // 
            cur_IDLabel.AutoSize = true;
            cur_IDLabel.Location = new System.Drawing.Point(774, 64);
            cur_IDLabel.Name = "cur_IDLabel";
            cur_IDLabel.Size = new System.Drawing.Size(54, 25);
            cur_IDLabel.TabIndex = 14;
            cur_IDLabel.Text = "العملة:";
            // 
            // notesLabel
            // 
            notesLabel.AutoSize = true;
            notesLabel.Location = new System.Drawing.Point(779, 107);
            notesLabel.Name = "notesLabel";
            notesLabel.Size = new System.Drawing.Size(60, 25);
            notesLabel.TabIndex = 18;
            notesLabel.Text = "البيــان:";
            // 
            // is_PostedLabel
            // 
            is_PostedLabel.AutoSize = true;
            is_PostedLabel.Location = new System.Drawing.Point(92, 10);
            is_PostedLabel.Name = "is_PostedLabel";
            is_PostedLabel.Size = new System.Drawing.Size(51, 25);
            is_PostedLabel.TabIndex = 20;
            is_PostedLabel.Text = "مرحل";
            // 
            // split_RV
            // 
            this.split_RV.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.split_RV.Location = new System.Drawing.Point(8, 75);
            this.split_RV.Name = "split_RV";
            this.split_RV.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // split_RV.Panel1
            // 
            this.split_RV.Panel1.AutoScroll = true;
            this.split_RV.Panel1.Controls.Add(this.rbtnCash);
            this.split_RV.Panel1.Controls.Add(this.rbtnBank);
            this.split_RV.Panel1.Controls.Add(this.voucher_DateDateTimePicker);
            this.split_RV.Panel1.Controls.Add(this.amount_ForeignTextBox);
            this.split_RV.Panel1.Controls.Add(this.exchange_RateTextBox);
            this.split_RV.Panel1.Controls.Add(this.amountTextBox);
            this.split_RV.Panel1.Controls.Add(voucher_NoLabel);
            this.split_RV.Panel1.Controls.Add(this.voucher_NoTextBox);
            this.split_RV.Panel1.Controls.Add(voucher_DateLabel);
            this.split_RV.Panel1.Controls.Add(doc_Type_IDLabel);
            this.split_RV.Panel1.Controls.Add(this.doc_Type_IDComboBox);
            this.split_RV.Panel1.Controls.Add(box_Acc_IDLabel);
            this.split_RV.Panel1.Controls.Add(this.box_Acc_IDComboBox);
            this.split_RV.Panel1.Controls.Add(amountLabel);
            this.split_RV.Panel1.Controls.Add(cur_IDLabel);
            this.split_RV.Panel1.Controls.Add(this.cur_IDComboBox);
            this.split_RV.Panel1.Controls.Add(notesLabel);
            this.split_RV.Panel1.Controls.Add(this.notesTextBox);
            this.split_RV.Panel1.Controls.Add(is_PostedLabel);
            this.split_RV.Panel1.Controls.Add(this.is_PostedCheckBox);
            this.split_RV.Panel1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            // 
            // split_RV.Panel2
            // 
            this.split_RV.Panel2.Controls.Add(this.grid_Details);
            this.split_RV.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.split_RV.Size = new System.Drawing.Size(1264, 650);
            this.split_RV.SplitterDistance = 158;
            this.split_RV.SplitterWidth = 5;
            this.split_RV.TabIndex = 0;
            // 
            // rbtnCash
            // 
            this.rbtnCash.Location = new System.Drawing.Point(623, 15);
            this.rbtnCash.Name = "rbtnCash";
            this.rbtnCash.Size = new System.Drawing.Size(71, 26);
            this.rbtnCash.TabIndex = 28;
            this.rbtnCash.Text = "نقدي";
            this.rbtnCash.UseVisualStyleBackColor = true;
            // 
            // rbtnBank
            // 
            this.rbtnBank.Location = new System.Drawing.Point(545, 15);
            this.rbtnBank.Name = "rbtnBank";
            this.rbtnBank.Size = new System.Drawing.Size(76, 27);
            this.rbtnBank.TabIndex = 27;
            this.rbtnBank.Text = "بنكي";
            this.rbtnBank.UseVisualStyleBackColor = true;
            // 
            // voucher_DateDateTimePicker
            // 
            this.voucher_DateDateTimePicker.DateValue = new System.DateTime(2026, 7, 20, 0, 0, 0, 0);
            this.voucher_DateDateTimePicker.Location = new System.Drawing.Point(198, 12);
            this.voucher_DateDateTimePicker.MaxLength = 10;
            this.voucher_DateDateTimePicker.Name = "voucher_DateDateTimePicker";
            this.voucher_DateDateTimePicker.Size = new System.Drawing.Size(200, 30);
            this.voucher_DateDateTimePicker.TabIndex = 26;
            this.voucher_DateDateTimePicker.Text = "20/07/2026";
            this.voucher_DateDateTimePicker.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // amount_ForeignTextBox
            // 
            this.amount_ForeignTextBox.FormatCategory = AlRowad_ERP.Core.NumericCategory.AccountingAmount;
            this.amount_ForeignTextBox.Location = new System.Drawing.Point(333, 64);
            this.amount_ForeignTextBox.Name = "amount_ForeignTextBox";
            this.amount_ForeignTextBox.Size = new System.Drawing.Size(155, 30);
            this.amount_ForeignTextBox.TabIndex = 25;
            this.amount_ForeignTextBox.Text = "0.00";
            this.amount_ForeignTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // exchange_RateTextBox
            // 
            this.exchange_RateTextBox.FormatCategory = AlRowad_ERP.Core.NumericCategory.ExchangeRate;
            this.exchange_RateTextBox.Location = new System.Drawing.Point(494, 64);
            this.exchange_RateTextBox.Name = "exchange_RateTextBox";
            this.exchange_RateTextBox.Size = new System.Drawing.Size(155, 30);
            this.exchange_RateTextBox.TabIndex = 24;
            this.exchange_RateTextBox.Text = "0.00";
            this.exchange_RateTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // amountTextBox
            // 
            this.amountTextBox.FormatCategory = AlRowad_ERP.Core.NumericCategory.AccountingAmount;
            this.amountTextBox.Location = new System.Drawing.Point(897, 107);
            this.amountTextBox.Name = "amountTextBox";
            this.amountTextBox.Size = new System.Drawing.Size(200, 30);
            this.amountTextBox.TabIndex = 23;
            this.amountTextBox.Text = "0.00";
            this.amountTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // voucher_NoTextBox
            // 
            this.voucher_NoTextBox.Location = new System.Drawing.Point(997, 12);
            this.voucher_NoTextBox.Name = "voucher_NoTextBox";
            this.voucher_NoTextBox.Size = new System.Drawing.Size(100, 30);
            this.voucher_NoTextBox.TabIndex = 3;
            // 
            // doc_Type_IDComboBox
            // 
            this.doc_Type_IDComboBox.FormattingEnabled = true;
            this.doc_Type_IDComboBox.Location = new System.Drawing.Point(706, 10);
            this.doc_Type_IDComboBox.Name = "doc_Type_IDComboBox";
            this.doc_Type_IDComboBox.Size = new System.Drawing.Size(127, 33);
            this.doc_Type_IDComboBox.TabIndex = 7;
            // 
            // box_Acc_IDComboBox
            // 
            this.box_Acc_IDComboBox.FormattingEnabled = true;
            this.box_Acc_IDComboBox.Location = new System.Drawing.Point(897, 61);
            this.box_Acc_IDComboBox.Name = "box_Acc_IDComboBox";
            this.box_Acc_IDComboBox.Size = new System.Drawing.Size(200, 33);
            this.box_Acc_IDComboBox.TabIndex = 9;
            // 
            // cur_IDComboBox
            // 
            this.cur_IDComboBox.FormattingEnabled = true;
            this.cur_IDComboBox.Location = new System.Drawing.Point(655, 61);
            this.cur_IDComboBox.Name = "cur_IDComboBox";
            this.cur_IDComboBox.Size = new System.Drawing.Size(113, 33);
            this.cur_IDComboBox.TabIndex = 15;
            // 
            // notesTextBox
            // 
            this.notesTextBox.Location = new System.Drawing.Point(333, 107);
            this.notesTextBox.Name = "notesTextBox";
            this.notesTextBox.Size = new System.Drawing.Size(435, 30);
            this.notesTextBox.TabIndex = 19;
            // 
            // is_PostedCheckBox
            // 
            this.is_PostedCheckBox.Location = new System.Drawing.Point(81, 32);
            this.is_PostedCheckBox.Name = "is_PostedCheckBox";
            this.is_PostedCheckBox.Size = new System.Drawing.Size(48, 40);
            this.is_PostedCheckBox.TabIndex = 21;
            this.is_PostedCheckBox.UseVisualStyleBackColor = true;
            // 
            // grid_Details
            // 
            this.grid_Details.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grid_Details.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.grid_Details.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid_Details.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Acc_ID,
            this.Acc_Name,
            this.Notes,
            this.Cur_ID,
            this.Exchange_Rate,
            this.Amount_Foreign,
            this.Amount_Credit,
            this.Cost_Center_ID});
            this.grid_Details.Cursor = System.Windows.Forms.Cursors.Default;
            this.grid_Details.EnableHeadersVisualStyles = false;
            this.grid_Details.GridColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.grid_Details.Location = new System.Drawing.Point(6, 3);
            this.grid_Details.Name = "grid_Details";
            this.grid_Details.RowHeadersVisible = false;
            this.grid_Details.RowHeadersWidth = 62;
            this.grid_Details.RowTemplate.Height = 29;
            this.grid_Details.Size = new System.Drawing.Size(1245, 409);
            this.grid_Details.TabIndex = 0;
            // 
            // Acc_ID
            // 
            this.Acc_ID.HeaderText = "رقم الحساب";
            this.Acc_ID.MinimumWidth = 8;
            this.Acc_ID.Name = "Acc_ID";
            this.Acc_ID.Width = 150;
            // 
            // Acc_Name
            // 
            this.Acc_Name.HeaderText = "اسم الحساب";
            this.Acc_Name.MinimumWidth = 8;
            this.Acc_Name.Name = "Acc_Name";
            this.Acc_Name.Width = 150;
            // 
            // Notes
            // 
            this.Notes.HeaderText = "البيان";
            this.Notes.MinimumWidth = 8;
            this.Notes.Name = "Notes";
            this.Notes.Width = 150;
            // 
            // Cur_ID
            // 
            this.Cur_ID.HeaderText = "العملة";
            this.Cur_ID.MinimumWidth = 8;
            this.Cur_ID.Name = "Cur_ID";
            this.Cur_ID.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Cur_ID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.Cur_ID.Width = 150;
            // 
            // Exchange_Rate
            // 
            this.Exchange_Rate.HeaderText = "سعر الصرف";
            this.Exchange_Rate.MinimumWidth = 8;
            this.Exchange_Rate.Name = "Exchange_Rate";
            this.Exchange_Rate.Width = 150;
            // 
            // Amount_Foreign
            // 
            this.Amount_Foreign.HeaderText = "المبلغ (أجنبي)";
            this.Amount_Foreign.MinimumWidth = 8;
            this.Amount_Foreign.Name = "Amount_Foreign";
            this.Amount_Foreign.Width = 150;
            // 
            // Amount_Credit
            // 
            this.Amount_Credit.HeaderText = "المبلغ(دائن)";
            this.Amount_Credit.MinimumWidth = 8;
            this.Amount_Credit.Name = "Amount_Credit";
            this.Amount_Credit.Width = 150;
            // 
            // Cost_Center_ID
            // 
            this.Cost_Center_ID.HeaderText = "مركز التكلفة";
            this.Cost_Center_ID.MinimumWidth = 8;
            this.Cost_Center_ID.Name = "Cost_Center_ID";
            this.Cost_Center_ID.Width = 150;
            // 
            // AlRowadToolBar
            // 
            this.AlRowadToolBar.BackColor = System.Drawing.Color.Transparent;
            this.AlRowadToolBar.Cursor = System.Windows.Forms.Cursors.PanNW;
            this.AlRowadToolBar.Location = new System.Drawing.Point(178, 17);
            this.AlRowadToolBar.Name = "AlRowadToolBar";
            this.AlRowadToolBar.Size = new System.Drawing.Size(984, 53);
            this.AlRowadToolBar.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(801, 687);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 25);
            this.label1.TabIndex = 2;
            this.label1.Text = "الاجمالي";
            // 
            // total_grid
            // 
            this.total_grid.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.total_grid.ForeColor = System.Drawing.Color.Red;
            this.total_grid.Location = new System.Drawing.Point(927, 687);
            this.total_grid.Name = "total_grid";
            this.total_grid.ReadOnly = true;
            this.total_grid.Size = new System.Drawing.Size(205, 32);
            this.total_grid.TabIndex = 3;
            // 
            // txt_Difference
            // 
            this.txt_Difference.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.txt_Difference.ForeColor = System.Drawing.Color.Lime;
            this.txt_Difference.Location = new System.Drawing.Point(590, 687);
            this.txt_Difference.Name = "txt_Difference";
            this.txt_Difference.ReadOnly = true;
            this.txt_Difference.Size = new System.Drawing.Size(205, 32);
            this.txt_Difference.TabIndex = 4;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Cursor = System.Windows.Forms.Cursors.Default;
            label4.ForeColor = System.Drawing.Color.BlanchedAlmond;
            label4.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            label4.Location = new System.Drawing.Point(198, 849);
            label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            label4.Size = new System.Drawing.Size(107, 25);
            label4.TabIndex = 70;
            label4.Text = "تاريخ التعديل :";
            label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Cursor = System.Windows.Forms.Cursors.Default;
            label3.ForeColor = System.Drawing.Color.BlanchedAlmond;
            label3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            label3.Location = new System.Drawing.Point(705, 805);
            label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            label3.Size = new System.Drawing.Size(105, 25);
            label3.TabIndex = 69;
            label3.Text = "المستـــخــدم : ";
            label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Cursor = System.Windows.Forms.Cursors.Default;
            label2.ForeColor = System.Drawing.Color.BlanchedAlmond;
            label2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            label2.Location = new System.Drawing.Point(705, 849);
            label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            label2.Size = new System.Drawing.Size(105, 25);
            label2.TabIndex = 68;
            label2.Text = "المستـــخــدم : ";
            label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Cursor = System.Windows.Forms.Cursors.Default;
            label5.ForeColor = System.Drawing.Color.BlanchedAlmond;
            label5.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            label5.Location = new System.Drawing.Point(198, 805);
            label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label5.Name = "label5";
            label5.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            label5.Size = new System.Drawing.Size(108, 25);
            label5.TabIndex = 67;
            label5.Text = "تاريخ الانشاء :";
            label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txt_UpdatedAt
            // 
            this.txt_UpdatedAt.Location = new System.Drawing.Point(342, 846);
            this.txt_UpdatedAt.Name = "txt_UpdatedAt";
            this.txt_UpdatedAt.Size = new System.Drawing.Size(336, 30);
            this.txt_UpdatedAt.TabIndex = 66;
            // 
            // txt_UpdatedBy
            // 
            this.txt_UpdatedBy.Location = new System.Drawing.Point(820, 846);
            this.txt_UpdatedBy.Name = "txt_UpdatedBy";
            this.txt_UpdatedBy.Size = new System.Drawing.Size(336, 30);
            this.txt_UpdatedBy.TabIndex = 65;
            // 
            // txt_CreatedBy
            // 
            this.txt_CreatedBy.Location = new System.Drawing.Point(820, 802);
            this.txt_CreatedBy.Name = "txt_CreatedBy";
            this.txt_CreatedBy.Size = new System.Drawing.Size(336, 30);
            this.txt_CreatedBy.TabIndex = 64;
            // 
            // txt_CreatedAt
            // 
            this.txt_CreatedAt.Location = new System.Drawing.Point(342, 802);
            this.txt_CreatedAt.Name = "txt_CreatedAt";
            this.txt_CreatedAt.Size = new System.Drawing.Size(336, 30);
            this.txt_CreatedAt.TabIndex = 63;
            // 
            // ReceiptVoucher
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.MediumBlue;
            this.ClientSize = new System.Drawing.Size(1278, 994);
            this.Controls.Add(label4);
            this.Controls.Add(label3);
            this.Controls.Add(label2);
            this.Controls.Add(label5);
            this.Controls.Add(this.txt_UpdatedAt);
            this.Controls.Add(this.txt_UpdatedBy);
            this.Controls.Add(this.txt_CreatedBy);
            this.Controls.Add(this.txt_CreatedAt);
            this.Controls.Add(this.txt_Difference);
            this.Controls.Add(this.total_grid);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.AlRowadToolBar);
            this.Controls.Add(this.split_RV);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ReceiptVoucher";
            this.Text = "سند قبض";
            this.split_RV.Panel1.ResumeLayout(false);
            this.split_RV.Panel1.PerformLayout();
            this.split_RV.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.split_RV)).EndInit();
            this.split_RV.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid_Details)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer split_RV;
        private System.Windows.Forms.TextBox voucher_NoTextBox;
        private System.Windows.Forms.ComboBox doc_Type_IDComboBox;
        private System.Windows.Forms.ComboBox box_Acc_IDComboBox;
        private System.Windows.Forms.ComboBox cur_IDComboBox;
        private System.Windows.Forms.TextBox notesTextBox;
        private System.Windows.Forms.CheckBox is_PostedCheckBox;
        private Controls.AlRowadToolBar AlRowadToolBar;
        private System.Windows.Forms.DataGridView grid_Details;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox total_grid;
        private System.Windows.Forms.DataGridViewTextBoxColumn Acc_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Acc_Name;
        private System.Windows.Forms.DataGridViewTextBoxColumn Notes;
        private System.Windows.Forms.DataGridViewComboBoxColumn Cur_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Exchange_Rate;
        private System.Windows.Forms.DataGridViewTextBoxColumn Amount_Foreign;
        private System.Windows.Forms.DataGridViewTextBoxColumn Amount_Credit;
        private System.Windows.Forms.DataGridViewComboBoxColumn Cost_Center_ID;
        private System.Windows.Forms.TextBox txt_Difference;
        private Controls.AlRowadNumericTextBox amountTextBox;
        private Controls.AlRowadNumericTextBox exchange_RateTextBox;
        private Controls.AlRowadNumericTextBox amount_ForeignTextBox;
        private Controls.AlRowadDateTextBox voucher_DateDateTimePicker;
        private System.Windows.Forms.CheckBox rbtnCash;
        private System.Windows.Forms.CheckBox rbtnBank;
        private System.Windows.Forms.TextBox txt_UpdatedAt;
        private System.Windows.Forms.TextBox txt_UpdatedBy;
        private System.Windows.Forms.TextBox txt_CreatedBy;
        private System.Windows.Forms.TextBox txt_CreatedAt;
    }
}