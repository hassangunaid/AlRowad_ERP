using AlRowad_ERP.Core; // حل مشكلة عدم التعرف على DatabaseHelper آلياً
using System;
using System.Data;
using System.Data.SqlClient;
using System.Security.AccessControl;
using System.Windows.Forms;

namespace AlRowad_ERP.Forms
{
    public partial class AccountsForm : BaseEntryForm // الوراثة من كلاس الاختصارات العالمي (النخاع الشوكي)
    {
        protected override string PrimaryIdFieldName => "acc_ID";
        // جدول بيانات مؤقت لحفظ الحسابات الخام من SQL Server لضمان سرعة المعالجة الشجرية
        private DataTable dtAccountsChart = new DataTable();

        public AccountsForm()
        {
            InitializeComponent();
            // الدستور: تفعيل وضع الاستعراض (Read Only) افتراضياً فور فتح الشاشة لحماية البيانات
            SetBrowseMode(true);

            this.Load += new System.EventHandler(this.AccountsForm_Load);
            this.parent_ID.TextChanged += new System.EventHandler(this.parent_ID_TextChanged);

            if (this.treeAccounts != null)
            {
                this.treeAccounts.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeAccounts_NodeMouseDoubleClick);
                this.treeAccounts.BeforeExpand += TreeAccounts_BeforeExpandCollapse;
                this.treeAccounts.BeforeCollapse += TreeAccounts_BeforeExpandCollapse;
            }
        }
        // 🎯 الدالة العبقرية لمنع الشجرة من الفتح والإغلاق التلقائي عند النقر المزدوج على اسم الحساب
        private void TreeAccounts_BeforeExpandCollapse(object sender, TreeViewCancelEventArgs e)
        {
            // إذا كان الفعل ناتجاً عن نقرة مستخدم بالماوس وليس برمجياً
            if (e.Action == TreeViewAction.ByMouse)
            {
                // إلغاء الفعل التلقائي (الـ Toggle) لكي تظل العقدة ثابتة ومستقرة
                e.Cancel = true;
            }
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // استدعاء دالة جلب العملات فور تشغيل الشاشة فعلياً
            تعبئة_قائمة_العملات_المتاحة();
        }

        private void تعبئة_قائمة_العملات_المتاحة()
        {
            try
            {
                // الاستعلام عن العملات الفعالة في النظام
                string query = "SELECT Cur_ID, Cur_Name FROM Currencies WHERE Is_Active = 1";

                using (SqlCommand cmd = new SqlCommand(query, DatabaseHelper.GetConnection()))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ أثناء تحميل العملات في شاشة الحسابات: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                }
        }
        // دالة التحكم بوضع الاستعراض (Browse Mode) بناءً على إعدادات واجهة المستخدم المعتمدة
        private void SetBrowseMode(bool isReadOnly)
        {
            acc_ID.ReadOnly = isReadOnly;
            acc_Name.ReadOnly = isReadOnly;
            acc_Type.Enabled = !isReadOnly; // تم تحويلها إلى ReadOnly للحفاظ على تناسق التصميم بدلاً من Enabled
            acc_Nature.Enabled = !isReadOnly;
            if (report_Type != null) report_Type.Enabled = !isReadOnly;
        }
        // ==========================================
        // 1. المحرك المركزي لبناء الشجرة وجلب البيانات
        // ==========================================
        private void BuildAccountsTreeStructure()
        {
            if (treeAccounts == null) return;
            try
            {
                // تنظيف الشجرة تماماً قبل إعادة البناء لعدم تكرار العقد
                treeAccounts.Nodes.Clear();

                // الدستور: جلب الدليل المحاسبي الشجري كاملاً مرتباً برقم الحساب (Acc_ID)
                string sqlQuery = "SELECT Acc_ID, Acc_Name, Parent_ID FROM Accounts ORDER BY Acc_ID";

                using (SqlCommand cmd = new SqlCommand(sqlQuery, DatabaseHelper.GetConnection()))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        dtAccountsChart.Clear();
                        da.Fill(dtAccountsChart);
                        }
                }

