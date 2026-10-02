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
    public partial class Items : BaseEntryForm
    {
        private readonly ItemRepository _itemRepo;

        public Items()
        {
            InitializeComponent();
            PrimaryIdFieldName = "item_IDTextBox";
            MainTableName = SystemConstants.Tables.Items;
            _itemRepo = new ItemRepository();
        }

        private async void Items_Load(object sender, EventArgs e)
        {
            try
            {
                // 1. تعبئة قائمة الوحدات (Dropdown Caching)
                await LoadUnitsDropdownAsync();

                // 2. إعداد شبكة العرض (الجريد)
                if (itemsDataGridView != null)
                {
                    itemsDataGridView.AutoGenerateColumns = false;
                    itemsDataGridView.AllowUserToAddRows = false;
                    itemsDataGridView.AllowUserToDeleteRows = false;
                    itemsDataGridView.ReadOnly = true;
                    itemsDataGridView.RowHeadersVisible = false;
                    itemsDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

                    itemsDataGridView.Columns.Clear();
                    itemsDataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "Item_ID", DataPropertyName = "Item_ID", HeaderText = "رقم الصنف", Width = 80 });
                    itemsDataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "Item_Name", DataPropertyName = "Item_Name", HeaderText = "اسم الصنف", Width = 150 });
                    itemsDataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "Base_Unit_Name", DataPropertyName = "Base_Unit_Name", HeaderText = "الوحدة الأساسية", Width = 100 });
                    itemsDataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "Default_Price", DataPropertyName = "Default_Price", HeaderText = "السعر الافتراضي", Width = 100 });

                    itemsDataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "CreatedInfo", DataPropertyName = "CreatedInfo", HeaderText = "المضيف وتاريخ الإضافة", Width = 230 });
                    itemsDataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "UpdatedInfo", DataPropertyName = "UpdatedInfo", HeaderText = "المعدل وتاريخ التعديل", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });

                    itemsDataGridView.CellDoubleClick += async (s, ev) => await ItemsDataGridView_CellDoubleClick(s, ev);
                }

                await LoadAllDataAsync();
                ChangeFormMode(FormMode.View);
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
        }

        #region دوال مساعدة وتحميل البيانات
        private string FormatUserInfo(object userIdObj, object userNameObj)
        {
            if (userIdObj == null || userIdObj == DBNull.Value || Convert.ToInt32(userIdObj) == 0) return "";
            string name = (userNameObj != null && userNameObj != DBNull.Value) ? userNameObj.ToString() : "مجهول";
            return $"{name} || {userIdObj}";
        }

        private async Task LoadUnitsDropdownAsync()
        {
            // تأكد أنك قمت بتغيير base_Unit_IDTextBox إلى cmb_BaseUnit في التصميم
            Control[] cmbControls = this.Controls.Find("cmb_BaseUnit", true);
            if (cmbControls.Length > 0 && cmbControls[0] is ComboBox cmb)
            {
                DataTable dtUnits = await _itemRepo.GetUnitsForDropdownAsync();
                cmb.DataSource = dtUnits;
                cmb.DisplayMember = "Unit_Name";
                cmb.ValueMember = "Unit_ID";
                cmb.SelectedIndex = -1;
            }
        }

        private async Task LoadAllDataAsync()
        {
            DataTable dtItems = await _itemRepo.GetAllActiveItemsAsync();
            itemsDataGridView.DataSource = dtItems;
        }

        private async Task LoadSingleItemDataAsync(int itemId)
        {
            DataTable dt = await _itemRepo.GetItemByIdAsync(itemId);
            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                item_IDTextBox.Text = row[SystemConstants.Columns.Item_ID].ToString();
                item_NameTextBox.Text = row[SystemConstants.Columns.Item_Name].ToString();
                default_PriceTextBox.Text = Convert.ToDecimal(row[SystemConstants.Columns.Default_Price]).ToString("F2");

                Control[] cmbControls = this.Controls.Find("cmb_BaseUnit", true);
                if (cmbControls.Length > 0 && cmbControls[0] is ComboBox cmb)
                {
                    cmb.SelectedValue = row[SystemConstants.Columns.Base_Unit_ID];
                }

                // حقول الرقابة (التي تعودنا عليها)
                Control[] cBy = this.Controls.Find("txt_CreatedBy", true); if (cBy.Length > 0) cBy[0].Text = FormatUserInfo(row[SystemConstants.Columns.Created_By], row["CreatedByName"]);
                Control[] cAt = this.Controls.Find("txt_CreatedAt", true); if (cAt.Length > 0) cAt[0].Text = row[SystemConstants.Columns.Created_At] != DBNull.Value ? Convert.ToDateTime(row[SystemConstants.Columns.Created_At]).ToString("yyyy/MM/dd hh:mm tt") : "";
                Control[] uBy = this.Controls.Find("txt_UpdatedBy", true); if (uBy.Length > 0) uBy[0].Text = FormatUserInfo(row[SystemConstants.Columns.Updated_By], row["UpdatedByName"]);
                Control[] uAt = this.Controls.Find("txt_UpdatedAt", true); if (uAt.Length > 0) uAt[0].Text = row[SystemConstants.Columns.Updated_At] != DBNull.Value ? Convert.ToDateTime(row[SystemConstants.Columns.Updated_At]).ToString("yyyy/MM/dd hh:mm tt") : "";

                ChangeFormMode(FormMode.RecordSelected);
            }
        }
        #endregion

        #region تفاعل المستخدم والبحث
        private async Task ItemsDataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || (CurrentMode != FormMode.View && CurrentMode != FormMode.RecordSelected)) return;

            try
            {
                int itemId = Convert.ToInt32(itemsDataGridView.Rows[e.RowIndex].Cells["Item_ID"].Value);
                await LoadSingleItemDataAsync(itemId);
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
                string query = $"SELECT {SystemConstants.Columns.Item_ID} AS [رقم الصنف], {SystemConstants.Columns.Item_Name} AS [اسم الصنف] FROM {SystemConstants.Tables.Items} WHERE {SystemConstants.Columns.Is_Deleted} = 0";

                using (var search = new AlRowad_ERP.HelpForms.UniversalSearchForm("بحث في دليل الأصناف (F9)", query))
                {
                    if (search.ShowDialog(this) == DialogResult.OK)
                    {
                        string selectedId = search.المعرف_المختار;
                        if (!string.IsNullOrWhiteSpace(selectedId))
                        {
                            await LoadSingleItemDataAsync(Convert.ToInt32(selectedId));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث خطأ أثناء فتح نافذة البحث: " + ex.Message, "نظام الرواد", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region العمليات السيادية (CRUD & Locks)
        public override void OnNew()
        {
            base.OnNew();
            item_IDTextBox.Text = "تلقائي";
            default_PriceTextBox.Text = "0.00";

            Control[] cmbControls = this.Controls.Find("cmb_BaseUnit", true);
            if (cmbControls.Length > 0 && cmbControls[0] is ComboBox cmb) cmb.SelectedIndex = -1;

            Control[] cBy = this.Controls.Find("txt_CreatedBy", true); if (cBy.Length > 0) cBy[0].Text = "";
            Control[] cAt = this.Controls.Find("txt_CreatedAt", true); if (cAt.Length > 0) cAt[0].Text = "";
            Control[] uBy = this.Controls.Find("txt_UpdatedBy", true); if (uBy.Length > 0) uBy[0].Text = "";
            Control[] uAt = this.Controls.Find("txt_UpdatedAt", true); if (uAt.Length > 0) uAt[0].Text = "";

            item_NameTextBox.Focus();
        }

        public override void OnEdit()
        {
            if (string.IsNullOrWhiteSpace(item_IDTextBox.Text) || item_IDTextBox.Text == "تلقائي")
            {
                MessageBox.Show("يرجى اختيار صنف للتعديل أولاً.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            base.OnEdit();
            item_NameTextBox.Focus();
        }

        protected override void LockControls(Control parent, bool isReadOnly)
        {
            base.LockControls(parent, isReadOnly);

            if (item_IDTextBox != null) item_IDTextBox.ReadOnly = true;
            if (itemsDataGridView != null) itemsDataGridView.Enabled = true;

            Control[] cBy = this.Controls.Find("txt_CreatedBy", true); if (cBy.Length > 0) ((TextBox)cBy[0]).ReadOnly = true;
            Control[] cAt = this.Controls.Find("txt_CreatedAt", true); if (cAt.Length > 0) ((TextBox)cAt[0]).ReadOnly = true;
            Control[] uBy = this.Controls.Find("txt_UpdatedBy", true); if (uBy.Length > 0) ((TextBox)uBy[0]).ReadOnly = true;
            Control[] uAt = this.Controls.Find("txt_UpdatedAt", true); if (uAt.Length > 0) ((TextBox)uAt[0]).ReadOnly = true;
        }

        protected override async Task<bool> ExecuteSaveToDatabaseAsync(SqlTransaction transaction)
        {
            if (string.IsNullOrWhiteSpace(item_NameTextBox.Text))
            {
                MessageBox.Show("يجب إدخال اسم الصنف.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            int baseUnitId = 0;
            Control[] cmbControls = this.Controls.Find("cmb_BaseUnit", true);
            if (cmbControls.Length > 0 && cmbControls[0] is ComboBox cmb && cmb.SelectedValue != null)
            {
                baseUnitId = Convert.ToInt32(cmb.SelectedValue);
            }

            if (baseUnitId == 0)
            {
                MessageBox.Show("يجب اختيار الوحدة الأساسية للصنف.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            int.TryParse(item_IDTextBox.Text, out int itemId);
            decimal.TryParse(default_PriceTextBox.Text, out decimal defaultPrice);

            bool isNew = (CurrentMode == FormMode.New);

            int savedId = await _itemRepo.SaveItemAsync(itemId, item_NameTextBox.Text.Trim(), baseUnitId, defaultPrice, this.CurrentUserId, isNew, transaction);

            if (savedId > 0)
            {
                item_IDTextBox.Text = savedId.ToString();
                return true;
            }
            return false;
        }

        protected override async Task<bool> ExecuteDeleteFromDatabaseAsync(SqlTransaction transaction)
        {
            if (string.IsNullOrWhiteSpace(item_IDTextBox.Text) || item_IDTextBox.Text == "تلقائي") return false;
            int itemId = int.Parse(item_IDTextBox.Text);

            return await _itemRepo.DeleteItemAsync(itemId, this.CurrentUserId, transaction);
        }

        protected override void RefreshData()
        {
            _ = LoadAllDataAsync();

            if (int.TryParse(item_IDTextBox.Text, out int currentId) && currentId > 0)
            {
                _ = LoadSingleItemDataAsync(currentId);
            }
        }
        #endregion
    }
}