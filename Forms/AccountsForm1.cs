using AlRowad_ERP.Core; // حل مشكلة عدم التعرف على DatabaseHelper آلياً
using System;
using System.Data;
using System.Data.SqlClient;
using System.Security.AccessControl;
using System.Windows.Forms;

namespace AlRowad_ERP.Forms
{
    public partial class AccountsForm1 : BaseEntryForm // الوراثة من كلاس الاختصارات العالمي (النخاع الشوكي)
    {
        // جدول بيانات مؤقت لحفظ الحسابات الخام من SQL Server لضمان سرعة المعالجة الشجرية
        private DataTable dtAccountsChart = new DataTable();

        public AccountsForm1()
        {
            InitializeComponent();
            // الدستور: تفعيل وضع الاستعراض (Read Only) افتراضياً فور فتح الشاشة لحماية البيانات
            SetBrowseMode(true);
        }

        // دالة التحكم بوضع الاستعراض (Browse Mode) بناءً على إعدادات واجهة المستخدم المعتمدة
        private void SetBrowseMode(bool isReadOnly)
        {
            acc_ID.ReadOnly = isReadOnly;
            acc_Name.ReadOnly = isReadOnly;
            acc_Type.ReadOnly = isReadOnly; // تم تحويلها إلى ReadOnly للحفاظ على تناسق التصميم بدلاً من Enabled
            acc_Nature.ReadOnly = isReadOnly;
        }

        // المحرك المركزي لجلب الحسابات وبناء الهيكل خماسي المستويات (أب -> ابن -> حفيد)
        private void BuildAccountsTreeStructure()
        {
            try
            {
                treeAccounts.Nodes.Clear();

                // الدستور: جلب الدليل المحاسبي الشجري كاملاً مرتباً برقم الحساب الفعلي المتواجد في قاعدة بياناتك (Acc_ID)
                string sqlQuery = "SELECT Acc_ID, Acc_Name, Parent_ID FROM Accounts ORDER BY Acc_ID";

                using (SqlCommand cmd = new SqlCommand(sqlQuery, DatabaseHelper.GetConnection()))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        dtAccountsChart.Clear();
                        da.Fill(dtAccountsChart);
                        }
                }

                // تصفية جلب المستوى الأول (الآباء الرئيسية التي ليس لها Parent_ID أو قيمتها فارغة)
                DataView dvRoot = new DataView(dtAccountsChart);
                dvRoot.RowFilter = "Parent_ID IS NULL OR Parent_ID = ''";

                foreach (DataRowView rowView in dvRoot)
                {
                    TreeNode rootNode = new TreeNode
                    {
                        Tag = rowView["Acc_ID"].ToString(),
                        Text = rowView["Acc_ID"].ToString() + " - " + rowView["Acc_Name"].ToString()
                    };

                    treeAccounts.Nodes.Add(rootNode);

                    // استدعاء المولد التكراري للغوص في بقية المستويات الأدنى (ابن -> حفيد...)
                    PopulateSubAccounts(rootNode, dtAccountsChart);
                }

                // الدستور: تظهر الشجرة في حالة طي (Collapsed) افتراضياً عند الفتح لمنع الازدحام المرئي
                treeAccounts.CollapseAll();
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show("خطأ في الوصول إلى قاعدة البيانات: " + sqlEx.Message, "خطأ قاعدة البيانات", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ برمي في معالجة الشجرة المحاسبية: " + ex.Message, "خطأ في النظام", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
        }

        // دالة الغوص التكراري الذكية لربط الحسابات التابعة حتى المستوى الخامس تلقائياً
        private void PopulateSubAccounts(TreeNode parentNode, DataTable dtSource)
        {
            string parentID = parentNode.Tag.ToString();

            DataView dvChildren = new DataView(dtSource);
            dvChildren.RowFilter = $"Parent_ID = '{parentID}'";

            foreach (DataRowView rowView in dvChildren)
            {
                TreeNode childNode = new TreeNode
                {
                    Tag = rowView["Acc_ID"].ToString(),
                    Text = rowView["Acc_ID"].ToString() + " - " + rowView["Acc_Name"].ToString()
                };

                parentNode.Nodes.Add(childNode);

                // النزول التكراري لدعم الهيكلية خماسية المستويات ديناميكياً
                PopulateSubAccounts(childNode, dtSource);
            }
        }

