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
    public partial class Stores : BaseEntryForm
    {
        public Stores()
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

        private void storesBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.storesBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.alRowad_ERPDataSet);

        }

        private void Stores_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'alRowad_ERPDataSet.Item_Balances' table. You can move, or remove it, as needed.
            this.item_BalancesTableAdapter.Fill(this.alRowad_ERPDataSet.Item_Balances);
            // TODO: This line of code loads data into the 'alRowad_ERPDataSet.Stores' table. You can move, or remove it, as needed.
            this.storesTableAdapter.Fill(this.alRowad_ERPDataSet.Stores);

        }
    }
}
