using AlRowad_ERP.Core.Constants;
using AlRowad_ERP.Data;
using AlRowad_ERP.UI.Base;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AlRowad_ERP.Forms
{
    public partial class AgencySettingsForm : BaseEntryForm
    {
        private readonly AgencySettingsRepository _agencyRepo;
        private bool _hasFinancialMovements = false;

        public AgencySettingsForm()
        {
            InitializeComponent();
            PrimaryIdFieldName = "txt_Agency_ID";
            MainTableName = SystemConstants.Tables.Agency_Settings;
            _agencyRepo = new AgencySettingsRepository();

            // تم إزالة ربط KeyDown القديم، وسنعتمد على ProcessCmdKey المعمارية (في الأسفل)
        }

        private async void AgencySettingsForm_Load(object sender, EventArgs e)
        {
            try
            {
                if (dgv_Agencies != null)
                {
                    dgv_Agencies.AutoGenerateColumns = false;
                    dgv_Agencies.AllowUserToAddRows = false;
                    dgv_Agencies.AllowUserToDeleteRows = false;
                    dgv_Agencies.ReadOnly = true;
                    dgv_Agencies.RowHeadersVisible = false;
                    dgv_Agencies.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

                    dgv_Agencies.Columns.Clear();
                    dgv_Agencies.Columns.Add(new DataGridViewTextBoxColumn { Name = "Agency_ID", DataPropertyName = "Agency_ID", HeaderText = "الرقم", Width = 60 });
                    dgv_Agencies.Columns.Add(new DataGridViewTextBoxColumn { Name = "Agency_Name", DataPropertyName = "Agency_Name", HeaderText = "اسم الوكالة", Width = 150 });
                    dgv_Agencies.Columns.Add(new DataGridViewTextBoxColumn { Name = "Farmer_Commission_Percent", DataPropertyName = "Farmer_Commission_Percent", HeaderText = "عمولة المزارع %", Width = 110 });
                    dgv_Agencies.Columns.Add(new DataGridViewTextBoxColumn { Name = "Buyer_Fee_Per_Package", DataPropertyName = "Buyer_Fee_Per_Package", HeaderText = "رسوم المشتري", Width = 110 });
                    dgv_Agencies.Columns.Add(new DataGridViewTextBoxColumn { Name = "UpdatedInfo", DataPropertyName = "UpdatedInfo", HeaderText = "المعدل وتاريخ التعديل", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });

                    dgv_Agencies.CellDoubleClick += async (s, ev) => await Dgv_Agencies_CellDoubleClick(s, ev);
                }

                await LoadAllDataAsync();
                ChangeFormMode(FormMode.View);
            }
            catch (Exception ex) { LogError(ex); }
        }

        // دالة مساعدة لتنسيق الرقابة
        private string FormatUserInfo(object userIdObj, object userNameObj)
        {
            if (userIdObj == null || userIdObj == DBNull.Value || Convert.ToInt32(userIdObj) == 0) return "";
            string name = (userNameObj != null && userNameObj != DBNull.Value) ? userNameObj.ToString() : "مجهول";
            return $"{name} || {userIdObj}";
        }

        private async Task LoadAllDataAsync()
        {
            DataTable dt = await _agencyRepo.GetAllAgenciesAsync();
            if (dgv_Agencies != null) dgv_Agencies.DataSource = dt;
        }

        private async Task LoadSingleAgencyAsync(int agencyId)
        {
            DataTable dt = await _agencyRepo.GetAgencyByIdAsync(agencyId);
            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];

                // البيانات الأساسية 
                txt_Agency_ID.Text = row[SystemConstants.Columns.Agency_ID].ToString();
                txt_Agency_Name.Text = row[SystemConstants.Columns.Agency_Name].ToString();
                Eng_Agency_Name.Text = row[SystemConstants.Columns.Eng_Agency_Name] != DBNull.Value ? row[SystemConstants.Columns.Eng_Agency_Name].ToString() : "";

                txt_Phone1.Text = row[SystemConstants.Columns.Agency_Phone1].ToString();
                txt_Phone2.Text = row[SystemConstants.Columns.Agency_Phone2].ToString();

                txt_Address.Text = row[SystemConstants.Columns.Agency_Address].ToString();
                Eng_txt_Address.Text = row[SystemConstants.Columns.Eng_Agency_Address] != DBNull.Value ? row[SystemConstants.Columns.Eng_Agency_Address].ToString() : "";

                txt_Notes.Text = row[SystemConstants.Columns.Agency_Notes].ToString();
                Eng_txt_Notes.Text = row[SystemConstants.Columns.Eng_Agency_Notes] != DBNull.Value ? row[SystemConstants.Columns.Eng_Agency_Notes].ToString() : "";

                // السياسات
                num_FarmerComm.Text = Convert.ToDecimal(row[SystemConstants.Columns.Farmer_Commission_Percent]).ToString("F2");
                num_BuyerFee.Text = Convert.ToDecimal(row[SystemConstants.Columns.Buyer_Fee_Per_Package]).ToString("F2");
                num_OfficeFee.Text = Convert.ToDecimal(row[SystemConstants.Columns.Office_Service_Fee]).ToString("F2");
                chk_AllowOverride.Checked = row[SystemConstants.Columns.Allow_Override_In_Invoice] != DBNull.Value && Convert.ToBoolean(row[SystemConstants.Columns.Allow_Override_In_Invoice]);

                // الحسابات المربوطة
                txt_AccFarmerID.Text = row[SystemConstants.Columns.Acc_Farmer_Commission].ToString();
                txt_AccFarmerName.Text = row["FarmerAccName"].ToString();

                txt_AccBuyerID.Text = row[SystemConstants.Columns.Acc_Buyer_Fee].ToString();
                txt_AccBuyerName.Text = row["BuyerAccName"].ToString();

                txt_AccOfficeID.Text = row[SystemConstants.Columns.Acc_Additional_Discount].ToString();
                txt_AccOfficeName.Text = row["OfficeAccName"].ToString();

                // حقول الرقابة السفلية
                Control[] cBy = this.Controls.Find("txt_CreatedBy", true); if (cBy.Length > 0) cBy[0].Text = FormatUserInfo(row[SystemConstants.Columns.Created_By], row["CreatedByName"]);
                Control[] cAt = this.Controls.Find("txt_CreatedAt", true); if (cAt.Length > 0) cAt[0].Text = row[SystemConstants.Columns.Created_At] != DBNull.Value ? Convert.ToDateTime(row[SystemConstants.Columns.Created_At]).ToString("yyyy/MM/dd hh:mm tt") : "";
                Control[] uBy = this.Controls.Find("txt_UpdatedBy", true); if (uBy.Length > 0) uBy[0].Text = FormatUserInfo(row[SystemConstants.Columns.Updated_By], row["UpdatedByName"]);
                Control[] uAt = this.Controls.Find("txt_UpdatedAt", true); if (uAt.Length > 0) uAt[0].Text = row[SystemConstants.Columns.Updated_At] != DBNull.Value ? Convert.ToDateTime(row[SystemConstants.Columns.Updated_At]).ToString("yyyy/MM/dd hh:mm tt") : "";

                ChangeFormMode(FormMode.RecordSelected);
            }
        }

        private async Task Dgv_Agencies_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || (CurrentMode != FormMode.View && CurrentMode != FormMode.RecordSelected)) return;
            try
            {
                int agencyId = Convert.ToInt32(dgv_Agencies.Rows[e.RowIndex].Cells["Agency_ID"].Value);
                await LoadSingleAgencyAsync(agencyId);
            }
            catch (Exception ex) { LogError(ex); }
        }

        // 🌟 تحديث معمارية دالة الإضافة (أصبحت Asynchronous لجلب الرقم بدون تجميد)
        public override async void OnNew()
        {
            base.OnNew(); // يقوم الأب بفتح الحقول وتغيير الحالة

            // جلب الرقم التسلسلي القادم من قاعدة البيانات
            int nextId = await _agencyRepo.GetNextAgencyIdAsync();
            txt_Agency_ID.Text = nextId.ToString();

            // القيم الافتراضية
            num_FarmerComm.Text = "5.00";
            num_BuyerFee.Text = "0.00";
            num_OfficeFee.Text = "0.00";
            chk_AllowOverride.Checked = true;
            _hasFinancialMovements = false;

            // تصفير حقول الرقابة
            Control[] cBy = this.Controls.Find("txt_CreatedBy", true); if (cBy.Length > 0) cBy[0].Text = "";
            Control[] cAt = this.Controls.Find("txt_CreatedAt", true); if (cAt.Length > 0) cAt[0].Text = "";
            Control[] uBy = this.Controls.Find("txt_UpdatedBy", true); if (uBy.Length > 0) uBy[0].Text = "";
            Control[] uAt = this.Controls.Find("txt_UpdatedAt", true); if (uAt.Length > 0) uAt[0].Text = "";

            txt_Agency_Name.Focus();
        }
        protected override async Task<bool> ExecuteSaveToDatabaseAsync(SqlTransaction transaction)
        {
            if (string.IsNullOrWhiteSpace(txt_Agency_Name.Text))
            {
                MessageBox.Show("يجب إدخال اسم الوكالة (عربي).", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txt_AccFarmerID.Text) || string.IsNullOrWhiteSpace(txt_AccBuyerID.Text))
            {
                MessageBox.Show("يجب ربط الحسابات عبر الضغط على F9 في تبويب السياسات.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            int.TryParse(txt_Agency_ID.Text, out int agencyId);
            decimal.TryParse(num_FarmerComm.Text, out decimal farmerComm);
            decimal.TryParse(num_BuyerFee.Text, out decimal buyerFee);
            decimal.TryParse(num_OfficeFee.Text, out decimal officeFee);

            bool isNew = (CurrentMode == FormMode.New);

            int savedId = await _agencyRepo.SaveAgencySettingsAsync(
                agencyId, txt_Agency_Name.Text.Trim(), Eng_Agency_Name.Text.Trim(),
                txt_Phone1.Text, txt_Phone2.Text,
                txt_Address.Text.Trim(), Eng_txt_Address.Text.Trim(),
                txt_Notes.Text.Trim(), Eng_txt_Notes.Text.Trim(),
                farmerComm, buyerFee, officeFee, chk_AllowOverride.Checked,
                txt_AccFarmerID.Text, txt_AccBuyerID.Text, txt_AccOfficeID.Text,
                this.CurrentUserId, isNew, transaction);

            if (savedId > 0)
            {
                txt_Agency_ID.Text = savedId.ToString();
                _hasFinancialMovements = false;
                return true;
            }
            return false;
        }

        protected override void RefreshData()
        {
            _ = LoadAllDataAsync();
            if (int.TryParse(txt_Agency_ID.Text, out int currentId) && currentId > 0)
            {
                _ = LoadSingleAgencyAsync(currentId);
            }
        }
        #region 1. محرك البحث (F9) - المطيع للحالة (يعمل فقط إذا كان الحقل متاحاً)
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.F9)
            {
                // يعمل فقط في حالة الإضافة أو التعديل
                if (CurrentMode == FormMode.New || CurrentMode == FormMode.Edit)
                {
                    TextBox activeTxt = this.ActiveControl as TextBox;

                    // 🌟 التطبيق الدستوري: هل الحقل نصي؟ وهل هو مفتوح للتعديل (غير مجمد)؟
                    if (activeTxt != null && !activeTxt.ReadOnly)
                    {
                        // هل الحقل هو أحد حقول أرقام الحسابات؟
                        if (activeTxt == txt_AccFarmerID || activeTxt == txt_AccBuyerID || activeTxt == txt_AccOfficeID)
                        {
                            OpenAccountSearchForm(activeTxt);
                            return true; // ابتلاع الحدث لكي لا يمر للأب
                        }
                    }
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void OpenAccountSearchForm(TextBox txtId)
        {
            TextBox txtName = null;
            if (txtId.Name == "txt_AccFarmerID") txtName = txt_AccFarmerName;
            else if (txtId.Name == "txt_AccBuyerID") txtName = txt_AccBuyerName;
            else if (txtId.Name == "txt_AccOfficeID") txtName = txt_AccOfficeName;

            string query = $@"
                SELECT {SystemConstants.Columns.Acc_ID} AS [رقم الحساب], 
                       {SystemConstants.Columns.Acc_Name} AS [اسم الحساب] 
                FROM {SystemConstants.Tables.Accounts} 
                WHERE Is_Stopped = 0";

            using (var search = new AlRowad_ERP.HelpForms.UniversalSearchForm("دليل الحسابات (F9)", query))
            {
                if (search.ShowDialog(this) == DialogResult.OK)
                {
                    txtId.Text = search.المعرف_المختار;
                    if (txtName != null) txtName.Text = search.الاسم_المختار;
                }
            }
        }
        #endregion

        #region 2. العمليات السيادية (إدارة التجميد والفتح)

        // 🌟 القفل المركزي: يُبقي حقول الأسماء والترقيم مجمدة دائماً وفي كل الحالات
        protected override void LockControls(Control parent, bool isReadOnly)
        {
            // الأب يتولى فتح أو قفل الشاشة بناءً على (View, New, Edit)
            base.LockControls(parent, isReadOnly);

            // استثناءات دستورية (حقول للعرض فقط دائماً)
            if (txt_Agency_ID != null) txt_Agency_ID.ReadOnly = true;
            if (txt_AccFarmerName != null) txt_AccFarmerName.ReadOnly = true;
            if (txt_AccBuyerName != null) txt_AccBuyerName.ReadOnly = true;
            if (txt_AccOfficeName != null) txt_AccOfficeName.ReadOnly = true;
        }

        // 🌟 عملية التعديل: هنا يتم تطبيق المنطق التجاري الذي وصفته بالملي
        public override async void OnEdit()
        {
            if (string.IsNullOrWhiteSpace(txt_Agency_ID.Text) || txt_Agency_ID.Text == "تلقائي") return;

            int agencyId = int.Parse(txt_Agency_ID.Text);

            // 1. فحص الحركة المالية للوكالة من قاعدة البيانات (استخبارات مبكرة)
            _hasFinancialMovements = await _agencyRepo.CheckIfAgencyHasMovementsAsync(agencyId);

            // 2. إعطاء الأمر للأب بفتح الشاشة بالكامل للتعديل
            base.OnEdit();

            // 3. التطبيق الصارم: إذا كان هناك حركة مالية، جمد حقول أرقام الحسابات فقط!
            if (_hasFinancialMovements)
            {
                txt_AccFarmerID.ReadOnly = true;
                txt_AccBuyerID.ReadOnly = true;
                txt_AccOfficeID.ReadOnly = true;

                // رسالة توضيحية صامتة لمرة واحدة للمستخدم لييفهم سبب قفل الحسابات
                MessageBox.Show("تم تجميد حسابات الربط لاحتواء هذه الوكالة على حركات مالية سابقة.\n(بقية بيانات الوكالة والإعدادات متاحة للتعديل).", "النظام الرقابي", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            // إذا لم يكن هناك حركة مالية، ستظل الحقول مفتوحة بناءً على الأمر base.OnEdit() أعلاه

            txt_Agency_Name.Focus();
        }

        #endregion

        #region 2. الدور الثاني: البحث العام في الشاشة (العملية السيادية OnSearch)
        // يعمل في وضع الاستعراض (View) للبحث عن وكالة مسجلة مسبقاً
        public override void OnSearch()
        {
            // تطبيق صارم للدستور: منع البحث العام إذا كنا في منتصف عملية إدخال
            if (CurrentMode == FormMode.New || CurrentMode == FormMode.Edit)
            {
                MessageBox.Show("لا يمكن البحث عن وكالة أخرى أثناء عملية الإضافة أو التعديل.\nالرجاء الحفظ أو التراجع أولاً.", "تنبيه النظام", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // تطبيق الدستور: استخدام الثوابت (No Magic Strings) لإنشاء استعلام البحث
            string searchQuery = $@"
                SELECT {SystemConstants.Columns.Agency_ID} AS [رقم الوكالة], 
                       {SystemConstants.Columns.Agency_Name} AS [اسم الوكالة],
                       {SystemConstants.Columns.Eng_Agency_Name} AS [الاسم الأجنبي]
                FROM {SystemConstants.Tables.Agency_Settings}";

            // استدعاء محرك البحث الشامل
            using (var search = new AlRowad_ERP.HelpForms.UniversalSearchForm("البحث عن وكالة مسجلة", searchQuery))
            {
                if (search.ShowDialog(this) == DialogResult.OK)
                {
                    // عند اختيار وكالة، نضع رقمها في الحقل المخصص
                    txt_Agency_ID.Text = search.المعرف_المختار;

                    // استدعاء دالة جلب البيانات لعرضها على الشاشة
                    _ = LoadSingleAgencyAsync(Convert.ToInt32(search.المعرف_المختار));
                }
            }
        }
        #endregion

    }
}