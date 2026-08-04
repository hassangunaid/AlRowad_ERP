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
            this.AlRowad_ERPDataSet = new AlRowad_ERP.AlRowad_ERPDataSet();
            this.accounts_ChartBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.accountsTableAdapter = new AlRowad_ERP.AlRowad_ERPDataSetTableAdapters.AccountsTableAdapter();
            this.tableAdapterManager = new AlRowad_ERP.AlRowad_ERPDataSetTableAdapters.TableAdapterManager();
            this.accountsTableAdapter1 = new AlRowad_ERP.AlRowad_ERPDataSetTableAdapters.AccountsTableAdapter();
            this.accountsBindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.acc_ID = new System.Windows.Forms.TextBox();
            this.acc_Name = new System.Windows.Forms.TextBox();
            this.acc_Name_En = new System.Windows.Forms.TextBox();
            this.parent_ID = new System.Windows.Forms.TextBox();
            this.account_Level = new System.Windows.Forms.TextBox();
            this.acc_Type = new System.Windows.Forms.ComboBox();
            this.accountTypesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.report_Type = new System.Windows.Forms.ComboBox();
            this.accountReportsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.is_Stopped = new System.Windows.Forms.CheckBox();
            this.treeAccounts = new System.Windows.Forms.TreeView();
            this.alRowadToolBar = new AlRowad_ERP.Controls.AlRowadToolBar();
            this.account_Allowed_CurrenciesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.account_Allowed_CurrenciesTableAdapter = new AlRowad_ERP.AlRowad_ERPDataSetTableAdapters.Account_Allowed_CurrenciesTableAdapter();
            this.currenciesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.currenciesTableAdapter = new AlRowad_ERP.AlRowad_ERPDataSetTableAdapters.CurrenciesTableAdapter();
            this.GrBox_currencies = new System.Windows.Forms.GroupBox();
            this.dgv_currencies = new System.Windows.Forms.DataGridView();
            this.Is_Active = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Cur_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Is_Default = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Is_Frozen = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Cur_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.account_NaturesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.account_NaturesTableAdapter = new AlRowad_ERP.AlRowad_ERPDataSetTableAdapters.Account_NaturesTableAdapter();
            this.acc_Nature = new System.Windows.Forms.ComboBox();
            this.account_TypesTableAdapter = new AlRowad_ERP.AlRowad_ERPDataSetTableAdapters.Account_TypesTableAdapter();
            this.account_ReportsTableAdapter = new AlRowad_ERP.AlRowad_ERPDataSetTableAdapters.Account_ReportsTableAdapter();
            acc_IDLabel = new System.Windows.Forms.Label();
            acc_NameLabel = new System.Windows.Forms.Label();
            acc_Name_EnLabel = new System.Windows.Forms.Label();
            parent_IDLabel = new System.Windows.Forms.Label();
            account_LevelLabel = new System.Windows.Forms.Label();
            acc_TypeLabel = new System.Windows.Forms.Label();
            acc_NatureLabel = new System.Windows.Forms.Label();
            report_TypeLabel = new System.Windows.Forms.Label();
            is_StoppedLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.AlRowad_ERPDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.accounts_ChartBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.accountsBindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.accountTypesBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.accountReportsBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.account_Allowed_CurrenciesBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.currenciesBindingSource)).BeginInit();
            this.GrBox_currencies.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_currencies)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.account_NaturesBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // acc_IDLabel
            // 
            acc_IDLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            acc_IDLabel.AutoSize = true;
            acc_IDLabel.Cursor = System.Windows.Forms.Cursors.Default;
            acc_IDLabel.ForeColor = System.Drawing.Color.BlanchedAlmond;
            acc_IDLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            acc_IDLabel.Location = new System.Drawing.Point(35, 212);
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
            acc_NameLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            acc_NameLabel.AutoSize = true;
            acc_NameLabel.Cursor = System.Windows.Forms.Cursors.Default;
            acc_NameLabel.ForeColor = System.Drawing.Color.BlanchedAlmond;
            acc_NameLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            acc_NameLabel.Location = new System.Drawing.Point(35, 293);
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
            acc_Name_EnLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            acc_Name_EnLabel.AutoSize = true;
            acc_Name_EnLabel.ForeColor = System.Drawing.Color.BlanchedAlmond;
            acc_Name_EnLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            acc_Name_EnLabel.Location = new System.Drawing.Point(621, 289);
            acc_Name_EnLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            acc_Name_EnLabel.Name = "acc_Name_EnLabel";
            acc_Name_EnLabel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            acc_Name_EnLabel.Size = new System.Drawing.Size(118, 25);
            acc_Name_EnLabel.TabIndex = 27;
            acc_Name_EnLabel.Text = "اسم الحسابEn:";
            // 
            // parent_IDLabel
            // 
            parent_IDLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            parent_IDLabel.AutoSize = true;
            parent_IDLabel.ForeColor = System.Drawing.Color.BlanchedAlmond;
            parent_IDLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            parent_IDLabel.Location = new System.Drawing.Point(565, 208);
            parent_IDLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            parent_IDLabel.Name = "parent_IDLabel";
            parent_IDLabel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            parent_IDLabel.Size = new System.Drawing.Size(150, 25);
            parent_IDLabel.TabIndex = 29;
            parent_IDLabel.Text = "رقم الحساب الرئيسي:";
            // 
            // account_LevelLabel
            // 
            account_LevelLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            account_LevelLabel.AutoSize = true;
            account_LevelLabel.ForeColor = System.Drawing.Color.BlanchedAlmond;
            account_LevelLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            account_LevelLabel.Location = new System.Drawing.Point(621, 346);
            account_LevelLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            account_LevelLabel.Name = "account_LevelLabel";
            account_LevelLabel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            account_LevelLabel.Size = new System.Drawing.Size(142, 25);
            account_LevelLabel.TabIndex = 31;
            account_LevelLabel.Text = "Account Level:";
            // 
            // acc_TypeLabel
            // 
            acc_TypeLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            acc_TypeLabel.AutoSize = true;
            acc_TypeLabel.ForeColor = System.Drawing.Color.BlanchedAlmond;
            acc_TypeLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            acc_TypeLabel.Location = new System.Drawing.Point(621, 403);
            acc_TypeLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            acc_TypeLabel.Name = "acc_TypeLabel";
            acc_TypeLabel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            acc_TypeLabel.Size = new System.Drawing.Size(102, 25);
            acc_TypeLabel.TabIndex = 33;
            acc_TypeLabel.Text = "Acc Type:";
            // 
            // acc_NatureLabel
            // 
            acc_NatureLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            acc_NatureLabel.AutoSize = true;
            acc_NatureLabel.Cursor = System.Windows.Forms.Cursors.Default;
            acc_NatureLabel.ForeColor = System.Drawing.Color.BlanchedAlmond;
            acc_NatureLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            acc_NatureLabel.Location = new System.Drawing.Point(35, 405);
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
            report_TypeLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            report_TypeLabel.AutoSize = true;
            report_TypeLabel.ForeColor = System.Drawing.Color.BlanchedAlmond;
            report_TypeLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            report_TypeLabel.Location = new System.Drawing.Point(621, 458);
            report_TypeLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            report_TypeLabel.Name = "report_TypeLabel";
            report_TypeLabel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            report_TypeLabel.Size = new System.Drawing.Size(125, 25);
            report_TypeLabel.TabIndex = 37;
            report_TypeLabel.Text = "Report Type:";
            // 
            // is_StoppedLabel
            // 
            is_StoppedLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            is_StoppedLabel.AutoSize = true;
            is_StoppedLabel.Cursor = System.Windows.Forms.Cursors.Default;
            is_StoppedLabel.ForeColor = System.Drawing.Color.BlanchedAlmond;
            is_StoppedLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            is_StoppedLabel.Location = new System.Drawing.Point(35, 458);
            is_StoppedLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            is_StoppedLabel.Name = "is_StoppedLabel";
            is_StoppedLabel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            is_StoppedLabel.Size = new System.Drawing.Size(112, 25);
            is_StoppedLabel.TabIndex = 39;
            is_StoppedLabel.Text = "Is Stopped:";
            is_StoppedLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // AlRowad_ERPDataSet
            // 
            this.AlRowad_ERPDataSet.DataSetName = "AlRowad_ERPDataSet";
            this.AlRowad_ERPDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // accountsTableAdapter
            // 
            this.accountsTableAdapter.ClearBeforeFill = true;
            // 
            // tableAdapterManager
            // 
            this.tableAdapterManager.Account_Allowed_CurrenciesTableAdapter = null;
            this.tableAdapterManager.Account_NaturesTableAdapter = null;
            this.tableAdapterManager.Account_ReportsTableAdapter = null;
            this.tableAdapterManager.Account_TypesTableAdapter = null;
            this.tableAdapterManager.AccountsTableAdapter = null;
            this.tableAdapterManager.BackupDataSetBeforeUpdate = false;
            this.tableAdapterManager.Connection = null;
            this.tableAdapterManager.CurrenciesTableAdapter = null;
            this.tableAdapterManager.CustomersTableAdapter = null;
            this.tableAdapterManager.Doc_TypesTableAdapter = null;
            this.tableAdapterManager.Invoice_DetailsTableAdapter = null;
            this.tableAdapterManager.Invoice_HeaderTableAdapter = null;
            this.tableAdapterManager.Item_BalancesTableAdapter = null;
            this.tableAdapterManager.ItemsTableAdapter = null;
            this.tableAdapterManager.Journal_DetailsTableAdapter = null;
            this.tableAdapterManager.Journal_HeaderTableAdapter = null;
            this.tableAdapterManager.Payment_MethodsTableAdapter = null;
            this.tableAdapterManager.Payment_VouchersTableAdapter = null;
            this.tableAdapterManager.StoresTableAdapter = null;
            this.tableAdapterManager.SuppliersTableAdapter = null;
            this.tableAdapterManager.System_ShortcutsTableAdapter = null;
            this.tableAdapterManager.UnitsTableAdapter = null;
            this.tableAdapterManager.UpdateOrder = AlRowad_ERP.AlRowad_ERPDataSetTableAdapters.TableAdapterManager.UpdateOrderOption.InsertUpdateDelete;
            // 
            // accountsTableAdapter1
            // 
            this.accountsTableAdapter1.ClearBeforeFill = true;
            // 
            // acc_ID
            // 
            this.acc_ID.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.acc_ID.Location = new System.Drawing.Point(232, 208);
            this.acc_ID.Margin = new System.Windows.Forms.Padding(4);
            this.acc_ID.Name = "acc_ID";
            this.acc_ID.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.acc_ID.Size = new System.Drawing.Size(247, 30);
            this.acc_ID.TabIndex = 24;
            // 
            // acc_Name
            // 
            this.acc_Name.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.acc_Name.Location = new System.Drawing.Point(232, 289);
            this.acc_Name.Margin = new System.Windows.Forms.Padding(4);
            this.acc_Name.Name = "acc_Name";
            this.acc_Name.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.acc_Name.Size = new System.Drawing.Size(364, 30);
            this.acc_Name.TabIndex = 26;
            // 
            // acc_Name_En
            // 
            this.acc_Name_En.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.acc_Name_En.Location = new System.Drawing.Point(819, 286);
            this.acc_Name_En.Margin = new System.Windows.Forms.Padding(4);
            this.acc_Name_En.Name = "acc_Name_En";
            this.acc_Name_En.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.acc_Name_En.Size = new System.Drawing.Size(137, 30);
            this.acc_Name_En.TabIndex = 28;
            // 
            // parent_ID
            // 
            this.parent_ID.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.parent_ID.Location = new System.Drawing.Point(773, 204);
            this.parent_ID.Margin = new System.Windows.Forms.Padding(4);
            this.parent_ID.Name = "parent_ID";
            this.parent_ID.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.parent_ID.Size = new System.Drawing.Size(137, 30);
            this.parent_ID.TabIndex = 30;
            // 
            // account_Level
            // 
            this.account_Level.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.account_Level.Location = new System.Drawing.Point(819, 342);
            this.account_Level.Margin = new System.Windows.Forms.Padding(4);
            this.account_Level.Name = "account_Level";
            this.account_Level.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.account_Level.Size = new System.Drawing.Size(137, 30);
            this.account_Level.TabIndex = 32;
            // 
            // acc_Type
            // 
            this.acc_Type.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.acc_Type.Location = new System.Drawing.Point(785, 399);
            this.acc_Type.Margin = new System.Windows.Forms.Padding(4);
            this.acc_Type.Name = "acc_Type";
            this.acc_Type.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.acc_Type.Size = new System.Drawing.Size(171, 33);
            this.acc_Type.TabIndex = 34;
            // 
            // report_Type
            // 
            this.report_Type.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.report_Type.Location = new System.Drawing.Point(819, 454);
            this.report_Type.Margin = new System.Windows.Forms.Padding(4);
            this.report_Type.Name = "report_Type";
            this.report_Type.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.report_Type.Size = new System.Drawing.Size(137, 33);
            this.report_Type.TabIndex = 38;
            // 
            // accountReportsBindingSource
            // 
            this.accountReportsBindingSource.DataMember = "Account_Reports";
            this.accountReportsBindingSource.DataSource = this.AlRowad_ERPDataSet;
            // 
            // is_Stopped
            // 
            this.is_Stopped.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.is_Stopped.ForeColor = System.Drawing.Color.BlanchedAlmond;
            this.is_Stopped.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.is_Stopped.Location = new System.Drawing.Point(232, 451);
            this.is_Stopped.Margin = new System.Windows.Forms.Padding(4);
            this.is_Stopped.Name = "is_Stopped";
            this.is_Stopped.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.is_Stopped.Size = new System.Drawing.Size(139, 32);
            this.is_Stopped.TabIndex = 40;
            this.is_Stopped.Text = "checkBox1";
            this.is_Stopped.UseVisualStyleBackColor = true;
            // 
            // treeAccounts
            // 
            this.treeAccounts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeAccounts.Location = new System.Drawing.Point(991, 122);
            this.treeAccounts.Margin = new System.Windows.Forms.Padding(4);
            this.treeAccounts.Name = "treeAccounts";
            this.treeAccounts.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.treeAccounts.Size = new System.Drawing.Size(545, 840);
            this.treeAccounts.TabIndex = 22;
            // 
            // alRowadToolBar
            // 
            this.alRowadToolBar.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.alRowadToolBar.BackColor = System.Drawing.Color.Transparent;
            this.alRowadToolBar.Cursor = System.Windows.Forms.Cursors.PanNW;
            this.alRowadToolBar.Location = new System.Drawing.Point(486, 41);
            this.alRowadToolBar.Margin = new System.Windows.Forms.Padding(4);
            this.alRowadToolBar.Name = "alRowadToolBar";
            this.alRowadToolBar.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.alRowadToolBar.Size = new System.Drawing.Size(1003, 52);
            this.alRowadToolBar.TabIndex = 21;
            // 
            // account_Allowed_CurrenciesBindingSource
            // 
            this.account_Allowed_CurrenciesBindingSource.DataMember = "Account_Allowed_Currencies";
            this.account_Allowed_CurrenciesBindingSource.DataSource = this.AlRowad_ERPDataSet;
            // 
            // account_Allowed_CurrenciesTableAdapter
            // 
            this.account_Allowed_CurrenciesTableAdapter.ClearBeforeFill = true;
            // 
            // currenciesBindingSource
            // 
            this.currenciesBindingSource.DataMember = "Currencies";
            this.currenciesBindingSource.DataSource = this.AlRowad_ERPDataSet;
            // 
            // currenciesTableAdapter
            // 
            this.currenciesTableAdapter.ClearBeforeFill = true;
            // 
            // GrBox_currencies
            // 
            this.GrBox_currencies.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GrBox_currencies.BackColor = System.Drawing.SystemColors.HighlightText;
            this.GrBox_currencies.Controls.Add(this.dgv_currencies);
            this.GrBox_currencies.Location = new System.Drawing.Point(29, 670);
            this.GrBox_currencies.Name = "GrBox_currencies";
            this.GrBox_currencies.Size = new System.Drawing.Size(881, 240);
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
            this.dgv_currencies.Location = new System.Drawing.Point(250, 29);
            this.dgv_currencies.Name = "dgv_currencies";
            this.dgv_currencies.RowHeadersVisible = false;
            this.dgv_currencies.RowHeadersWidth = 62;
            this.dgv_currencies.RowTemplate.Height = 29;
            this.dgv_currencies.Size = new System.Drawing.Size(607, 207);
            this.dgv_currencies.TabIndex = 0;
            // 
            // Is_Active
            // 
            this.Is_Active.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
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
            this.Cur_Name.HeaderText = "اسم العملة";
            this.Cur_Name.MinimumWidth = 8;
            this.Cur_Name.Name = "Cur_Name";
            // 
            // Is_Default
            // 
            this.Is_Default.HeaderText = "العملة الافتراضية ";
            this.Is_Default.MinimumWidth = 8;
            this.Is_Default.Name = "Is_Default";
            this.Is_Default.Width = 150;
            // 
            // Is_Frozen
            // 
            this.Is_Frozen.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Is_Frozen.HeaderText = "توقيف";
            this.Is_Frozen.MinimumWidth = 8;
            this.Is_Frozen.Name = "Is_Frozen";
            this.Is_Frozen.Width = 70;
            // 
            // Cur_ID
            // 
            this.Cur_ID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Cur_ID.HeaderText = "رقم العملة";
            this.Cur_ID.MinimumWidth = 8;
            this.Cur_ID.Name = "Cur_ID";
            this.Cur_ID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Cur_ID.Visible = false;
            this.Cur_ID.Width = 150;
            // 
            // account_NaturesBindingSource
            // 
            this.account_NaturesBindingSource.DataMember = "Account_Natures";
            this.account_NaturesBindingSource.DataSource = this.AlRowad_ERPDataSet;
            // 
            // account_NaturesTableAdapter
            // 
            this.account_NaturesTableAdapter.ClearBeforeFill = true;
            // 
            // acc_Nature
            // 
            this.acc_Nature.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.acc_Nature.FormattingEnabled = true;
            this.acc_Nature.Location = new System.Drawing.Point(157, 402);
            this.acc_Nature.Name = "acc_Nature";
            this.acc_Nature.Size = new System.Drawing.Size(192, 33);
            this.acc_Nature.TabIndex = 43;
            // 
            // account_TypesTableAdapter
            // 
            this.account_TypesTableAdapter.ClearBeforeFill = true;
            // 
            // account_ReportsTableAdapter
            // 
            this.account_ReportsTableAdapter.ClearBeforeFill = true;
            // 
            // AccountsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Highlight;
            this.ClientSize = new System.Drawing.Size(1571, 982);
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
            ((System.ComponentModel.ISupportInitialize)(this.AlRowad_ERPDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.accounts_ChartBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.accountsBindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.accountTypesBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.accountReportsBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.account_Allowed_CurrenciesBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.currenciesBindingSource)).EndInit();
            this.GrBox_currencies.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_currencies)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.account_NaturesBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void NewMethod()
        {
            this.accountsTableAdapter = new AlRowad_ERP.AlRowad_ERPDataSetTableAdapters.AccountsTableAdapter();
        }

        #endregion
        private AlRowad_ERPDataSet alRowad_ERPDataSet;
        private BindingSource accountsBindingSource;
        //  private System.ComponentModel.IContainer components;
        private AlRowad_ERPDataSetTableAdapters.AccountsTableAdapter accountsTableAdapter;
        private AlRowad_ERPDataSetTableAdapters.TableAdapterManager tableAdapterManager;
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
        private BindingSource account_Allowed_CurrenciesBindingSource;
        private AlRowad_ERPDataSetTableAdapters.Account_Allowed_CurrenciesTableAdapter account_Allowed_CurrenciesTableAdapter;
        private BindingSource currenciesBindingSource;
        private AlRowad_ERPDataSetTableAdapters.CurrenciesTableAdapter currenciesTableAdapter;
        private GroupBox GrBox_currencies;
        private DataGridView dgv_currencies;
        private DataGridViewCheckBoxColumn Is_Active;
        private DataGridViewTextBoxColumn Cur_Name;
        private DataGridViewCheckBoxColumn Is_Default;
        private DataGridViewCheckBoxColumn Is_Frozen;
        private DataGridViewTextBoxColumn Cur_ID;
        private BindingSource account_NaturesBindingSource;
        private AlRowad_ERPDataSetTableAdapters.Account_NaturesTableAdapter account_NaturesTableAdapter;
        private ComboBox acc_Nature;
        private BindingSource accountTypesBindingSource;
        private AlRowad_ERPDataSetTableAdapters.Account_TypesTableAdapter account_TypesTableAdapter;
        private BindingSource accountReportsBindingSource;
        private AlRowad_ERPDataSetTableAdapters.Account_ReportsTableAdapter account_ReportsTableAdapter;
        // 🚀 أسطر حجز كائنات وأدوات الخلفية (Data Binding & TableAdapters)
        // الصق هذه الأسطر في أسفل الكلاس لتختفي الأخطاء فوراً
    }
}