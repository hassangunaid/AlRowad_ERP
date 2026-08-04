using AlRowad_ERP.Core;
using AlRowad_ERP.Core.Constants;
using AlRowad_ERP.Core.Constants;
using AlRowad_ERP.Core.Entities;
using AlRowad_ERP.Data;
using AlRowad_ERP.UI.Base;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AlRowadERP.UI.Themes
{
    // 1. الهيكلية الوراثية: الوراثة حصراً من BaseEntryForm
    public partial class Frm_ThemeManager : BaseEntryForm
    {
        // 2. الذاكرة المؤقتة (Caching): خريطة ربط الأدوات بثوابت قاعدة البيانات لمنع الـ Looping
        private readonly Dictionary<Control, string> _themeTokensMap;

        public Frm_ThemeManager()
        {
            InitializeComponent();

            // بناء خريطة الربط مرة واحدة في الذاكرة لتسريع الحفظ والجلب (No Magic Strings)
            _themeTokensMap = new Dictionary<Control, string>
            {
                { cmbFontFamily, ThemeTokens.FontFamily },
                { numFontSize, ThemeTokens.FontSize }
                // مستقبلاً: نضيف هنا أي أدوات أخرى مثل ButtonStyle
            };
        }

        // 3. السلاسة والأداء العالي: تحميل البيانات الثقيلة بـ Async دون تجميد الشاشة
        private async void Frm_ThemeManager_Load(object sender, EventArgs e)
        {
            // تنفيذ العمليات بشكل متوازٍ (Parallel) لتقليل وقت التحميل
            await Task.WhenAll(
                LoadPredefinedThemesAsync(),
                LoadSystemFontsAsync()
            );
        }
        protected override async Task<bool> ExecuteDeleteFromDatabaseAsync(SqlTransaction transaction)
        {
            // 1. استخراج رقم السجل المراد حذفه من الواجهة
            var controls = this.Controls.Find(PrimaryIdFieldName, true);
            if (controls.Length == 0 || !(controls[0] is TextBox idField) || string.IsNullOrWhiteSpace(idField.Text))
                return false;

            int themeId = Convert.ToInt32(idField.Text);
            int deletedByUserId = this.CurrentUserId; // قراءة هوية المستخدم من الأب

            // 2. تفويض المهمة إلى طبقة البيانات (Repository)
            return await ThemeRepository.SoftDeleteAsync(themeId, deletedByUserId, transaction);
        }

        private async Task LoadPredefinedThemesAsync()
        {
            try
            {
                var themes = await AlRowad_ERP.Data.ThemeRepository.GetPredefinedThemesAsync();
                cmbPredefinedThemes.DataSource = themes;
                cmbPredefinedThemes.DisplayMember = "ThemeName";
                cmbPredefinedThemes.ValueMember = "ThemeID";
            }
            catch (Exception ex)
            {
                LogError(ex); // دالة مركزية من الـ BaseEntryForm
            }
        }

        private async Task LoadSystemFontsAsync()
        {
            // تشغيل جلب الخطوط في مسار منفصل لضمان أقصى سرعة
            await Task.Run(() =>
            {
                var fonts = new List<string>();
                foreach (var fontFamily in System.Drawing.FontFamily.Families)
                {
                    fonts.Add(fontFamily.Name);
                }

                // العودة للمسار الرئيسي (UI Thread) لتعبئة الأداة
                this.Invoke((MethodInvoker)delegate {
                    cmbFontFamily.DataSource = fonts;
                });
            });
        }

        // 4. العمليات السيادية (Overrides): التجاوز للتحكم بحالة الشاشة
        public override void OnNew()
        {
            base.OnNew(); // سيقوم بتهيئة حالة الشاشة وفتح الأدوات عبر LockControls

            // تصفير القيم
            cmbPredefinedThemes.SelectedIndex = -1;
            cmbFontFamily.SelectedIndex = -1;
            numFontSize.Value = 10; // القيمة الافتراضية للخط
        }

        // 5. سلامة البيانات (ACID) & السلاسة: عملية الحفظ 
        protected override async Task<bool> ExecuteSaveToDatabaseAsync()
        {
            try
            {
                // أ. تجهيز Master Data
                var themeMaster = new ThemeEntity
                {
                    ThemeID = (this.CurrentMode == FormMode.New) ? 0 : Convert.ToInt32(txtThemeID.Text),
                    ThemeName = txtThemeName.Text.Trim(),
                    IsActive = chkIsActive.Checked,
                    CreatedBy = AlRowad_ERP.Core.Constants.SystemConstants.CurrentUser.Username, 
                    IsPredefined = false // لأن المستخدم هو من يحفظه، فلا يكون ثيماً جاهزاً للنظام
                };

                // ب. تجهيز Details Data (Tokens) عبر الذاكرة المؤقتة (Caching Dictionary)
                var themeTokensList = new List<ThemeTokenEntity>();
                foreach (var mapping in _themeTokensMap)
                {
                    string valueToSave = string.Empty;

                    // معالجة نوع الأداة للحصول على القيمة
                    if (mapping.Key is ComboBox cmb) valueToSave = cmb.Text;
                    else if (mapping.Key is NumericUpDown num) valueToSave = num.Value.ToString();

                    themeTokensList.Add(new ThemeTokenEntity
                    {
                        TokenKey = mapping.Value,
                        TokenValue = valueToSave
                    });
                }

                // ج. استدعاء المستودع ليتم الحفظ ككتلة واحدة Transaction
                return await AlRowad_ERP.Data.ThemeRepository.SaveThemeWithTokensAsync(themeMaster, themeTokensList, this.CurrentMode);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return false;
            }
        }
    }
}