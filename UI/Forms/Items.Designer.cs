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
            this.itemsDataGridView = new System.Windows.Forms.DataGridView();
            this.item_IDTextBox = new System.Windows.Forms.TextBox();
            this.item_NameTextBox = new System.Windows.Forms.TextBox();
            this.base_Unit_IDTextBox = new System.Windows.Forms.TextBox();
            this.default_PriceTextBox = new System.Windows.Forms.TextBox();
            this.alRowadToolBar1 = new AlRowad_ERP.Controls.AlRowadToolBar();
            item_IDLabel = new System.Windows.Forms.Label();
            item_NameLabel = new System.Windows.Forms.Label();
            base_Unit_IDLabel = new System.Windows.Forms.Label();
            default_PriceLabel = new System.Windows.Forms.Label();
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
            this.itemsDataGridView.Location = new System.Drawing.Point(34, 330);
            this.itemsDataGridView.Name = "itemsDataGridView";
            this.itemsDataGridView.RowHeadersWidth = 62;
            this.itemsDataGridView.RowTemplate.Height = 29;
            this.itemsDataGridView.Size = new System.Drawing.Size(906, 270);
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
            // base_Unit_IDTextBox
            // 
            this.base_Unit_IDTextBox.Location = new System.Drawing.Point(459, 204);
            this.base_Unit_IDTextBox.Name = "base_Unit_IDTextBox";
            this.base_Unit_IDTextBox.Size = new System.Drawing.Size(290, 30);
            this.base_Unit_IDTextBox.TabIndex = 7;
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
            // Items
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1067, 620);
            this.Controls.Add(this.alRowadToolBar1);
            this.Controls.Add(item_IDLabel);
            this.Controls.Add(this.item_IDTextBox);
            this.Controls.Add(item_NameLabel);
            this.Controls.Add(this.item_NameTextBox);
            this.Controls.Add(base_Unit_IDLabel);
            this.Controls.Add(this.base_Unit_IDTextBox);
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
        private System.Windows.Forms.TextBox base_Unit_IDTextBox;
        private System.Windows.Forms.TextBox default_PriceTextBox;
        private Controls.AlRowadToolBar alRowadToolBar1;
    }
}