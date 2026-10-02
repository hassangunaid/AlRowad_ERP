namespace AlRowad_ERP.Forms
{
    partial class Items
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
            System.Windows.Forms.Label item_IDLabel;
            System.Windows.Forms.Label item_NameLabel;
            System.Windows.Forms.Label base_Unit_IDLabel;
            System.Windows.Forms.Label default_PriceLabel;
            System.Windows.Forms.Label label4;
            System.Windows.Forms.Label label3;
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Label label1;
            this.itemsDataGridView = new System.Windows.Forms.DataGridView();
            this.item_IDTextBox = new System.Windows.Forms.TextBox();
            this.item_NameTextBox = new System.Windows.Forms.TextBox();
            this.default_PriceTextBox = new System.Windows.Forms.TextBox();
            this.alRowadToolBar1 = new AlRowad_ERP.Controls.AlRowadToolBar();
            this.cmb_BaseUnit = new System.Windows.Forms.ComboBox();
            this.txt_UpdatedAt = new System.Windows.Forms.TextBox();
            this.txt_UpdatedBy = new System.Windows.Forms.TextBox();
            this.txt_CreatedBy = new System.Windows.Forms.TextBox();
            this.txt_CreatedAt = new System.Windows.Forms.TextBox();
            item_IDLabel = new System.Windows.Forms.Label();
            item_NameLabel = new System.Windows.Forms.Label();
            base_Unit_IDLabel = new System.Windows.Forms.Label();
            default_PriceLabel = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.itemsDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // item_IDLabel
            // 
            item_IDLabel.AutoSize = true;
            item_IDLabel.Location = new System.Drawing.Point(311, 132);
            item_IDLabel.Name = "item_IDLabel";
            item_IDLabel.Size = new System.Drawing.Size(79, 25);
            item_IDLabel.TabIndex = 2;
            item_IDLabel.Text = "Item ID:";
            // 
            // item_NameLabel
            // 
            item_NameLabel.AutoSize = true;
            item_NameLabel.Location = new System.Drawing.Point(311, 168);
            item_NameLabel.Name = "item_NameLabel";
            item_NameLabel.Size = new System.Drawing.Size(112, 25);
            item_NameLabel.TabIndex = 4;
            item_NameLabel.Text = "Item Name:";
            // 
            // base_Unit_IDLabel
            // 
            base_Unit_IDLabel.AutoSize = true;
            base_Unit_IDLabel.Location = new System.Drawing.Point(311, 204);
            base_Unit_IDLabel.Name = "base_Unit_IDLabel";
            base_Unit_IDLabel.Size = new System.Drawing.Size(126, 25);
            base_Unit_IDLabel.TabIndex = 6;
            base_Unit_IDLabel.Text = "Base Unit ID:";
            // 
            // default_PriceLabel
            // 
            default_PriceLabel.AutoSize = true;
            default_PriceLabel.Location = new System.Drawing.Point(311, 240);
            default_PriceLabel.Name = "default_PriceLabel";
            default_PriceLabel.Size = new System.Drawing.Size(128, 25);
            default_PriceLabel.TabIndex = 8;
            default_PriceLabel.Text = "Default Price:";
            // 
            // itemsDataGridView
            // 
            this.itemsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.itemsDataGridView.Location = new System.Drawing.Point(170, 276);
            this.itemsDataGridView.Name = "itemsDataGridView";
            this.itemsDataGridView.RowHeadersWidth = 62;
            this.itemsDataGridView.RowTemplate.Height = 29;
            this.itemsDataGridView.Size = new System.Drawing.Size(906, 323);
            this.itemsDataGridView.TabIndex = 1;
            // 
            // item_IDTextBox
            // 
            this.item_IDTextBox.Location = new System.Drawing.Point(459, 132);
            this.item_IDTextBox.Name = "item_IDTextBox";
            this.item_IDTextBox.Size = new System.Drawing.Size(290, 30);
            this.item_IDTextBox.TabIndex = 3;
            // 
            // item_NameTextBox
            // 
            this.item_NameTextBox.Location = new System.Drawing.Point(459, 168);
            this.item_NameTextBox.Name = "item_NameTextBox";
            this.item_NameTextBox.Size = new System.Drawing.Size(290, 30);
            this.item_NameTextBox.TabIndex = 5;
            // 
            // default_PriceTextBox
            // 
            this.default_PriceTextBox.Location = new System.Drawing.Point(459, 240);
            this.default_PriceTextBox.Name = "default_PriceTextBox";
            this.default_PriceTextBox.Size = new System.Drawing.Size(290, 30);
            this.default_PriceTextBox.TabIndex = 9;
            // 
            // alRowadToolBar1
            // 
            this.alRowadToolBar1.BackColor = System.Drawing.Color.Transparent;
            this.alRowadToolBar1.Cursor = System.Windows.Forms.Cursors.PanNW;
            this.alRowadToolBar1.Location = new System.Drawing.Point(12, 34);
            this.alRowadToolBar1.Name = "alRowadToolBar1";
            this.alRowadToolBar1.Size = new System.Drawing.Size(1014, 53);
            this.alRowadToolBar1.TabIndex = 10;
            // 
            // cmb_BaseUnit
            // 
            this.cmb_BaseUnit.FormattingEnabled = true;
            this.cmb_BaseUnit.Location = new System.Drawing.Point(459, 201);
            this.cmb_BaseUnit.Name = "cmb_BaseUnit";
            this.cmb_BaseUnit.Size = new System.Drawing.Size(290, 33);
            this.cmb_BaseUnit.TabIndex = 11;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Cursor = System.Windows.Forms.Cursors.Default;
            label4.ForeColor = System.Drawing.Color.BlanchedAlmond;
            label4.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            label4.Location = new System.Drawing.Point(96, 695);
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
            label3.Location = new System.Drawing.Point(603, 651);
            label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            label3.Size = new System.Drawing.Size(105, 25);
            label3.TabIndex = 61;
            label3.Text = "المستـــخــدم : ";
            label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Cursor = System.Windows.Forms.Cursors.Default;
            label2.ForeColor = System.Drawing.Color.BlanchedAlmond;
            label2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            label2.Location = new System.Drawing.Point(603, 695);
            label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            label2.Size = new System.Drawing.Size(105, 25);
            label2.TabIndex = 60;
            label2.Text = "المستـــخــدم : ";
            label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Cursor = System.Windows.Forms.Cursors.Default;
            label1.ForeColor = System.Drawing.Color.BlanchedAlmond;
            label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            label1.Location = new System.Drawing.Point(96, 651);
            label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            label1.Size = new System.Drawing.Size(108, 25);
            label1.TabIndex = 59;
            label1.Text = "تاريخ الانشاء :";
            label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txt_UpdatedAt
            // 
            this.txt_UpdatedAt.Location = new System.Drawing.Point(240, 692);
            this.txt_UpdatedAt.Name = "txt_UpdatedAt";
            this.txt_UpdatedAt.Size = new System.Drawing.Size(336, 30);
            this.txt_UpdatedAt.TabIndex = 58;
            this.txt_UpdatedAt.Tag = "Updated_At";
            // 
            // txt_UpdatedBy
            // 
            this.txt_UpdatedBy.Location = new System.Drawing.Point(718, 692);
            this.txt_UpdatedBy.Name = "txt_UpdatedBy";
            this.txt_UpdatedBy.Size = new System.Drawing.Size(336, 30);
            this.txt_UpdatedBy.TabIndex = 57;
            this.txt_UpdatedBy.Tag = "Updated_By";
            // 
            // txt_CreatedBy
            // 
            this.txt_CreatedBy.Location = new System.Drawing.Point(718, 648);
            this.txt_CreatedBy.Name = "txt_CreatedBy";
            this.txt_CreatedBy.Size = new System.Drawing.Size(336, 30);
            this.txt_CreatedBy.TabIndex = 56;
            this.txt_CreatedBy.Tag = "Created_By";
            // 
            // txt_CreatedAt
            // 
            this.txt_CreatedAt.Location = new System.Drawing.Point(240, 648);
            this.txt_CreatedAt.Name = "txt_CreatedAt";
            this.txt_CreatedAt.Size = new System.Drawing.Size(336, 30);
            this.txt_CreatedAt.TabIndex = 55;
            this.txt_CreatedAt.Tag = "Created_At";
            // 
            // Items
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1178, 744);
            this.Controls.Add(label4);
            this.Controls.Add(label3);
            this.Controls.Add(label2);
            this.Controls.Add(label1);
            this.Controls.Add(this.txt_UpdatedAt);
            this.Controls.Add(this.txt_UpdatedBy);
            this.Controls.Add(this.txt_CreatedBy);
            this.Controls.Add(this.txt_CreatedAt);
            this.Controls.Add(this.cmb_BaseUnit);
            this.Controls.Add(this.alRowadToolBar1);
            this.Controls.Add(item_IDLabel);
            this.Controls.Add(this.item_IDTextBox);
            this.Controls.Add(item_NameLabel);
            this.Controls.Add(this.item_NameTextBox);
            this.Controls.Add(base_Unit_IDLabel);
            this.Controls.Add(default_PriceLabel);
            this.Controls.Add(this.default_PriceTextBox);
            this.Controls.Add(this.itemsDataGridView);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Items";
            this.Text = "بيانات الاصناف";
            this.Load += new System.EventHandler(this.Items_Load);
            ((System.ComponentModel.ISupportInitialize)(this.itemsDataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.DataGridView itemsDataGridView;
        private System.Windows.Forms.TextBox item_IDTextBox;
        private System.Windows.Forms.TextBox item_NameTextBox;
        private System.Windows.Forms.TextBox default_PriceTextBox;
        private Controls.AlRowadToolBar alRowadToolBar1;
        private System.Windows.Forms.ComboBox cmb_BaseUnit;
        private System.Windows.Forms.TextBox txt_UpdatedAt;
        private System.Windows.Forms.TextBox txt_UpdatedBy;
        private System.Windows.Forms.TextBox txt_CreatedBy;
        private System.Windows.Forms.TextBox txt_CreatedAt;
    }
}