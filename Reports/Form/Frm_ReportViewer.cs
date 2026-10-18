using AlRowad_ERP.Core;
using AlRowad_ERP.Core.Constants;
using AlRowad_ERP.Repositories;
using AlRowad_ERP.UI.Base;
using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AlRowad_ERP.Reports.Form
{
    public partial class Frm_ReportViewer : BaseEntryForm
    {
        private readonly CompanyRepository _companyRepo;

        public Frm_ReportViewer()
        {
            InitializeComponent();
            _companyRepo = new CompanyRepository();

            this.RightToLeft = RightToLeft.Yes;
            this.RightToLeftLayout = true;
        }

        /// <summary>
        /// 🌟 مكتشف المسارات الذكي: يحل مشكلة مسارات التقارير نهائياً في بيئة التطوير وبيئة النشر
        /// </summary>
        public static string ResolveReportPath(string reportFileName)
        {
            // إذا تم تمرير مسار كامل وموجود بالفعل، أعده فوراً
            if (File.Exists(reportFileName))
                return reportFileName;

            string fileNameOnly = Path.GetFileName(reportFileName);
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string projectDir = Path.GetFullPath(Path.Combine(baseDir, @"..\..\"));

            // قائمة المسارات المحتملة مرتبة حسب الأولوية
            string[] candidatePaths = new[]
            {
                // 1. مجلد السورس الأساسي (أثناء التطوير ليرى المبرمج التعديلات فوراً دون الحاجة لـ Copy Always)
                Path.Combine(projectDir, "Reports", "Templates", fileNameOnly),
                Path.Combine(projectDir, "Reports", fileNameOnly),
                // 2. مجلد التشغيل التنفيذي (عند النشر للعميل)
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
        /// 🌟 دالة العرض المركزية غير المتزامنة مع فحص آمن للبارامترات ودعم البارامترات المخصصة
        /// </summary>
        public async Task LoadReportAsync(
            string reportFileOrPath,
            string dataSetName,
            object dataSource,
            string reportTitle = "تقرير نظام الرواد",
            Dictionary<string, string> extraParameters = null)
        {
            try
            {
                // 1. تحديد المسار الصحيح للملف
                string resolvedPath = ResolveReportPath(reportFileOrPath);

                reportViewer1.LocalReport.DataSources.Clear();
                reportViewer1.LocalReport.ReportPath = resolvedPath;

                // 2. ربط مصدر البيانات (إذا تم تمريره)
                if (!string.IsNullOrWhiteSpace(dataSetName) && dataSource != null)
                {
                    ReportDataSource rds = new ReportDataSource(dataSetName, dataSource);
                    reportViewer1.LocalReport.DataSources.Add(rds);
                }

                // 3. جلب معلومات الشركة من قاعدة البيانات
                var companyInfo = await _companyRepo.GetCompanyInfoAsync();

                // تجميع كافة البارامترات القياسية
                var allParams = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
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

                // دمج البارامترات الإضافية الخاصة بالتقرير (مثل من تاريخ، إلى تاريخ، اسم الحساب)
                if (extraParameters != null)
                {
                    foreach (var kvp in extraParameters)
                    {
                        allParams[kvp.Key] = kvp.Value ?? string.Empty;
                    }
                }

                // 4. 🌟 حماية ذكية: إرسال البارامترات المعرفة فعلياً داخل ملف RDLC فقط لمنع انهيار التقرير
                var definedParamInfo = reportViewer1.LocalReport.GetParameters();
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
                    reportViewer1.LocalReport.SetParameters(validParametersToSet);
                }

                // 5. ضبط إعدادات العرض والطباعة
                reportViewer1.SetDisplayMode(DisplayMode.PrintLayout);
                reportViewer1.ZoomMode = ZoomMode.PageWidth;
                reportViewer1.RefreshReport();
            }
            catch (Exception ex)
            {
                string detailedError = ex.InnerException != null
                    ? $"{ex.Message}\nالتفاصيل: {ex.InnerException.Message}"
                    : ex.Message;

                DatabaseHelper.LogSystemError(detailedError, ex.StackTrace, "Frm_ReportViewer_LoadReportAsync");
                MessageBox.Show($"خطأ تقني في تشغيل التقرير:\n{detailedError}", "خطأ محرك التقارير", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}