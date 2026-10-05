using AlRowad_ERP.Core;
using AlRowad_ERP.Core.Constants;
using AlRowad_ERP.Data;
using AlRowad_ERP.UI.Base;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AlRowad_ERP.Forms
{
    public partial class DriverForm : BaseEntryForm
    {
        private readonly DriverRepository _driverRepo;

        public DriverForm()
        {
            InitializeComponent();

            // 🌟 الربط الدستوري الموحد للـ Auto-Bind
            PrimaryIdFieldName = "txt_Driver_ID";
            MainTableName = SystemConstants.Tables.Drivers;

            _driverRepo = new DriverRepository();
        }

        private void DriverForm_Load(object sender, EventArgs e)
        {
            try
            {
                // إجبار الشاشة على وضع العرض عند الفتح (إدارة الحالة المركزية)
                ChangeFormMode(FormMode.View);
            }
            catch (Exception ex) { LogError(ex); }
        }

        #region العمليات السيادية وإدارة الحالة
        // 🌟 السيادة المركزية في فتح وقفل الأدوات
        protected override void LockControls(Control parent, bool isReadOnly)
        {
            base.LockControls(parent, isReadOnly);

            // الترقيم دائماً للقراءة فقط
            if (txt_Driver_ID != null) txt_Driver_ID.ReadOnly = true;
        }

        public override void OnNew()
        {
            base.OnNew(); // يجهز الشاشة ويغير الحالة إلى FormMode.New
            txt_Driver_ID.Text = "تلقائي";
            txt_Driver_Name.Focus();
        }

        // 🌟 الحفظ المغلف بـ SqlTransaction للحماية التامة (ACID)
        protected override async Task<bool> ExecuteSaveToDatabaseAsync(SqlTransaction trans)
        {
            if (string.IsNullOrWhiteSpace(txt_Driver_Name.Text))
            {
                MessageBox.Show("يجب إدخال اسم السائق.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            int.TryParse(txt_Driver_ID.Text, out int driverId);
            bool isNew = (CurrentMode == FormMode.New);

            int savedId = await _driverRepo.SaveDriverAsync(
                driverId,
                txt_Driver_Name.Text,
                txt_Phone_Number.Text,
                txt_License_Number.Text,
                txt_Vehicle_Type.Text,
                txt_Notes.Text,
                this.CurrentUserId,
                isNew,
                trans);

            if (savedId > 0)
            {
                txt_Driver_ID.Text = savedId.ToString();
                return true;
            }
            return false;
        }

        // 🌟 الحذف الدستوري
        protected override async Task<bool> ExecuteDeleteFromDatabaseAsync(SqlTransaction trans)
        {
            if (int.TryParse(txt_Driver_ID.Text, out int driverId) && driverId > 0)
            {
                await _driverRepo.ExecuteDeleteAsync(driverId, this.CurrentUserId, trans);
                return true;
            }
            return false;
        }

        // 🌟 محرك البحث الموحد
        // 🌟 محرك البحث الموحد
        // 🌟 محرك البحث الموحد (محدث ليعمل بنظام Async/Await)
        public override async void OnSearch()
        {
            using (var search = new HelpForms.UniversalSearchForm("بحث عن السائقين", _driverRepo.GetSearchQuery()))
            {
                if (search.ShowDialog() == DialogResult.OK)
                {
                    if (int.TryParse(search.المعرف_المختار, out int selectedId))
                    {
                        // استدعاء جلب البيانات لامتزامناً دون تجميد الشاشة
                        await LoadDriverDataAsync(selectedId);
                    }
                }
            }
        }
            #endregion
        // 🌟 دالة الجلب اللامتزامنة (تطبيق مبدأ السلاسة والأداء العالي)
        private async Task LoadDriverDataAsync(int driverId)
        {
            var dt = await _driverRepo.GetDriverAsync(driverId);
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];

                txt_Driver_ID.Text = row[SystemConstants.Columns.Driver_ID].ToString();
                txt_Driver_Name.Text = row["Driver_Name"].ToString();
                txt_Phone_Number.Text = row["Phone_Number"].ToString();
                txt_License_Number.Text = row["License_Number"].ToString();
                txt_Vehicle_Type.Text = row["Vehicle_Type"].ToString();
                txt_Notes.Text = row["Notes"].ToString();

                // تطبيق الدستور: تغيير وضع الشاشة مركزياً بناءً على الحالة
                ChangeFormMode(FormMode.RecordSelected);
            }
        }
    }
}