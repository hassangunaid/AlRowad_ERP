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
using AlRowad_ERP.UI.Base;
using AlRowad_ERP.Core.Constants;

namespace AlRowad_ERP.Forms
{
    public partial class System_Shortcuts : BaseEntryForm
    {
        public System_Shortcuts()
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

        private void system_ShortcutsBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.system_ShortcutsBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.alRowad_ERPDataSet);

        }

        private void System_Shortcuts_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'alRowad_ERPDataSet.System_Shortcuts' table. You can move, or remove it, as needed.
            this.system_ShortcutsTableAdapter.Fill(this.alRowad_ERPDataSet.System_Shortcuts);

        }
    }
}
