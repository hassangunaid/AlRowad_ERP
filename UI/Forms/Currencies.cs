using AlRowad_ERP.Core;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks; // 🌟 إضافة دعم التزامن
using System.Windows.Forms;
using AlRowad_ERP.UI.Base;
using AlRowad_ERP.Core.Constants;



namespace AlRowad_ERP.Forms
{
    public partial class Currencies : BaseEntryForm
    {
        private byte[] _currentRowVersion;

        public Currencies()
        {
            InitializeComponent();
            PrimaryIdFieldName = "cur_IDTextBox";
            this.MainTableName = "Currencies";

            // 🌟 الفحص الآلي للارتباطات المرجعية
            this.DeleteDependencies.Add("Purchase_Invoices", "Vendor_ID");
            this.DeleteDependencies.Add("Purchase_Orders", "Vendor_ID");

            this.Load += CurrenciesForm_Load;

            // ربط حدث التغيير للعملة المحلية لتطبيق الإقفال اللحظي
            if (chk_IsLocalCurrency != null)
                chk_IsLocalCurrency.CheckedChanged += async (s, e) => await Chk_IsLocalCurrency_CheckedChangedAsync(s, e);

            // ربط حدث العملة الافتراضية لتطبيق الحماية اللحظية
            if (chk_IsBaseCurrency != null)
                chk_IsBaseCurrency.CheckedChanged += async (s, e) => await Chk_IsBaseCurrency_CheckedChangedAsync(s, e);

            if (min_RateTextBox != null) min_RateTextBox.Leave += MinMaxRate_Leave;
            if (max_RateTextBox != null) max_RateTextBox.Leave += MinMaxRate_Leave;
        }

        private void CurrenciesForm_Load(object sender, EventArgs e)
        {
            try
            {
                // 1. إعداد الجريد ليعمل كعارض لتاريخ العملة المستعرضة فقط (History View)
                if (currenciesDataGridView != null)
                {
                    currenciesDataGridView.AutoGenerateColumns = false;
                    currenciesDataGridView.ReadOnly = true;
                    currenciesDataGridView.AllowUserToAddRows = false;
                    currenciesDataGridView.AllowUserToDeleteRows = false;

                    // ربط الأعمدة البرمجية بما يتوافق مع استعلام التاريخ
                    if (currenciesDataGridView.Columns.Contains("Cur_ID")) currenciesDataGridView.Columns["Cur_ID"].DataPropertyName = "Cur_ID";
                    if (currenciesDataGridView.Columns.Contains("Cur_Name")) currenciesDataGridView.Columns["Cur_Name"].DataPropertyName = "Cur_Name";
                    if (currenciesDataGridView.Columns.Contains("Exchange_Rate")) currenciesDataGridView.Columns["Exchange_Rate"].DataPropertyName = "Exchange_Rate";
                    if (currenciesDataGridView.Columns.Contains("Min_Exchange_Rate")) currenciesDataGridView.Columns["Min_Exchange_Rate"].DataPropertyName = "Min_Exchange_Rate";
                    if (currenciesDataGridView.Columns.Contains("Max_Exchange_Rate")) currenciesDataGridView.Columns["Max_Exchange_Rate"].DataPropertyName = "Max_Exchange_Rate";
                    if (currenciesDataGridView.Columns.Contains("Change_Date")) currenciesDataGridView.Columns["Change_Date"].DataPropertyName = "Change_Date";
                    if (currenciesDataGridView.Columns.Contains("Modified_By_Col"))
                        currenciesDataGridView.Columns["Modified_By_Col"].DataPropertyName = "ModifiedBy";

                }

                // النظام يفتح دائماً في وضع الاستعراض المحمي
                ChangeFormMode(FormMode.View);
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ أثناء تهيئة الشاشة: " + ex.Message, "نظام الرواد - خطأ فني", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #region دوال التدقيق والتحديث (Audit Trail)
        private string FormatUserInfo(object userIdObj, object userNameObj)
        {
            if (userIdObj == null || userIdObj == DBNull.Value || Convert.ToInt32(userIdObj) == 0) return "";
            string name = (userNameObj != null && userNameObj != DBNull.Value) ? userNameObj.ToString() : "مجهول";
            return $"{name} || {userIdObj}";
        }

        // 🚀 استدعاء أوتوماتيكي لامتزامن من BaseEntryForm فور نجاح الحفظ لتحديث الشاشة
        protected override async void RefreshData()
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(cur_IDTextBox.Text))
                {
                    await LoadCurrencyDataAsync(cur_IDTextBox.Text.Trim());
                }
            }
            catch { }
        }
        #endregion