                // التحقق من أن الجدول يحتوي على بيانات فعلياً قبل البدء
                if (dtAccountsChart == null || dtAccountsChart.Rows.Count == 0)
                {
                    MessageBox.Show("تم الاتصال بنجاح، ولكن جدول الحسابات فارغ حالياً في قاعدة البيانات.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // تصفية جلب المستوى الأول (الآباء الرئيسية)
                // تم تدوير الشرط ليدعم: NULL، النص الفارغ ''، أو القيمة الصفرية '0' لضمان ملء الشجرة
                DataView dvRoot = new DataView(dtAccountsChart);
                dvRoot.RowFilter = "Parent_ID IS NULL OR Parent_ID = '' OR Parent_ID = '0' OR Parent_ID = 'root'";

                foreach (DataRowView rowView in dvRoot)
                {
                    TreeNode rootNode = new TreeNode
                    {
                        Tag = rowView["Acc_ID"].ToString().Trim(),
                        Text = rowView["Acc_ID"].ToString().Trim() + " - " + rowView["Acc_Name"].ToString().Trim()
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

        // ==========================================
        // 2. دالة الغوص التكراري الذكية لربط الحسابات التابعة (المستويات الأدنى)
        // ==========================================
        private void PopulateSubAccounts(TreeNode parentNode, DataTable dtSource)
        {
            string parentID = parentNode.Tag.ToString().Trim();

            DataView dvChildren = new DataView(dtSource);
            dvChildren.RowFilter = $"Parent_ID = '{parentID}'";

            foreach (DataRowView rowView in dvChildren)
            {
                TreeNode childNode = new TreeNode
                {
                    Tag = rowView["Acc_ID"].ToString().Trim(),
                    Text = rowView["Acc_ID"].ToString().Trim() + " - " + rowView["Acc_Name"].ToString().Trim()
                };

                parentNode.Nodes.Add(childNode);

                // النزول التكراري لدعم الهيكلية خماسية المستويات ديناميكياً (الابن يصبح أباً لأبنائه)
                PopulateSubAccounts(childNode, dtSource);
            }
        }
        private void تحميل_عملات_الحساب_بالجدول(string accId, bool isNewAccount = false)
        {
            try
            {
                string query = "";
                if (isNewAccount)
                {
                    query = "SELECT Cur_ID, Cur_Name, 0 AS Is_Active, 0 AS Is_Frozen, 0 AS Is_Default FROM Currencies WHERE Is_Active = 1";
                }
                else
                {
                    query = @"SELECT c.Cur_ID, c.Cur_Name, 
                             ISNULL(a.Is_Active, 0) AS Is_Active,
                             ISNULL(a.Is_Frozen, 0) AS Is_Frozen,
                             ISNULL(a.Is_Default, 0) AS Is_Default
                      FROM Currencies c
                      LEFT JOIN Account_Allowed_Currencies a 
                      ON c.Cur_ID = a.Cur_ID AND a.Acc_ID = @Acc_ID 
                      WHERE c.Is_Active = 1";
                }

                using (SqlCommand cmd = new SqlCommand(query, DatabaseHelper.GetConnection()))
                {
                    if (!isNewAccount)
                    {
                        cmd.Parameters.AddWithValue("@Acc_ID", accId.Trim());
                    }

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        dgv_currencies.Rows.Clear();

                        foreach (DataRow row in dt.Rows)
                        {
                            // 🎯 ضخ البيانات بالترتيب الدقيق المطابق لترتيب شاشتك بالصورة:
                            // 0: التفعيل، 1: اسم العملة، 2: العملة الافتراضية، 3: توقيف، 4: رقم العملة
                            dgv_currencies.Rows.Add(
                                Convert.ToBoolean(row["Is_Active"]),  // 0
                                row["Cur_Name"].ToString(),           // 1
                                Convert.ToBoolean(row["Is_Default"]), // 2 - العملة الافتراضية
                                Convert.ToBoolean(row["Is_Frozen"]),  // 3 - توقيف
                                row["Cur_ID"]                         // 4 - رقم العملة
                            );
                        }

                        // تشغيل القفل والتلوين الرمادي فوراً للعملات غير المختارة
                        تهيئة_حالة_خلايا_الجدول_الافتراضية();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ أثناء تحميل جدول العملات بالترتيب الجديد: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally { }
        }
        private void حفظ_جدول_عملات_الحساب(string accId)
        {
            int level = 0;
            int.TryParse(account_Level.Text, out level);
            if (level != 4 && level != 5) return;

            try
            {
                string deleteOldCurrencies = "DELETE FROM Account_Allowed_Currencies WHERE Acc_ID = @AccID";
                using (SqlCommand cmdDel = new SqlCommand(deleteOldCurrencies, DatabaseHelper.GetConnection()))
                {
                    cmdDel.Parameters.AddWithValue("@AccID", accId.Trim());
                    cmdDel.ExecuteNonQuery();
                }

                string insertQuery = @"INSERT INTO Account_Allowed_Currencies (Acc_ID, Cur_ID, Is_Active, Is_Frozen, Is_Default) 
                               VALUES (@AccID, @CurrID, @IsActive, @IsFrozen, @IsDefault)";

                foreach (DataGridViewRow row in dgv_currencies.Rows)
                {
                    if (row.IsNewRow) continue;

                    // كود العملة يقع الآن في الخلية رقم 4 بحسب الصورة المرفقة
                    if (row.Cells[4].Value == null || string.IsNullOrWhiteSpace(row.Cells[4].Value.ToString())) continue;

                    string curIdStr = row.Cells[4].Value.ToString();
                    bool act = row.Cells[0].Value != null && Convert.ToBoolean(row.Cells[0].Value);
                    bool isDef = row.Cells[2].Value != null && Convert.ToBoolean(row.Cells[2].Value); // عمود 2 الافتراضية
                    bool frz = row.Cells[3].Value != null && Convert.ToBoolean(row.Cells[3].Value); // عمود 3 التوقيف

                    if (act || frz)
                    {
                        using (SqlCommand cmdIns = new SqlCommand(insertQuery, DatabaseHelper.GetConnection()))
                        {
                            cmdIns.Parameters.AddWithValue("@AccID", accId.Trim());
                            cmdIns.Parameters.AddWithValue("@CurrID", Convert.ToInt32(curIdStr));
                            cmdIns.Parameters.AddWithValue("@IsActive", act);
                            cmdIns.Parameters.AddWithValue("@IsFrozen", frz);
                            cmdIns.Parameters.AddWithValue("@IsDefault", isDef);

                            cmdIns.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ أثناء حفظ الصلاحيات بالترتيب الجديد: " + ex.Message, "خطأ في النظام", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                }
        }
        // 🚀 المحرك الداخلي للحفظ الفعلي في شاشة الحسابات (يتعامل مع الـ SQL مباشرة)
        protected override bool ExecuteSaveToDatabase()
        {
            int checkLevel = 0;
            int.TryParse(account_Level.Text, out checkLevel);

            // 1. الشرط الدستوري للحسابات الفرعية (مستوى 4 أو 5)
            if (checkLevel == 4 || checkLevel == 5)
            {
                bool hasSelectedCurrency = false;
                bool hasDefaultCurrency = false;

                foreach (DataGridViewRow row in dgv_currencies.Rows)
                {
                    if (row.IsNewRow) continue;

                    // 🎯 تحديث الترتيب الصافي وفقاً للخريطة البصرية المعتمدة:
                    // Cell[0]: تفعيل العملة | Cell[2]: العملة الافتراضية | Cell[3]: توقيف العملة
                    bool act = row.Cells[0].Value != null && Convert.ToBoolean(row.Cells[0].Value);
                    bool isDef = row.Cells[2].Value != null && Convert.ToBoolean(row.Cells[2].Value);
                    bool frz = row.Cells[3].Value != null && Convert.ToBoolean(row.Cells[3].Value);

                    // تعتبر العملة مستخدمة للحساب إذا كانت مفعّلة للعمل أو مجمدة مؤقتاً
                    if (act || frz)
                    {
                        hasSelectedCurrency = true;

                        // فحص تحديد العملة الافتراضية
                        if (isDef)
                        {
                            hasDefaultCurrency = true;
                        }
                    }
                }

                if (!hasSelectedCurrency)
                {
                    MessageBox.Show("تنبيه جودة البيانات: لا يمكن حفظ حساب فرعي دون تفعيل عملة واحدة له على الأقل في جدول العملات.", "منع الحفظ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                if (!hasDefaultCurrency)
                {
                    MessageBox.Show("تنبيه جودة البيانات: يجب تحديد عملة واحدة لتكون العملة الافتراضية لهذا الحساب الفرعي.", "منع الحفظ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }

            string sqlQuery = "";

            if (EntrySource == "Edit")
            {
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
                sqlQuery = @"INSERT INTO Accounts (Acc_ID, Acc_Name, Acc_Name_En, Parent_ID, Account_Level, Acc_Type, Acc_Nature, Report_Type, Is_Stopped) 
          VALUES (@Acc_ID, @Acc_Name, @Acc_Name_En, @Parent_ID, @Account_Level, @Acc_Type, @Acc_Nature, @Report_Type, @Is_Stopped)";
            }

            try
            {
                using (SqlCommand cmd = new SqlCommand(sqlQuery, DatabaseHelper.GetConnection()))
                {
                    cmd.Parameters.AddWithValue("@Acc_ID", acc_ID.Text.Trim());
                    cmd.Parameters.AddWithValue("@Acc_Name", acc_Name.Text.Trim());
                    cmd.Parameters.AddWithValue("@Acc_Name_En", string.IsNullOrWhiteSpace(acc_Name_En.Text) ? (object)DBNull.Value : acc_Name_En.Text.Trim());

                    int level = 1;
                    int.TryParse(account_Level.Text, out level);
                    cmd.Parameters.AddWithValue("@Account_Level", level == 0 ? 1 : level);

                    int typeVal = 0, natureVal = 0, reportVal = 0;
                    int.TryParse(acc_Type.SelectedValue?.ToString(), out typeVal);
                    int.TryParse(acc_Nature.SelectedValue?.ToString(), out natureVal);
                    int.TryParse(report_Type.SelectedValue?.ToString(), out reportVal);

                    cmd.Parameters.AddWithValue("@Acc_Type", typeVal);
                    cmd.Parameters.AddWithValue("@Acc_Nature", natureVal);
                    cmd.Parameters.AddWithValue("@Report_Type", reportVal);
                    cmd.Parameters.AddWithValue("@Is_Stopped", is_Stopped.Checked);
                    cmd.Parameters.AddWithValue("@Parent_ID", string.IsNullOrWhiteSpace(parent_ID.Text) ? (object)DBNull.Value : parent_ID.Text.Trim());

                    cmd.ExecuteNonQuery();
                }

                // حفظ جدول العملات الفرعي بالهيكلية الجديدة المحمية
                حفظ_جدول_عملات_الحساب(acc_ID.Text.Trim());
            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث خطأ أثناء حفظ الحساب في قاعدة البيانات: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            finally
            {
                }

            // ─── [التحكم بالتحديث والاحتفاظ بالبيانات حياً] ───
            this.Invoke((MethodInvoker)delegate
            {
                // 🎯 تم إزالة دالة التصفير بنجاح لتبقي البيانات معروضة للتأمل والمراجعة
                BuildAccountsTreeStructure();
            });

            // إجبار الشاشة على العودة لوضع الاستعراض وتجميد كافة العناصر فوراً عبر دالة الأب الموحدة
            EntrySource = "Browse";
            SetState(false);

            return true;
        }
        public override void OnSave()
        {
            if (string.IsNullOrWhiteSpace(acc_ID.Text) || string.IsNullOrWhiteSpace(acc_Name.Text))
            {
                MessageBox.Show("يرجى ملء الحقول السيادية للحساب قبل الحفظ.", "تنبيه جودة العمل", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // استدعاء دالة الأب السيادية التي ستحتضن دالة الـ ExecuteSaveToDatabase أعلاه
            base.OnSave();
        }
        private void accountsBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.accountsBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.alRowad_ERPDataSet);

        }
        private void تهيئة_القوائم_المنسدلة_للحسابات()
        {
            try
            {
                // 1. نوع الحساب
                using (SqlDataAdapter da = new SqlDataAdapter("SELECT Type_ID, Type_Name FROM Account_Types ORDER BY Type_ID", DatabaseHelper.GetConnection()))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    acc_Type.DataSource = dt;
                    acc_Type.DisplayMember = "Type_Name"; // يعرض الاسم العربي
                    acc_Type.ValueMember = "Type_ID";     // يحتفظ بالرقم خلف الكواليس
                }

                // 2. طبيعة الحساب
                using (SqlDataAdapter da = new SqlDataAdapter("SELECT Nature_ID, Nature_Name FROM Account_Natures ORDER BY Nature_ID", DatabaseHelper.GetConnection()))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    acc_Nature.DataSource = dt;
                    acc_Nature.DisplayMember = "Nature_Name";
                    acc_Nature.ValueMember = "Nature_ID";
                }

                // 3. نوع التقرير
                using (SqlDataAdapter da = new SqlDataAdapter("SELECT Report_ID, Report_Name FROM Account_Reports ORDER BY Report_ID", DatabaseHelper.GetConnection()))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    report_Type.DataSource = dt;
                    report_Type.DisplayMember = "Report_Name";
                    report_Type.ValueMember = "Report_ID";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ أثناء تعبئة القوائم: " + ex.Message);
            }
            finally
            {
                }
        }
        private void accounts_ChartBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.accountsBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.alRowad_ERPDataSet);

        }

        // دالة الـ Load الفعلية المرتبطة بالواجهة - تم دمج البناء ودعم الـ RTL والدستور بداخلها
        // دالة الـ Load الموحدة والوحيدة المعتمدة والمربوطة بالتصميم البصري بنجاح
        private void AccountsForm_Load(object sender, EventArgs e)
        {
            try
            {
                // 1. دعم العربية الكامل (RTL) وضبط تموضع الشجرة في جهة اليمين
                treeAccounts.RightToLeft = RightToLeft.Yes;
                treeAccounts.RightToLeftLayout = true;

                // 2. شحن مراجع العملات والعملات المتاحة
                this.currenciesTableAdapter.Fill(this.AlRowad_ERPDataSet.Currencies);
                this.account_Allowed_CurrenciesTableAdapter.Fill(this.AlRowad_ERPDataSet.Account_Allowed_Currencies);
                this.dgv_currencies.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_currencies_CellContentClick);
                // 3. 🌟 استدعاء دالتنا هنا لشحن وتجهيز القوائم المنسدلة (قبل ربط أي وضع شاشة)
                تهيئة_القوائم_المنسدلة_للحسابات();

                // 4. بناء هيكلية الدليل المحاسبي الشجري خماسي المستويات
                BuildAccountsTreeStructure();

                // 5. تطبيق سياسة الدستور السيادية: تصفير وتطهير الواجهة وتفعيل وضع الاستعراض
                if (this.ParentForm == null)
                {
                    SetBrowseMode(true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ أثناء تحميل بيانات الشاشة: " + ex.Message, "تنبيه جودة العمل", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }        // التعديل: حدث النقر المزدوج لعرض تفاصيل الحساب كاملاً

        private void dgv_currencies_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                dgv_currencies.CommitEdit(DataGridViewDataErrorContexts.Commit);

                bool isActive = Convert.ToBoolean(dgv_currencies.Rows[e.RowIndex].Cells[0].Value);
                bool isDefault = Convert.ToBoolean(dgv_currencies.Rows[e.RowIndex].Cells[2].Value); // عمود 2
                bool isFrozen = Convert.ToBoolean(dgv_currencies.Rows[e.RowIndex].Cells[3].Value);  // عمود 3
                string currencyName = dgv_currencies.Rows[e.RowIndex].Cells[1].Value?.ToString();

                // ----------------------------------------------------
                // ❌ الرقابة الجوهرية (العمود الأول): التحكم في التفعيل والقفل حياً
                // ----------------------------------------------------
                if (e.ColumnIndex == 0)
                {
                    if (isActive)
                    {
                        dgv_currencies.Rows[e.RowIndex].Cells[2].ReadOnly = false;
                        dgv_currencies.Rows[e.RowIndex].Cells[3].ReadOnly = false;

                        dgv_currencies.Rows[e.RowIndex].Cells[2].Style.BackColor = System.Drawing.Color.White;
                        dgv_currencies.Rows[e.RowIndex].Cells[3].Style.BackColor = System.Drawing.Color.White;
                    }
                    else
                    {
                        dgv_currencies.Rows[e.RowIndex].Cells[2].Value = false;
                        dgv_currencies.Rows[e.RowIndex].Cells[3].Value = false;

                        dgv_currencies.Rows[e.RowIndex].Cells[2].ReadOnly = true;
                        dgv_currencies.Rows[e.RowIndex].Cells[3].ReadOnly = true;

                        dgv_currencies.Rows[e.RowIndex].Cells[2].Style.BackColor = System.Drawing.Color.LightGray;
                        dgv_currencies.Rows[e.RowIndex].Cells[3].Style.BackColor = System.Drawing.Color.LightGray;
                    }
                    dgv_currencies.RefreshEdit();
                    return;
                }

                // حماية أمنية من النقرات العشوائية وهي مقفلة
                if (!isActive && (e.ColumnIndex == 2 || e.ColumnIndex == 3))
                {
                    dgv_currencies.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = false;
                    dgv_currencies.RefreshEdit();
                    return;
                }

                // ----------------------------------------------------
                // 🌟 سياسة عمود "العملة الافتراضية" (العمود رقم 2)
                // ----------------------------------------------------
                if (e.ColumnIndex == 2)
                {
                    if (isDefault == true)
                    {
                        if (isFrozen == true)
                        {
                            dgv_currencies.Rows[e.RowIndex].Cells[2].Value = false;
                            dgv_currencies.RefreshEdit();
                            MessageBox.Show("محاسبياً: لا يمكن تعيين عملة موقوفة/مجمدة كعملة افتراضية للحساب.", "الرقابة البرمجية", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        // الاختيار الأحادي للعملة الافتراضية في الجدول
                        foreach (DataGridViewRow row in dgv_currencies.Rows)
                        {
                            if (row.Index != e.RowIndex && !row.IsNewRow)
                            {
                                row.Cells[2].Value = false;
                            }
                        }
                        dgv_currencies.RefreshEdit();
                    }
                }

                // ----------------------------------------------------
                // ❄️ سياسة عمود "توقيف" (العمود رقم 3)
                // ----------------------------------------------------
                if (e.ColumnIndex == 3)
                {
                    if (isFrozen == true)
                    {
                        if (isDefault == true)
                        {
                            dgv_currencies.Rows[e.RowIndex].Cells[3].Value = false;
                            dgv_currencies.RefreshEdit();
                            MessageBox.Show("محاسبياً: لا يمكن إيقاف العمل بالعملة الافتراضية للحساب. قم بتغيير العملة الافتراضية أولاً.", "منع التضارب المحاسبي", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            return;
                        }
                    }
                }
            }
        }
        private void تهيئة_حالة_خلايا_الجدول_الافتراضية()
        {
            foreach (DataGridViewRow row in dgv_currencies.Rows)
            {
                if (row.IsNewRow) continue;

                if (row.Cells[0].Value == null) row.Cells[0].Value = false;
                if (row.Cells[2].Value == null) row.Cells[2].Value = false; // الافتراضي
                if (row.Cells[3].Value == null) row.Cells[3].Value = false; // التوقيف

                bool isActive = Convert.ToBoolean(row.Cells[0].Value);

                if (!isActive)
                {
                    row.Cells[2].Value = false;
                    row.Cells[3].Value = false;

                    row.Cells[2].ReadOnly = true; // قفل الافتراضي
                    row.Cells[3].ReadOnly = true; // قفل التوقيف

                    row.Cells[2].Style.BackColor = System.Drawing.Color.LightGray;
                    row.Cells[3].Style.BackColor = System.Drawing.Color.LightGray;
                }
                else
                {
                    row.Cells[2].ReadOnly = false;
                    row.Cells[3].ReadOnly = false;

                    row.Cells[2].Style.BackColor = System.Drawing.Color.White;
                    row.Cells[3].Style.BackColor = System.Drawing.Color.White;
                }
            }
            dgv_currencies.Refresh();
        }
        private void treeAccounts_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node == null || string.IsNullOrEmpty(e.Node.Tag?.ToString())) return;

            // الدستور: منع جلب تفاصيل أخرى إذا كان المستخدم في وضع إدخال أو تعديل نشط
            if (EntrySource == "New" || EntrySource == "Edit") return;

            string selectedAccIdStr = e.Node.Tag.ToString();
            try
            {
                string query = "SELECT Acc_ID, Acc_Name, Acc_Name_En, Parent_ID, Account_Level, Acc_Type, Acc_Nature, Report_Type, Is_Stopped FROM Accounts WHERE Acc_ID = @Acc_ID";

                DataTable dtDetails = new DataTable();

                // فتح الاتصال وسحب البيانات بشكل فوري وآمن عبر الـ Adapter
                using (SqlCommand cmd = new SqlCommand(query, DatabaseHelper.GetConnection()))
                {
                    cmd.Parameters.AddWithValue("@Acc_ID", selectedAccIdStr);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtDetails);
                    }
                }
                // إغلاق فوري للاتصال لحمايته

                // التحقق من وجود الحساب وتوزيع البيانات على الحقول
                if (dtDetails.Rows.Count > 0)
                {
                    DataRow row = dtDetails.Rows[0];

                    acc_ID.Text = row["Acc_ID"].ToString();
                    acc_Name.Text = row["Acc_Name"].ToString();
                    acc_Name_En.Text = row["Acc_Name_En"] != DBNull.Value ? row["Acc_Name_En"].ToString() : "";
                    parent_ID.Text = row["Parent_ID"] != DBNull.Value ? row["Parent_ID"].ToString() : "";
                    account_Level.Text = row["Account_Level"].ToString();

                    // 🎯 تحديث آمن لاختيار القيمة الصحيحة في الـ ComboBox خلف الكواليس
                    if (acc_Type != null) acc_Type.SelectedValue = row["Acc_Type"];
                    if (acc_Nature != null) acc_Nature.SelectedValue = row["Acc_Nature"];
                    if (report_Type != null) report_Type.SelectedValue = row["Report_Type"];

                    is_Stopped.Checked = Convert.ToBoolean(row["Is_Stopped"]);

                    // قراءة مستوى الحساب والتحكم التفاعلي في جدول العملات
                    int currentLevel = Convert.ToInt32(row["Account_Level"]);
                    تحميل_جدول_عملات_الحساب(selectedAccIdStr);

                    if (currentLevel == 4 || currentLevel == 5)
                    {
                        dgv_currencies.Enabled = true; // تفعيل الجدول للحسابات الفرعية
                        string currentAccountId = Convert.ToString(selectedAccIdStr);
                        تحميل_عملات_الحساب_بالجدول(currentAccountId, false);
                    }
                    else
                    {
                        dgv_currencies.Rows.Clear();    // تنظيف الجدول للحسابات الرئيسية
                        dgv_currencies.Enabled = false; // تجميد الجدول تماماً للحسابات الرئيسية
                    }

                    // 🎯 تثبيت وضع الاستعراض الصارم وإجبار كل الحقول (بما فيها القوائم) على القفل والبهتان
                    EntrySource = "Browse";
                    SetState(false); // إخبار الأب بقفل كل الخانات (false تعني قفل الحقول وتفعيل أزرار شريط الأدوات)
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ في جلب بيانات الحساب المحدّد: " + ex.Message, "خطأ في النظام", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
        }        // الدستور: حماية الحقول السيادية عند استخدام أزرار شريط الأدوات (تعديل / جديد)

        private void تحميل_جدول_عملات_الحساب(string accId)
        {
            try
            {
                // 1. أولاً: نقوم بإلغاء تفعيل كافة الأسطر في الـ Grid كإعادة تعيين (Reset)
                foreach (DataGridViewRow row in dgv_currencies.Rows)
                {
                    if (row.IsNewRow) continue;
                    row.Cells[0].Value = false; // إلغاء النشط
                    row.Cells[2].Value = false; // إلغاء الافتراضي (العمود 2)
                    row.Cells[3].Value = false; // إلغاء التوقيف (العمود 3)
                }

                // 2. فتح الاتصال وجلب البيانات المخزنة لهذا الحساب من جدول الربط
                string query = "SELECT Cur_ID, Is_Active, Is_Frozen, Is_Default FROM Account_Allowed_Currencies WHERE Acc_ID = @Acc_ID";

                using (SqlCommand cmd = new SqlCommand(query, DatabaseHelper.GetConnection()))
                {
                    cmd.Parameters.AddWithValue("@Acc_ID", accId);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string curId = reader["Cur_ID"].ToString();
                            bool isActive = Convert.ToBoolean(reader["Is_Active"]);
                            bool isFrozen = Convert.ToBoolean(reader["Is_Frozen"]);
                            bool isDefault = Convert.ToBoolean(reader["Is_Default"]);

                            // 3. البحث عن العملة المطابقة داخل الـ DataGridView وتحديث حالتها بالترتيب الدقيق لشاشتك
                            foreach (DataGridViewRow row in dgv_currencies.Rows)
                            {
                                // رقم العملة يقع في الخلية رقم 4
                                if (row.Cells[4].Value != null && row.Cells[4].Value.ToString() == curId)
                                {
                                    row.Cells[0].Value = isActive;  // عمود 0: تفعيل
                                    row.Cells[2].Value = isDefault; // عمود 2: العملة الافتراضية
                                    row.Cells[3].Value = isFrozen;  // عمود 3: توقيف
                                    break;
                                }
                            }
                        }
                    }
                }

                // تشغيل الحمية البصرية لتلوين العملات غير النشطة بالرمادي فور تحميل الحساب
                تهيئة_حالة_خلايا_الجدول_الافتراضية();
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ أثناء تحميل صلاحيات العملات للحساب: " + ex.Message, "خطأ في النظام", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                }
        }

        public override void OnDelete()
        {
            // 1. التأكد من أن المستخدم قام باختيار حساب من الشجرة أولاً، وأنه في وضع الاستعراض (Browse)
            if (string.IsNullOrEmpty(acc_ID.Text) || EntrySource != "Browse") // تعديل: استخدام EntrySource بدلاً من CurrentMode
            {
                MessageBox.Show("تنبيه: يجب استعراض أو اختيار الحساب الرئيسي المراد حذفه أولاً.", "منع الحذف", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2. رسالة تأكيد تحذيرية للمستخدم
            DialogResult result = MessageBox.Show($"تنبيه هام: هل أنت متأكد من رغبتك في حذف الحساب: ({acc_Name.Text}) نهائياً من الدليل المحاسبي؟",
                                                  "تأكيد حذف حساب", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

            if (result == DialogResult.Yes)
            {
                ExecuteDeleteParentAccount(acc_ID.Text.Trim());
            }
        }

        private void ExecuteDeleteParentAccount(string parentAccountCode)
        {
            try
            {
                // الخطوة 1: الفحص الشجري الأمني - التحقق من وجود أي حسابات أبناء (تحليلية أو فرعية) تابعة لهذا الحساب
                string checkChildrenQuery = @"SELECT COUNT(*) FROM Accounts 
                                      WHERE Acc_ID LIKE @Pattern 
                                        AND Acc_ID != @ParentID";

                using (SqlCommand cmdCheck = new SqlCommand(checkChildrenQuery, DatabaseHelper.GetConnection()))
                {
                    cmdCheck.Parameters.AddWithValue("@Pattern", parentAccountCode + "%");
                    cmdCheck.Parameters.AddWithValue("@ParentID", parentAccountCode);

                    int childrenCount = Convert.ToInt32(cmdCheck.ExecuteScalar());

                    if (childrenCount > 0)
                    {
                        MessageBox.Show($"فشل الحذف: لا يمكن حذف هذا الحساب لوجود ({childrenCount}) حسابات فرعية/تحليلية تابعة له في الدليل المحاسبي. يجب حذف أو نقل الحسابات الفرعية أولاً.",
                                        "خرق سلامة الشجرة المحاسبية", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                }

                // الخطوة 2: تنفيذ الحذف الفعلي للحساب من جدول الحسابات
                string deleteQuery = "DELETE FROM Accounts WHERE Acc_ID = @ParentID";
                using (SqlCommand cmdDelete = new SqlCommand(deleteQuery, DatabaseHelper.GetConnection()))
                {
                    cmdDelete.Parameters.AddWithValue("@ParentID", parentAccountCode);
                    int rowsAffected = cmdDelete.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("تم حذف الحساب بنجاح من الدليل المحاسبي.", "تأكيد النظام", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // تنظيف الشاشة وإعادتها لوضع الاستعراض الصحيح
                        تفريغ_خانات_الحسابات_الرئيسية();

                        // حل المشكلة: تمرير true ليتوافق مع بارامتر الدالة المعرّفة في كودك
                        SetBrowseMode(true);

                        // إعادة بناء الشجرة المحوسبة لكي يختفي الحساب المحذوف فوراً أمام المستخدم
                        BuildAccountsTreeStructure();
                    }
                    else
                    {
                        MessageBox.Show("لم يتم العثور على الحساب في قاعدة البيانات، قد يكون قد حُذف مسبقاً.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
            }
            catch (SqlException ex)
            {
                // الفحص المحاسبي: منع الحذف إذا كان الحساب مرتبطاً بقيود أو حركات سابقة (Foreign Key constraint - Error 547)
                if (ex.Number == 547)
                {
                    MessageBox.Show("فشل الحذف الأمني: هذا الحساب مرتبط بمعاملات مالية أو جداول أخرى داخل النظام (مثل حركات القيود أو صلاحيات العملات) ولا يمكن الاستغناء عنه حالياً.",
                                    "حماية الحسابات المالية", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                else
                {
                    MessageBox.Show($"حدث خطأ في السيرفر أثناء محاولة الحذف: {ex.Message}", "خطأ قاعدة بيانات", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"حدث خطأ عام: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                }
        }

        // دالة مساعدة لتصفير الحقول بعد عملية الحذف
        private void تفريغ_خانات_الحسابات_الرئيسية()
        {
            acc_ID.Clear();
            acc_Name.Clear();
            // أضف هنا أي حقول إضافية في شاشة الحسابات الرئيسية مثل (نوع الحساب، مستوى الحساب، إلخ) ليتم تصفيرها
        }

        protected override void SetState(bool editing)
        {
            // 1. دع الأب يقوم بواجبه القومي في تفعيل وإيقاف كل الأدوات والأزرار
            base.SetState(editing);

            // 2. فرض الحماية المحاسبية على الحقل المفتاحي (Primary Key) في وضع التعديل فقط
            if (EntrySource == "Edit" && editing)
            {
                acc_ID.ReadOnly = true;
                acc_ID.Enabled = false; // حماية مطلقة لكي لا يتغير الرقم وتتدمر القيود
            }
            else
            {
                acc_ID.ReadOnly = !editing;
            }
        }        // حدث الضغط على F2 (جديد) - لفتح الحقول وتصفيرها تمهيداً للإدخال
        public override void OnNew()
        {
            // 1. تصفير الحقول الأساسية
            acc_ID.Text = "";
            acc_Name.Text = "";
            acc_Name_En.Text = "";

            // 2. فحص العقدة المحددة حالياً في الشجرة لتحديد الأب والمستوى
            string parentIdValue = "";
            int calculatedLevel = 5; // الافتراضي للحسابات الفرعية

            if (treeAccounts.SelectedNode != null)
            {
                int selectedNodeLevel = treeAccounts.SelectedNode.Level + 1;

                if (selectedNodeLevel >= 5)
                {
                    // الحساب المحدد مستواه 5 -> الحساب الجديد سيكون "أخاً" له ويرث نفس الأب
                    if (treeAccounts.SelectedNode.Parent != null)
                    {
                        parentIdValue = treeAccounts.SelectedNode.Parent.Tag.ToString();
                    }
                    else
                    {
                        parentIdValue = parent_ID.Text.Trim();
                    }
                    calculatedLevel = 5;
                }
                else
                {
                    // الحساب المختار رئيسي -> الحساب الجديد سيكون "ابناً" له
                    parentIdValue = treeAccounts.SelectedNode.Tag.ToString();
                    calculatedLevel = selectedNodeLevel + 1;
                }
            }

            // تعبئة حقول الأب والمستوى المحسوبة
            parent_ID.Text = parentIdValue;
            account_Level.Text = calculatedLevel.ToString();

            // 3. 🚀 [هندسة الترقيم التلقائي]: جلب رقم الحساب التالي بناءً على الأب لتسلسل الشجرة
            if (!string.IsNullOrEmpty(parentIdValue))
            {
                try
                {
                    // استعلام لجلب أكبر رقم حساب يبتدئ برقم حساب الأب (لضمان صحة تسلسل الفروع)
                    string query = "SELECT MAX(Acc_ID) FROM Accounts WHERE Parent_ID = @Parent_ID";

                    using (SqlCommand cmd = new SqlCommand(query, DatabaseHelper.GetConnection()))
                    {
                        cmd.Parameters.AddWithValue("@Parent_ID", parentIdValue);
                        object result = cmd.ExecuteScalar();

                        if (result != null && result != DBNull.Value)
                        {
                            // إذا وُجدت حسابات سابقة تحت هذا الأب، نأخذ أكبر رقم ونزيده 1
                            long maxId = Convert.ToInt64(result);
                            acc_ID.Text = (maxId + 1).ToString();
                        }
                        else
                        {
                            // إذا كان هذا هو "أول ابن" لهذا الأب، نضع رقم الأب متبوعاً بـ 01 أو 001 حسب تصميم الدليل لديك
                            // هنا افترضنا إضافة "01" كأول حساب فرعي، يمكنك تعديلها حسب طول خانات الدليل عندك
                            acc_ID.Text = parentIdValue + "01";
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("خطأ في توليد رقم الحساب التلقائي: " + ex.Message);
                }
                finally
                {
                    }
                // تصفير خيارات العملات عند الضغط على جديد لتهيئته للحساب القادم
                foreach (DataGridViewRow row in dgv_currencies.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        row.Cells[0].Value = false;
                        row.Cells[2].Value = false;
                        row.Cells["Is_Default"].Value = false;
                    }
                }
            }

            // 4. تصفير بقية الخيارات الافتراضية
            acc_Type.Text = "0";
            acc_Nature.Text = "0";
            report_Type.Text = "0";
            is_Stopped.Checked = false;

            // 5. فتح وضع الحقول للإدخال وتغيير مؤشر الإدخال
            EntrySource = "New";
            SetState(true);

            // 6. التحكم التلقائي بجدول العملات
            if (calculatedLevel == 4 || calculatedLevel == 5)
            {
                dgv_currencies.Enabled = true;

                // 🎯 التصحيح: تمرير نص فارغ "" بدلاً من ToString() لأن الحساب جديد كلياً
                تحميل_عملات_الحساب_بالجدول("", true);
            }
            else
            {
                dgv_currencies.Rows.Clear();
                dgv_currencies.Enabled = false;
            }

            // 7. توجيه المؤشر مباشرة إلى اسم الحساب لأن الرقم تولد تلقائياً وجاهز!
            acc_Name.Focus();
        }
        private void parent_ID_TextChanged(object sender, EventArgs e)
        {
            // إذا كانت الخانة فارغة، فهذا حساب مستوى أول (جذر)
            if (string.IsNullOrWhiteSpace(parent_ID.Text))
            {
                account_Level.Text = "1";
                dgv_currencies.Rows.Clear();
                dgv_currencies.Enabled = false; // الحسابات الرئيسية مقفلة العملات
                return;
            }

            try
            {
                // الاستعلام عن مستوى حساب الأب المدرج
                string query = "SELECT Account_Level FROM Accounts WHERE Acc_ID = @Parent_ID";

                using (SqlCommand cmd = new SqlCommand(query, DatabaseHelper.GetConnection()))
                {
                    cmd.Parameters.AddWithValue("@Parent_ID", parent_ID.Text.Trim());
                    object result = cmd.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        int parentLevel = Convert.ToInt32(result);
                        int currentLevel = parentLevel + 1; // الدستور: مستوى الابن = مستوى الأب + 1

                        account_Level.Text = currentLevel.ToString();

                        // التفاعل الذكي مع جدول العملات بناءً على المستوى الجديد المحسوب تلقائياً
                        if (currentLevel == 4 || currentLevel == 5)
                        {
                            dgv_currencies.Enabled = true;
                            // إذا كنا في وضع إضافة حساب جديد، نجهز العملات خام
                            if (EntrySource == "New" && dgv_currencies.Rows.Count == 0)
                            {
                                تحميل_عملات_الحساب_بالجدول(ToString(), true);
                            }
                        }
                        else
                        {
                            dgv_currencies.Rows.Clear();
                            dgv_currencies.Enabled = false; // تجميد الجدول لأن الحساب رئيسي
                        }
                    }
                    else
                    {
                        // إذا كتب المستخدم رقم أب غير موجود كودياً في القاعدة بعد
                        account_Level.Text = "1";
                        dgv_currencies.Rows.Clear();
                        dgv_currencies.Enabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                // معالجة صامتة أثناء الكتابة السريعة لعدم إزعاج المستخدم برسائل الخطأ
                System.Diagnostics.Debug.WriteLine("خطأ احتساب المستوى تلقائياً: " + ex.Message);
            }
            finally
            {
                }
        }
        private AlRowad_ERPDataSet AlRowad_ERPDataSet;
        private System.Windows.Forms.BindingSource accounts_ChartBindingSource;
        private AlRowad_ERPDataSetTableAdapters.TableAdapterManager tableAdapterManager1;
        private AlRowad_ERPDataSetTableAdapters.AccountsTableAdapter accountsTableAdapter1;
        private System.Windows.Forms.BindingSource accountsBindingSource1;
        // 🚀 تفعيل حدث زر التعديل (OnEdit) لفتح الحقول وجدول العملات
        // 🚀 تفعيل حدث زر التعديل (OnEdit) المتوافق مع شروط الشجرة والشريط
        public override void OnEdit()
        {
            if (string.IsNullOrWhiteSpace(acc_ID.Text))
            {
                MessageBox.Show("يرجى اختيار الحساب المراد تعديله من الشجرة أولاً بنقر مزدوج.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            EntrySource = "Edit";
            SetState(true);

            int level = 0;
            int.TryParse(account_Level.Text, out level);
            dgv_currencies.Enabled = (level == 4 || level == 5);

            // 💡 تشغيل التهيئة فور الانتقال لوضع التعديل لضمان قفل خيارات العملات غير المختارة وتلوينها بالرمادي
            if (dgv_currencies.Enabled)
            {
                تهيئة_حالة_خلايا_الجدول_الافتراضية();
            }

            acc_Name.Focus();
        }
        public override void OnAddFrom()
        {
            // 1. التأكد أولاً من وجود حساب محدد ليتم النسخ منه
            if (string.IsNullOrWhiteSpace(acc_ID.Text))
            {
                MessageBox.Show("يرجى اختيار الحساب المراد النسخ منه من الشجرة أولاً.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2. الاحتفاظ بالبيانات الحالية في متغيرات مؤقتة قبل تغيير حالة الشاشة أو تصفير الـ ID
            string currentParentId = parent_ID.Text.Trim();
            string currentLevel = account_Level.Text.Trim();
            string currentType = acc_Type.Text.Trim();
            string currentNature = acc_Nature.Text.Trim();
            string currentReport = report_Type.Text.Trim();

            // 3. تغيير حالة الشاشة برمجياً إلى وضع "الإضافة من" وفتح الحقول
            EntrySource = "AddFrom";
            SetState(true);

            // 4. توليد رقم الحساب الجديد تلقائياً بناءً على السجل الأخير في قاعدة البيانات
            string nextAccId = GetNextAccountID(currentParentId, currentLevel);

            if (!string.IsNullOrEmpty(nextAccId))
            {
                acc_ID.Text = nextAccId;
            }
            else
            {
                // حل احتياطي في حال فشل الجلب التلقائي (يترك الحقل فارغاً ليدخله المستخدم يدوياً)
                acc_ID.Clear();
                MessageBox.Show("لم نتمكن من توليد الرقم التالي تلقائياً، يرجى كتابة رقم الحساب الجديد يدوياً.", "تنويه", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            // 5. إعادة تثبيت البيانات المنسوخة في الخانات (لأن تصفير أو استدعاء SetState قد يؤثر عليها)
            parent_ID.Text = currentParentId;
            account_Level.Text = currentLevel;
            acc_Type.Text = currentType;
            acc_Nature.Text = currentNature;
            report_Type.Text = currentReport;

            // نقوم بإضافة كلمة (نسخة) أو (معدل) لاسم الحساب لكي يدرك المستخدم أنه يقوم بإضافة سجل جديد
            acc_Name.Text = acc_Name.Text + " - نسخة";

            // 6. توجيه المؤشر مباشرة لاسم الحساب ليقوم بكتابة الاسم الجديد فوراً
            acc_Name.Focus();
            acc_Name.SelectAll();
        }

        /// <summary>
        /// دالة ذكية تقوم بجلب آخر رقم حساب موجود تحت نفس الأب وتزيد عليه 1
        /// </summary>
        private string GetNextAccountID(string parentId, string levelStr)
        {
            string nextId = "";
            int level = 1;
            int.TryParse(levelStr, out level);

            // إذا كان الحساب في المستوى الأول (ليس له أب)
            string query = "";
            if (string.IsNullOrWhiteSpace(parentId) || level == 1)
            {
                query = "SELECT MAX(CAST(Acc_ID AS BIGINT)) FROM Accounts WHERE Parent_ID IS NULL OR Parent_ID = ''";
            }
            else
            {
                query = "SELECT MAX(CAST(Acc_ID AS BIGINT)) FROM Accounts WHERE Parent_ID = @Parent_ID";
            }

            try
            {
                using (SqlCommand cmd = new SqlCommand(query, DatabaseHelper.GetConnection()))
                {
                    if (!string.IsNullOrWhiteSpace(parentId) && level > 1)
                    {
                        cmd.Parameters.AddWithValue("@Parent_ID", parentId);
                    }

                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        long lastId = Convert.ToInt64(result);
                        nextId = (lastId + 1).ToString();
                    }
                    else
                    {
                        // إذا لم يكن هناك أي حساب فرعي تحت هذا الأب بعد، ننشئ الرقم الأول
                        // مثلاً لو الأب 11 والفرعي أول حساب يكون 1101 أو 11001 حسب طول الرتبة لديك
                        if (!string.IsNullOrWhiteSpace(parentId))
                        {
                            nextId = parentId + "01"; // تخصيص افتراضي ترقيمي ميكانيكي لـ ERP
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("خطأ أثناء توليد رقم الحساب التالي: " + ex.Message);
            }
            finally
            {
                }

            return nextId;
        }

    }
}
