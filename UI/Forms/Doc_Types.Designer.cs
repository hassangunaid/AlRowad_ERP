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
            System.Windows.Forms.Label doc_Type_IDLabel;
            System.Windows.Forms.Label doc_NameLabel;
            System.Windows.Forms.Label module_NameLabel;
            this.doc_Type_IDTextBox = new System.Windows.Forms.TextBox();
            this.doc_NameTextBox = new System.Windows.Forms.TextBox();
            this.module_NameTextBox = new System.Windows.Forms.TextBox();
            this.alRowadToolBar1 = new AlRowad_ERP.Controls.AlRowadToolBar();
            doc_Type_IDLabel = new System.Windows.Forms.Label();
            doc_NameLabel = new System.Windows.Forms.Label();
            module_NameLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
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
            // doc_NameLabel
            // 
            doc_NameLabel.AutoSize = true;
            doc_NameLabel.Location = new System.Drawing.Point(436, 190);
            doc_NameLabel.Name = "doc_NameLabel";
            doc_NameLabel.Size = new System.Drawing.Size(110, 25);
            doc_NameLabel.TabIndex = 2;
            doc_NameLabel.Text = "Doc Name:";
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
            // doc_Type_IDTextBox
            // 
            this.doc_Type_IDTextBox.Location = new System.Drawing.Point(582, 151);
            this.doc_Type_IDTextBox.Name = "doc_Type_IDTextBox";
            this.doc_Type_IDTextBox.Size = new System.Drawing.Size(100, 30);
            this.doc_Type_IDTextBox.TabIndex = 1;
            // 
            // doc_NameTextBox
            // 
            this.doc_NameTextBox.Location = new System.Drawing.Point(582, 187);
            this.doc_NameTextBox.Name = "doc_NameTextBox";
            this.doc_NameTextBox.Size = new System.Drawing.Size(100, 30);
            this.doc_NameTextBox.TabIndex = 3;
            // 
            // module_NameTextBox
            // 
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
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox doc_Type_IDTextBox;
        private System.Windows.Forms.TextBox doc_NameTextBox;
        private System.Windows.Forms.TextBox module_NameTextBox;
        private Controls.AlRowadToolBar alRowadToolBar1;
    }
}