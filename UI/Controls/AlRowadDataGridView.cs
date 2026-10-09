using System;
using System.Drawing;
using System.Windows.Forms;

namespace AlRowad_ERP.UI.Controls
{
    public class AlRowadDataGridView : DataGridView
    {
        public event EventHandler<DataGridViewCellEventArgs> OnF9Pressed;

        // تعريف القائمة المنسدلة (Context Menu)
        private ContextMenuStrip _gridContextMenu;
        private ToolStripMenuItem _deleteMenuItem;

        // أحداث مستقبلية لتكرار السجل أو عرض الرصيد (بحيث تبرمجها كل شاشة حسب حاجتها)
        public event EventHandler<DataGridViewRowEventArgs> OnDuplicateRecordRequested;
        public event EventHandler<DataGridViewRowEventArgs> OnShowBalanceRequested;

        public AlRowadDataGridView()
        {
            this.EnableHeadersVisualStyles = false;
            this.RowHeadersVisible = false;
            this.BackgroundColor = SystemColors.Control;
            this.BorderStyle = BorderStyle.Fixed3D;
            this.CellBorderStyle = DataGridViewCellBorderStyle.Sunken;
            this.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            this.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 10F);
            this.ColumnHeadersHeight = 34;
            this.RowTemplate.Height = 29;

            // بناء القائمة المنسدلة
            InitializeContextMenu();
        }

        // ==========================================
        // بناء وتجهيز قائمة الزر الأيمن (Context Menu)
        // ==========================================
        private void InitializeContextMenu()
        {
            _gridContextMenu = new ContextMenuStrip();
            _gridContextMenu.Font = new Font("Microsoft Sans Serif", 10F);

            // 1. عنصر الحذف (مدمج داخلياً لأنه سلوك عام للجريد)
            _deleteMenuItem = new ToolStripMenuItem("حذف السجل المحدد");
            _deleteMenuItem.Click += DeleteMenuItem_Click;

            // 2. عناصر مستقبلية (كمثال على التوسع)
            var duplicateMenuItem = new ToolStripMenuItem("تكرار السجل");
            duplicateMenuItem.Click += (s, e) => { if (this.CurrentRow != null) OnDuplicateRecordRequested?.Invoke(this, new DataGridViewRowEventArgs(this.CurrentRow)); };

            var balanceMenuItem = new ToolStripMenuItem("عرض رصيد الصنف/الحساب");
            balanceMenuItem.Click += (s, e) => { if (this.CurrentRow != null) OnShowBalanceRequested?.Invoke(this, new DataGridViewRowEventArgs(this.CurrentRow)); };

            _gridContextMenu.Items.Add(_deleteMenuItem);
            _gridContextMenu.Items.Add(new ToolStripSeparator());
            _gridContextMenu.Items.Add(duplicateMenuItem);
            _gridContextMenu.Items.Add(balanceMenuItem);

            // ربط القائمة بالجريد
            this.ContextMenuStrip = _gridContextMenu;

            // التحكم بحالة الأزرار قبل فتح القائمة (احتراماً لـ LockControls)
            _gridContextMenu.Opening += GridContextMenu_Opening;
        }

