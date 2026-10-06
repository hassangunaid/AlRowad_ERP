namespace AlRowad_ERP.Forms
{
    partial class DriverForm
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
            this.alRowadToolBar1 = new AlRowad_ERP.Controls.AlRowadToolBar();
            this.txt_Driver_Name = new System.Windows.Forms.TextBox();
            this.txt_Notes = new System.Windows.Forms.TextBox();
            this.txt_Driver_ID = new System.Windows.Forms.TextBox();
            this.txt_Phone_Number = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txt_License_Number = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txt_Vehicle_Type = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // alRowadToolBar1
            // 
            this.alRowadToolBar1.BackColor = System.Drawing.Color.Transparent;
            this.alRowadToolBar1.Cursor = System.Windows.Forms.Cursors.PanNW;
            this.alRowadToolBar1.Location = new System.Drawing.Point(12, 21);
            this.alRowadToolBar1.Name = "alRowadToolBar1";
            this.alRowadToolBar1.Size = new System.Drawing.Size(984, 53);
            this.alRowadToolBar1.TabIndex = 0;
            // 
            // txt_Driver_Name
            // 
            this.txt_Driver_Name.Location = new System.Drawing.Point(540, 226);
            this.txt_Driver_Name.Name = "txt_Driver_Name";
            this.txt_Driver_Name.Size = new System.Drawing.Size(260, 30);
            this.txt_Driver_Name.TabIndex = 1;
            // 
            // txt_Notes
            // 
            this.txt_Notes.Location = new System.Drawing.Point(295, 308);
            this.txt_Notes.Name = "txt_Notes";
            this.txt_Notes.Size = new System.Drawing.Size(349, 30);
            this.txt_Notes.TabIndex = 2;
            // 
            // txt_Driver_ID
            // 
            this.txt_Driver_ID.Location = new System.Drawing.Point(671, 173);
            this.txt_Driver_ID.Name = "txt_Driver_ID";
            this.txt_Driver_ID.Size = new System.Drawing.Size(129, 30);
            this.txt_Driver_ID.TabIndex = 3;
            // 
            // txt_Phone_Number
            // 
            this.txt_Phone_Number.Location = new System.Drawing.Point(619, 262);
            this.txt_Phone_Number.Name = "txt_Phone_Number";
            this.txt_Phone_Number.Size = new System.Drawing.Size(181, 30);
            this.txt_Phone_Number.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(861, 183);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 25);
            this.label1.TabIndex = 5;
            this.label1.Text = "رقم السائق";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(861, 231);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 25);
            this.label2.TabIndex = 6;
            this.label2.Text = "اسم السائق";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(865, 265);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 25);
            this.label3.TabIndex = 7;
            this.label3.Text = "رقم التلفون";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(649, 309);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(74, 25);
            this.label4.TabIndex = 8;
            this.label4.Text = "ملاحظات";
            // 
            // txt_License_Number
            // 
            this.txt_License_Number.Location = new System.Drawing.Point(189, 226);
            this.txt_License_Number.Name = "txt_License_Number";
            this.txt_License_Number.Size = new System.Drawing.Size(144, 30);
            this.txt_License_Number.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(339, 229);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(93, 25);
            this.label5.TabIndex = 10;
            this.label5.Text = "رقم الرخصه";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(339, 173);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(88, 25);
            this.label6.TabIndex = 12;
            this.label6.Text = "نوع المركبة";
            // 
            // txt_Vehicle_Type
            // 
            this.txt_Vehicle_Type.Location = new System.Drawing.Point(233, 170);
            this.txt_Vehicle_Type.Name = "txt_Vehicle_Type";
            this.txt_Vehicle_Type.Size = new System.Drawing.Size(100, 30);
            this.txt_Vehicle_Type.TabIndex = 11;
            // 
            // DriverForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1019, 450);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txt_Vehicle_Type);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txt_License_Number);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txt_Phone_Number);
            this.Controls.Add(this.txt_Driver_ID);
            this.Controls.Add(this.txt_Notes);
            this.Controls.Add(this.txt_Driver_Name);
            this.Controls.Add(this.alRowadToolBar1);
            this.Name = "DriverForm";
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.RightToLeftLayout = false;
            this.Text = "بيانات السائقين";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Controls.AlRowadToolBar alRowadToolBar1;
        private System.Windows.Forms.TextBox txt_Driver_Name;
        private System.Windows.Forms.TextBox txt_Notes;
        private System.Windows.Forms.TextBox txt_Driver_ID;
        private System.Windows.Forms.TextBox txt_Phone_Number;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txt_License_Number;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txt_Vehicle_Type;
    }
}