using AlRowad_ERP.Core;
using AlRowad_ERP.Core.Constants;
using AlRowad_ERP.Data.Reports;
using AlRowad_ERP.HelpForms;
using AlRowad_ERP.Models.Reports;
using AlRowad_ERP.Reports.Form;
using AlRowad_ERP.UI.Base;
using AlRowad_ERP.UI.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AlRowad_ERP.UI.Reports
{
    /// <summary>
    /// شاشة تقرير كشف الحساب (متعدد العملات والتبويبات) - متوافقة مع دستور الرواد ERP المعماري
    /// </summary>
    public partial class Frm_AccountStatement : BaseEntryForm
    {
        private readonly ReportAccountRepository _reportRepo;
        private TabControl _tabCurrencies;
        private List<AccountStatementDto> _currentStatementData;

        public Frm_AccountStatement()
        {
            InitializeComponent();
            _reportRepo = new ReportAccountRepository();

            if (dgvStatement != null)
            {
                dgvStatement.AllowUserToOrderColumns = true;
                dgvStatement.ReadOnly = true;
                dgvStatement.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                SetupDataGridViewFormatting(dgvStatement);
            }

            if (btnPrintGrid != null)
            {
                btnPrintGrid.Click -= btnPrintGrid_Click;
                btnPrintGrid.Click += btnPrintGrid_Click;
            }
            if (btnPrintOfficial != null)
            {
                btnPrintOfficial.Click -= btnPrintOfficial_Click;
                btnPrintOfficial.Click += btnPrintOfficial_Click;
            }
        }
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            // 🌟 نقل التركيز الفوري لحقل رقم الحساب وتجاوز TabIndex = 0 الخاص بزر التحديث
            this.BeginInvoke(new Action(() =>
            {
                if (txtAccountId != null && txtAccountId.CanFocus)
                {
                    this.ActiveControl = txtAccountId;
                    txtAccountId.Focus();
                    txtAccountId.SelectAll();
                }
            }));
        }

        // =========================================================
        // 1. الالتزام بالمادة (1 و 2 و 3 و 5): تهيئة الشاشة وجلب الفترة والعملات عبر المستودع
        // =========================================================
        protected override async void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // فك القفل عن الشاشة عبر الدالة المركزية لأنها شاشة استعلام (تقرير)
            LockControls(this, false);

            if (chkIncludeUnposted != null) chkIncludeUnposted.Checked = true;
            if (dtpTo != null) dtpTo.DateValue = DateTime.Now.Date;

            if (txtAccountName != null) txtAccountName.ReadOnly = true;
            if (dgvStatement != null) dgvStatement.ReadOnly = true;

            if (txtAccountId != null)
            {
                txtAccountId.KeyDown -= txtAccountId_KeyDown;
                txtAccountId.KeyDown += txtAccountId_KeyDown;
                txtAccountId.Focus();
            }

            if (btnLoadData != null)
            {
                btnLoadData.Click -= btnLoadData_Click;
                btnLoadData.Click += btnLoadData_Click;
            }

            // جلب تاريخ بداية الفترة المالية وقائمة العملات بشكل غير متزامن
            await InitializeLookupsAsync();
        }

        private async Task InitializeLookupsAsync()
        {
            if (dtpFrom != null)
            {
                try
                {
                    DateTime startDate = await Task.Run(() => _reportRepo.GetCurrentPeriodStartDate());
                    dtpFrom.DateValue = startDate;
                }
                catch (Exception ex)
                {
                    DatabaseHelper.LogSystemError(ex.Message, ex.StackTrace, "Load_Financial_Period");
                    dtpFrom.DateValue = new DateTime(DateTime.Now.Year, 1, 1);
                }
            }

            if (cmbCurrency != null)
            {
                try
                {
                    DataTable dtCurrencies = await _reportRepo.GetCurrenciesLookupAsync();
                    cmbCurrency.DataSource = dtCurrencies;
                    cmbCurrency.DisplayMember = "Currency_Name";
                    cmbCurrency.ValueMember = "Currency_ID";
                    cmbCurrency.DropDownStyle = ComboBoxStyle.DropDownList;
                    cmbCurrency.RightToLeft = RightToLeft.Yes;
                    cmbCurrency.SelectedValue = 0; // 0 = كافة العملات
                }
                catch (Exception ex)
                {
                    DatabaseHelper.LogSystemError(ex.Message, ex.StackTrace, "Load_Currencies_Lookup");
                }
            }
        }

        // =========================================================
        // 2. التنقل السلس بمفتاح Enter
        // =========================================================
        private void txtAccountId_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                SendKeys.Send("{TAB}");
            }
        }

        // =========================================================
        // 3. الالتزام بالمادة (1 و 5): العملية السيادية F9 عبر المستودع بدون SQL مباشر
        // =========================================================
        internal override void OnF9Pressed()
        {
            base.OnF9Pressed();

            Control currentActiveControl = GetActiveControlDeep(this);

            if (currentActiveControl != null && currentActiveControl.Name == "txtAccountId")
            {
                string accountQuery = _reportRepo.GetAccountsLookupQuery();

                using (var searchForm = new UniversalSearchForm("دليل الحسابات", accountQuery))
                {
                    if (searchForm.ShowDialog() == DialogResult.OK)
                    {
                        txtAccountId.Text = searchForm.المعرف_المختار;
                        txtAccountName.Text = searchForm.الاسم_المختار;

                        if (dtpFrom != null) dtpFrom.Focus();
                    }
                }
            }
        }

        // =========================================================
        // 4. التقاط المفاتيح وتوجيه F9 للدالة السيادية الموحدة
        // =========================================================
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            Control currentActiveControl = GetActiveControlDeep(this);

            if (keyData == Keys.F9 && currentActiveControl != null && currentActiveControl.Name == "txtAccountId")
            {
                OnF9Pressed();
                return true;
            }

            if (keyData == Keys.Enter && currentActiveControl != null && currentActiveControl.Name == "txtAccountId")
            {
                SendKeys.Send("{TAB}");
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private Control GetActiveControlDeep(ContainerControl container)
        {
            Control activeControl = container.ActiveControl;
            while (activeControl is ContainerControl nestedContainer)
            {
                activeControl = nestedContainer.ActiveControl;
            }
            return activeControl;
        }

        // =========================================================
        // 5. الالتزام بالمادة (2 و 3): جلب البيانات غير المتزامن وتقسيم التبويبات حسب العملة
        // =========================================================
        private async void btnLoadData_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtAccountId.Text))
            {
                MessageBox.Show("يرجى اختيار الحساب المراد عرض كشفه.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            LockControls(this, true);
            try
            {
                int? selectedCurrencyId = null;
                if (cmbCurrency != null && cmbCurrency.SelectedValue != null &&
                    int.TryParse(cmbCurrency.SelectedValue.ToString(), out int currId) && currId > 0)
                {
                    selectedCurrencyId = currId;
                }

                _currentStatementData = await _reportRepo.GetStatementAsync(
                    txtAccountId.Text.Trim(),
                    dtpFrom.DateValue,
                    dtpTo.DateValue,
                    chkIncludeUnposted.Checked,
                    selectedCurrencyId);

                dgvStatement.DataSource = _currentStatementData;
                SetupDataGridViewFormatting(dgvStatement);

                // تقسيم العرض في الشاشة إلى تبويبات مستقلة لكل عملة
                BindStatementByCurrencyTabs(_currentStatementData);

                if (_currentStatementData == null || _currentStatementData.Count == 0)
                {
                    MessageBox.Show("لا توجد حركات مالية لهذا الحساب في الفترة المحددة.", "معلومة", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                DatabaseHelper.LogSystemError(ex.Message, ex.StackTrace, "Frm_AccountStatement_btnLoadData");
                MessageBox.Show($"حدث خطأ أثناء جلب البيانات: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                LockControls(this, false);
                if (txtAccountName != null) txtAccountName.ReadOnly = true;
                if (dgvStatement != null) dgvStatement.ReadOnly = true;
            }
        }

        // =========================================================
        // 6. محرك التبويبات الديناميكي (TabControl) لتقسيم الجريد حسب العملة
        // =========================================================
        private void BindStatementByCurrencyTabs(List<AccountStatementDto> allData)
        {
            if (_tabCurrencies == null)
            {
                _tabCurrencies = new TabControl
                {
                    Name = "tabCurrencies",
                    Location = dgvStatement.Location,
                    Size = dgvStatement.Size,
                    Anchor = dgvStatement.Anchor,
                    RightToLeft = RightToLeft.Yes,
                    RightToLeftLayout = true,
                    Font = new Font("Arial", 10, FontStyle.Bold)
                };

                Control parentContainer = dgvStatement.Parent ?? this;
                parentContainer.Controls.Add(_tabCurrencies);
                _tabCurrencies.BringToFront();

                // إخفاء الجريد الأصلي واستخدامه كمرجع قياسي
                dgvStatement.Visible = false;
            }

            _tabCurrencies.SuspendLayout();
            _tabCurrencies.TabPages.Clear();

            if (allData == null || allData.Count == 0)
            {
                _tabCurrencies.ResumeLayout();
                return;
            }

            var currencyGroups = allData.GroupBy(x => new { x.Currency_ID, x.Currency_Name });

            foreach (var group in currencyGroups)
            {
                string currencyTitle = !string.IsNullOrWhiteSpace(group.Key.Currency_Name)
                    ? group.Key.Currency_Name
                    : "محلي";

                var groupItems = group.ToList();
                var summaryItem = groupItems.First();

                TabPage tabPage = new TabPage($"{currencyTitle}  ({summaryItem.Final_Balance:#,##0.00} {summaryItem.Balance_Type_Text})")
                {
                    BackColor = Color.White,
                    Padding = new Padding(4),
                    Tag = currencyTitle
                };

                AlRowadDataGridView dgvCurrency = new AlRowadDataGridView
                {
                    Dock = DockStyle.Fill,
                    ReadOnly = true,
                    AllowUserToAddRows = false,
                    AllowUserToDeleteRows = false,
                    AllowUserToOrderColumns = true,
                    BackgroundColor = dgvStatement.BackgroundColor,
                    BorderStyle = dgvStatement.BorderStyle,
                    CellBorderStyle = dgvStatement.CellBorderStyle,
                    ColumnHeadersBorderStyle = dgvStatement.ColumnHeadersBorderStyle,
                    ColumnHeadersDefaultCellStyle = dgvStatement.ColumnHeadersDefaultCellStyle.Clone(),
                    EnableHeadersVisualStyles = false,
                    RightToLeft = RightToLeft.Yes,
                    RowHeadersVisible = false,
                    SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                    DataSource = groupItems
                };

                SetupDataGridViewFormatting(dgvCurrency);

                // شريط الخلاصة الختامية والتفقيط أسفل كل تبويب عملة
                Panel pnlSummary = new Panel
                {
                    Dock = DockStyle.Bottom,
                    Height = 40,
                    BackColor = Color.AliceBlue,
                    BorderStyle = BorderStyle.FixedSingle
                };

                Label lblSummary = new Label
                {
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleRight,
                    Font = new Font("Arial", 10, FontStyle.Bold),
                    ForeColor = Color.DarkBlue,
                    Padding = new Padding(0, 0, 10, 0),
                    Text = $"إجمالي المدين: {summaryItem.Total_Debit:#,##0.00}   |   " +
                           $"إجمالي الدائن: {summaryItem.Total_Credit:#,##0.00}   |   " +
                           $"الرصيد ({summaryItem.Balance_Type_Text}): {summaryItem.Final_Balance:#,##0.00} {currencyTitle} — {summaryItem.Amount_In_Words}"
                };

                pnlSummary.Controls.Add(lblSummary);
                tabPage.Controls.Add(dgvCurrency);
                tabPage.Controls.Add(pnlSummary);

                _tabCurrencies.TabPages.Add(tabPage);
            }

            _tabCurrencies.ResumeLayout();
        }

        // =========================================================
        // 7. إعداد وتنسيق الجريد بالسياسة الموحدة للأرقام والتواريخ
        // =========================================================
        private void SetupDataGridViewFormatting(DataGridView grid)
        {
            if (grid == null || grid.Columns.Count == 0) return;

            grid.RightToLeft = RightToLeft.Yes;
            grid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            grid.RowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

            string numberFormat = SystemConstants.Formatting.DefaultCurrencyFormat;

            if (grid.Columns["Trans_Date"] != null)
            {
                grid.Columns["Trans_Date"].HeaderText = "التاريخ";
                grid.Columns["Trans_Date"].Width = 120;
                grid.Columns["Trans_Date"].DefaultCellStyle.Format = "yyyy-MM-dd";
                grid.Columns["Trans_Date"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                grid.Columns["Trans_Date"].DisplayIndex = 0;
            }

            if (grid.Columns["Description"] != null)
            {
                grid.Columns["Description"].HeaderText = "البيان";
                grid.Columns["Description"].Width = 350;
                grid.Columns["Description"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                grid.Columns["Description"].DisplayIndex = 1;
            }

            if (grid.Columns["Source_ID"] != null)
            {
                grid.Columns["Source_ID"].HeaderText = "رقم المستند";
                grid.Columns["Source_ID"].Width = 100;
                grid.Columns["Source_ID"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                grid.Columns["Source_ID"].DisplayIndex = 2;
            }

            if (grid.Columns["Debit"] != null)
            {
                grid.Columns["Debit"].HeaderText = "مدين";
                grid.Columns["Debit"].Width = 120;
                grid.Columns["Debit"].DefaultCellStyle.Format = numberFormat;
                grid.Columns["Debit"].DisplayIndex = 3;
            }

            if (grid.Columns["Credit"] != null)
            {
                grid.Columns["Credit"].HeaderText = "دائن";
                grid.Columns["Credit"].Width = 120;
                grid.Columns["Credit"].DefaultCellStyle.Format = numberFormat;
                grid.Columns["Credit"].DisplayIndex = 4;
            }

            if (grid.Columns["Running_Balance"] != null)
            {
                grid.Columns["Running_Balance"].HeaderText = "الرصيد التراكمي";
                grid.Columns["Running_Balance"].Width = 140;
                grid.Columns["Running_Balance"].DefaultCellStyle.Format = numberFormat;
                grid.Columns["Running_Balance"].DisplayIndex = 5;
            }

            if (grid.Columns["Currency_Name"] != null)
            {
                grid.Columns["Currency_Name"].HeaderText = "العملة";
                grid.Columns["Currency_Name"].Width = 100;
                grid.Columns["Currency_Name"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                grid.Columns["Currency_Name"].DisplayIndex = 6;
            }

            if (grid.Columns["Post_Status"] != null)
            {
                grid.Columns["Post_Status"].HeaderText = "حالة القيد";
                grid.Columns["Post_Status"].Width = 110;
                grid.Columns["Post_Status"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                grid.Columns["Post_Status"].DisplayIndex = 7;
            }

            grid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
        }

        /// <summary>
        /// الحصول على الشبكة النشطة حالياً (من التبويب المفتوح أو الشبكة الافتراضية)
        /// </summary>
        private DataGridView GetActiveCurrencyGrid(out string activeCurrencyName)
        {
            activeCurrencyName = "كافة العملات";
            if (_tabCurrencies != null && _tabCurrencies.SelectedTab != null)
            {
                activeCurrencyName = _tabCurrencies.SelectedTab.Tag?.ToString() ?? _tabCurrencies.SelectedTab.Text;
                var gridInTab = _tabCurrencies.SelectedTab.Controls.OfType<DataGridView>().FirstOrDefault();
                if (gridInTab != null) return gridInTab;
            }
            return dgvStatement;
        }

        // =========================================================
        // 8. طباعة الشبكة المرنة (تطبع شبكة التبويب النشط مع عملته وخلاصته)
        // =========================================================
        private void btnPrintGrid_Click(object sender, EventArgs e)
        {
            DataGridView targetGrid = GetActiveCurrencyGrid(out string activeCurrencyName);

            if (targetGrid == null || targetGrid.Rows.Count == 0)
            {
                MessageBox.Show("لا توجد بيانات في الشبكة لطباعتها. يرجى تحديث البيانات أولاً.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                PrintDocument printDocument = new PrintDocument();
                int currentRow = 0;

                printDocument.PrintPage += (s, ev) =>
                {
                    float leftMargin = ev.MarginBounds.Left;
                    float topMargin = ev.MarginBounds.Top;
                    float rightMargin = ev.MarginBounds.Right;
                    float currentY = topMargin + 10;

                    StringFormat rtlFormat = new StringFormat
                    {
                        FormatFlags = StringFormatFlags.DirectionRightToLeft
                    };

                    // 1. ترويسة التقرير الرئيسية
                    Font titleFont = new Font("Arial", 14, FontStyle.Bold);
                    ev.Graphics.DrawString($"كشف حساب تفصيلي ({activeCurrencyName}) - نظام الرواد ERP", titleFont, Brushes.DarkBlue, rightMargin, currentY, rtlFormat);
                    currentY += 35;

                    // 2. ترويسة معلومات الحساب
                    Font infoFont = new Font("Arial", 10, FontStyle.Bold);
                    string accountInfo = $"رقم الحساب: {txtAccountId.Text}    |    اسم الحساب: {txtAccountName.Text}    |    العملة: {activeCurrencyName}";
                    ev.Graphics.DrawString(accountInfo, infoFont, Brushes.Black, rightMargin, currentY, rtlFormat);
                    currentY += 25;

                    Font subInfoFont = new Font("Arial", 9, FontStyle.Regular);
                    string periodInfo = $"الفترة من: {dtpFrom.Text} إلى: {dtpTo.Text}";
                    ev.Graphics.DrawString(periodInfo, subInfoFont, Brushes.Gray, rightMargin, currentY, rtlFormat);
                    currentY += 35;

                    // 3. جلب الأعمدة المرئية مرتبة حسب الشاشة
                    var visibleCols = targetGrid.Columns.Cast<DataGridViewColumn>()
                        .Where(c => c.Visible)
                        .OrderBy(c => c.DisplayIndex)
                        .ToList();

                    float totalColsWidth = visibleCols.Sum(c => c.Width);
                    float availableWidth = rightMargin - leftMargin;
                    float scaleFactor = availableWidth / totalColsWidth;

                    Dictionary<int, float> colXPositions = new Dictionary<int, float>();
                    Dictionary<int, float> colWidths = new Dictionary<int, float>();

                    float activeX = rightMargin;
                    foreach (var col in visibleCols)
                    {
                        float adjustedWidth = col.Width * scaleFactor;
                        activeX -= adjustedWidth;
                        colXPositions[col.Index] = activeX;
                        colWidths[col.Index] = adjustedWidth;
                    }

                    // 4. رسم رؤوس الأعمدة
                    Font headerFont = new Font("Arial", 9, FontStyle.Bold);
                    Brush headerBrush = new SolidBrush(Color.LightGray);
                    Pen borderPen = new Pen(Color.Black, 1);

                    foreach (var col in visibleCols)
                    {
                        float colX = colXPositions[col.Index];
                        float colW = colWidths[col.Index];

                        ev.Graphics.FillRectangle(headerBrush, colX, currentY, colW, 25);
                        ev.Graphics.DrawRectangle(borderPen, colX, currentY, colW, 25);

                        ev.Graphics.DrawString(col.HeaderText, headerFont, Brushes.Black,
                            new RectangleF(colX, currentY + 4, colW, 20),
                            new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                    }
                    currentY += 25;

                    // 5. رسم صفوف البيانات
                    Font rowFont = new Font("Arial", 9, FontStyle.Regular);
                    Pen cellPen = new Pen(Color.Silver, 1);
                    float lineHeight = 25;

                    while (currentRow < targetGrid.Rows.Count)
                    {
                        DataGridViewRow row = targetGrid.Rows[currentRow];

                        foreach (var col in visibleCols)
                        {
                            float colX = colXPositions[col.Index];
                            float colW = colWidths[col.Index];

                            object rawVal = row.Cells[col.Index].Value;
                            string cellValue = string.Empty;

                            if (rawVal is DateTime dt)
                            {
                                cellValue = dt.ToString("yyyy-MM-dd");
                            }
                            else if (rawVal is decimal decVal)
                            {
                                cellValue = decVal.ToString(SystemConstants.Formatting.DefaultCurrencyFormat);
                            }
                            else if (rawVal != null)
                            {
                                cellValue = rawVal.ToString();
                            }

                            ev.Graphics.DrawRectangle(cellPen, colX, currentY, colW, lineHeight);

                            StringAlignment alignment = StringAlignment.Near;
                            if (col.Name == "Debit" || col.Name == "Credit" || col.Name == "Running_Balance")
                                alignment = StringAlignment.Far;
                            else if (col.Name == "Trans_Date" || col.Name == "Source_ID" || col.Name == "Post_Status" || col.Name == "Currency_Name")
                                alignment = StringAlignment.Center;

                            StringFormat cellFormat = new StringFormat
                            {
                                Alignment = alignment,
                                LineAlignment = StringAlignment.Center
                            };

                            ev.Graphics.DrawString(cellValue, rowFont, Brushes.Black,
                                new RectangleF(colX + 2, currentY, colW - 4, lineHeight), cellFormat);
                        }

                        currentY += lineHeight;
                        currentRow++;

                        if (currentY + lineHeight > ev.MarginBounds.Bottom)
                        {
                            ev.HasMorePages = true;
                            return;
                        }
                    }

                    // رسم خلاصة العملة أسفل الجدول في الصفحة الأخيرة
                    if (targetGrid.DataSource is List<AccountStatementDto> tabList && tabList.Count > 0)
                    {
                        var summary = tabList.First();
                        currentY += 10;
                        string summaryLine = $"إجمالي المدين: {summary.Total_Debit:#,##0.00}  |  إجمالي الدائن: {summary.Total_Credit:#,##0.00}  |  الرصيد ({summary.Balance_Type_Text}): {summary.Final_Balance:#,##0.00} {summary.Currency_Name}";
                        ev.Graphics.DrawString(summaryLine, infoFont, Brushes.DarkBlue, rightMargin, currentY, rtlFormat);
                        currentY += 22;
                        ev.Graphics.DrawString($"المبلغ كتابةً: {summary.Amount_In_Words}", infoFont, Brushes.DarkRed, rightMargin, currentY, rtlFormat);
                    }

                    ev.HasMorePages = false;
                    currentRow = 0;
                };

                Form customPreviewForm = new Form
                {
                    Text = $"معاينة طباعة كشف الحساب ({activeCurrencyName}) - نظام الرواد ERP",
                    Width = 1000,
                    Height = 800,
                    RightToLeft = RightToLeft.Yes,
                    RightToLeftLayout = true,
                    StartPosition = FormStartPosition.CenterScreen
                };

                PrintPreviewControl previewControl = new PrintPreviewControl
                {
                    Document = printDocument,
                    Dock = DockStyle.Fill,
                    Zoom = 1.0
                };

                FlowLayoutPanel panelTop = new FlowLayoutPanel
                {
                    Dock = DockStyle.Top,
                    Height = 45,
                    FlowDirection = FlowDirection.RightToLeft,
                    Padding = new Padding(10)
                };

                Button btnZoomIn = new Button { Text = "🔍 تكبير (+)", Width = 100, Height = 30 };
                Button btnZoomOut = new Button { Text = "🔍 تصغير (-)", Width = 100, Height = 30 };
                Button btnPrint = new Button { Text = "🖨️ طباعة", Width = 90, Height = 30 };

                btnZoomIn.Click += (s, ev) => { if (previewControl.Zoom < 3.0) previewControl.Zoom += 0.25; };
                btnZoomOut.Click += (s, ev) => { if (previewControl.Zoom > 0.4) previewControl.Zoom -= 0.25; };
                btnPrint.Click += (s, ev) =>
                {
                    PrintDialog pDialog = new PrintDialog { Document = printDocument };
                    if (pDialog.ShowDialog() == DialogResult.OK) printDocument.Print();
                };

                panelTop.Controls.Add(btnPrint);
                panelTop.Controls.Add(btnZoomIn);
                panelTop.Controls.Add(btnZoomOut);

                customPreviewForm.Controls.Add(previewControl);
                customPreviewForm.Controls.Add(panelTop);

                customPreviewForm.ShowDialog(this);
            }
            catch (Exception ex)
            {
                DatabaseHelper.LogSystemError(ex.Message, ex.StackTrace, "Frm_AccountStatement_btnPrintGrid");
                MessageBox.Show($"حدث خطأ أثناء معاينة الطباعة: {ex.Message}", "خطأ فني", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // =========================================================
        // 9. الطباعة الرسمية RDLC: تطبع العملة المختارة أو كافة العملات (كل عملة في صفحة مستقلة)
        // =========================================================
        // =========================================================
        // 9. الطباعة الرسمية RDLC: فصل اسم التقرير الرسمي عن عنوان التبويب ودعم تعدد التقارير غير المقيد
        // =========================================================
        private async void btnPrintOfficial_Click(object sender, EventArgs e)
        {
            if (_currentStatementData == null || _currentStatementData.Count == 0)
            {
                MessageBox.Show("لا توجد بيانات لطباعتها. يرجى استدعاء البيانات أولاً.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // 1. الاسم الرسمي للتقرير (يظهر في منتصف الورقة أعلى التواريخ)
                string officialReportTitle = "تقرير كشف حساب تفصيلي";

                // 2. عنوان التبويب في شاشة العارض (لتمييز التبويبات عند فتح أكثر من كشف حساب)
                string tabHeaderTitle = $"كشف حساب: {txtAccountName.Text}";

                var extraParams = new Dictionary<string, string>
                {
                    ["FromDate"] = dtpFrom.DateValue.ToString("yyyy-MM-dd"),
                    ["ToDate"] = dtpTo.DateValue.ToString("yyyy-MM-dd")
                };

                string dataSetName = "Rpt_AccountStatement";

                // 🌟 استدعاء العارض المركزي غير المقيد مع تمرير اسم التقرير وعنوان التبويب منفصلين
                await Frm_ReportViewer.OpenReportTabAsync(
                    "Rpt_AccountStatement.rdlc",
                    dataSetName,
                    _currentStatementData,
                    officialReportTitle,
                    extraParams,
                    tabHeaderTitle);
            }
            catch (Exception ex)
            {
                DatabaseHelper.LogSystemError(ex.Message, ex.StackTrace, "Frm_AccountStatement_btnPrintOfficial");
                MessageBox.Show($"حدث خطأ أثناء فتح عارض التقارير: {ex.Message}", "خطأ فني", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}