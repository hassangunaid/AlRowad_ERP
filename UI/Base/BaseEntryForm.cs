using AlRowad_ERP.Controls;
using AlRowad_ERP.HelpForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using AlRowad_ERP.Core.Entities;
using AlRowad_ERP.Core.Helpers;
using AlRowad_ERP.Core.Constants;
using AlRowad_ERP.AlRowad_ERPDataSetTableAdapters;
using AlRowad_ERP.Core;



namespace AlRowad_ERP.UI.Base
{
    public enum DocumentStatus { Draft = 0, Pending = 1, Posted = 2 }
    public enum FormMode { New, Edit, View, RecordSelected }

    public class BaseEntryForm : BaseForm
    {

        protected AlRowadToolBar MainToolBar { get; private set; }

        public FormMode CurrentMode { get; private set; } = FormMode.View;
        public DocumentStatus CurrentDocumentStatus { get; set; } = DocumentStatus.Draft;
        public virtual string PrimaryIdFieldName { get; set; } = "";
        public virtual string MainTableName { get; set; } = "";
        private readonly UserEntity _formOwnerUser; protected Dictionary<string, string> DeleteDependencies { get; private set; } = new Dictionary<string, string>();

        // 🌟 الذاكرة المؤقتة لربط الأدوات (Caching)
        private Dictionary<string, Control> _boundControlsCache = new Dictionary<string, Control>();
        private Dictionary<string, object> _originalValues = new Dictionary<string, object>();

        public BaseEntryForm()
        {
            this.KeyPreview = true;
            this.Load += BaseEntryForm_Load;
            this.FormClosing += BaseEntryForm_FormClosing;
            _formOwnerUser = SystemConstants.CurrentUser;
        }

        protected UserEntity ActiveUser => _formOwnerUser;
        protected int CurrentUserId => _formOwnerUser?.UserID ?? 0;
        protected string CurrentUserName => _formOwnerUser?.Username ?? "System";
        protected string CurrentUserFullName => _formOwnerUser?.FullName ?? "مستخدم غير معروف";

        #region محرك الربط والذاكرة المؤقتة (Auto-Bind Engine)

        private void BuildControlsCache(Control parent)
        {
            foreach (Control ctrl in parent.Controls)
            {
                if (ctrl == MainToolBar) continue;

                if (ctrl is TextBox || ctrl is ComboBox || ctrl is CheckBox || ctrl is DateTimePicker)
                {
                    string dbColumn = GetDbColumnName(ctrl);
                    if (!string.IsNullOrWhiteSpace(dbColumn) && !_boundControlsCache.ContainsKey(dbColumn))
                    {
                        _boundControlsCache[dbColumn] = ctrl;
                    }
                }
                if (ctrl.HasChildren) BuildControlsCache(ctrl);
            }
        }

        protected virtual string GetDbColumnName(Control ctrl)
        {
            if (ctrl.Tag != null && !string.IsNullOrWhiteSpace(ctrl.Tag.ToString())) return ctrl.Tag.ToString().Trim();
            string name = ctrl.Name;
            if (name.StartsWith("txt_") || name.StartsWith("cmb_") || name.StartsWith("chk_") || name.StartsWith("dtp_"))
                return name.Substring(4);
            return name;
        }

        protected virtual void AutoBindRecord(DataRow row)
        {
            foreach (DataColumn column in row.Table.Columns)
            {
                if (_boundControlsCache.TryGetValue(column.ColumnName, out Control ctrl) && row[column] != DBNull.Value)
                {
                    if (ctrl is TextBox txt) txt.Text = row[column].ToString();
                    else if (ctrl is CheckBox chk) chk.Checked = Convert.ToBoolean(row[column]);
                    else if (ctrl is ComboBox cmb) cmb.SelectedValue = row[column];
                    else if (ctrl is DateTimePicker dtp) dtp.Value = Convert.ToDateTime(row[column]);
                }
            }

            if (_boundControlsCache.TryGetValue(SystemConstants.CreatedBy, out Control createdByCtrl))
                createdByCtrl.Text = FormatUserInfo(row[SystemConstants.CreatedBy], row.Table.Columns.Contains("CreatedByName") ? row["CreatedByName"] : null);
            if (_boundControlsCache.TryGetValue(SystemConstants.UpdatedBy, out Control updatedByCtrl))
                updatedByCtrl.Text = FormatUserInfo(row[SystemConstants.UpdatedBy], row.Table.Columns.Contains("UpdatedByName") ? row["UpdatedByName"] : null);
        }

