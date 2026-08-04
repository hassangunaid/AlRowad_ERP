using System;
using System.Data;
using System.Drawing;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        public string الاسم_المختار { get; private set; }
        public string العملة_المختارة { get; private set; }

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

            this.Load += async (s, e) => await SearchForm_LoadAsync();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter)
            {
                if (grid_نتائج_البحث.CurrentRow != null)
                {
                    ConfirmSelection();
                    return true;
                }
            }
            else if (keyData == Keys.Down && this.ActiveControl == txt_البحث_الفوري)
            {
                if (grid_نتائج_البحث.Rows.Count > 0)
                {
                    grid_نتائج_البحث.Focus();
                    return true;
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private async Task SearchForm_LoadAsync()
        {
            await ExecuteSearchQueryAsync();
            txt_البحث_الفوري.Select();
        }

        private void Grid_نتائج_البحث_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            Point clientPoint = grid_نتائج_البحث.PointToClient(Cursor.Position);
            DataGridView.HitTestInfo hit = grid_نتائج_البحث.HitTest(clientPoint.X, clientPoint.Y);

            if (hit.Type != DataGridViewHitTestType.Cell || hit.RowIndex != e.RowIndex || hit.ColumnIndex != e.ColumnIndex)
                return;

            if (grid_نتائج_البحث.CurrentRow == null || grid_نتائج_البحث.CurrentRow.Index != e.RowIndex)
                return;

            ConfirmSelection();
        }

        private async Task ExecuteSearchQueryAsync()
        {
            try
            {
                DataTable dt = await DatabaseHelper.GetTableAsync(_استعلام_الجلب);
                grid_نتائج_البحث.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ أثناء جلب بيانات محرك البحث: " + ex.Message, "خطأ فني", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 🚀 الفلترة الديناميكية المتقدمة (فكرتك الرائعة)
        private void Txt_البحث_الفوري_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (grid_نتائج_البحث.DataSource is DataTable dt && dt.Columns.Count > 0)
                {
                    string safeText = txt_البحث_الفوري.Text.Replace("'", "''").Trim();

                    // في حال تفريغ مربع البحث، استعادة كافة البيانات
                    if (string.IsNullOrEmpty(safeText))
                    {
                        dt.DefaultView.RowFilter = string.Empty;
                        return;
                    }

                    // بناء الفلتر الديناميكي للمرور على كافة الأعمدة
                    List<string> filters = new List<string>();
                    foreach (DataColumn col in dt.Columns)
                    {
                        // 💡 استخدام CONVERT للسماح بالبحث داخل الحقول الرقمية والتاريخية كأنها نصوص
                        filters.Add($"CONVERT([{col.ColumnName}], 'System.String') LIKE '%{safeText}%'");
                    }

                    // دمج جميع الفلاتر بـ OR
                    dt.DefaultView.RowFilter = string.Join(" OR ", filters);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("خطأ أثناء الفلترة الشاملة: " + ex.Message);
            }
        }

        private void ConfirmSelection()
        {
            if (grid_نتائج_البحث.CurrentRow != null)
            {
                if (grid_نتائج_البحث.CurrentRow.Cells[0].Value != null)
                    this.المعرف_المختار = grid_نتائج_البحث.CurrentRow.Cells[0].Value.ToString();

                if (grid_نتائج_البحث.CurrentRow.Cells.Count > 1 && grid_نتائج_البحث.CurrentRow.Cells[1].Value != null)
                    this.الاسم_المختار = grid_نتائج_البحث.CurrentRow.Cells[1].Value.ToString();

                if (grid_نتائج_البحث.CurrentRow.Cells.Count > 2 && grid_نتائج_البحث.CurrentRow.Cells[2].Value != null)
                    this.العملة_المختارة = grid_نتائج_البحث.CurrentRow.Cells[2].Value.ToString();

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }
}