        private void GridContextMenu_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // إذا كان الجريد للقراءة فقط (وضع العرض)، يتم تعطيل زر الحذف آلياً
            _deleteMenuItem.Enabled = !this.ReadOnly && this.AllowUserToDeleteRows && this.CurrentRow != null && !this.CurrentRow.IsNewRow;
        }

        private void DeleteMenuItem_Click(object sender, EventArgs e)
        {
            if (this.CurrentRow != null && !this.CurrentRow.IsNewRow)
            {
                if (MessageBox.Show("هل أنت متأكد من حذف السجل المحدد؟", "تأكيد الحذف", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    this.Rows.Remove(this.CurrentRow);
                }
            }
        }

        // ==========================================
        // تحديد السطر آلياً عند النقر بالزر الأيمن
        // ==========================================
        protected override void OnCellMouseDown(DataGridViewCellMouseEventArgs e)
        {
            base.OnCellMouseDown(e);

            // إذا نقر المستخدم بالزر الأيمن على سطر بيانات وليس على الترويسة
            if (e.Button == MouseButtons.Right && e.RowIndex >= 0)
            {
                this.ClearSelection();
                this.Rows[e.RowIndex].Selected = true;

                // تعيين الخلية الحالية لتجنب أخطاء الحذف والتعديل
                this.CurrentCell = this.Rows[e.RowIndex].Cells[e.ColumnIndex >= 0 ? e.ColumnIndex : 0];
            }
        }

        // ==========================================
        // اعتراض المفاتيح (Enter للتحرك و F9 للبحث)
        // ==========================================
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter)
            {
                return this.ProcessTabKey(Keys.Tab);
            }

            if (keyData == Keys.F9)
            {
                if (this.CurrentCell != null)
                {
                    // إنهاء أي تعديل نصي معلق في الخلية قبل فتح شاشة البحث بقوة
                    if (this.IsCurrentCellInEditMode)
                    {
                        this.EndEdit();
                    }

                    OnF9Pressed?.Invoke(this, new DataGridViewCellEventArgs(this.CurrentCell.ColumnIndex, this.CurrentCell.RowIndex));
                }
                return true; // إيقاف السلوك الافتراضي
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        // أضف هذا التجاوز (Override) داخل كلاس AlRowadDataGridView
        protected override void OnColumnAdded(DataGridViewColumnEventArgs e)
        {
            base.OnColumnAdded(e);

            string colName = e.Column.Name.ToLower();

            // أتمتة التنسيق المحاسبي (N2) لأي عمود مالي أو كمي في النظام بأكمله
            if (colName.Contains("qty") || colName.Contains("quantity") ||
                colName.Contains("price") || colName.Contains("discount") ||
                colName.Contains("amount") || colName.Contains("total") ||
                colName.Contains("balance"))
            {
                e.Column.DefaultCellStyle.Format = "N2";
                e.Column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter; // أو MiddleLeft حسب رغبتك
            }
        }

        // ==========================================
        // الترقيم التلقائي لعمود التسلسل (Col_Serial)
        // ==========================================
        protected override void OnRowsAdded(DataGridViewRowsAddedEventArgs e)
        {
            base.OnRowsAdded(e);
            UpdateRowSerials();
        }

        protected override void OnRowsRemoved(DataGridViewRowsRemovedEventArgs e)
        {
            base.OnRowsRemoved(e);
            UpdateRowSerials();
        }

        private void UpdateRowSerials()
        {
            if (this.Columns.Contains("Col_Serial"))
            {
                for (int i = 0; i < this.Rows.Count; i++)
                {
                    if (!this.Rows[i].IsNewRow)
                        this.Rows[i].Cells["Col_Serial"].Value = (i + 1).ToString();
                }
            }
        }

        // ==========================================
        // الفلترة التلقائية للأعمدة الرقمية
        // ==========================================
        protected override void OnEditingControlShowing(DataGridViewEditingControlShowingEventArgs e)
        {
            base.OnEditingControlShowing(e);

            if (this.CurrentCell == null) return;

            string colName = this.Columns[this.CurrentCell.ColumnIndex].Name.ToLower();

            if (colName.Contains("qty") || colName.Contains("quantity") ||
                colName.Contains("price") || colName.Contains("discount") ||
                colName.Contains("amount") || colName.Contains("total"))
            {
                if (e.Control is TextBox txt)
                {
                    txt.KeyPress -= NumericCell_KeyPress;
                    txt.KeyPress += NumericCell_KeyPress;
                }
            }
            else
            {
                if (e.Control is TextBox txt)
                {
                    txt.KeyPress -= NumericCell_KeyPress;
                }
            }
        }

        private void NumericCell_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
                e.Handled = true;

            if (sender is TextBox txt && e.KeyChar == '.' && txt.Text.IndexOf('.') > -1)
                e.Handled = true;
        }

        protected override void OnCurrentCellDirtyStateChanged(EventArgs e)
        {
            base.OnCurrentCellDirtyStateChanged(e);
            if (this.IsCurrentCellDirty)
            {
                if (this.CurrentCell is DataGridViewCheckBoxCell || this.CurrentCell is DataGridViewComboBoxCell)
                {
                    this.CommitEdit(DataGridViewDataErrorContexts.Commit);
                }
            }
        }
    }
}   