        protected virtual string FormatUserInfo(object userIdObj, object userNameObj)
        {
            if (userIdObj == null || userIdObj == DBNull.Value || Convert.ToInt32(userIdObj) == 0) return "";
            string name = (userNameObj != null && userNameObj != DBNull.Value) ? userNameObj.ToString() : "مجهول";
            return $"{name} || {userIdObj}";
        }

        protected virtual async Task<DataTable> LoadMainRecordDataAsync(string recordId)
        {
            if (string.IsNullOrWhiteSpace(MainTableName) || string.IsNullOrWhiteSpace(PrimaryIdFieldName)) return null;

            var controls = this.Controls.Find(PrimaryIdFieldName, true);
            if (controls.Length == 0) return null;

            string dbColumnName = GetDbColumnName(controls[0]);
            string query = $"SELECT * FROM {MainTableName} WHERE {dbColumnName} = @ID";
            SqlParameter[] p = { new SqlParameter("@ID", recordId) };

            return await DatabaseHelper.GetTableAsync(query, p);
        }

        #endregion

        #region محرك الفحص والحذف الديناميكي

        protected virtual async Task<bool> ValidateDependenciesBeforeDeleteAsync()
        {
            if (DeleteDependencies.Count == 0) return true;

            var controls = this.Controls.Find(PrimaryIdFieldName, true);
            if (controls.Length == 0 || !(controls[0] is TextBox idField) || string.IsNullOrWhiteSpace(idField.Text)) return false;

            foreach (var dependency in DeleteDependencies)
            {
                if (await IsRecordUsedInTableAsync(dependency.Key, dependency.Value, idField.Text))
                {
                    MessageBox.Show($"منع أمني: لا يمكن حذف هذا السجل لارتباطه بحركات في جدول ({dependency.Key}).", "ارتباط مرجعي", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return false;
                }
            }
            return true;
        }


        protected async Task<bool> IsRecordUsedInTableAsync(string tableName, string columnName, object idValue)
        {
            string query = $"SELECT COUNT(1) FROM {tableName} WHERE {columnName} = @ID";
            SqlParameter[] p = { new SqlParameter("@ID", idValue) };
            object result = await DatabaseHelper.ExecuteScalarAsync(query, p);
            return (result != null ? Convert.ToInt32(result) : 0) > 0;
        }

        #endregion

        #region العقود السيادية (Async Contracts)

        protected virtual async Task<bool> ExecuteSaveToDatabaseAsync(SqlTransaction transaction)
        {
            return await Task.FromResult(false);
        }
        protected virtual async Task<bool> ExecuteDeleteFromDatabaseAsync(SqlTransaction transaction)
        {
            // الدالة الافتراضية تعيد false. 
            // يجب على الشاشة الابنة (مثل شاشة الموردين أو الثيمات) تجاوز هذه الدالة 
            // وإرسال أمر الحذف إلى طبقة الـ Repository الخاصة بها.
            return await Task.FromResult(false);
        }
        protected virtual bool ValidateFields(Control parent) => true;
        protected virtual string GetNextId() { return string.Empty; }
        protected virtual void RefreshData() { }
        protected virtual void OnF9Pressed() { }
        protected virtual void OnF8Pressed() { }
        protected virtual void OnF7Pressed() { }
        protected virtual void OnF3Pressed() { }
        protected virtual void OnF2Pressed() { }
        protected virtual void OnAddFrom() { }

        #endregion

        #region العمليات الأساسية مع دعم التزامن (CRUD Operations - Async)

        public virtual async void OnSave()
        {
            if (!ValidateFields(this)) return;

            MainToolBar.Enabled = false;

            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                await conn.OpenAsync();
                using (SqlTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        bool isSaved = await ExecuteSaveToDatabaseAsync(transaction);

                        if (isSaved)
                        {
                            transaction.Commit();
                            ChangeFormMode(FormMode.RecordSelected);
                            MessageBox.Show("تم حفظ البيانات بنجاح.", "نظام الرواد ERP", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            RefreshData();
                        }
                        else
                        {
                            transaction.Rollback();
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction?.Rollback();
                        MessageBox.Show($"حدث خطأ أثناء الحفظ:\n{ex.Message}", "خطأ برمجي - نظام الرواد", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        MainToolBar.Enabled = true;
                    }
                }
            }
        }

        public virtual async void OnDelete()
        {
            if (this.CurrentDocumentStatus == DocumentStatus.Posted && !UserSession.IsSuperAdmin)
            {
                MessageBox.Show("لا يمكن حذف مستند مُرحل إلى الحسابات. يرجى إلغاء الترحيل أولاً.", "حظر رقابي", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            if (!await ValidateDependenciesBeforeDeleteAsync()) return;

            if (MessageBox.Show("هل أنت متأكد من حذف هذا السجل؟", "تأكيد الحذف", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;

            MainToolBar.Enabled = false;

            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                await conn.OpenAsync();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        if (await ExecuteDeleteFromDatabaseAsync(trans))
                        {
                            trans.Commit();
                            MessageBox.Show("تم الحذف بنجاح.", "نظام الرواد", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            OnNew();
                        }
                        else trans.Rollback();
                    }
                    catch (Exception ex)
                    {
                        trans?.Rollback();
                        MessageBox.Show("فشل الحذف! السبب: \n" + ex.Message, "حماية النظام", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally { MainToolBar.Enabled = true; }
                }
            }
        }

        #endregion

        #region إدارة الحالة والواجهة

        private void BaseEntryForm_Load(object sender, EventArgs e)
        {
                MainToolBar = this.Controls.OfType<AlRowadToolBar>().FirstOrDefault();
            if (MainToolBar != null)
            {
                MainToolBar.btn_New.Click += (s, ev) => OnNew();
                MainToolBar.btn_Edit.Click += (s, ev) => OnEdit();
                MainToolBar.btn_Save.Click += (s, ev) => OnSave();
                MainToolBar.btn_Cancel.Click += (s, ev) => OnCancel();
                MainToolBar.btn_Delete.Click += (s, ev) => OnDelete();
                MainToolBar.btn_Search.Click += (s, ev) => OnSearch();
                MainToolBar.btn_Close.Click += (s, ev) => OnClose();
                MainToolBar.btn_AddFrom.Click += (s, ev) => OnAddFrom();
            }

            BuildControlsCache(this);
            ApplySelectAllOnFocus(this.Controls);
            ChangeFormMode(FormMode.View);
        }

        public void ChangeFormMode(FormMode mode)
        {
            CurrentMode = mode;
            bool isFieldsReadOnly = (mode == FormMode.View || mode == FormMode.RecordSelected);

            if (MainToolBar != null)
            {
                switch (mode)
                {
                    case FormMode.View:
                        MainToolBar.btn_New.Enabled = true;
                        MainToolBar.btn_Search.Enabled = true;
                        MainToolBar.btn_Edit.Enabled = false;
                        MainToolBar.btn_Delete.Enabled = false;
                        MainToolBar.btn_Save.Enabled = false;
                        MainToolBar.btn_Cancel.Enabled = false;
                        MainToolBar.btn_Print.Enabled = false;
                        MainToolBar.btn_AddFrom.Enabled = false;
                        break;

                    case FormMode.RecordSelected:
                        MainToolBar.btn_New.Enabled = true;
                        MainToolBar.btn_Search.Enabled = true;
                        MainToolBar.btn_Edit.Enabled = true;
                        MainToolBar.btn_Delete.Enabled = true;
                        MainToolBar.btn_Save.Enabled = false;
                        MainToolBar.btn_Cancel.Enabled = false;
                        MainToolBar.btn_Print.Enabled = true;
                        MainToolBar.btn_AddFrom.Enabled = true;
                        break;

                    case FormMode.New:
                    case FormMode.Edit:
                        MainToolBar.btn_New.Enabled = false;
                        MainToolBar.btn_Search.Enabled = false;
                        MainToolBar.btn_Edit.Enabled = false;
                        MainToolBar.btn_Delete.Enabled = false;
                        MainToolBar.btn_Save.Enabled = true;
                        MainToolBar.btn_Cancel.Enabled = true;
                        MainToolBar.btn_Print.Enabled = false;
                        MainToolBar.btn_AddFrom.Enabled = false;
                        break;
                }
            }

            LockControls(this, isFieldsReadOnly);
        }

        protected virtual void LockControls(Control parent, bool isReadOnly)
        {
            bool finalReadOnlyStatus = isReadOnly;
            if (this.CurrentDocumentStatus == DocumentStatus.Posted && !UserSession.IsSuperAdmin) finalReadOnlyStatus = true;

            foreach (Control c in parent.Controls)
            {
                if (c == MainToolBar) continue;
                if (c is TextBox txt)
                {
                    txt.ReadOnly = finalReadOnlyStatus;
                    string name = txt.Name.ToLower();
                    if (name.Contains("createdby") || name.Contains("updatedby") || name.Contains("createdat") || name.Contains("updatedat")) txt.ReadOnly = true;
                }
                else if (c is ComboBox || c is CheckBox || c is DateTimePicker || c is DataGridView) c.Enabled = !finalReadOnlyStatus;

                if (c.HasChildren) LockControls(c, finalReadOnlyStatus);
            }
        }

        public virtual void OnNew()
        {
            ClearForm(this);
            if (!string.IsNullOrEmpty(PrimaryIdFieldName))
            {
                var controls = this.Controls.Find(PrimaryIdFieldName, true);
                if (controls.Length > 0 && controls[0] is TextBox idField) idField.Text = GetNextId();
            }
            CurrentDocumentStatus = DocumentStatus.Draft;
            ChangeFormMode(FormMode.New);
        }

        public virtual void OnEdit()
        {
            CaptureOriginalValues(this);
            ChangeFormMode(FormMode.Edit);
        }

        public virtual void OnCancel()
        {
            DialogResult result = MessageBox.Show("هل أنت متأكد من إلغاء كافة التغييرات الحالية والتراجع؟", "تأكيد التراجع", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (result == DialogResult.No) return;

            if (CurrentMode == FormMode.New)
            {
                ClearForm(this);
                ChangeFormMode(FormMode.View);
            }
            else if (CurrentMode == FormMode.Edit)
            {
                RestoreOriginalValues(this);
                ChangeFormMode(FormMode.RecordSelected);
            }
        }

        public virtual void OnSearch()
        {
            try
            {
                using (UniversalSearchForm searchForm = new UniversalSearchForm("البحث في النظام", ""))
                {
                    if (searchForm.ShowDialog(this) == DialogResult.OK)
                    {
                        ChangeFormMode(FormMode.RecordSelected);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث خطأ أثناء فتح نافذة البحث: " + ex.Message, "خطأ فني", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public virtual void OnClose() { this.Close(); }

        private void BaseEntryForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (CurrentMode == FormMode.New || CurrentMode == FormMode.Edit)
            {
                MessageBox.Show("تنبيه أمني: لا يمكنك إغلاق الشاشة وهي في وضع الإدخال أو التعديل.", "حظر الخروج", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                e.Cancel = true;
            }
        }

        protected bool IsPeriodClosed(DateTime transactionDate)
        {
            string query = @"SELECT COUNT(1) FROM Financial_Periods WHERE Is_Closed = 1 AND @TxDate >= Start_Date AND @TxDate <= End_Date";
            SqlParameter[] p = { new SqlParameter("@TxDate", SqlDbType.DateTime) { Value = transactionDate.Date } };
            object result = DatabaseHelper.ExecuteScalar(query, p);
            return (result != null ? Convert.ToInt32(result) : 0) > 0;
        }

        protected bool ValidateFinancialPeriodBeforeSave(DateTime transactionDate)
        {
            if (IsPeriodClosed(transactionDate))
            {
                MessageBox.Show("خطأ أمني ومحاسبي: لا يمكن إجراء حفظ لحركة تقع ضمن 'فترة محاسبية مقفلة'.", "إقفال المحاسبة", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }
            return true;
        }

        protected void ClearForm(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                if (c is TextBox t) t.Clear();
                else if (c is ComboBox cb) cb.SelectedIndex = -1;
                else if (c is CheckBox chk) chk.Checked = false;
                if (c.HasChildren) ClearForm(c);
            }
        }

        protected virtual void CaptureOriginalValues(Control parent)
        {
            if (parent == this) _originalValues.Clear();
            foreach (Control c in parent.Controls)
            {
                if (c == MainToolBar) continue;
                if (c is TextBox txt) _originalValues[c.Name] = txt.Text;
                else if (c is CheckBox chk) _originalValues[c.Name] = chk.Checked;
                else if (c is ComboBox cmb) _originalValues[c.Name] = cmb.SelectedIndex;
                else if (c is DateTimePicker dtp) _originalValues[c.Name] = dtp.Value;
                if (c.HasChildren) CaptureOriginalValues(c);
            }
        }

        protected virtual void RestoreOriginalValues(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                if (c == MainToolBar) continue;
                if (_originalValues.ContainsKey(c.Name))
                {
                    if (c is TextBox txt) txt.Text = _originalValues[c.Name]?.ToString();
                    else if (c is CheckBox chk) chk.Checked = Convert.ToBoolean(_originalValues[c.Name]);
                    else if (c is ComboBox cmb) cmb.SelectedIndex = Convert.ToInt32(_originalValues[c.Name]);
                    else if (c is DateTimePicker dtp) dtp.Value = Convert.ToDateTime(_originalValues[c.Name]);
                }
                if (c.HasChildren) RestoreOriginalValues(c);
            }
        }

        private void ApplySelectAllOnFocus(Control.ControlCollection controls)
        {
            foreach (Control ctrl in controls)
            {
                if (ctrl is TextBox txt)
                {
                    txt.Enter += (sender, e) => { txt.BeginInvoke(new Action(() => txt.SelectAll())); };
                    txt.MouseClick += (sender, e) => { if (txt.SelectionLength == 0) txt.SelectAll(); };
                }
                if (ctrl.HasChildren) ApplySelectAllOnFocus(ctrl.Controls);
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.F10 && MainToolBar != null && MainToolBar.btn_Save.Enabled) { OnSave(); return true; }
            if (keyData == Keys.F6 && MainToolBar != null && MainToolBar.btn_New.Enabled) { OnNew(); return true; }
            if (keyData == Keys.F5 && MainToolBar != null && MainToolBar.btn_Edit.Enabled) { OnEdit(); return true; }
            if (keyData == Keys.F4 && CurrentMode != FormMode.View && MainToolBar != null && MainToolBar.btn_Cancel.Enabled) { OnCancel(); return true; }
            if (keyData == (Keys.Control | Keys.D) && CurrentMode == FormMode.RecordSelected && MainToolBar != null && MainToolBar.btn_Delete.Enabled) { OnDelete(); return true; }
            if (keyData == Keys.F9)
            {
                if ((CurrentMode == FormMode.View || CurrentMode == FormMode.RecordSelected) && MainToolBar != null && MainToolBar.btn_Search.Enabled) OnSearch();
                else if (CurrentMode != FormMode.View && CurrentMode != FormMode.RecordSelected) OnF9Pressed();
                return true;
            }
            if (CurrentMode == FormMode.New || CurrentMode == FormMode.Edit)
            {
                if (keyData == Keys.F8) { OnF8Pressed(); return true; }
                if (keyData == Keys.F7) { OnF7Pressed(); return true; }
                if (keyData == Keys.F3) { OnF3Pressed(); return true; }
                if (keyData == Keys.F2) { OnF2Pressed(); return true; }
            }
            if (keyData == Keys.Enter)
            {
                Control activeControl = this.ActiveControl;
                if (activeControl != null && !(activeControl is Button) && !(activeControl is TextBox txt && txt.Multiline) && !(activeControl is DataGridView || activeControl.Parent is DataGridView))
                {
                    SendKeys.Send("{TAB}");
                    return true;
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        #endregion
        // 2. دالة تسجيل الأخطاء المركزية
        protected void LogError(Exception ex)
        {
            // تطبيقاً للدستور، يتم معالجة الأخطاء هنا لعدم تكرار الكود
            MessageBox.Show(ex.Message, "خطأ في النظام", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }


        // 3. الدالة السيادية للحفظ المتزامن (يجب أن تكون virtual ليتم تجاوزها)
        protected virtual Task<bool> ExecuteSaveToDatabaseAsync()
        {
            // الدالة الافتراضية تعيد false، والشاشات الابنة (مثل شاشة الثيمات) ستقوم بكتابة المنطق الخاص بها
            return Task.FromResult(false);
        }

        #region محرك قائمة الشبكة السياقية
        protected ContextMenuStrip gridContextMenu;
        private DataGridView _activeGridForMenu;
        private int _clickedRowIndex = -1;

        protected void AttachContextMenuToGrid(DataGridView grid)
        {
            if (gridContextMenu == null)
            {
                gridContextMenu = new ContextMenuStrip { RightToLeft = RightToLeft.Yes };
                var deleteOption = new ToolStripMenuItem("حذف السطر الحالي");
                deleteOption.Click += DeleteRowOption_Click;
                gridContextMenu.Items.Add(deleteOption);
            }

            grid.CellMouseDown += (sender, e) =>
            {
                if (e.Button == MouseButtons.Right && e.RowIndex >= 0 && e.ColumnIndex >= 0)
                {
                    var clickedGrid = sender as DataGridView;
                    if (this.CurrentMode == FormMode.View || clickedGrid.Rows[e.RowIndex].IsNewRow) return;

                    clickedGrid.ClearSelection();
                    clickedGrid.Rows[e.RowIndex].Selected = true;
                    clickedGrid.CurrentCell = clickedGrid.Rows[e.RowIndex].Cells[e.ColumnIndex];

                    _activeGridForMenu = clickedGrid;
                    _clickedRowIndex = e.RowIndex;
                    gridContextMenu.Show(Cursor.Position);
                }
            };
        }

        private void DeleteRowOption_Click(object sender, EventArgs e)
        {
            if (this.CurrentMode == FormMode.View || _activeGridForMenu == null || _clickedRowIndex < 0) return;
            if (MessageBox.Show("هل أنت متأكد من حذف هذا السطر؟", "تأكيد الحذف", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                _activeGridForMenu.Rows.RemoveAt(_clickedRowIndex);
                OnGridRowDeleted(_activeGridForMenu);
            }
        }

        protected virtual void OnGridRowDeleted(DataGridView grid) { }
        #endregion

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.BackColor = System.Drawing.Color.DarkBlue;
            this.ClientSize = new System.Drawing.Size(1082, 396);
            this.Name = "BaseEntryForm";
            this.ResumeLayout(false);
        }
    }
}