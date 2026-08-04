using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace AlRowad_ERP.Controls
{
    [ToolboxItem(true)]
    public partial class AlRowadToolBar : UserControl
    {
        public AlRowadToolBar()
        {
            InitializeComponent();
        }

        private void btn_New_Click(object sender, EventArgs e)
        {
            // سيتم توجيه هذا الحدث من الشاشة الأب BaseEntryForm
        }

        private void AlRowadToolBar_Load(object sender, EventArgs e)
        {
            // إعدادات إضافية عند تحميل الشريط إن لزم الأمر
        }
    }
}