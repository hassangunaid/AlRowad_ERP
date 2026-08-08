using System;
using System.Windows.Forms;
using AlRowad_ERP.UI.Base;
using AlRowad_ERP.Controls;

namespace AlRowad_ERP.UI.Helpers
{
    public static class ShortcutManager
    {
        /// <summary>
        /// الدالة المركزية لمعالجة كافة اختصارات النظام وفقاً لحالة الشاشة
        /// </summary>
        public static bool HandleCommandKey(BaseEntryForm form, AlRowadToolBar toolBar, ref Message msg, Keys keyData)
        {
            // 1. العمليات السيادية الأساسية
            if (keyData == Keys.F10 && toolBar != null && toolBar.btn_Save.Enabled) { form.OnSave(); return true; }
            if (keyData == Keys.F6 && toolBar != null && toolBar.btn_New.Enabled) { form.OnNew(); return true; }
            if (keyData == Keys.F5 && toolBar != null && toolBar.btn_Edit.Enabled) { form.OnEdit(); return true; }

            if (keyData == Keys.F4 && form.CurrentMode != FormMode.View && toolBar != null && toolBar.btn_Cancel.Enabled)
            {
                form.OnCancel();
                return true;
            }

            if (keyData == (Keys.Control | Keys.D) && form.CurrentMode == FormMode.RecordSelected && toolBar != null && toolBar.btn_Delete.Enabled)
            {
                form.OnDelete();
                return true;
            }

            // 2. عمليات البحث والاستدعاء (F9)
            if (keyData == Keys.F9)
            {
                if ((form.CurrentMode == FormMode.View || form.CurrentMode == FormMode.RecordSelected) && toolBar != null && toolBar.btn_Search.Enabled)
                {
                    form.OnSearch();
                }
                else if (form.CurrentMode != FormMode.View && form.CurrentMode != FormMode.RecordSelected)
                {
                    form.OnF9Pressed(); // استدعاء الدالة الافتراضية ليتم تجاوزها في الشاشات الفرعية
                }
                return true;
            }

            // 3. اختصارات خاصة بوضعي الإضافة والتعديل فقط
            if (form.CurrentMode == FormMode.New || form.CurrentMode == FormMode.Edit)
            {
                if (keyData == Keys.F8) { form.OnF8Pressed(); return true; }
                if (keyData == Keys.F7) { form.OnF7Pressed(); return true; }
                if (keyData == Keys.F3) { form.OnF3Pressed(); return true; }
                if (keyData == Keys.F2) { form.OnF2Pressed(); return true; }
            }

            // 4. تحويل زر Enter إلى Tab للتنقل السلس (باستثناء الأزرار والحقول متعددة الأسطر والشبكات)
            if (keyData == Keys.Enter)
            {
                Control activeControl = form.ActiveControl;
                if (activeControl != null &&
                    !(activeControl is Button) &&
                    !(activeControl is TextBox txt && txt.Multiline) &&
                    !(activeControl is DataGridView || activeControl.Parent is DataGridView))
                {
                    SendKeys.Send("{TAB}");
                    return true;
                }
            }

            // لم يتم التقاط أي اختصار معروف
            return false;
        }
    }
}