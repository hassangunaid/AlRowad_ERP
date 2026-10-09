using AlRowad_ERP.Core.Constants;
using AlRowad_ERP.Core.Helpers;
using AlRowad_ERP.Data;
using AlRowad_ERP.UI.Base;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AlRowad_ERP.Forms
{
    // 1. الوراثة الصارمة من BaseMasterDetailForm لشاشات الرأس والتفاصيل
    public partial class ShipmentReceiptForm : BaseMasterDetailForm
    {
        private readonly ShipmentReceiptRepository _shipmentRepo;
        private bool isHandlingGridCalculation = false;

        public ShipmentReceiptForm()
        {
            InitializeComponent();
            PrimaryIdFieldName = "txt_Shipment_ID";
            MainTableName = SystemConstants.Tables.Shipment_Receipt_Headers;
            _shipmentRepo = new ShipmentReceiptRepository();

            // 2. ربط الجريد بالفئة الأب لتقوم بإدارته وقلفه وتنظيفه آلياً
            this.MainDetailsGrid = this.dgv_Details;
        }

        // تجاوز دالة التحميل الأساسية لضمان تنفيذها دائماً وعدم تأثرها بأخطاء الديزاينر
        protected override async void OnLoad(EventArgs e)
        {
            base.OnLoad(e); // استدعاء الأب لضمان اكتمال دورة حياة الشاشة وتطبيق الوراثة

            try
            {
                // 1. ربط الأحداث السيادية
                dgv_Details.OnF9Pressed -= Dgv_Details_OnF9Pressed;
                dgv_Details.OnF9Pressed += Dgv_Details_OnF9Pressed;

                dgv_Details.CellValueChanged -= dgv_Details_CellValueChanged;
                dgv_Details.CellValueChanged += dgv_Details_CellValueChanged;

                dgv_Details.RowsRemoved -= Dgv_Details_RowsRemoved;
                dgv_Details.RowsRemoved += Dgv_Details_RowsRemoved;

                if (txt_Driver_Name != null)
                {
                    txt_Driver_Name.DoubleClick -= txt_Driver_Name_DoubleClick;
                    txt_Driver_Name.DoubleClick += txt_Driver_Name_DoubleClick;
                    txt_Driver_Name.ReadOnly = true;
                }

                dgv_Details.AutoGenerateColumns = false;

                // ==========================================
                // الإصلاحات المعمارية بناءً على التوجيهات
                // ==========================================

                // فتح حقول البحث الذكي (المزارع والصنف) لتمكين الكتابة قبل F9
                if (dgv_Details.Columns.Contains("Col_Farmer_Name")) dgv_Details.Columns["Col_Farmer_Name"].ReadOnly = false;
                if (dgv_Details.Columns.Contains("Col_Item_Name")) dgv_Details.Columns["Col_Item_Name"].ReadOnly = false;

                // فرض التنسيق المحاسبي (N2) بقوة بعد رسم الديزاينر وإجبار نوع الخلية على الأرقام
                string[] numericCols = { "Col_Quantity", "Col_Estimated_Discount", "Col_Estimated_Price", "Col_Estimated_Total" };
                foreach (string col in numericCols)
                {
                    if (dgv_Details.Columns.Contains(col))
                    {
                        dgv_Details.Columns[col].DefaultCellStyle.Format = "N2";
                        dgv_Details.Columns[col].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dgv_Details.Columns[col].ValueType = typeof(decimal);
                    }
                }

                // تحميل القاموس الشامل للوحدات على مستوى العمود لمنع اختفاء الأسماء
                await SetupUnitsComboBoxAsync();

                // تطبيق إدارة الحالة
                ChangeFormMode(FormMode.View);
            }
            catch (Exception ex)
            {
                GlobalExceptionHandler.LogError(ex, "ShipmentReceiptForm_OnLoad");
            }
        }

        // الدالة المسماة الخاصة بحدث حذف السطر
        private void Dgv_Details_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            CalculateGridTotals();
        }

        // ==========================================
        // إدارة الحالة والحقول المقفلة
        // ==========================================
        protected override void LockControls(Control parent, bool isReadOnly)
        {
            base.LockControls(parent, isReadOnly);

            // استثناء حقول البحث الذكي طالما الشاشة قابلة للتعديل
            if (dgv_Details != null && !isReadOnly)
            {
                if (dgv_Details.Columns.Contains("Col_Farmer_Name")) dgv_Details.Columns["Col_Farmer_Name"].ReadOnly = false;
                if (dgv_Details.Columns.Contains("Col_Item_Name")) dgv_Details.Columns["Col_Item_Name"].ReadOnly = false;
            }
        }

        // ==========================================
        // العمليات السيادية (Sovereign Operations)
        // ==========================================

        public override async void OnNew()
        {
            base.OnNew(); // سيقوم الأب بتنظيف الشبكة (dgv_Details.Rows.Clear) آلياً

            txt_Shipment_Code.Text = await _shipmentRepo.GenerateNextShipmentCodeAsync();

            // تطبيق حماية تعدد المستخدمين (الترقيم اللحظي الوهمي)
            int expectedId = await _shipmentRepo.GetNextExpectedShipmentIdAsync();
            txt_Shipment_ID.Text = expectedId.ToString();

            dtp_Receipt_Date.DateValue = DateTime.Now;
            if (txt_Total_Estimated != null) txt_Total_Estimated.Text = "0.00";
            txt_Driver_Name.Focus();
        }

        // 5. سلامة البيانات المحاسبية (ACID Transactions)
        protected override async Task<bool> ExecuteSaveToDatabaseAsync(SqlTransaction trans)
        {
            if (!ValidateMasterData() || !ValidateDetailsData()) return false;

            int.TryParse(txt_Shipment_ID.Text, out int shipmentId);
            decimal.TryParse(txt_Total_Estimated?.Text, out decimal totalEst);

            DataTable dtDetails = GetDetailsFromGrid();

            // إرسال الرأس والتفاصيل ككتلة واحدة (TVP) للمستودع وتمرير المعاملة trans لضمان الـ ACID
            int savedId = await _shipmentRepo.SaveShipmentReceiptAsync(
                shipmentId, txt_Shipment_Code.Text, dtp_Receipt_Date.DateValue,
                txt_Driver_Name.Text.Trim(), txt_Vehicle_Number.Text.Trim(), txt_Driver_Phone.Text.Trim(), txt_Notes.Text?.Trim(),
                totalEst, dtDetails, this.CurrentUserId, CurrentMode == FormMode.New, trans);

            if (savedId > 0)
            {
                txt_Shipment_ID.Text = savedId.ToString(); // تحديث بالرقم الفعلي من السيرفر
                return true;
            }
            return false;
        }

        protected override bool ValidateMasterData()
        {
            if (string.IsNullOrWhiteSpace(txt_Driver_Name.Text))
            {
                MessageBox.Show("يجب إدخال اسم السائق.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private DataTable GetDetailsFromGrid()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(SystemConstants.Columns.Farmer_ID, typeof(string));
            dt.Columns.Add(SystemConstants.Columns.Item_ID, typeof(int));
            dt.Columns.Add(SystemConstants.Columns.Unit_ID, typeof(int));
            dt.Columns.Add(SystemConstants.Columns.Quantity, typeof(decimal));
            dt.Columns.Add(SystemConstants.Columns.Estimated_Discount, typeof(decimal));
            dt.Columns.Add(SystemConstants.Columns.Estimated_Price, typeof(decimal));
            dt.Columns.Add(SystemConstants.Columns.Notes, typeof(string));

            foreach (DataGridViewRow row in dgv_Details.Rows)
            {
                if (row.IsNewRow || row.Cells["Col_Farmer_ID"].Value == null || row.Cells["Col_Item_ID"].Value == null) continue;

                DataRow dr = dt.NewRow();
                dr[SystemConstants.Columns.Farmer_ID] = row.Cells["Col_Farmer_ID"].Value;
                dr[SystemConstants.Columns.Item_ID] = row.Cells["Col_Item_ID"].Value;
                dr[SystemConstants.Columns.Unit_ID] = row.Cells["Col_Unit_ID"].Value ?? DBNull.Value;
                dr[SystemConstants.Columns.Quantity] = row.Cells["Col_Quantity"].Value ?? 0m;
                dr[SystemConstants.Columns.Estimated_Discount] = row.Cells["Col_Estimated_Discount"].Value ?? 0m;
                dr[SystemConstants.Columns.Estimated_Price] = row.Cells["Col_Estimated_Price"].Value ?? 0m;
                dr[SystemConstants.Columns.Notes] = row.Cells["Col_Notes"].Value?.ToString() ?? "";
                dt.Rows.Add(dr);
            }
            return dt;
        }

        // ==========================================
        // حسابات الشبكة (فقط المنطق المحاسبي بقي هنا)
        // ==========================================

        private void dgv_Details_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || CurrentMode == FormMode.View || isHandlingGridCalculation) return;

            string colName = dgv_Details.Columns[e.ColumnIndex].Name;

            if (colName == "Col_Quantity" || colName == "Col_Estimated_Price" || colName == "Col_Estimated_Discount")
            {
                var row = dgv_Details.Rows[e.RowIndex];

                decimal.TryParse(row.Cells["Col_Quantity"].Value?.ToString(), out decimal qty);
                decimal.TryParse(row.Cells["Col_Estimated_Discount"].Value?.ToString(), out decimal qtyDiscount);
                decimal.TryParse(row.Cells["Col_Estimated_Price"].Value?.ToString(), out decimal estPrice);

                try
                {
                    isHandlingGridCalculation = true;
                    decimal netQuantity = Math.Max(0, qty - qtyDiscount);
                    decimal lineTotal = netQuantity * estPrice;

                    // تم التعديل: تمرير الرقم كـ decimal ليتم تطبيق التنسيق N2 عليه
                    row.Cells["Col_Estimated_Total"].Value = lineTotal;
                }
                finally
                {
                    isHandlingGridCalculation = false;
                }

                CalculateGridTotals();
            }
        }

        private void CalculateGridTotals()
        {
            if (isHandlingGridCalculation) return;

            decimal total = 0;
            foreach (DataGridViewRow row in dgv_Details.Rows)
            {
                if (row.IsNewRow) continue;
                decimal.TryParse(row.Cells["Col_Estimated_Total"].Value?.ToString(), out decimal lineTotal);
                total += lineTotal;
            }
            if (txt_Total_Estimated != null) txt_Total_Estimated.Text = total.ToString("N2");
        }

        // ==========================================
        // أدلة البحث (Search Forms & F9) والبحث الذكي
        // ==========================================

        // التقاط F9 من الشاشة (للرأس)
        internal override void OnF9Pressed()
        {
            if (CurrentMode == FormMode.View) return;

            if (txt_Driver_Name != null && txt_Driver_Name.Focused)
            {
                OpenDriverSearch();
            }
        }

        // التقاط F9 الموجه من أداة الجريد المخصصة الخاصة بنا
        private void Dgv_Details_OnF9Pressed(object sender, DataGridViewCellEventArgs e)
        {
            if (CurrentMode == FormMode.View || e.RowIndex < 0) return;
            string colName = dgv_Details.Columns[e.ColumnIndex].Name;

            // استخراج النص الذي كتبه المستخدم كجزء من الاسم للبحث الذكي
            string searchText = "";
            if (dgv_Details.IsCurrentCellInEditMode && dgv_Details.EditingControl != null)
            {
                searchText = dgv_Details.EditingControl.Text.Trim();
                dgv_Details.EndEdit(); // إنهاء وضع التعديل برمجياً لفتح الشاشة بنجاح
            }
            else if (dgv_Details.CurrentCell.Value != null)
            {
                searchText = dgv_Details.CurrentCell.Value.ToString().Trim();
            }

            if (colName == "Col_Farmer_ID" || colName == "Col_Farmer_Name")
                OpenFarmerSearch(e.RowIndex, searchText); // تمرير النص للفلترة
            else if (colName == "Col_Item_ID" || colName == "Col_Item_Name")
                _ = OpenItemSearchAsync(e.RowIndex, searchText); // تمرير النص للفلترة
        }

        private void OpenDriverSearch()
        {
            string query = _shipmentRepo.GetDriversSearchQuery();
            using (var search = new AlRowad_ERP.HelpForms.UniversalSearchForm("دليل السائقين (F9)", query))
            {
                if (search.ShowDialog() == DialogResult.OK)
                {
                    txt_Driver_Name.Text = search.الاسم_المختار;
                    if (txt_Driver_Phone != null)
                    {
                        txt_Driver_Phone.Clear();
                        txt_Driver_Phone.Focus();
                    }
                }
            }
        }

        private void txt_Driver_Name_DoubleClick(object sender, EventArgs e)
        {
            if (CurrentMode != FormMode.View) OpenDriverSearch();
        }

        private void dgv_Details_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // محاكاة لضغط F9 عند النقر المزدوج على الخلايا
            Dgv_Details_OnF9Pressed(sender, e);
        }

        private void OpenFarmerSearch(int rowIndex, string searchText)
        {
            string query = _shipmentRepo.GetFarmersSearchQuery(searchText);
            using (var search = new AlRowad_ERP.HelpForms.UniversalSearchForm("دليل المزارعين (F9)", query))
            {
                if (search.ShowDialog() == DialogResult.OK)
                {
                    dgv_Details.Rows[rowIndex].Cells["Col_Farmer_ID"].Value = search.المعرف_المختار;
                    dgv_Details.Rows[rowIndex].Cells["Col_Farmer_Name"].Value = search.الاسم_المختار;
                }
            }
        }

        private async Task OpenItemSearchAsync(int rowIndex, string searchText)
        {
            string query = _shipmentRepo.GetItemsSearchQuery(searchText);
            using (var search = new AlRowad_ERP.HelpForms.UniversalSearchForm("دليل الأصناف (F9)", query))
            {
                if (search.ShowDialog() == DialogResult.OK)
                {
                    dgv_Details.Rows[rowIndex].Cells["Col_Item_ID"].Value = search.المعرف_المختار;
                    dgv_Details.Rows[rowIndex].Cells["Col_Item_Name"].Value = search.الاسم_المختار;

                    if (int.TryParse(search.المعرف_المختار.ToString(), out int itemId))
                    {
                        await LoadItemUnitsIntoCellAsync(rowIndex, itemId);
                    }
                }
            }
        }

        // ==========================================
        // تخصيص بيانات الوحدات لكل خلية (Cell-Level Binding)
        // ==========================================
        private async Task LoadItemUnitsIntoCellAsync(int rowIndex, int itemId)
        {
            try
            {
                if (dgv_Details.Rows[rowIndex].Cells["Col_Unit_ID"] is DataGridViewComboBoxCell comboCell)
                {
                    DataTable dtItemUnits = await _shipmentRepo.GetItemUnitsDataTableAsync(itemId);

                    // السر المعماري: فك ارتباط الخلية بالعمود العام وتفريغها أولاً لضمان عدم ظهور כל الوحدات
                    comboCell.DataSource = null;
                    comboCell.Items.Clear();

                    // ربط الخلية بوحدات هذا الصنف حصراً
                    comboCell.DataSource = dtItemUnits;
                    comboCell.DisplayMember = SystemConstants.Columns.Unit_Name;
                    comboCell.ValueMember = SystemConstants.Columns.Unit_ID;

                    // اختيار الوحدة الافتراضية الأولى
                    if (dtItemUnits.Rows.Count > 0)
                    {
                        comboCell.Value = dtItemUnits.Rows[0][SystemConstants.Columns.Unit_ID];
                    }
                }
            }
            catch (Exception ex)
            {
                GlobalExceptionHandler.LogError(ex, "LoadItemUnitsIntoCellAsync");
            }
        }

        private async Task SetupUnitsComboBoxAsync()
        {
            try
            {
                if (dgv_Details.Columns["Col_Unit_ID"] is DataGridViewComboBoxColumn comboCol)
                {
                    // تحميل القاموس الشامل على مستوى العمود لكي يحتفظ الجريد بالأسماء بعد الخروج من الخلية
                    DataTable dtUnits = await _shipmentRepo.GetUnitsDataTableAsync();
                    comboCol.DataSource = dtUnits;
                    comboCol.DisplayMember = SystemConstants.Columns.Unit_Name;
                    comboCol.ValueMember = SystemConstants.Columns.Unit_ID;
                }
            }
            catch (Exception ex)
            {
                GlobalExceptionHandler.LogError(ex, "SetupUnitsComboBoxAsync");
            }
        }
        // ==========================================
        // تفعيل زر البحث السيادي في شريط الأدوات
        // ==========================================
        public override void OnSearch()
        {
            base.OnSearch();

            // استدعاء نافذة البحث العامة الخاصة بسجلات حمولات الصادر/الوارِد
            string query = "SELECT Shipment_ID AS [رقم الحمولة], Shipment_Code AS [رقم المستند], Shipment_Date AS [التاريخ], Driver_Name AS [اسم السائق] FROM Shipment_Receipt_Headers WHERE Is_Deleted = 0";

            using (var search = new AlRowad_ERP.HelpForms.UniversalSearchForm("بحث في بيانات الحمولات (F9)", query))
            {
                if (search.ShowDialog() == DialogResult.OK)
                {
                    string selectedId = search.المعرف_المختار;
                    if (!string.IsNullOrEmpty(selectedId))
                    {
                        // استدعاء دالة جلب السجل وتعبئته في الشاشة (RecordSelected Mode)
                        _ = LoadShipmentDataAsync(Convert.ToInt32(selectedId));
                    }
                }
            }
        }

        // دالة مساعدة لجلب بيانات الحمولة المحددة وعرضها في الشاشة
        private async Task LoadShipmentDataAsync(int shipmentId)
        {
            try
            {
                // جلب بيانات الرأس وتعبئتها عبر محرك الربط وتغيير وضع الشاشة إلى View
                DataTable dtHeader = await _shipmentRepo.GetShipmentHeaderAsync(shipmentId);
                if (dtHeader != null && dtHeader.Rows.Count > 0)
                {
                    DataRow row = dtHeader.Rows[0];
                    txt_Shipment_ID.Text = row["Shipment_ID"].ToString();
                    txt_Shipment_Code.Text = row["Shipment_Code"].ToString();
                    dtp_Receipt_Date.DateValue = Convert.ToDateTime(row["Shipment_Date"]);
                    txt_Driver_Name.Text = row["Driver_Name"].ToString();
                    txt_Vehicle_Number.Text = row["Vehicle_Number"]?.ToString() ?? "";
                    txt_Driver_Phone.Text = row["Driver_Phone"]?.ToString() ?? "";
                    txt_Notes.Text = row["Notes"]?.ToString() ?? "";
                    txt_Total_Estimated.Text = Convert.ToDecimal(row["Total_Estimated"]).ToString("N2");

                    // جلب تفاصيل الحمولة وتعبئتها في الشبكة
                    DataTable dtDetails = await _shipmentRepo.GetShipmentDetailsAsync(shipmentId);
                    dgv_Details.Rows.Clear();
                    foreach (DataRow dr in dtDetails.Rows)
                    {
                        int rowIndex = dgv_Details.Rows.Add();
                        var gridRow = dgv_Details.Rows[rowIndex];
                        gridRow.Cells["Col_Farmer_ID"].Value = dr["Supp_ID"];
                        gridRow.Cells["Col_Farmer_Name"].Value = dr["Supp_Name"];
                        gridRow.Cells["Col_Item_ID"].Value = dr["Item_ID"];
                        gridRow.Cells["Col_Item_Name"].Value = dr["Item_Name"];
                        gridRow.Cells["Col_Unit_ID"].Value = dr["Unit_ID"];
                        gridRow.Cells["Col_Quantity"].Value = Convert.ToDecimal(dr["Quantity"]);
                        gridRow.Cells["Col_Estimated_Discount"].Value = Convert.ToDecimal(dr["Estimated_Discount"]);
                        gridRow.Cells["Col_Estimated_Price"].Value = Convert.ToDecimal(dr["Estimated_Price"]);
                        gridRow.Cells["Col_Estimated_Total"].Value = Convert.ToDecimal(dr["Estimated_Total"]);
                        gridRow.Cells["Col_Notes"].Value = dr["Notes"]?.ToString() ?? "";
                    }

                    ChangeFormMode(FormMode.View);
                }
            }
            catch (Exception ex)
            {
                GlobalExceptionHandler.LogError(ex, "LoadShipmentDataAsync");
            }
        }
    }
}