        #region محركات جلب وعرض البيانات المخصصة (Master-Detail)

        // 🌟 الجلب اللامتزامن لبيانات العملة
        private async Task LoadCurrencyDataAsync(string curId)
        {
            string sqlMaster = $@"
                SELECT C.*, 
                       ISNULL(UC.Full_Name, UC.Username) AS CreatedByName,
                       ISNULL(UU.Full_Name, UU.Username) AS UpdatedByName
                FROM Currencies C
                LEFT JOIN Users UC ON C.{SystemConstants.AuditFields.CreatedBy} = UC.User_ID
                LEFT JOIN Users UU ON C.{SystemConstants.AuditFields.UpdatedBy} = UU.User_ID
                WHERE C.Cur_ID = @CurID";

            DataTable dtMaster = await DatabaseHelper.GetTableAsync(sqlMaster, new[] { new SqlParameter("@CurID", curId) });

            if (dtMaster.Rows.Count > 0)
            {
                DataRow row = dtMaster.Rows[0];

                if (dtMaster.Columns.Contains("RowVersion") && row["RowVersion"] != DBNull.Value)
                {
                    _currentRowVersion = (byte[])row["RowVersion"];
                }

                if (cur_IDTextBox != null) cur_IDTextBox.Text = row["Cur_ID"].ToString();
                if (cur_NameTextBox != null) cur_NameTextBox.Text = row["Cur_Name"].ToString();

                if (cur_SymbolTextBox != null && dtMaster.Columns.Contains("Cur_Symbol"))
                    cur_SymbolTextBox.Text = row["Cur_Symbol"].ToString();

                if (exchange_RateTextBox != null) exchange_RateTextBox.Text = Convert.ToDecimal(row["Exchange_Rate"]).ToString("F4");
                if (min_RateTextBox != null) min_RateTextBox.Text = Convert.ToDecimal(row["Min_Exchange_Rate"]).ToString("F4");
                if (max_RateTextBox != null) max_RateTextBox.Text = Convert.ToDecimal(row["Max_Exchange_Rate"]).ToString("F4");

                if (is_ActiveCheckBox != null) is_ActiveCheckBox.Checked = !Convert.ToBoolean(row["Is_Active"] != DBNull.Value ? row["Is_Active"] : true);
                if (chk_IsBaseCurrency != null && dtMaster.Columns.Contains("Is_Base_Currency"))
                    chk_IsBaseCurrency.Checked = Convert.ToBoolean(row["Is_Base_Currency"] != DBNull.Value ? row["Is_Base_Currency"] : false);
                if (chk_IsLocalCurrency != null && dtMaster.Columns.Contains("Is_Local_Currency"))
                    chk_IsLocalCurrency.Checked = Convert.ToBoolean(row["Is_Local_Currency"] != DBNull.Value ? row["Is_Local_Currency"] : false);

                // 🛡️ تعبئة حقول التدقيق والرقابة باستعمال SystemConstants
                if (txt_CreatedBy != null) txt_CreatedBy.Text = FormatUserInfo(row[SystemConstants.AuditFields.CreatedBy], row["CreatedByName"]);
                if (txt_CreatedAt != null) txt_CreatedAt.Text = row[SystemConstants.AuditFields.CreatedAt] != DBNull.Value ? Convert.ToDateTime(row[SystemConstants.AuditFields.CreatedAt]).ToString("yyyy/MM/dd  hh:mm tt") : "";

                if (txt_UpdatedBy != null) txt_UpdatedBy.Text = FormatUserInfo(row[SystemConstants.AuditFields.UpdatedBy], row["UpdatedByName"]);
                if (txt_UpdatedAt != null) txt_UpdatedAt.Text = row[SystemConstants.AuditFields.UpdatedAt] != DBNull.Value ? Convert.ToDateTime(row[SystemConstants.AuditFields.UpdatedAt]).ToString("yyyy/MM/dd  hh:mm tt") : "";

                await LoadCurrencyHistoryAsync(curId);
                await ApplyDynamicLocksAsync();

                ChangeFormMode(FormMode.RecordSelected);
            }
        }

