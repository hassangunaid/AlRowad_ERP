using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AlRowad_ERP.Core;

namespace AlRowad_ERP.Forms
{
    public partial class Journal_Header : BaseEntryForm
    {
        public Journal_Header()
        {
            InitializeComponent();
        }

        private void Customers_Load(object sender, EventArgs e)
        {
            try
            {
                // تحميل البيانات من قاعدة البيانات
                // TODO: إضافة كود تحميل البيانات هنا
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ أثناء تحميل البيانات: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
