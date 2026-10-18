using AlRowad_ERP.Core;
using AlRowad_ERP.Data;
using AlRowad_ERP.Models;
using AlRowad_ERP.Repositories;
using AlRowad_ERP.UI.Base;
using Dapper;
using System;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AlRowad_ERP.UI.Settings
{
    public partial class Frm_CompanyInfo : BaseEntryForm
    {
        private readonly CompanyRepository _companyRepo;
        private byte[] _logoBytes = null;

        public Frm_CompanyInfo()
        {
            InitializeComponent();
            _companyRepo = new CompanyRepository();
        }

        protected override async void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            await LoadDataAsync();

            // ضبط الوضع الافتراضي للشاشة على العرض (View) عبر محرك الحالة المركزي
            ChangeFormMode(FormMode.View);
        }

        private async Task LoadDataAsync()
        {
            try
            {
                var company = await _companyRepo.GetCompanyInfoAsync();
                if (company != null)
                {
                    txtCompanyNameAr.Text = company.Company_Name_Ar;
                    txtCompanyNameEn.Text = company.Company_Name_En;
                    txtTaxNumber.Text = company.Tax_Number;
                    txtCommercialRecord.Text = company.Commercial_Record;
                    txtAddress.Text = company.Address;
                    txtPhone.Text = company.Phone;
                    txtEmail.Text = company.Email;

                    if (company.Logo != null && company.Logo.Length > 0)
                    {
                        _logoBytes = company.Logo;
                        using (var ms = new MemoryStream(_logoBytes))
                        {
                            picLogo.Image = System.Drawing.Image.FromStream(ms);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DatabaseHelper.LogSystemError(ex.Message, ex.StackTrace, "Frm_CompanyInfo_Load");
                MessageBox.Show($"حدث خطأ أثناء تحميل بيانات الشركة: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // 🌟 تفعيل زر استعراض واختيار شعار الشركة وحفظه كبايتات لتخزينه في قاعدة البيانات
        private void btnBrowseLogo_Click(object sender, EventArgs e)
        {
            using (var openDlg = new OpenFileDialog())
            {
                openDlg.Title = "اختر شعار الشركة";
                openDlg.Filter = "Image Files (*.jpg; *.jpeg; *.png; *.bmp)|*.jpg; *.jpeg; *.png; *.bmp";

                if (openDlg.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // قراءة الصورة كـ Byte Array لتخزينها في قاعدة البيانات بطريقة الأنظمة العالمية
                        _logoBytes = File.ReadAllBytes(openDlg.FileName);

                        // عرض الصورة في الـ PictureBox للمستخدم فوراً
                        using (var ms = new MemoryStream(_logoBytes))
                        {
                            picLogo.Image = System.Drawing.Image.FromStream(ms);
                        }
                    }
                    catch (Exception ex)
                    {
                        DatabaseHelper.LogSystemError(ex.Message, ex.StackTrace, "Frm_CompanyInfo_BrowseLogo");
                        MessageBox.Show($"فشل تحميل الصورة: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        // 🌟 العمليات السيادية الموروثة من BaseEntryForm
        public override void OnNew()
        {
            base.OnNew();
            ClearControls();
            ChangeFormMode(FormMode.New);
        }

        public override void OnEdit()
        {
            base.OnEdit();
            ChangeFormMode(FormMode.Edit);
        }

        public override void OnCancel()
        {
            base.OnCancel();
            _ = LoadDataAsync();
            ChangeFormMode(FormMode.View);
        }

        // 🌟 الالتزام بتوقيع الدستور الحقيقي الذي يستقبل SqlTransaction
        // 🌟 التعديل المتوافق مع دستور النظام ومعايير الـ ACID Transactions
        protected override async Task<bool> ExecuteSaveToDatabaseAsync(SqlTransaction transaction)
        {
            try
            {
                var company = new CompanyInfoDto
                {
                    Company_Name_Ar = txtCompanyNameAr.Text.Trim(),
                    Company_Name_En = txtCompanyNameEn.Text.Trim(),
                    Tax_Number = txtTaxNumber.Text.Trim(),
                    Commercial_Record = txtCommercialRecord.Text.Trim(),
                    Address = txtAddress.Text.Trim(),
                    Phone = txtPhone.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    Logo = _logoBytes
                };

                // ملاحظة هامة: بما أن المعاملة (transaction) يتم تمريرها من الإطار الأساسي،
                // يجب استخدام اتصال المعاملة الحالي (transaction.Connection) حصراً وعدم فتح اتصال جديد منفصل.
                var connection = transaction.Connection;

                string checkQuery = "SELECT COUNT(1) FROM Company_Info";
                int count = await connection.ExecuteScalarAsync<int>(checkQuery, transaction: transaction);

                string query = count > 0
                    ? @"UPDATE Company_Info SET Company_Name_Ar = @Company_Name_Ar, Company_Name_En = @Company_Name_En, Tax_Number = @Tax_Number, Commercial_Record = @Commercial_Record, Address = @Address, Phone = @Phone, Email = @Email, Logo = @Logo"
                    : @"INSERT INTO Company_Info (Company_Name_Ar, Company_Name_En, Tax_Number, Commercial_Record, Address, Phone, Email, Logo) VALUES (@Company_Name_Ar, @Company_Name_En, @Tax_Number, @Commercial_Record, @Address, @Phone, @Email, @Logo)";

                await connection.ExecuteAsync(query, company, transaction: transaction);

                ChangeFormMode(FormMode.View);
                return true;
            }
            catch (Exception ex)
            {
                DatabaseHelper.LogSystemError(ex.Message, ex.StackTrace, "Frm_CompanyInfo_Save");
                MessageBox.Show($"فشل الحفظ: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        // زر اختيار الشعار المرئي

        private void ClearControls()
        {
            txtCompanyNameAr.Clear();
            txtCompanyNameEn.Clear();
            txtTaxNumber.Clear();
            txtCommercialRecord.Clear();
            txtAddress.Clear();
            txtPhone.Clear();
            txtEmail.Clear();
            picLogo.Image = null;
            _logoBytes = null;
        }
    }
}