        // 🌟 الجلب اللامتزامن لتاريخ تعديلات العملة
        private async Task LoadCurrencyHistoryAsync(string curId)
        {
            if (currenciesDataGridView == null) return;

            string sqlHistory = @"
                SELECT C.Cur_Name, 
                       H.Exchange_Rate, 
                       H.Min_Exchange_Rate, 
                       H.Max_Exchange_Rate, 
                       H.Change_Date, 
                       H.Cur_ID,
                       ISNULL(U.Full_Name, U.Username) AS ModifiedBy
                FROM Currency_Rates_History H 
                INNER JOIN Currencies C ON H.Cur_ID = C.Cur_ID 
                LEFT JOIN Users U ON H.Modified_By = U.User_ID
                WHERE H.Cur_ID = @CurID 
                ORDER BY H.Change_Date DESC";

            DataTable dtHistory = await DatabaseHelper.GetTableAsync(sqlHistory, new[] { new SqlParameter("@CurID", curId) });
            currenciesDataGridView.DataSource = dtHistory;
        }

        #endregion

        #region الديناميكية والرقابة اللحظية (Async)

        private async Task Chk_IsLocalCurrency_CheckedChangedAsync(object sender, EventArgs e)
        {
            if (CurrentMode == FormMode.View) return;
            await ApplyDynamicLocksAsync();
        }

        private async Task Chk_IsBaseCurrency_CheckedChangedAsync(object sender, EventArgs e)
        {
            if (CurrentMode == FormMode.View) return;
            await ApplyDynamicLocksAsync();
        }

        private async Task ApplyDynamicLocksAsync()
        {
            if (CurrentMode == FormMode.View) return;

            bool isLocal = chk_IsLocalCurrency != null && chk_IsLocalCurrency.Checked;
            bool isBase = chk_IsBaseCurrency != null && chk_IsBaseCurrency.Checked;

            // 1. تسعير العملة المحلية الإجباري
            if (isLocal)
            {
                if (exchange_RateTextBox != null) { exchange_RateTextBox.Text = "1.0000"; exchange_RateTextBox.ReadOnly = true; }
                if (min_RateTextBox != null) { min_RateTextBox.Text = "1.0000"; min_RateTextBox.ReadOnly = true; }
                if (max_RateTextBox != null) { max_RateTextBox.Text = "1.0000"; max_RateTextBox.ReadOnly = true; }
            }
            else
            {
                if (exchange_RateTextBox != null) exchange_RateTextBox.ReadOnly = false;
                if (min_RateTextBox != null) min_RateTextBox.ReadOnly = false;
                if (max_RateTextBox != null) max_RateTextBox.ReadOnly = false;
            }

            // 2. حماية خيار "العملة المحلية" بطريقة لامتزامنة
            if (chk_IsLocalCurrency != null)
            {
                if (!isLocal)
                {
                    string sqlCheck = "SELECT COUNT(Cur_ID) FROM Currencies WHERE Is_Local_Currency = 1";
                    if (CurrentMode == FormMode.Edit && !string.IsNullOrWhiteSpace(cur_IDTextBox.Text))
                    {
                        sqlCheck += $" AND Cur_ID <> {cur_IDTextBox.Text}";
                    }
                    int localCount = Convert.ToInt32(await DatabaseHelper.ExecuteScalarAsync(sqlCheck, null) ?? 0);
                    chk_IsLocalCurrency.Enabled = (localCount == 0);
                }
                else
                {
                    chk_IsLocalCurrency.Enabled = false;
                }
            }

            // 3. حماية حالة التفعيل (Is_Active) إذا كانت العملة افتراضية
            if (chk_IsBaseCurrency != null && is_ActiveCheckBox != null)
            {
                if (isBase)
                {
                    is_ActiveCheckBox.Checked = false;
                    is_ActiveCheckBox.Enabled = false;
                }
                else
                {
                    is_ActiveCheckBox.Enabled = true;
                }
            }
        }

