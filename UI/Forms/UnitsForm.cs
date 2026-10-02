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
    public partial class UnitsForm : BaseEntryForm
    {
        private readonly UnitRepository _unitRepo;

        public UnitsForm()
        {
            InitializeComponent();
            PrimaryIdFieldName = "unit_IDTextBox";
            MainTableName = SystemConstants.Tables.Units;
            _unitRepo = new UnitRepository();
        }

        private async void UnitsForm_Load(object sender, EventArgs e)
        {
            try
            {
                if (unitsDataGridView != null)
                {
                    unitsDataGridView.AutoGenerateColumns = false;
                    unitsDataGridView.AllowUserToAddRows = false;
                    unitsDataGridView.AllowUserToDeleteRows = false;
                    unitsDataGridView.ReadOnly = true;
                    unitsDataGridView.RowHeadersVisible = false;
                    unitsDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

                    unitsDataGridView.Columns.Clear();
                    unitsDataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "Unit_ID", DataPropertyName = "Unit_ID", HeaderText = "رقم الوحدة", Width = 80 });
                    unitsDataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "Unit_Name", DataPropertyName = "Unit_Name", HeaderText = "اسم الوحدة", Width = 150 });
                    unitsDataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "Conversion_Factor", DataPropertyName = "Conversion_Factor", HeaderText = "معامل التحويل", Width = 100 });

                    // 🌟 استدعاء الحقول المدمجة الجديدة من الـ Repository
                    unitsDataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "CreatedInfo", DataPropertyName = "CreatedInfo", HeaderText = "المضيف وتاريخ الإضافة", Width = 230 });

                    // 🌟 جعل الحقل الأخير يمتد للآخر
                    unitsDataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "UpdatedInfo", DataPropertyName = "UpdatedInfo", HeaderText = "المعدل وتاريخ التعديل", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });

                    unitsDataGridView.CellDoubleClick += async (s, ev) => await UnitsDataGridView_CellDoubleClick(s, ev);
                }

                await LoadAllDataAsync();
                ChangeFormMode(FormMode.View);
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
        }
        #region دوال التدقيق (Audit Trail)
        // دالة مساعدة لتنسيق اسم المستخدم ورقمه
        private string FormatUserInfo(object userIdObj, object userNameObj)
        {
            if (userIdObj == null || userIdObj == DBNull.Value || Convert.ToInt32(userIdObj) == 0) return "";
            string name = (userNameObj != null && userNameObj != DBNull.Value) ? userNameObj.ToString() : "مجهول";
            return $"{name} || {userIdObj}";
        }
        #endregion

        private async Task LoadAllDataAsync()
        {
            DataTable dtUnits = await _unitRepo.GetAllActiveUnitsAsync();
            unitsDataGridView.DataSource = dtUnits;
        }

        // 🌟 دالة جلب السجل الواحد مع حقول الرقابة والمضيف/المعدل
        private async Task LoadSingleUnitDataAsync(int unitId)
        {
            DataTable dt = await _unitRepo.GetUnitByIdAsync(unitId);
            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                unit_IDTextBox.Text = row[SystemConstants.Columns.Unit_ID].ToString();
                unit_NameTextBox.Text = row[SystemConstants.Columns.Unit_Name].ToString();
                conversion_FactorTextBox.Text = Convert.ToDecimal(row[SystemConstants.Columns.Conversion_Factor]).ToString("F4");

                // تعبئة حقول الرقابة اللحظية (تأكد من وجود هذه الأدوات txt_CreatedBy وما شابهها في التصميم)
                Control[] cBy = this.Controls.Find("txt_CreatedBy", true);
                if (cBy.Length > 0) cBy[0].Text = FormatUserInfo(row[SystemConstants.Columns.Created_By], row["CreatedByName"]);

                Control[] cAt = this.Controls.Find("txt_CreatedAt", true);
                if (cAt.Length > 0) cAt[0].Text = row[SystemConstants.Columns.Created_At] != DBNull.Value ? Convert.ToDateTime(row[SystemConstants.Columns.Created_At]).ToString("yyyy/MM/dd hh:mm tt") : "";

                Control[] uBy = this.Controls.Find("txt_UpdatedBy", true);
                if (uBy.Length > 0) uBy[0].Text = FormatUserInfo(row[SystemConstants.Columns.Updated_By], row["UpdatedByName"]);

                Control[] uAt = this.Controls.Find("txt_UpdatedAt", true);
                if (uAt.Length > 0) uAt[0].Text = row[SystemConstants.Columns.Updated_At] != DBNull.Value ? Convert.ToDateTime(row[SystemConstants.Columns.Updated_At]).ToString("yyyy/MM/dd hh:mm tt") : "";

                ChangeFormMode(FormMode.RecordSelected);
            }
        }

        // 🌟 التعديل الجذري لحدث النقر لمنع توقفه بعد المرة الأولى
        private async Task UnitsDataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // السماح بالنقر إذا كانت الشاشة في وضع "الاستعراض" أو "تم اختيار سجل"
            if (e.RowIndex < 0 || (CurrentMode != FormMode.View && CurrentMode != FormMode.RecordSelected)) return;

            try
            {
                int unitId = Convert.ToInt32(unitsDataGridView.Rows[e.RowIndex].Cells["Unit_ID"].Value);
                await LoadSingleUnitDataAsync(unitId);
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
        }
        public override async void OnSearch()
        {
            try
            {
                string query = $"SELECT {SystemConstants.Columns.Unit_ID} AS [رقم الوحدة], {SystemConstants.Columns.Unit_Name} AS [اسم الوحدة] FROM {SystemConstants.Tables.Units} WHERE {SystemConstants.Columns.Is_Deleted} = 0";

                using (var search = new AlRowad_ERP.HelpForms.UniversalSearchForm("بحث في دليل الوحدات (F9)", query))
                {
                    if (search.ShowDialog(this) == DialogResult.OK)
                    {
                        string selectedId = search.المعرف_المختار;
                        if (!string.IsNullOrWhiteSpace(selectedId))
                        {
                            await LoadSingleUnitDataAsync(Convert.ToInt32(selectedId));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث خطأ أثناء فتح نافذة البحث: " + ex.Message, "نظام الرواد", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public override void OnNew()
        {
            base.OnNew();
            unit_IDTextBox.Text = "تلقائي";
            conversion_FactorTextBox.Text = "1.0000";

            // تصفير حقول الرقابة
            Control[] cBy = this.Controls.Find("txt_CreatedBy", true); if (cBy.Length > 0) cBy[0].Text = "";
            Control[] cAt = this.Controls.Find("txt_CreatedAt", true); if (cAt.Length > 0) cAt[0].Text = "";
            Control[] uBy = this.Controls.Find("txt_UpdatedBy", true); if (uBy.Length > 0) uBy[0].Text = "";
            Control[] uAt = this.Controls.Find("txt_UpdatedAt", true); if (uAt.Length > 0) uAt[0].Text = "";

            unit_NameTextBox.Focus();
        }

        public override void OnEdit()
        {
            if (string.IsNullOrWhiteSpace(unit_IDTextBox.Text) || unit_IDTextBox.Text == "تلقائي")
            {
                MessageBox.Show("يرجى اختيار وحدة للتعديل أولاً.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            base.OnEdit();
            unit_NameTextBox.Focus();
        }

        protected override void LockControls(Control parent, bool isReadOnly)
        {
            base.LockControls(parent, isReadOnly);

            if (unit_IDTextBox != null) unit_IDTextBox.ReadOnly = true;
            if (unitsDataGridView != null) unitsDataGridView.Enabled = true;

            // حماية حقول الرقابة (إذا كانت موجودة)
            Control[] cBy = this.Controls.Find("txt_CreatedBy", true); if (cBy.Length > 0) ((TextBox)cBy[0]).ReadOnly = true;
            Control[] cAt = this.Controls.Find("txt_CreatedAt", true); if (cAt.Length > 0) ((TextBox)cAt[0]).ReadOnly = true;
            Control[] uBy = this.Controls.Find("txt_UpdatedBy", true); if (uBy.Length > 0) ((TextBox)uBy[0]).ReadOnly = true;
            Control[] uAt = this.Controls.Find("txt_UpdatedAt", true); if (uAt.Length > 0) ((TextBox)uAt[0]).ReadOnly = true;
        }

        protected override async Task<bool> ExecuteSaveToDatabaseAsync(SqlTransaction transaction)
        {
            if (string.IsNullOrWhiteSpace(unit_NameTextBox.Text))
            {
                MessageBox.Show("يجب إدخال اسم الوحدة.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            int.TryParse(unit_IDTextBox.Text, out int unitId);
            decimal.TryParse(conversion_FactorTextBox.Text, out decimal conversionFactor);
            if (conversionFactor <= 0) conversionFactor = 1;

            bool isNew = (CurrentMode == FormMode.New);

            int savedId = await _unitRepo.SaveUnitAsync(unitId, unit_NameTextBox.Text.Trim(), conversionFactor, this.CurrentUserId, isNew, transaction);

            if (savedId > 0)
            {
                unit_IDTextBox.Text = savedId.ToString();
                return true;
            }
            return false;
        }

        protected override async Task<bool> ExecuteDeleteFromDatabaseAsync(SqlTransaction transaction)
        {
            if (string.IsNullOrWhiteSpace(unit_IDTextBox.Text) || unit_IDTextBox.Text == "تلقائي") return false;
            int unitId = int.Parse(unit_IDTextBox.Text);

            return await _unitRepo.DeleteUnitAsync(unitId, this.CurrentUserId, transaction);
        }

        protected override void RefreshData()
        {
            _ = LoadAllDataAsync();

            if (int.TryParse(unit_IDTextBox.Text, out int currentId) && currentId > 0)
            {
                _ = LoadSingleUnitDataAsync(currentId);
            }
        }
    }
}