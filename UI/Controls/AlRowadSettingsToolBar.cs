using System.Drawing;
using System.Windows.Forms;
using AlRowad_ERP.UI.Base;

namespace AlRowad_ERP.Controls
{
    public partial class AlRowadSettingsToolBar : UserControl
    {
        public AlRowadSettingsToolBar()
        {
            InitializeComponent();
            ApplyCustomStyling();
        }

        private void ApplyCustomStyling()
        {
            btn_Edit.BackColor = Color.FromArgb(41, 128, 185);
            btn_Edit.ForeColor = Color.White;

            btn_Save.BackColor = Color.FromArgb(39, 174, 96);
            btn_Save.ForeColor = Color.White;

            btn_Cancel.BackColor = Color.FromArgb(149, 165, 166);
            btn_Cancel.ForeColor = Color.White;

            btn_Close.BackColor = Color.FromArgb(192, 57, 43);
            btn_Close.ForeColor = Color.White;
        }

        /// <summary>
        /// ضبط أزرار الشريط حصرياً بناءً على حالة الشاشة المعتمدة في الدستور
        /// </summary>
        public void UpdateButtonsState(FormMode mode)
        {
            switch (mode)
            {
                case FormMode.View:
                case FormMode.RecordSelected:
                    btn_Edit.Enabled = true;
                    btn_Save.Enabled = false;
                    btn_Cancel.Enabled = false;
                    btn_Close.Enabled = true;
                    break;

                case FormMode.New:
                case FormMode.Edit:
                    btn_Edit.Enabled = false;
                    btn_Save.Enabled = true;
                    btn_Cancel.Enabled = true;
                    btn_Close.Enabled = false; // منع الخروج أثناء التعديل لمنع فقدان البيانات
                    break;
            }
        }
    }
}