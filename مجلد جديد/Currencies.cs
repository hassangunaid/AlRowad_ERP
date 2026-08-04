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
    public partial class Currencies : Form
    {
        public Currencies()
        {
            InitializeComponent();
        }

        private void currenciesBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.Validate();
                this.currenciesBindingSource.EndEdit();
                this.tableAdapterManager.UpdateAll(this.alRowad_ERPDataSet);
                MessageBox.Show("تم حفظ البيانات بنجاح!", "تأكيد", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ أثناء حفظ البيانات: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CurrenciesForm_Load(object sender, EventArgs e)
        {
            try
            {
                // TODO: This line of code loads data into the 'alRowad_ERPDataSet.Currencies' table. You can move, or remove it, as needed.
                this.currenciesTableAdapter.Fill(this.alRowad_ERPDataSet.Currencies);
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ أثناء تحميل البيانات: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
