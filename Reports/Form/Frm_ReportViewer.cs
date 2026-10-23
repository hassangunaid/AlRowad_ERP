using AlRowad_ERP.Core;
using AlRowad_ERP.Core.Constants;
using AlRowad_ERP.Repositories;
using AlRowad_ERP.UI.Base;
using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AlRowad_ERP.Reports.Form
{
    public partial class Frm_ReportViewer : BaseEntryForm
    {
        private readonly CompanyRepository _companyRepo;
        private readonly SmoothScrollMessageFilter _scrollFilter;
        private static Frm_ReportViewer _activeViewerInstance;

        public Frm_ReportViewer()
        {
            InitializeComponent();
            _companyRepo = new CompanyRepository();

            this.RightToLeft = RightToLeft.Yes;
            this.RightToLeftLayout = true;

            // 🌟 إظهار العارض في شريط المهام كنافذة مستقلة بأيقونة مميزة للتقارير
            this.ShowInTaskbar = true;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.WindowState = FormWindowState.Maximized;
            this.Icon = CreateReportIcon();

            // 🌟 تفعيل فلتر التمرير العمودي السلس (لمنع القفز السريع للورقة)
            _scrollFilter = new SmoothScrollMessageFilter(this);
            Application.AddMessageFilter(_scrollFilter);
        }

        /// <summary>
        /// 🌟 فتح التقرير في العارض المركزي (إذا كان العارض مفتوحاً يضيف تبويباً جديداً بداخله بدلاً من فتح نافذة جديدة)
        /// </summary>
        public static async Task OpenReportTabAsync(
            string reportFileOrPath,
            string dataSetName,
            object dataSource,
            string reportTitle = "تقرير نظام الرواد",
            Dictionary<string, string> extraParameters = null,
            string tabTitle = null)
        {
            if (_activeViewerInstance == null || _activeViewerInstance.IsDisposed)
            {
                _activeViewerInstance = new Frm_ReportViewer();
            }

            if (!_activeViewerInstance.Visible)
            {
                // عرض غير مقيد (Modeless) بدون Owner لفصله تماماً في شريط المهام وإتاحة العمل على الشاشات الأخرى
                _activeViewerInstance.Show();
            }
            else
            {
                if (_activeViewerInstance.WindowState == FormWindowState.Minimized)
                    _activeViewerInstance.WindowState = FormWindowState.Maximized;

                _activeViewerInstance.Activate();
            }

            await _activeViewerInstance.LoadReportAsync(
                reportFileOrPath,
                dataSetName,
                dataSource,
                reportTitle,
                extraParameters,
                tabTitle);
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            // 🌟 فصل أيقونة عارض التقارير عن أيقونة النظام الرئيسي في شريط مهام ويندوز (Windows Taskbar)
            SetWindowAppUserModelId(this.Handle, "AlRowad.ERP.ReportViewer.IndependentWindow");
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            if (_scrollFilter != null)
            {
                Application.RemoveMessageFilter(_scrollFilter);
            }
            if (_activeViewerInstance == this)
            {
                _activeViewerInstance = null;
            }
            base.OnFormClosed(e);
        }

        public static string ResolveReportPath(string reportFileName)
        {
            if (File.Exists(reportFileName))
                return reportFileName;

            string fileNameOnly = Path.GetFileName(reportFileName);
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string projectDir = Path.GetFullPath(Path.Combine(baseDir, @"..\..\"));

            string[] candidatePaths = new[]
            {
                Path.Combine(projectDir, "Reports", "Templates", fileNameOnly),
                Path.Combine(projectDir, "Reports", fileNameOnly),
                Path.Combine(baseDir, "Reports", "Templates", fileNameOnly),
                Path.Combine(baseDir, "Reports", fileNameOnly),
                Path.Combine(baseDir, fileNameOnly)
            };

            foreach (var path in candidatePaths)
            {
                if (File.Exists(path))
                    return path;
            }

            throw new FileNotFoundException($"لم يتم العثور على ملف التقرير ({fileNameOnly}) في أي من المسارات المعتمدة للنظام.");
        }

        /// <summary>
        /// 🌟 تحميل التقرير في تبويب مستقل مع فصل عنوان التقرير الرسمي عن عنوان التبويب وضبط الزوم 100%
        /// </summary>
        public async Task LoadReportAsync(
            string reportFileOrPath,
            string dataSetName,
            object dataSource,
            string reportTitle = "تقرير نظام الرواد",
            Dictionary<string, string> extraParameters = null,
            string tabTitle = null)
        {
            try
            {
                string resolvedPath = ResolveReportPath(reportFileOrPath);

                // عنوان التبويب (يعرض مثلاً "كشف حساب: علي حشيبر") بينما reportTitle يعرض "تقرير كشف حساب تفصيلي"
                string effectiveTabTitle = !string.IsNullOrWhiteSpace(tabTitle) ? tabTitle : reportTitle;

                TabPage tabPage = new TabPage(effectiveTabTitle)
                {
                    BackColor = Color.White,
                    Padding = new Padding(4)
                };

                ReportViewer reportViewer = new ReportViewer
                {
                    Dock = DockStyle.Fill,
                    ProcessingMode = ProcessingMode.Local
                };

                reportViewer.LocalReport.DataSources.Clear();
                reportViewer.LocalReport.ReportPath = resolvedPath;

                if (!string.IsNullOrWhiteSpace(dataSetName) && dataSource != null)
                {
                    ReportDataSource rds = new ReportDataSource(dataSetName, dataSource);
                    reportViewer.LocalReport.DataSources.Add(rds);
                }

                var companyInfo = await _companyRepo.GetCompanyInfoAsync();

                var allParams = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    ["NumberFormat"] = SystemConstants.Formatting.DefaultCurrencyFormat,
                    ["CompanyNameAr"] = companyInfo?.Company_Name_Ar ?? string.Empty,
                    ["CompanyNameEn"] = companyInfo?.Company_Name_En ?? string.Empty,
                    ["CompanyActivityDesc"] = companyInfo?.Company_Activity_Desc ?? string.Empty,
                    ["CompanyAddress"] = companyInfo?.Address ?? string.Empty,
                    ["CompanyPhone"] = companyInfo?.Phone ?? string.Empty,
                    ["TaxNumber"] = companyInfo?.Tax_Number ?? string.Empty,
                    ["CommercialRecord"] = companyInfo?.Commercial_Record ?? string.Empty,
                    ["CompanyLogo"] = companyInfo?.Logo != null ? Convert.ToBase64String(companyInfo.Logo) : string.Empty,
                    ["ReportTitle"] = reportTitle ?? "تقرير نظام الرواد",
                    ["PrintDate"] = $"تاريخ الطباعة: {DateTime.Now:yyyy/MM/dd HH:mm} | المستخدم: {SystemConstants.CurrentUser?.Username ?? "System"}"
                };

                if (extraParameters != null)
                {
                    foreach (var kvp in extraParameters)
                    {
                        allParams[kvp.Key] = kvp.Value ?? string.Empty;
                    }
                }

                var definedParamInfo = reportViewer.LocalReport.GetParameters();
                var validParametersToSet = new List<ReportParameter>();

                foreach (var paramInfo in definedParamInfo)
                {
                    if (allParams.TryGetValue(paramInfo.Name, out string val))
                    {
                        validParametersToSet.Add(new ReportParameter(paramInfo.Name, val));
                    }
                }

                if (validParametersToSet.Count > 0)
                {
                    reportViewer.LocalReport.SetParameters(validParametersToSet);
                }

                // 🌟 ضبط وضع الطباعة والحجم الافتراضي على 100% تماماً
                reportViewer.SetDisplayMode(DisplayMode.PrintLayout);
                reportViewer.ZoomMode = ZoomMode.Percent;
                reportViewer.ZoomPercent = 100;

                tabPage.Controls.Add(reportViewer);
                tabControlReports.TabPages.Add(tabPage);
                tabControlReports.SelectedTab = tabPage;

                reportViewer.RefreshReport();
            }
            catch (Exception ex)
            {
                string detailedError = ex.InnerException?.InnerException?.Message
                    ?? ex.InnerException?.Message
                    ?? ex.Message;

                DatabaseHelper.LogSystemError(detailedError, ex.StackTrace, "Frm_ReportViewer_LoadReportAsync");
                MessageBox.Show($"خطأ تقني في تشغيل التقرير:\n{detailedError}", "خطأ محرك التقارير", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // =========================================================================
        // 🌟 محرك التمرير العمودي السلس (يعالج سرعة التمرير المفرطة في ReportViewer)
        // =========================================================================
        // =========================================================================
        // 🌟 محرك التمرير العمودي السلس والمدعم للوحة اللمس (Trackpad & Mouse Wheel)
        // =========================================================================
        private class SmoothScrollMessageFilter : IMessageFilter
        {
            private const int WM_MOUSEWHEEL = 0x020A;
            private const int WM_VSCROLL = 0x0115;
            private const int SB_LINEUP = 0;
            private const int SB_LINEDOWN = 1;

            private readonly Control _ownerForm;
            private int _deltaAccumulator = 0; // تجميع الإشارات الدقيقة القادمة من لوحة اللمس

            [DllImport("user32.dll")]
            private static extern IntPtr WindowFromPoint(Point pt);

            [DllImport("user32.dll")]
            private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

            public SmoothScrollMessageFilter(Control ownerForm)
            {
                _ownerForm = ownerForm;
            }

            public bool PreFilterMessage(ref Message m)
            {
                if (m.Msg == WM_MOUSEWHEEL && _ownerForm != null && _ownerForm.Visible && System.Windows.Forms.Form.ActiveForm == _ownerForm)
                {
                    // إذا كان زر Ctrl مضغوطاً، نعمل زูم طبيعي (Ctrl + Wheel)
                    if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
                        return false;

                    IntPtr hWndUnderMouse = WindowFromPoint(Cursor.Position);
                    if (hWndUnderMouse != IntPtr.Zero)
                    {
                        int delta = (short)((m.WParam.ToInt64() >> 16) & 0xFFFF);

                        // 🌟 تجميع قيم التمرير لأن لوحة اللمس ترسل قيم صغيرة متواصلة
                        _deltaAccumulator += delta;

                        // عندما تصل الإشارات المتراكمة للحد المناسب، يتم تحريك التقرير بسلاسة
                        if (Math.Abs(_deltaAccumulator) >= 30)
                        {
                            int direction = _deltaAccumulator > 0 ? SB_LINEUP : SB_LINEDOWN;

                            // تمرير ناعم ومضبوط
                            SendMessage(hWndUnderMouse, WM_VSCROLL, (IntPtr)direction, IntPtr.Zero);

                            _deltaAccumulator = 0; // إعادة تعيين المجمع للحركة التالية
                        }

                        return true; // منع القفزة الكبيرة الافتراضية وتفعيل التمرير السلس
                    }
                }
                return false;
            }
        }
        // =========================================================================
        // 🌟 فصل أيقونة النافذة في شريط مهام ويندوز (Windows Taskbar Grouping API)
        // =========================================================================
        [DllImport("shell32.dll", SetLastError = true)]
        private static extern int SHGetPropertyStoreForWindow(IntPtr handle, ref Guid riid, [Out, MarshalAs(UnmanagedType.Interface)] out IPropertyStore propertyStore);

        [ComImport, Guid("886D8EEB-8CF2-4446-8D02-CDBA1DBDCF99"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IPropertyStore
        {
            int GetCount([Out] out uint cProps);
            int GetAt([In] uint iProp, out PropertyKey pkey);
            int GetValue([In] ref PropertyKey key, [Out] out PropVariant pv);
            int SetValue([In] ref PropertyKey key, [In] ref PropVariant pv);
            int Commit();
        }

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        private struct PropertyKey
        {
            public Guid fmtid;
            public uint pid;
        }

        [StructLayout(LayoutKind.Explicit)]
        private struct PropVariant
        {
            [FieldOffset(0)] public ushort vt;
            [FieldOffset(8)] public IntPtr pszVal;
        }

        private static void SetWindowAppUserModelId(IntPtr hwnd, string appId)
        {
            try
            {
                Guid iid = new Guid("886D8EEB-8CF2-4446-8D02-CDBA1DBDCF99");
                if (SHGetPropertyStoreForWindow(hwnd, ref iid, out IPropertyStore store) == 0 && store != null)
                {
                    PropertyKey pkey = new PropertyKey
                    {
                        fmtid = new Guid("9F4C2855-9F79-4B39-A8D0-E1D42DE1D5F3"),
                        pid = 5 // PKEY_AppUserModel_ID
                    };
                    PropVariant pv = new PropVariant
                    {
                        vt = 31, // VT_LPWSTR
                        pszVal = Marshal.StringToCoTaskMemUni(appId)
                    };
                    store.SetValue(ref pkey, ref pv);
                    store.Commit();
                    Marshal.FreeCoTaskMem(pv.pszVal);
                }
            }
            catch
            {
                // تجاهل آمن في حال الأنظمة القديمة
            }
        }

        private static Icon CreateReportIcon()
        {
            Bitmap bmp = new Bitmap(32, 32);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                using (Brush bgBrush = new SolidBrush(Color.FromArgb(0, 78, 152)))
                    g.FillRectangle(bgBrush, 2, 2, 28, 28);
                using (Brush pageBrush = new SolidBrush(Color.White))
                    g.FillRectangle(pageBrush, 6, 5, 20, 22);
                using (Pen linePen = new Pen(Color.FromArgb(0, 78, 152), 2))
                {
                    g.DrawLine(linePen, 9, 10, 23, 10);
                    g.DrawLine(linePen, 9, 15, 23, 15);
                    g.DrawLine(linePen, 9, 20, 18, 20);
                }
            }
            return Icon.FromHandle(bmp.GetHicon());
        }
    }
}