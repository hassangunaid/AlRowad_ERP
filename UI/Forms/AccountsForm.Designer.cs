using System.Windows.Forms;

namespace AlRowad_ERP.Forms
{
    partial class AccountsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    // دالة تدمير الكائنات وتطهير الذاكرة عند إغلاق الشاشة لمنع تسريب موارد النظام
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose(); // سيتم التعرف عليها الآن بنجاح 100%
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.Label acc_IDLabel;
            System.Windows.Forms.Label acc_NameLabel;
            System.Windows.Forms.Label acc_Name_EnLabel;
            System.Windows.Forms.Label parent_IDLabel;
            System.Windows.Forms.Label account_LevelLabel;
            System.Windows.Forms.Label acc_TypeLabel;
            System.Windows.Forms.Label acc_NatureLabel;
            System.Windows.Forms.Label report_TypeLabel;
            System.Windows.Forms.Label is_StoppedLabel;
            System.Windows.Forms.Label label1;
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Label label3;
            System.Windows.Forms.Label label4;
            this.acc_ID = new System.Windows.Forms.TextBox();
            this.acc_Name = new System.Windows.Forms.TextBox();
            this.acc_Name_En = new System.Windows.Forms.TextBox();
            this.parent_ID = new System.Windows.Forms.TextBox();
            this.account_Level = new System.Windows.Forms.TextBox();
            this.acc_Type = new System.Windows.Forms.ComboBox();
            this.report_Type = new System.Windows.Forms.ComboBox();
            this.is_Stopped = new System.Windows.Forms.CheckBox();
            this.treeAccounts = new System.Windows.Forms.TreeView();
            this.alRowadToolBar = new AlRowad_ERP.Controls.AlRowadToolBar();
            this.GrBox_currencies = new System.Windows.Forms.GroupBox();
            this.dgv_currencies = new System.Windows.Forms.DataGridView();
            this.Is_Active = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Cur_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Is_Default = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Is_Frozen = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Cur_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.acc_Nature = new System.Windows.Forms.ComboBox();
            this.btnCollapseAll = new System.Windows.Forms.Button();
            this.btnExpandAll = new System.Windows.Forms.Button();
            this.Refresh = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.iconToolStripButton1 = new FontAwesome.Sharp.IconToolStripButton();
            this.txt_CreatedAt = new System.Windows.Forms.TextBox();
            this.txt_CreatedBy = new System.Windows.Forms.TextBox();
            this.txt_UpdatedAt = new System.Windows.Forms.TextBox();
            this.txt_UpdatedBy = new System.Windows.Forms.TextBox();
            acc_IDLabel = new System.Windows.Forms.Label();
            acc_NameLabel = new System.Windows.Forms.Label();
            acc_Name_EnLabel = new System.Windows.Forms.Label();
            parent_IDLabel = new System.Windows.Forms.Label();
            account_LevelLabel = new System.Windows.Forms.Label();
            acc_TypeLabel = new System.Windows.Forms.Label();
            acc_NatureLabel = new System.Windows.Forms.Label();
            report_TypeLabel = new System.Windows.Forms.Label();
            is_StoppedLabel = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            this.GrBox_currencies.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_currencies)).BeginInit();
            this.SuspendLayout();
            // 
            // acc_IDLabel
            // 
            acc_IDLabel.AutoSize = true;
            acc_IDLabel.Cursor = System.Windows.Forms.Cursors.Default;
            acc_IDLabel.ForeColor = System.Drawing.Color.BlanchedAlmond;
            acc_IDLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            acc_IDLabel.Location = new System.Drawing.Point(29, 154);
            acc_IDLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            acc_IDLabel.Name = "acc_IDLabel";
            acc_IDLabel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            acc_IDLabel.Size = new System.Drawing.Size(94, 25);
            acc_IDLabel.TabIndex = 23;
            acc_IDLabel.Text = "رقم الحساب:";
            acc_IDLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // acc_NameLabel
            // 
            acc_NameLabel.AutoSize = true;
            acc_NameLabel.Cursor = System.Windows.Forms.Cursors.Default;
            acc_NameLabel.ForeColor = System.Drawing.Color.BlanchedAlmond;
            acc_NameLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            acc_NameLabel.Location = new System.Drawing.Point(29, 211);
            acc_NameLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            acc_NameLabel.Name = "acc_NameLabel";
            acc_NameLabel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            acc_NameLabel.Size = new System.Drawing.Size(94, 25);
            acc_NameLabel.TabIndex = 25;
            acc_NameLabel.Text = "اسم الحساب:";
            acc_NameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // acc_Name_EnLabel
            // 
            acc_Name_EnLabel.AutoSize = true;
            acc_Name_EnLabel.ForeColor = System.Drawing.Color.BlanchedAlmond;
            acc_Name_EnLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            acc_Name_EnLabel.Location = new System.Drawing.Point(29, 266);
            acc_Name_EnLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            acc_Name_EnLabel.Name = "acc_Name_EnLabel";
            acc_Name_EnLabel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            acc_Name_EnLabel.Size = new System.Drawing.Size(147, 25);
            acc_Name_EnLabel.TabIndex = 27;
            acc_Name_EnLabel.Text = "Account Name:";
            // 
            // parent_IDLabel
            // 
            parent_IDLabel.AutoSize = true;
            parent_IDLabel.ForeColor = System.Drawing.Color.BlanchedAlmond;
            parent_IDLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            parent_IDLabel.Location = new System.Drawing.Point(384, 154);
            parent_IDLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            parent_IDLabel.Name = "parent_IDLabel";
            parent_IDLabel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            parent_IDLabel.Size = new System.Drawing.Size(150, 25);
            parent_IDLabel.TabIndex = 29;
            parent_IDLabel.Text = "رقم الحساب الرئيسي:";
            // 
            // account_LevelLabel
            // 
            account_LevelLabel.AutoSize = true;
            account_LevelLabel.ForeColor = System.Drawing.Color.BlanchedAlmond;
            account_LevelLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            account_LevelLabel.Location = new System.Drawing.Point(384, 320);
            account_LevelLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            account_LevelLabel.Name = "account_LevelLabel";
            account_LevelLabel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            account_LevelLabel.Size = new System.Drawing.Size(142, 25);
            account_LevelLabel.TabIndex = 31;
            account_LevelLabel.Text = "Account Level:";
            // 
            // acc_TypeLabel
            // 
            acc_TypeLabel.AutoSize = true;
            acc_TypeLabel.ForeColor = System.Drawing.Color.BlanchedAlmond;
            acc_TypeLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            acc_TypeLabel.Location = new System.Drawing.Point(29, 379);
            acc_TypeLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            acc_TypeLabel.Name = "acc_TypeLabel";
            acc_TypeLabel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            acc_TypeLabel.Size = new System.Drawing.Size(102, 25);
            acc_TypeLabel.TabIndex = 33;
            acc_TypeLabel.Text = "Acc Type:";
            // 
            // acc_NatureLabel
            // 
            acc_NatureLabel.AutoSize = true;
            acc_NatureLabel.Cursor = System.Windows.Forms.Cursors.Default;
            acc_NatureLabel.ForeColor = System.Drawing.Color.BlanchedAlmond;
            acc_NatureLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            acc_NatureLabel.Location = new System.Drawing.Point(29, 314);
            acc_NatureLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            acc_NatureLabel.Name = "acc_NatureLabel";
            acc_NatureLabel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            acc_NatureLabel.Size = new System.Drawing.Size(115, 25);
            acc_NatureLabel.TabIndex = 35;
            acc_NatureLabel.Text = "Acc Nature:";
            acc_NatureLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // report_TypeLabel
            // 
            report_TypeLabel.AutoSize = true;
            report_TypeLabel.ForeColor = System.Drawing.Color.BlanchedAlmond;
            report_TypeLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            report_TypeLabel.Location = new System.Drawing.Point(384, 379);
            report_TypeLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            report_TypeLabel.Name = "report_TypeLabel";
            report_TypeLabel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            report_TypeLabel.Size = new System.Drawing.Size(125, 25);
            report_TypeLabel.TabIndex = 37;
            report_TypeLabel.Text = "Report Type:";
            // 
            // is_StoppedLabel
            // 
            is_StoppedLabel.AutoSize = true;
            is_StoppedLabel.Cursor = System.Windows.Forms.Cursors.Default;
            is_StoppedLabel.ForeColor = System.Drawing.Color.BlanchedAlmond;
            is_StoppedLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            is_StoppedLabel.Location = new System.Drawing.Point(569, 238);
            is_StoppedLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            is_StoppedLabel.Name = "is_StoppedLabel";
            is_StoppedLabel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            is_StoppedLabel.Size = new System.Drawing.Size(67, 25);
            is_StoppedLabel.TabIndex = 39;
            is_StoppedLabel.Tag = "Is_Stopped";
            is_StoppedLabel.Text = "ايــقـــاف";
            is_StoppedLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Cursor = System.Windows.Forms.Cursors.Default;
            label1.ForeColor = System.Drawing.Color.BlanchedAlmond;
            label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            label1.Location = new System.Drawing.Point(97, 832);
            label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            label1.Size = new System.Drawing.Size(108, 25);
            label1.TabIndex = 51;
            label1.Text = "تاريخ الانشاء :";
            label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Cursor = System.Windows.Forms.Cursors.Default;
            label2.ForeColor = System.Drawing.Color.BlanchedAlmond;
            label2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            label2.Location = new System.Drawing.Point(604, 876);
            label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            label2.Size = new System.Drawing.Size(105, 25);
            label2.TabIndex = 52;
            label2.Text = "المستـــخــدم : ";
            label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Cursor = System.Windows.Forms.Cursors.Default;
            label3.ForeColor = System.Drawing.Color.BlanchedAlmond;
            label3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            label3.Location = new System.Drawing.Point(604, 832);
            label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            label3.Size = new System.Drawing.Size(105, 25);
            label3.TabIndex = 53;
            label3.Text = "المستـــخــدم : ";
            label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Cursor = System.Windows.Forms.Cursors.Default;
            label4.ForeColor = System.Drawing.Color.BlanchedAlmond;
            label4.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            label4.Location = new System.Drawing.Point(97, 876);
            label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            label4.Size = new System.Drawing.Size(107, 25);
            label4.TabIndex = 54;
            label4.Text = "تاريخ التعديل :";
            label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // acc_ID
            // 
            this.acc_ID.Location = new System.Drawing.Point(183, 151);
            this.acc_ID.Margin = new System.Windows.Forms.Padding(4);
            this.acc_ID.Name = "acc_ID";
            this.acc_ID.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.acc_ID.Size = new System.Drawing.Size(193, 30);
            this.acc_ID.TabIndex = 24;
            this.acc_ID.Tag = "Acc_ID";
            // 
            // acc_Name
            // 
            this.acc_Name.Location = new System.Drawing.Point(183, 211);
            this.acc_Name.Margin = new System.Windows.Forms.Padding(4);
            this.acc_Name.Name = "acc_Name";
            this.acc_Name.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.acc_Name.Size = new System.Drawing.Size(364, 30);
            this.acc_Name.TabIndex = 26;
            this.acc_Name.Tag = "Acc_Name";
            // 
            // acc_Name_En
            // 
            this.acc_Name_En.Location = new System.Drawing.Point(183, 266);
            this.acc_Name_En.Margin = new System.Windows.Forms.Padding(4);
            this.acc_Name_En.Name = "acc_Name_En";
            this.acc_Name_En.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.acc_Name_En.Size = new System.Drawing.Size(364, 30);
            this.acc_Name_En.TabIndex = 28;
            this.acc_Name_En.Tag = "Acc_Name_En";
            // 
            // parent_ID
            // 
            this.parent_ID.Location = new System.Drawing.Point(552, 151);
            this.parent_ID.Margin = new System.Windows.Forms.Padding(4);
            this.parent_ID.Name = "parent_ID";
            this.parent_ID.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.parent_ID.Size = new System.Drawing.Size(137, 30);
            this.parent_ID.TabIndex = 30;
            this.parent_ID.Tag = "Parent_ID";
            // 
            // account_Level
            // 
            this.account_Level.Location = new System.Drawing.Point(552, 317);
            this.account_Level.Margin = new System.Windows.Forms.Padding(4);
            this.account_Level.Name = "account_Level";
            this.account_Level.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.account_Level.Size = new System.Drawing.Size(137, 30);
            this.account_Level.TabIndex = 32;
            this.account_Level.Tag = "Account_Level";
            // 
            // acc_Type
            // 
            this.acc_Type.Location = new System.Drawing.Point(183, 374);
            this.acc_Type.Margin = new System.Windows.Forms.Padding(4);
            this.acc_Type.Name = "acc_Type";
            this.acc_Type.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.acc_Type.Size = new System.Drawing.Size(137, 33);
            this.acc_Type.TabIndex = 34;
            this.acc_Type.Tag = "Acc_Type";
            // 
            // report_Type
            // 
            this.report_Type.Location = new System.Drawing.Point(552, 374);
            this.report_Type.Margin = new System.Windows.Forms.Padding(4);
            this.report_Type.Name = "report_Type";
            this.report_Type.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.report_Type.Size = new System.Drawing.Size(137, 33);
            this.report_Type.TabIndex = 38;
            this.report_Type.Tag = "Report_Type";
            // 
            // is_Stopped
            // 
            this.is_Stopped.ForeColor = System.Drawing.Color.BlanchedAlmond;
            this.is_Stopped.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.is_Stopped.Location = new System.Drawing.Point(643, 238);
            this.is_Stopped.Margin = new System.Windows.Forms.Padding(4);
            this.is_Stopped.Name = "is_Stopped";
            this.is_Stopped.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.is_Stopped.Size = new System.Drawing.Size(27, 35);
            this.is_Stopped.TabIndex = 40;
            this.is_Stopped.UseVisualStyleBackColor = true;
            // 
            // treeAccounts
            // 
            this.treeAccounts.Location = new System.Drawing.Point(696, 154);
            this.treeAccounts.Margin = new System.Windows.Forms.Padding(4);
            this.treeAccounts.Name = "treeAccounts";
            this.treeAccounts.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.treeAccounts.Size = new System.Drawing.Size(474, 600);
            this.treeAccounts.TabIndex = 22;
            // 
            // alRowadToolBar
            // 
            this.alRowadToolBar.BackColor = System.Drawing.Color.Transparent;
            this.alRowadToolBar.Cursor = System.Windows.Forms.Cursors.PanNW;
            this.alRowadToolBar.Location = new System.Drawing.Point(68, 22);
            this.alRowadToolBar.Margin = new System.Windows.Forms.Padding(4);
            this.alRowadToolBar.Name = "alRowadToolBar";
            this.alRowadToolBar.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.alRowadToolBar.Size = new System.Drawing.Size(1003, 54);
            this.alRowadToolBar.TabIndex = 21;
            // 
            // GrBox_currencies
            // 
            this.GrBox_currencies.BackColor = System.Drawing.SystemColors.HighlightText;
            this.GrBox_currencies.Controls.Add(this.dgv_currencies);
            this.GrBox_currencies.Location = new System.Drawing.Point(6, 439);
            this.GrBox_currencies.Name = "GrBox_currencies";
            this.GrBox_currencies.Size = new System.Drawing.Size(682, 253);
            this.GrBox_currencies.TabIndex = 43;
            this.GrBox_currencies.TabStop = false;
            this.GrBox_currencies.Text = "العملات";
            // 
            // dgv_currencies
            // 
            this.dgv_currencies.AllowUserToAddRows = false;
            this.dgv_currencies.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_currencies.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Is_Active,
            this.Cur_Name,
            this.Is_Default,
            this.Is_Frozen,
            this.Cur_ID});
            this.dgv_currencies.Location = new System.Drawing.Point(69, 29);
            this.dgv_currencies.Name = "dgv_currencies";
            this.dgv_currencies.RowHeadersVisible = false;
            this.dgv_currencies.RowHeadersWidth = 62;
            this.dgv_currencies.RowTemplate.Height = 29;
            this.dgv_currencies.Size = new System.Drawing.Size(607, 207);
            this.dgv_currencies.TabIndex = 2;
            // 
            // Is_Active
            // 
            this.Is_Active.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Is_Active.DataPropertyName = "Is_Active";
            this.Is_Active.HeaderText = "▣";
            this.Is_Active.MinimumWidth = 8;
            this.Is_Active.Name = "Is_Active";
            this.Is_Active.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Is_Active.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.Is_Active.Width = 70;
            // 
            // Cur_Name
            // 
            this.Cur_Name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Cur_Name.DataPropertyName = "Cur_Name";
            this.Cur_Name.HeaderText = "اسم العملة";
            this.Cur_Name.MinimumWidth = 8;
            this.Cur_Name.Name = "Cur_Name";
            // 
            // Is_Default
            // 
            this.Is_Default.DataPropertyName = "Is_Default";
            this.Is_Default.HeaderText = "العملة الافتراضية ";
            this.Is_Default.MinimumWidth = 8;
            this.Is_Default.Name = "Is_Default";
            this.Is_Default.Width = 150;
            // 
            // Is_Frozen
            // 
            this.Is_Frozen.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Is_Frozen.DataPropertyName = "Is_Frozen";
            this.Is_Frozen.HeaderText = "توقيف";
            this.Is_Frozen.MinimumWidth = 8;
            this.Is_Frozen.Name = "Is_Frozen";
            this.Is_Frozen.Width = 70;
            // 
            // Cur_ID
            // 
            this.Cur_ID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Cur_ID.DataPropertyName = "Cur_ID";
            this.Cur_ID.HeaderText = "رقم العملة";
            this.Cur_ID.MinimumWidth = 8;
            this.Cur_ID.Name = "Cur_ID";
            this.Cur_ID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Cur_ID.Visible = false;
            this.Cur_ID.Width = 150;
            // 
            // acc_Nature
            // 
            this.acc_Nature.FormattingEnabled = true;
            this.acc_Nature.Location = new System.Drawing.Point(183, 314);
            this.acc_Nature.Name = "acc_Nature";
            this.acc_Nature.Size = new System.Drawing.Size(192, 33);
            this.acc_Nature.TabIndex = 43;
            this.acc_Nature.Tag = "Acc_Nature";
            // 
            // btnCollapseAll
            // 
            this.btnCollapseAll.Location = new System.Drawing.Point(696, 114);
            this.btnCollapseAll.Name = "btnCollapseAll";
            this.btnCollapseAll.Size = new System.Drawing.Size(48, 33);
            this.btnCollapseAll.TabIndex = 44;
            this.btnCollapseAll.Text = "طي";
            this.btnCollapseAll.UseVisualStyleBackColor = true;
            this.btnCollapseAll.Click += new System.EventHandler(this.btnCollapseAll_Click);
            // 
            // btnExpandAll
            // 
            this.btnExpandAll.Location = new System.Drawing.Point(1114, 114);
            this.btnExpandAll.Name = "btnExpandAll";
            this.btnExpandAll.Size = new System.Drawing.Size(56, 33);
            this.btnExpandAll.TabIndex = 45;
            this.btnExpandAll.Text = "توسعه";
            this.btnExpandAll.UseVisualStyleBackColor = true;
            this.btnExpandAll.Click += new System.EventHandler(this.btnExpandAll_Click);
            // 
            // Refresh
            // 
            this.Refresh.Location = new System.Drawing.Point(889, 114);
            this.Refresh.Name = "Refresh";
            this.Refresh.Size = new System.Drawing.Size(67, 33);
            this.Refresh.TabIndex = 46;
            this.Refresh.Text = "تحديث";
            this.Refresh.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.Refresh.UseVisualStyleBackColor = true;
            this.Refresh.Click += new System.EventHandler(this.Refresh_Click);
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
            // txt_CreatedAt
            // 
            this.txt_CreatedAt.Location = new System.Drawing.Point(241, 829);
            this.txt_CreatedAt.Name = "txt_CreatedAt";
            this.txt_CreatedAt.Size = new System.Drawing.Size(336, 30);
            this.txt_CreatedAt.TabIndex = 47;
            this.txt_CreatedAt.Tag = "Created_At";
            // 
            // txt_CreatedBy
            // 
            this.txt_CreatedBy.Location = new System.Drawing.Point(719, 829);
            this.txt_CreatedBy.Name = "txt_CreatedBy";
            this.txt_CreatedBy.Size = new System.Drawing.Size(336, 30);
            this.txt_CreatedBy.TabIndex = 48;
            this.txt_CreatedBy.Tag = "Created_By";
            // 
            // txt_UpdatedAt
            // 
            this.txt_UpdatedAt.Location = new System.Drawing.Point(241, 873);
            this.txt_UpdatedAt.Name = "txt_UpdatedAt";
            this.txt_UpdatedAt.Size = new System.Drawing.Size(336, 30);
            this.txt_UpdatedAt.TabIndex = 50;
            this.txt_UpdatedAt.Tag = "Updated_At";
            // 
            // txt_UpdatedBy
            // 
            this.txt_UpdatedBy.Location = new System.Drawing.Point(719, 873);
            this.txt_UpdatedBy.Name = "txt_UpdatedBy";
            this.txt_UpdatedBy.Size = new System.Drawing.Size(336, 30);
            this.txt_UpdatedBy.TabIndex = 49;
            this.txt_UpdatedBy.Tag = "Updated_By";
            // 
            // AccountsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.MediumBlue;
            this.ClientSize = new System.Drawing.Size(1183, 944);
            this.Controls.Add(label4);
            this.Controls.Add(label3);
            this.Controls.Add(label2);
            this.Controls.Add(label1);
            this.Controls.Add(this.txt_UpdatedAt);
            this.Controls.Add(this.txt_UpdatedBy);
            this.Controls.Add(this.txt_CreatedBy);
            this.Controls.Add(this.txt_CreatedAt);
            this.Controls.Add(this.Refresh);
            this.Controls.Add(this.btnExpandAll);
            this.Controls.Add(this.btnCollapseAll);
            this.Controls.Add(this.acc_Nature);
            this.Controls.Add(this.GrBox_currencies);
            this.Controls.Add(acc_IDLabel);
            this.Controls.Add(this.acc_ID);
            this.Controls.Add(acc_NameLabel);
            this.Controls.Add(this.acc_Name);
            this.Controls.Add(acc_Name_EnLabel);
            this.Controls.Add(this.acc_Name_En);
            this.Controls.Add(parent_IDLabel);
            this.Controls.Add(this.parent_ID);
            this.Controls.Add(account_LevelLabel);
            this.Controls.Add(this.account_Level);
            this.Controls.Add(acc_TypeLabel);
            this.Controls.Add(this.acc_Type);
            this.Controls.Add(acc_NatureLabel);
            this.Controls.Add(report_TypeLabel);
            this.Controls.Add(this.report_Type);
            this.Controls.Add(is_StoppedLabel);
            this.Controls.Add(this.is_Stopped);
            this.Controls.Add(this.treeAccounts);
            this.Controls.Add(this.alRowadToolBar);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "AccountsForm";
            this.Text = "AccountsForm";
            this.Load += new System.EventHandler(this.AccountsForm_Load);
            this.GrBox_currencies.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_currencies)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion
        private AlRowad_ERPDataSet alRowad_ERPDataSet;
        private BindingSource accountsBindingSource;
        private BindingNavigator accountsBindingNavigator;
        private ToolStripButton bindingNavigatorAddNewItem;
        private ToolStripLabel bindingNavigatorCountItem;
        private ToolStripButton bindingNavigatorDeleteItem;
        private ToolStripButton bindingNavigatorMoveFirstItem;
        private ToolStripButton bindingNavigatorMovePreviousItem;
        private ToolStripSeparator bindingNavigatorSeparator;
        private ToolStripTextBox bindingNavigatorPositionItem;
        private ToolStripSeparator bindingNavigatorSeparator1;
        private ToolStripButton bindingNavigatorMoveNextItem;
        private ToolStripButton bindingNavigatorMoveLastItem;
        private ToolStripSeparator bindingNavigatorSeparator2;
        private ToolStripButton accountsBindingNavigatorSaveItem;


        private System.Windows.Forms.TextBox acc_ID;
        private System.Windows.Forms.TextBox acc_Name;
        private System.Windows.Forms.TextBox acc_Name_En;
        private System.Windows.Forms.TextBox parent_ID;
        private System.Windows.Forms.TextBox account_Level;
        private System.Windows.Forms.ComboBox acc_Type;
        private System.Windows.Forms.ComboBox report_Type;
        private System.Windows.Forms.CheckBox is_Stopped;
        private System.Windows.Forms.TreeView treeAccounts;
        private Controls.AlRowadToolBar alRowadToolBar;
        private GroupBox GrBox_currencies;
        private ComboBox acc_Nature;
        private Button btnCollapseAll;
        private Button btnExpandAll;
        private Button Refresh;
        private ToolTip toolTip1;
        private FontAwesome.Sharp.IconToolStripButton iconToolStripButton1;
        private TextBox txt_CreatedAt;
        private TextBox txt_CreatedBy;
        private TextBox txt_UpdatedAt;
        private TextBox txt_UpdatedBy;
        private DataGridView dgv_currencies;
        private DataGridViewCheckBoxColumn Is_Active;
        private DataGridViewTextBoxColumn Cur_Name;
        private DataGridViewCheckBoxColumn Is_Default;
        private DataGridViewCheckBoxColumn Is_Frozen;
        private DataGridViewTextBoxColumn Cur_ID;
        // 🚀 أسطر حجز كائنات وأدوات الخلفية (Data Binding & TableAdapters)
        // الصق هذه الأسطر في أسفل الكلاس لتختفي الأخطاء فوراً
    }
}