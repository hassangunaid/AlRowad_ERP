namespace AlRowad_ERP.Controls
{
    partial class AlRowadToolBar
    {
        private System.ComponentModel.IContainer components = null;

        public FontAwesome.Sharp.IconButton btn_Save;
        public FontAwesome.Sharp.IconButton btn_New;
        public FontAwesome.Sharp.IconButton btn_AddFrom;
        public FontAwesome.Sharp.IconButton btn_Edit;
        public FontAwesome.Sharp.IconButton btn_Delete;
        public FontAwesome.Sharp.IconButton btn_Search;
        public FontAwesome.Sharp.IconButton btn_Cancel;
        public FontAwesome.Sharp.IconButton btn_Print;
        public FontAwesome.Sharp.IconButton btn_Close;
        private System.Windows.Forms.ToolTip toolTip1;

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
            this.components = new System.ComponentModel.Container();
            this.btn_Save = new FontAwesome.Sharp.IconButton();
            this.btn_New = new FontAwesome.Sharp.IconButton();
            this.btn_AddFrom = new FontAwesome.Sharp.IconButton();
            this.btn_Edit = new FontAwesome.Sharp.IconButton();
            this.btn_Delete = new FontAwesome.Sharp.IconButton();
            this.btn_Search = new FontAwesome.Sharp.IconButton();
            this.btn_Cancel = new FontAwesome.Sharp.IconButton();
            this.btn_Print = new FontAwesome.Sharp.IconButton();
            this.btn_Close = new FontAwesome.Sharp.IconButton();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // btn_Save
            // 
            this.btn_Save.AccessibleName = "حفظ";
            this.btn_Save.IconChar = FontAwesome.Sharp.IconChar.FloppyDisk;
            this.btn_Save.IconColor = System.Drawing.Color.Blue;
            this.btn_Save.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btn_Save.Location = new System.Drawing.Point(346, 3);
            this.btn_Save.Name = "btn_Save";
            this.btn_Save.Size = new System.Drawing.Size(53, 48);
            this.btn_Save.TabIndex = 1;
            this.toolTip1.SetToolTip(this.btn_Save, "حفظ(F10)");
            this.btn_Save.UseVisualStyleBackColor = true;
            // 
            // btn_New
            // 
            this.btn_New.AccessibleName = "اضافة";
            this.btn_New.IconChar = FontAwesome.Sharp.IconChar.FileMedical;
            this.btn_New.IconColor = System.Drawing.Color.Blue;
            this.btn_New.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btn_New.Location = new System.Drawing.Point(837, 3);
            this.btn_New.Name = "btn_New";
            this.btn_New.Size = new System.Drawing.Size(53, 48);
            this.btn_New.TabIndex = 2;
            this.toolTip1.SetToolTip(this.btn_New, "اضافة(F6)");
            this.btn_New.UseVisualStyleBackColor = true;
            this.btn_New.Click += new System.EventHandler(this.btn_New_Click);
            // 
            // btn_AddFrom
            // 
            this.btn_AddFrom.AccessibleDescription = "اضافة من ";
            this.btn_AddFrom.AccessibleName = "اضافة من";
            this.btn_AddFrom.IconChar = FontAwesome.Sharp.IconChar.Clone;
            this.btn_AddFrom.IconColor = System.Drawing.Color.Blue;
            this.btn_AddFrom.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btn_AddFrom.Location = new System.Drawing.Point(896, 3);
            this.btn_AddFrom.Name = "btn_AddFrom";
            this.btn_AddFrom.Size = new System.Drawing.Size(53, 48);
            this.btn_AddFrom.TabIndex = 3;
            this.toolTip1.SetToolTip(this.btn_AddFrom, "اضافة من");
            this.btn_AddFrom.UseVisualStyleBackColor = true;
            // 
            // btn_Edit
            // 
            this.btn_Edit.AccessibleName = "تعديل";
            this.btn_Edit.IconChar = FontAwesome.Sharp.IconChar.FilePen;
            this.btn_Edit.IconColor = System.Drawing.Color.Blue;
            this.btn_Edit.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btn_Edit.Location = new System.Drawing.Point(778, 3);
            this.btn_Edit.Name = "btn_Edit";
            this.btn_Edit.Size = new System.Drawing.Size(53, 48);
            this.btn_Edit.TabIndex = 4;
            this.toolTip1.SetToolTip(this.btn_Edit, "تعديل(F5)");
            this.btn_Edit.UseVisualStyleBackColor = true;
            // 
            // btn_Delete
            // 
            this.btn_Delete.AccessibleName = "حذف";
            this.btn_Delete.IconChar = FontAwesome.Sharp.IconChar.FileCircleXmark;
            this.btn_Delete.IconColor = System.Drawing.Color.Blue;
            this.btn_Delete.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btn_Delete.Location = new System.Drawing.Point(287, 3);
            this.btn_Delete.Name = "btn_Delete";
            this.btn_Delete.Size = new System.Drawing.Size(53, 48);
            this.btn_Delete.TabIndex = 5;
            this.toolTip1.SetToolTip(this.btn_Delete, "حذف(Ctrl+D)");
            this.btn_Delete.UseVisualStyleBackColor = true;
            // 
            // btn_Search
            // 
            this.btn_Search.AccessibleName = "بحث";
            this.btn_Search.IconChar = FontAwesome.Sharp.IconChar.FileWaveform;
            this.btn_Search.IconColor = System.Drawing.Color.Blue;
            this.btn_Search.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btn_Search.Location = new System.Drawing.Point(719, 3);
            this.btn_Search.Name = "btn_Search";
            this.btn_Search.Size = new System.Drawing.Size(53, 48);
            this.btn_Search.TabIndex = 6;
            this.toolTip1.SetToolTip(this.btn_Search, "بحث(F9)");
            this.btn_Search.UseVisualStyleBackColor = true;
            // 
            // btn_Cancel
            // 
            this.btn_Cancel.AccessibleName = "تراجع";
            this.btn_Cancel.IconChar = FontAwesome.Sharp.IconChar.FileUpload;
            this.btn_Cancel.IconColor = System.Drawing.Color.Blue;
            this.btn_Cancel.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btn_Cancel.Location = new System.Drawing.Point(660, 3);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Size = new System.Drawing.Size(53, 48);
            this.btn_Cancel.TabIndex = 7;
            this.toolTip1.SetToolTip(this.btn_Cancel, "تراجع(F4)");
            this.btn_Cancel.UseVisualStyleBackColor = true;
            // 
            // btn_Print
            // 
            this.btn_Print.AccessibleName = "طباعة";
            this.btn_Print.IconChar = FontAwesome.Sharp.IconChar.Audible;
            this.btn_Print.IconColor = System.Drawing.Color.Blue;
            this.btn_Print.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btn_Print.Location = new System.Drawing.Point(471, 1);
            this.btn_Print.Name = "btn_Print";
            this.btn_Print.Size = new System.Drawing.Size(53, 48);
            this.btn_Print.TabIndex = 8;
            this.toolTip1.SetToolTip(this.btn_Print, "طباعة");
            this.btn_Print.UseVisualStyleBackColor = true;
            // 
            // btn_Close
            // 
            this.btn_Close.AccessibleName = "خروج";
            this.btn_Close.IconChar = FontAwesome.Sharp.IconChar.Minus;
            this.btn_Close.IconColor = System.Drawing.Color.Blue;
            this.btn_Close.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btn_Close.Location = new System.Drawing.Point(46, 3);
            this.btn_Close.Name = "btn_Close";
            this.btn_Close.Size = new System.Drawing.Size(53, 48);
            this.btn_Close.TabIndex = 9;
            this.toolTip1.SetToolTip(this.btn_Close, "خروج");
            this.btn_Close.UseVisualStyleBackColor = true;
            // 
            // AlRowadToolBar
            // 
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.btn_Close);
            this.Controls.Add(this.btn_Print);
            this.Controls.Add(this.btn_Cancel);
            this.Controls.Add(this.btn_Search);
            this.Controls.Add(this.btn_Delete);
            this.Controls.Add(this.btn_Edit);
            this.Controls.Add(this.btn_AddFrom);
            this.Controls.Add(this.btn_New);
            this.Controls.Add(this.btn_Save);
            this.Cursor = System.Windows.Forms.Cursors.PanNW;
            this.Name = "AlRowadToolBar";
            this.Size = new System.Drawing.Size(984, 53);
            this.Load += new System.EventHandler(this.AlRowadToolBar_Load);
            this.ResumeLayout(false);

        }
    }
}