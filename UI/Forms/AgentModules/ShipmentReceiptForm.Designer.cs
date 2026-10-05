namespace AlRowad_ERP.Forms
{
    partial class ShipmentReceiptForm
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
            System.Windows.Forms.Label label4;
            System.Windows.Forms.Label label3;
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Label label5;
            this.dtp_Receipt_Date = new AlRowad_ERP.Controls.AlRowadDateTextBox();
            this.alRowadToolBar1 = new AlRowad_ERP.Controls.AlRowadToolBar();
            this.txt_Shipment_ID = new System.Windows.Forms.TextBox();
            this.رقم_المستند = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.اسم_السائق = new System.Windows.Forms.Label();
            this.التاريخ = new System.Windows.Forms.Label();
            this.حالة_المستند = new System.Windows.Forms.Label();
            this.txt_Shipment_Code = new System.Windows.Forms.TextBox();
            this.رقم_لوحة_المركبة = new System.Windows.Forms.Label();
            this.txt_Vehicle_Number = new System.Windows.Forms.TextBox();
            this.txt_Notes = new System.Windows.Forms.TextBox();
            this.ملاحظات = new System.Windows.Forms.Label();
            this.txt_UpdatedAt = new System.Windows.Forms.TextBox();
            this.txt_UpdatedBy = new System.Windows.Forms.TextBox();
            this.txt_CreatedBy = new System.Windows.Forms.TextBox();
            this.txt_CreatedAt = new System.Windows.Forms.TextBox();
            this.txt_Driver_Name = new System.Windows.Forms.TextBox();
            this.txt_Driver_Phone = new System.Windows.Forms.TextBox();
            this.txt_Total_Estimated = new System.Windows.Forms.TextBox();
            this.dgv_Details = new System.Windows.Forms.DataGridView();
            this.AlRowadToolBar = new AlRowad_ERP.Controls.AlRowadToolBar();
            this.object_5ff8d5d3_0f9b_4a07_9152_ce98e5e1eeef = new AlRowad_ERP.Controls.AlRowadToolBar();
            this.object_79d06daa_3f67_4754_a428_0417b4b6f866 = new System.Windows.Forms.Form();
            this.object_d3fe5650_dbbe_4f64_9731_6cb4a05ac45d = new System.Windows.Forms.Form();
            this.Col_Serial = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col_Farmer_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col_Farmer_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col_Item_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col_Unit_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col_Quantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col_Estimated_Discount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col_Estimated_Price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col_Estimated_Total = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col_Notes = new System.Windows.Forms.DataGridViewTextBoxColumn();
            label4 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Details)).BeginInit();
            this.SuspendLayout();
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Cursor = System.Windows.Forms.Cursors.Default;
            label4.ForeColor = System.Drawing.Color.Black;
            label4.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            label4.Location = new System.Drawing.Point(341, 957);
            label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            label4.Size = new System.Drawing.Size(107, 25);
            label4.TabIndex = 78;
            label4.Text = "تاريخ التعديل :";
            label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Cursor = System.Windows.Forms.Cursors.Default;
            label3.ForeColor = System.Drawing.Color.Black;
            label3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            label3.Location = new System.Drawing.Point(848, 913);
            label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            label3.Size = new System.Drawing.Size(105, 25);
            label3.TabIndex = 77;
            label3.Text = "المستـــخــدم : ";
            label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Cursor = System.Windows.Forms.Cursors.Default;
            label2.ForeColor = System.Drawing.Color.Black;
            label2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            label2.Location = new System.Drawing.Point(848, 957);
            label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            label2.Size = new System.Drawing.Size(105, 25);
            label2.TabIndex = 76;
            label2.Text = "المستـــخــدم : ";
            label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Cursor = System.Windows.Forms.Cursors.Default;
            label5.ForeColor = System.Drawing.Color.Black;
            label5.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            label5.Location = new System.Drawing.Point(341, 913);
            label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label5.Name = "label5";
            label5.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            label5.Size = new System.Drawing.Size(108, 25);
            label5.TabIndex = 75;
            label5.Text = "تاريخ الانشاء :";
            label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // dtp_Receipt_Date
            // 
            this.dtp_Receipt_Date.DateValue = new System.DateTime(2026, 10, 5, 0, 0, 0, 0);
            this.dtp_Receipt_Date.Location = new System.Drawing.Point(1098, 156);
            this.dtp_Receipt_Date.Margin = new System.Windows.Forms.Padding(4);
            this.dtp_Receipt_Date.MaxLength = 10;
            this.dtp_Receipt_Date.Name = "dtp_Receipt_Date";
            this.dtp_Receipt_Date.Size = new System.Drawing.Size(184, 30);
            this.dtp_Receipt_Date.TabIndex = 0;
            this.dtp_Receipt_Date.Text = "05/10/2026";
            this.dtp_Receipt_Date.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // alRowadToolBar1
            // 
            this.alRowadToolBar1.BackColor = System.Drawing.Color.Transparent;
            this.alRowadToolBar1.Cursor = System.Windows.Forms.Cursors.PanNW;
            this.alRowadToolBar1.Location = new System.Drawing.Point(485, 32);
            this.alRowadToolBar1.Margin = new System.Windows.Forms.Padding(4);
            this.alRowadToolBar1.Name = "alRowadToolBar1";
            this.alRowadToolBar1.Size = new System.Drawing.Size(964, 52);
            this.alRowadToolBar1.TabIndex = 1;
            // 
            // txt_Shipment_ID
            // 
            this.txt_Shipment_ID.Location = new System.Drawing.Point(84, 80);
            this.txt_Shipment_ID.Margin = new System.Windows.Forms.Padding(4);
            this.txt_Shipment_ID.Name = "txt_Shipment_ID";
            this.txt_Shipment_ID.Size = new System.Drawing.Size(132, 30);
            this.txt_Shipment_ID.TabIndex = 2;
            this.txt_Shipment_ID.Visible = false;
            // 
            // رقم_المستند
            // 
            this.رقم_المستند.AutoSize = true;
            this.رقم_المستند.Location = new System.Drawing.Point(117, 119);
            this.رقم_المستند.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.رقم_المستند.Name = "رقم_المستند";
            this.رقم_المستند.Size = new System.Drawing.Size(83, 25);
            this.رقم_المستند.TabIndex = 3;
            this.رقم_المستند.Text = "رقم المستند";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(1359, 113);
            this.comboBox1.Margin = new System.Windows.Forms.Padding(4);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(160, 33);
            this.comboBox1.TabIndex = 4;
            // 
            // اسم_السائق
            // 
            this.اسم_السائق.AutoSize = true;
            this.اسم_السائق.Location = new System.Drawing.Point(263, 120);
            this.اسم_السائق.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.اسم_السائق.Name = "اسم_السائق";
            this.اسم_السائق.Size = new System.Drawing.Size(80, 25);
            this.اسم_السائق.TabIndex = 5;
            this.اسم_السائق.Text = "اسم السائق";
            // 
            // التاريخ
            // 
            this.التاريخ.AutoSize = true;
            this.التاريخ.Location = new System.Drawing.Point(1024, 160);
            this.التاريخ.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.التاريخ.Name = "التاريخ";
            this.التاريخ.Size = new System.Drawing.Size(56, 25);
            this.التاريخ.TabIndex = 6;
            this.التاريخ.Text = "التاريخ";
            // 
            // حالة_المستند
            // 
            this.حالة_المستند.AutoSize = true;
            this.حالة_المستند.Location = new System.Drawing.Point(1216, 117);
            this.حالة_المستند.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.حالة_المستند.Name = "حالة_المستند";
            this.حالة_المستند.Size = new System.Drawing.Size(89, 25);
            this.حالة_المستند.TabIndex = 7;
            this.حالة_المستند.Text = "حالة المستند";
            // 
            // txt_Shipment_Code
            // 
            this.txt_Shipment_Code.Location = new System.Drawing.Point(206, 118);
            this.txt_Shipment_Code.Margin = new System.Windows.Forms.Padding(4);
            this.txt_Shipment_Code.Name = "txt_Shipment_Code";
            this.txt_Shipment_Code.Size = new System.Drawing.Size(55, 30);
            this.txt_Shipment_Code.TabIndex = 8;
            // 
            // رقم_لوحة_المركبة
            // 
            this.رقم_لوحة_المركبة.AutoSize = true;
            this.رقم_لوحة_المركبة.Location = new System.Drawing.Point(942, 119);
            this.رقم_لوحة_المركبة.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.رقم_لوحة_المركبة.Name = "رقم_لوحة_المركبة";
            this.رقم_لوحة_المركبة.Size = new System.Drawing.Size(122, 25);
            this.رقم_لوحة_المركبة.TabIndex = 10;
            this.رقم_لوحة_المركبة.Text = "رقم لوحة المركبة";
            // 
            // txt_Vehicle_Number
            // 
            this.txt_Vehicle_Number.Location = new System.Drawing.Point(1067, 116);
            this.txt_Vehicle_Number.Margin = new System.Windows.Forms.Padding(4);
            this.txt_Vehicle_Number.Name = "txt_Vehicle_Number";
            this.txt_Vehicle_Number.Size = new System.Drawing.Size(132, 30);
            this.txt_Vehicle_Number.TabIndex = 9;
            // 
            // txt_Notes
            // 
            this.txt_Notes.Location = new System.Drawing.Point(306, 156);
            this.txt_Notes.Margin = new System.Windows.Forms.Padding(4);
            this.txt_Notes.Name = "txt_Notes";
            this.txt_Notes.Size = new System.Drawing.Size(628, 30);
            this.txt_Notes.TabIndex = 12;
            // 
            // ملاحظات
            // 
            this.ملاحظات.AutoSize = true;
            this.ملاحظات.Location = new System.Drawing.Point(138, 160);
            this.ملاحظات.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ملاحظات.Name = "ملاحظات";
            this.ملاحظات.Size = new System.Drawing.Size(126, 25);
            this.ملاحظات.TabIndex = 11;
            this.ملاحظات.Text = "مــــلاحــــــظـــات";
            // 
            // txt_UpdatedAt
            // 
            this.txt_UpdatedAt.Location = new System.Drawing.Point(485, 954);
            this.txt_UpdatedAt.Name = "txt_UpdatedAt";
            this.txt_UpdatedAt.Size = new System.Drawing.Size(336, 30);
            this.txt_UpdatedAt.TabIndex = 74;
            // 
            // txt_UpdatedBy
            // 
            this.txt_UpdatedBy.Location = new System.Drawing.Point(963, 954);
            this.txt_UpdatedBy.Name = "txt_UpdatedBy";
            this.txt_UpdatedBy.Size = new System.Drawing.Size(336, 30);
            this.txt_UpdatedBy.TabIndex = 73;
            // 
            // txt_CreatedBy
            // 
            this.txt_CreatedBy.Location = new System.Drawing.Point(963, 910);
            this.txt_CreatedBy.Name = "txt_CreatedBy";
            this.txt_CreatedBy.Size = new System.Drawing.Size(336, 30);
            this.txt_CreatedBy.TabIndex = 72;
            // 
            // txt_CreatedAt
            // 
            this.txt_CreatedAt.Location = new System.Drawing.Point(485, 910);
            this.txt_CreatedAt.Name = "txt_CreatedAt";
            this.txt_CreatedAt.Size = new System.Drawing.Size(336, 30);
            this.txt_CreatedAt.TabIndex = 71;
            // 
            // txt_Driver_Name
            // 
            this.txt_Driver_Name.Location = new System.Drawing.Point(351, 117);
            this.txt_Driver_Name.Margin = new System.Windows.Forms.Padding(4);
            this.txt_Driver_Name.Name = "txt_Driver_Name";
            this.txt_Driver_Name.Size = new System.Drawing.Size(403, 30);
            this.txt_Driver_Name.TabIndex = 79;
            // 
            // txt_Driver_Phone
            // 
            this.txt_Driver_Phone.Location = new System.Drawing.Point(762, 116);
            this.txt_Driver_Phone.Margin = new System.Windows.Forms.Padding(4);
            this.txt_Driver_Phone.Name = "txt_Driver_Phone";
            this.txt_Driver_Phone.Size = new System.Drawing.Size(181, 30);
            this.txt_Driver_Phone.TabIndex = 80;
            // 
            // txt_Total_Estimated
            // 
            this.txt_Total_Estimated.Location = new System.Drawing.Point(1347, 156);
            this.txt_Total_Estimated.Margin = new System.Windows.Forms.Padding(4);
            this.txt_Total_Estimated.Name = "txt_Total_Estimated";
            this.txt_Total_Estimated.Size = new System.Drawing.Size(253, 30);
            this.txt_Total_Estimated.TabIndex = 82;
            // 
            // dgv_Details
            // 
            this.dgv_Details.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_Details.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Col_Serial,
            this.Col_Farmer_Name,
            this.Col_Farmer_ID,
            this.Col_Item_Name,
            this.Col_Unit_ID,
            this.Col_Quantity,
            this.Col_Estimated_Discount,
            this.Col_Estimated_Price,
            this.Col_Estimated_Total,
            this.Col_Notes});
            this.dgv_Details.EnableHeadersVisualStyles = false;
            this.dgv_Details.Location = new System.Drawing.Point(65, 234);
            this.dgv_Details.Name = "dgv_Details";
            this.dgv_Details.RowHeadersVisible = false;
            this.dgv_Details.RowHeadersWidth = 62;
            this.dgv_Details.RowTemplate.Height = 29;
            this.dgv_Details.Size = new System.Drawing.Size(1536, 602);
            this.dgv_Details.TabIndex = 83;
            // 
            // AlRowadToolBar
            // 
            this.AlRowadToolBar.BackColor = System.Drawing.Color.Transparent;
            this.AlRowadToolBar.Cursor = System.Windows.Forms.Cursors.PanNW;
            this.AlRowadToolBar.Location = new System.Drawing.Point(272, 12);
            this.AlRowadToolBar.Name = "AlRowadToolBar";
            this.AlRowadToolBar.Size = new System.Drawing.Size(984, 53);
            this.AlRowadToolBar.TabIndex = 0;
            // 
            // object_5ff8d5d3_0f9b_4a07_9152_ce98e5e1eeef
            // 
            this.object_5ff8d5d3_0f9b_4a07_9152_ce98e5e1eeef.BackColor = System.Drawing.Color.Transparent;
            this.object_5ff8d5d3_0f9b_4a07_9152_ce98e5e1eeef.Cursor = System.Windows.Forms.Cursors.PanNW;
            this.object_5ff8d5d3_0f9b_4a07_9152_ce98e5e1eeef.Location = new System.Drawing.Point(272, 12);
            this.object_5ff8d5d3_0f9b_4a07_9152_ce98e5e1eeef.Name = "object_5ff8d5d3_0f9b_4a07_9152_ce98e5e1eeef";
            this.object_5ff8d5d3_0f9b_4a07_9152_ce98e5e1eeef.Size = new System.Drawing.Size(984, 53);
            this.object_5ff8d5d3_0f9b_4a07_9152_ce98e5e1eeef.TabIndex = 1;
            this.object_5ff8d5d3_0f9b_4a07_9152_ce98e5e1eeef.Visible = false;
            // 
            // object_79d06daa_3f67_4754_a428_0417b4b6f866
            // 
            this.object_79d06daa_3f67_4754_a428_0417b4b6f866.BackColor = System.Drawing.SystemColors.Control;
            this.object_79d06daa_3f67_4754_a428_0417b4b6f866.ClientSize = new System.Drawing.Size(800, 450);
            this.object_79d06daa_3f67_4754_a428_0417b4b6f866.Location = new System.Drawing.Point(22, 22);
            this.object_79d06daa_3f67_4754_a428_0417b4b6f866.Name = "object_79d06daa_3f67_4754_a428_0417b4b6f866";
            this.object_79d06daa_3f67_4754_a428_0417b4b6f866.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.object_79d06daa_3f67_4754_a428_0417b4b6f866.RightToLeftLayout = true;
            this.object_79d06daa_3f67_4754_a428_0417b4b6f866.Tag = "ShipmentReceiptForm";
            this.object_79d06daa_3f67_4754_a428_0417b4b6f866.Visible = false;
            // 
            // object_d3fe5650_dbbe_4f64_9731_6cb4a05ac45d
            // 
            this.object_d3fe5650_dbbe_4f64_9731_6cb4a05ac45d.BackColor = System.Drawing.SystemColors.Control;
            this.object_d3fe5650_dbbe_4f64_9731_6cb4a05ac45d.ClientSize = new System.Drawing.Size(800, 450);
            this.object_d3fe5650_dbbe_4f64_9731_6cb4a05ac45d.Location = new System.Drawing.Point(22, 22);
            this.object_d3fe5650_dbbe_4f64_9731_6cb4a05ac45d.Name = "object_d3fe5650_dbbe_4f64_9731_6cb4a05ac45d";
            this.object_d3fe5650_dbbe_4f64_9731_6cb4a05ac45d.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.object_d3fe5650_dbbe_4f64_9731_6cb4a05ac45d.RightToLeftLayout = true;
            this.object_d3fe5650_dbbe_4f64_9731_6cb4a05ac45d.Tag = "ShipmentReceiptForm";
            this.object_d3fe5650_dbbe_4f64_9731_6cb4a05ac45d.Visible = false;
            // 
            // Col_Serial
            // 
            this.Col_Serial.HeaderText = "تسلسلي";
            this.Col_Serial.MinimumWidth = 8;
            this.Col_Serial.Name = "Col_Serial";
            this.Col_Serial.Width = 150;
            // 
            // Col_Farmer_Name
            // 
            this.Col_Farmer_Name.HeaderText = "اسم المزارع";
            this.Col_Farmer_Name.MinimumWidth = 8;
            this.Col_Farmer_Name.Name = "Col_Farmer_Name";
            this.Col_Farmer_Name.Width = 150;
            // 
            // Col_Farmer_ID
            // 
            this.Col_Farmer_ID.HeaderText = "رقم حساب المزارع";
            this.Col_Farmer_ID.MinimumWidth = 8;
            this.Col_Farmer_ID.Name = "Col_Farmer_ID";
            this.Col_Farmer_ID.Visible = false;
            this.Col_Farmer_ID.Width = 150;
            // 
            // Col_Item_Name
            // 
            this.Col_Item_Name.HeaderText = "الصنف";
            this.Col_Item_Name.MinimumWidth = 8;
            this.Col_Item_Name.Name = "Col_Item_Name";
            this.Col_Item_Name.Width = 150;
            // 
            // Col_Unit_ID
            // 
            this.Col_Unit_ID.HeaderText = "الوحدة";
            this.Col_Unit_ID.MinimumWidth = 8;
            this.Col_Unit_ID.Name = "Col_Unit_ID";
            this.Col_Unit_ID.Width = 150;
            // 
            // Col_Quantity
            // 
            this.Col_Quantity.HeaderText = "الكمية";
            this.Col_Quantity.MinimumWidth = 8;
            this.Col_Quantity.Name = "Col_Quantity";
            this.Col_Quantity.Width = 150;
            // 
            // Col_Estimated_Discount
            // 
            this.Col_Estimated_Discount.HeaderText = "الخصم";
            this.Col_Estimated_Discount.MinimumWidth = 8;
            this.Col_Estimated_Discount.Name = "Col_Estimated_Discount";
            this.Col_Estimated_Discount.Width = 150;
            // 
            // Col_Estimated_Price
            // 
            this.Col_Estimated_Price.HeaderText = "السعر[تقديري]";
            this.Col_Estimated_Price.MinimumWidth = 8;
            this.Col_Estimated_Price.Name = "Col_Estimated_Price";
            this.Col_Estimated_Price.Width = 150;
            // 
            // Col_Estimated_Total
            // 
            this.Col_Estimated_Total.HeaderText = "الاجمالي[تقديري]";
            this.Col_Estimated_Total.MinimumWidth = 8;
            this.Col_Estimated_Total.Name = "Col_Estimated_Total";
            this.Col_Estimated_Total.Width = 150;
            // 
            // Col_Notes
            // 
            this.Col_Notes.HeaderText = "ملاحظات";
            this.Col_Notes.MinimumWidth = 8;
            this.Col_Notes.Name = "Col_Notes";
            this.Col_Notes.Width = 150;
            // 
            // ShipmentReceiptForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1613, 1050);
            this.Controls.Add(this.dgv_Details);
            this.Controls.Add(this.txt_Total_Estimated);
            this.Controls.Add(this.txt_Driver_Phone);
            this.Controls.Add(this.txt_Driver_Name);
            this.Controls.Add(label4);
            this.Controls.Add(label3);
            this.Controls.Add(label2);
            this.Controls.Add(label5);
            this.Controls.Add(this.txt_UpdatedAt);
            this.Controls.Add(this.txt_UpdatedBy);
            this.Controls.Add(this.txt_CreatedBy);
            this.Controls.Add(this.txt_CreatedAt);
            this.Controls.Add(this.txt_Notes);
            this.Controls.Add(this.ملاحظات);
            this.Controls.Add(this.رقم_لوحة_المركبة);
            this.Controls.Add(this.txt_Vehicle_Number);
            this.Controls.Add(this.txt_Shipment_Code);
            this.Controls.Add(this.حالة_المستند);
            this.Controls.Add(this.التاريخ);
            this.Controls.Add(this.اسم_السائق);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.رقم_المستند);
            this.Controls.Add(this.txt_Shipment_ID);
            this.Controls.Add(this.alRowadToolBar1);
            this.Controls.Add(this.dtp_Receipt_Date);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ShipmentReceiptForm";
            this.Tag = "";
            this.Text = "بيانات الحمولة";
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Details)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Controls.AlRowadDateTextBox dtp_Receipt_Date;
        private Controls.AlRowadToolBar alRowadToolBar1;
        private System.Windows.Forms.TextBox txt_Shipment_ID;
        private System.Windows.Forms.Label رقم_المستند;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label اسم_السائق;
        private System.Windows.Forms.Label التاريخ;
        private System.Windows.Forms.Label حالة_المستند;
        private System.Windows.Forms.TextBox txt_Shipment_Code;
        private System.Windows.Forms.Label رقم_لوحة_المركبة;
        private System.Windows.Forms.TextBox txt_Vehicle_Number;
        private System.Windows.Forms.TextBox txt_Notes;
        private System.Windows.Forms.Label ملاحظات;
        private object label1;
        private System.Windows.Forms.TextBox txt_UpdatedAt;
        private System.Windows.Forms.TextBox txt_UpdatedBy;
        private System.Windows.Forms.TextBox txt_CreatedBy;
        private System.Windows.Forms.TextBox txt_CreatedAt;
        private System.Windows.Forms.TextBox txt_Driver_Name;
        private System.Windows.Forms.TextBox txt_Driver_Phone;
        private System.Windows.Forms.TextBox txt_Total_Estimated;
        private System.Windows.Forms.DataGridView dgv_Details;
        private Controls.AlRowadToolBar AlRowadToolBar;
        private Controls.AlRowadToolBar object_5ff8d5d3_0f9b_4a07_9152_ce98e5e1eeef;
        private System.Windows.Forms.Form object_79d06daa_3f67_4754_a428_0417b4b6f866;
        private System.Windows.Forms.Form object_d3fe5650_dbbe_4f64_9731_6cb4a05ac45d;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_Serial;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_Farmer_Name;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_Farmer_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_Item_Name;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_Unit_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_Quantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_Estimated_Discount;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_Estimated_Price;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_Estimated_Total;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_Notes;
    }
}