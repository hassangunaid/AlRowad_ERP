using AlRowad_ERP.Core;
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
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AlRowad_ERP.UI.Reports
{
    /// <summary>
    /// شاشة تقرير كشف الحساب - متوافقة مع دستور الرواد ERP المعماري
    /// </summary>
    public partial class Frm_AccountStatement : BaseEntryForm
    {
        private readonly ReportAccountRepository _reportRepo;

        public Frm_AccountStatement()
        {
            InitializeComponent();
            _reportRepo = new ReportAccountRepository();

            // تهيئة الجريد ليكون مرناً للمستخدم
            if (dgvStatement != null)
            {
                dgvStatement.AllowUserToOrderColumns = true;
                dgvStatement.ReadOnly = true;
                dgvStatement.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                SetupDataGridViewFormatting();
            }

            // ربط أحداث الأزرار بأمان مع منع التكرار
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

        // =========================================================
        // 1. الالتزام بالمادة (1 و 2 و 3 و 5): تهيئة الشاشة وجلب الفترة عبر المستودع
        // =========================================================
        protected override async void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // فك القفل عن الشاشة عبر الدالة المركزية لأنها شاشة استعلام (تقرير)
            LockControls(this, false);

            // تفعيل خيار "كشف حساب قبل الترحيل" وتاريخ اليوم افتراضياً
            if (chkIncludeUnposted != null) chkIncludeUnposted.Checked = true;
            if (dtpTo != null) dtpTo.DateValue = DateTime.Now.Date;

            // حماية الحقول التي لا يجب الكتابة فيها يدوياً
            if (txtAccountName != null) txtAccountName.ReadOnly = true;
            if (dgvStatement != null) dgvStatement.ReadOnly = true;

            // ربط أحداث الإدخال والبحث
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

            // جلب تاريخ "بداية الفترة المالية" بشكل غير متزامن من المستودع (Repository) مباشرة
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
                // استدعاء الاستعلام من طبقة البيانات (ReportAccountRepository)
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
        // 5. الالتزام بالمادة (2 و 3): جلب البيانات غير المتزامن (Async/Await)
        // =========================================================
        private async void btnLoadData_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtAccountId.Text))
            {
                MessageBox.Show("يرجى اختيار الحساب المراد عرض كشفه.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // قفل الشاشة مركزياً أثناء المعالجة
            LockControls(this, true);
            try
            {
                var data = await _reportRepo.GetStatementAsync(
                    txtAccountId.Text,
                    dtpFrom.DateValue,
                    dtpTo.DateValue,
                    chkIncludeUnposted.Checked);

                dgvStatement.DataSource = data;
                SetupDataGridViewFormatting();

                if (data == null || data.Count == 0)
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
                // إعادة فتح الأدوات عبر الدالة المركزية بعد انتهاء الجلب
                LockControls(this, false);
                if (txtAccountName != null) txtAccountName.ReadOnly = true;
                if (dgvStatement != null) dgvStatement.ReadOnly = true;
            }
        }

        // =========================================================
        // 6. إعداد وتنسيق الجريد (الترتيب المحاسبي الاحترافي، RTL، والاحتواء التلقائي)
        // =========================================================
        private void SetupDataGridViewFormatting()
        {
            if (dgvStatement == null) return;

            dgvStatement.RightToLeft = RightToLeft.Yes;
            dgvStatement.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvStatement.RowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvStatement.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

            // أ. التاريخ
            if (dgvStatement.Columns["Trans_Date"] != null)
            {
                dgvStatement.Columns["Trans_Date"].HeaderText = "التاريخ";
                dgvStatement.Columns["Trans_Date"].Width = 130;
                dgvStatement.Columns["Trans_Date"].DefaultCellStyle.Format = "yyyy-MM-dd";
                dgvStatement.Columns["Trans_Date"].DisplayIndex = 0;
            }

            // ب. البيان
            if (dgvStatement.Columns["Description"] != null)
            {
                dgvStatement.Columns["Description"].HeaderText = "البيان";
                dgvStatement.Columns["Description"].Width = 350;
                dgvStatement.Columns["Description"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                dgvStatement.Columns["Description"].DisplayIndex = 1;
            }

            // ج. رقم المستند
            if (dgvStatement.Columns["Source_ID"] != null)
            {
                dgvStatement.Columns["Source_ID"].HeaderText = "رقم المستند";
                dgvStatement.Columns["Source_ID"].Width = 100;
                dgvStatement.Columns["Source_ID"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvStatement.Columns["Source_ID"].Visible = true;
                dgvStatement.Columns["Source_ID"].DisplayIndex = 2;
            }

            // د. مدين
            if (dgvStatement.Columns["Debit"] != null)
            {
                dgvStatement.Columns["Debit"].HeaderText = "مدين";
                dgvStatement.Columns["Debit"].Width = 110;
                dgvStatement.Columns["Debit"].DefaultCellStyle.Format = "N2";
                dgvStatement.Columns["Debit"].DisplayIndex = 3;
            }

            // هـ. دائن
            if (dgvStatement.Columns["Credit"] != null)
            {
                dgvStatement.Columns["Credit"].HeaderText = "دائن";
                dgvStatement.Columns["Credit"].Width = 110;
                dgvStatement.Columns["Credit"].DefaultCellStyle.Format = "N2";
                dgvStatement.Columns["Credit"].DisplayIndex = 4;
            }

            // و. الرصيد التراكمي
            if (dgvStatement.Columns["Running_Balance"] != null)
            {
                dgvStatement.Columns["Running_Balance"].HeaderText = "الرصيد التراكمي";
                dgvStatement.Columns["Running_Balance"].Width = 140;
                dgvStatement.Columns["Running_Balance"].DefaultCellStyle.Format = "N2";
                dgvStatement.Columns["Running_Balance"].DisplayIndex = 5;
            }

            // ز. حالة القيد
            if (dgvStatement.Columns["Post_Status"] != null)
            {
                dgvStatement.Columns["Post_Status"].HeaderText = "حالة القيد";
                dgvStatement.Columns["Post_Status"].Width = 120;
                dgvStatement.Columns["Post_Status"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvStatement.Columns["Post_Status"].DisplayIndex = 6;
            }

            dgvStatement.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
        }

        // =========================================================
        // 7. طباعة الشبكة المرنة (معاينة مخصصة، زوم، وترويسة الحساب) - كاملة
        // =========================================================
        private void btnPrintGrid_Click(object sender, EventArgs e)
        {
            if (dgvStatement == null || dgvStatement.Rows.Count == 0)
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
                    ev.Graphics.DrawString("كشف حساب تفصيلي - نظام الرواد ERP", titleFont, Brushes.DarkBlue, rightMargin, currentY, rtlFormat);
                    currentY += 35;

                    // 2. ترويسة معلومات الحساب (الرقم والاسم)
                    Font infoFont = new Font("Arial", 10, FontStyle.Bold);
                    string accountInfo = $"رقم الحساب: {txtAccountId.Text}    |    اسم الحساب: {txtAccountName.Text}";
                    ev.Graphics.DrawString(accountInfo, infoFont, Brushes.Black, rightMargin, currentY, rtlFormat);
                    currentY += 25;

                    Font subInfoFont = new Font("Arial", 9, FontStyle.Regular);
                    string periodInfo = $"الفترة من: {dtpFrom.Text} إلى: {dtpTo.Text}";
                    ev.Graphics.DrawString(periodInfo, subInfoFont, Brushes.Gray, rightMargin, currentY, rtlFormat);
                    currentY += 35;

                    // 3. جلب الأعمدة المرئية مرتبة تماماً حسب الشاشة (DisplayIndex)
                    var visibleCols = dgvStatement.Columns.Cast<DataGridViewColumn>()
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

                    while (currentRow < dgvStatement.Rows.Count)
                    {
                        DataGridViewRow row = dgvStatement.Rows[currentRow];

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
                            else if (rawVal != null)
                            {
                                cellValue = rawVal.ToString();
                                if (DateTime.TryParse(cellValue, out DateTime parsedDate) && cellValue.Contains("00:00:00"))
                                {
                                    cellValue = parsedDate.ToString("yyyy-MM-dd");
                                }
                            }

                            ev.Graphics.DrawRectangle(cellPen, colX, currentY, colW, lineHeight);

                            StringAlignment alignment = StringAlignment.Near;
                            if (col.Name == "Debit" || col.Name == "Credit" || col.Name == "Running_Balance")
                                alignment = StringAlignment.Far;
                            else if (col.Name == "Trans_Date" || col.Name == "Source_ID" || col.Name == "Post_Status")
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

                    ev.HasMorePages = false;
                    currentRow = 0;
                };

                // 6. نافذة المعاينة المخصصة
                Form customPreviewForm = new Form
                {
                    Text = "معاينة طباعة كشف الحساب - نظام الرواد ERP",
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
        // 8. الطباعة الرسمية: فصل اسم التقرير العام عن بيانات صاحب الحساب
        // =========================================================
        private async void btnPrintOfficial_Click(object sender, EventArgs e)
        {
            var currentData = dgvStatement.DataSource as List<AccountStatementDto>;
            if (currentData == null || currentData.Count == 0)
            {
                MessageBox.Show("لا توجد بيانات لطباعتها. يرجى استدعاء البيانات أولاً.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // 1. المتغير الخاص باسم التقرير العام (يظهر في الترويسة مكان [@ReportTitle])
                // يمكنك قراءته ديناميكياً من عنوان الشاشة this.Text أو كتابته بوضوح:
                string reportTitle = "تقرير كشف حساب تفصيلي";

                // 2. المتغيرات الخاصة بتقرير كشف الحساب (تظهر في الـ Body أسفل الخط)
                var extraParams = new Dictionary<string, string>
                {
                    ["FromDate"] = dtpFrom.DateValue.ToString("yyyy-MM-dd"),
                    ["ToDate"] = dtpTo.DateValue.ToString("yyyy-MM-dd"),
                    ["AccountId"] = txtAccountId.Text,
                    ["AccountName"] = txtAccountName.Text
                };

                using (Frm_ReportViewer viewer = new Frm_ReportViewer())
                {
                    string dataSetName = "Rpt_AccountStatement";

                    // إرسال اسم التقرير الصافي لـ [@ReportTitle]، وإرسال صاحب الحساب ضمن extraParams
                    await viewer.LoadReportAsync("Rpt_AccountStatement.rdlc", dataSetName, currentData, reportTitle, extraParams);
                    viewer.ShowDialog(this);
                }
            }
            catch (Exception ex)
            {
                DatabaseHelper.LogSystemError(ex.Message, ex.StackTrace, "Frm_AccountStatement_btnPrintOfficial");
                MessageBox.Show($"حدث خطأ أثناء فتح عارض التقارير: {ex.Message}", "خطأ فني", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}