        // حدث الضغط على F5 للحفظ - مصحح بالكامل لمطابقة مسميات حقول قاعدة البيانات ومصقول برمجياً
        // حدث الضغط على الحفظ المطور: يفرق ديناميكياً بين الحساب الجديد والتعديل
        public override void OnSave()
        {
            if (string.IsNullOrWhiteSpace(acc_ID.Text) || string.IsNullOrWhiteSpace(acc_Name.Text))
            {
                MessageBox.Show("يرجى ملء الحقول السيادية للحساب قبل الحفظ.", "تنبيه جودة العمل", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string sqlQuery = "";

                // الفحص الدستوري الذكي لوضع العملية الحالية لشريط الأدوات
                if (EntrySource == "Edit")
                {
                    // حالة التعديل: تحديث البيانات بناءً على رقم الحساب المقفل
                    sqlQuery = @"UPDATE Accounts 
                                 SET Acc_Name = @Acc_Name, 
                                     Acc_Name_En = @Acc_Name_En, 
                                     Parent_ID = @Parent_ID, 
                                     Account_Level = @Account_Level, 
                                     Acc_Type = @Acc_Type, 
                                     Acc_Nature = @Acc_Nature, 
                                     Report_Type = @Report_Type, 
                                     Is_Stopped = @Is_Stopped 
                                 WHERE Acc_ID = @Acc_ID";
                }
                else
                {
                    // حالة الإضافة الجديدة القياسية
                    sqlQuery = @"INSERT INTO Accounts (Acc_ID, Acc_Name, Acc_Name_En, Parent_ID, Account_Level, Acc_Type, Acc_Nature, Report_Type, Is_Stopped) 
                                 VALUES (@Acc_ID, @Acc_Name, @Acc_Name_En, @Parent_ID, @Account_Level, @Acc_Type, @Acc_Nature, @Report_Type, @Is_Stopped)";
                }

                using (SqlCommand cmd = new SqlCommand(sqlQuery, DatabaseHelper.GetConnection()))
                {
                    // تمرير الباراميترز الموحدة للعمليتين
                    cmd.Parameters.AddWithValue("@Acc_ID", acc_ID.Text.Trim());
                    cmd.Parameters.AddWithValue("@Acc_Name", acc_Name.Text.Trim());
                    cmd.Parameters.AddWithValue("@Acc_Name_En", string.IsNullOrWhiteSpace(acc_Name_En.Text) ? (object)DBNull.Value : acc_Name_En.Text.Trim());

                    int level = 1;
                    int.TryParse(account_Level.Text, out level);
                    cmd.Parameters.AddWithValue("@Account_Level", level == 0 ? 1 : level);

                    int typeVal = 0, natureVal = 0, reportVal = 0;
                    int.TryParse(acc_Type.Text, out typeVal);
                    int.TryParse(acc_Nature.Text, out natureVal);
                    int.TryParse(report_Type.Text, out reportVal);

                    cmd.Parameters.AddWithValue("@Acc_Type", typeVal);
                    cmd.Parameters.AddWithValue("@Acc_Nature", natureVal);
                    cmd.Parameters.AddWithValue("@Report_Type", reportVal);
                    cmd.Parameters.AddWithValue("@Is_Stopped", is_Stopped.Checked);

                    // الحل: جلب رقم الأب مباشرة من الخانة النصية parent_ID لضمان عدم ضياع التبعية الشجرية أثناء التعديل
                    cmd.Parameters.AddWithValue("@Parent_ID", string.IsNullOrWhiteSpace(parent_ID.Text) ? (object)DBNull.Value : parent_ID.Text.Trim());

                    cmd.ExecuteNonQuery();
                    }

                MessageBox.Show("تم حفظ البيانات وتحديث الدليل بنجاح تام.", "تأكيد النظام", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // التعديل الحتمي لتنبيه شريط الأدوات الخاص بك بنجاح الحفظ وإعادة قفل الحقول
                this.ClearForm(this);   // تفريغ الحقول عبر دالة الدستور في الأب الأكبر
                EntrySource = "None";         // تصغير مؤشر الإدخال لحماية الإغلاق
                SetState(false);              // إعطاء أمر لشريط الأدوات وقفل الحقول بالعودة لوضع الاستعراض
                BuildAccountsTreeStructure(); // تحديث فوري للشجرة المحاسبية
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show("خطأ في قاعدة البيانات: " + sqlEx.Message, "خطأ في الحفظ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            catch (Exception ex)
            {
                MessageBox.Show("فشلت عملية الحفظ: " + ex.Message, "خطأ في قاعدة البيانات", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
        }
                 // دالة الـ Load الموحدة المعتمدة والمربوطة بالتصميم البصري بنجاح
        private void AccountsForm_Load_1(object sender, EventArgs e)
        {
            try
            {
                // 1. دعم العربية الكامل (RTL) وضبط تموضع الشجرة في جهة اليمين
                treeAccounts.RightToLeft = RightToLeft.Yes;
                treeAccounts.RightToLeftLayout = true;

                // 2. تحميل البيانات في الـ DataSet والـ TableAdapters التلقائية للنظام
                this.accounts_ChartTableAdapter.Fill(this.AlRowad_ERPDataSet.Accounts_Chart);
                this.accountsTableAdapter1.Fill(this.AlRowad_ERPDataSet.Accounts);

                // 3. التعديل الجذري: إيقاف الـ Data Binding التلقائي المزعج لكي تفتح الشاشة فارغة
                this.accountsBindingSource1.DataSource = null;

                // 4. بناء هيكلية الدليل المحاسبي الشجري خماسي المستويات
                BuildAccountsTreeStructure();

                // 5. تطبيق سياسة الدستور السيادية: تصفير وتطهير الواجهة تماماً عند الظهور الأول
                this.ClearForm(this);
                SetBrowseMode(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ أثناء تحميل بيانات الشاشة: " + ex.Message, "تنبيه جودة العمل", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void accountsBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.accountsBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.alRowad_ERPDataSet);
        }

        // تعاريف الكائنات وعناصر الواجهة التلقائية للفيجوال ستوديو
        private Controls.AlRowadToolBar alRowadToolBar1;
        private AlRowad_ERPDataSet alRowad_ERPDataSet;
        private BindingSource accountsBindingSource;
        private System.ComponentModel.IContainer components;
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

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.Label acc_IDLabel;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AccountsForm1));
            System.Windows.Forms.Label acc_NameLabel;
            System.Windows.Forms.Label acc_Name_EnLabel;
            System.Windows.Forms.Label parent_IDLabel;
            System.Windows.Forms.Label account_LevelLabel;
            System.Windows.Forms.Label acc_TypeLabel;
            System.Windows.Forms.Label acc_NatureLabel;
            System.Windows.Forms.Label report_TypeLabel;
            System.Windows.Forms.Label is_StoppedLabel;
            this.alRowadToolBar2 = new AlRowad_ERP.Controls.AlRowadToolBar();
            this.treeAccounts = new System.Windows.Forms.TreeView();
            this.AlRowad_ERPDataSet = new AlRowad_ERP.AlRowad_ERPDataSet();
            this.accounts_ChartBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.accounts_ChartTableAdapter = new AlRowad_ERP.AlRowad_ERPDataSetTableAdapters.Accounts_ChartTableAdapter();
            this.tableAdapterManager1 = new AlRowad_ERP.AlRowad_ERPDataSetTableAdapters.TableAdapterManager();
            this.accountsTableAdapter1 = new AlRowad_ERP.AlRowad_ERPDataSetTableAdapters.AccountsTableAdapter();
            this.accountsBindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.acc_ID = new System.Windows.Forms.TextBox();
            this.acc_Name = new System.Windows.Forms.TextBox();
            this.acc_Name_En = new System.Windows.Forms.TextBox();
            this.parent_ID = new System.Windows.Forms.TextBox();
            this.account_Level = new System.Windows.Forms.TextBox();
            this.acc_Type = new System.Windows.Forms.TextBox();
            this.acc_Nature = new System.Windows.Forms.TextBox();
            this.report_Type = new System.Windows.Forms.TextBox();
            this.is_Stopped = new System.Windows.Forms.CheckBox();
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
            this.SuspendLayout();
            // 
            // acc_IDLabel
            // 
            resources.ApplyResources(acc_IDLabel, "acc_IDLabel");
            acc_IDLabel.Name = "acc_IDLabel";
            // 
            // acc_NameLabel
            // 
            resources.ApplyResources(acc_NameLabel, "acc_NameLabel");
            acc_NameLabel.Name = "acc_NameLabel";
            // 
            // acc_Name_EnLabel
            // 
            resources.ApplyResources(acc_Name_EnLabel, "acc_Name_EnLabel");
            acc_Name_EnLabel.Name = "acc_Name_EnLabel";
            // 
            // parent_IDLabel
            // 
            resources.ApplyResources(parent_IDLabel, "parent_IDLabel");
            parent_IDLabel.Name = "parent_IDLabel";
            // 
            // account_LevelLabel
            // 
            resources.ApplyResources(account_LevelLabel, "account_LevelLabel");
            account_LevelLabel.Name = "account_LevelLabel";
            // 
            // acc_TypeLabel
            // 
            resources.ApplyResources(acc_TypeLabel, "acc_TypeLabel");
            acc_TypeLabel.Name = "acc_TypeLabel";
            // 
            // acc_NatureLabel
            // 
            resources.ApplyResources(acc_NatureLabel, "acc_NatureLabel");
            acc_NatureLabel.Name = "acc_NatureLabel";
            // 
            // report_TypeLabel
            // 
            resources.ApplyResources(report_TypeLabel, "report_TypeLabel");
            report_TypeLabel.Name = "report_TypeLabel";
            // 
            // is_StoppedLabel
            // 
            resources.ApplyResources(is_StoppedLabel, "is_StoppedLabel");
            is_StoppedLabel.Name = "is_StoppedLabel";
            // 
            // alRowadToolBar2
            // 
            resources.ApplyResources(this.alRowadToolBar2, "alRowadToolBar2");
            this.alRowadToolBar2.Name = "alRowadToolBar2";
            // 
            // treeAccounts
            // 
            resources.ApplyResources(this.treeAccounts, "treeAccounts");
            this.treeAccounts.Name = "treeAccounts";
            this.treeAccounts.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeAccounts_NodeMouseDoubleClick);
            // 
            // AlRowad_ERPDataSet
            // 
            this.AlRowad_ERPDataSet.DataSetName = "AlRowad_ERPDataSet";
            this.AlRowad_ERPDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // accounts_ChartBindingSource
            // 
            this.accounts_ChartBindingSource.DataMember = "Accounts_Chart";
            this.accounts_ChartBindingSource.DataSource = this.AlRowad_ERPDataSet;
            // 
            // accounts_ChartTableAdapter
            // 
            this.accounts_ChartTableAdapter.ClearBeforeFill = true;
            // 
            // tableAdapterManager1
            // 
            this.tableAdapterManager1.Account_Allowed_CurrenciesTableAdapter = null;
            this.tableAdapterManager1.Accounts_ChartTableAdapter = this.accounts_ChartTableAdapter;
            this.tableAdapterManager1.AccountsTableAdapter = this.accountsTableAdapter1;
            this.tableAdapterManager1.BackupDataSetBeforeUpdate = false;
            this.tableAdapterManager1.CurrenciesTableAdapter = null;
            this.tableAdapterManager1.CustomersTableAdapter = null;
            this.tableAdapterManager1.Doc_TypesTableAdapter = null;
            this.tableAdapterManager1.Invoice_DetailsTableAdapter = null;
            this.tableAdapterManager1.Invoice_HeaderTableAdapter = null;
            this.tableAdapterManager1.Item_BalancesTableAdapter = null;
            this.tableAdapterManager1.ItemsTableAdapter = null;
            this.tableAdapterManager1.Journal_DetailsTableAdapter = null;
            this.tableAdapterManager1.Journal_HeaderTableAdapter = null;
            this.tableAdapterManager1.Payment_MethodsTableAdapter = null;
            this.tableAdapterManager1.Payment_VouchersTableAdapter = null;
            this.tableAdapterManager1.StoresTableAdapter = null;
            this.tableAdapterManager1.SuppliersTableAdapter = null;
            this.tableAdapterManager1.System_ShortcutsTableAdapter = null;
            this.tableAdapterManager1.UnitsTableAdapter = null;
            this.tableAdapterManager1.UpdateOrder = AlRowad_ERP.AlRowad_ERPDataSetTableAdapters.TableAdapterManager.UpdateOrderOption.InsertUpdateDelete;
            // 
            // accountsTableAdapter1
            // 
            this.accountsTableAdapter1.ClearBeforeFill = true;
            // 
            // accountsBindingSource1
            // 
            this.accountsBindingSource1.DataMember = "Accounts";
            this.accountsBindingSource1.DataSource = this.AlRowad_ERPDataSet;
            this.accountsBindingSource1.CurrentChanged += new System.EventHandler(this.accountsBindingSource1_CurrentChanged);
            // 
            // acc_ID
            // 
            resources.ApplyResources(this.acc_ID, "acc_ID");
            this.acc_ID.Name = "acc_ID";
            // 
            // acc_Name
            // 
            resources.ApplyResources(this.acc_Name, "acc_Name");
            this.acc_Name.Name = "acc_Name";
            // 
            // acc_Name_En
            // 
            resources.ApplyResources(this.acc_Name_En, "acc_Name_En");
            this.acc_Name_En.Name = "acc_Name_En";
            // 
            // parent_ID
            // 
            resources.ApplyResources(this.parent_ID, "parent_ID");
            this.parent_ID.Name = "parent_ID";
            // 
            // account_Level
            // 
            resources.ApplyResources(this.account_Level, "account_Level");
            this.account_Level.Name = "account_Level";
            // 
            // acc_Type
            // 
            resources.ApplyResources(this.acc_Type, "acc_Type");
            this.acc_Type.Name = "acc_Type";
            // 
            // acc_Nature
            // 
            resources.ApplyResources(this.acc_Nature, "acc_Nature");
            this.acc_Nature.Name = "acc_Nature";
            // 
            // report_Type
            // 
            resources.ApplyResources(this.report_Type, "report_Type");
            this.report_Type.Name = "report_Type";
            // 
            // is_Stopped
            // 
            this.is_Stopped.DataBindings.Add(new System.Windows.Forms.Binding("CheckState", this.accountsBindingSource1, "Is_Stopped", true));
            resources.ApplyResources(this.is_Stopped, "is_Stopped");
            this.is_Stopped.Name = "is_Stopped";
            this.is_Stopped.UseVisualStyleBackColor = true;
            // 
            // AccountsForm1
            // 
            resources.ApplyResources(this, "$this");
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
            this.Controls.Add(this.acc_Nature);
            this.Controls.Add(report_TypeLabel);
            this.Controls.Add(this.report_Type);
            this.Controls.Add(is_StoppedLabel);
            this.Controls.Add(this.is_Stopped);
            this.Controls.Add(this.treeAccounts);
            this.Controls.Add(this.alRowadToolBar2);
            this.Name = "AccountsForm1";
            this.Load += new System.EventHandler(this.AccountsForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.AlRowad_ERPDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.accounts_ChartBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.accountsBindingSource1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private Controls.AlRowadToolBar alRowadToolBar2;
        private TreeView treeAccounts;
        private AlRowad_ERPDataSet AlRowad_ERPDataSet;
        private BindingSource accounts_ChartBindingSource;
        private AlRowad_ERPDataSetTableAdapters.Accounts_ChartTableAdapter accounts_ChartTableAdapter;
        private AlRowad_ERPDataSetTableAdapters.TableAdapterManager tableAdapterManager1;

        private void accounts_ChartBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.accounts_ChartBindingSource.EndEdit();
            this.tableAdapterManager1.UpdateAll(this.AlRowad_ERPDataSet);
        }

        // دالة الـ Load الفعلية المرتبطة بالواجهة - تم دمج البناء ودعم الـ RTL والدستور بداخلها
        private void AccountsForm_Load(object sender, EventArgs e)
        {
            try
            {
                // 1. دعم العربية الكامل (RTL) وضبط تموضع الشجرة في جهة اليمين طبقاً للدستور
                treeAccounts.RightToLeft = RightToLeft.Yes;
                treeAccounts.RightToLeftLayout = true;

                // 2. تحميل البيانات في الـ DataSets التلقائية للنظام
                this.accountsTableAdapter1.Fill(this.AlRowad_ERPDataSet.Accounts);
                this.accounts_ChartTableAdapter.Fill(this.AlRowad_ERPDataSet.Accounts_Chart);

                // 3. استدعاء المحرك المركزي لبناء شجرة الحسابات خماسية المستويات فوراً
                BuildAccountsTreeStructure();
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ أثناء تحميل بيانات الشاشة: " + ex.Message, "تنبيه جودة العمل", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // حدث النقر على أي حساب في الشجرة - مطوّر لجلب وتوزيع كافة الخانات المتاحة من جدول Accounts الفعلي
        private void treeAccounts_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node == null || string.IsNullOrEmpty(e.Node.Tag?.ToString())) return;

            try
            {
                // تم تصحيح مسميات الأعمدة هنا لتطابق الجدول الوظيفي Accounts الفعلي بنسبة 100%
                string query = "SELECT Acc_ID, Acc_Name, Acc_Name_En, Parent_ID, Account_Level, Acc_Type, Acc_Nature, Report_Type, Is_Stopped FROM Accounts WHERE Acc_ID = @Acc_ID";

                using (SqlCommand cmd = new SqlCommand(query, DatabaseHelper.GetConnection()))
                {
                    cmd.Parameters.AddWithValue("@Acc_ID", e.Node.Tag.ToString());

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // تعبئة الخانات المصممة بالبيانات الفعلية من واقع جدول الحسابات النظيف والمحمي
                            acc_ID.Text = reader["Acc_ID"].ToString();
                            acc_Name.Text = reader["Acc_Name"].ToString();
                            acc_Name_En.Text = reader["Acc_Name_En"] != DBNull.Value ? reader["Acc_Name_En"].ToString() : "";
                            parent_ID.Text = reader["Parent_ID"] != DBNull.Value ? reader["Parent_ID"].ToString() : "";
                            account_Level.Text = reader["Account_Level"].ToString();
                            acc_Type.Text = reader["Acc_Type"].ToString();
                            acc_Nature.Text = reader["Acc_Nature"].ToString();
                            report_Type.Text = reader["Report_Type"].ToString();
                            is_Stopped.Checked = Convert.ToBoolean(reader["Is_Stopped"]);

                            // الدستور الحتمي: تثبيت وضع الاستعراض (Read Only) حماية للبيانات عند التنقل
                            SetBrowseMode(true);
                        }
                    }
                    }
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ في جلب بيانات الحساب المحدّد: " + ex.Message, "خطأ في النظام", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
        }
        private AlRowad_ERPDataSetTableAdapters.AccountsTableAdapter accountsTableAdapter1;
        private BindingSource accountsBindingSource1;
        private TextBox acc_ID;
        private TextBox acc_Name;
        private TextBox acc_Name_En;
        private TextBox parent_ID;
        private TextBox account_Level;
        private TextBox acc_Type;
        private TextBox acc_Nature;
        private TextBox report_Type;
        private CheckBox is_Stopped;

        // التعديل: حدث النقر المزدوج لعرض تفاصيل الحساب كاملاً
        private void treeAccounts_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node == null || string.IsNullOrEmpty(e.Node.Tag?.ToString())) return;

            try
            {
                string query = "SELECT Acc_ID, Acc_Name, Acc_Name_En, Parent_ID, Account_Level, Acc_Type, Acc_Nature, Report_Type, Is_Stopped FROM Accounts WHERE Acc_ID = @Acc_ID";

                using (SqlCommand cmd = new SqlCommand(query, DatabaseHelper.GetConnection()))
                {
                    cmd.Parameters.AddWithValue("@Acc_ID", e.Node.Tag.ToString());

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            acc_ID.Text = reader["Acc_ID"].ToString();
                            acc_Name.Text = reader["Acc_Name"].ToString();
                            acc_Name_En.Text = reader["Acc_Name_En"] != DBNull.Value ? reader["Acc_Name_En"].ToString() : "";
                            parent_ID.Text = reader["Parent_ID"] != DBNull.Value ? reader["Parent_ID"].ToString() : "";
                            account_Level.Text = reader["Account_Level"].ToString();
                            acc_Type.Text = reader["Acc_Type"].ToString();
                            acc_Nature.Text = reader["Acc_Nature"].ToString();
                            report_Type.Text = reader["Report_Type"].ToString();
                            is_Stopped.Checked = Convert.ToBoolean(reader["Is_Stopped"]);

                            SetBrowseMode(true);
                        }
                    }
                    }
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ في جلب بيانات الحساب المحدّد: " + ex.Message, "خطأ في النظام", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
        }


        // الدستور: حماية الحقول السيادية عند استخدام أزرار شريط الأدوات (تعديل / جديد)
        protected override void SetState(bool editing)
        {
            // 1. استدعاء دالة الأب لكي تقوم بإقفال/فتح أزرار شريط الأدوات (AlRowadToolBar) ديناميكياً كالمعتاد
            base.SetState(editing);

            // 2. التحكم في خصائص القراءة فقط للحقول بناءً على وضع التعديل العام
            acc_ID.ReadOnly = !editing;
            acc_Name.ReadOnly = !editing;
            acc_Type.ReadOnly = !editing;
            acc_Nature.ReadOnly = !editing;

            // 3. منع تعديل رقم الحساب نهائياً إذا قام المستخدم بالضغط على زر "تعديل" من شريط الأدوات
            if (EntrySource == "Edit" && editing)
            {
                acc_ID.ReadOnly = true; // حماية الحساب من التغيير العشوائي لعدم ضرب القيود
            }
        }

        private void accountsBindingSource1_CurrentChanged(object sender, EventArgs e)
        {

        }
    }
}
