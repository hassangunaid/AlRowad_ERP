using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using AlRowad_ERP.Core;

namespace AlRowad_ERP.HelpForms
{
    public class UniversalSearchForm : Form
    {
        public TextBox txt_البحث_الفوري;
        public DataGridView grid_نتائج_البحث;
        public Button btn_تأكيد;
        public Button btn_إلغاء;

        private string _استعلام_الجلب;
        public string المعرف_المختار { get; private set; }

        public UniversalSearchForm(string عنوان_الشاشة, string استعلام_SQL)
        {
            this._استعلام_الجلب = استعلام_SQL;

            this.Text = عنوان_الشاشة;
            this.Size = new Size(650, 500);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.RightToLeft = RightToLeft.Yes;
            this.RightToLeftLayout = true;

            InitializeCustomComponents();
        }

        private void InitializeCustomComponents()
        {
            txt_البحث_الفوري = new TextBox { Location = new Point(15, 15), Size = new Size(600, 30), Font = new Font("Segoe UI", 11) };
            txt_البحث_الفوري.TextChanged += Txt_البحث_الفوري_TextChanged;

            grid_نتائج_البحث = new DataGridView
            {
                Location = new Point(15, 60),
                Size = new Size(600, 330),
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AllowUserToAddRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                RowHeadersVisible = false
            };
            grid_نتائج_البحث.CellDoubleClick += Grid_نتائج_البحث_CellDoubleClick;
            btn_تأكيد = new Button { Text = "موافق", Location = new Point(15, 405), Size = new Size(110, 40), Font = new Font("Segoe UI", 10, FontStyle.Bold), BackColor = Color.LightBlue };
            btn_تأكيد.Click += (s, e) => ConfirmSelection();

            btn_إلغاء = new Button { Text = "إلغاء", Location = new Point(135, 405), Size = new Size(110, 40), Font = new Font("Segoe UI", 10) };
            btn_إلغاء.Click += (s, e) => { this.DialogResult = DialogResult.Cancel; this.Close(); };

            this.Controls.Add(txt_البحث_الفوري);
            this.Controls.Add(grid_نتائج_البحث);
            this.Controls.Add(btn_تأكيد);
            this.Controls.Add(btn_إلغاء);

            this.Load += SearchForm_Load;
        }

        private void SearchForm_Load(object sender, EventArgs e)
        {
            ExecuteSearchQuery();
            txt_البحث_الفوري.Focus();
        }

        private void ExecuteSearchQuery()
        {
            try
            {
                // تأمين فتح وإغلاق الاتصال بشكل صارم داخل الدالة
                using (SqlCommand cmd = new SqlCommand(_استعلام_الجلب, DatabaseHelper.GetConnection()))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        grid_نتائج_البحث.DataSource = dt;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ أثناء جلب بيانات محرك البحث: " + ex.Message, "خطأ فني", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                }
        }

        private void Txt_البحث_الفوري_TextChanged(object sender, EventArgs e)
        {
            try
            {
                // الفلترة تتم داخلياً في الذاكرة على الـ DataView المربوطة ولا تذهب للسيرفر مجدداً
                if (grid_نتائج_البحث.DataSource is DataTable dt && dt.Columns.Count > 1)
                {
                    string اسم_العامود = dt.Columns[1].ColumnName;

                    // استبدال الرموز الخاصة لتفادي أخطاء الـ SQL Injection الداخلي في الفلترة
                    string safeText = txt_البحث_الفوري.Text.Replace("'", "''").Trim();

                    dt.DefaultView.RowFilter = string.Format("[{0}] LIKE '%{1}%'", اسم_العامود, safeText);
                }
            }
            catch (Exception ex)
            {
                // منع انهيار البرنامج في حالة حدوث خطأ أثناء الفلترة السريعة
                System.Diagnostics.Debug.WriteLine("خطأ أثناء الفلترة: " + ex.Message);
            }
        }

        private void Grid_نتائج_البحث_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) ConfirmSelection();
        }

        private void ConfirmSelection()
        {
            if (grid_نتائج_البحث.CurrentRow != null)
            {
                if (grid_نتائج_البحث.CurrentRow.Cells[0].Value != null)
                {
                    المعرف_المختار = grid_نتائج_البحث.CurrentRow.Cells[0].Value.ToString();
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
        }
    }
}