using AlRowad_ERP.Core;
using AlRowad_ERP.Core.Constants;
using AlRowad_ERP.Core.Entities;
using AlRowad_ERP.Core.Helpers;
using AlRowad_ERP.Data;      // للوصول إلى ThemeRepository
using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AlRowad_ERP.UI.Engines
{
    public static class ThemeManager
    {
        private static ThemeEntity _activeTheme;

        // تطبيق الدستور: استخدام التخزين المؤقت (Caching) لمرة واحدة
        public static async Task InitializeAsync()
        {
            try
            {
                // الاعتماد المباشر على طبقة البيانات لجلب الثيم الافتراضي
                // تم إلغاء new ThemeRepository لأن الكلاس static
                _activeTheme = await ThemeRepository.GetDefaultThemeAsync();

                // إذا لم يعثر على ثيم افتراضي، يمكن تعيين ثيم أساسي من الذاكرة (Fallback)
                if (_activeTheme == null)
                {
                    // _activeTheme = DefaultSystemTheme;
                }
            }
            catch (Exception ex)
            {
                // يجب تسجيل الخطأ هنا عبر LogError أو نظام التسجيل المركزي
                // لمنع انهيار النظام عند بداية التشغيل
                throw new Exception("فشل في تهيئة مظهر النظام: " + ex.Message);
            }
        }
        public static void ApplyThemeToForm(Form form)
        {
            if (_activeTheme == null || form == null) return;

            Color bgColor = GetColorFromToken(ThemeTokens.BackgroundColor, SystemColors.Control);
            Color primaryColor = GetColorFromToken(ThemeTokens.PrimaryColor, Color.SteelBlue);
            Color textOnPrimary = GetColorFromToken(ThemeTokens.TextOnPrimary, Color.White);
            Color textPrimary = GetColorFromToken(ThemeTokens.TextPrimary, Color.Black);
            Color surfaceColor = GetColorFromToken(ThemeTokens.SurfaceColor, Color.White);

            form.BackColor = bgColor;

            if (float.TryParse(GetTokenValue(ThemeTokens.BaseFontSize), out float fs))
            {
                string fontFamily = GetTokenValue(ThemeTokens.BaseFontFamily) ?? "Segoe UI";
                form.Font = new Font(fontFamily, fs);
            }

            ApplyThemeToControls(form.Controls, primaryColor, textOnPrimary, textPrimary, surfaceColor);
        }

        private static void ApplyThemeToControls(Control.ControlCollection controls, Color primary, Color textOnPrimary, Color textPrimary, Color surface)
        {
            foreach (Control ctrl in controls)
            {
                if (ctrl is Button btn)
                {
                    btn.BackColor = primary;
                    btn.ForeColor = textOnPrimary;
                    btn.FlatStyle = FlatStyle.Flat;
                }
                else if (ctrl is Panel || ctrl is GroupBox)
                {
                    ctrl.BackColor = surface;
                }
                else if (ctrl is DataGridView grid)
                {
                    grid.BackgroundColor = surface;
                    grid.ColumnHeadersDefaultCellStyle.BackColor = primary;
                    grid.ColumnHeadersDefaultCellStyle.ForeColor = textOnPrimary;
                    grid.EnableHeadersVisualStyles = false;
                }

                if (ctrl.HasChildren)
                {
                    ApplyThemeToControls(ctrl.Controls, primary, textOnPrimary, textPrimary, surface);
                }
            }
        }

        private static string GetTokenValue(string tokenKey)
        {
            if (_activeTheme != null && _activeTheme.Tokens.TryGetValue(tokenKey, out string value))
                return value;
            return null;
        }

        private static Color GetColorFromToken(string tokenKey, Color fallback)
        {
            return ColorHelper.FromHex(GetTokenValue(tokenKey), fallback);
        }
    }
}