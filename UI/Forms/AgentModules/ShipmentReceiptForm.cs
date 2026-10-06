using AlRowad_ERP.Core;
using AlRowad_ERP.Core.Constants;
using AlRowad_ERP.Core.Helpers;
using AlRowad_ERP.Data;
using AlRowad_ERP.UI.Base;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AlRowad_ERP.Forms
{
    public partial class ShipmentReceiptForm : BaseEntryForm
    {
        private readonly ShipmentReceiptRepository _shipmentRepo;
        private bool isHandlingGridCalculation = false;

        public ShipmentReceiptForm()
        {
            InitializeComponent();
            PrimaryIdFieldName = "txt_Shipment_ID";
            MainTableName = SystemConstants.Tables.Shipment_Receipt_Headers;
            _shipmentRepo = new ShipmentReceiptRepository();
        }

        private async void ShipmentReceiptForm_Load(object sender, EventArgs e)
        {
            try
            {
                dgv_Details.DataError += DataGridView_DataError;
                dgv_Details.CellDoubleClick += dgv_Details_CellDoubleClick;
                dgv_Details.RowsRemoved += (s, ev) => CalculateGridTotals();
                dgv_Details.EditingControlShowing += dgv_Details_EditingControlShowing;
                dgv_Details.CellValueChanged += dgv_Details_CellValueChanged;
                dgv_Details.CurrentCellDirtyStateChanged += dgv_Details_CurrentCellDirtyStateChanged;
                dgv_Details.RowPostPaint += dgv_Details_RowPostPaint;
                dgv_Details.KeyDown += dgv_Details_KeyDown;

                if (txt_Driver_Name != null)
                {
                    txt_Driver_Name.DoubleClick += txt_Driver_Name_DoubleClick;
                    txt_Driver_Name.ReadOnly = true;
                }

                dgv_Details.AutoGenerateColumns = false;
                await SetupUnitsComboBoxAsync(); // 👈 ربط الوحدات بالـ ComboBox للجريد

                ChangeFormMode(FormMode.View);
            }
            catch (Exception ex)
            {
                GlobalExceptionHandler.LogError(ex, "ShipmentReceiptForm_Load");
            }
        }

        private async Task SetupUnitsComboBoxAsync()
        {
            try
            {
                if (dgv_Details.Columns["Col_Unit_ID"] is DataGridViewComboBoxColumn comboCol)
                {
                    DataTable dtUnits = await _shipmentRepo.GetUnitsDataTableAsync();
                    comboCol.DataSource = dtUnits;
                    comboCol.DisplayMember = "Unit_Name";
                    comboCol.ValueMember = "Unit_ID";
                }
            }
            catch (Exception ex)
            {
                GlobalExceptionHandler.LogError(ex, "SetupUnitsComboBoxAsync");
            }
        }

        protected override void LockControls(Control parent, bool isReadOnly)
        {
            base.LockControls(parent, isReadOnly);

            if (dgv_Details != null)
            {
                dgv_Details.ReadOnly = isReadOnly;
                dgv_Details.AllowUserToAddRows = !isReadOnly;
                dgv_Details.AllowUserToDeleteRows = !isReadOnly;

                if (dgv_Details.Columns.Contains("Col_Serial")) dgv_Details.Columns["Col_Serial"].ReadOnly = true;
                if (dgv_Details.Columns.Contains("Col_Farmer_Name")) dgv_Details.Columns["Col_Farmer_Name"].ReadOnly = true;
                if (dgv_Details.Columns.Contains("Col_Item_Name")) dgv_Details.Columns["Col_Item_Name"].ReadOnly = true;
                if (dgv_Details.Columns.Contains("Col_Estimated_Total")) dgv_Details.Columns["Col_Estimated_Total"].ReadOnly = true;
            }

            if (txt_Shipment_ID != null) txt_Shipment_ID.ReadOnly = true;
            if (txt_Shipment_Code != null) txt_Shipment_Code.ReadOnly = true;
            if (txt_Total_Estimated != null) txt_Total_Estimated.ReadOnly = true;
        }

        public override async void OnNew()
        {
            base.OnNew();
            txt_Shipment_Code.Text = await _shipmentRepo.GenerateNextShipmentCodeAsync();
            txt_Shipment_ID.Text = "تلقائي";
            dtp_Receipt_Date.DateValue = DateTime.Now;

            dgv_Details.Rows.Clear();
            if (txt_Total_Estimated != null) txt_Total_Estimated.Text = "0.00";
            txt_Driver_Name.Focus();
        }

        protected override async Task<bool> ExecuteSaveToDatabaseAsync(SqlTransaction trans)
        {
            if (string.IsNullOrWhiteSpace(txt_Driver_Name.Text))
            {
                MessageBox.Show("يجب إدخال اسم السائق.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (dgv_Details.Rows.Count == 0 || (dgv_Details.Rows.Count == 1 && dgv_Details.Rows[0].IsNewRow))
            {
                MessageBox.Show("يجب إدخال بيانات المزارعين والأصناف.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            int.TryParse(txt_Shipment_ID.Text, out int shipmentId);
            decimal totalEst = 0;
            if (txt_Total_Estimated != null) decimal.TryParse(txt_Total_Estimated.Text, out totalEst);

            DataTable dtDetails = GetDetailsFromGrid();

            int savedId = await _shipmentRepo.SaveShipmentReceiptAsync(
                shipmentId, txt_Shipment_Code.Text, dtp_Receipt_Date.DateValue,
                txt_Driver_Name.Text.Trim(), txt_Vehicle_Number.Text.Trim(), txt_Driver_Phone.Text.Trim(), txt_Notes.Text?.Trim(),
                totalEst, dtDetails, this.CurrentUserId, CurrentMode == FormMode.New, trans);

            if (savedId > 0)
            {
                txt_Shipment_ID.Text = savedId.ToString();
                return true;
            }
            return false;
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
                dr[SystemConstants.Columns.Unit_ID] = row.Cells["Col_Unit_ID"].Value ?? 1;
                dr[SystemConstants.Columns.Quantity] = row.Cells["Col_Quantity"].Value ?? 0m;
                dr[SystemConstants.Columns.Estimated_Discount] = row.Cells["Col_Estimated_Discount"].Value ?? 0m;
                dr[SystemConstants.Columns.Estimated_Price] = row.Cells["Col_Estimated_Price"].Value ?? 0m;
                dr[SystemConstants.Columns.Notes] = row.Cells["Col_Notes"].Value?.ToString() ?? "";
                dt.Rows.Add(dr);
            }
            return dt;
        }

        // 👈 سياسة التنقل الاحترافية عبر Enter (تصرف كالـ Tab والانتقال المرن بين الأعمدة المرئية)
        private void dgv_Details_KeyDown(object sender, KeyEventArgs e)
        {
            if (CurrentMode == FormMode.View) return;

            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;

                if (dgv_Details.CurrentCell != null)
                {
                    int colIdx = dgv_Details.CurrentCell.ColumnIndex;
                    int rowIdx = dgv_Details.CurrentCell.RowIndex;
                    string colName = dgv_Details.Columns[colIdx].Name;

                    if (colName == "Col_Farmer_Name" || colName == "Col_Farmer_ID")
                    {
                        OpenFarmerSearch(rowIdx);
                        return;
                    }
                    else if (colName == "Col_Item_Name" || colName == "Col_Item_ID")
                    {
                        OpenItemSearch(rowIdx);
                        return;
                    }

                    // البحث عن العمود المرئي التالي أفقياً لتجاوز الحقول المخفية
                    int nextColIdx = colIdx + 1;
                    while (nextColIdx < dgv_Details.Columns.Count && !dgv_Details.Columns[nextColIdx].Visible)
                    {
                        nextColIdx++;
                    }

                    if (nextColIdx < dgv_Details.Columns.Count)
                    {
                        dgv_Details.CurrentCell = dgv_Details.Rows[rowIdx].Cells[nextColIdx];
                    }
                    else
                    {
                        if (rowIdx < dgv_Details.Rows.Count - 1)
                        {
                            dgv_Details.CurrentCell = dgv_Details.Rows[rowIdx + 1].Cells["Col_Farmer_Name"];
                        }
                        else
                        {
                            dgv_Details.AllowUserToAddRows = true;
                            dgv_Details.CurrentCell = dgv_Details.Rows[rowIdx + 1].Cells["Col_Farmer_Name"];
                        }
                    }
                }
            }
        }

        private void dgv_Details_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgv_Details.IsCurrentCellDirty)
            {
                dgv_Details.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void dgv_Details_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || CurrentMode == FormMode.View || isHandlingGridCalculation) return;

            string colName = dgv_Details.Columns[e.ColumnIndex].Name;

            // 👈 الحساب اللحظي الفوري للمعادلة: (الكمية - الخصم) * السعر
            if (colName == "Col_Quantity" || colName == "Col_Estimated_Price" || colName == "Col_Estimated_Discount")
            {
                var row = dgv_Details.Rows[e.RowIndex];

                decimal.TryParse(row.Cells["Col_Quantity"].Value?.ToString(), out decimal qty);
                decimal.TryParse(row.Cells["Col_Estimated_Discount"].Value?.ToString(), out decimal qtyDiscount);
                decimal.TryParse(row.Cells["Col_Estimated_Price"].Value?.ToString(), out decimal estPrice);

                isHandlingGridCalculation = true;

                decimal netQuantity = qty - qtyDiscount;
                if (netQuantity < 0) netQuantity = 0;

                decimal lineTotal = netQuantity * estPrice;
                row.Cells["Col_Estimated_Total"].Value = lineTotal.ToString("F2");

                isHandlingGridCalculation = false;

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

        private void dgv_Details_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            if (dgv_Details.Columns.Contains("Col_Serial"))
            {
                string serialNumber = (e.RowIndex + 1).ToString();
                var centerFormat = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                var headerBounds = new Rectangle(e.RowBounds.Left, e.RowBounds.Top, dgv_Details.Columns["Col_Serial"].Width, e.RowBounds.Height);
                e.Graphics.DrawString(serialNumber, dgv_Details.Font, SystemBrushes.ControlText, headerBounds, centerFormat);
            }
        }

        private void dgv_Details_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e) { CalculateGridTotals(); }

        internal override void OnF9Pressed()
        {
            if (CurrentMode == FormMode.View) return;

            if (txt_Driver_Name != null && txt_Driver_Name.Focused)
            {
                OpenDriverSearch();
                return;
            }

            if (dgv_Details.CurrentCell != null)
            {
                string colName = dgv_Details.Columns[dgv_Details.CurrentCell.ColumnIndex].Name;
                if (colName == "Col_Farmer_ID" || colName == "Col_Farmer_Name")
                    OpenFarmerSearch(dgv_Details.CurrentCell.RowIndex);
                else if (colName == "Col_Item_ID" || colName == "Col_Item_Name")
                    OpenItemSearch(dgv_Details.CurrentCell.RowIndex);
            }
        }

        private void OpenDriverSearch()
        {
            if (CurrentMode == FormMode.View) return;

            string query = "SELECT Driver_ID AS [رقم السائق], Driver_Name AS [اسم السائق] FROM Drivers WHERE Is_Deleted = 0";

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
            if (CurrentMode == FormMode.View || e.RowIndex < 0) return;
            string colName = dgv_Details.Columns[e.ColumnIndex].Name;
            if (colName == "Col_Farmer_ID" || colName == "Col_Farmer_Name") OpenFarmerSearch(e.RowIndex);
            else if (colName == "Col_Item_ID" || colName == "Col_Item_Name") OpenItemSearch(e.RowIndex);
        }

        private void OpenFarmerSearch(int rowIndex)
        {
            using (var search = new AlRowad_ERP.HelpForms.UniversalSearchForm("دليل المزارعين (F9)", "SELECT Supp_ID AS [رقم المزارع], Supp_Name AS [اسم المزارع] FROM Suppliers WHERE Is_Farmer = 1 AND Is_Deleted = 0"))
            {
                if (search.ShowDialog() == DialogResult.OK)
                {
                    dgv_Details.Rows[rowIndex].Cells["Col_Farmer_ID"].Value = search.المعرف_المختار;
                    dgv_Details.Rows[rowIndex].Cells["Col_Farmer_Name"].Value = search.الاسم_المختار;
                }
            }
        }

        private void OpenItemSearch(int rowIndex)
        {
            using (var search = new AlRowad_ERP.HelpForms.UniversalSearchForm("دليل الأصناف (F9)", "SELECT Item_ID AS [رقم الصنف], Item_Name AS [اسم الصنف] FROM Items WHERE Is_Deleted = 0"))
            {
                if (search.ShowDialog() == DialogResult.OK)
                {
                    dgv_Details.Rows[rowIndex].Cells["Col_Item_ID"].Value = search.المعرف_المختار;
                    dgv_Details.Rows[rowIndex].Cells["Col_Item_Name"].Value = search.الاسم_المختار;
                }
            }
        }

        private void dgv_Details_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (dgv_Details.CurrentCell == null) return;
            string colName = dgv_Details.Columns[dgv_Details.CurrentCell.ColumnIndex].Name;
            if (colName == "Col_Quantity" || colName == "Col_Estimated_Price" || colName == "Col_Estimated_Discount")
            {
                TextBox txt = e.Control as TextBox;
                if (txt != null) { txt.KeyPress -= NumericGridCell_KeyPress; txt.KeyPress += NumericGridCell_KeyPress; }
            }
        }

        private void NumericGridCell_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.') e.Handled = true;
            TextBox txt = sender as TextBox;
            if (txt != null && e.KeyChar == '.' && txt.Text.IndexOf('.') > -1) e.Handled = true;
        }

        private void DataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e) { e.ThrowException = false; e.Cancel = true; }
    }
}