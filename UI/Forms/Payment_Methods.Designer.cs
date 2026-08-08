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
            System.Windows.Forms.Label method_IDLabel;
            System.Windows.Forms.Label method_NameLabel;
            this.method_IDTextBox = new System.Windows.Forms.TextBox();
            this.method_NameTextBox = new System.Windows.Forms.TextBox();
            this.alRowadToolBar1 = new AlRowad_ERP.Controls.AlRowadToolBar();
            method_IDLabel = new System.Windows.Forms.Label();
            method_NameLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
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
            // method_NameLabel
            // 
            method_NameLabel.AutoSize = true;
            method_NameLabel.Location = new System.Drawing.Point(382, 257);
            method_NameLabel.Name = "method_NameLabel";
            method_NameLabel.Size = new System.Drawing.Size(141, 25);
            method_NameLabel.TabIndex = 3;
            method_NameLabel.Text = "Method Name:";
            // 
            // method_IDTextBox
            // 
            this.method_IDTextBox.Location = new System.Drawing.Point(529, 218);
            this.method_IDTextBox.Name = "method_IDTextBox";
            this.method_IDTextBox.Size = new System.Drawing.Size(100, 30);
            this.method_IDTextBox.TabIndex = 2;
            // 
            // method_NameTextBox
            // 
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
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox method_IDTextBox;
        private System.Windows.Forms.TextBox method_NameTextBox;
        private Controls.AlRowadToolBar alRowadToolBar1;
    }
}