        protected override void LockControls(Control parent, bool isReadOnly)
        {
            base.LockControls(parent, isReadOnly);

            if (cur_IDTextBox != null) cur_IDTextBox.ReadOnly = true;
            if (currenciesDataGridView != null) currenciesDataGridView.Enabled = true;

            if (txt_CreatedBy != null) txt_CreatedBy.ReadOnly = true;
            if (txt_CreatedAt != null) txt_CreatedAt.ReadOnly = true;
            if (txt_UpdatedBy != null) txt_UpdatedBy.ReadOnly = true;
            if (txt_UpdatedAt != null) txt_UpdatedAt.ReadOnly = true;

            if (!isReadOnly)
            {
                // إطلاق التهيئة بشكل آمن دون تجميد
                _ = ApplyDynamicLocksAsync();
            }
        }

        #endregion

        #region العمليات السيادية (CRUD) المتوافقة مع الدستور

        public override async void OnNew()
        {
            base.OnNew();

            var systemRepo = new AlRowad_ERP.Data.SystemRepository();
                
            if (exchange_RateTextBox != null) exchange_RateTextBox.Text = "1.0000";
            if (min_RateTextBox != null) min_RateTextBox.Text = "1.0000";
            if (max_RateTextBox != null) max_RateTextBox.Text = "1.0000";
            if (is_ActiveCheckBox != null) { is_ActiveCheckBox.Checked = false; is_ActiveCheckBox.Enabled = true; }
            if (chk_IsBaseCurrency != null) chk_IsBaseCurrency.Checked = false;
            if (chk_IsLocalCurrency != null) chk_IsLocalCurrency.Checked = false;

            if (currenciesDataGridView != null) currenciesDataGridView.DataSource = null;

            await ApplyDynamicLocksAsync();
            cur_NameTextBox.Focus();
        }

