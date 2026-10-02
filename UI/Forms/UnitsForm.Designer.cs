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
            System.Windows.Forms.Label unit_IDLabel;
            System.Windows.Forms.Label unit_NameLabel;
            System.Windows.Forms.Label conversion_FactorLabel;
            System.Windows.Forms.Label label4;
            System.Windows.Forms.Label label3;
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Label label1;
            this.unit_IDTextBox = new System.Windows.Forms.TextBox();
            this.unit_NameTextBox = new System.Windows.Forms.TextBox();
            this.conversion_FactorTextBox = new System.Windows.Forms.TextBox();
            this.unitsDataGridView = new System.Windows.Forms.DataGridView();
            this.alRowadToolBar1 = new AlRowad_ERP.Controls.AlRowadToolBar();
            this.txt_UpdatedAt = new System.Windows.Forms.TextBox();
            this.txt_UpdatedBy = new System.Windows.Forms.TextBox();
            this.txt_CreatedBy = new System.Windows.Forms.TextBox();
            this.txt_CreatedAt = new System.Windows.Forms.TextBox();
            unit_IDLabel = new System.Windows.Forms.Label();
            unit_NameLabel = new System.Windows.Forms.Label();
            conversion_FactorLabel = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.unitsDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // unit_IDLabel
            // 
            unit_IDLabel.AutoSize = true;
            unit_IDLabel.Location = new System.Drawing.Point(311, 93);
            unit_IDLabel.Name = "unit_IDLabel";
            unit_IDLabel.Size = new System.Drawing.Size(76, 25);
            unit_IDLabel.TabIndex = 1;
            unit_IDLabel.Text = "Unit ID:";
            // 
            // unit_NameLabel
            // 
            unit_NameLabel.AutoSize = true;
            unit_NameLabel.Location = new System.Drawing.Point(311, 129);
            unit_NameLabel.Name = "unit_NameLabel";
            unit_NameLabel.Size = new System.Drawing.Size(109, 25);
            unit_NameLabel.TabIndex = 3;
            unit_NameLabel.Text = "Unit Name:";
            // 
            // conversion_FactorLabel
            // 
            conversion_FactorLabel.AutoSize = true;
            conversion_FactorLabel.Location = new System.Drawing.Point(311, 165);
            conversion_FactorLabel.Name = "conversion_FactorLabel";
            conversion_FactorLabel.Size = new System.Drawing.Size(178, 25);
            conversion_FactorLabel.TabIndex = 5;
            conversion_FactorLabel.Text = "Conversion Factor:";
            // 
            // unit_IDTextBox
            // 
            this.unit_IDTextBox.Location = new System.Drawing.Point(526, 93);
            this.unit_IDTextBox.Name = "unit_IDTextBox";
            this.unit_IDTextBox.Size = new System.Drawing.Size(257, 30);
            this.unit_IDTextBox.TabIndex = 2;
            // 
            // unit_NameTextBox
            // 
            this.unit_NameTextBox.Location = new System.Drawing.Point(526, 129);
            this.unit_NameTextBox.Name = "unit_NameTextBox";
            this.unit_NameTextBox.Size = new System.Drawing.Size(257, 30);
            this.unit_NameTextBox.TabIndex = 4;
            // 
            // conversion_FactorTextBox
            // 
            this.conversion_FactorTextBox.Location = new System.Drawing.Point(526, 165);
            this.conversion_FactorTextBox.Name = "conversion_FactorTextBox";
            this.conversion_FactorTextBox.Size = new System.Drawing.Size(257, 30);
            this.conversion_FactorTextBox.TabIndex = 6;
            // 
            // unitsDataGridView
            // 
            this.unitsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.unitsDataGridView.Location = new System.Drawing.Point(28, 217);
            this.unitsDataGridView.Name = "unitsDataGridView";
            this.unitsDataGridView.RowHeadersWidth = 62;
            this.unitsDataGridView.RowTemplate.Height = 29;
            this.unitsDataGridView.Size = new System.Drawing.Size(969, 420);
            this.unitsDataGridView.TabIndex = 7;
            // 
            // alRowadToolBar1
            // 
            this.alRowadToolBar1.BackColor = System.Drawing.Color.Transparent;
            this.alRowadToolBar1.Cursor = System.Windows.Forms.Cursors.PanNW;
            this.alRowadToolBar1.Location = new System.Drawing.Point(64, 22);
            this.alRowadToolBar1.Name = "alRowadToolBar1";
            this.alRowadToolBar1.Size = new System.Drawing.Size(1014, 53);
            this.alRowadToolBar1.TabIndex = 8;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Cursor = System.Windows.Forms.Cursors.Default;
            label4.ForeColor = System.Drawing.Color.BlanchedAlmond;
            label4.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            label4.Location = new System.Drawing.Point(110, 707);
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
            label3.Location = new System.Drawing.Point(617, 663);
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
            label2.Location = new System.Drawing.Point(617, 707);
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
            label1.Location = new System.Drawing.Point(110, 663);
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
            this.txt_UpdatedAt.Location = new System.Drawing.Point(254, 704);
            this.txt_UpdatedAt.Name = "txt_UpdatedAt";
            this.txt_UpdatedAt.Size = new System.Drawing.Size(336, 30);
            this.txt_UpdatedAt.TabIndex = 58;
            this.txt_UpdatedAt.Tag = "Updated_At";
            // 
            // txt_UpdatedBy
            // 
            this.txt_UpdatedBy.Location = new System.Drawing.Point(732, 704);
            this.txt_UpdatedBy.Name = "txt_UpdatedBy";
            this.txt_UpdatedBy.Size = new System.Drawing.Size(336, 30);
            this.txt_UpdatedBy.TabIndex = 57;
            this.txt_UpdatedBy.Tag = "Updated_By";
            // 
            // txt_CreatedBy
            // 
            this.txt_CreatedBy.Location = new System.Drawing.Point(732, 660);
            this.txt_CreatedBy.Name = "txt_CreatedBy";
            this.txt_CreatedBy.Size = new System.Drawing.Size(336, 30);
            this.txt_CreatedBy.TabIndex = 56;
            this.txt_CreatedBy.Tag = "Created_By";
            // 
            // txt_CreatedAt
            // 
            this.txt_CreatedAt.Location = new System.Drawing.Point(254, 660);
            this.txt_CreatedAt.Name = "txt_CreatedAt";
            this.txt_CreatedAt.Size = new System.Drawing.Size(336, 30);
            this.txt_CreatedAt.TabIndex = 55;
            this.txt_CreatedAt.Tag = "Created_At";
            // 
            // UnitsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(1178, 744);
            this.Controls.Add(label4);
            this.Controls.Add(label3);
            this.Controls.Add(label2);
            this.Controls.Add(label1);
            this.Controls.Add(this.txt_UpdatedAt);
            this.Controls.Add(this.txt_UpdatedBy);
            this.Controls.Add(this.txt_CreatedBy);
            this.Controls.Add(this.txt_CreatedAt);
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
            ((System.ComponentModel.ISupportInitialize)(this.unitsDataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        private System.Windows.Forms.TextBox unit_IDTextBox;
        private System.Windows.Forms.TextBox unit_NameTextBox;
        private System.Windows.Forms.TextBox conversion_FactorTextBox;
        private System.Windows.Forms.DataGridView unitsDataGridView;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn;
        private Controls.AlRowadToolBar alRowadToolBar1;
        private TextBox txt_UpdatedAt;
        private TextBox txt_UpdatedBy;
        private TextBox txt_CreatedBy;
        private TextBox txt_CreatedAt;
    }
}