        public override async void OnSearch()
        {
            try
            {
                string query = "SELECT Cur_ID AS [رقم العملة], Cur_Name AS [اسم العملة], Exchange_Rate AS [سعر الصرف] FROM Currencies";

                using (var search = new AlRowad_ERP.HelpForms.UniversalSearchForm("بحث في دليل العملات (F9)", query))
                {
                    if (search.ShowDialog(this) == DialogResult.OK)
                    {
                        string selectedCurId = search.المعرف_المختار;

                        if (!string.IsNullOrWhiteSpace(selectedCurId))
                        {
                            await LoadCurrencyDataAsync(selectedCurId);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث خطأ أثناء فتح نافذة البحث: " + ex.Message, "نظام الرواد", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public override async void OnEdit()
        {
            if (string.IsNullOrWhiteSpace(cur_IDTextBox.Text))
            {
                MessageBox.Show("يرجى اختيار العملة المراد تعديلها عبر شاشة البحث أولاً.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            base.OnEdit();
            await ApplyDynamicLocksAsync();
            cur_NameTextBox.Focus();
        }

        // 🌟 الحفظ اللامتزامن بالاعتماد على الثوابت
        protected override async Task<bool> ExecuteSaveToDatabaseAsync(SqlTransaction trans)
        {
            if (string.IsNullOrWhiteSpace(cur_NameTextBox.Text))
            {
                MessageBox.Show("اسم العملة حقل إجباري.", "منع الحفظ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            int curId = Convert.ToInt32(cur_IDTextBox.Text.Trim());
            string curName = cur_NameTextBox.Text.Trim();
            string curSymbol = cur_SymbolTextBox != null ? cur_SymbolTextBox.Text.Trim() : "";

            bool isActive = is_ActiveCheckBox != null && !is_ActiveCheckBox.Checked;
            bool isBase = chk_IsBaseCurrency != null && chk_IsBaseCurrency.Checked;
            bool isLocal = chk_IsLocalCurrency != null && chk_IsLocalCurrency.Checked;

            decimal newRate = 1m, newMin = 1m, newMax = 1m;
            if (exchange_RateTextBox != null) decimal.TryParse(exchange_RateTextBox.Text, out newRate);
            if (min_RateTextBox != null) decimal.TryParse(min_RateTextBox.Text, out newMin);
            if (max_RateTextBox != null) decimal.TryParse(max_RateTextBox.Text, out newMax);

            if (isLocal)
            {
                string sqlLocal = "UPDATE Currencies SET Is_Local_Currency = 0 WHERE Cur_ID <> @CurrentID";
                await DatabaseHelper.ExecuteNonQueryAsync(sqlLocal, new[] { new SqlParameter("@CurrentID", curId) }, trans);

                newRate = 1.0000m; newMin = 1.0000m; newMax = 1.0000m;
            }

            if (isBase && !isActive)
            {
                MessageBox.Show("خطأ أمني: لا يمكن إيقاف تفعيل العملة الافتراضية للنظام!", "رفض الحفظ - رقابة الرواد", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }

            if (newMin > newMax)
            {
                MessageBox.Show("خطأ هندسي: الحد الأدنى لا يمكن أن يكون أكبر من الحد الأعلى.", "رفض الحفظ - رقابة الرواد", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }

            if (newRate < newMin || newRate > newMax)
            {
                MessageBox.Show($"خطأ أمني: سعر الصرف ({newRate}) خارج النطاق المسموح بين ({newMin}) و ({newMax}).", "رفض الحفظ", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }

            if (isBase)
            {
                string sqlBase = "UPDATE Currencies SET Is_Base_Currency = 0 WHERE Cur_ID <> @CurrentID";
                await DatabaseHelper.ExecuteNonQueryAsync(sqlBase, new[] { new SqlParameter("@CurrentID", curId) }, trans);
            }

            try
            {
                if (CurrentMode == FormMode.Edit)
                {
                    bool priceChanged = false;
                    string sqlGetOld = "SELECT Exchange_Rate, Min_Exchange_Rate, Max_Exchange_Rate FROM Currencies WHERE Cur_ID = @ID";
                    DataTable dtOld = await DatabaseHelper.GetTableAsync(sqlGetOld, new[] { new SqlParameter("@ID", curId) });

                    if (dtOld.Rows.Count > 0)
                    {
                        decimal oldRate = Convert.ToDecimal(dtOld.Rows[0]["Exchange_Rate"]);
                        decimal oldMin = Convert.ToDecimal(dtOld.Rows[0]["Min_Exchange_Rate"]);
                        decimal oldMax = Convert.ToDecimal(dtOld.Rows[0]["Max_Exchange_Rate"]);

                        if (oldRate != newRate || oldMin != newMin || oldMax != newMax)
                        {
                            priceChanged = true;
                        }
                    }

                    string sqlUpdate = $@"UPDATE Currencies 
                                         SET Cur_Name = @Name, Cur_Symbol = @Symbol, Exchange_Rate = @Rate, 
                                             Min_Exchange_Rate = @MinRate, Max_Exchange_Rate = @MaxRate, 
                                             Is_Active = @IsActive, Is_Base_Currency = @IsBase, Is_Local_Currency = @IsLocal,
                                             {SystemConstants.AuditFields.UpdatedBy}  = @UpdatedBy,  {SystemConstants.AuditFields.UpdatedAt} = GETDATE()
                                         WHERE Cur_ID = @ID AND RowVersion = @OldRowVersion";

                    var pUpdate = new System.Collections.Generic.List<SqlParameter> {
                        new SqlParameter("@ID", curId), new SqlParameter("@Name", curName),
                        new SqlParameter("@Symbol", curSymbol), new SqlParameter("@Rate", newRate),
                        new SqlParameter("@MinRate", newMin), new SqlParameter("@MaxRate", newMax),
                        new SqlParameter("@IsActive", isActive), new SqlParameter("@IsBase", isBase),
                        new SqlParameter("@IsLocal", isLocal),
                        new SqlParameter("@UpdatedBy", UserSession.UserId),
                        new SqlParameter("@OldRowVersion", SqlDbType.Timestamp) { Value = _currentRowVersion ?? (object)DBNull.Value }
                    };

                    int rowsAffected = await DatabaseHelper.ExecuteNonQueryAsync(sqlUpdate, pUpdate.ToArray(), trans);

                    if (rowsAffected == 0)
                    {
                        throw new InvalidOperationException("تضارب بيانات: تم تعديل هذا السجل من قبل مستخدم آخر أثناء استعراضك له. يرجى التراجع وتحديث الشاشة لعرض أحدث البيانات.");
                    }

                    if (priceChanged)
                    {
                        string sqlHistory = @"INSERT INTO Currency_Rates_History (Cur_ID, Exchange_Rate, Min_Exchange_Rate, Max_Exchange_Rate, Change_Date, Modified_By) 
                                              VALUES (@CurID, @Rate, @Min, @Max, GETDATE(), @ModBy)";
                        SqlParameter[] pHistory = {
                            new SqlParameter("@CurID", curId), new SqlParameter("@Rate", newRate),
                            new SqlParameter("@Min", newMin), new SqlParameter("@Max", newMax),
                            new SqlParameter("@ModBy", UserSession.UserId)
                        };
                        await DatabaseHelper.ExecuteNonQueryAsync(sqlHistory, pHistory, trans);
                    }

                    DatabaseHelper.LogAuditTransaction(trans, "Currencies", curId.ToString(), "UPDATE", "تم الحفظ المسبق", $"تحديث بيانات العملة: {curName}", "تعديل عملة");
                }
                else if (CurrentMode == FormMode.New)
                {
                    string sqlInsert = $@"INSERT INTO Currencies 
                                         (Cur_ID, Cur_Name, Cur_Symbol, Exchange_Rate, Min_Exchange_Rate, Max_Exchange_Rate, Is_Active, Is_Base_Currency, Is_Local_Currency, {SystemConstants.AuditFields.CreatedBy} ,  {SystemConstants.AuditFields.CreatedAt}) 
                                         VALUES 
                                         (@ID, @Name, @Symbol, @Rate, @MinRate, @MaxRate, @IsActive, @IsBase, @IsLocal, @CreatedBy, GETDATE())";

                    SqlParameter[] pInsert = {
                        new SqlParameter("@ID", curId), new SqlParameter("@Name", curName),
                        new SqlParameter("@Symbol", curSymbol), new SqlParameter("@Rate", newRate),
                        new SqlParameter("@MinRate", newMin), new SqlParameter("@MaxRate", newMax),
                        new SqlParameter("@IsActive", isActive), new SqlParameter("@IsBase", isBase),
                        new SqlParameter("@IsLocal", isLocal),
                        new SqlParameter("@CreatedBy", UserSession.UserId)
                    };
                    await DatabaseHelper.ExecuteNonQueryAsync(sqlInsert, pInsert, trans);

                    string sqlHistoryInit = @"INSERT INTO Currency_Rates_History (Cur_ID, Exchange_Rate, Min_Exchange_Rate, Max_Exchange_Rate, Change_Date, Modified_By) 
                                              VALUES (@CurID, @Rate, @Min, @Max, GETDATE(), @ModBy)";
                    await DatabaseHelper.ExecuteNonQueryAsync(sqlHistoryInit, new[] {
                        new SqlParameter("@CurID", curId), new SqlParameter("@Rate", newRate),
                        new SqlParameter("@Min", newMin), new SqlParameter("@Max", newMax),
                        new SqlParameter("@ModBy", UserSession.UserId)
                    }, trans);
                }

                return true;
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "تضارب في التزامن", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ أثناء حفظ بيانات العملة: " + ex.Message, "خطأ نظام", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void MinMaxRate_Leave(object sender, EventArgs e)
        {
            if (CurrentMode == FormMode.View) return;

            decimal.TryParse(min_RateTextBox.Text, out decimal min);
            decimal.TryParse(max_RateTextBox.Text, out decimal max);

            if (min > max && max > 0)
            {
                MessageBox.Show("تنبيه منطقي: الحد الأدنى لسعر الصرف يجب أن يكون أقل من أو يساوي الحد الأعلى!", "إدخال خاطئ", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                TextBox txt = sender as TextBox;
                if (txt != null)
                {
                    txt.Focus();
                    txt.SelectAll();
                }
            }
        }

        public override async void OnCancel()
        {
            if (MessageBox.Show("هل أنت متأكد من التراجع عن التعديلات؟", "تأكيد التراجع", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (!string.IsNullOrWhiteSpace(cur_IDTextBox.Text))
                {
                    await LoadCurrencyDataAsync(cur_IDTextBox.Text);
                }
                else
                {
                    ClearFormFields(this);
                    if (currenciesDataGridView != null) currenciesDataGridView.DataSource = null;
                    ChangeFormMode(FormMode.View);
                }
            }
        }

        public override async void OnDelete()
        {
            if (CurrentMode != FormMode.View && CurrentMode != FormMode.RecordSelected || string.IsNullOrWhiteSpace(cur_IDTextBox.Text))
            {
                MessageBox.Show("يرجى اختيار عملة لاستعراضها وحذفها.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (chk_IsLocalCurrency != null && chk_IsLocalCurrency.Checked)
            {
                MessageBox.Show("عذراً، لا يمكن حذف العملة المحلية للنظام!", "منع الحذف", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            if (chk_IsBaseCurrency != null && chk_IsBaseCurrency.Checked)
            {
                MessageBox.Show("عذراً، لا يمكن حذف العملة الافتراضية للنظام!", "منع الحذف", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            if (MessageBox.Show($"هل أنت متأكد من حذف العملة ({cur_NameTextBox.Text}) نهائياً؟", "تأكيد الحذف", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                using (SqlConnection conn = DatabaseHelper.GetConnection())
                {
                    await conn.OpenAsync();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            int curId = Convert.ToInt32(cur_IDTextBox.Text);

                            await DatabaseHelper.ExecuteNonQueryAsync("DELETE FROM Currency_Rates_History WHERE Cur_ID = @ID", new[] { new SqlParameter("@ID", curId) }, trans);
                            await DatabaseHelper.ExecuteNonQueryAsync("DELETE FROM Currencies WHERE Cur_ID = @ID", new[] { new SqlParameter("@ID", curId) }, trans);

                            trans.Commit();
                            MessageBox.Show("تم حذف العملة بنجاح.", "تأكيد", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            ClearFormFields(this);

                            if (currenciesDataGridView != null) currenciesDataGridView.DataSource = null;
                            ChangeFormMode(FormMode.View);
                        }
                        catch (SqlException ex)
                        {
                            trans.Rollback();
                            if (ex.Number == 547)
                                MessageBox.Show("مرفوض! هذه العملة مستخدمة في عمليات مالية سابقة.", "حماية النظام", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            else
                                MessageBox.Show("حدث خطأ في قاعدة البيانات: " + ex.Message, "خطأ فني", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }
        #endregion

        // 🌟 الحماية المرجعية اللامتزامنة
        protected override async Task<bool> ValidateDependenciesBeforeDeleteAsync()
        {
            string curId = cur_IDTextBox.Text;

            if (await IsRecordUsedInTableAsync("System_Ledger", "Currency_ID", curId))
            {
                MessageBox.Show("منع أمني: لا يمكن حذف هذه العملة لارتباطها بقيود وحركات مالية سابقة في النظام.", "ارتباط مالي", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }

            if (await IsRecordUsedInTableAsync("Cash_Voucher_Header", "Cur_ID", curId))
            {
                MessageBox.Show("منع أمني: لا يمكن حذف هذه العملة لأنه تم استخدامها في سندات قبض سابقة.", "ارتباط مالي", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }

            return true;
        }
    }
}